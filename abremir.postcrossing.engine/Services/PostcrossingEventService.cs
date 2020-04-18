using abremir.postcrossing.engine.Clients;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using abremir.postcrossing.engine.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace abremir.postcrossing.engine.Services
{
    public class PostcrossingEventService : IPostcrossingEventService
    {
        private readonly IPostcrossingEngineSettingsService _postcrossingEngineSettingsService;
        private readonly IPostcrossingClient _postcrossingClient;
        private readonly IInsightsRepository _insightsRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IPostcrossingEventProcessor _postcrossingEventProcessor;

        private readonly SemaphoreSlim _semaphore;

        private long? _currentLatestPostcrossingEventId;

        public PostcrossingEventService(
            IPostcrossingEngineSettingsService postcrossingEngineSettingsService,
            IPostcrossingClient postcrossingClient,
            IInsightsRepository insightsRepository,
            IEventRepository eventRepository,
            IPostcrossingEventProcessor postcrossingEventProcessor)
        {
            _semaphore = new SemaphoreSlim(1);

            _postcrossingEngineSettingsService = postcrossingEngineSettingsService;
            _postcrossingClient = postcrossingClient;
            _insightsRepository = insightsRepository;
            _eventRepository = eventRepository;
            _postcrossingEventProcessor = postcrossingEventProcessor;
        }

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
