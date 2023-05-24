# Getting started

Use this documentation to configure your local development environment.

- [Get it working](#get-it-working)
  - [Prerequisites](#prerequisites)
  - [Build frontend assets](#build-frontend-assets)
  - [Build and run dotnet project](#build-and-run-dotnet-project)
  - [Run in Docker (optional)](#run-in-docker-optional)
- [Supercharge your dev environment](#supercharge-your-dev-environment)
  - [Configure linting and code cleanup](#configure-linting-and-code-cleanup)

## Get it working

### Prerequisites

You will need:

- .NET 6 sdk
- npm

Recommended:

- Rider or Visual Studio with ReSharper
- Docker

### Build frontend assets

Navigate to the project file working directory then:

```bash
npm install
npm run build
```

### Build and run dotnet project

- Ensure you have [built the frontend assets](#build-frontend-assets) before building the dotnet project.
- Run/debug project as normal in your chosen IDE

Beware that `ASPNETCORE_ENVIRONMENT` for local development is set to "LocalDevelopment" (in `launchsettings.json`)

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
- Configure C# linting
  - Go to Tools -> Actions on Save
  - Tick `Reformat and Cleanup Code`
  - Ensure that Profile is set to `DfE.FindInformationAcademiesTrusts`

If using Rider or Resharper then the `DfE.FindInformationAcademiesTrusts.sln.DotSettings` should be automatically identified and used for manual C# code cleanup.
