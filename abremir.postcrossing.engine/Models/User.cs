using abremir.postcrossing.engine.Assets;

namespace abremir.postcrossing.engine.Models
{
    public class User
    {
        public Country Country { get; set; }
        public string Name { get; set; }
        public string Link => $"/user/{Name}";
    }
}
