using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Repositories;
using abremir.postcrossing.engine.tests.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Linq;

namespace abremir.postcrossing.engine.tests.Repositories
{
    [TestClass]
    public class CountryRepositoryTests : DataTestsBase
    {
        private readonly ICountryRepository _countryRepository;

        public CountryRepositoryTests()
        {
            _countryRepository = new CountryRepository(MemoryRepositoryService);
        }

        [TestMethod]
        public void Add_NullCountry_ReturnsNullAndDoesNotInsert()
        {
            var result = _countryRepository.Add(null);

            Check.That(result).IsNull();
            Check.That(_countryRepository.All()).CountIs(0);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Add_InvalidName_ReturnsNullAndDoesNotInsert(string name)
        {
            var country = new Country
            {
                Code = "XX",
                Name = name
            };

            var result = _countryRepository.Add(country);

            Check.That(result).IsNull();
            Check.That(_countryRepository.All()).CountIs(0);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Add_InvalidCode_ReturnsNullAndDoesNotInsert(string code)
        {
            var country = new Country
            {
                Code = code,
                Name = "country name"
            };

            var result = _countryRepository.Add(country);

            Check.That(result).IsNull();
            Check.That(_countryRepository.All()).CountIs(0);
        }

        [TestMethod]
        public void Add_ValidCountryModel_InsertsAndReturnsCountry()
        {
            var country = new Country
            {
                Code = "XX",
                Name = "country name"
            };

            var result = _countryRepository.Add(country);

            var allCountries = _countryRepository.All().ToList();
            Check.That(result).IsNotNull();
            Check.That(allCountries).CountIs(1);
            Check.That(allCountries[0]).HasFieldsWithSameValues(country);
        }

        [TestMethod]
        public void Get_CountryDoesNotExist_ReturnsNull()
        {
            var result = _countryRepository.Get(country => country.Code == "XX");

            Check.That(result).IsNull();
        }

        [TestMethod]
        public void Get_CountryExists_ReturnsCountry()
        {
            var countryUnderTest = new Country
            {
                Code = "CC",
                Name = "country name"
            };

            InsertData(countryUnderTest);

            var result = _countryRepository.Get(country => country.Code == countryUnderTest.Code);

            Check.That(result).IsNotNull();
            Check.That(result).HasFieldsWithSameValues(countryUnderTest);
        }
    }
}
