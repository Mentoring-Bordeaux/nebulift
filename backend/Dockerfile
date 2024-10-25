FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Nebulift.csproj", "./"]

RUN dotnet restore "Nebulift.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet build "Nebulift.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nebulift.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Nebulift.dll"]
