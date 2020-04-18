using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;
using LiteDB;
using System;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Unknown)]
    public class EventBase
    {
        [BsonId(false)]
        public long EventId { get; set; }

        public PostcrossingEventTypeEnum EventType { get; set; }
        public string RawEvent { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
