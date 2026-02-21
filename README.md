# OutSystems.Extension.PhoneNumberValidator

OutSystems ODC External Library wrapper for Google's [libphonenumber](https://github.com/google/libphonenumber) (C# port: [libphonenumber-csharp](https://github.com/twcclegg/libphonenumber-csharp) v9.0.24). Provides phone number validation, formatting, type detection, region detection, and number matching for international phone numbers.

## Actions

### PhoneNumberValidate

Parses and validates a phone number. Returns whether it is valid, its type, and formatted representations.

| Direction | Parameter | Type | Description |
|-----------|-----------|------|-------------|
| Input | phoneNumber | Text | The phone number to validate. Accepts international (`+1 650-253-0000`) or national (`(650) 253-0000`) format. |
| Input | regionCode | Text | ISO 3166-1 alpha-2 code (e.g., `US`, `PT`, `GB`). Required for national format. Can be empty for `+` prefixed numbers. |
| Output | isValid | Boolean | `True` if the phone number is valid. |
| Output | isPossibleNumber | Boolean | `True` if the number could possibly be valid (less strict). Useful for partial input validation. |
| Output | phoneNumberType | Text | `MOBILE`, `FIXED_LINE`, `FIXED_LINE_OR_MOBILE`, `TOLL_FREE`, `PREMIUM_RATE`, `SHARED_COST`, `VOIP`, `PERSONAL_NUMBER`, `PAGER`, `UAN`, `VOICEMAIL`, or `UNKNOWN`. |
| Output | formattedInternational | Text | e.g., `+1 650-253-0000` |
| Output | formattedNational | Text | e.g., `(650) 253-0000` |
| Output | formattedE164 | Text | e.g., `+16502530000` — recommended format for storage. |
| Output | countryCode | Integer | Country calling code (e.g., `1` for US/CA, `351` for PT, `44` for GB). `0` if parsing fails. |
| Output | detectedRegionCode | Text | Detected ISO 3166-1 alpha-2 code (e.g., `US`, `PT`). May differ from input regionCode. |

### PhoneNumberFormat

Formats a phone number into all standard representations.

| Direction | Parameter | Type | Description |
|-----------|-----------|------|-------------|
| Input | phoneNumber | Text | The phone number to format. |
| Input | regionCode | Text | ISO 3166-1 alpha-2 code. Required for national format. |
| Output | success | Boolean | `True` if the number was parsed and formatted successfully. |
| Output | errorMessage | Text | Error details on failure; empty on success. |
| Output | formattedInternational | Text | e.g., `+1 650-253-0000` |
| Output | formattedNational | Text | e.g., `(650) 253-0000` |
| Output | formattedE164 | Text | e.g., `+16502530000` |
| Output | formattedRFC3966 | Text | e.g., `tel:+1-650-253-0000` — useful for `tel:` hyperlinks. |

### PhoneNumberMatch

Compares two phone numbers to determine if they refer to the same line.

| Direction | Parameter | Type | Description |
|-----------|-----------|------|-------------|
| Input | phoneNumber1 | Text | First phone number. |
| Input | phoneNumber2 | Text | Second phone number. |
| Input | regionCode | Text | Default region for parsing numbers in national format. |
| Output | isMatch | Boolean | `True` if the numbers match (`EXACT_MATCH` or `NSN_MATCH`). |
| Output | matchType | Text | `EXACT_MATCH`, `NSN_MATCH`, `SHORT_NSN_MATCH`, `NO_MATCH`, or `NOT_A_NUMBER`. |
| Output | errorMessage | Text | Indicates which phone number failed to parse; empty when both are parseable. |

### PhoneNumberGetRegion

Detects the region (country) and country calling code for a phone number.

| Direction | Parameter | Type | Description |
|-----------|-----------|------|-------------|
| Input | phoneNumber | Text | The phone number to analyze. Best results with `+` prefix. |
| Input | regionCode | Text | Default region for parsing numbers in national format. |
| Output | success | Boolean | `True` if the number was parsed successfully. |
| Output | errorMessage | Text | Error details on failure; empty on success. |
| Output | detectedRegionCode | Text | Detected ISO 3166-1 alpha-2 code (e.g., `US`, `PT`, `GB`). |
| Output | countryCode | Integer | Country calling code (e.g., `1`, `351`, `44`). |

## Usage Notes

- **regionCode** is an ISO 3166-1 alpha-2 code (e.g., `US`, `PT`, `GB`). It is case-insensitive and whitespace-tolerant — `us`, ` PT ` both work.
- Provide regionCode when the phone number is in **national format** (no `+` prefix). When the number is in **international format** (e.g., `+351912345678`), regionCode can be left empty.
- Store phone numbers in **E.164 format** (e.g., `+16502530000`) for best interoperability across systems.
- Invalid or unparseable phone numbers return `isValid = False` / `success = False` with default output values — they do **not** throw exceptions. Only a `null` phone number input throws an exception.

## Technical Details

- **Library:** [libphonenumber-csharp](https://github.com/twcclegg/libphonenumber-csharp) v9.0.24
- **Target:** .NET 8.0, linux-x64 (framework-dependent)
- **ODC SDK:** OutSystems.ExternalLibraries.SDK v1.5.0
- **License:** BSD-3-Clause
