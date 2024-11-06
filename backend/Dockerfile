FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier le fichier projet de l'API
COPY ["src/Nebulift.Api/Nebulift.Api.csproj", "src/Nebulift.Api/"]
RUN dotnet restore "src/Nebulift.Api/Nebulift.Api.csproj"

# Copier tous les fichiers source
COPY . .

# Build le projet
RUN dotnet publish "src/Nebulift.Api/Nebulift.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Nebulift.Api.dll"]