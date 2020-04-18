using abremir.postcrossing.engine.Models.PostcrossingEvents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace abremir.postcrossing.engine.Clients
{
    public interface IPostcrossingClient
    {
        Task<IEnumerable<EventBase>> GetPostcrossingEventsAsync(long fromEventId = 0);
    }
}
