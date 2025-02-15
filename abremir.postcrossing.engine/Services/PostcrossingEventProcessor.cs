using System.Collections.Generic;
using System.Linq;
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

        public long GetLatestEventId(IEnumerable<EventBase> postcrossingEvents, long currentLatestPostcrossingEventId)
        {
            if (!postcrossingEvents.Any())
            {
                return currentLatestPostcrossingEventId;
            }

            var latestPostcrossingEventId = postcrossingEvents
                .Max(postcrossingEvent => postcrossingEvent.EventId);

            if (_postcrossingEngineSettingsService.PersistData)
            {
                _insightsRepository.SetLatestPostcrossingEventId(latestPostcrossingEventId);
            }

            return latestPostcrossingEventId;
        }

        public IEnumerable<EventBase> BuildResultForRequestedEventType(PostcrossingEventTypeEnum postcrossingEventType, long fromPostcrossingEventId, IEnumerable<EventBase> postcrossingEvents)
        {
            var result = new List<EventBase>();

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Register))
            {
                result.AddRange(GetEventsStartingFrom<Register>(fromPostcrossingEventId, postcrossingEvents));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Send))
            {
                result.AddRange(GetEventsStartingFrom<Send>(fromPostcrossingEventId, postcrossingEvents));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.SignUp))
            {
                result.AddRange(GetEventsStartingFrom<SignUp>(fromPostcrossingEventId, postcrossingEvents));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Upload))
            {
                result.AddRange(GetEventsStartingFrom<Upload>(fromPostcrossingEventId, postcrossingEvents));
            }

            if (postcrossingEventType.HasFlag(PostcrossingEventTypeEnum.Unknown))
            {
                result.AddRange(GetEventsStartingFrom<EventBase>(fromPostcrossingEventId, postcrossingEvents));
            }

            return result.OrderBy(r => r.EventId);
        }

        private IEnumerable<T> GetEventsStartingFrom<T>(long fromPostcrossingEventId, IEnumerable<EventBase> postcrossingEvents) where T : EventBase
        {
            var associatedEventType = AssociatedEventType.GetAssociatedEventType<T>();

            if (associatedEventType == null)
            {
                return [];
            }

            return _postcrossingEngineSettingsService.PersistData
                ? _eventRepository
                    .FindEventsWithIdGreaterThan<T>(fromPostcrossingEventId)
                : postcrossingEvents
                    .Where(e => e.EventType == associatedEventType && e.EventId > fromPostcrossingEventId)
                    .Select(e => e as T);
        }
    }
}
