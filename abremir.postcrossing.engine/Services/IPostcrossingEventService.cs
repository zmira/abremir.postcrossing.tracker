using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace abremir.postcrossing.engine.Services
{
    public interface IPostcrossingEventService
    {
        Task<IEnumerable<EventBase>> GetLatestEventsAsync(PostcrossingEventTypeEnum type = PostcrossingEventTypeEnum.All, long? fromEventId = null);
    }
}
