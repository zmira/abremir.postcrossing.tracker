using abremir.postcrossing.engine.Services;
using LiteDB;
using LiteDB.Engine;

namespace abremir.postcrossing.engine.tests.Configuration
{
    public class TestRepositoryService : IRepositoryService, ITestRepositoryService
    {
        private TempStream _tempStream;

        public long CompactRepository()
        {
            throw new System.NotImplementedException();
        }

        public ILiteRepository GetRepository()
        {
            _tempStream ??= new TempStream("abremir.postcrossing.engine.tests.litedb");

            var liteRepository = new LiteRepository(_tempStream);

            RepositoryService.SetupIndexes(liteRepository.Database);

            return liteRepository;
        }

        public void ResetDatabase()
        {
            _tempStream?.Dispose();
            _tempStream = null;
        }
    }
}
