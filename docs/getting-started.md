# Getting started

Use this documentation to configure your local development environment.

- [Get it working (without Docker)](#get-it-working-without-docker)
  - [Prerequisites](#prerequisites)
  - [Configure local user secrets](#configure-local-user-secrets)
  - [Build and watch frontend assets](#build-and-watch-frontend-assets)
  - [Build and run dotnet project](#build-and-run-dotnet-project)
- [Get it working (with Docker)](#get-it-working-with-docker)
  - [Known issues with Docker](./docker-issues.md)

## Get it working (without Docker)

### Prerequisites

- .NET 7 SDK
- node
- npm

### Configure local user secrets

Use the dotnet user secrets tool to set local secrets, any missing required secrets will cause the application to fail at startup with an exception detailing which secrets are missing.

```bash
cd DfE.FindInformationAcademiesTrusts

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

> **Warning**
> If you have trouble getting the docker compose files to work, see [known issues with Docker](./docker-issues.md).

Before running the application in Docker:

- ensure the Docker engine is running
- navigate to the `docker` directory

Then you can either run:

1. Just the application

    The `docker-compose.yml` is used by the GitHub actions pipeline and is as close to the real application as you can get on a local machine.

    First copy the `.env.example` file, save as `.env` and populate the application secrets within. Then build and start the container:

    ```bash
    docker compose -f docker-compose.yml up -d --build     # build and run the application alone
    ```

1. Run your local application against the fake database

    The `docker-compose.db.yml` file is used for running our Cypress tests in an isolated environment against a fake database — which you can also use for local development.
    
    ```
    # build and run the application and a local db containing fake data -> most useful for tests
    docker compose -f ~/docker/docker-compose.db.yml up -d --build  
    ```

    You can then run the application as normal (using your IDE or `dotnet run`)

Once you are done, ensure that you stop the container(s)!

```bash
docker compose -f docker-compose.db.yml down -v
```
