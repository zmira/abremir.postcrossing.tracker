using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using System.Collections.Generic;

namespace abremir.postcrossing.engine.Services
{
    public interface IPostcrossingEventProcessor
    {
        long GetLatestEventId(IEnumerable<EventBase> postcrossingEvents, long currentLatestPostcrossingEventId);
        IEnumerable<EventBase> BuildResultForRequestedEventType(PostcrossingEventTypeEnum postcrossingEventType, long fromPostcrossingEventId, IEnumerable<EventBase> postcrossingEvents);
    }
}
