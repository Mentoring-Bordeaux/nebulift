namespace Nebulift.Api.Templates;

using System;
using YamlDotNet.Serialization; 
using YamlDotNet.Serialization.NamingConventions; 

class PulumiConfigGenerator {
   private string _filePath;
   private dynamic _yamlObject;

   public PulumiConfigGenerator(string filePath) {
      _filePath = filePath;
      loadTemplateFile();
   }

   private void loadTemplateFile() {
      var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
      var yaml = File.ReadAllText(_filePath);
      _yamlObject = deserializer.Deserialize<dynamic>(yaml);
   }

   public void generateConfig(string username, string repoName, string outputPath) {
      _yamlObject["github"]["username"] = username;
      _yamlObject["github"]["repository_name"] = repoName;

      var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
      var updatedYaml = serializer.Serialize(_yamlObject);
      File.WriteAllText(outputPath, yaml);
   }
} 