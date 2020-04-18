using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Upload)]
    public class Upload : EventBase
    {
        public User User { get; set; }
        public Postcard Postcard { get; set; }
    }
}
