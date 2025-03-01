using abremir.postcrossing.engine.Configuration;
using abremir.postcrossing.engine.Interfaces;
using abremir.postcrossing.engine.Services;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 // Namespace does not match folder structure
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
                .AddSingleton<IPostcrossingEngineSettingsService, PostcrossingEngineSettingsService>()
                .AddSingleton<IMigrationRunner, MigrationRunner>()
                .AddSingleton<IFileSystemService, FileSystemService>();

            return serviceCollection;
        }
    }
}
