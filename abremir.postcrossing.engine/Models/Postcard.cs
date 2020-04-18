using abremir.postcrossing.engine.Assets;

namespace abremir.postcrossing.engine.Models
{
    public class Postcard
    {
        public Country Country { get; set; }
        public string PostcardId { get; set; }
        public string Link => $"/postcards/{PostcardId}";
    }
}
