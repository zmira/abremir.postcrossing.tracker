using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Attributes;
using abremir.postcrossing.engine.Models.Enumerations;
using LiteDB;

namespace abremir.postcrossing.engine.Models.PostcrossingEvents
{
    [AssociatedEventType(PostcrossingEventTypeEnum.Send)]
    public class Send : EventBase
    {
        [BsonRef(PostcrossingTrackerConstants.UserCollectionName)]
        public User User { get; set; }

        [BsonRef(PostcrossingTrackerConstants.CountryCollectionName)]
        public Country ToCountry { get; set; }
    }
}
