public class Player
{
    private Screen Screen;
    private Snake Snake;
    private readonly string[] directions = { "r", "d", "l", "u" };
    private int S;
    private readonly int visualizationThreshold = 10000;

    public Player(Screen screen, Snake snake)
    {
        Screen = screen;
        Snake = snake;
        S = screen.Width * screen.Height;
    }

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
    
    public void SimulateLCMZigZagMoves(int totalMoves, string filePath)
    {
        int a = 1, b = 2; // Start with initial dimensions A and B
        int currentLCM = Utils.LCM(a, b); // Compute initial LCM
        int movesUntilNextExtraStep = currentLCM; // Number of moves until we add the extra "right" step
        int moveCount = 0; // Track total moves
        int mainMoveIndex = 0; // Index to alternate between "right" and "down"
        string[] mainMoves = { "r", "d" }; // Zigzag moves

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            while (moveCount < totalMoves)
            {
                writer.Write(mainMoves[mainMoveIndex]);
                mainMoveIndex = (mainMoveIndex + 1) % 2;
                moveCount++;

                // Check if it's time to add the extra "right" step
                if (moveCount % movesUntilNextExtraStep == 0) // movesUntilNextExtraStep
                {
                    writer.Write("r");
                    moveCount++;

                    a += 2;
                    b += 2;
                    
                    currentLCM = Utils.LCM(a, b);
                    movesUntilNextExtraStep = currentLCM;
                }

                if (moveCount >= totalMoves)
                {
                    break;
                }
            }
        }
    }


    public int Play(string filePath, bool verbose = false)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            char move;

            if (S <= visualizationThreshold)
            {
                Screen.UpdateScreen(Snake);
                Screen.DrawScreenText();
            }

            int moveCount = 0;
            bool gameEnded;
            
            int currentChar;
            while ((currentChar = reader.Read()) != -1)
            {
                move = (char)currentChar;
                
                if (char.IsWhiteSpace(move))
                    continue;

                moveCount++;

                Snake.ChangeDirection(move.ToString());
                gameEnded = UpdateAndCheck(verbose);

                if (gameEnded)
                {
                    moveCount++;
                    break;
                }
            }
            return moveCount; // Return total moves if the file ends naturally
        }
    }

    public bool UpdateAndCheck(bool verbose)
    {
        Screen.UpdateScreen(Snake);
        if (S <= visualizationThreshold)
        {
            // Draw intermediate states of screen only if verbose is enabled
            if (verbose)
            {
                Screen.DrawScreenText();
                Thread.Sleep(50);
            }
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