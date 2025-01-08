# Set the major version of dotnet
ARG DOTNET_VERSION=8.0
# Set the major version of nodejs
ARG NODEJS_VERSION_MAJOR=22

# Note: For optimal Docker caching, ensure all steps within a build stage are arranged in order of "least likely to be impacted by changes"

### Build frontend assets using node js ###
FROM "node:${NODEJS_VERSION_MAJOR}-bullseye-slim" AS assets
WORKDIR /app

# - Copy over and restore npm packages separately so they can be cached by Docker
COPY ./DfE.FindInformationAcademiesTrusts/package.json      /app
COPY ./DfE.FindInformationAcademiesTrusts/package-lock.json /app
RUN npm ci

# - Only copy over frontend assets so this can be cached independently to other app changes
COPY ./DfE.FindInformationAcademiesTrusts/webpack.config.js /app
COPY ./DfE.FindInformationAcademiesTrusts/assets/           /app/assets/
RUN npm run build

### Build the app using the dotnet SDK ###
FROM "mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0" AS build
WORKDIR /build

# - Copy over csprojs and restore nuget packages separately so they can be cached by Docker
COPY ./DfE.FindInformationAcademiesTrusts.Data.Hardcoded/DfE.FindInformationAcademiesTrusts.Data.Hardcoded.csproj       /build/DfE.FindInformationAcademiesTrusts.Data.Hardcoded/
COPY ./DfE.FindInformationAcademiesTrusts.Data.FiatDb/DfE.FindInformationAcademiesTrusts.Data.FiatDb.csproj             /build/DfE.FindInformationAcademiesTrusts.Data.FiatDb/
COPY ./DfE.FindInformationAcademiesTrusts.Data/DfE.FindInformationAcademiesTrusts.Data.csproj                           /build/DfE.FindInformationAcademiesTrusts.Data/
COPY ./DfE.FindInformationAcademiesTrusts.Data.AcademiesDb/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.csproj   /build/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb/
COPY ./DfE.FindInformationAcademiesTrusts/DfE.FindInformationAcademiesTrusts.csproj                                     /build/DfE.FindInformationAcademiesTrusts/
RUN ["dotnet", "restore", "DfE.FindInformationAcademiesTrusts"]

# - Only copy over relevant C# code so this can be cached independently to other app changes
COPY ./DfE.FindInformationAcademiesTrusts.Data.Hardcoded/   /build/DfE.FindInformationAcademiesTrusts.Data.Hardcoded/
COPY ./DfE.FindInformationAcademiesTrusts.Data.FiatDb/      /build/DfE.FindInformationAcademiesTrusts.Data.FiatDb/
COPY ./DfE.FindInformationAcademiesTrusts.Data/             /build/DfE.FindInformationAcademiesTrusts.Data/
COPY ./DfE.FindInformationAcademiesTrusts.Data.AcademiesDb/ /build/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb/
COPY ./DfE.FindInformationAcademiesTrusts/                  /build/DfE.FindInformationAcademiesTrusts/
RUN ["dotnet", "build", "DfE.FindInformationAcademiesTrusts", "--no-restore", "-c", "Release"]

RUN ["dotnet", "publish", "DfE.FindInformationAcademiesTrusts", "--no-build", "-o", "/app"]

### Entity Framework: Migration Builder ###
FROM build AS efbuilder
WORKDIR /build
ENV PATH=$PATH:/root/.dotnet/tools
RUN ["mkdir", "/sql"]
RUN ["dotnet", "tool", "install", "--global", "dotnet-ef"]
RUN ["dotnet", "ef", "migrations", "bundle", "-r", "linux-x64", "-p", "DfE.FindInformationAcademiesTrusts", "--context", "DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts.FiatDbContext", "--configuration", "Release", "--no-build", "-o", "/sql/migratefiatdb"]
COPY ./docker/init-docker-entrypoint.sh /sql/entrypoint.sh
RUN ["chmod", "+x", "/sql/entrypoint.sh"]

### Entity Framework: Migration Runner ###
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS initcontainer
WORKDIR /sql
COPY --from=efbuilder /sql /sql
COPY --from=build /app/appsettings* /Dfe.FindInformationAcademiesTrusts/
RUN chown "$APP_UID" "/sql" -R
RUN chown "$APP_UID" "/Dfe.FindInformationAcademiesTrusts" -R
USER $APP_UID

### Build a runtime environment ###
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/find-information-about-academies-and-trusts"
LABEL org.opencontainers.image.description="Find Information about Academies and Trusts"

# - Copy and configure docker entrypoint script
COPY ./docker/web-docker-entrypoint.sh /app/docker-entrypoint.sh
RUN ["chmod", "+x", "/app/docker-entrypoint.sh"]

# - Copy frontend assets
COPY --from=assets /app/wwwroot /app/wwwroot

# - Copy dotnet app
COPY --from=build /app /app

USER $APP_UID
