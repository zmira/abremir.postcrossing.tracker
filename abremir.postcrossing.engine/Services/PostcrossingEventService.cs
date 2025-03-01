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
                var currentLatestPostcrossingEventId = await GetLatestPostcrossingEventId().ConfigureAwait(false);
                var fromPostcrossingEvent = fromPostcrossingEventId ?? currentLatestPostcrossingEventId;

                var postcrossingEvents = await _postcrossingClient.GetPostcrossingEventsAsync(fromPostcrossingEvent).ConfigureAwait(false);

                _currentLatestPostcrossingEventId = await _postcrossingEventProcessor.GetLatestEventId(postcrossingEvents, currentLatestPostcrossingEventId).ConfigureAwait(false);

                if (_postcrossingEngineSettingsService.PersistData)
                {
                    await _eventRepository.Add(postcrossingEvents).ConfigureAwait(true);
                }

                return await _postcrossingEventProcessor.BuildResultForRequestedEventType(postcrossingEventType, fromPostcrossingEvent, postcrossingEvents).ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<long> GetLatestPostcrossingEventId()
        {
            return _currentLatestPostcrossingEventId ?? (_currentLatestPostcrossingEventId = _postcrossingEngineSettingsService.PersistData
                    ? await _insightsRepository.GetLatestPostcrossingEventId().ConfigureAwait(false)
                    : 1).Value;
        }
    }
}
