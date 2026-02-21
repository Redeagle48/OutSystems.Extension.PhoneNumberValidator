using PhoneNumbers;
using System;

namespace OutSystems.PhoneNumberValidator
{
    public class PhoneNumberValidator : IPhoneNumberValidator
    {
        private static readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

        private static string? NormalizeRegion(string regionCode)
        {
            return string.IsNullOrWhiteSpace(regionCode)
                ? null
                : regionCode.Trim().ToUpperInvariant();
        }

        public void PhoneNumberValidate(
            string phoneNumber,
            string regionCode,
            out bool isValid,
            out bool isPossibleNumber,
            out string phoneNumberType,
            out string formattedInternational,
            out string formattedNational,
            out string formattedE164,
            out int countryCode,
            out string detectedRegionCode)
        {
            if (phoneNumber is null) throw new ArgumentNullException(nameof(phoneNumber));
            regionCode ??= "";

            isValid = false;
            isPossibleNumber = false;
            phoneNumberType = "UNKNOWN";
            formattedInternational = "";
            formattedNational = "";
            formattedE164 = "";
            countryCode = 0;
            detectedRegionCode = "";

            PhoneNumber number;
            try
            {
                number = _phoneUtil.Parse(phoneNumber, NormalizeRegion(regionCode));
            }
            catch (NumberParseException)
            {
                return;
            }

            isValid = _phoneUtil.IsValidNumber(number);
            isPossibleNumber = _phoneUtil.IsPossibleNumber(number);
            phoneNumberType = _phoneUtil.GetNumberType(number).ToString();
            countryCode = number.CountryCode;
            detectedRegionCode = _phoneUtil.GetRegionCodeForNumber(number) ?? "";

            try
            {
                formattedInternational = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
                formattedNational = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.NATIONAL);
                formattedE164 = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164);
            }
            catch (Exception)
            {
                // If formatting fails for the parsed number, leave as empty strings
            }
        }

        public void PhoneNumberFormat(
            string phoneNumber,
            string regionCode,
            out bool success,
            out string errorMessage,
            out string formattedInternational,
            out string formattedNational,
            out string formattedE164,
            out string formattedRFC3966)
        {
            if (phoneNumber is null) throw new ArgumentNullException(nameof(phoneNumber));
            regionCode ??= "";

            success = false;
            errorMessage = "";
            formattedInternational = "";
            formattedNational = "";
            formattedE164 = "";
            formattedRFC3966 = "";

            PhoneNumber number;
            try
            {
                number = _phoneUtil.Parse(phoneNumber, NormalizeRegion(regionCode));
            }
            catch (NumberParseException ex)
            {
                errorMessage = ex.Message;
                return;
            }

            success = true;
            formattedInternational = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
            formattedNational = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.NATIONAL);
            formattedE164 = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164);
            formattedRFC3966 = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.RFC3966);
        }

        public void PhoneNumberMatch(
            string phoneNumber1,
            string phoneNumber2,
            string regionCode,
            out bool isMatch,
            out string matchType,
            out string errorMessage)
        {
            if (phoneNumber1 is null) throw new ArgumentNullException(nameof(phoneNumber1));
            if (phoneNumber2 is null) throw new ArgumentNullException(nameof(phoneNumber2));
            regionCode ??= "";

            isMatch = false;
            matchType = "NOT_A_NUMBER";
            errorMessage = "";

            var region = NormalizeRegion(regionCode);

            PhoneNumber num1, num2;
            try
            {
                num1 = _phoneUtil.Parse(phoneNumber1, region);
            }
            catch (NumberParseException ex)
            {
                errorMessage = $"phoneNumber1: {ex.Message}";
                return;
            }

            try
            {
                num2 = _phoneUtil.Parse(phoneNumber2, region);
            }
            catch (NumberParseException ex)
            {
                errorMessage = $"phoneNumber2: {ex.Message}";
                return;
            }

            var result = _phoneUtil.IsNumberMatch(num1, num2);
            matchType = result.ToString();
            isMatch = result == PhoneNumberUtil.MatchType.EXACT_MATCH
                   || result == PhoneNumberUtil.MatchType.NSN_MATCH;
        }

        public void PhoneNumberGetRegion(
            string phoneNumber,
            string regionCode,
            out bool success,
            out string errorMessage,
            out string detectedRegionCode,
            out int countryCode)
        {
            if (phoneNumber is null) throw new ArgumentNullException(nameof(phoneNumber));
            regionCode ??= "";

            success = false;
            errorMessage = "";
            detectedRegionCode = "";
            countryCode = 0;

            PhoneNumber number;
            try
            {
                number = _phoneUtil.Parse(phoneNumber, NormalizeRegion(regionCode));
            }
            catch (NumberParseException ex)
            {
                errorMessage = ex.Message;
                return;
            }

            success = true;
            countryCode = number.CountryCode;
            detectedRegionCode = _phoneUtil.GetRegionCodeForNumber(number) ?? "";
        }
    }
}
