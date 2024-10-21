using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

// Define global variables

class MyStack : Stack
{

    public MyStack()
    {
        // Retrieve configuration and set config variables
        var config = new Config(); 
        var location = config.Get("azure-native:location");

        var stackName = Pulumi.Deployment.Instance.StackName;
        var projectName = Pulumi.Deployment.Instance.ProjectName;

        var appServiceSkuName = config.Require("AppServiceSkuName");
        var staticWebAppSkuName = config.Require("StaticWebAppSkuName");

        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup($"rg-{projectName}-{stackName}-");

        // Create an App Service Plan
        var appServicePlan = new AppServicePlan($"wapp-plan-{projectName}-{stackName}-", new AppServicePlanArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuDescriptionArgs
            {
                Name = appServiceSkuName
            }
        });

        // Create an App Service
        var appService = new WebApp($"wapp-{projectName}-{stackName}-", new WebAppArgs
        {
            ResourceGroupName = resourceGroup.Name,
            ServerFarmId = appServicePlan.Id
        });

        // Create a Static Web App
        var staticWebApp = new StaticSite($"stapp-{projectName}-{stackName}-", new StaticSiteArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuDescriptionArgs
            {
                Name = staticWebAppSkuName
            },
            BuildProperties = new StaticSiteBuildPropertiesArgs
            {
                // No build properties : CI/CD will build
            }
        });

        // // Link front-end to back-end
        // // Only possible on paid plan
        // var backendLink = new StaticSiteLinkedBackend($"backlink-{projectName}-{stackName}-", new()
        // {
        //     Name = staticWebApp.Name,
        //     BackendResourceId = appService.Id,
        //     ResourceGroupName = resourceGroup.Name,
        // });

        // Export the variable dictionary
        this.Endpoint = Output.Format($"https://{appService.DefaultHostName}");
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