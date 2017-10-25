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
        private IServiceProvider serviceProvider;

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
                .AddEventHandler(sp => new AccountAuthenticatedEventHandler(sp.GetService<IRepositoryContext>()))
                .AddEventHandler(sp => new AccountCreatedEventHandler())
                .AddApworks()
                .WithDataServiceSupport(new DataServiceConfigurationOptions(sp => 
                    new EntityFrameworkRepositoryContext(sp.GetService<AuditingDataContext>())))
                .Configure();

            this.serviceProvider = services.BuildServiceProvider();
            
            var messageBus = new MessageBus(connectionFactory, messageSerializer,
                exchangeName, ExchangeType.Topic, queueName);
            messageBus.MessageReceived += MessageBus_MessageReceived;
            messageBus.Subscribe("wetext.*");
        }

        private void MessageBus_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var eventHandlers = this.serviceProvider.GetServices<IIntentionalEventHandler>();

            var @event = (IDictionary<string, object>)e.Message;
            var eventMetadata = (IDictionary<string, object>)@event["Metadata"];
            var intent = eventMetadata["$apworks:event.intent"].ToString();

            (from handler in eventHandlers
             where handler.HandlingIntention.Equals(intent)
             select handler)
            .ToList()
            .ForEach(async h => await h.HandleAsync(@event));
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
