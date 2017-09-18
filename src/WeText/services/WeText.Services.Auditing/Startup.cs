using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Apworks.Messaging.RabbitMQ;
using RabbitMQ.Client;
using Apworks.Serialization.Json;
using Apworks.Messaging;
using WeText.Services.Shared.Events;
using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using WeText.Services.Auditing.Models;
using Apworks.Integration.AspNetCore;
using Apworks.Integration.AspNetCore.Configuration;

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

            services.AddScoped<AuditingDataContext>();
            services.AddApworks()
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
            bool.TryParse(e.Message.Metadata[IntegrationEvent.IntegrationEventMetadataKey]?.ToString(), out bool isIntegrationEvent);
            Guid.TryParse(e.Message.Id.ToString(), out Guid eventId);
            DateTime.TryParse(e.Message.Timestamp.ToString(), out DateTime eventTimestamp);
            var intent = e.Message.Metadata["$apworks:event.intent"].ToString();
            var payload = e.Message.ToString();

            var item = new EventItem
            {
                EventId = eventId,
                Intent = intent,
                IsIntegration = isIntegrationEvent,
                Payload = payload,
                Timestamp = eventTimestamp
            };

            try
            {
                var repositoryContext = this.serviceProvider.GetService<IRepositoryContext>();
                var repository = repositoryContext.GetRepository<Guid, EventItem>();
                repository.Add(item);
                repositoryContext.Commit();
            }
            catch
            {
                // TODO: add log here and prevent the program from exit unexpectly.
            }
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
