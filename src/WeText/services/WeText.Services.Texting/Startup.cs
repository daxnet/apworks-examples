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

namespace WeText.Services.Texting
{
    public class Startup
    {
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
            var rabbitHost = this.Configuration["rabbit:host"];
            var rabbitExchangeName = this.Configuration["rabbit:exchange"];
            var rabbitQueueName = this.Configuration["rabbit:queue"];
            var connectionFactory = new ConnectionFactory { HostName = rabbitHost };
            var messageSerializer = new MessageJsonSerializer();

            // Add framework services.
            services.AddMvc();

            services.AddApworks()
                .WithCommandSubscriber(new CommandBus(connectionFactory, messageSerializer, rabbitExchangeName, ExchangeType.Topic, rabbitQueueName))
                .WithDefaultCommandConsumer("commands.texting")
                //.AddCommandHandler(null)
                .WithEventSubscriber(new EventBus(connectionFactory, messageSerializer, rabbitExchangeName, ExchangeType.Topic, rabbitQueueName))
                .WithDefaultEventConsumer("events.*")
                .AddEventHandler(new AccountCreatedEventHandler(this.Configuration))
                .Configure();

            var serviceProvider = services.BuildServiceProvider();

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
