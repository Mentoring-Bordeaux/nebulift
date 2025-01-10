import * as github from "@pulumi/github";
import * as nebulift from "./nebulift"; // We might need to make a module for this

const inputs: nebulift.Inputs = nebulift.init();
const repositoryName = inputs.getConstant("repositoryName");
const isPrivate = inputs.getConstant("privateRepository");

console.log(`Creating repository ${repositoryName}...`);
console.log(`Private repository: ${isPrivate}`);

// Create the GitHub repository first
const repo = new github.Repository("repo", {
  name: repositoryName,
  visibility: isPrivate ? "private" : "public",
  hasIssues: true,
  hasWiki: true,
  hasProjects: true,
});

// Create files only after the repository is created
// and replace all the placeholders with the actual values
const sourcePath = "code";
repo.name.apply((name) =>
  nebulift.addSourceCode(repositoryName, sourcePath)
);

export const repoName = repo.name;
