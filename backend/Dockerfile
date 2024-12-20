FROM pulumi/pulumi-dotnet

COPY src/Nebulift.Api/ .

RUN dotnet build Nebulift.Api.csproj -c Release -o /app

RUN dotnet publish Nebulift.Api.csproj -c Release -o /app

EXPOSE 5052
EXPOSE 7109

CMD [ "dotnet", "run", "bin/Release/net8.0/Nebulift.Api.dll", "--launch-profile", "https" ]