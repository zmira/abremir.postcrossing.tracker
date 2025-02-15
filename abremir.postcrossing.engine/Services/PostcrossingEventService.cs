using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Clients;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using abremir.postcrossing.engine.Repositories;

namespace abremir.postcrossing.engine.Services
{
    public class PostcrossingEventService(
        IPostcrossingEngineSettingsService postcrossingEngineSettingsService,
        IPostcrossingClient postcrossingClient,
        IInsightsRepository insightsRepository,
        IEventRepository eventRepository,
        IPostcrossingEventProcessor postcrossingEventProcessor) : IPostcrossingEventService
    {
        private readonly IPostcrossingEngineSettingsService _postcrossingEngineSettingsService = postcrossingEngineSettingsService;
        private readonly IPostcrossingClient _postcrossingClient = postcrossingClient;
        private readonly IInsightsRepository _insightsRepository = insightsRepository;
        private readonly IEventRepository _eventRepository = eventRepository;
        private readonly IPostcrossingEventProcessor _postcrossingEventProcessor = postcrossingEventProcessor;

        private readonly SemaphoreSlim _semaphore = new(1);

        private long? _currentLatestPostcrossingEventId;

        public async Task<IEnumerable<EventBase>> GetLatestEventsAsync(PostcrossingEventTypeEnum postcrossingEventType = PostcrossingEventTypeEnum.All, long? fromPostcrossingEventId = null)
        {
            await _semaphore.WaitAsync();

            try
            {
                var currentLatestPostcrossingEventId = GetLatestPostcrossingEventId();
                var fromPostcrossingEvent = fromPostcrossingEventId ?? currentLatestPostcrossingEventId;

                var postcrossingEvents = await _postcrossingClient.GetPostcrossingEventsAsync(fromPostcrossingEvent);

                _currentLatestPostcrossingEventId = _postcrossingEventProcessor.GetLatestEventId(postcrossingEvents, currentLatestPostcrossingEventId);

                if (_postcrossingEngineSettingsService.PersistData)
                {
                    _eventRepository.Add(postcrossingEvents);
                }

                return _postcrossingEventProcessor.BuildResultForRequestedEventType(postcrossingEventType, fromPostcrossingEvent, postcrossingEvents);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private long GetLatestPostcrossingEventId()
        {
            return _currentLatestPostcrossingEventId ?? (_currentLatestPostcrossingEventId = _postcrossingEngineSettingsService.PersistData
                    ? _insightsRepository.GetLatestPostcrossingEventId()
                    : 1).Value;
        }
    }
}
