using Pulumi.Automation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{  
    static async Task Main(string[] args)
    {
        try
        {   
            var pulumiUser = Environment.GetEnvironmentVariable("PULUMI_USER");
            if (string.IsNullOrEmpty(pulumiUser))
            {
                Console.WriteLine("Erreur : La variable d'environnement PULUMI_USER n'est pas définie.");
                return;
            }
            Console.WriteLine("Pulumi user: " + pulumiUser);

            var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            if (string.IsNullOrEmpty(githubToken))
            {
                Console.WriteLine("Erreur : La variable d'environnement GITHUB_TOKEN n'est pas définie.");
                return;
            }

            var inputs = new Inputs
            {
               macros = new Macros
               {
                   projectName = "Poc options C#",
                   projectDescription = "A configurable template for Nebulift executed with C#"
               },
               constants = new Constants
               {
                   repositoryName = "Nebulift_Template",
                   privateRepository = false
               },
               env = new Dictionary<string, object>()
            };

            var inputsJson = JsonSerializer.Serialize(inputs);
            Console.WriteLine("Inputs: " + inputsJson);

            var stackName = $"{pulumiUser}/poc-options/dev3";
            var url = "https://github.com/Mentoring-Bordeaux/nebulift.git";
            
            var gitAuth = new RemoteGitAuthArgs
            {
                PersonalAccessToken = githubToken
            };

            var stackArgs = new RemoteGitProgramArgs(stackName, url) 
            {
                ProjectPath = "templates/poc-options",
                Branch = "feature/template-execution",
                Auth = gitAuth
            };
            
            var envValue = new EnvironmentVariableValue(inputsJson);
            stackArgs.EnvironmentVariables = new Dictionary<string, EnvironmentVariableValue>
            {
                { "NEBULIFT_INPUTS", envValue }
            };
            
            var stack = await Pulumi.Automation.RemoteWorkspace.CreateOrSelectStackAsync(stackArgs);

            // Preview the changes
            Console.WriteLine("Running preview ...");
            var previewResult = await stack.PreviewAsync();
            Console.WriteLine("End of preview");
            Console.WriteLine("Preview result: " + JsonSerializer.Serialize(previewResult));
            // Console.WriteLine("Preview result: " + previewResult);

            // Apply the changes
            Console.WriteLine("Running update ...");
            var upResult = await stack.UpAsync();
            Console.WriteLine("End of preview");
            Console.WriteLine("Update result: " + JsonSerializer.Serialize(upResult));

            // Optionally, destroy the stack
            // await stack.DestroyAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error running Pulumi automation: " + ex.Message);
            Console.WriteLine("Stack trace: " + ex.StackTrace);
        }
    }
}

class Inputs
{
   public required Macros macros { get; set; }
   public required Constants constants { get; set; }
   public required Dictionary<string, object> env { get; set; }
}

class Macros
{
   public string projectName { get; set; } = string.Empty;
   public string projectDescription { get; set; } = string.Empty;
}

class Constants
{
   public string repositoryName { get; set; } = string.Empty;
   public bool privateRepository { get; set; } = false;
}