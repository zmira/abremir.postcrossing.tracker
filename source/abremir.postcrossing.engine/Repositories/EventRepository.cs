using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using abremir.postcrossing.engine.Services;
using LiteDB;
using LiteDB.Async;

namespace abremir.postcrossing.engine.Repositories
{
    public class EventRepository(
        IRepositoryService repositoryService,
        IEventComposer eventComposer) : IEventRepository
    {
        private readonly IRepositoryService _repositoryService = repositoryService;
        private readonly IEventComposer _eventComposer = eventComposer;

        public async Task<EventBase> Add(EventBase postcrossingEvent)
        {
            return (await Add([postcrossingEvent]).ConfigureAwait(false)).FirstOrDefault();
        }

        public async Task<IEnumerable<EventBase>> Add(IEnumerable<EventBase> postcrossingEvents)
        {
            var result = new List<EventBase>();

            foreach (var postcrossingEvent in postcrossingEvents)
            {
                if (string.IsNullOrWhiteSpace(postcrossingEvent.RawEvent))
                {
                    continue;
                }

                postcrossingEvent.Timestamp = DateTimeOffset.Now;

                EventBase @event = postcrossingEvent.EventType switch
                {
                    PostcrossingEventTypeEnum.Register => await _eventComposer.ComposeEvent<Register>(postcrossingEvent).ConfigureAwait(false),
                    PostcrossingEventTypeEnum.Send => await _eventComposer.ComposeEvent<Send>(postcrossingEvent).ConfigureAwait(false),
                    PostcrossingEventTypeEnum.SignUp => await _eventComposer.ComposeEvent<SignUp>(postcrossingEvent).ConfigureAwait(false),
                    PostcrossingEventTypeEnum.Upload => await _eventComposer.ComposeEvent<Upload>(postcrossingEvent).ConfigureAwait(false),
                    _ => postcrossingEvent,
                };

                using var repository = _repositoryService.GetRepository();
                await repository.InsertAsync(@event, PostcrossingTrackerConstants.EventCollectionName).ConfigureAwait(false);

                result.Add(@event);
            }

            return result;
        }

        public async Task<T> Get<T>(long eventId) where T : EventBase
        {
            var associatedEventType = AssociatedEventType.GetAssociatedEventType<T>();

            if (associatedEventType == null)
            {
                return null;
            }

            using var repository = _repositoryService.GetRepository();

            return await GetQueryable<T>(repository)
                .Where(postcrossingEvent => postcrossingEvent.EventType == associatedEventType && postcrossingEvent.EventId == eventId)
                .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> FindEventsWithIdGreaterThan<T>(long eventId) where T : EventBase
        {
            var associatedEventType = AssociatedEventType.GetAssociatedEventType<T>();

            if (associatedEventType == null)
            {
                return [];
            }

            using var repository = _repositoryService.GetRepository();

            return await GetQueryable<T>(repository)
                .Where(postcrossingEvent => postcrossingEvent.EventType == associatedEventType && postcrossingEvent.EventId > eventId)
                .ToListAsync().ConfigureAwait(false);
        }

        private ILiteQueryableAsync<T> GetQueryable<T>(ILiteRepositoryAsync repository) where T : EventBase
        {
            if (typeof(T) == typeof(Register))
            {
                return repository
                    .Query<Register>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryableAsync<T>;
            }

            if (typeof(T) == typeof(Send))
            {
                return repository
                    .Query<Send>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryableAsync<T>;
            }

            if (typeof(T) == typeof(SignUp))
            {
                return repository
                    .Query<SignUp>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryableAsync<T>;
            }

            if (typeof(T) == typeof(Upload))
            {
                return repository
                    .Query<Upload>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryableAsync<T>;
            }

            return repository
                .Query<EventBase>(PostcrossingTrackerConstants.EventCollectionName) as ILiteQueryableAsync<T>;
        }
    }
}
