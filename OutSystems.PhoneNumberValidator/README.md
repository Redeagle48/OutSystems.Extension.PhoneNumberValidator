# OutSystems.PhoneNumberValidator

OutSystems ODC External Library for validating, formatting, and comparing international phone numbers using Google's [libphonenumber](https://github.com/google/libphonenumber) (C# port: [libphonenumber-csharp](https://github.com/twcclegg/libphonenumber-csharp) v9.0.24).

## Structures

- **PhoneNumberInfo** — Validation and identification: IsValid, IsPossibleNumber, PhoneNumberType, CountryCode, DetectedRegionCode.
- **PhoneNumberFormats** — Formatted representations: International, National, E164, RFC3966.

## Actions

### PhoneNumberValidate
Parses and validates a phone number. Returns a `PhoneNumberInfo` (validity, type, country code, region) and `PhoneNumberFormats` (all formatted representations).

### PhoneNumberFormat
Formats a phone number into all standard representations. Returns `success = False` with an error message if the number cannot be parsed, plus a `PhoneNumberFormats` structure.

### PhoneNumberMatch
Compares two phone numbers and determines if they refer to the same line. Returns match type (`EXACT_MATCH`, `NSN_MATCH`, `SHORT_NSN_MATCH`, `NO_MATCH`, `NOT_A_NUMBER`) and indicates which input failed to parse if applicable.

### PhoneNumberGetRegion
Detects the region (country) and country calling code for a phone number.

## Usage Notes
- Provide `regionCode` (ISO 3166-1 alpha-2, e.g., `US`, `PT`, `GB`) when the phone number is in national format (without the `+` international prefix). The value is case-insensitive and whitespace-tolerant.
- When the phone number is in international format (e.g., `+351912345678`), regionCode can be left empty.
- Store phone numbers in E.164 format (e.g., `+16502530000`) for best interoperability.
- Invalid inputs return `IsValid = False` / `success = False` — they do not throw exceptions.
