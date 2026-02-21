using NUnit.Framework;
using PhoneNumbers;
using Validator = OutSystems.PhoneNumberValidator.PhoneNumberValidator;

namespace OutSystems.PhoneNumberValidator.Tests
{
    [TestFixture]
    public class PhoneNumberValidateTests
    {
        private static readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

        // ── Valid international numbers ──

        [TestCase("+1 650-253-0000", "US")]
        [TestCase("+44 20 7946 0958", "GB")]
        [TestCase("+351 912 345 678", "PT")]
        [TestCase("+49 30 123456", "DE")]
        [TestCase("+81 3-1234-5678", "JP")]
        [TestCase("+61 2 1234 5678", "AU")]
        public void Parse_ValidInternationalNumbers_IsValid(string number, string region)
        {
            var parsed = _phoneUtil.Parse(number, region);
            Assert.That(_phoneUtil.IsValidNumber(parsed), Is.True,
                $"Expected '{number}' to be valid.");
        }

        // ── Valid national numbers with region ──

        [TestCase("(650) 253-0000", "US")]
        [TestCase("020 7946 0958", "GB")]
        [TestCase("912 345 678", "PT")]
        public void Parse_ValidNationalNumbers_IsValid(string number, string region)
        {
            var parsed = _phoneUtil.Parse(number, region);
            Assert.That(_phoneUtil.IsValidNumber(parsed), Is.True,
                $"Expected '{number}' with region '{region}' to be valid.");
        }

        // ── Invalid numbers ──

        [TestCase("not-a-number", "US")]
        [TestCase("123", "US")]
        [TestCase("+1 000-000-0000", "US")]
        public void Parse_InvalidNumbers_IsNotValid(string number, string region)
        {
            try
            {
                var parsed = _phoneUtil.Parse(number, region);
                Assert.That(_phoneUtil.IsValidNumber(parsed), Is.False,
                    $"Expected '{number}' to be invalid.");
            }
            catch (NumberParseException)
            {
                Assert.Pass("Number could not be parsed (expected for invalid input).");
            }
        }

        // ── Number type detection ──

        [TestCase("+44 7911 123456", "GB", PhoneNumberType.MOBILE)]
        [TestCase("+1 800-234-5678", "US", PhoneNumberType.TOLL_FREE)]
        public void GetNumberType_ReturnsExpectedType(
            string number, string region, PhoneNumberType expectedType)
        {
            var parsed = _phoneUtil.Parse(number, region);
            Assert.That(_phoneUtil.GetNumberType(parsed), Is.EqualTo(expectedType));
        }

        // ── E.164 without region ──

        [TestCase("+16502530000")]
        [TestCase("+442079460958")]
        [TestCase("+351912345678")]
        public void Parse_E164WithoutRegion_Succeeds(string number)
        {
            var parsed = _phoneUtil.Parse(number, null);
            Assert.That(_phoneUtil.IsValidNumber(parsed), Is.True);
        }

        // ── Wrapper integration tests ──

        [Test]
        public void PhoneNumberValidate_ValidNumber_ReturnsExpectedOutputs()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "+1 650-253-0000", "US",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(info.IsValid, Is.True);
                Assert.That(info.IsPossibleNumber, Is.True);
                Assert.That(info.PhoneNumberType, Is.Not.Empty);
                Assert.That(info.CountryCode, Is.EqualTo(1));
                Assert.That(info.DetectedRegionCode, Is.EqualTo("US"));
                Assert.That(formats.International, Does.StartWith("+1"));
                Assert.That(formats.E164, Is.EqualTo("+16502530000"));
            });
        }

        [Test]
        public void PhoneNumberValidate_PortugalMobile_ReturnsExpectedOutputs()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "+351 912 345 678", "",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(info.IsValid, Is.True);
                Assert.That(info.IsPossibleNumber, Is.True);
                Assert.That(info.PhoneNumberType, Is.EqualTo("MOBILE"));
                Assert.That(info.CountryCode, Is.EqualTo(351));
                Assert.That(info.DetectedRegionCode, Is.EqualTo("PT"));
                Assert.That(formats.E164, Is.EqualTo("+351912345678"));
            });
        }

        [Test]
        public void PhoneNumberValidate_InvalidNumber_ReturnsFalse()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "not-a-number", "US",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(info.IsValid, Is.False);
                Assert.That(formats.E164, Is.Empty);
            });
        }

        [Test]
        public void PhoneNumberValidate_NullInput_ThrowsArgumentNullException()
        {
            var validator = new Validator();
            Assert.Throws<ArgumentNullException>(() =>
                validator.PhoneNumberValidate(null!, "US", out _, out _));
        }

        [Test]
        public void PhoneNumberValidate_NationalFormatWithRegion_Works()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "(650) 253-0000", "US",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(info.IsValid, Is.True);
                Assert.That(info.CountryCode, Is.EqualTo(1));
                Assert.That(info.DetectedRegionCode, Is.EqualTo("US"));
            });
        }

        [Test]
        public void PhoneNumberValidate_EmptyString_ReturnsFalse()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "", "US",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.That(info.IsValid, Is.False);
        }

        // ── Edge cases: region code handling ──

        [Test]
        public void PhoneNumberValidate_LowercaseRegion_Works()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "(650) 253-0000", "us",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.That(info.IsValid, Is.True);
        }

        [Test]
        public void PhoneNumberValidate_PaddedRegion_Works()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "(650) 253-0000", " US ",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.That(info.IsValid, Is.True);
        }

        [Test]
        public void PhoneNumberValidate_NullRegionCode_WithInternationalNumber()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "+351912345678", null!,
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(info.IsValid, Is.True);
                Assert.That(info.DetectedRegionCode, Is.EqualTo("PT"));
            });
        }

        [Test]
        public void PhoneNumberValidate_InvalidRegionCode_WithInternationalNumber()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "+351912345678", "XX",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(info.IsValid, Is.True);
                Assert.That(info.DetectedRegionCode, Is.EqualTo("PT"));
            });
        }

        [Test]
        public void PhoneNumberValidate_InvalidRegionCode_WithNationalNumber_ReturnsFalse()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "(650) 253-0000", "XX",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.That(info.IsValid, Is.False);
        }

        [Test]
        public void PhoneNumberValidate_VeryLongInput_ReturnsFalse()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                new string('1', 500), "US",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.That(info.IsValid, Is.False);
        }

        [Test]
        public void PhoneNumberValidate_ValidNumber_FormatsIncludeRFC3966()
        {
            var validator = new Validator();
            validator.PhoneNumberValidate(
                "+16502530000", "",
                out PhoneNumberInfo info, out PhoneNumberFormats formats);

            Assert.Multiple(() =>
            {
                Assert.That(info.IsValid, Is.True);
                Assert.That(formats.RFC3966, Is.EqualTo("tel:+1-650-253-0000"));
            });
        }
    }
}
