using LiteDB;

namespace abremir.postcrossing.engine.Models
{
    public class Country
    {
        [BsonId(true)]
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        [BsonIgnore]
        public string Link => $"/country/{Code}";

        [BsonIgnore]
        public string Flag => $"/Assets/flags/{Code.ToLower()}.svg";
    }
}
