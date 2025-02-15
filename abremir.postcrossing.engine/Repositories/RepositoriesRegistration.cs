using abremir.postcrossing.engine.Repositories;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class RepositoriesRegistration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IInsightsRepository, InsightsRepository>()
                .AddSingleton<ICountryRepository, CountryRepository>()
                .AddSingleton<IPostcardRepository, PostcardRepository>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IEventRepository, EventRepository>()
                .AddSingleton<IEventComposer, EventComposer>();

            return serviceCollection;
        }
    }
}
