public class Snake
{
    public (int x, int y) Position { get; set; }
    public (int x, int y)? PrevPosition { get; set; }
    public string Direction { get; set; }
    public string PrevDirection { get; set; }
    
    public Snake(Screen screen)
    {
        var random = new Random();
        Position = (random.Next(screen.Width), random.Next(screen.Height));
        Direction = "right";
    }
    
    /// <summary>
    /// Updates the snake's direction.
    /// </summary>
    /// <param name="newDirection">The new direction for the snake ("u", "d", "l", "r").</param>
    public void ChangeDirection(string newDirection)
    {
        Direction = newDirection;
    }
}