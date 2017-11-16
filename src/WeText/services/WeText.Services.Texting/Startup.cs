using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Apworks.Messaging.RabbitMQ;
using RabbitMQ.Client;
using Apworks.Serialization.Json;
using Apworks.Messaging;
using Apworks.Integration.AspNetCore;
using WeText.Services.Texting.EventHandlers;
using Apworks.Events;
using Apworks.Commands;
using Apworks.Integration.AspNetCore.Hal;
using Hal.Builders;
using WeText.Services.Texting.CommandHandlers;

namespace WeText.Services.Texting
{
    public class Startup
    {
        private ICommandSender commandSender;
        private IEventConsumer eventConsumer;
        private ICommandConsumer commandConsumer;

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

            var connectionFactory = new ConnectionFactory { HostName = rabbitHost };
            var messageSerializer = new MessageJsonSerializer();

            // Add framework services.
            services.AddMvc();

            services.AddApworks()
                .WithCommandSender(new CommandBus(connectionFactory, messageSerializer, rabbitExchangeName, ExchangeType.Topic))
                .WithCommandSubscriber(new CommandBus(connectionFactory, messageSerializer, rabbitExchangeName, ExchangeType.Topic, rabbitCommandQueueName))
                .WithDefaultCommandConsumer("commands.*")
                .AddCommandHandler(new PostTextCommandHandler())
                .WithEventSubscriber(new EventBus(connectionFactory, messageSerializer, rabbitExchangeName, ExchangeType.Topic, rabbitEventQueueName))
                .WithDefaultEventConsumer("events.*")
                .AddEventHandler(new AccountCreatedEventHandler(this.Configuration))
                .Configure();

            var serviceProvider = services.BuildServiceProvider();

            this.commandSender = serviceProvider.GetService<ICommandSender>();

            this.eventConsumer = serviceProvider.GetService<IEventConsumer>();
            this.eventConsumer.Consume();

            this.commandConsumer = serviceProvider.GetService<ICommandConsumer>();
            this.commandConsumer.Consume();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                this.commandSender.Dispose();
                this.eventConsumer.Dispose();
                this.commandConsumer.Dispose();
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
