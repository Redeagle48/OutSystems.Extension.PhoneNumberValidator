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
            out PhoneNumberInfo phoneNumberInfo,
            out PhoneNumberFormats phoneNumberFormats)
        {
            if (phoneNumber is null) throw new ArgumentNullException(nameof(phoneNumber));
            regionCode ??= "";

            phoneNumberInfo = new PhoneNumberInfo
            {
                IsValid = false,
                IsPossibleNumber = false,
                PhoneNumberType = "UNKNOWN",
                CountryCode = 0,
                DetectedRegionCode = ""
            };

            phoneNumberFormats = new PhoneNumberFormats
            {
                International = "",
                National = "",
                E164 = "",
                RFC3966 = ""
            };

            PhoneNumber number;
            try
            {
                number = _phoneUtil.Parse(phoneNumber, NormalizeRegion(regionCode));
            }
            catch (NumberParseException)
            {
                return;
            }

            phoneNumberInfo.IsValid = _phoneUtil.IsValidNumber(number);
            phoneNumberInfo.IsPossibleNumber = _phoneUtil.IsPossibleNumber(number);
            phoneNumberInfo.PhoneNumberType = _phoneUtil.GetNumberType(number).ToString();
            phoneNumberInfo.CountryCode = number.CountryCode;
            phoneNumberInfo.DetectedRegionCode = _phoneUtil.GetRegionCodeForNumber(number) ?? "";

            try
            {
                phoneNumberFormats.International = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
                phoneNumberFormats.National = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.NATIONAL);
                phoneNumberFormats.E164 = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164);
                phoneNumberFormats.RFC3966 = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.RFC3966);
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
            out PhoneNumberFormats phoneNumberFormats)
        {
            if (phoneNumber is null) throw new ArgumentNullException(nameof(phoneNumber));
            regionCode ??= "";

            success = false;
            errorMessage = "";
            phoneNumberFormats = new PhoneNumberFormats
            {
                International = "",
                National = "",
                E164 = "",
                RFC3966 = ""
            };

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
            phoneNumberFormats.International = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.INTERNATIONAL);
            phoneNumberFormats.National = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.NATIONAL);
            phoneNumberFormats.E164 = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.E164);
            phoneNumberFormats.RFC3966 = _phoneUtil.Format(number, PhoneNumbers.PhoneNumberFormat.RFC3966);
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
