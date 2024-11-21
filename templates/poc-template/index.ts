import * as pulumi from "@pulumi/pulumi";
import * as github from "@pulumi/github";
import * as fs from "fs";

// Define the repository and branch where the file will be uploaded
const repositoryName = "poc-template";

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
const helloWorldFile = new github.RepositoryFile("helloWorldFile", {
    repository: repo.name,
    file: "helloworld.txt",
    content: "Hello, World!",
    commitMessage: "Add helloworld.txt",
});

// Export the repository name and file path
export const repoName = repo.name;
export const filePath = helloWorldFile.file;
