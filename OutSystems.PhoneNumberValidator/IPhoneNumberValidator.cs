using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.PhoneNumberValidator
{
    /// <summary>
    /// Validates, formats, and compares international phone numbers using Google's
    /// libphonenumber library (C# port v9.0.24).
    /// </summary>
    [OSInterface(
        Description = "Validates, formats, and compares international phone numbers using Google's libphonenumber library. Supports parsing, validation, formatting (E.164, international, national, RFC3966), number type detection, region detection, and number matching.",
        IconResourceName = "OutSystems.PhoneNumberValidator.resources.PhoneNumberValidator_icon.png"
    )]
    public interface IPhoneNumberValidator
    {
        /// <summary>
        /// Parses and validates a phone number, returning its type and formatted representations.
        /// </summary>
        [OSAction(
            Description = "Parses and validates a phone number. Returns whether it is valid, its type (MOBILE, FIXED_LINE, TOLL_FREE, etc.), and formatted representations (international, national, E.164). Provide regionCode (e.g., 'US', 'PT', 'GB') when the number is in national format.",
            IconResourceName = "OutSystems.PhoneNumberValidator.resources.PhoneNumberValidator_icon.png"
        )]
        void PhoneNumberValidate(
            [OSParameterAttribute(Description = "The phone number to validate. Can be in international format (e.g., '+1 650-253-0000') or national format (e.g., '(650) 253-0000'). When in national format, regionCode must be provided.")]
            string phoneNumber,

            [OSParameterAttribute(Description = "ISO 3166-1 alpha-2 region code (e.g., 'US', 'PT', 'GB'). Required when phone number is in national format. Can be empty when phone number includes country code with '+'.")]
            string regionCode,

            [OSParameterAttribute(Description = "True if the phone number is valid; otherwise false.")]
            out bool isValid,

            [OSParameterAttribute(Description = "True if the phone number could possibly be valid (less strict than isValid). Useful for partial input validation.")]
            out bool isPossibleNumber,

            [OSParameterAttribute(Description = "The type of phone number: MOBILE, FIXED_LINE, FIXED_LINE_OR_MOBILE, TOLL_FREE, PREMIUM_RATE, SHARED_COST, VOIP, PERSONAL_NUMBER, PAGER, UAN, VOICEMAIL, or UNKNOWN.")]
            out string phoneNumberType,

            [OSParameterAttribute(Description = "Phone number in international format (e.g., '+1 650-253-0000').")]
            out string formattedInternational,

            [OSParameterAttribute(Description = "Phone number in national format (e.g., '(650) 253-0000').")]
            out string formattedNational,

            [OSParameterAttribute(Description = "Phone number in E.164 format (e.g., '+16502530000'). This is the recommended format for storage.")]
            out string formattedE164,

            [OSParameterAttribute(Description = "The country calling code (e.g., 1 for US/CA, 351 for PT, 44 for GB). Returns 0 if parsing fails.")]
            out int countryCode,

            [OSParameterAttribute(Description = "The detected ISO 3166-1 alpha-2 region code for the number (e.g., 'US', 'PT'). May differ from input regionCode. Empty if parsing fails.")]
            out string detectedRegionCode
        );

        /// <summary>
        /// Formats a phone number in all available formats.
        /// </summary>
        [OSAction(
            Description = "Formats a phone number into international, national, E.164, and RFC3966 formats. Returns success=false with an error message if the number cannot be parsed.",
            IconResourceName = "OutSystems.PhoneNumberValidator.resources.PhoneNumberValidator_icon.png"
        )]
        void PhoneNumberFormat(
            [OSParameterAttribute(Description = "The phone number to format. Can be in international format (e.g., '+1 650-253-0000') or national format.")]
            string phoneNumber,

            [OSParameterAttribute(Description = "ISO 3166-1 alpha-2 region code (e.g., 'US', 'PT', 'GB'). Required when phone number is in national format.")]
            string regionCode,

            [OSParameterAttribute(Description = "True if the phone number was parsed and formatted successfully.")]
            out bool success,

            [OSParameterAttribute(Description = "Error message if parsing failed; empty on success.")]
            out string errorMessage,

            [OSParameterAttribute(Description = "Phone number in international format (e.g., '+1 650-253-0000').")]
            out string formattedInternational,

            [OSParameterAttribute(Description = "Phone number in national format (e.g., '(650) 253-0000').")]
            out string formattedNational,

            [OSParameterAttribute(Description = "Phone number in E.164 format (e.g., '+16502530000').")]
            out string formattedE164,

            [OSParameterAttribute(Description = "Phone number in RFC3966 format (e.g., 'tel:+1-650-253-0000'). Useful for 'tel:' hyperlinks.")]
            out string formattedRFC3966
        );

        /// <summary>
        /// Checks if two phone numbers are the same.
        /// </summary>
        [OSAction(
            Description = "Compares two phone numbers and determines if they match. Returns the match type (EXACT_MATCH, NSN_MATCH, SHORT_NSN_MATCH, NO_MATCH, NOT_A_NUMBER) and a boolean indicating whether the numbers match.",
            IconResourceName = "OutSystems.PhoneNumberValidator.resources.PhoneNumberValidator_icon.png"
        )]
        void PhoneNumberMatch(
            [OSParameterAttribute(Description = "The first phone number to compare.")]
            string phoneNumber1,

            [OSParameterAttribute(Description = "The second phone number to compare.")]
            string phoneNumber2,

            [OSParameterAttribute(Description = "ISO 3166-1 alpha-2 region code (e.g., 'US', 'PT'). Used as default region when parsing numbers in national format.")]
            string regionCode,

            [OSParameterAttribute(Description = "True if the two phone numbers match (EXACT_MATCH or NSN_MATCH).")]
            out bool isMatch,

            [OSParameterAttribute(Description = "The match type: EXACT_MATCH (numbers are identical), NSN_MATCH (national significant numbers match), SHORT_NSN_MATCH (one number is a shorter version of the other), NO_MATCH (numbers do not match), or NOT_A_NUMBER (one or both inputs cannot be parsed).")]
            out string matchType,

            [OSParameterAttribute(Description = "Error message indicating which phone number failed to parse; empty when both numbers are parseable.")]
            out string errorMessage
        );

        /// <summary>
        /// Gets the region and country code information for a phone number.
        /// </summary>
        [OSAction(
            Description = "Detects the region (country) and country calling code for a phone number. The phone number should include the country code ('+' prefix) for best results.",
            IconResourceName = "OutSystems.PhoneNumberValidator.resources.PhoneNumberValidator_icon.png"
        )]
        void PhoneNumberGetRegion(
            [OSParameterAttribute(Description = "The phone number to analyze. Should ideally be in international format with '+' prefix.")]
            string phoneNumber,

            [OSParameterAttribute(Description = "ISO 3166-1 alpha-2 region code (e.g., 'US', 'PT'). Used as default when phone number is in national format.")]
            string regionCode,

            [OSParameterAttribute(Description = "True if the phone number was parsed successfully.")]
            out bool success,

            [OSParameterAttribute(Description = "Error message if parsing failed; empty on success.")]
            out string errorMessage,

            [OSParameterAttribute(Description = "The detected ISO 3166-1 alpha-2 region code (e.g., 'US', 'PT', 'GB').")]
            out string detectedRegionCode,

            [OSParameterAttribute(Description = "The country calling code (e.g., 1 for US/CA, 351 for PT, 44 for GB).")]
            out int countryCode
        );
    }
}
