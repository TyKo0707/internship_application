using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, string> configDefaults = LoadConfig.LoadJsonConfig();
        
        // Use the ArgumentHandler to parse arguments
        var argumentHandler = new ArgumentHandler(args, configDefaults);
        var (width, height, verbose, choice, generateMoves) = argumentHandler.ParseArguments();

        var screen = new Screen(width, height, findApple: false);
        var snake = new Snake(screen);
        var player = new Player(screen, snake);
        String movesPath = configDefaults["moves_path"];
        int numMovesToGenerate = int.Parse(configDefaults["num_moves"]);

        var stopwatch = Stopwatch.StartNew();

        int totalMoves = 0;

        switch (choice)
        {
            case "spiral":
                if (generateMoves)
                {
                    player.SimulateSpiralMoves(numMovesToGenerate, movesPath + "spiral_moves.txt");
                    Console.WriteLine($"Generated {numMovesToGenerate} moves for {choice} method.");
                }

                totalMoves = player.Play(movesPath + "spiral_moves.txt", verbose);
                break;
            case "lcm_zigzag":
                if (generateMoves)
                {
                    player.SimulateLCMZigZagMoves(numMovesToGenerate, movesPath + "lcm_zigzag_moves.txt");
                    Console.WriteLine($"Generated {numMovesToGenerate} moves for {choice} method.");
                }

                totalMoves = player.Play(movesPath + "lcm_zigzag_moves.txt", verbose);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        stopwatch.Stop();
        PrintRoundData(totalMoves, stopwatch);
    }

    private static void PrintRoundData(int totalMoves, Stopwatch stopwatch)
    {
        Console.WriteLine($"Total number of moves: {totalMoves}");
        Console.WriteLine($"Time taken (in sec.): {stopwatch.Elapsed.TotalSeconds}");
    }
}