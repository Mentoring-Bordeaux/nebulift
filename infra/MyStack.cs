using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

// Define global variables

class MyStack : Stack
{
    const string repoUrl = "https://github.com/Mentoring-Bordeaux/nebulift";
    const string sitePath = "src/front"; // TODO : change the sitePath
    const string apiPath = "src/back"; // TODO : change the apiPath

    public MyStack()
    {
        // Retrieve the current stack name
        var stackName = Pulumi.Deployment.Instance.StackName;

        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("rg-nebulift-dev-westeurope-");

        // Create an App Service Plan
        var appServicePlan = new AppServicePlan("wapp-plan-nebulift-dev-westeurope-", new AppServicePlanArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Sku = new SkuDescriptionArgs
            {
                Name = stackName == "prod" ? "B1" : "F1",
                Tier = stackName == "prod" ? "Basic" : "Free"
            }
        });

        // Create an App Service
        var appService = new WebApp("wapp-nebulift-dev-westeurope-", new WebAppArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            ServerFarmId = appServicePlan.Id
        });

        // Export the App Service URL
        this.Endpoint = Output.Format($"https://{appService.DefaultHostName}");

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
        this.ResourceGroupName = resourceGroup.Name;
        this.AppServicePlanName = appServicePlan.Name;
        this.StaticWebAppName = staticWebApp.Name;
    }

    [Output]
    public Output<string> Endpoint { get; set; }

    [Output]
    public Output<string> ResourceGroupName { get; set; }

    [Output]
    public Output<string> AppServicePlanName { get; set; }

    [Output]
    public Output<string> StaticWebAppName { get; set; }
}