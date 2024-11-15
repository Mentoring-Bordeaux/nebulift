namespace Nebulift.Api.Templates;

public class HelloWorld
{
   public string CodeManager { get; set; } = "Github";
   public string ApiKey { get; set; }

   public HelloWorld(string apiKey)
   {
       ApiKey = apiKey;
   }
}
