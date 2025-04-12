using abremir.postcrossing.engine.Clients;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 // Namespace does not match folder structure
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
