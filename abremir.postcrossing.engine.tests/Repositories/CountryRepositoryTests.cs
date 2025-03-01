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
    public class CountryRepositoryTests : DataTestsBase
    {
        private readonly CountryRepository _countryRepository;

        public CountryRepositoryTests()
        {
            _countryRepository = new CountryRepository(MemoryRepositoryService);
        }

        [TestMethod]
        public async Task Add_NullCountry_ReturnsNullAndDoesNotInsert()
        {
            var result = await _countryRepository.Add(null);

            Check.That(result).IsNull();
            Check.ThatCode(_countryRepository.All).WhichResult().IsEmpty();
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Add_InvalidName_ReturnsNullAndDoesNotInsert(string name)
        {
            var country = new Country
            {
                Code = "XX",
                Name = name
            };

            var result = await _countryRepository.Add(country);

            Check.That(result).IsNull();
            Check.ThatCode(_countryRepository.All).WhichResult().IsEmpty();
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Add_InvalidCode_ReturnsNullAndDoesNotInsert(string code)
        {
            var country = new Country
            {
                Code = code,
                Name = "country name"
            };

            var result = await _countryRepository.Add(country);

            Check.That(result).IsNull();
            Check.ThatCode(_countryRepository.All).WhichResult().IsEmpty();
        }

        [TestMethod]
        public async Task Add_ValidCountryModel_InsertsAndReturnsCountry()
        {
            var country = new Country
            {
                Code = "XX",
                Name = "country name"
            };

            var result = await _countryRepository.Add(country);

            var allCountries = (await _countryRepository.All()).ToList();
            Check.That(result).IsNotNull();
            Check.That(allCountries).CountIs(1);
            Check.That(allCountries[0]).HasFieldsWithSameValues(country);
        }

        [TestMethod]
        public async Task Get_CountryDoesNotExist_ReturnsNull()
        {
            var result = await _countryRepository.Get(country => country.Code == "XX");

            Check.That(result).IsNull();
        }

        [TestMethod]
        public async Task Get_CountryExists_ReturnsCountry()
        {
            var countryUnderTest = new Country
            {
                Code = "CC",
                Name = "country name"
            };

            await InsertData(countryUnderTest);

            var result = await _countryRepository.Get(country => country.Code == countryUnderTest.Code);

            Check.That(result).IsNotNull();
            Check.That(result).HasFieldsWithSameValues(countryUnderTest);
        }
    }
}
