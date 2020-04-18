namespace abremir.postcrossing.engine.Assets
{
    public static class PostcrossingTrackerConstants
    {
        // http://static1.postcrossing.com/liveEvents/getLast?lastEventId=0
        public const string PostcrossingLiveEventsDomain = "http://static1.postcrossing.com";
        public static readonly string[] PostcrossingLiveEventsPathSegments = new[] { "liveEvents", "getLast" };
        public const string PostcrossingLiveEventsQueryParameter = "lastEventId";
    }
}
