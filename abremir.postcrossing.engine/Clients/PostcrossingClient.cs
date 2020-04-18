using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Helpers;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abremir.postcrossing.engine.Clients
{
    public class PostcrossingClient : IPostcrossingClient
    {
        public async Task<IEnumerable<EventBase>> GetPostcrossingEventsAsync(long fromEventId = 0)
        {
            return (await GetRawPostcrossingEventsAsync(fromEventId))
                .Select(MapToEventBase)
                .Where(@event => @event != null);
        }

        private async Task<List<List<string>>> GetRawPostcrossingEventsAsync(long fromEventId = 0)
        {
            return await PostcrossingTrackerConstants
                .PostcrossingLiveEventsDomain
                .AppendPathSegments(PostcrossingTrackerConstants.PostcrossingLiveEventsPathSegments)
                .SetQueryParam(PostcrossingTrackerConstants.PostcrossingLiveEventsQueryParameter, fromEventId)
                .GetJsonAsync<List<List<string>>>();
        }

        private EventBase MapToEventBase(List<string> postcrossingEvent)
        {
            var @event = EventBaseHelper.MapToEventBase(postcrossingEvent[1]);

            if (@event != null)
            {
                @event.EventId = long.Parse(postcrossingEvent[0]);
            }

            return @event;
        }
    }
}
