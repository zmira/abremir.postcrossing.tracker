using abremir.postcrossing.engine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace abremir.postcrossing.engine.tests.Configuration
{
    public class DataTestsBase
    {
        protected static readonly IRepositoryService MemoryRepositoryService = new TestRepositoryService();

        private void ResetDatabase()
        {
            (MemoryRepositoryService as ITestRepositoryService)?.ResetDatabase();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ResetDatabase();
        }

        protected T InsertData<T>(T dataToInsert)
        {
            return InsertData(new[] { dataToInsert }).First();
        }

        protected T[] InsertData<T>(T[] dataToInsert)
        {
            using var repository = MemoryRepositoryService.GetRepository();

            repository.Insert<T>(dataToInsert);

            return dataToInsert;
        }
    }
}
