using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using abremir.postcrossing.engine.Repositories;

namespace abremir.postcrossing.engine.Services
{
    public class PostcrossingEventProcessor(
        IPostcrossingEngineSettingsService postcrossingEngineSettingsService,
        IInsightsRepository insightsRepository,
        IEventRepository eventRepository) : IPostcrossingEventProcessor
    {
        private readonly IPostcrossingEngineSettingsService _postcrossingEngineSettingsService = postcrossingEngineSettingsService;
        private readonly IInsightsRepository _insightsRepository = insightsRepository;
        private readonly IEventRepository _eventRepository = eventRepository;

        public async Task<long> GetLatestEventId(IEnumerable<EventBase> postcrossingEvents, long currentLatestPostcrossingEventId)
        {
            if (!postcrossingEvents.Any())
            {
                return currentLatestPostcrossingEventId;
            }

            var latestPostcrossingEventId = postcrossingEvents
                .Max(postcrossingEvent => postcrossingEvent.EventId);

            if (_postcrossingEngineSettingsService.PersistData)
            {
                await _insightsRepository.SetLatestPostcrossingEventId(latestPostcrossingEventId).ConfigureAwait(false);
            }

            return latestPostcrossingEventId;
        }

        public async Task<IEnumerable<EventBase>> BuildResultForRequestedEventType(PostcrossingEventTypeEnum postcrossingEventType, long fromPostcrossingEventId, IEnumerable<EventBase> postcrossingEvents)
        {
            var result = new List<EventBase>();

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Register))
            {
                result.AddRange(await GetEventsStartingFrom<Register>(fromPostcrossingEventId, postcrossingEvents).ConfigureAwait(false));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Send))
            {
                result.AddRange(await GetEventsStartingFrom<Send>(fromPostcrossingEventId, postcrossingEvents).ConfigureAwait(false));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.SignUp))
            {
                result.AddRange(await GetEventsStartingFrom<SignUp>(fromPostcrossingEventId, postcrossingEvents).ConfigureAwait(false));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Upload))
            {
                result.AddRange(await GetEventsStartingFrom<Upload>(fromPostcrossingEventId, postcrossingEvents).ConfigureAwait(false));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Unknown))
            {
                result.AddRange(await GetEventsStartingFrom<EventBase>(fromPostcrossingEventId, postcrossingEvents).ConfigureAwait(false));
            }

            return result.OrderBy(r => r.EventId);
        }

        private async Task<IEnumerable<T>> GetEventsStartingFrom<T>(long fromPostcrossingEventId, IEnumerable<EventBase> postcrossingEvents) where T : EventBase
        {
            var associatedEventType = AssociatedEventType.GetAssociatedEventType<T>();

            if (associatedEventType == null)
            {
                return [];
            }

            return _postcrossingEngineSettingsService.PersistData
                ? await _eventRepository
                    .FindEventsWithIdGreaterThan<T>(fromPostcrossingEventId).ConfigureAwait(false)
                : postcrossingEvents
                    .Where(e => e.EventType == associatedEventType && e.EventId > fromPostcrossingEventId)
                    .Select(e => e as T);
        }
    }
}
