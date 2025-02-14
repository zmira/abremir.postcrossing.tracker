using System;
using System.Collections.Generic;
using System.Linq;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models.Enumerations;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using abremir.postcrossing.engine.Services;
using LiteDB;

namespace abremir.postcrossing.engine.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IEventComposer _eventComposer;

        public EventRepository(
            IRepositoryService repositoryService,
            IEventComposer eventComposer)
        {
            _repositoryService = repositoryService;
            _eventComposer = eventComposer;
        }

        public EventBase Add(EventBase postcrossingEvent)
        {
            return Add(new[] { postcrossingEvent }).FirstOrDefault();
        }

        public IEnumerable<EventBase> Add(IEnumerable<EventBase> postcrossingEvents)
        {
            var result = new List<EventBase>();
            using var repository = _repositoryService.GetRepository();

            foreach (var postcrossingEvent in postcrossingEvents)
            {
                if (string.IsNullOrWhiteSpace(postcrossingEvent.RawEvent))
                {
                    continue;
                }

                postcrossingEvent.Timestamp = DateTimeOffset.Now;

                EventBase @event;
                switch (postcrossingEvent.EventType)
                {
                    case PostcrossingEventTypeEnum.Register:
                        @event = _eventComposer.ComposeEvent<Register>(postcrossingEvent);
                        repository.Insert(@event, PostcrossingTrackerConstants.EventCollectionName);
                        break;
                    case PostcrossingEventTypeEnum.Send:
                        @event = _eventComposer.ComposeEvent<Send>(postcrossingEvent);
                        repository.Insert(@event, PostcrossingTrackerConstants.EventCollectionName);
                        break;
                    case PostcrossingEventTypeEnum.SignUp:
                        @event = _eventComposer.ComposeEvent<SignUp>(postcrossingEvent);
                        repository.Insert(@event, PostcrossingTrackerConstants.EventCollectionName);
                        break;
                    case PostcrossingEventTypeEnum.Upload:
                        @event = _eventComposer.ComposeEvent<Upload>(postcrossingEvent);
                        repository.Insert(@event, PostcrossingTrackerConstants.EventCollectionName);
                        break;
                    default:
                        @event = postcrossingEvent;
                        repository.Insert(@event, PostcrossingTrackerConstants.EventCollectionName);
                        break;
                }

                result.Add(@event);
            }

            return result;
        }

        public T Get<T>(long eventId) where T : EventBase
        {
            var associatedEventType = AssociatedEventType.GetAssociatedEventType<T>();

            if (associatedEventType == null)
            {
                return null;
            }

            using var repository = _repositoryService.GetRepository();

            return GetQueryable<T>(repository)
                .Where(postcrossingEvent => postcrossingEvent.EventType == associatedEventType && postcrossingEvent.EventId == eventId)
                .FirstOrDefault();
        }

        public IEnumerable<T> FindEventsWithIdGreaterThan<T>(long eventId) where T : EventBase
        {
            var associatedEventType = AssociatedEventType.GetAssociatedEventType<T>();

            if (associatedEventType == null)
            {
                return Enumerable.Empty<T>();
            }

            using var repository = _repositoryService.GetRepository();

            return GetQueryable<T>(repository)
                .Where(postcrossingEvent => postcrossingEvent.EventType == associatedEventType && postcrossingEvent.EventId > eventId)
                .ToList();
        }

        private ILiteQueryable<T> GetQueryable<T>(ILiteRepository repository) where T : EventBase
        {
            if (typeof(T) == typeof(Register))
            {
                return repository
                    .Query<Register>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryable<T>;
            }

            if (typeof(T) == typeof(Send))
            {
                return repository
                    .Query<Send>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryable<T>;
            }

            if (typeof(T) == typeof(SignUp))
            {
                return repository
                    .Query<SignUp>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryable<T>;
            }

            if (typeof(T) == typeof(Upload))
            {
                return repository
                    .Query<Upload>(PostcrossingTrackerConstants.EventCollectionName)
                    .IncludeAll() as ILiteQueryable<T>;
            }

            return repository
                .Query<EventBase>(PostcrossingTrackerConstants.EventCollectionName) as ILiteQueryable<T>;
        }
    }
}
