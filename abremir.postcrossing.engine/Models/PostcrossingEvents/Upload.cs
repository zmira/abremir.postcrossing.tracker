using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;
using LiteDB;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Upload)]
    public class Upload : EventBase
    {
        [BsonRef(PostcrossingTrackerConstants.UserCollectionName)]
        public User User { get; set; }

        [BsonRef(PostcrossingTrackerConstants.PostcardCollectionName)]
        public Postcard Postcard { get; set; }
    }
}
