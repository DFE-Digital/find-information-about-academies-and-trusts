# Getting started

Use this documentation to configure your local development environment.

- [Get it working](#get-it-working)
  - [Prerequisites](#prerequisites)
  - [Configure local user secrets](#configure-local-user-secrets)
  - [Build and watch frontend assets](#build-and-watch-frontend-assets)
  - [Build and run dotnet project](#build-and-run-dotnet-project)
  - [Run in Docker (optional)](#run-in-docker-optional)
- [Run tests](#run-tests)
  - [Unit tests](#unit-tests)
  - [Accessibility and UI tests](#accessibility-and-ui-tests)
  - [Accessing tests reports in the pipeline](#accessing-test-reports-in-the-pipeline)
    - [Viewing Playwright traces](#viewing-playwright-traces)
- [Supercharge your dev environment](#supercharge-your-dev-environment)
  - [Set up continuous testing](#set-up-continuous-testing)
  - [Analyse test coverage](#analyse-test-coverage)
  - [Configure linting and code cleanup](#configure-linting-and-code-cleanup)

## Get it working

### Prerequisites

You will need:

- .NET 7 SDK
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

### Build and watch frontend assets

The frontend assets must be built before the .NET project. The assets are built into the `wwwroot` folder.

```bash
cd DfE.FindInformationAcademiesTrusts
npm install
npm run dev
```
The `npm run dev` script will watch for any changes to your sass and JavaScript entry files. Simply refresh the browser window to reflect changes whilst your dotnet project is running.

You will need to stop (`ctrl-c`) and rerun the script if you change any saved images.

### Build and run dotnet project

- Ensure you have [built the frontend assets](#build-and-watch-frontend-assets) before building the dotnet project.
- Run/debug project as normal in your chosen IDE

Note that the default `ASPNETCORE_ENVIRONMENT` for local development is `"LocalDevelopment"` (configured in `launchsettings.json`)

### Run in Docker (optional)

Under most circumstances you won't need to run the application in Docker locally. If you do need to then:

- navigate to the `docker` directory
- copy the `.env.example` file, save as `.env` and populate the secrets within
- run `docker-compose up`

## Run tests

### Unit tests

All .NET code is [unit tested](./test-approach.md) where possible. 

You can run unit tests using your preferred IDE, or open a terminal in the project directory and run:

```bash
dotnet test
```

### Accessibility and UI tests

Accessibility and UI tests are written using [Playwright](https://playwright.dev/), with external dependencies (e.g. APIs) mocked using [WireMock](https://github.com/HBOCodeLabs/wiremock-captain). 
To run these tests locally it is easiest to run your app and the mock API using Docker:

1. Create or update the file `tests/playwright/.env` with
```dotenv
PLAYWRIGHT_BASEURL="http://localhost/"
```
2. Open a terminal in your repository and run:
```bash
cd DfE.FindInformationAcademiesTrusts.UnitTests/playwright

# run docker image with build flag to watch for the latest code changes
docker compose -f ../../docker/docker-compose.ci.yml up -d --build

# install dependencies
npm install

# run tests 
npx playwright test 

# OR
npx playwright test {folder-name}/* # run a particular set of tests
npx playright test --headed # run in headed mode
npx playwright test --trace <mode> # overwrite tracing mode set in config 

# remove docker image when done
docker compose -f ../../docker/docker-compose.ci.yml down

```
For more information on running and debugging Playwright tests it is worth familiarising yourself with the Playwright docs on [debugging](https://playwright.dev/docs/debug) and [command line flags](https://playwright.dev/docs/test-cli).

### Integration tests

[Integration tests](./test-approach.md) are also run in [Playwright](https://playwright.dev/), but do not have dependencies mocked. If you wish to run these locally you will need to run the app locally before running your tests.

1. Run the .NET application using your preferred IDE or by running `dotnet run`
2. Check the localhost port the application is running on and add it `tests/playwright/.env` as `PLAYWRIGHT_BASEURL`

```dotenv
 PLAYWRIGHT_BASEURL="http://localhost:5163"
```

3. Open a terminal to run your tests

```shell
 cd DfE.FindInformationAcademiesTrusts.UnitTests/playwright
 npm install
 npm run test:integration
```

### Accessing test reports in the pipeline

Automated tests are run in the pipeline as a GitHub workflow on opening a pull request to main—as per our [test approach](./test-approach.md). To access reports and traces for tests run in the pipeline:

1. Click on 'details' on the relevant check in the pull request
2. Click on Summary in the left-hand menu
3. Under 'Artifacts' you will find downloadable reports

You can also click on the job to see details of any error messages for failed test runs.

#### **Viewing Playwright traces**

If a Playwright test fails, the test is rerun and a [trace](https://playwright.dev/docs/trace-viewer-intro) (recorded test run) is captured. You will need to use a [trace viewer](https://playwright.dev/docs/trace-viewer) to view traces captured during CI.

To access the trace, download the Playwright Report using the step above, and download the zip file of the trace under 'Retry#1'. We suggest dragging and dropping the downloaded zip file into Playwright's [online trace viewer](https://trace.playwright.dev/), or you can also view it [using the command line](https://trace.playwright.dev/). 

## Supercharge your dev environment

### Set up continuous testing

We recommend setting Rider to run unit tests on save, for fast feedback on changes.

- Go to Settings -> Plugins and check that `dotCover` is enabled
- Go to Settings -> Build, Execution, Deployment -> Unit Testing -> Continuous Testing and select 'Automatically start tests in continuous testing sessions on **Save**'
- Go to or open a Unit Tests session (Tests -> Create New Session), open the 'Continuous testing modes' menu and select 'Run all tests'

### Analyse test coverage

This project uses a [mutation score](https://stryker-mutator.io/docs/) to analyse effective test coverage when opening up a new pull request.
To check these scores locally you will need to [install Stryker.Net](https://stryker-mutator.io/docs/stryker-net/getting-started/).

Once Stryker is installed, run the following to run and open a Stryker report:

```bash
dotnet stryker -o
```

You will be able to find all reports in the `StrykerOutput` folder in your project root.

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
