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

            services.AddScoped<AuditingDataContext>()
                .AddScoped<IEventSubscriber>(x => new EventBus(connectionFactory, messageSerializer, exchangeName, ExchangeType.Topic, queueName))
                .AddScoped<IEventHandler>(x => new AccountAuthenticatedEventHandler(x.GetService<IRepositoryContext>()))
                .AddScoped<IEventConsumer>(x =>
                {
                    var handlers = x.GetServices<IEventHandler>();
                    return new EventConsumer(x.GetService<IEventSubscriber>(), handlers, "wetext.*");
                })
                .AddApworks()
                .WithDataServiceSupport(new DataServiceConfigurationOptions(sp =>
                    new EntityFrameworkRepositoryContext(sp.GetService<AuditingDataContext>())))
                .Configure();

            //var eventSubscriber = new EventBus(connectionFactory, messageSerializer, exchangeName, ExchangeType.Topic, queueName);
            //var eventHandlers = new IEventHandler[] { new AccountAuthenticatedEventHandler(services.BuildServiceProvider().GetService<IRepositoryContext>()) };
            //var eventConsumer = new EventConsumer(eventSubscriber, eventHandlers, "wetext.*");
            //eventSubscriber.MessageReceived += (x, y) =>
            //  {
            //      var message = y.Message;
            //  };

            //eventSubscriber.Subscribe("wetext.*");

            // Resolve the event consumer
            var consumer = services.BuildServiceProvider().GetService<IEventConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnrichDataServiceExceptionResponse();

            app.UseMvc();
        }
    }
}
