ARG ASPNET_IMAGE_TAG=8.0-bookworm-slim-amd64
ARG NODEJS_IMAGE_TAG=18-bullseye-slim
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

COPY ./docker/web-docker-entrypoint.sh /app/docker-entrypoint.sh

# Stage 3 - Put into Docker container that will actually be run
FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG} AS final
LABEL org.opencontainers.image.source=https://github.com/DFE-Digital/find-information-about-academies-and-trusts
COPY --from=publish /app /app
WORKDIR /app
RUN chmod +x ./docker-entrypoint.sh

EXPOSE 8080/tcp
