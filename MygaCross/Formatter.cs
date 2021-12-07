namespace MygaCross
{
    public static class Formatter
    {
        public static bool OfType<T>(object obj)
        {
            return obj is T;

        }
    }
}