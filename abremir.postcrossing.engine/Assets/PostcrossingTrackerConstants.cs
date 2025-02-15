namespace abremir.postcrossing.engine.Assets
{
    public static class PostcrossingTrackerConstants
    {
        // http://static1.postcrossing.com/liveEvents/getLast?lastEventId=0
        public const string PostcrossingLiveEventsDomain = "http://static1.postcrossing.com";
        public static readonly string[] PostcrossingLiveEventsPathSegments = ["liveEvents", "getLast"];
        public const string PostcrossingLiveEventsQueryParameter = "lastEventId";

        public const string PostcrossingTrackerDatabaseFilename = "postcrossing-tracker.litedb";

        public const string EventCollectionName = "event";
        public const string CountryCollectionName = "country";
        public const string PostcardCollectionName = "postcard";
        public const string UserCollectionName = "user";
        public const string TrackerInsightsCollectionName = "tracker_insights";

        public const long TicksPerHundredthOfSecond = 100000;
    }
}
