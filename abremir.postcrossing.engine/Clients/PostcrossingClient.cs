using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Helpers;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using Flurl;
using Flurl.Http;

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

        private static async Task<List<JsonArray>> GetRawPostcrossingEventsAsync(long fromEventId = 0)
        {
            return await PostcrossingTrackerConstants
                .PostcrossingLiveEventsDomain
                .AppendPathSegments(PostcrossingTrackerConstants.PostcrossingLiveEventsPathSegments)
                .SetQueryParam(PostcrossingTrackerConstants.PostcrossingLiveEventsQueryParameter, fromEventId)
                .GetJsonAsync<List<JsonArray>>();
        }

        private EventBase MapToEventBase(JsonArray postcrossingEvent)
        {
            var @event = EventBaseHelper.MapToEventBase(postcrossingEvent[1].GetValue<string>());

            if (@event != null)
            {
                @event.EventId = postcrossingEvent[0].GetValue<long>();
            }

            return @event;
        }
    }
}
