using System.Collections.Generic;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Services
{
    public interface IPostcrossingEventService
    {
        Task<IEnumerable<EventBase>> GetLatestEventsAsync(PostcrossingEventTypeEnum type = PostcrossingEventTypeEnum.All, long? fromEventId = null);
    }
}
