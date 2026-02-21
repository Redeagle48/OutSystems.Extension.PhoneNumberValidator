using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.PhoneNumberValidator
{
    [OSStructure(Description = "Phone number validation and identification details.")]
    public struct PhoneNumberInfo
    {
        [OSStructureField(Description = "True if the phone number is valid; otherwise false.")]
        public bool IsValid;

        [OSStructureField(Description = "True if the phone number could possibly be valid (less strict than IsValid). Useful for partial input validation.")]
        public bool IsPossibleNumber;

        [OSStructureField(Description = "The type of phone number: MOBILE, FIXED_LINE, FIXED_LINE_OR_MOBILE, TOLL_FREE, PREMIUM_RATE, SHARED_COST, VOIP, PERSONAL_NUMBER, PAGER, UAN, VOICEMAIL, or UNKNOWN.")]
        public string PhoneNumberType;

        [OSStructureField(Description = "The country calling code (e.g., 1 for US/CA, 351 for PT, 44 for GB). Returns 0 if parsing fails.")]
        public int CountryCode;

        [OSStructureField(Description = "The detected ISO 3166-1 alpha-2 region code for the number (e.g., 'US', 'PT'). May differ from input regionCode. Empty if parsing fails.")]
        public string DetectedRegionCode;
    }
}
