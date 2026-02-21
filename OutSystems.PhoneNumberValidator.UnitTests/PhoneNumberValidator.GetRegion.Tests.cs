using NUnit.Framework;
using PhoneNumbers;
using Validator = OutSystems.PhoneNumberValidator.PhoneNumberValidator;

namespace OutSystems.PhoneNumberValidator.Tests
{
    [TestFixture]
    public class PhoneNumberGetRegionTests
    {
        private static readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

        [TestCase("+1 650-253-0000", "US", 1)]
        [TestCase("+44 20 7946 0958", "GB", 44)]
        [TestCase("+351 912 345 678", "PT", 351)]
        [TestCase("+49 30 123456", "DE", 49)]
        [TestCase("+81 3-1234-5678", "JP", 81)]
        public void GetRegion_InternationalNumbers(
            string number, string expectedRegion, int expectedCountryCode)
        {
            var parsed = _phoneUtil.Parse(number, null);
            Assert.Multiple(() =>
            {
                Assert.That(_phoneUtil.GetRegionCodeForNumber(parsed),
                    Is.EqualTo(expectedRegion));
                Assert.That(parsed.CountryCode, Is.EqualTo(expectedCountryCode));
            });
        }

        // ── Wrapper integration tests ──

        [Test]
        public void PhoneNumberGetRegion_ValidNumber_Portugal()
        {
            var validator = new Validator();
            validator.PhoneNumberGetRegion(
                "+351 912 345 678", "",
                out bool success, out string error,
                out string region, out int code);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(error, Is.Empty);
                Assert.That(region, Is.EqualTo("PT"));
                Assert.That(code, Is.EqualTo(351));
            });
        }

        [Test]
        public void PhoneNumberGetRegion_ValidNumber_UK()
        {
            var validator = new Validator();
            validator.PhoneNumberGetRegion(
                "+44 20 7946 0958", "",
                out bool success, out string error,
                out string region, out int code);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(region, Is.EqualTo("GB"));
                Assert.That(code, Is.EqualTo(44));
            });
        }

        [Test]
        public void PhoneNumberGetRegion_NationalWithRegion()
        {
            var validator = new Validator();
            validator.PhoneNumberGetRegion(
                "(650) 253-0000", "US",
                out bool success, out string error,
                out string region, out int code);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(region, Is.EqualTo("US"));
                Assert.That(code, Is.EqualTo(1));
            });
        }

        [Test]
        public void PhoneNumberGetRegion_InvalidNumber_ReturnsFalse()
        {
            var validator = new Validator();
            validator.PhoneNumberGetRegion(
                "garbage", "US",
                out bool success, out string error,
                out string region, out int code);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(error, Is.Not.Empty);
                Assert.That(region, Is.Empty);
                Assert.That(code, Is.EqualTo(0));
            });
        }

        [Test]
        public void PhoneNumberGetRegion_NullInput_ThrowsArgumentNullException()
        {
            var validator = new Validator();
            Assert.Throws<ArgumentNullException>(() =>
                validator.PhoneNumberGetRegion(
                    null!, "US",
                    out _, out _, out _, out _));
        }

        // ── Edge cases: region code handling ──

        [Test]
        public void PhoneNumberGetRegion_LowercaseRegion_Works()
        {
            var validator = new Validator();
            validator.PhoneNumberGetRegion(
                "(650) 253-0000", "us",
                out bool success, out string error,
                out string region, out int code);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(region, Is.EqualTo("US"));
            });
        }

        [Test]
        public void PhoneNumberGetRegion_NullRegionCode_InternationalNumber()
        {
            var validator = new Validator();
            validator.PhoneNumberGetRegion(
                "+351912345678", null!,
                out bool success, out string error,
                out string region, out int code);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(region, Is.EqualTo("PT"));
                Assert.That(code, Is.EqualTo(351));
            });
        }

        [Test]
        public void PhoneNumberGetRegion_InvalidRegionCode_InternationalNumber()
        {
            var validator = new Validator();
            validator.PhoneNumberGetRegion(
                "+351912345678", "ZZZ",
                out bool success, out string error,
                out string region, out int code);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(region, Is.EqualTo("PT"));
            });
        }
    }
}
