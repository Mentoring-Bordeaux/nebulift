namespace Nebulift.Templates;

using System.IO;
using Pulumi;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

public class BlobCreator : Stack
{
    private const string ResourceGroupName = "template-rg";
    private const string ResourceGroupLocation = "WestEurope";

    public BlobCreator()
    {
        // Create a storage account with a blob storage
        // Purposely without any specific name, as it is shared across all Azure
        var storageAccount = new StorageAccount("templatestorage", new StorageAccountArgs
        {
            ResourceGroupName = ResourceGroupName,
            AllowBlobPublicAccess = true,
            Kind = Kind.StorageV2,
            Location = ResourceGroupLocation,
            Sku = new SkuArgs
            {
                Name = SkuName.Standard_LRS,
            },
        });

        // Create a blob container for template data
        var container = new BlobContainer("template-data", new BlobContainerArgs
        {
            ContainerName = "template-data",
            AccountName = storageAccount.Name,
            ResourceGroupName = ResourceGroupName,
            PublicAccess = PublicAccess.Container, // Allow public access to blobs
        });

        // Change ../backend/src/Nebulift.Api/appsettings.json to replace https://{accountName}.blob.core.windows.net/{containerName}
        const string appSettingsTemplatePath = "../backend/src/Nebulift.Api/appsettings-template.json";
        const string appSettingsResultPath = "../backend/src/Nebulift.Api/appsettings.json";
        // Reading and replacing contents in the appsettings template
        var appSettings = File.ReadAllText(appSettingsTemplatePath);
        var newUrlOutput = storageAccount.Name.Apply(accountName =>
            container.Name.Apply(containerName =>
                $"https://{accountName}.blob.core.windows.net/{containerName}"
            )
        );

        // Using Apply to ensure correct handling of Output<T>
        newUrlOutput.Apply(newUrl =>
        {
            var updatedAppSettings =
                appSettings.Replace("https://{accountName}.blob.core.windows.net/{containerName}", newUrl);
            File.WriteAllText(appSettingsResultPath, updatedAppSettings);
            return newUrl;
        });

        // Format the list URL using Output.Format
        var listUrl = Output.Format($"{newUrlOutput}?comp=list");

        // Correctly assign FilesLocation using Output.Apply or Output.Format
        FilesLocation = Output.Format($"{listUrl}");

        // For every folder in current directory, if it contains a Pulumi.yaml file, upload json files to storage account
        var toUpload = new[] { "inputs.json", "outputs.json", "identity.json" };
        foreach (var folder in Directory.GetDirectories("."))
        {
            if (!File.Exists(Path.Combine(folder, "Pulumi.yaml")))
                continue;

            foreach (var file in toUpload)
            {
                var filePath = Path.Combine(folder, file);
                if (!File.Exists(filePath))
                    continue;

                new Blob(filePath, new BlobArgs
                {
                    ResourceGroupName = ResourceGroupName,
                    AccountName = storageAccount.Name,
                    ContainerName = container.Name,
                    Source = new FileAsset(filePath),
                    ContentType = "application/octet-stream",
                });
            }
        }
    }

    [Output] public Output<string> FilesLocation { get; set; }
}