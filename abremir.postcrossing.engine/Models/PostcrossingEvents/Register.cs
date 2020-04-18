using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Register)]
    public class Register : EventBase
    {
        public User FromUser { get; set; }
        public User User { get; set; }
        public Postcard Postcard { get; set; }
    }
}
