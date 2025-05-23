﻿using System.Xml.Linq;
using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

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
        [DataRow(@"<a title=""{0} flag"" href=""/country/{1}""><i class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/postcard"">postcard</a> from <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a>", 1)]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/postcard"">postcard</a> from <a title=""{0} flag"" href=""/country/{1}""><i class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a>", 2)]
        [DataRow(@"<a title=""{0} flag"" href=""/country/{1}""><i class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a> sent a postcard to <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>", 1)]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> sent a postcard to <i title=""{0} flag"" class=""flag flag-{1}""></i> <a href=""/country/{1}"">{0}</a>", 2)]
        [DataRow(@"<a href =""/user/user"">user</a> from <i title=""{0} flag"" class=""flag flag-{1}""></i> <a href=""/country/{1}"">{0}</a> just signed up", 1)]
        [DataRow(@"<a title=""{0} flag"" href=""/country/{1}""><i class=""flag flag-{1}""></i></a> <a href=""/user/user"">user</a> uploaded postcard <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/postcards/postcard"">card</a>", 1)]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> uploaded postcard <a title=""{0} flag"" href=""/country/{1}""><i class=""flag flag-{1}""></i></a> <a href=""/postcards/postcard"">card</a>", 2)]
        public void ToCountry_CountryElementFound_ReturnsCountryModel(string rawEvent, int countryIndex)
        {
            const string country = "TEST_COUNTRY";
            const string countryCode = "TC";
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
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/{0}"">postcard</a> from <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a>")]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> uploaded postcard <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/postcards/{0}"">card</a>")]
        public void ToPostcard_PostcardElementFound_ReturnsPostcardModel(string rawEvent)
        {
            const string cardId = "CARD_ID";
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
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a> received a <a href=""/postcards/postcard"">postcard</a> from <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a>", 1)]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/user"">user</a> received a <a href=""/postcards/postcard"">postcard</a> from <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a>", 2)]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a> sent a postcard to <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a>", 1)]
        [DataRow(@"<a href =""/user/{0}"">{0}</a> from <i title=""country flag"" class=""flag flag-XX""></i> <a href=""/country/XX"">country</a> just signed up", 1)]
        [DataRow(@"<a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/user/{0}"">{0}</a> uploaded postcard <a title=""country flag"" href=""/country/XX""><i class=""flag flag-XX""></i></a> <a href=""/postcards/postcard"">card</a>", 1)]
        public void ToUser_UserElementFoundWithCountry_ReturnsUserModelWithCountry(string rawEvent, int userIndex)
        {
            const string username = "TEST_USER";
            var xelement = XElement.Parse($"<root>{string.Format(rawEvent, username)}</root>");

            var result = xelement.ToUser(userIndex);

            Check.That(result)
                .IsNotNull()
                .And.IsInstanceOf<User>();
            Check.That(result.Name).IsEqualTo(username);
        }
    }
}
