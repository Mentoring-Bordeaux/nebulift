FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5052
EXPOSE 7109
ENV ASPNETCORE_URLS="http://+:5052"
ENV DOTNET_LAUNCH_PROFILE="http"
ARG PULUMI_ACCESS_TOKEN_VALUE
ENV PULUMI_ACCESS_TOKEN=${PULUMI_ACCESS_TOKEN_VALUE}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Nebulift.Api/Nebulift.Api.csproj", "src/Nebulift.Api/"]
RUN dotnet restore "src/Nebulift.Api/Nebulift.Api.csproj"
COPY . .
WORKDIR "/src/src/Nebulift.Api"
RUN dotnet build "Nebulift.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Nebulift.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nebulift.Api.dll"]
