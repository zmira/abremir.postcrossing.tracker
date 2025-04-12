using abremir.postcrossing.engine.Assets;
using LiteDB;

namespace abremir.postcrossing.engine.Models
{
    public class Postcard
    {
        [BsonId(true)]
        public int Id { get; set; }

        [BsonRef(PostcrossingTrackerConstants.CountryCollectionName)]
        public Country Country { get; set; }

        public string PostcardId { get; set; }

        [BsonIgnore]
        public string Link => $"/postcards/{PostcardId}";
    }
}
