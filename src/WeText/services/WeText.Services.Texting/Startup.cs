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
            // Add framework services.
            services.AddMvc();

            var messageSerializer = new MessageJsonSerializer();

            var integrationMessageBusHost = this.Configuration["rabbit.integration:host"];
            var integrationExchangeName = this.Configuration["rabbit.integration:exchange"];
            var integrationMessageQueue = this.Configuration["rabbit.integration:queue"];

            var integrationMessageBus = new MessageBus(new ConnectionFactory { HostName = integrationMessageBusHost },
                messageSerializer, integrationExchangeName, ExchangeType.Topic, integrationMessageQueue);

            integrationMessageBus.MessageReceived += IntegrationMessageBus_MessageReceived;
            integrationMessageBus.Subscribe("wetext.*");
        }

        private void IntegrationMessageBus_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
