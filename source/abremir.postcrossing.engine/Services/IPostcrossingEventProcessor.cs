using System.Collections.Generic;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Services
{
    public interface IPostcrossingEventProcessor
    {
        Task<long> GetLatestEventId(IEnumerable<EventBase> postcrossingEvents, long currentLatestPostcrossingEventId);
        Task<IEnumerable<EventBase>> BuildResultForRequestedEventType(PostcrossingEventTypeEnum postcrossingEventType, long fromPostcrossingEventId, IEnumerable<EventBase> postcrossingEvents);
    }
}
