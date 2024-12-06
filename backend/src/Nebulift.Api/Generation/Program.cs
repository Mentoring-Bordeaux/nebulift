using System;
using System.Threading.Tasks;
using Pulumi.Automation;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var stackName = FullyQualifiedStackName.Create(
                "Adib0",
                "poc-template",
                "dev"
            );

            var stackArgs = new RemoteProgramArgs
            {
                StackName = stackName,
                Url = "https://github.com/Mentoring-Bordeaux/nebulift.git",
                ProjectPath = "templates/poc-template",
                Branch = "poc/generation",
                EnvironmentVariables =
                {
                    { "GITHUB_TOKEN", Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? "" }
                }
            };

            var stack = await RemoteWorkspace.CreateOrSelectStackAsync(stackArgs);

            // Preview the changes
            var previewResult = await stack.PreviewAsync();
            Console.WriteLine("Preview result: {0}", previewResult);

            // Apply the changes
            var upResult = await stack.UpAsync();
            Console.WriteLine("Update result: {0}", upResult.Summary);

            // Optionally, destroy the stack
            // await stack.DestroyAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error running Pulumi automation: {0}", ex);
        }
    }
}
