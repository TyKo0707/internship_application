using System.Text.Json;

public class LoadConfig
{
    
    /// <summary>
    /// Loads configuration from a JSON file into a dictionary.
    /// The method attempts to locate the file at two possible paths.
    /// </summary>
    /// <returns>A dictionary containing the configuration key-value pairs.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the configuration file is not found at either path.</exception>
    /// <exception cref="JsonException">Thrown if the JSON content is invalid or cannot be deserialized.</exception>
    public static Dictionary<string, string> LoadJsonConfig()
    {
        const string shortPath = "./configs/main_config.json";
        const string longPath = "../../../../../configs/main_config.json";

        // Determine which path to use
        string filePath = File.Exists(shortPath) ? shortPath : longPath;

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configuration file not found at '{filePath}'.");
        }

        // Read and deserialize JSON content
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) 
               ?? throw new JsonException("Failed to deserialize configuration file.");
    }
}
