﻿using abremir.postcrossing.engine.Configuration;
using abremir.postcrossing.engine.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            LiteDbConfiguration.Configure();

            serviceCollection
                .AddSingleton<IRepositoryService, RepositoryService>()
                .AddSingleton<IPostcrossingEventService, PostcrossingEventService>()
                .AddSingleton<IPostcrossingEventProcessor, PostcrossingEventProcessor>()
                .AddSingleton<IPostcrossingEngineSettingsService, PostcrossingEngineSettingsService>();

            return serviceCollection;
        }
    }
}
