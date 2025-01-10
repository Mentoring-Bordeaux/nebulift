import * as pulumi from "@pulumi/pulumi";
import * as github from "@pulumi/github";
import * as azure from "@pulumi/azure-native";
import * as fs from "fs";

// Define the repository and branch where the file will be uploaded
const repositoryName = "static-hosting";


// Create an Azure Resource Group
const resourceGroup = new azure.resources.ResourceGroup("resourceGroup", {
    location: "WestEurope",
});

// Create a Storage Account to save an index.html file
const storageAccount = new azure.storage.StorageAccount("nebuliftstorage", {
    resourceGroupName: resourceGroup.name,
    allowBlobPublicAccess: true,
    sku: {
        name: azure.storage.SkuName.Standard_LRS,
    },
    kind: azure.storage.Kind.StorageV2,
});

export const staticEndpoint = storageAccount.primaryEndpoints.web;
