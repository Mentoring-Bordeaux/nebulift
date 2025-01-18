import * as pulumi from "@pulumi/pulumi";
import * as github from "@pulumi/github";
import * as azure from "@pulumi/azure-native";
import * as fs from "fs";

// Define the repository and branch where the file will be uploaded
const repositoryName = "static-hosting";

// Create a new GitHub repository
const repo = new github.Repository("repo", {
    name: repositoryName,
    description: "A new repository created with Pulumi",
    visibility: "public",
    autoInit: true,
    hasIssues: true,
    hasWiki: true,
    hasProjects: true,
});

// Create a new file in the GitHub repository from a string
new github.RepositoryFile("helloWorldFile", {
    repository: repo.name,
    file: "helloworld.txt",
    content: "Hello, World!",
    commitMessage: "Add helloworld.txt",
});


// Create a new file in the GitHub repository from a local file
new github.RepositoryFile("indexHtmlFile", {
    repository: repo.name,
    file: "index.html",
    commitMessage: "Add index.html",
    content: fs.readFileSync("./index.html", "utf-8"),
});

// Export the repository URL
export const repositoryUrl = repo.gitCloneUrl;

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

// Enable static website hosting on the Storage Account
new azure.storage.StorageAccountStaticWebsite(
    "staticWebsite",
    {
        accountName: storageAccount.name,
        resourceGroupName: resourceGroup.name,
        indexDocument: "index.html",
        error404Document: "404.html",
    }
);

// Create a $web container for static website files
const webContainer = new azure.storage.BlobContainer("$web", {
    accountName: storageAccount.name,
    resourceGroupName: resourceGroup.name,
    publicAccess: azure.storage.PublicAccess.Blob, // Allow public access to blobs
});

// Example: Uploading a sample index.html to $web container
const indexHtml = new azure.storage.Blob("index.html", {
    resourceGroupName: resourceGroup.name,
    accountName: storageAccount.name,
    containerName: webContainer.name,
    source: new pulumi.asset.FileAsset("index.html"), // Assumes index.html is in the same directory
    contentType: "text/html",
});

export const staticEndpoint = storageAccount.primaryEndpoints.web;
export const indexUri = indexHtml.url;
