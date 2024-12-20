# Set the major version of dotnet
ARG DOTNET_VERSION=8.0
# Set the major version of nodejs
ARG NODEJS_VERSION_MAJOR=22

# Build frontend assets using node js
FROM "node:${NODEJS_VERSION_MAJOR}-bullseye-slim" AS assets
WORKDIR /app
COPY ./DfE.FindInformationAcademiesTrusts/ /app
RUN npm install
RUN npm run build

# Build the app using the dotnet SDK
FROM "mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0" AS build
WORKDIR /build
COPY . /build
RUN ["dotnet", "restore", "DfE.FindInformationAcademiesTrusts"]
RUN ["dotnet", "build", "DfE.FindInformationAcademiesTrusts", "--no-restore", "-c", "Release"]
RUN ["dotnet", "publish", "DfE.FindInformationAcademiesTrusts", "--no-build", "-o", "/app"]

# Install SQL tools to allow migrations to be run
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS base
RUN curl "https://packages.microsoft.com/config/rhel/9/prod.repo" | tee /etc/yum.repos.d/mssql-release.repo
ENV ACCEPT_EULA=Y
RUN ["tdnf", "update"]
RUN ["tdnf", "install", "-y", "mssql-tools18"]
RUN ["tdnf", "clean", "all"]

# Build a runtime environment
FROM base AS runtime
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/find-information-about-academies-and-trusts"
COPY --from=build /app /app
COPY --from=assets /app/wwwroot /app/wwwroot
COPY ./DfE.FindInformationAcademiesTrusts.Data.FiatDb/Migrations/FiatDbMigrationScript.sql /app/sql/FiatDbMigrationScript.sql
COPY ./docker/web-docker-entrypoint.sh /app/docker-entrypoint.sh
RUN ["chmod", "+x", "/app/docker-entrypoint.sh"]
RUN ["touch", "/app/sql/FiatDbMigrationScriptOutput.txt"]
RUN chown "$APP_UID" "/app/sql" -R
USER $APP_UID
