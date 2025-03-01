using System.Linq;
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

        protected static T InsertData<T>(T dataToInsert)
        {
            return InsertData([dataToInsert]).First();
        }

        protected static T[] InsertData<T>(T[] dataToInsert)
        {
            using var repository = MemoryRepositoryService.GetRepository();

            repository.Insert<T>(dataToInsert);

            return dataToInsert;
        }
    }
}
