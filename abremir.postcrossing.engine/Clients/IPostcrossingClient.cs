using System.Collections.Generic;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models.PostcrossingEvents;

namespace abremir.postcrossing.engine.Clients
{
    public interface IPostcrossingClient
    {
        Task<IEnumerable<EventBase>> GetPostcrossingEventsAsync(long fromEventId = 0);
    }
}
