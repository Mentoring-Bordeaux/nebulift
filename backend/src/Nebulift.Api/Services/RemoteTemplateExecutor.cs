namespace Nebulift.Api.Services;

using Types;
using Pulumi.Automation;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// Runs templates remotely (using a GitHub repository) with Pulumi Automation.
/// </summary>
public class RemoteTemplateExecutor : ITemplateExecutor
{
    private static readonly JsonSerializerOptions _serializerOptions = new () { WriteIndented = true };
    private readonly ILogger<RemoteTemplateExecutor> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteTemplateExecutor"/> class.
    /// </summary>
    /// <param name="logger"> An instance of type <see cref="ILogger{RemoteTemplateExecutor}"/> for logging.</param>
    public RemoteTemplateExecutor(ILogger<RemoteTemplateExecutor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Main method to execute a template, stored in a given GitHub repository.
    /// </summary>
    /// <param name="identity">The identity of the template to execute.</param>
    /// <param name="inputs">The inputs to the template.</param>
    /// <returns>
    /// The outputs of the template execution, or <c>null</c> if the execution failed.
    /// </returns>
    public async Task<TemplateOutputs?> ExecuteTemplate(TemplateIdentity identity, TemplateInputs inputs)
    {
        _logger.LogInformation("Executing template with ID: {TemplateName}", identity.Name);
        var inputNode = inputs.Content["templateData"]?["inputs"];
        var inputString = Serialize(inputNode);

        var authNode = inputs.Content["templateData"]?["auth"];
        var pulumiUser = authNode?["pulumiUser"]?.ToString();
        var githubToken = authNode?["githubToken"]?.ToString();
        var azureClientId = authNode?["azureClientId"]?.ToString();
        var azureClientSecret = authNode?["azureClientSecret"]?.ToString();
        var azureSubscriptionId = authNode?["azureSubscriptionId"]?.ToString();
        var azureTenantId = authNode?["azureTenantId"]?.ToString();

        if (pulumiUser == null || githubToken == null)
        {
            _logger.LogWarning("Error: Missing required fields in the template inputs.");
            return null;
        }

        var stackName = $"{pulumiUser}/{identity.Name}/dev-{Guid.NewGuid().ToString("N")[..8]}";

        var stackArgs = new RemoteGitProgramArgs(stackName, identity.Url)
        {
            ProjectPath = identity.Path,
            Branch = identity.Branch,
            Auth = new RemoteGitAuthArgs { PersonalAccessToken = githubToken },
            EnvironmentVariables = new Dictionary<string, EnvironmentVariableValue>
            {
                { "NODE_OPTIONS", new EnvironmentVariableValue("--max-old-space-size=1000000") },
                { "NEBULIFT_INPUTS", new EnvironmentVariableValue(inputString) },
                { "ARM_CLIENT_ID", new EnvironmentVariableValue(azureClientId) },
                { "ARM_CLIENT_SECRET", new EnvironmentVariableValue(azureClientSecret, true) },
                { "ARM_SUBSCRIPTION_ID", new EnvironmentVariableValue(azureSubscriptionId) },
                { "ARM_TENANT_ID", new EnvironmentVariableValue(azureTenantId) },
            },
        };

        var stack = await RemoteWorkspace.CreateStackAsync(stackArgs);

        _logger.LogInformation("Pulumi deployment URL: https://app.pulumi.com/{StackName}/deployments/1", stackName);

        var upResult = await stack.UpAsync();

        if (upResult.Summary.Result != UpdateState.Succeeded)
        {
            _logger.LogError("Error: Update failed. Full update result:\n{UpdateResult}", Serialize(upResult));
            return null;
        }

        var outputDict = new Dictionary<string, string>();
        foreach (var output in upResult.Outputs)
        {
            outputDict[output.Key] = $"{output.Value.Value}";
            _logger.LogInformation("Output: {Key} = {Value}", output.Key, output.Value.Value);
        }

        return new TemplateOutputs(outputDict);
    }

    private static string Serialize(object node)
    {
        return JsonSerializer.Serialize(node, _serializerOptions);
    }
}