using Apworks.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeText.Services.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventHandler(this IServiceCollection serviceCollection, Func<IServiceProvider, IIntentionalEventHandler> eventHandlerFactory, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped<IIntentionalEventHandler>(eventHandlerFactory);
                case ServiceLifetime.Singleton:
                    return serviceCollection.AddSingleton<IIntentionalEventHandler>(eventHandlerFactory);
                default:
                    return serviceCollection.AddTransient<IIntentionalEventHandler>(eventHandlerFactory);
            }
        }
    }
}
