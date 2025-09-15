# Use .NET SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY MyBackend/*.csproj ./MyBackend/
RUN dotnet restore MyBackend/MyBackend.csproj

# Copy everything and build
COPY MyBackend/. ./MyBackend/
WORKDIR /src/MyBackend
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyBackend.dll"]
