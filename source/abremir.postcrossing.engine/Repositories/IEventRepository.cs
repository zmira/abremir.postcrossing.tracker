using System.Collections.Generic;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Repositories
{
    public interface IEventRepository
    {
        Task<EventBase> Add(EventBase postcrossingEvent);
        Task<IEnumerable<EventBase>> Add(IEnumerable<EventBase> postcrossingEvents);
        Task<T> Get<T>(long eventId) where T : EventBase;
        Task<IEnumerable<T>> FindEventsWithIdGreaterThan<T>(long eventId) where T : EventBase;
    }
}
