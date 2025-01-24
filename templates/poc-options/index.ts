import * as github from "@pulumi/github";
import * as nebulift from "./nebulift"; // We could make a module for this


const inputs: nebulift.Inputs = nebulift.init();

const githubValues = inputs.getSectionDict("github");

const repoName = githubValues["repository_name"];
const visibility = githubValues["visibility"];
const contributors = githubValues["contributors"];

const projectName = githubValues["project_name"];
const projectDescription = githubValues["project_description"];
const macros = { projectName, projectDescription };


// Create the GitHub repository first
const repo = new github.Repository("repo", {
  name: repoName,
  visibility: visibility,
  hasIssues: true,
  hasWiki: true,
  hasProjects: true,
});

// Create files only after the repository is created
// and replace all the placeholders with the actual values
const sourcePath = "./code";
console.log(`Adding source code from ${sourcePath} to repository ${repoName}...`);
repo.name.apply((name) => {
  nebulift.addSourceCode(repoName, sourcePath, macros)

  for (const user of contributors) {
    new github.RepositoryCollaborator("repo_user" + user, {
      repository: repo.name,
      username: user,
    });
  }
});

export const repositoryUrl = repo.httpCloneUrl;
