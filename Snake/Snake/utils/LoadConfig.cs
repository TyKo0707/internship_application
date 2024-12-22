using System.Text.Json;

public class LoadConfig
{
    public static Dictionary<string, string> LoadJsonConfig()
    {
        var shortPath = "./cfg.json";
        var longPath = "../../../../../cfg.json";
        var filePath = System.IO.File.Exists(shortPath) ? shortPath : longPath;
        
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file '{filePath}' not found.");
        }

        // Read the JSON content from the file
        string json = File.ReadAllText(filePath);

        // Deserialize the JSON content into a dictionary
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    }
}