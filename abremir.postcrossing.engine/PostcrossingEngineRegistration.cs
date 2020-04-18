namespace Microsoft.Extensions.DependencyInjection
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
