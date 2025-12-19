namespace AOC2025
{
    public static class Global
    {
        public static string InputPath = Directory.GetParent(Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? ""))?.FullName ?? "";
        public static IEnumerable<T> Get8Neighbors<T>(this T[,] arr, int x, int y)
        {
            int sizeX = arr.GetLength(0);
            int sizeY = arr.GetLength(1);
            if (InBound(x - 1, y - 1)) { yield return arr[x - 1, y - 1]; }
            if (InBound(x, y - 1)) { yield return arr[x, y - 1]; }
            if (InBound(x + 1, y - 1)) { yield return arr[x + 1, y - 1]; }
            if (InBound(x + 1, y)) { yield return arr[x + 1, y]; }
            if (InBound(x + 1, y + 1)) { yield return arr[x + 1, y + 1]; }
            if (InBound(x, y + 1)) { yield return arr[x, y + 1]; }
            if (InBound(x - 1, y + 1)) { yield return arr[x - 1, y + 1]; }
            if (InBound(x - 1, y)) { yield return arr[x - 1, y]; }
            bool InBound(int x, int y) => 0 <= x && x < sizeX && 0 <= y && y < sizeY;
        }
    }
}
