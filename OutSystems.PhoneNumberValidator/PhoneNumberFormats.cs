using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.PhoneNumberValidator
{
    [OSStructure(Description = "Phone number formatted in standard representations.")]
    public struct PhoneNumberFormats
    {
        [OSStructureField(Description = "Phone number in international format (e.g., '+1 650-253-0000').")]
        public string International;

        [OSStructureField(Description = "Phone number in national format (e.g., '(650) 253-0000').")]
        public string National;

        [OSStructureField(Description = "Phone number in E.164 format (e.g., '+16502530000'). This is the recommended format for storage.")]
        public string E164;

        [OSStructureField(Description = "Phone number in RFC3966 format (e.g., 'tel:+1-650-253-0000'). Useful for 'tel:' hyperlinks.")]
        public string RFC3966;
    }
}
