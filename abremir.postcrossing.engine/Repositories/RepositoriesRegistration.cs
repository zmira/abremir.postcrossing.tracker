using abremir.postcrossing.engine.Repositories;

namespace Microsoft.Extensions.DependencyInjection
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
