public class Utils
{
    public static int LCM(int a, int b)
    {
        return (a * b) / GCD(a, b);
    }

    public static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}