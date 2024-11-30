import {
  RemoteWorkspace,
  fullyQualifiedStackName,
} from "@pulumi/pulumi/automation";

const pulumiUser = process.env.PULUMI_USER;
if (!pulumiUser) {
  throw new Error("PULUMI_USER env var is required");
}
console.log("Pulumi user:", pulumiUser);

const inputs = {
  macros: {
    // Will be replace in the source code
    projectName: "Poc Options",
    projectDescription: "A configurable template for Nebulift.",
  },
  constants: {
    // Will be used in the pulumi code
    repositoryName: "NebuliftTemplate",
    privateRepository: false,
  },
  env: {},
};

(async () => {
  try {
    const stackName = fullyQualifiedStackName(
      pulumiUser,
      "poc-options",
      "dev"
    );

    const stack = await RemoteWorkspace.createOrSelectStack(
      {
        stackName,
        url: "https://github.com/Mentoring-Bordeaux/nebulift.git",
        projectPath: "templates/poc-options",
        branch: "poc/options",
      },
      {
        envVars: {
          // Sent to the template's pulumi code
          GITHUB_TOKEN: process.env.GITHUB_TOKEN || "",
          NEBULIFT_INPUTS: JSON.stringify(inputs),
        },
      }
    );

    // Preview the changes
    const previewResult = await stack.preview();
    console.log("Preview result:", previewResult);

    // Apply the changes
    const upResult = await stack.up();
    console.log("Update result:", upResult.summary);

    // Optionally, destroy the stack
    // await stack.destroy();
  } catch (error) {
    console.error("Error running Pulumi automation:", error);
  }
})();
