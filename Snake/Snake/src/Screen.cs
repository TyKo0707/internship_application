public class Screen
{
    public int Width;
    public int Height;
    private bool FindApple;
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
        ScreenVisual = new string[Height, Width];
        RemainingUnvisitedCells = Height * Width - 1;

        for (int i = 0; i < Height; i++)
        for (int j = 0; j < Width; j++)
            ScreenVisual[i, j] = " . ";

        if (FindApple)
        {
            var random = new Random();
            ApplePosition = (random.Next(Width), random.Next(Height));
            ScreenVisual[ApplePosition.y, ApplePosition.x] = " A ";
        }
    }

    public bool sendSignal(Snake snake)
    {
        var (x, y) = snake.Position;
        snake.PrevPosition = (x, y);
        snake.PrevDirection = snake.Direction;

        switch (snake.Direction)
        {
            case "u":
                y = (y == 0) ? Height - 1 : y - 1;
                break;
            case "d":
                y = (y + 1 == Height) ? 0 : y + 1;
                break;
            case "l":
                x = (x == 0) ? Width - 1 : x - 1;
                break;
            case "r":
                x = (x + 1 == Width) ? 0 : x + 1;
                break;
        }

        snake.Position = (x, y);

        if (FindApple)
        {
            return snake.Position == ApplePosition;
        }
        else
        {
            if (ScreenVisual[y, x] == " . ")
            {
                RemainingUnvisitedCells--;
            }

            if (RemainingUnvisitedCells > 0)
            {
                return false;
            }

            return true;
        }
    }

    public void UpdateScreen(Snake snake)
    {
        var (snakeX, snakeY) = snake.Position;

        if (snake.PrevPosition != null)
        {
            var (prevX, prevY) = snake.PrevPosition.Value;
            ScreenVisual[prevY, prevX] = MapDirectionsArrows[snake.PrevDirection];
        }

        ScreenVisual[snakeY, snakeX] = " S ";
    }

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