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
    public class UserRepositoryTests : DataTestsBase
    {
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _userRepository = new UserRepository(MemoryRepositoryService);
        }

        [TestMethod]
        public async Task Add_NullUser_ReturnsNullAndDoesNotInsert()
        {
            var result = await _userRepository.Add(null);

            Check.That(result).IsNull();
            Check.ThatCode(_userRepository.All).WhichResult().IsEmpty();
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task Add_InvalidName_ReturnsNullAndDoesNotInsert(string name)
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

            var result = await _userRepository.Add(user);

            Check.That(result).IsNull();
            Check.ThatCode(_userRepository.All).WhichResult().IsEmpty();
        }

        [TestMethod]
        public async Task Add_NullCountry_ReturnsNullAndDoesNotInsert()
        {
            var user = new User
            {
                Name = "user name",
                Country = null
            };

            var result = await _userRepository.Add(user);

            Check.That(result).IsNull();
            Check.ThatCode(_userRepository.All).WhichResult().IsEmpty();
        }

        [TestMethod]
        public async Task Add_InvalidCountryId_ReturnsNullAndDoesNotInsert()
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

            var result = await _userRepository.Add(user);

            Check.That(result).IsNull();
            Check.ThatCode(_userRepository.All).WhichResult().IsEmpty();
        }

        [TestMethod]
        public async Task Add_ValidUser_InsertsAndReturnsUser()
        {
            var country = new Country
            {
                Code = "XX",
                Name = "country name"
            };

            await InsertData(country);

            var user = new User
            {
                Name = "user name",
                Country = country
            };

            var result = await _userRepository.Add(user);

            var allUsers = (await _userRepository.All()).ToList();
            Check.That(result).IsNotNull();
            Check.That(allUsers).CountIs(1);
            Check.That(allUsers[0]).HasFieldsWithSameValues(user);
        }

        [TestMethod]
        public async Task Get_UserDoesNotExist_ReturnsNull()
        {
            var result = await _userRepository.Get(user => user.Name == "USERNAME");

            Check.That(result).IsNull();
        }

        [TestMethod]
        public async Task Get_UserExists_ReturnsUser()
        {
            var country = new Country
            {
                Code = "CC",
                Name = "country name"
            };

            await InsertData(country);

            var userUnderTest = new User
            {
                Country = country,
                Name = "USERNAME"
            };

            await InsertData(userUnderTest);

            var result = await _userRepository.Get(user => user.Name == userUnderTest.Name);

            Check.That(result).IsNotNull();
            Check.That(result).HasFieldsWithSameValues(userUnderTest);
        }
    }
}
