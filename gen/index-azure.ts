import {
  RemoteWorkspace,
  fullyQualifiedStackName,
} from "@pulumi/pulumi/automation";

(async () => {
  try {
    const stackName = fullyQualifiedStackName(
      <user / or organization>,
      "static-hosting",
      "dev"
    );

    const stack = await RemoteWorkspace.createOrSelectStack(
      {
        stackName,
        url: "https://github.com/Mentoring-Bordeaux/nebulift.git",
        projectPath: "templates/static-hosting",
        branch: "poc/generation",
      },
      {
        envVars: {
          AZURE_SUBSCRIPTION_ID: "eb5d6be8-0ea2-414c-99c2-35555ad030e2",
          AZURE_CLIENT_ID: process.env.AZURE_CLIENT_ID ?? "", // TODO: Create a service principal ?
          AZURE_CLIENT_SECRET: {
            secret: process.env.AZURE_CLIENT_SECRET ?? "", // TODO: Create a service principal ?
          },
          AZURE_TENANT_ID: "7136878c-4ab3-4d6e-920f-f2868a6de747",
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
