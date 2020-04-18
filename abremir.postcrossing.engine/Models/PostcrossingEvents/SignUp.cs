using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.SignUp)]
    public class SignUp : EventBase
    {
        public User User { get; set; }
    }
}
