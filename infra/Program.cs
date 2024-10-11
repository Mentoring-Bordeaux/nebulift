using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using System.Collections.Generic;

// Define global variables
const string repoUrl = "https://github.com/Mentoring-Bordeaux/nebulift";
const string sitePath = "src/front"; // TODO : change the sitePath
const string apiPath = "src/back"; // TODO : change the apiPath



return await Pulumi.Deployment.RunAsync(() =>
{
    // Retrieve the current stack name
    var stackName = Pulumi.Deployment.Instance.StackName;

    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("rg-nebulift-dev-westeurope-");

    // Create an App Service
    var appServicePlan = new AppServicePlan("wapp-nebulift-dev-westeurope-", new AppServicePlanArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Location = resourceGroup.Location,
        Sku = new SkuDescriptionArgs
        {
            Name = stackName == "prod" ? "B1" : "F1",
            Tier = stackName == "prod" ? "Basic" : "Free"
        }
    });

    // Create a Static Web App
    var staticWebApp = new StaticSite("stapp-nebulift-dev-westeurope-", new StaticSiteArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Location = resourceGroup.Location,
        RepositoryUrl = repoUrl,
        Branch = "main",
        BuildProperties = new StaticSiteBuildPropertiesArgs
        {
            AppLocation = sitePath,
            ApiLocation = apiPath,
            OutputLocation = "build"
        },
        Sku = new SkuDescriptionArgs
        {
            Name = "Free"
        }
    });

    // Export the variable dictionary
    return new Dictionary<string, object?>
    {
        ["ResourceGroupName"] = resourceGroup.Name,
        ["AppServicePlanName"] = appServicePlan.Name,
        ["StaticWebAppName"] = staticWebApp.Name
    };
});