using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Repositories;
using abremir.postcrossing.engine.tests.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Linq;

namespace abremir.postcrossing.engine.tests.Repositories
{
    [TestClass]
    public class PostcardRepositoryTests : DataTestsBase
    {
        private readonly IPostcardRepository _postcardRepository;

        public PostcardRepositoryTests()
        {
            _postcardRepository = new PostcardRepository(MemoryRepositoryService);
        }

        [TestMethod]
        public void Add_NullPOstcard_ReturnsNullAndDoesNotInsert()
        {
            var result = _postcardRepository.Add(null);

            Check.That(result).IsNull();
            Check.That(_postcardRepository.All()).CountIs(0);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Add_InvalidPostcardId_ReturnsNullAndDoesNotInsert(string postcardId)
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

            var result = _postcardRepository.Add(postcard);

            Check.That(result).IsNull();
            Check.That(_postcardRepository.All()).CountIs(0);
        }

        [TestMethod]
        public void Add_NullCountry_ReturnsNullAndDoesNotInsert()
        {
            var postcard = new Postcard
            {
                PostcardId = "POSTCARD_ID",
                Country = null
            };

            var result = _postcardRepository.Add(postcard);

            Check.That(result).IsNull();
            Check.That(_postcardRepository.All()).CountIs(0);
        }

        [TestMethod]
        public void Add_InvalidCountryId_ReturnsNullAndDoesNotInsert()
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

            var result = _postcardRepository.Add(postcard);

            Check.That(result).IsNull();
            Check.That(_postcardRepository.All()).CountIs(0);
        }

        [TestMethod]
        public void Add_ValidPostcard_InsertsAndReturnsPostcard()
        {
            var country = new Country
            {
                Code = "XX",
                Name = "country name"
            };

            InsertData(country);

            var postcard = new Postcard
            {
                PostcardId = "POSTCARD_ID",
                Country = country
            };

            var result = _postcardRepository.Add(postcard);

            var allPostcards = _postcardRepository.All().ToList();
            Check.That(result).IsNotNull();
            Check.That(allPostcards).CountIs(1);
            Check.That(allPostcards[0]).HasFieldsWithSameValues(postcard);
        }

        [TestMethod]
        public void Get_PostcardDoesNotExist_ReturnsNull()
        {
            var result = _postcardRepository.Get(postcard => postcard.PostcardId == "POSTCARD_ID");

            Check.That(result).IsNull();
        }

        [TestMethod]
        public void Get_PostcardExists_ReturnsPostcard()
        {
            var country = new Country
            {
                Code = "CC",
                Name = "country name"
            };

            InsertData(country);

            var postcardUnderTest = new Postcard
            {
                Country = country,
                PostcardId = "POSTCARD_ID"
            };

            InsertData(postcardUnderTest);

            var result = _postcardRepository.Get(postcard => postcard.PostcardId == postcardUnderTest.PostcardId);

            Check.That(result).IsNotNull();
            Check.That(result).HasFieldsWithSameValues(postcardUnderTest);
        }
    }
}
