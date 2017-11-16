using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Apworks.Integration.AspNetCore;
using Apworks.Integration.AspNetCore.Configuration;
using Apworks.Repositories.MongoDB;
using Apworks.Messaging;
using Apworks.Serialization.Json;
using Apworks.Messaging.RabbitMQ;
using RabbitMQ.Client;
using Apworks.Events;

namespace WeText.Services.Accounts
{
    public class Startup
    {
        private IEventPublisher eventPublisher;

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

            var mongoServer = this.Configuration["mongo:server"];
            var mongoPort = Convert.ToInt32(this.Configuration["mongo:port"]);
            var mongoDatabase = this.Configuration["mongo:db"];
            var rabbitHost = this.Configuration["rabbit:host"];
            var exchangeName = this.Configuration["rabbit:exchange"];

            var messageSerializer = new MessageJsonSerializer();

            services.AddApworks()
                .WithEventPublisher(new EventBus(new ConnectionFactory { HostName = rabbitHost }, messageSerializer, exchangeName, ExchangeType.Topic))
                .WithDataServiceSupport(new DataServiceConfigurationOptions
                    (new MongoRepositoryContext
                        (new MongoRepositorySettings(mongoServer, mongoPort, mongoDatabase))))
                .Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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
