namespace Nebulift.Api.Templates;

using Types;
using Pulumi.Automation;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

/// <summary>
/// A local template service implementation for accessing and running templates stored in the Nebulift project repository.
/// </summary>
public class GitHubTemplateExecutor : ITemplateExecutor
{
    
    private string serialize(JsonNode node)
    {
        return JsonSerializer.Serialize(node, new JsonSerializerOptions { WriteIndented = true });
    }
    
    public async Task<TemplateOutputs> ExecuteTemplate(string id, TemplateInputs inputs)
    {
        Console.WriteLine("Executing template with ID: " + id);
        var inputNode = inputs.Content["templateData"]?["inputs"];
        var inputString = serialize(inputNode);
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

            var stackArgs = new RemoteGitProgramArgs(stackName, url)
            {
                ProjectPath = "templates/" + id,
                Branch = "feature/template-execution",
                Auth = new RemoteGitAuthArgs { PersonalAccessToken = githubToken },
            };

            var envValue = new EnvironmentVariableValue(inputString);
            stackArgs.EnvironmentVariables = new Dictionary<string, EnvironmentVariableValue>
                { { "NEBULIFT_INPUTS", envValue } };

            var stack = await RemoteWorkspace.CreateOrSelectStackAsync(stackArgs);

            // Preview the changes
            Console.WriteLine("Running preview ...");
            var previewResult = await stack.PreviewAsync();
            var previewString = JsonSerializer.Serialize(previewResult, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("End of preview");
            Console.WriteLine("Preview result: " + previewString);

            // Apply the changes
            Console.WriteLine("Running update ...");
            var upResult = await stack.UpAsync();
            var upString = JsonSerializer.Serialize(upResult, new JsonSerializerOptions { WriteIndented = true });
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
}