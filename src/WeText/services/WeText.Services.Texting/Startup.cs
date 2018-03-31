using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Apworks.Messaging.RabbitMQ;
using RabbitMQ.Client;
using Apworks.Serialization.Json;
using Apworks.Integration.AspNetCore;
using WeText.Services.Texting.EventHandlers;
using Apworks.Events;
using Apworks.Commands;
using WeText.Services.Texting.CommandHandlers;
using Apworks.Repositories;
using Apworks.Snapshots;
using Apworks.EventStore.SQLServer;
using Apworks.EventStore.AdoNet;
using Apworks.Integration.AspNetCore.Messaging;
using WeText.Services.Shared.Events;
using WeText.Services.Texting.Commands;

namespace WeText.Services.Texting
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Reads the host machine which runs the RabbitMQ.
            var rabbitHost = this.Configuration["rabbit:host"];

            // Reads the RabbitMQ exchange setting.
            var rabbitExchangeName = this.Configuration["rabbit:exchange"];

            // Reads the name of the event queue.
            var rabbitEventQueueName = this.Configuration["rabbit:eventQueue"];

            // Reads the name of the command queue.
            var rabbitCommandQueueName = this.Configuration["rabbit:commandQueue"];

            // Reads connection string of the event store database.
            var eventStoreConnectionString = this.Configuration["mssql:event.db"];

            // Reads connection string of the query database.
            var queryDatabaseConnectionString = this.Configuration["mssql:query.db"];

            // Event/Command subscribers/publishers
            var connectionFactory = new ConnectionFactory { HostName = rabbitHost };
            var messageSerializer = new MessageJsonSerializer();
            var messageHandlerExecutionContext = new ServiceProviderMessageHandlerExecutionContext(services, x => x.BuildServiceProvider());

            var eventSubscriber = new RabbitEventBus(connectionFactory, messageSerializer, messageHandlerExecutionContext, rabbitExchangeName, ExchangeType.Topic, rabbitEventQueueName);
            eventSubscriber.Subscribe<AccountCreatedEvent, AccountCreatedEventHandler>();
            eventSubscriber.Subscribe<TextContentChangedEvent, TextContentChangedEventHandler>();

            var commandSender = new RabbitCommandBus(connectionFactory, messageSerializer, messageHandlerExecutionContext, rabbitExchangeName, ExchangeType.Topic);
            var commandSubscriber = new RabbitCommandBus(connectionFactory, messageSerializer, messageHandlerExecutionContext, rabbitExchangeName, ExchangeType.Topic, rabbitCommandQueueName);
            commandSubscriber.Subscribe<PostTextCommand, PostTextCommandHandler>();

            services.AddSingleton<IEventSubscriber>(eventSubscriber);
            services.AddSingleton<ICommandSender>(commandSender);
            services.AddSingleton<ICommandSubscriber>(commandSubscriber);

            // Domain Repository
            var eventStorageConfig = new AdoNetEventStoreConfiguration(eventStoreConnectionString);
            var objectSerializer = new ObjectJsonSerializer();
            services.AddSingleton<ISnapshotProvider, SuppressedSnapshotProvider>();
            services.AddTransient<IEventPublisher>(x => new RabbitEventBus(connectionFactory, messageSerializer, messageHandlerExecutionContext, rabbitExchangeName, ExchangeType.Topic));
            services.AddTransient<IEventStore>(x => new SqlServerEventStore(eventStorageConfig, objectSerializer));
            services.AddTransient<IDomainRepository, EventSourcingDomainRepository>();
            
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            var eventSubscriber = app.ApplicationServices.GetRequiredService<IEventSubscriber>();
            var commandSender = app.ApplicationServices.GetRequiredService<ICommandSender>();
            var commandSubscriber = app.ApplicationServices.GetRequiredService<ICommandSubscriber>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                eventSubscriber.Dispose();
                commandSender.Dispose();
                commandSubscriber.Dispose();
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.EnrichDataServiceExceptionResponse();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMvc();
        }
    }
}
