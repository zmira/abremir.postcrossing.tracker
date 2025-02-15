using System.Linq;
using abremir.postcrossing.engine.Models;
using abremir.postcrossing.engine.Repositories;
using abremir.postcrossing.engine.tests.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace abremir.postcrossing.engine.tests.Repositories
{
    [TestClass]
    public class UserRepositoryTests : DataTestsBase
    {
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _userRepository = new UserRepository(MemoryRepositoryService);
        }

        [TestMethod]
        public void Add_NullUser_ReturnsNullAndDoesNotInsert()
        {
            var result = _userRepository.Add(null);

            Check.That(result).IsNull();
            Check.That(_userRepository.All()).CountIs(0);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Add_InvalidName_ReturnsNullAndDoesNotInsert(string name)
        {
            var user = new User
            {
                Name = name,
                Country = new Country
                {
                    Id = 1,
                    Code = "XX",
                    Name = "country name"
                }
            };

            var result = _userRepository.Add(user);

            Check.That(result).IsNull();
            Check.That(_userRepository.All()).CountIs(0);
        }

        [TestMethod]
        public void Add_NullCountry_ReturnsNullAndDoesNotInsert()
        {
            var user = new User
            {
                Name = "user name",
                Country = null
            };

            var result = _userRepository.Add(user);

            Check.That(result).IsNull();
            Check.That(_userRepository.All()).CountIs(0);
        }

        [TestMethod]
        public void Add_InvalidCountryId_ReturnsNullAndDoesNotInsert()
        {
            var user = new User
            {
                Name = "user name",
                Country = new Country
                {
                    Id = 0,
                    Code = "XX",
                    Name = "country name"
                }
            };

            var result = _userRepository.Add(user);

            Check.That(result).IsNull();
            Check.That(_userRepository.All()).CountIs(0);
        }

        [TestMethod]
        public void Add_ValidUser_InsertsAndReturnsUser()
        {
            var country = new Country
            {
                Code = "XX",
                Name = "country name"
            };

            InsertData(country);

            var user = new User
            {
                Name = "user name",
                Country = country
            };

            var result = _userRepository.Add(user);

            var allUsers = _userRepository.All().ToList();
            Check.That(result).IsNotNull();
            Check.That(allUsers).CountIs(1);
            Check.That(allUsers[0]).HasFieldsWithSameValues(user);
        }

        [TestMethod]
        public void Get_UserDoesNotExist_ReturnsNull()
        {
            var result = _userRepository.Get(user => user.Name == "USERNAME");

            Check.That(result).IsNull();
        }

        [TestMethod]
        public void Get_UserExists_ReturnsUser()
        {
            var country = new Country
            {
                Code = "CC",
                Name = "country name"
            };

            InsertData(country);

            var userUnderTest = new User
            {
                Country = country,
                Name = "USERNAME"
            };

            InsertData(userUnderTest);

            var result = _userRepository.Get(user => user.Name == userUnderTest.Name);

            Check.That(result).IsNotNull();
            Check.That(result).HasFieldsWithSameValues(userUnderTest);
        }
    }
}
