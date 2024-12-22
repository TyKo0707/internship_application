public static class Utils
{
    /// <summary>
    /// Calculates the Least Common Multiple (LCM) of two integers.
    /// </summary>
    public static int LCM(int a, int b) => (a / GCD(a, b)) * b;

    /// <summary>
    /// Calculates the Greatest Common Divisor (GCD) of two integers using the Euclidean algorithm.
    /// </summary>
    public static int GCD(int a, int b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return a;
    }
}