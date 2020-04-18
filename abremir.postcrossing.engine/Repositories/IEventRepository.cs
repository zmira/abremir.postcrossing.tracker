using abremir.postcrossing.engine.Models.PostcrossingEvents;
using System.Collections.Generic;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IEventRepository
    {
        EventBase Add(EventBase postcrossingEvent);
        IEnumerable<EventBase> Add(IEnumerable<EventBase> postcrossingEvents);
        T Get<T>(long eventId) where T : EventBase;
        IEnumerable<T> FindEventsWithIdGreaterThan<T>(long eventId) where T : EventBase;
    }
}
