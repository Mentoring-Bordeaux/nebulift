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
