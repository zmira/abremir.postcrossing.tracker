using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Services;

namespace abremir.postcrossing.engine.Repositories
{
    public class InsightsRepository : IInsightsRepository
    {
        private readonly IRepositoryService _repositoryService;

        public InsightsRepository(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public long GetLatestPostcrossingEventId()
        {
            return GetInsights()?.LatestPostcrossingEventId ?? 0;
        }

        public void SetLatestPostcrossingEventId(long eventId)
        {
            var insights = GetInsights() ?? new Insights { Id = 1 };
            insights.LatestPostcrossingEventId = eventId;

            using var repository = _repositoryService.GetRepository();

            repository.Upsert(insights, PostcrossingTrackerConstants.TrackerInsightsCollectionName);
        }

        private Insights GetInsights()
        {
            using var repository = _repositoryService.GetRepository();

            return repository.FirstOrDefault<Insights>("1 = 1", PostcrossingTrackerConstants.TrackerInsightsCollectionName);
        }
    }
}
