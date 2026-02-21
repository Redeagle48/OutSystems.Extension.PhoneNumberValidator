using NUnit.Framework;
using PhoneNumbers;
using Validator = OutSystems.PhoneNumberValidator.PhoneNumberValidator;

namespace OutSystems.PhoneNumberValidator.Tests
{
    [TestFixture]
    public class PhoneNumberFormatTests
    {
        private static readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

        [Test]
        public void Format_USNumber_AllFormats()
        {
            var number = _phoneUtil.Parse("+16502530000", null);

            Assert.Multiple(() =>
            {
                Assert.That(_phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL),
                    Is.EqualTo("+1 650-253-0000"));
                Assert.That(_phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.NATIONAL),
                    Is.EqualTo("(650) 253-0000"));
                Assert.That(_phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164),
                    Is.EqualTo("+16502530000"));
                Assert.That(_phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.RFC3966),
                    Is.EqualTo("tel:+1-650-253-0000"));
            });
        }

        [Test]
        public void Format_PTNumber_AllFormats()
        {
            var number = _phoneUtil.Parse("+351912345678", null);

            Assert.Multiple(() =>
            {
                Assert.That(_phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164),
                    Is.EqualTo("+351912345678"));
                Assert.That(_phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL),
                    Does.StartWith("+351"));
            });
        }

        // ── Wrapper integration tests ──

        [Test]
        public void PhoneNumberFormat_ValidNumber_ReturnsAllFormats()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "+1 650-253-0000", "",
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(error, Is.Empty);
                Assert.That(formats.International, Is.EqualTo("+1 650-253-0000"));
                Assert.That(formats.National, Is.EqualTo("(650) 253-0000"));
                Assert.That(formats.E164, Is.EqualTo("+16502530000"));
                Assert.That(formats.RFC3966, Is.EqualTo("tel:+1-650-253-0000"));
            });
        }

        [Test]
        public void PhoneNumberFormat_PortugalNumber_ReturnsAllFormats()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "+351912345678", "",
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(formats.E164, Is.EqualTo("+351912345678"));
                Assert.That(formats.RFC3966, Does.StartWith("tel:"));
            });
        }

        [Test]
        public void PhoneNumberFormat_InvalidNumber_ReturnsFalseWithError()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "garbage", "US",
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(error, Is.Not.Empty);
                Assert.That(formats.International, Is.Empty);
                Assert.That(formats.National, Is.Empty);
                Assert.That(formats.E164, Is.Empty);
                Assert.That(formats.RFC3966, Is.Empty);
            });
        }

        [Test]
        public void PhoneNumberFormat_NullInput_ThrowsArgumentNullException()
        {
            var validator = new Validator();
            Assert.Throws<ArgumentNullException>(() =>
                validator.PhoneNumberFormat(null!, "US", out _, out _, out _));
        }

        [Test]
        public void PhoneNumberFormat_NationalWithRegion_Succeeds()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "(650) 253-0000", "US",
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(formats.E164, Is.EqualTo("+16502530000"));
            });
        }

        // ── Edge cases: region code handling ──

        [Test]
        public void PhoneNumberFormat_LowercaseRegion_Works()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "(650) 253-0000", "us",
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(formats.E164, Is.EqualTo("+16502530000"));
            });
        }

        [Test]
        public void PhoneNumberFormat_PaddedRegion_Works()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "(650) 253-0000", " US ",
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.That(success, Is.True);
        }

        [Test]
        public void PhoneNumberFormat_InvalidRegionCode_NationalNumber_Fails()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "(650) 253-0000", "XX",
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.That(success, Is.False);
        }

        [Test]
        public void PhoneNumberFormat_NullRegionCode_InternationalNumber_Succeeds()
        {
            var validator = new Validator();
            validator.PhoneNumberFormat(
                "+16502530000", null!,
                out bool success, out string error, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(formats.E164, Is.EqualTo("+16502530000"));
            });
        }
    }
}
