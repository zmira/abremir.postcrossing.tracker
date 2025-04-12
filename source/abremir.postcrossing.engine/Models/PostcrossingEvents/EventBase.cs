using System;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models.Enumerations;
using LiteDB;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Unknown)]
    public class EventBase
    {
        [BsonId(false)]
        public long EventId { get; set; }

        public PostcrossingEventTypeEnum EventType { get; set; }
        public string RawEvent { get; set; }

        private DateTimeOffset _timestamp;
        public DateTimeOffset Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value.ToHundredthOfSecond(); }
        }
    }
}
