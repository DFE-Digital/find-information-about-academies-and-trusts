ARG ASPNET_IMAGE_TAG=6.0.9-bullseye-slim

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish

WORKDIR /build
ENV DEBIAN_FRONTEND=noninteractive
COPY . .

RUN dotnet restore DfE.FindInformationAcademiesTrusts
RUN dotnet build DfE.FindInformationAcademiesTrusts -c Release
RUN dotnet publish DfE.FindInformationAcademiesTrusts -c Release -o /app --no-build

COPY ./script/web-docker-entrypoint.sh /app/docker-entrypoint.sh

FROM "mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG}" AS final

COPY --from=publish /app /app
WORKDIR /app
RUN chmod +x ./docker-entrypoint.sh
EXPOSE 80/tcp
