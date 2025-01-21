namespace Nebulift.Api.Services.Blob;

using Types;
using Pulumi.Automation;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Exceptions;
using Templates;

/// <summary>
/// Runs templates remotely (using a GitHub repository) with Pulumi Automation.
/// </summary>
public class RemoteTemplateExecutor : ITemplateExecutor, IDisposable
{
    private readonly RemoteTemplateStorage _templateStorage;

    private static readonly JsonSerializerOptions _serializerOptions = new () { WriteIndented = true };

    private readonly ILogger<RemoteTemplateExecutor> _logger;
    private readonly HttpClient _client = new ();
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteTemplateExecutor"/> class.
    /// </summary>
    /// <param name="templateStorage"> A reference of the <see cref="ITemplateStorage"/> singleton.</param>
    /// <param name="logger"> An instance of type <see cref="ILogger{RemoteTemplateExecutor}"/> for logging.</param>
    public RemoteTemplateExecutor(ITemplateStorage templateStorage, ILogger<RemoteTemplateExecutor> logger)
    {
        _templateStorage = (RemoteTemplateStorage)templateStorage;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInformation("Remote template executor initializing.");
    }

    /// <summary>
    /// Main method to execute a template, stored in a given GitHub repository.
    /// </summary>
    /// <param name="id">The id of the template to execute.</param>
    /// <param name="inputs">The inputs to the template.</param>
    /// <returns>
    /// The outputs of the template execution, or <c>null</c> if the execution failed.
    /// </returns>
    public async Task<TemplateOutputs?> ExecuteTemplate(string id, TemplateInputs inputs)
    {
        _logger.LogInformation("Executing template with ID: {Id}", id);
        var templateCodeReference = _templateStorage.GetTemplateCodeReference(id);

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
            throw new MissingInputsException("Missing required inputs: pulumiUser and/or githubToken");
        }

        var stackName = $"{pulumiUser}/{id}/dev-{Guid.NewGuid().ToString("N")[..8]}";

        var stackArgs = new RemoteGitProgramArgs(stackName, templateCodeReference.Url.ToString())
        {
            ProjectPath = templateCodeReference.Path,
            Branch = templateCodeReference.Branch,
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
            string errorMessage = "Error: Update failed. Full update result:\n" + Serialize(upResult);
            throw new FailedTemplateExecutionException(errorMessage);
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

    /// <summary>
    /// Necessary for implementing the <see cref="IDisposable"/> interface.
    /// </summary>
    /// <param name="disposing">Whether the object is being disposed.</param>
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _client.Dispose();
        }

        _disposed = true;
    }

    /// <summary>
    /// Cleans up the resources used by the <see cref="RemoteTemplateStorage"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
    }
}