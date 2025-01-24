import * as pulumi from "@pulumi/pulumi";
import * as azure from "@pulumi/azure-native";
import * as fs from "fs";

// Resource group
const resourceGroup = new azure.resources.ResourceGroup("rg-basic-saas", {
    location: "WestEurope"
});

// Static Web App
const staticWebApp = new azure.web.StaticSite("stapp-basic-saas", {
    resourceGroupName: resourceGroup.name,
    location: resourceGroup.location,
    repositoryUrl: "https://github.com/your/repo",
    branch: "main",
    buildProperties: {
        appLocation: "./front",
        apiLocation: "./back",
        outputLocation: "./build",
    },
    sku: {
        tier: "Free",
        name: "Free",
    },
});

// App Service Plan
const appServicePlan = new azure.web.AppServicePlan("asplan-basic-saas", {
    resourceGroupName: resourceGroup.name,
    location: resourceGroup.location,
    sku: {
        name: "B1",
        tier: "Basic",
    },
});

// App Service
const appService = new azure.web.WebApp("wapp-basic-plan", {
    resourceGroupName: resourceGroup.name,
    location: resourceGroup.location,
    serverFarmId: appServicePlan.id,
    siteConfig: {
        appSettings: [
            {
                name: "WEBSITE_RUN_FROM_PACKAGE",
                value: "https://path/to/your/back.zip",
            },
        ],
    },
});

// Upload front-end build to Static Web App
const frontZip = new azure.storage.Blob("frontZip", {
    resourceGroupName: resourceGroup.name,
    accountName: staticWebApp.name,
    containerName: "$web",
    source: new pulumi.asset.FileArchive("./front"),
});

// Upload back-end build to App Service
const backZip = new azure.storage.Blob("backZip", {
    resourceGroupName: resourceGroup.name,
    accountName: appService.name,
    containerName: "back",
    source: new pulumi.asset.FileArchive("./back"),
});
