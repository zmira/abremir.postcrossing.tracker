using abremir.postcrossing.engine.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System;

namespace abremir.postcrossing.engine.tests.Extensions
{
    [TestClass]
    public class DateTimeOffsetExtensionTests
    {
        [DataTestMethod]
        [DataRow(119, 110)]
        [DataRow(990, 990)]
        public void ToHundredthOfSecond_WithThousandthOfSecondValue_ReturnsDateTimeOffsetWithZeroThousandthOfSecondValue(int milliseconds, int expectedMilliseconds)
        {
            var dateTimeOffsetUnderTest = new DateTimeOffset(2020, 1, 1, 1, 1, 1, milliseconds, DateTimeOffset.Now.Offset);
            var expectedResult = new DateTimeOffset(2020, 1, 1, 1, 1, 1, expectedMilliseconds, DateTimeOffset.Now.Offset);

            var result = dateTimeOffsetUnderTest.ToHundredthOfSecond();

            Check.That(result).IsEqualTo(expectedResult);
        }
    }
}
