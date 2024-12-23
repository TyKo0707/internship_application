public class Player
{
    private Screen Screen;
    private Snake Snake;
    private int S;
    private readonly string[] directions = { "r", "d", "l", "u" };
    private readonly int visualizationThreshold = 10000;

    public Player(Screen screen, Snake snake)
    {
        Screen = screen;
        Snake = snake;
        S = screen.Width * screen.Height;
    }

    /// <summary>
    /// Simulates a spiral movement pattern and writes the moves to a file.
    /// </summary>
    /// <param name="totalMoves">The total number of moves to simulate.</param>
    /// <param name="filePath">The path to the file where moves will be written.</param>
    public void SimulateSpiralMoves(int totalMoves, string filePath)
    {
        int stepsInDirection = 1;
        int moveCount = 0;
        int turnCount = 0;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int moveIndex = 0; moveIndex < totalMoves; moveIndex++)
            {
                writer.Write(directions[turnCount % 4]);
                moveCount++;

                if (moveCount == stepsInDirection)
                {
                    turnCount++;
                    moveCount = 0;

                    if (turnCount % 2 == 0)
                    {
                        stepsInDirection++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Simulates a simple zig-zag movement pattern and writes the moves to a file.
    /// </summary>
    /// <param name="totalMoves">The total number of moves to simulate.</param>
    /// <param name="filePath">The path to the file where moves will be written.</param>
    public void SimulateZigZagMoves(int totalMoves, string filePath)
    {
        string[] zigZagMoves = { "r", "d" };
        int zigZagIndex = 0;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int moveCount = 0; moveCount < totalMoves; moveCount++)
            {
                writer.Write(zigZagMoves[zigZagIndex]);
                zigZagIndex = (zigZagIndex + 1) % zigZagMoves.Length; // Alternate between "r" and "d"
            }
        }
    }

    /// <summary>
    /// Simulates a zig-zag movement pattern based on the LCM of two increasing integers and writes the moves to a file.
    /// </summary>
    /// <param name="totalMoves">The total number of moves to simulate.</param>
    /// <param name="filePath">The path to the file where moves will be written.</param>
    /// <param name="k">The integer coefficient of growth of movesUntilNextExtraStep (i.e. a and b)</param>
    public void SimulateLCMZigZagMoves(int totalMoves, string filePath, int k = 1)
    {
        int a = 1, b = 2;
        int currentLCM = Utils.LCM(a, b);
        int movesUntilNextExtraStep = currentLCM;
        int moveCount = 0;
        int zigZagIndex = 0;
        string[] zigZagMoves = { "r", "d" };

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            while (moveCount < totalMoves)
            {
                writer.Write(zigZagMoves[zigZagIndex]);

                zigZagIndex = (zigZagIndex + 1) % zigZagMoves.Length;
                moveCount++;
                movesUntilNextExtraStep--;

                // Check if it's time to add the extra "right" step
                if (movesUntilNextExtraStep == 0)
                {
                    writer.Write("r");
                    moveCount++;

                    a += k;
                    b += k;

                    currentLCM = Utils.LCM(a, b);
                    movesUntilNextExtraStep = currentLCM;
                }
            }
        }
    }

    /// <summary>
    /// Plays the game by reading moves from a file, updating the game state, and optionally visualizing the progress.
    /// </summary>
    /// <param name="filePath">The path to the file containing the moves.</param>
    /// <param name="verbose">Flag to enable detailed visualization during the game.</param>
    /// <returns>Total number of moves processed, including the move that ends the game.</returns>
    public int Play(string filePath, bool verbose = false)
    {
        int moveCount = 0;

        using (StreamReader reader = new StreamReader(filePath))
        {
            if (S <= visualizationThreshold)
            {
                Screen.UpdateScreen(Snake);
                Screen.DrawScreenText();
            }

            int currentChar;
            while ((currentChar = reader.Read()) != -1)
            {
                char move = (char)currentChar;

                if (char.IsWhiteSpace(move))
                    continue;

                moveCount++;

                Snake.ChangeDirection(move.ToString());
                if (UpdateAndCheck(verbose))
                {
                    moveCount++;
                    break;
                }
            }
        }

        return moveCount;
    }

    /// <summary>
    /// Updates the game state (Screen.UpdateScreen), checks for end conditions (Screen.sendSignal),
    /// and optionally visualizes intermediate states.
    /// </summary>
    /// <param name="verbose">Flag to enable detailed visualization during the update.</param>
    /// <returns>True if the game ends during this update; otherwise, false.</returns>
    public bool UpdateAndCheck(bool verbose)
    {
        Screen.UpdateScreen(Snake);

        if (S <= visualizationThreshold && verbose)
        {
            Screen.DrawScreenText();
            Thread.Sleep(50);
        }

        if (Screen.sendSignal(Snake))
        {
            if (S <= visualizationThreshold)
            {
                Screen.UpdateScreen(Snake);
                Screen.DrawScreenText();
            }

            return true;
        }

        return false;
    }
}