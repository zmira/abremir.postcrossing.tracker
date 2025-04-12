using LiteDB;

namespace abremir.postcrossing.engine.Models
{
    public class Insights
    {
        [BsonId(true)]
        public int Id { get; set; }

        public long LatestPostcrossingEventId { get; set; }
    }
}
