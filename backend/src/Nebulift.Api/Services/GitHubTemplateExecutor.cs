namespace Nebulift.Api.Services;

using Types;
using Pulumi.Automation;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// A local template service implementation for accessing and running templates stored in the Nebulift project repository.
/// </summary>
public class GitHubTemplateExecutor : ITemplateExecutor
{
    public async Task<TemplateOutputs> ExecuteTemplate(string id, TemplateInputs inputs)
    {
        Console.WriteLine("Executing template with ID: " + id);
        var inputNode = inputs.Content["templateData"]?["inputs"];
        var inputString = Serialize(inputNode);
        Console.WriteLine("Template inputs: " + inputString);

        var authNode = inputs.Content["templateData"]?["auth"];
        var pulumiUser = authNode?["pulumiUser"]?.ToString();
        var githubToken = authNode?["githubToken"]?.ToString();

        Console.WriteLine(pulumiUser);
        Console.WriteLine(githubToken);

        if (pulumiUser == null || githubToken == null)
        {
            Console.WriteLine("Error: Missing required fields in the template inputs.");
            return new TemplateOutputs();
        }

        try
        {
            var stackName = $"{pulumiUser}/{id}/dev-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            const string url = "https://github.com/Mentoring-Bordeaux/nebulift.git";

            // var stackArgs = new RemoteGitProgramArgs(stackName, url)
            // {
            //     ProjectPath = "templates/" + id,
            //     Branch = "feature/template-execution",
            //     Auth = new RemoteGitAuthArgs { PersonalAccessToken = githubToken },
            // };

            var stackArgs = new LocalProgramArgs(stackName, "../../../templates/" + id);

            var envValue = new EnvironmentVariableValue(inputString);
            // stackArgs.EnvironmentVariables = new Dictionary<string, EnvironmentVariableValue>
            //     { { "NEBULIFT_INPUTS", envValue } };

            var stack = await LocalWorkspace.CreateOrSelectStackAsync(stackArgs);

            // Preview the changes
            Console.WriteLine("Running preview ...");
            var previewResult = await stack.PreviewAsync();
            var previewString = Serialize(previewResult);
            Console.WriteLine("End of preview");
            Console.WriteLine("Preview result: " + previewString);

            // Apply the changes
            Console.WriteLine("Running update ...");
            var upResult = await stack.UpAsync();
            var upString = Serialize(upResult);
            Console.WriteLine("End of preview");
            Console.WriteLine("Update result: " + upString);

            // Optionally, destroy the stack
            // await stack.DestroyAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error running Pulumi automation: " + ex.Message);
            Console.WriteLine("Stack trace: " + ex.StackTrace);
            throw;
        }

        return new TemplateOutputs();
    }

    private static string Serialize(object node)
    {
        return JsonSerializer.Serialize(node, new JsonSerializerOptions { WriteIndented = true });
    }
}