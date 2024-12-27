using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

class Program
{
    // Dictionary that maps move generation methods to their respective implementations
    private static readonly Dictionary<string, Action<Player, int, string>> moveGenerators = new()
    {
        { "spiral", (player, numMoves, path) => player.SimulateSpiralMoves(numMoves, path) },
        { "zigzag", (player, numMoves, path) => player.SimulateZigZagMoves(numMoves, path) }
    };

    static void Main(string[] args)
    {
        // Load configuration defaults
        Dictionary<string, string> configDefaults = LoadConfig.LoadJsonConfig();

        // Parse arguments using ArgumentHandler
        var argumentHandler = new ArgumentHandler(args, configDefaults);
        var (width, height, verbose, choice, generateMoves) = argumentHandler.ParseArguments();

        // Initialize game components
        var screen = new Screen(width, height, findApple: false);
        var snake = new Snake(screen);
        var player = new Player(screen, snake);

        // Retrieve paths and move generation count from config
        string movesPath = configDefaults["moves_path"];
        int numMovesToGenerate = int.Parse(configDefaults["num_moves"]);

        var stopwatch = Stopwatch.StartNew();
        int totalMoves = 0;

        // Handle user choice and simulate or play moves
        switch (choice)
        {
            case "spiral":
                HandleChoice(player, "spiral", generateMoves, numMovesToGenerate, movesPath, verbose, ref totalMoves);
                break;
            
            case "zigzag":
                HandleChoice(player, "zigzag", generateMoves, numMovesToGenerate, movesPath, verbose, ref totalMoves);
                break;
            
            case string s when s.Contains("dynamic_zigzag"):
                HandleChoice(player, choice, generateMoves, numMovesToGenerate, movesPath, verbose, ref totalMoves);
                break;

            default:
                Console.WriteLine("Invalid choice of method.");
                break;
        }

        stopwatch.Stop();
        PrintRoundData(totalMoves, stopwatch);
    }

    private static void HandleChoice(Player player, string choice, bool generateMoves, int numMovesToGenerate,
        string movesPath, bool verbose, ref int totalMoves)
    {
        string movesFilePath = movesPath + $"{choice}_moves.txt";

        if (generateMoves)
        {
            // Special handling for dynamic zigzag method
            if (choice.Contains("dynamic_zigzag"))
            {
                string pattern = @"k(\d+)$";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(choice);
                int kValue = 0;
                if (match.Success)
                {
                    string kValueStr = match.Groups[1].Value;
                    int.TryParse(kValueStr, out kValue);
                }

                if (kValue > 0)
                {
                    player.SimulateKDynamicZigZagMoves(numMovesToGenerate, movesFilePath, kValue);
                }
                else
                {
                    player.SimulateKDynamicZigZagMoves(numMovesToGenerate, movesFilePath);
                }
            }
            else
            {
                moveGenerators[choice](player, numMovesToGenerate, movesFilePath);
            }
            
            Console.WriteLine($"Generated {numMovesToGenerate} moves for {choice} method.");
        }

        totalMoves = player.Play(movesFilePath, verbose);
    }

    private static void PrintRoundData(int totalMoves, Stopwatch stopwatch)
    {
        Console.WriteLine($"Total number of moves: {totalMoves}");
        Console.WriteLine($"Time taken (in sec.): {stopwatch.Elapsed.TotalSeconds}");
    }
}