ARG DOTNET_SDK=7.0

# Build and run console app to generate fake data
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK}
COPY . /app
WORKDIR /app/tests/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker

RUN mv ./data/ /data
