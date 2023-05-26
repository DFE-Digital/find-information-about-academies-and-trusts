# Getting started

Use this documentation to configure your local development environment.

- [Get it working](#get-it-working)
  - [Prerequisites](#prerequisites)
  - [Configure local user secrets](#configure-local-user-secrets)
  - [Build frontend assets](#build-frontend-assets)
  - [Build and run dotnet project](#build-and-run-dotnet-project)
  - [Run in Docker (optional)](#run-in-docker-optional)
- [Supercharge your dev environment](#supercharge-your-dev-environment)
  - [Configure linting and code cleanup](#configure-linting-and-code-cleanup)

## Get it working

### Prerequisites

You will need:

- .NET 6 SDK
- npm

Recommended:

- Rider or Visual Studio with ReSharper
- Docker

### Configure local user secrets

Use the dotnet user secrets tool to set local secrets, any missing required secrets will cause the application to fail at startup with an exception detailing which secrets are missing.

```bash
dotnet user-secrets set "AcademiesApi:Endpoint" "[endpoint goes here]"
dotnet user-secrets set "AcademiesApi:Key" "[key goes here]"
```

### Build frontend assets

The frontend assets must be built before the .NET project. The assets are built into the `wwwroot` folder.

```bash
cd DfE.FindInformationAcademiesTrusts
npm install
npm run build
```

### Build and run dotnet project

- Ensure you have [built the frontend assets](#build-frontend-assets) before building the dotnet project.
- Run/debug project as normal in your chosen IDE

Note that the default `ASPNETCORE_ENVIRONMENT` for local development is `"LocalDevelopment"` (configured in `launchsettings.json`)

### Run in Docker (optional)

Under most circumstances you won't need to run the application in Docker locally. If you do need to then:

- copy the `.env.example` file, save as `.env` and populate the secrets within
- run `docker-compose up`

## Supercharge your dev environment

### Configure linting and code cleanup

We recommend setting Rider to clean code on save.

- Open settings
- Configure JavaScript linting
  - Go to Languages & Frameworks -> JavaScript -> Code Quality Tools -> ESLint
  - Select `Manual ESLint configuration`
  - In the dropdown for ESLint package, press the down arrow and select `.../DfE.FindInformationAcademiesTrusts/node_modules/standard`. If nothing is here then ensure you have done an `npm install` in the project directory
  - Tick `Run ESLint --fix on save`
  - Go to Editor > Code style > JavaScript
  - In the top right click `Set from...` and select `JavaScript Standard Style`
- Configure CSS linting
  - Go to Languages & Frameworks -> Style Sheets -> Stylelint
  - Select `Enable`
  - In the dropdown for Stylelint package, press the down arrow and select `.../DfE.FindInformationAcademiesTrusts/node_modules/styleint`. If nothing is here then ensure you have done an `npm install` in the project directory
  - In `Run for files` enter: "{\*_/_,\*}.{css,scss}"
- Configure C# linting
  - Go to Tools -> Actions on Save
  - Tick `Reformat and Cleanup Code`
  - Ensure that Profile is set to `DfE.FindInformationAcademiesTrusts`

If using Rider or Resharper then the `DfE.FindInformationAcademiesTrusts.sln.DotSettings` should be automatically identified and used for manual C# code cleanup.

You can also run linting on JavaScript, CSS and SCSS files using the command line:

```bash
cd DfE.FindInformationAcademiesTrusts
npm run lint ## for a list of issues
npm run lint:fix ## to scan and fix issues
```
