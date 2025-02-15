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

        private static Country NonTrimmedCountry() => new()
        {
            Code = " XX ",
            Name = " COUNTRY NAME "
        };

        private static Country TrimmedCountry() => new()
        {
            Code = "XX",
            Name = "COUNTRY NAME"
        };

        private static User NonTrimmedUser() => new()
        {
            Name = " NAME ",
            Country = NonTrimmedCountry()
        };

        private static User TrimmedUser() => new()
        {
            Name = "NAME",
            Country = TrimmedCountry()
        };

        private static Postcard NonTrimmedPostcard() => new()
        {
            Country = NonTrimmedCountry(),
            PostcardId = " POSTCARD ID "
        };

        private static Postcard TrimmedPostcard() => new()
        {
            Country = TrimmedCountry(),
            PostcardId = "POSTCARD ID"
        };
    }
}
