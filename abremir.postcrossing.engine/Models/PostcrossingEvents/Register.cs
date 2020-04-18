using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;
using LiteDB;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Register)]
    public class Register : EventBase
    {
        [BsonRef(PostcrossingTrackerConstants.UserCollectionName)]
        public User FromUser { get; set; }

        [BsonRef(PostcrossingTrackerConstants.UserCollectionName)]
        public User User { get; set; }

        [BsonRef(PostcrossingTrackerConstants.PostcardCollectionName)]
        public Postcard Postcard { get; set; }
    }
}
