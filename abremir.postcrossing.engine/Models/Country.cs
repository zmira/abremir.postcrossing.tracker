namespace abremir.postcrossing.engine.Models
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Link => $"/country/{Code}";
    }
}
