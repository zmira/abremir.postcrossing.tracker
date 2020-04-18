using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;
using System;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Unknown)]
    public class EventBase
    {
        public long EventId { get; set; }
        public PostcrossingEventTypeEnum EventType { get; set; }
        public string RawEvent { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
