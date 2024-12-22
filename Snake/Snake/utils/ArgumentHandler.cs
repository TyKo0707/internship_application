class ArgumentHandler
{
    private readonly string[] _args;
    private readonly Dictionary<string, string> _configDefaults;

    public ArgumentHandler(string[] args, Dictionary<string, string> configDefaults)
    {
        _args = args;
        _configDefaults = configDefaults;
    }

    public (int width, int height, bool verbose, string choice, bool generateMoves) ParseArguments()
    {
        Dictionary<string, string> parsedArgs = ParseArgumentsToDictionary(_args);

        int width = GetArgumentValue(parsedArgs, "--width", "width", "positional-0",
            defaultValue: int.Parse(_configDefaults["width"]));
        int height = GetArgumentValue(parsedArgs, "--height", "height", "positional-1",
            defaultValue: int.Parse(_configDefaults["height"]));
        int verbose = GetArgumentValue(parsedArgs, "--verbose", "verbose", "positional-2",
            defaultValue: int.Parse(_configDefaults["verbose"]));
        string choice = GetArgumentValue(parsedArgs, "--choice", "choice", "positional-3",
            defaultValue: _configDefaults["choice"]);
        int generateMoves = GetArgumentValue(parsedArgs, "--generate_moves", "generate_moves", "positional-4",
            defaultValue: int.Parse(_configDefaults["generate_moves"]));

        return (width, height, verbose > 0, choice, generateMoves > 0);
    }

    private Dictionary<string, string> ParseArgumentsToDictionary(string[] args)
    {
        var arguments = new Dictionary<string, string>();
        int positionalIndex = 0;

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (arg.StartsWith("--"))
            {
                if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
                {
                    arguments[arg] = args[i + 1];
                    i++; // Skip the value
                }
                else
                {
                    arguments[arg] = "true"; // Handle flags
                }
            }
            else
            {
                // Handle positional arguments
                arguments[$"positional-{positionalIndex}"] = arg;
                positionalIndex++;
            }
        }

        return arguments;
    }

    private T GetArgumentValue<T>(Dictionary<string, string> args, string namedKey1, string namedKey2,
        string positionalKey, T defaultValue)
    {
        string value = null;

        if (args.ContainsKey(namedKey1))
        {
            value = args[namedKey1];
        }
        else if (args.ContainsKey(namedKey2))
        {
            value = args[namedKey2];
        }
        else if (args.ContainsKey(positionalKey))
        {
            value = args[positionalKey];
        }

        if (value != null)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        return defaultValue;
    }
}