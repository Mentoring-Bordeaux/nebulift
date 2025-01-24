import * as github from "@pulumi/github";
import * as nebulift from "./nebulift"; // We could make a module for this

console.log(`Program arguments: ${process.argv}`);

const inputs: nebulift.Inputs = nebulift.init();

const githubValues = inputs.getSectionDict("github");

const repositoryName = githubValues["repository_name"];
const visibility = githubValues["visibility"];
const contributors = githubValues["contributors"];

const projectName = githubValues["project_name"];
const projectDescription = githubValues["project_description"];
const macros = { projectName, projectDescription };


console.log(process.env);
console.log(`Creating repository ${repositoryName}...`);
console.log(`Visibility: ${visibility}`);

// Create the GitHub repository first
const repo = new github.Repository("repo", {
  name: repositoryName,
  visibility: visibility,
  hasIssues: true,
  hasWiki: true,
  hasProjects: true,
});

// Create files only after the repository is created
// and replace all the placeholders with the actual values
const sourcePath = "./code";
console.log(`Adding source code from ${sourcePath} to repository ${repositoryName}...`);
repo.name.apply((name) => {
  nebulift.addSourceCode(repositoryName, 'sourcePath', macros)

  for (const user of contributors) {
    new github.RepositoryCollaborator("repo_user" + user, {
      repository: repo.name,
      username: user,
    });
  }
});

export const repoName = repo.name;
