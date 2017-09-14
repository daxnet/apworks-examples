using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Apworks.Messaging.RabbitMQ;
using RabbitMQ.Client;
using Apworks.Serialization.Json;
using Apworks.Messaging;
using WeText.Services.Shared.Events;
using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using WeText.Services.Auditing.Models;

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

            services.AddScoped<AuditingDataContext>();
            services.AddScoped<IRepositoryContext>(provider => new EntityFrameworkRepositoryContext(provider.GetService<AuditingDataContext>()));

            var rabbitHost = this.Configuration["rabbit:host"];
            var exchangeName = this.Configuration["rabbit:exchange"];
            var connectionFactory = new ConnectionFactory { HostName = rabbitHost };
            var messageSerializer = new MessageJsonSerializer();
            
            var messageBus = new MessageBus(connectionFactory, messageSerializer,
                exchangeName, ExchangeType.Topic);
            messageBus.MessageReceived += MessageBus_MessageReceived;
            messageBus.Subscribe("wetext.*");

            this.serviceProvider = services.BuildServiceProvider();
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

            app.UseMvc();
        }
    }
}
