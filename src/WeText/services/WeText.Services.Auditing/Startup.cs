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
using Apworks.Integration.AspNetCore.Messaging;
using WeText.Services.Shared.Events;

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
            var messageHandlerExecutionContext = new ServiceProviderMessageHandlerExecutionContext(services, x => x.BuildServiceProvider());
            var messageSerializer = new MessageJsonSerializer();

            var eventSubscriber = new RabbitEventBus(connectionFactory, messageSerializer, messageHandlerExecutionContext, exchangeName, ExchangeType.Topic);
            eventSubscriber.Subscribe<AccountAuthenticatedEvent, AccountAuthenticatedEventHandler>();

            services.AddSingleton<IEventSubscriber>(eventSubscriber);

            services
                .AddApworks()
                .WithDataServiceSupport(new DataServiceConfigurationOptions(sp =>
                    new EntityFrameworkRepositoryContext(new AuditingDataContext())))
                .Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            var eventSubscriber = app.ApplicationServices.GetRequiredService<IEventSubscriber>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                eventSubscriber.Dispose();
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
