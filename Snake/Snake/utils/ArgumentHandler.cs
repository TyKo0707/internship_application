class ArgumentHandler
{
    private readonly string[] _args;
    private readonly Dictionary<string, string> _configDefaults;

    public ArgumentHandler(string[] args, Dictionary<string, string> configDefaults)
    {
        _args = args;
        _configDefaults = configDefaults;
    }

    /// <summary>
    /// Parses command-line arguments into usable values with defaults.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - width (int): The screen width.
    /// - height (int): The screen height.
    /// - verbose (bool): Verbosity flag.
    /// - choice (string): The user's choice.
    /// - generateMoves (bool): Flag to generate moves.
    /// </returns>
    public (int width, int height, bool verbose, string choice, bool generateMoves) ParseArguments()
    {
        var parsedArgs = ParseArgumentsToDictionary(_args);

        int width = GetArgumentValue(parsedArgs, "--width", "width", "positional-0",
            int.Parse(_configDefaults["width"]));
        int height = GetArgumentValue(parsedArgs, "--height", "height", "positional-1",
            int.Parse(_configDefaults["height"]));
        bool verbose = GetArgumentValue(parsedArgs, "--verbose", "verbose", "positional-2",
            int.Parse(_configDefaults["verbose"])) > 0;
        string choice = GetArgumentValue(parsedArgs, "--choice", "choice", "positional-3", _configDefaults["choice"]);
        bool generateMoves = GetArgumentValue(parsedArgs, "--generate_moves", "generate_moves", "positional-4",
            int.Parse(_configDefaults["generate_moves"])) > 0;

        return (width, height, verbose, choice, generateMoves);
    }

    /// <summary>
    /// Parses the command-line arguments into a dictionary for easier lookup.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>A dictionary mapping argument names to their values.</returns>
    private Dictionary<string, string> ParseArgumentsToDictionary(string[] args)
    {
        var arguments = new Dictionary<string, string>();
        int positionalIndex = 0;

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (arg.StartsWith("--"))
            {
                arguments[arg] = (i + 1 < args.Length && !args[i + 1].StartsWith("--")) ? args[++i] : "true";
            }
            else
            {
                arguments[$"positional-{positionalIndex++}"] = arg;
            }
        }

        return arguments;
    }

    /// <summary>
    /// Retrieves a value from the parsed arguments or returns the default.
    /// </summary>
    /// <typeparam name="T">The type of the value to return.</typeparam>
    /// <param name="args">The dictionary of parsed arguments.</param>
    /// <param name="namedKey1">The primary named argument key.</param>
    /// <param name="namedKey2">The secondary named argument key.</param>
    /// <param name="positionalKey">The key for a positional argument.</param>
    /// <param name="defaultValue">The default value to return if no key is found.</param>
    /// <returns>The value from the arguments or the default.</returns>
    private T GetArgumentValue<T>(Dictionary<string, string> args, string namedKey1, string namedKey2, string positionalKey, T defaultValue)
    {
        if (args.TryGetValue(namedKey1, out var value) || 
            args.TryGetValue(namedKey2, out value) || 
            args.TryGetValue(positionalKey, out value))
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        return defaultValue;
    }
}