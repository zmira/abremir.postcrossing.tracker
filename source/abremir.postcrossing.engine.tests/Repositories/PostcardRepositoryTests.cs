using System.Linq;
using System.Threading.Tasks;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Repositories;
using abremir.postcrossing.engine.tests.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace abremir.postcrossing.engine.tests.Repositories
{
    [TestClass]
    public class PostcardRepositoryTests : DataTestsBase
    {
        private readonly PostcardRepository _postcardRepository;

        public PostcardRepositoryTests()
        {
            _postcardRepository = new PostcardRepository(MemoryRepositoryService);
        }

        [TestMethod]
        public async Task Add_NullPostcard_ReturnsNullAndDoesNotInsert()
        {
            var result = await _postcardRepository.Add(null);

            Check.That(result).IsNull();
            Check.ThatCode(_postcardRepository.All).WhichResult().IsEmpty();
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Add_InvalidPostcardId_ReturnsNullAndDoesNotInsert(string postcardId)
        {
            var postcard = new Postcard
            {
                PostcardId = postcardId,
                Country = new Country
                {
                    Id = 1,
                    Code = "XX",
                    Name = "country name"
                }
            };

            var result = await _postcardRepository.Add(postcard);

            Check.That(result).IsNull();
            Check.ThatCode(_postcardRepository.All).WhichResult().IsEmpty();
        }

        [TestMethod]
        public async Task Add_NullCountry_ReturnsNullAndDoesNotInsert()
        {
            var postcard = new Postcard
            {
                PostcardId = "POSTCARD_ID",
                Country = null
            };

            var result = await _postcardRepository.Add(postcard);

            Check.That(result).IsNull();
            Check.ThatCode(_postcardRepository.All).WhichResult().IsEmpty();
        }

        [TestMethod]
        public async Task Add_InvalidCountryId_ReturnsNullAndDoesNotInsert()
        {
            var postcard = new Postcard
            {
                PostcardId = "POSTCARD_ID",
                Country = new Country
                {
                    Id = 0,
                    Code = "XX",
                    Name = "country name"
                }
            };

            var result = await _postcardRepository.Add(postcard);

            Check.That(result).IsNull();
            Check.ThatCode(_postcardRepository.All).WhichResult().IsEmpty();
        }

        [TestMethod]
        public async Task Add_ValidPostcard_InsertsAndReturnsPostcard()
        {
            var country = new Country
            {
                Code = "XX",
                Name = "country name"
            };

            await InsertData(country);

            var postcard = new Postcard
            {
                PostcardId = "POSTCARD_ID",
                Country = country
            };

            var result = await _postcardRepository.Add(postcard);

            var allPostcards = (await _postcardRepository.All()).ToList();
            Check.That(result).IsNotNull();
            Check.That(allPostcards).CountIs(1);
            Check.That(allPostcards[0]).HasFieldsWithSameValues(postcard);
        }

        [TestMethod]
        public async Task Get_PostcardDoesNotExist_ReturnsNull()
        {
            var result = await _postcardRepository.Get(postcard => postcard.PostcardId == "POSTCARD_ID");

            Check.That(result).IsNull();
        }

        [TestMethod]
        public async Task Get_PostcardExists_ReturnsPostcard()
        {
            var country = new Country
            {
                Code = "CC",
                Name = "country name"
            };

            await InsertData(country);

            var postcardUnderTest = new Postcard
            {
                Country = country,
                PostcardId = "POSTCARD_ID"
            };

            await InsertData(postcardUnderTest);

            var result = await _postcardRepository.Get(postcard => postcard.PostcardId == postcardUnderTest.PostcardId);

            Check.That(result).IsNotNull();
            Check.That(result).HasFieldsWithSameValues(postcardUnderTest);
        }
    }
}
