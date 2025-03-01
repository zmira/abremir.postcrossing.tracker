using abremir.postcrossing.engine.Configuration;
using abremir.postcrossing.engine.Interfaces;
using abremir.postcrossing.engine.Services;
using LiteDB.Async;
using LiteDB.Engine;
using NSubstitute;

namespace abremir.postcrossing.engine.tests.Configuration
{
    public class TestRepositoryService : IRepositoryService, IMemoryRepositoryService
    {
        private TempStream _tempStream;

        private readonly IFileSystemService _fileSystemService;
        private readonly ILiteRepositoryAsync _repository;

        public TestRepositoryService()
        {
            _fileSystemService = Substitute.For<IFileSystemService>();

            LiteDbConfiguration.Configure();
        }

        public ILiteRepositoryAsync GetRepository()
        {
            if (_tempStream is null)
            {
                _tempStream = new TempStream("abremir.postcrossing.engine.tests.litedb");

                _fileSystemService.GetStreamForDbFile(Arg.Any<string>()).Returns(_tempStream);
            }

            var repositoryService = new RepositoryService(_fileSystemService, new MigrationRunner());

            return repositoryService.GetRepository();
        }

        public void ResetDatabase()
        {
            _tempStream?.Dispose();
            _tempStream = null;
        }
    }
}
