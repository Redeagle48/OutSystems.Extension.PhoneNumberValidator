using NUnit.Framework;
using PhoneNumbers;
using Validator = OutSystems.PhoneNumberValidator.PhoneNumberValidator;

namespace OutSystems.PhoneNumberValidator.Tests
{
    [TestFixture]
    public class PhoneNumberMatchTests
    {
        private static readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

        [Test]
        public void IsNumberMatch_ExactMatch()
        {
            var num1 = _phoneUtil.Parse("+1 650-253-0000", "US");
            var num2 = _phoneUtil.Parse("+16502530000", "US");
            Assert.That(_phoneUtil.IsNumberMatch(num1, num2),
                Is.EqualTo(PhoneNumberUtil.MatchType.EXACT_MATCH));
        }

        [Test]
        public void IsNumberMatch_NoMatch()
        {
            var num1 = _phoneUtil.Parse("+1 650-253-0000", "US");
            var num2 = _phoneUtil.Parse("+44 20 7946 0958", "GB");
            Assert.That(_phoneUtil.IsNumberMatch(num1, num2),
                Is.EqualTo(PhoneNumberUtil.MatchType.NO_MATCH));
        }

        // ── Wrapper integration tests ──

        [Test]
        public void PhoneNumberMatch_SameNumber_DifferentFormats()
        {
            var validator = new Validator();
            validator.PhoneNumberMatch(
                "+1 650-253-0000", "+16502530000", "US",
                out bool isMatch, out string matchType, out string errorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(isMatch, Is.True);
                Assert.That(matchType, Is.EqualTo("EXACT_MATCH"));
                Assert.That(errorMessage, Is.Empty);
            });
        }

        [Test]
        public void PhoneNumberMatch_NationalFormats_SameRegion()
        {
            var validator = new Validator();
            validator.PhoneNumberMatch(
                "(650) 253-0000", "650-253-0000", "US",
                out bool isMatch, out string matchType, out string errorMessage);

            Assert.That(isMatch, Is.True);
        }

        [Test]
        public void PhoneNumberMatch_DifferentNumbers()
        {
            var validator = new Validator();
            validator.PhoneNumberMatch(
                "+1 650-253-0000", "+44 20 7946 0958", "",
                out bool isMatch, out string matchType, out string errorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(isMatch, Is.False);
                Assert.That(matchType, Is.EqualTo("NO_MATCH"));
                Assert.That(errorMessage, Is.Empty);
            });
        }

        [Test]
        public void PhoneNumberMatch_FirstNumberInvalid_ReturnsErrorForPhoneNumber1()
        {
            var validator = new Validator();
            validator.PhoneNumberMatch(
                "garbage", "+1 650-253-0000", "US",
                out bool isMatch, out string matchType, out string errorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(isMatch, Is.False);
                Assert.That(matchType, Is.EqualTo("NOT_A_NUMBER"));
                Assert.That(errorMessage, Does.StartWith("phoneNumber1:"));
            });
        }

        [Test]
        public void PhoneNumberMatch_SecondNumberInvalid_ReturnsErrorForPhoneNumber2()
        {
            var validator = new Validator();
            validator.PhoneNumberMatch(
                "+1 650-253-0000", "garbage", "US",
                out bool isMatch, out string matchType, out string errorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(isMatch, Is.False);
                Assert.That(matchType, Is.EqualTo("NOT_A_NUMBER"));
                Assert.That(errorMessage, Does.StartWith("phoneNumber2:"));
            });
        }

        [Test]
        public void PhoneNumberMatch_NullInput_ThrowsArgumentNullException()
        {
            var validator = new Validator();
            Assert.Throws<ArgumentNullException>(() =>
                validator.PhoneNumberMatch(
                    null!, "+1 650-253-0000", "US",
                    out _, out _, out _));
        }

        [Test]
        public void PhoneNumberMatch_NullSecondInput_ThrowsArgumentNullException()
        {
            var validator = new Validator();
            Assert.Throws<ArgumentNullException>(() =>
                validator.PhoneNumberMatch(
                    "+1 650-253-0000", null!, "US",
                    out _, out _, out _));
        }

        // ── Edge cases: region code handling ──

        [Test]
        public void PhoneNumberMatch_LowercaseRegion_Works()
        {
            var validator = new Validator();
            validator.PhoneNumberMatch(
                "(650) 253-0000", "650-253-0000", "us",
                out bool isMatch, out string matchType, out string errorMessage);

            Assert.That(isMatch, Is.True);
        }

        [Test]
        public void PhoneNumberMatch_NullRegionCode_WithInternationalNumbers()
        {
            var validator = new Validator();
            validator.PhoneNumberMatch(
                "+16502530000", "+1 650-253-0000", null!,
                out bool isMatch, out string matchType, out string errorMessage);

            Assert.Multiple(() =>
            {
                Assert.That(isMatch, Is.True);
                Assert.That(matchType, Is.EqualTo("EXACT_MATCH"));
            });
        }
    }
}
