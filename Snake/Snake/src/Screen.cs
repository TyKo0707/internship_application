public class Screen
{
    public int Width { get; }
    public int Height { get; }
    private bool FindApple { get; }
    private string[,] ScreenVisual;
    private (int x, int y) ApplePosition;
    private int RemainingUnvisitedCells;

    private static Dictionary<string, string> MapDirectionsArrows = new()
    {
        { "u", " ↑ " },
        { "d", " ↓ " },
        { "l", " ← " },
        { "r", " → " }
    };

    public Screen(int width, int height, bool findApple = true)
    {
        Width = width;
        Height = height;
        FindApple = findApple;
        ScreenVisual = InitializeScreen();
        RemainingUnvisitedCells = Height * Width - 1;

        if (FindApple)
        {
            PlaceApple();
        }
    }

    private string[,] InitializeScreen()
    {
        var screen = new string[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                screen[i, j] = " . ";
            }
        }

        return screen;
    }

    private void PlaceApple()
    {
        var random = new Random();
        ApplePosition = (random.Next(Width), random.Next(Height));
        ScreenVisual[ApplePosition.y, ApplePosition.x] = " A ";
    }

    /// <summary>
    /// Updates the screen and determines whether the snake has reached the apple or completed visiting all cells.
    /// </summary>
    /// <param name="snake">The snake object containing its position and direction.</param>
    /// <returns>True if the game ends; otherwise, false.</returns>
    public bool sendSignal(Snake snake)
    {
        MoveSnake(snake);

        if (FindApple)
        {
            return snake.Position == ApplePosition;
        }

        if (ScreenVisual[snake.Position.y, snake.Position.x] == " . ")
        {
            RemainingUnvisitedCells--;
        }

        return RemainingUnvisitedCells == 0;
    }

    private void MoveSnake(Snake snake)
    {
        var (x, y) = snake.Position;
        snake.PrevPosition = (x, y);
        snake.PrevDirection = snake.Direction;

        snake.Position = snake.Direction switch
        {
            "u" => (x, y == 0 ? Height - 1 : y - 1),
            "d" => (x, (y + 1) % Height),
            "l" => (x == 0 ? Width - 1 : x - 1, y),
            "r" => ((x + 1) % Width, y),
            _ => (x, y)
        };
    }

    /// <summary>
    /// Updates the screen visuals with the snake's new position and direction.
    /// </summary>
    /// <param name="snake">The snake object containing its position and direction.</param>
    public void UpdateScreen(Snake snake)
    {
        if (snake.PrevPosition != null)
        {
            var (prevX, prevY) = snake.PrevPosition.Value;
            ScreenVisual[prevY, prevX] = MapDirectionsArrows[snake.PrevDirection];
        }

        ScreenVisual[snake.Position.y, snake.Position.x] = " S ";
    }

    /// <summary>
    /// Draws the current state of the screen to the console.
    /// </summary>
    public void DrawScreenText()
    {
        Console.WriteLine("\n" + new string('=', Width * 3 + 2));
        for (int i = 0; i < Height; i++)
        {
            Console.Write("|");
            for (int j = 0; j < Width; j++)
            {
                Console.Write(ScreenVisual[i, j]);
            }

            Console.WriteLine("|");
        }

        Console.WriteLine(new string('=', Width * 3 + 2));
    }
}