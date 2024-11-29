import * as github from "@pulumi/github";

// Get data from the environment variables
const data = process.env.DATA;
if (!data) {
    throw new Error("DATA env var is required");
}
const parsedData = JSON.parse(data);

const repositoryName = parsedData.repositoryName;

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

