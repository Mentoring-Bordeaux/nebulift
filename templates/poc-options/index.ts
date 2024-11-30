import * as github from "@pulumi/github";
import * as nebulift from "./nebulift"; // We might need to make a module for this

process.env = {
  ...process.env,
  NEBULIFT_INPUTS: JSON.stringify({
    macros: { // Will be replace in the source code
        projectName: "Poc Options",
        projectDescription: "A configurable template for Nebulift",
    },
    constants: { // Will be used in the pulumi code
        repositoryName: "NebuliftTemplate",
        privateRepository: true,
    },
    env: {},
  }),
};

const inputs: nebulift.Inputs = nebulift.init();
const repositoryName = inputs.getConstant("repositoryName");
const isPrivate = inputs.getConstant("privateRepository");

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
