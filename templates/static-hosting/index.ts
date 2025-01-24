import * as pulumi from "@pulumi/pulumi";
import * as github from "@pulumi/github";
import * as azure from "@pulumi/azure-native";
import * as fs from "fs";
import * as nebulift from "./nebulift";


const inputs: nebulift.Inputs = nebulift.init();

const githubValues = inputs.getSectionDict("github");

const repoName = githubValues["repository_name"];
const visibility = githubValues["visibility"];
const contributors = githubValues["contributors"];

const welcomeMessage = inputs.getSectionValue("azure", "welcome_message");



// Create a new GitHub repository
const repo = new github.Repository("repo", {
    name: repoName,
    description: "Repository created by Pulumi",
    visibility: visibility,
    autoInit: true,
    hasIssues: true,
    hasWiki: true,
    hasProjects: true,
});

const sourcePath = "./code";
console.log(`Adding source code from ${sourcePath} to repository ${repoName}...`);
repo.name.apply((name) => {
  nebulift.addSourceCode(repoName, sourcePath, {welcomeMessage})

  for (const user of contributors) {
    new github.RepositoryCollaborator("repo_user" + user, {
      repository: repo.name,
      username: user,
    });
  }
});


// Export the repository URL
export const repositoryUrl = repo.httpCloneUrl;

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

let fileContent = fs.readFileSync(sourcePath + "/index.html", "utf8");

// Replace all macros in the file
const regex = new RegExp(`@@@welcomeMessage@@@`, "g");
fileContent = fileContent.replace(regex, welcomeMessage);

// Example: Uploading a sample index.html to $web container
const indexHtml = new azure.storage.Blob("index.html", {
    resourceGroupName: resourceGroup.name,
    accountName: storageAccount.name,
    containerName: webContainer.name,
    source: new pulumi.asset.StringAsset(fileContent), // Assumes index.html is in the same directory
    contentType: "text/html",
    blobName: "index.html"
});

export const staticEndpoint = storageAccount.primaryEndpoints.web;
export const indexUri = indexHtml.url;
