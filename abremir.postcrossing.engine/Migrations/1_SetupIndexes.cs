using System.Threading.Tasks;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Interfaces;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using LiteDB.Async;

namespace abremir.postcrossing.engine.Migrations
{
    public class _1_SetupIndexes : IMigration
    {
        public int MigrationId => 1;

        public Task Apply(ILiteDatabaseAsync liteDatabase)
        {
            Task<bool>[] liteDatabaseIndexes = [
                liteDatabase.GetCollection<Country>(PostcrossingTrackerConstants.CountryCollectionName).EnsureIndexAsync(country => country.Name, true),
                liteDatabase.GetCollection<Country>(PostcrossingTrackerConstants.CountryCollectionName).EnsureIndexAsync(country => country.Code, true),
                liteDatabase.GetCollection<User>(PostcrossingTrackerConstants.UserCollectionName).EnsureIndexAsync(user => user.Name, true),
                liteDatabase.GetCollection<User>(PostcrossingTrackerConstants.UserCollectionName).EnsureIndexAsync(user => user.Country),
                liteDatabase.GetCollection<Postcard>(PostcrossingTrackerConstants.PostcardCollectionName).EnsureIndexAsync(postcard => postcard.PostcardId, true),
                liteDatabase.GetCollection<Postcard>(PostcrossingTrackerConstants.PostcardCollectionName).EnsureIndexAsync(postcard => postcard.Country),
                liteDatabase.GetCollection<EventBase>(PostcrossingTrackerConstants.EventCollectionName).EnsureIndexAsync(@event => @event.EventType)
            ];

            Task.WaitAll(liteDatabaseIndexes);

            liteDatabase.UserVersion++;

            return Task.CompletedTask;
        }
    }
}
