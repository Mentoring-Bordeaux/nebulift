import * as pulumi from "@pulumi/pulumi";
import * as azure from "@pulumi/azure-native";
import * as fs from "fs";

// Create an Azure Resource Group
const resourceGroup = new azure.resources.ResourceGroup("resourceGroup", {
    location: "WestEurope",
    resourceGroupName: "template-rg",
});

// Create a Storage Account to save an index.html file
const storageAccount = new azure.storage.StorageAccount("templatestorage", {
    resourceGroupName: resourceGroup.name,
    accountName: "nebulifttemplatestorage",
    allowBlobPublicAccess: true,
    sku: {
        name: azure.storage.SkuName.Standard_LRS,
    },
    kind: azure.storage.Kind.StorageV2,
    location: resourceGroup.location,
});

// Create a blob container for static website files
const blobContainer = new azure.storage.BlobContainer("template-data", {
    accountName: storageAccount.name,
    resourceGroupName: resourceGroup.name,
    publicAccess: azure.storage.PublicAccess.Container, // Allow public access to blobs
});

export const filesLocation = pulumi.interpolate`https://${storageAccount.name}.blob.core.windows.net/${blobContainer.name}?comp=list`;


// For every folder in current directory, if it contains a Pulumi.yaml file, upload json files to storage account
const toUpload = ["inputs.json", "outputs.json", "identity.json"];
for (const folder of fs.readdirSync(".")) {
    if (!fs.lstatSync(folder).isDirectory()) continue;
    if (!fs.existsSync(`${folder}/Pulumi.yaml`)) continue;
    
    for (const file of toUpload) {
        const filePath = `${folder}/${file}`;
        if (!fs.existsSync(filePath)) continue;

        new azure.storage.Blob(filePath, {
            resourceGroupName: resourceGroup.name,
            accountName: storageAccount.name,
            containerName: blobContainer.name,
            source: new pulumi.asset.FileAsset(filePath),
            contentType: "application/octet-stream",
        });
    }
}

