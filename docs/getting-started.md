# Getting started

Use this documentation to configure your local development environment.

- [Get it working (without Docker)](#get-it-working-without-docker)
  - [Prerequisites](#prerequisites)
  - [Configure local user secrets](#configure-local-user-secrets)
  - [Build and watch frontend assets](#build-and-watch-frontend-assets)
  - [Build and run dotnet project](#build-and-run-dotnet-project)
- [Get it working (with Docker)](#get-it-working-with-docker)
  - [Run the fake database for local development](#run-the-fake-database-for-local-development)

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

```bash
dotnet user-secrets set "ConnectionStrings:AcademiesDb" "Server=localhost;User Id=sa;Password=mySuperStrong_pa55word!!!;TrustServerCertificate=true"
```

You can then run the application as normal.
