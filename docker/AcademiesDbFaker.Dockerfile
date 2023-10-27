ARG DOTNET_SDK=7.0
ARG MSSQL_VERSION=2022-latest
ARG DB_PASSWORD

# Stage 1 - Build and run console app to generate fake data
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK} AS data
COPY . .

WORKDIR /tests/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet run

FROM mcr.microsoft.com/mssql/server:${MSSQL_VERSION} AS sqlserver

COPY --from=data /tests/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker/data/ .
