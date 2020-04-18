using abremir.postcrossing.engine.Extensions;
using abremir.postcrossing.engine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace abremir.postcrossing.engine.tests.Extensions
{
    [TestClass]
    public class ObjectExtensionTests
    {
        [TestMethod]
        public void TrimAllStrings_NullObject_DoesNotThrow()
        {
            object TestObject = null;

            Check.ThatCode(() => TestObject.TrimAllStrings()).Not.ThrowsAny();
        }

        [TestMethod]
        public void TrimAllStrings_CountryObject_AllStringsTrimmed()
        {
            var country = NonTrimmedCountry();
            var expectedResult = TrimmedCountry();

            country.TrimAllStrings();

            Check.That(country).HasFieldsWithSameValues(expectedResult);
        }

        [TestMethod]
        public void TrimAllStrings_PostcardDocumentObject_AllStringsTrimmed()
        {
            var postcard = NonTrimmedPostcard();
            var expectedResult = TrimmedPostcard();

            postcard.TrimAllStrings();

            Check.That(postcard).HasFieldsWithSameValues(expectedResult);
        }

        [TestMethod]
        public void TrimAllStrings_UserObject_AllStringsTrimmed()
        {
            var user = NonTrimmedUser();
            var expectedResult = TrimmedUser();

            user.TrimAllStrings();

            Check.That(user).HasFieldsWithSameValues(expectedResult);
        }

        private Country NonTrimmedCountry() => new Country
        {
            Code = " XX ",
            Name = " COUNTRY NAME "
        };

        private Country TrimmedCountry() => new Country
        {
            Code = "XX",
            Name = "COUNTRY NAME"
        };

        private User NonTrimmedUser() => new User
        {
            Name = " NAME ",
            Country = NonTrimmedCountry()
        };

        private User TrimmedUser() => new User
        {
            Name = "NAME",
            Country = TrimmedCountry()
        };

        private Postcard NonTrimmedPostcard() => new Postcard
        {
            Country = NonTrimmedCountry(),
            PostcardId = " POSTCARD ID "
        };

        private Postcard TrimmedPostcard() => new Postcard
        {
            Country = TrimmedCountry(),
            PostcardId = "POSTCARD ID"
        };
    }
}
