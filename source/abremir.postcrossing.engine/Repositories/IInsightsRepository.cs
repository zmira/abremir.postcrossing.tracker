using System.Threading.Tasks;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IInsightsRepository
    {
        Task<long> GetLatestPostcrossingEventId();
        Task SetLatestPostcrossingEventId(long eventId);
    }
}
