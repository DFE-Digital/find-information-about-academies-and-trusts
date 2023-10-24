# Getting started

Use this documentation to configure your local development environment.

- [Get it working (without Docker)](#get-it-working-without-docker)
  - [Prerequisites](#prerequisites)
  - [Configure local user secrets](#configure-local-user-secrets)
  - [Build and watch frontend assets](#build-and-watch-frontend-assets)
  - [Build and run dotnet project](#build-and-run-dotnet-project)
- [Get it working (with Docker)](#get-it-working-with-docker)
  - [Run the fake database for local development](#run-the-fake-database-for-local-development)
- [Run tests locally](#run-tests-locally)
  - [Unit tests](#unit-tests)
  - [Accessibility and UI tests](#accessibility-and-ui-tests)
  - [Integration and Deployment tests](#integration-and-deployment-tests)
- [Supercharge your dev environment](#supercharge-your-dev-environment)
  - [Set up continuous testing](#set-up-continuous-testing)
  - [Analyse test coverage](#analyse-test-coverage)
  - [Configure linting and code cleanup](#configure-linting-and-code-cleanup)

## Get it working (without Docker)

### Prerequisites

- .NET 7 SDK
- node
- npm

### Configure local user secrets

Use the dotnet user secrets tool to set local secrets, any missing required secrets will cause the application to fail at startup with an exception detailing which secrets are missing.

```bash
cd DfE.FindInformationAcademiesTrusts
dotnet user-secrets set "AcademiesApi:Endpoint" "[secret goes here]"
dotnet user-secrets set "AcademiesApi:Key" "[secret goes here]"
dotnet user-secrets set "AzureAd:ClientID" "[secret goes here]"
dotnet user-secrets set "AzureAd:ClientSecret" "[secret goes here]"
dotnet user-secrets set "AzureAd:TenantID" "[secret goes here]"
dotnet user-secrets set "ConnectionStrings:AcademiesDb" "[secret goes here]"
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

- Install project tools by running `dotnet tool restore` from project directory.
- Ensure you have [built the frontend assets](#build-and-watch-frontend-assets) before building the dotnet project.
- Run/debug project as normal in your chosen IDE

Note that the default `ASPNETCORE_ENVIRONMENT` for local development is `"LocalDevelopment"` (configured in `launchsettings.json`)

## Get it working (with Docker)

Before running the application in Docker:

- ensure the Docker engine is running.
- navigate to the `docker` directory
- copy the `.env.example` file, save as `.env` and populate the application secrets within

If you are running on Apple M1 chip the SQL Server image may not work. This can be fixed by:

- Docker Settings > General: [X] Use virtualization framework and
- Docker Settings > Features in Development: [X] Use Rosetta...

There are two Docker compose files in the `docker` directory:

```bash
docker compose -f docker-compose.yml up -d --build     # build and run the application alone
docker compose -f docker-compose.ci.yml up -d --build  # build and run the application and the mock api together -> most useful for tests
```

Once you are done, ensure that you stop the container(s)!

```bash
docker compose -f docker-compose.yml down
docker compose -f docker-compose.ci.yml down
```

### Run the fake database for local development

The `docker-compose.ci.yml` file is used for running our Playwright tests in an isolated environment against a fake database—which you can also use for local development. Follow the steps above to run the docker compose file, and then update your dotnet user secrets to point to the database in the docker container:

```
dotnet user-secrets set "ConnectionStrings:AcademiesDb" "Server=localhost;User Id=sa;Password=mySuperStrong_pa55word!!!;TrustServerCertificate=true"
```

You can then run the application as normal.

## Run tests locally

### Unit tests

All .NET code is [unit tested](./test-approach.md) where possible.

You can run unit tests using your preferred IDE, or open a terminal in the project directory and run:

```bash
dotnet test
```

### Accessibility and UI tests

Accessibility and UI tests are written using [Playwright](https://playwright.dev/), with external dependencies (e.g. APIs) mocked using a [fake database](#run-the-fake-database-for-local-development). You can run the tests against a docker instance, or run the application locally.

1. Create or update the file `tests/playwright/.env`

**If running tests against the docker instance of the app:**
```dotenv
PLAYWRIGHT_BASEURL="http://localhost/"
```

**If running tests in your local environment**
```dotenv
PLAYWRIGHT_BASEURL="http://localhost:{port} # e.g. http://localhost:5163
```

For your local environment you will also need to change your database connection string (dotnet user secrets) to point to the docker instance:

```
dotnet user-secrets set "ConnectionStrings:AcademiesDb" "Server=localhost;User Id=sa;Password=mySuperStrong_pa55word!!!;TrustServerCertificate=true"
```

2. Open a terminal in your repository and run:

```bash
cd tests/playwright

# install dependencies
npm install
npx playwright install

# run docker image with an application rebuild
# you will need to run this even if you are running tests locally - to create the fake database for tests
npm run docker:start 

# run tests 
npm run test:ci

# OR
npm run test:ui # run only ui tests
npm run test:ui:trace # run only ui tests in chromium, with trace mode
npm run test:a11y # run only accessibility tests

npx playwright test {folder-name}/* # run a particular set of tests
npx playright test --headed # run in headed mode
npx playwright test --trace=on # get a time machine attached to each test result in the report

# remove docker image when done
npm run docker:stop
```

If you are running on Apple M1 chip the SQL Server image may not work in Docker, see [Get it working with Docker](#get-it-working-with-docker) for how to fix this.

For more information on running and debugging Playwright tests it is worth familiarising yourself with the Playwright docs on [debugging](https://playwright.dev/docs/debug) and [command line flags](https://playwright.dev/docs/test-cli).

### Integration and Deployment tests

[Integration and Deployment tests](./test-approach.md) are also run in [Playwright](https://playwright.dev/), but do not have dependencies mocked. If you wish to run these locally you will need to run the app locally before running your tests. Alternatively you can run them against deployed dev or test environments.

1. Run the .NET application using your preferred IDE or by running `dotnet run`
2. Configure/create `tests/playwright/.env` and add the following:

```dotenv
PLAYWRIGHT_BASEURL="http://localhost:5163" # This should be the localhost port the application is running on, or the deployed application url you wish to test against
TEST_USER_ACCOUNT_NAME="<insert here>" # Include the domain name
TEST_USER_ACCOUNT_PASSWORD="<insert here>" # 
```

3. Open a terminal to run your tests

```shell
 cd DfE.FindInformationAcademiesTrusts.UnitTests/playwright
 npm install
 npm run test:integration
```

## Supercharge your dev environment

We recommend using Rider or Visual Studio with ReSharper.

### Set up continuous testing

We recommend setting Rider to run unit tests on save, for fast feedback on changes.

- Go to Settings -> Plugins and check that `dotCover` is enabled
- Go to Settings -> Build, Execution, Deployment -> Unit Testing -> Continuous Testing and select 'Automatically start tests in continuous testing sessions on **Save**'
- Go to or open a Unit Tests session (Tests -> Create New Session), open the 'Continuous testing modes' menu and select 'Run all tests'

### Analyse test coverage

This project uses a [mutation score](https://stryker-mutator.io/docs/) to analyse effective test coverage when opening up a new pull request.
Stryker.Net is included in our dotnet-tools manifest for checking mutation score locally. 

Install tools by running `dotnet tool restore`.

Run the following to run and open a Stryker report:

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
