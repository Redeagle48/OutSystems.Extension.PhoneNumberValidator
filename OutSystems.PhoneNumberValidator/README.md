# OutSystems.PhoneNumberValidator

OutSystems ODC External Library for validating, formatting, and comparing international phone numbers using Google's [libphonenumber](https://github.com/google/libphonenumber) (C# port: [libphonenumber-csharp](https://github.com/twcclegg/libphonenumber-csharp) v9.0.24).

## Actions

### PhoneNumberValidate
Parses and validates a phone number. Returns whether it is valid, its type (`MOBILE`, `FIXED_LINE`, `TOLL_FREE`, etc.), formatted representations (international, national, E.164), country calling code, and detected region.

### PhoneNumberFormat
Formats a phone number into International, National, E.164, and RFC3966 formats. Returns `success = False` with an error message if the number cannot be parsed.

### PhoneNumberMatch
Compares two phone numbers and determines if they refer to the same line. Returns match type (`EXACT_MATCH`, `NSN_MATCH`, `SHORT_NSN_MATCH`, `NO_MATCH`, `NOT_A_NUMBER`) and indicates which input failed to parse if applicable.

### PhoneNumberGetRegion
Detects the region (country) and country calling code for a phone number.

## Usage Notes
- Provide `regionCode` (ISO 3166-1 alpha-2, e.g., `US`, `PT`, `GB`) when the phone number is in national format (without the `+` international prefix). The value is case-insensitive and whitespace-tolerant.
- When the phone number is in international format (e.g., `+351912345678`), regionCode can be left empty.
- Store phone numbers in E.164 format (e.g., `+16502530000`) for best interoperability.
- Invalid inputs return `isValid = False` / `success = False` — they do not throw exceptions.
