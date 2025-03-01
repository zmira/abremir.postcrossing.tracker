using System.Threading.Tasks;
using abremir.postcrossing.engine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace abremir.postcrossing.engine.tests.Configuration
{
    public class DataTestsBase
    {
        protected static readonly IRepositoryService MemoryRepositoryService = new TestRepositoryService();

        private static void ResetDatabase()
        {
            (MemoryRepositoryService as IMemoryRepositoryService)?.ResetDatabase();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ResetDatabase();
        }

        protected static async Task<T> InsertData<T>(T dataToInsert)
        {
            return (await InsertData([dataToInsert]).ConfigureAwait(false))[0];
        }

        protected static async Task<T[]> InsertData<T>(T[] dataToInsert)
        {
            using var repository = MemoryRepositoryService.GetRepository();

            await repository.InsertAsync<T>(dataToInsert).ConfigureAwait(false);

            return dataToInsert;
        }
    }
}
