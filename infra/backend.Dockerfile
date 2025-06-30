# Step runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

# Step build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Agora.sln", "."]
COPY ["Agora.API/Agora.API.csproj", "Agora.API/"]
COPY ["Agora.Infrastructure/Agora.Infrastructure.csproj", "Agora.Infrastructure/"]
COPY ["Agora.Core/Agora.Core.csproj", "Agora.Core/"]

RUN dotnet restore

COPY . .

WORKDIR /src/Agora.API
RUN dotnet build -c Release -o /app/build

# Step publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Step finale runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Agora.API.dll"]
