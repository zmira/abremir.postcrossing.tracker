using abremir.postcrossing.engine.Clients;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ClientsRegistration
    {
        public static IServiceCollection AddClients(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IPostcrossingClient, PostcrossingClient>();

            return serviceCollection;
        }
    }
}
