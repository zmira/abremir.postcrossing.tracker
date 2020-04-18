using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Send)]
    public class Send : EventBase
    {
        public User User { get; set; }
        public Country ToCountry { get; set; }
    }
}
