using Pulumi.Automation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {   
            var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            if (string.IsNullOrEmpty(githubToken))
            {
                Console.WriteLine("Erreur : La variable d'environnement GITHUB_TOKEN n'est pas définie.");
                return;
            }

            var stackName = "Adib0/poc-template/dev";
            var url = "https://github.com/Mentoring-Bordeaux/nebulift.git";

            // var startInfo = new ProcessStartInfo
            // {
            //     FileName = "dotnet",
            //     Arguments = "run",
            //     UseShellExecute = false,
            //     RedirectStandardOutput = true,
            //     RedirectStandardError = true
            // };
            // startInfo.EnvironmentVariables["GITHUB_TOKEN"] = githubToken;

            var gitAuth = new RemoteGitAuthArgs
            {
                PersonalAccessToken = githubToken
            };

            var stackArgs = new RemoteGitProgramArgs(stackName, url) 
            {
                ProjectPath = "templates/poc-template",
                Branch = "feature/template-execution",
                Auth = gitAuth
            };
            
            var stack = await Pulumi.Automation.RemoteWorkspace.CreateOrSelectStackAsync(stackArgs);

            // Preview the changes
            Console.WriteLine("Running preview ...");
            var previewResult = await stack.PreviewAsync();
            Console.WriteLine("End of preview");
            Console.WriteLine("Preview result: " + Newtonsoft.Json.JsonConvert.SerializeObject(previewResult, Newtonsoft.Json.Formatting.Indented));


            // Apply the changes
            Console.WriteLine("Running update ...");
            var upResult = await stack.UpAsync();
            Console.WriteLine("End of preview");
            Console.WriteLine("Update result: " + Newtonsoft.Json.JsonConvert.SerializeObject(upResult, Newtonsoft.Json.Formatting.Indented));

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