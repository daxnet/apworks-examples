using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Apworks.Messaging.RabbitMQ;
using RabbitMQ.Client;
using Apworks.Serialization.Json;
using Apworks.Messaging;
using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using Apworks.Integration.AspNetCore;
using Apworks.Integration.AspNetCore.Configuration;
using System.Collections.Generic;
using WeText.Services.Shared;
using WeText.Services.Auditing.EventHandlers;
using Apworks.Events;
using System.Linq;

namespace WeText.Services.Auditing
{
    public class Startup
    {
        private IEventConsumer eventConsumer;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var rabbitHost = this.Configuration["rabbit:host"];
            var exchangeName = this.Configuration["rabbit:exchange"];
            var queueName = this.Configuration["rabbit:queue"];
            var connectionFactory = new ConnectionFactory { HostName = rabbitHost };
            var messageSerializer = new MessageJsonSerializer();

            services
                .AddApworks()
                .WithEventSubscriber(x=>new EventBus(connectionFactory, messageSerializer, exchangeName, ExchangeType.Topic, queueName))
                .WithDefaultEventConsumer("events.*")
                .AddEventHandler(x=> new AccountAuthenticatedEventHandler(x.GetService<IRepositoryContext>()))
                .WithDataServiceSupport(new DataServiceConfigurationOptions(sp =>
                    new EntityFrameworkRepositoryContext(new AuditingDataContext())))
                .Configure();

            var serviceProvider = services.BuildServiceProvider();

            this.eventConsumer = serviceProvider.GetService<IEventConsumer>();
            this.eventConsumer.Consume();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                this.eventConsumer.Dispose();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnrichDataServiceExceptionResponse();
            app.UseMvc();
        }
    }
}
