using System.IO;
using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Models.PostcrossingEvents;
using LiteDB;

namespace abremir.postcrossing.engine.Services
{
    public class RepositoryService : IRepositoryService
    {
        public ILiteRepository GetRepository()
        {
            var repository = new LiteRepository(new ConnectionString(GetLocalPathToDatabaseFile())
            {
                Connection = ConnectionType.Shared
            });

            SetupIndexes(repository.Database);

            return repository;
        }

        public long CompactRepository()
        {
            using var repository = GetRepository();

            return repository.Database.Rebuild();
        }

        public static void SetupIndexes(ILiteDatabase liteDatabase)
        {
            if (liteDatabase.UserVersion == 0)
            {
                liteDatabase.GetCollection<Country>(PostcrossingTrackerConstants.CountryCollectionName).EnsureIndex(country => country.Name, true);
                liteDatabase.GetCollection<Country>(PostcrossingTrackerConstants.CountryCollectionName).EnsureIndex(country => country.Code, true);
                liteDatabase.GetCollection<User>(PostcrossingTrackerConstants.UserCollectionName).EnsureIndex(user => user.Name, true);
                liteDatabase.GetCollection<User>(PostcrossingTrackerConstants.UserCollectionName).EnsureIndex(user => user.Country);
                liteDatabase.GetCollection<Postcard>(PostcrossingTrackerConstants.PostcardCollectionName).EnsureIndex(postcard => postcard.PostcardId, true);
                liteDatabase.GetCollection<Postcard>(PostcrossingTrackerConstants.PostcardCollectionName).EnsureIndex(postcard => postcard.Country);
                liteDatabase.GetCollection<EventBase>(PostcrossingTrackerConstants.EventCollectionName).EnsureIndex(@event => @event.EventType);

                liteDatabase.UserVersion = 1;
            }
        }

        private static string GetLocalPathToDatabaseFile()
        {
            var pathToDataFolder = Path.Combine(Directory.GetCurrentDirectory(), "data");

            if (!Directory.Exists(pathToDataFolder))
            {
                Directory.CreateDirectory(pathToDataFolder);
            }

            return Path.Combine(pathToDataFolder, PostcrossingTrackerConstants.PostcrossingTrackerDatabaseFilename);
        }
    }
}
