using abremir.postcrossing.engine.Assets;
using LiteDB;

namespace abremir.postcrossing.engine.Models
{
    public class User
    {
        [BsonId(true)]
        public int Id { get; set; }

        [BsonRef(PostcrossingTrackerConstants.CountryCollectionName)]
        public Country Country { get; set; }

        public string Name { get; set; }

        [BsonIgnore]
        public string Link => $"/user/{Name}";
    }
}
