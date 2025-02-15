using LiteDB;

namespace abremir.postcrossing.engine.Configuration
{
    public static class LiteDbConfiguration
    {
        public static void Configure()
        {
            BsonMapper.Global.TrimWhitespace = true;
            BsonMapper.Global.EmptyStringToNull = false;
        }
    }
}
