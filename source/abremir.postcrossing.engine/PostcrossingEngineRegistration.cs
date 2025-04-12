﻿#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class PostcrossingEngineRegistration
    {
        public static IServiceCollection RegisterPostcrossingEngine(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddServices()
                .AddRepositories()
                .AddClients();

            return serviceCollection;
        }
    }
}
