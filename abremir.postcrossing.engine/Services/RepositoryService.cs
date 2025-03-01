using abremir.postcrossing.engine.Assets;
using abremir.postcrossing.engine.Interfaces;
using LiteDB.Async;

namespace abremir.postcrossing.engine.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IMigrationRunner _migrationRunner;

        public RepositoryService(IFileSystemService fileSystemService, IMigrationRunner migrationRunner)
        {
            _fileSystemService = fileSystemService;
            _migrationRunner = migrationRunner;

            _fileSystemService.EnsureLocalDataFolder();
        }

        public ILiteRepositoryAsync GetRepository()
        {
            var repository = new LiteRepositoryAsync(_fileSystemService.GetStreamForDbFile(PostcrossingTrackerConstants.PostcrossingTrackerDatabaseFilename));

            _migrationRunner.ApplyMigrations(repository.Database);

            return repository;
        }
    }
}
