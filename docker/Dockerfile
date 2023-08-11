ARG ASPNET_IMAGE_TAG=7.0-bullseye-slim
ARG NODEJS_IMAGE_TAG=18-bullseye-slim
ARG DOTNET_SDK=7.0

# Stage 1 - Build frontend assets
FROM node:${NODEJS_IMAGE_TAG} as assets
COPY . .
WORKDIR /DfE.FindInformationAcademiesTrusts
RUN npm install
RUN npm run build

# Stage 2 - Build and publish dotnet application
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK} AS publish
COPY --from=assets . .
RUN dotnet restore DfE.FindInformationAcademiesTrusts
RUN dotnet build DfE.FindInformationAcademiesTrusts -c Release
RUN dotnet publish DfE.FindInformationAcademiesTrusts -c Release -o /app --no-build

COPY ./docker/web-docker-entrypoint.sh /app/docker-entrypoint.sh

# Stage 3 - Put into Docker container that will actually be run
FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG} AS final
COPY --from=publish /app /app
WORKDIR /app
RUN chmod +x ./docker-entrypoint.sh
EXPOSE 80/tcp
