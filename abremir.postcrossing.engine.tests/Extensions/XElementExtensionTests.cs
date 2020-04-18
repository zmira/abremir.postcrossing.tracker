using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Xml.Linq;

namespace abremir.postcrossing.engine.tests.Extensions
{
    [TestClass]
    public class XElementExtensionTests
    {
        [TestMethod]
        public void ToCountry_CountryElementNotFound_ReturnsNull()
        {
            var xelement = XElement.Parse("<root></root>");

            var result = xelement.ToCountry();

            Check.That(result).IsNull();
        }

        [DataTestMethod]
        [DataRow(@"<a href=""/country/{1}""><i title=""{0}"" class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/card"">postcard</a> from <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a>", 1)]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/card"">postcard</a> from <a href=""/country/{1}""><i title=""{0}"" class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a>", 2)]
        [DataRow(@"<a href=""/country/{1}""><i title=""{0}"" class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a> sent a postcard to <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/country/XX"">country</a>", 1)]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> sent a postcard to <a href=""/country/{1}""><i title=""{0}"" class=""flag flag-{1}""></i></a> <a href=""/country/{1}"">{0}</a>", 3)]
        [DataRow(@"<a href=""/user/user"">user</a> from <i title=""{0}"" class=""flag flag-{1}""></i> <a href=""/country/{1}"">{0}</a> just signed up", 1)]
        [DataRow(@"<a href=""/country/{1}""><i title=""{0}"" class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a> uploaded postcard <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/postcards/card"">card</a>", 1)]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> uploaded postcard <a href=""/country/{1}""><i title=""{0}"" class=""flag flag-{1}""></i></a> <a href=""/postcards/card"">card</a>", 2)]
        public void ToCountry_CountryElementFound_ReturnsCountryModel(string rawEvent, int countryIndex)
        {
            var country = "TEST_COUNTRY";
            var countryCode = "TC";
            var xelement = XElement.Parse($"<root>{string.Format(rawEvent, country, countryCode)}</root>");

            var result = xelement.ToCountry(countryIndex);

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<Country>();
            Check.That(result.Code).IsEqualTo(countryCode);
            Check.That(result.Name).IsEqualTo(country);
        }

        [TestMethod]
        public void ToPostcard_PostcardElementNotFoundReturnsNull()
        {
            var xelement = XElement.Parse("<root></root>");

            var result = xelement.ToPostcard();

            Check.That(result).IsNull();
        }

        [DataTestMethod]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/{0}"">postcard</a> from <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a>")]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> uploaded postcard <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/postcards/{0}"">{0}</a>")]
        public void ToPostcard_PostcardElementFound_ReturnsPostcardModel(string rawEvent)
        {
            var cardId = "CARD_ID";
            var xelement = XElement.Parse($"<root>{string.Format(rawEvent, cardId)}</root>");

            var result = xelement.ToPostcard();

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<Postcard>();
            Check.That(result.PostcardId).IsEqualTo(cardId);
        }

        [TestMethod]
        public void ToUser_UserElementNotFound_ReturnNull()
        {
            var xelement = XElement.Parse("<root></root>");

            var result = xelement.ToUser();

            Check.That(result).IsNull();
        }

        [DataTestMethod]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a> received a <a href=""/postcards/card"">postcard</a> from <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a>", 1)]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/card"">postcard</a> from <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a>", 2)]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a> sent a postcard to <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/country/XX"">country</a>", 1)]
        [DataRow(@"<a href=""/user/{0}"">{0}</a> from <i title=""country"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a> just signed up", 1)]
        [DataRow(@"<a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a> uploaded postcard <a href=""/country/XX""><i title=""country"" class=""flag flag-XX""></i></a> <a href=""/postcards/card"">card</a>", 1)]
        public void ToUser_UserElementFoundWithCountry_ReturnsUserModelWithCountry(string rawEvent, int userIndex)
        {
            var username = "TEST_USER";
            var xelement = XElement.Parse($"<root>{string.Format(rawEvent, username)}</root>");

            var result = xelement.ToUser(userIndex);

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<User>();
            Check.That(result.Name).IsEqualTo(username);
        }
    }
}
