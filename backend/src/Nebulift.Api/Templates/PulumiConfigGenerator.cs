// namespace Nebulift.Api.Templates;

// using System.IO;
// using YamlDotNet.RepresentationModel;
// using YamlDotNet.Serialization; 
// using YamlDotNet.Serialization.NamingConventions; 

// public class PulumiConfigGenerator {
//    private readonly string _filePath;
//    private YamlStream _yamlObject;

//    public PulumiConfigGenerator(string filePath) {
//       _filePath = filePath;
//       _yamlObject = new YamlStream();
//       loadTemplateFile();
//    }

//    private void loadTemplateFile() {
//       var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
//       var yaml = File.ReadAllText(_filePath);
//       _yamlObject = deserializer.Deserialize<dynamic>(yaml);
//    }

//    public void generateConfig(string username, string repoName, string outputPath) {
//       _yamlObject["github"]["username"] = username;
//       _yamlObject["github"]["repository_name"] = repoName;

//       var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
//       var updatedYaml = serializer.Serialize(_yamlObject);
//       File.WriteAllText(outputPath, yaml);
//    }
// } 