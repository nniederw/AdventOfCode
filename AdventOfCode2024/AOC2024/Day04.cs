using System.Linq;

namespace AOC2024
{
    class Day04 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            var board = Input.InterpretStringsAs2DCharArray(lines);
            int width = board.GetLength(0);
            int height = board.GetLength(1);
            Console.WriteLine(Part1(board, width, height));
            Console.WriteLine(Part2(board, width, height));
        }
        private int Part1(char[,] board, int width, int height)
        {
            string SearchStr = "XMAS";
            List<string> SearchSpace = new List<string> { SearchStr, new string(SearchStr.Reverse().ToArray()) };
            int result = 0;
            int length = SearchStr.Length;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result += new[]{
                    GetString(board, x, y, 1, 0, length, width, height),
                    GetString(board, x, y, 1, 1, length, width, height),
                    GetString(board, x, y, 0, 1, length, width, height),
                    GetString(board, x, y, -1, 1, length, width, height)
                    }.Count(i => SearchSpace.Contains(i));
                }
            }
            return result;
        }
        private bool InBound(int x, int y, int width, int height)
            => 0 <= x && x < width && 0 <= y && y < height;
        private IEnumerable<(int x, int y)> BoundedChain(int x, int y, int dx, int dy, int length, int width, int height)
            => Chain(x, y, dx, dy, length).Where(i => InBound(i.x, i.y, width, height));
        private IEnumerable<(int x, int y)> Chain(int x, int y, int dx, int dy, int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return (x, y);
                x += dx;
                y += dy;
            }
        }
        private string GetString(char[,] board, int x, int y, int dx, int dy, int length, int width, int height)
        {
            var chain = BoundedChain(x, y, dx, dy, length, width, height);
            return new string(chain.Select(i => board[i.x, i.y]).ToArray());
        }
        private int Part2(char[,] board, int width, int height)
        {
            string SearchStr = "MAS";
            List<string> SearchSpace = new List<string> { SearchStr, new string(SearchStr.Reverse().ToArray()) };

            int result = 0;
            int length = SearchStr.Length;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var matches = new[]{
                    GetString(board, x, y, 1, -1, length, width, height),
                    GetString(board, x + 2, y, -1, -1, length, width, height)
                    }.Count(SearchSpace.Contains);
                    if (matches == 2)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
    }
}