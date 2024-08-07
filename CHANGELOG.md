# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/). To see an example from a mature product in the program [see the Complete products changelog that follows the same methodology](https://github.com/DFE-Digital/dfe-complete-conversions-transfers-and-changes/blob/main/CHANGELOG.md).

## [Unreleased][unreleased]

### Added

- Feature flags added to allow for selectively showing features in different environments

## [Release-3][release-3] (production-2024-08-09.2694)

### Changed

- Accessibility statement has been updated to reflect the recent accessibility audit.
- Trust details page load time decreased.
- Implemented service and repository design patterns for retrieving data from AcademiesDb.
- Data source information cached to increase performance of all Trust pages.
- Trust summary information (UID, name, type and number of academies) cached to improve performance of all Trust pages.
- Updated hardcoded free schools meals data to be updated to 23/24, resolving a bug relating to new Local Authorities within that data

## [Release-2][release-2] (production-2024-07-29.2601)

### Changed

- Filter so only the open Gias Groups (Trusts) are shown across the application
- Updated Feedback link to reflect new feedback form for the product

## [Release-1][release-1] (production-2024-07-18.2517)

### Changed 

- Migrated the application to LTS .NET 8. (FIAT was on .NET 7 which Microsoft ended support for in May).
- Updated text on "404 - Not Found" page to adhere to DfE design pattern and added the not found url to the support email template.

[unreleased]:
  https://github.com/DFE-Digital/dfe-complete-conversions-transfers-and-changes/compare/production-2024-08-09.2694...HEAD
[release-1]:
  https://github.com/DFE-Digital/find-information-about-academies-and-trusts/releases/tag/production-2024-07-18.2517
[release-2]:
  https://github.com/DFE-Digital/find-information-about-academies-and-trusts/releases/tag/production-2024-07-29.2601
[release-3]:
  https://github.com/DFE-Digital/find-information-about-academies-and-trusts/releases/tag/production-2024-08-09.2694