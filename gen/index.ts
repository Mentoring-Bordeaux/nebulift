import {
  RemoteWorkspace,
  fullyQualifiedStackName,
} from "@pulumi/pulumi/automation";

(async () => {
  try {
    const stackName = fullyQualifiedStackName(
      <user / organization>,
      "poc-template",
      "dev"
    );

    const stack = await RemoteWorkspace.createOrSelectStack(
      {
        stackName,
        url: "https://github.com/Mentoring-Bordeaux/nebulift.git",
        projectPath: "templates/poc-template",
        branch: "poc/generation",
      },
      {
        envVars: {
          GITHUB_TOKEN: process.env.GITHUB_TOKEN || "",
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
