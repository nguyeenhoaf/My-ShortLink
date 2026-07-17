FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Services/Redirect/Redirect.API/Redirect.API.csproj Services/Redirect/Redirect.API/
COPY src/Services/Redirect/Redirect.Application/Redirect.Application.csproj Services/Redirect/Redirect.Application/
COPY src/Services/DataContext/Infras.MongoDb/Infras.MongoDb.csproj Services/DataContext/Infras.MongoDb/
COPY src/Services/DataContext/Infras.Redis/Infras.Redis.csproj Services/DataContext/Infras.Redis/
COPY src/Services/Common/HealthCheck/HealthCheck.csproj Services/Common/HealthCheck/

RUN dotnet restore Services/Redirect/Redirect.API.csproj

COPY src/Services/DataContext/Domain/Domain.csproj Services/DataContext/Domain/
COPY src/Services/DataContext/Application/Application.csproj Services/DataContext/Application/
COPY src/Services/DataContext/Utility/Utility.csproj Services/DataContext/Utility/

COPY src/Services/Redirect/ Services/Redirect/
COPY src/Services/DataContext/ Services/DataContext/
COPY src/Services/Common/ Services/Common/

WORKDIR /src/Services/Redirect/Redirect.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Redirect.API.dll"]