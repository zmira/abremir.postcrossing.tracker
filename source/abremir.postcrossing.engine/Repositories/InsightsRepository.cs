using System.Threading.Tasks;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Services;

namespace abremir.postcrossing.engine.Repositories
{
    public class InsightsRepository(IRepositoryService repositoryService) : IInsightsRepository
    {
        private readonly IRepositoryService _repositoryService = repositoryService;

        public async Task<long> GetLatestPostcrossingEventId()
        {
            return (await GetInsights().ConfigureAwait(false))?.LatestPostcrossingEventId ?? 0;
        }

        public async Task SetLatestPostcrossingEventId(long eventId)
        {
            var insights = (await GetInsights().ConfigureAwait(false)) ?? new Insights { Id = 1 };
            insights.LatestPostcrossingEventId = eventId;

            using var repository = _repositoryService.GetRepository();

            await repository.UpsertAsync(insights, PostcrossingTrackerConstants.TrackerInsightsCollectionName).ConfigureAwait(false);
        }

        private async Task<Insights> GetInsights()
        {
            using var repository = _repositoryService.GetRepository();

            return await repository.FirstOrDefaultAsync<Insights>("1 = 1", PostcrossingTrackerConstants.TrackerInsightsCollectionName).ConfigureAwait(false);
        }
    }
}
