using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Apworks.Integration.AspNetCore;
using Apworks.Repositories.MongoDB;
using Apworks.Integration.AspNetCore.Configuration;

namespace Apworks.Examples.CustomerService.MongoDB
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

            var mongoServer = this.Configuration["mongo:server"];
            var mongoPort = Convert.ToInt32(this.Configuration["mongo:port"]);
            var mongoDatabase = this.Configuration["mongo:db"];

            services.AddApworks()
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

            app.UseMvc();
        }
    }
}
