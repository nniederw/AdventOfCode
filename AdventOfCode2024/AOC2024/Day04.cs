namespace AOC2024
{
    class Day04 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        private const string ToSearch = "XMAS";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<string> input = new();
            foreach (var line in lines)
            {
                input.Add(line);
            }
            if (!input.TrueForAll(i => i.Length == input.First().Length))
            {
                Console.WriteLine($"Something doesn't add up in {nameof(Day04)}");
            }
            int width = input.First().Length;
            int height = input.Count;
            char[,] board = new char[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    board[x, y] = input[y][x];
                }
            }
            Console.WriteLine(Part1(board, width, height));
            Console.WriteLine(Part2(board, width, height));
        }
        private int Part1(char[,] board, int width, int height)
        {
            string toSearchReverse = new string(ToSearch.Reverse().ToArray());
            int result = 0;
            int length = ToSearch.Length;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    List<string> strs = new();
                    try { strs.Add(GetString(board, x, y, 1, 0, length)); } catch { }
                    try { strs.Add(GetString(board, x, y, 1, 1, length)); } catch { }
                    try { strs.Add(GetString(board, x, y, 0, 1, length)); } catch { }
                    try { strs.Add(GetString(board, x, y, -1, 1, length)); } catch { }
                    result += strs.Count(i => i == ToSearch || i == toSearchReverse);
                }
                Console.WriteLine($"Searched column {x}, found {result} many XMAS");
            }
            return result;
        }
        private bool InBound()
        {
            return false;
        }
        private string GetString(char[,] board, int x, int y, int dx, int dy, int length)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += board[x, y];
                x += dx;
                y += dy;
            }
            return result;
        }
        private int Part2(char[,] board, int width, int height)
        {
            string toSearch = "MAS";
            string toSearchReverse = new string(toSearch.Reverse().ToArray());
            int result = 0;
            int length = toSearch.Length;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    List<string> strs = new();
                    try { strs.Add(GetString(board, x, y, 1, -1, length)); } catch { }
                    try { strs.Add(GetString(board, x + 2, y, -1, -1, length)); } catch { }
                    if (strs.Count(i => i == toSearch || i == toSearchReverse) == 2)
                    { result++; }
                }
                Console.WriteLine($"Searched column {x}, found {result} many XMAS");
            }
            return result;
        }
    }
}