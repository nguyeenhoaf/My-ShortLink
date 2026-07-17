FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Services/Write/Write.API/Write.API.csproj Services/Write/Write.API/
COPY src/Services/Write/Write.Application/Write.Application.csproj Services/Write/Write.Application/
COPY src/Services/DataContext/Infras.MongoDb/Infras.MongoDb.csproj Services/DataContext/Infras.MongoDb/
COPY src/Services/DataContext/Infras.Redis/Infras.Redis.csproj Services/DataContext/Infras.Redis/

RUN dotnet restore Services/Write/Write.API.csproj

COPY src/Services/DataContext/Domain/Domain.csproj Services/DataContext/Domain/
COPY src/Services/DataContext/Application/Application.csproj Services/DataContext/Application/
COPY src/Services/DataContext/Utility/Utility.csproj Services/DataContext/Utility/

COPY src/Services/Write/ Services/Write/
COPY src/Services/DataContext/ Services/DataContext/

WORKDIR /src/Services/Write/Write.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Write.API.dll"]