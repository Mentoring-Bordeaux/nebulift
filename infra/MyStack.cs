using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

// Define global variables

class MyStack : Stack
{
    private string CreateResourceName(params string[] args){
        string name = "";
        foreach (string item in args)
        {
            name += item + "-";
        }
        return name;
    }

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
        var resourceGroup = new ResourceGroup(CreateResourceName("rg", projectName, stackName));

        // Create an App Service Plan
        var appServicePlan = new AppServicePlan(CreateResourceName("wapp-plan", projectName, stackName), new AppServicePlanArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Sku = new SkuDescriptionArgs
            {
                Name = appServiceSkuName
            }
        });

        // Create an App Service
        var appService = new WebApp(CreateResourceName("wapp", projectName, stackName), new WebAppArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            ServerFarmId = appServicePlan.Id
        });

        // Create a Static Web App
        var staticWebApp = new StaticSite(CreateResourceName("stapp", projectName, stackName), new StaticSiteArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Location = resourceGroup.Location,
            Sku = new SkuDescriptionArgs
            {
                Name = staticWebAppSkuName
            },
            BuildProperties = new StaticSiteBuildPropertiesArgs
            {
                // No build properties : CI/CD will build
            }
        });

        // Link front-end to back-end
        // Only possible on paid plan
        // TODO : replace if condition with better practice
        if (staticWebAppSkuName == "Standard" || staticWebAppSkuName == "Dedicated") {
            var backendLink = new StaticSiteLinkedBackend(CreateResourceName("backlink", projectName, stackName), new()
            {
                Name = staticWebApp.Name,
                BackendResourceId = appService.Id,
                LinkedBackendName = appService.Name,
                Region = resourceGroup.Location,
                ResourceGroupName = resourceGroup.Name,
            });
        }

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