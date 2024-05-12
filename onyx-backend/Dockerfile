FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["budget/src/Budget.API/Budget.API.csproj", "budget/src/Budget.API/"]
COPY ["budget/src/Budget.Application/Budget.Application.csproj", "budget/src/Budget.Application/"]
COPY ["budget/src/Budget.Domain/Budget.Domain.csproj", "budget/src/Budget.Domain/"]
COPY ["shared-kernel/Extensions/Extensions.csproj", "shared-kernel/Extensions/"]
COPY ["shared-kernel/Abstractions/Abstractions.csproj", "shared-kernel/Abstractions/"]
COPY ["shared-kernel/Models/Models.csproj", "shared-kernel/Models/"]
COPY ["budget/src/Budget.Infrastructure/Budget.Infrastructure.csproj", "budget/src/Budget.Infrastructure/"]
RUN dotnet restore "./budget/src/Budget.API/Budget.API.csproj"

COPY . .

RUN rm -f ./budget/src/Budget.API/appsettings.Development.json

WORKDIR "/src/budget/src/Budget.API"
RUN dotnet build "./Budget.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./Budget.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development
ARG COSMOSDB_ACCOUNT_URI
ARG COSMOSDB_PRIMARY_KEY
ARG COSMOSDB_DATABASE
ENV CosmosDb__AccountUri=$COSMOSDB_ACCOUNT_URI
ENV CosmosDb__PrimaryKey=$COSMOSDB_PRIMARY_KEY
ENV CosmosDb__Database=$COSMOSDB_DATABASE

ENTRYPOINT ["dotnet", "Budget.API.dll"]
