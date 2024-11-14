ARG ASPNET_IMAGE_TAG=8.0-bookworm-slim-amd64
ARG NODEJS_IMAGE_TAG=22-bullseye-slim
ARG DOTNET_SDK=8.0

# Stage 1 - Build frontend assets
FROM node:${NODEJS_IMAGE_TAG} AS assets
COPY . /repo
WORKDIR /repo/DfE.FindInformationAcademiesTrusts
RUN npm install
RUN npm run build

# Stage 2 - Build and publish dotnet application
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK} AS publish
COPY --from=assets /repo /repo
WORKDIR /repo
RUN dotnet restore DfE.FindInformationAcademiesTrusts
RUN dotnet build DfE.FindInformationAcademiesTrusts -c Release
RUN dotnet publish DfE.FindInformationAcademiesTrusts -c Release -o /app --no-build

COPY ./DfE.FindInformationAcademiesTrusts.Data.FiatDb/Migrations/FiatDbMigrationScript.sql /app/sql/FiatDbMigrationScript.sql
COPY ./docker/web-docker-entrypoint.sh /app/docker-entrypoint.sh

# Stage 3 - Put into Docker container that will actually be run
FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG} AS final
LABEL org.opencontainers.image.source=https://github.com/DFE-Digital/find-information-about-academies-and-trusts
COPY --from=publish /app /app
WORKDIR /app
RUN chmod +x ./docker-entrypoint.sh

EXPOSE 8080/tcp

# Stage 4 - Install SQL tools to allow migrations to be run
RUN apt-get update
RUN apt-get install unixodbc curl gnupg jq -y
RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
RUN curl https://packages.microsoft.com/keys/microsoft.asc | tee /etc/apt/trusted.gpg.d/microsoft.asc
RUN curl https://packages.microsoft.com/config/debian/12/prod.list | tee /etc/apt/sources.list.d/mssql-release.list
RUN apt-get update
RUN ACCEPT_EULA=Y apt-get install msodbcsql18 mssql-tools18 -y
