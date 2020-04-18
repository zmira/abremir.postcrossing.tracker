namespace abremir.postcrossing.engine.Repositories
{
    public interface IInsightsRepository
    {
        long GetLatestPostcrossingEventId();
        void SetLatestPostcrossingEventId(long eventId);
    }
}
