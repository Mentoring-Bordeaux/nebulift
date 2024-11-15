namespace Nebulift.Api.Templates;

using Types;
using System.Text.Json;

public interface TemplateLocalRepository {

    private readonly string templatesFolder = "../../../../templates";

    ublic object GetTemplateIdentity(string id)
    {
        // Construct the path to the identity.json file
        var identityFilePath = Path.Combine(templatesFolder, id, "identity.json");

        // Check if the file exists
        if (!File.Exists(identityFilePath))
        {
            throw new FileNotFoundException($"Identity file not found at path: {identityFilePath}");
        }

        // Read and deserialize the JSON file
        var jsonContent = File.ReadAllText(identityFilePath);
        var identity = JsonSerializer.Deserialize<TIdentity>(jsonContent);

        if (identity == null)
        {
            throw new InvalidDataException("Failed to deserialize identity.json into a TIdentity object.");
        }

        return identity;
    }


    public TInputs GetTemplateInputs(string id){
        return new TInputs();
    }

    public TOutputs GetTemplateOutputs(string id){
        return new TOutputs();
    }
}