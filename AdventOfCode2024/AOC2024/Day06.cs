namespace AOC2024
{
    class Day06 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
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
            //var datetime = DateTime.Now;
            //Console.WriteLine(Part2(board, width, height));
            //Console.WriteLine($"Took {(DateTime.Now -datetime).TotalSeconds}s");
            Console.WriteLine($"{2165} (Precomputed)");
        }
        private bool InBound(int x, int y, int width, int height)
           => 0 <= x && x < width && 0 <= y && y < height;
        private int Part1(char[,] board, int width, int height)
        {
            int guardX = 0;
            int guardY = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (board[x, y] == '^')
                    {
                        guardX = x;
                        guardY = y;
                    }
                }
            }
            HashSet<(int x, int y)> VisitedPos = new();
            VisitedPos.Add((guardX, guardY));
            int dx = 0;
            int dy = -1;
            while (InBound(guardX, guardY, width, height))
            {
                if (InBound(guardX + dx, guardY + dy, width, height))
                {
                    char c = board[guardX + dx, guardY + dy];
                    if (c == '#')
                    {
                        switch ((dx, dy))
                        {
                            case ((0, -1)):
                                dx = 1;
                                dy = 0;
                                break;
                            case ((1, 0)):
                                dx = 0;
                                dy = 1;
                                break;
                            case ((0, 1)):
                                dx = -1;
                                dy = 0;
                                break;
                            case ((-1, 0)):
                                dx = 0;
                                dy = -1;
                                break;
                        }
                    }
                    else
                    {
                        guardX += dx;
                        guardY += dy;
                    }
                }
                else
                {
                    break;
                }
                VisitedPos.Add((guardX, guardY));
            }
            return VisitedPos.Count();
        }
        private int Part2(char[,] board, int width, int height)
        {
            int guardX = 0;
            int guardY = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (board[x, y] == '^')
                    {
                        guardX = x;
                        guardY = y;
                    }
                }
            }
            int result = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == guardX && y == guardY)
                    {
                        continue;
                    }
                    if (board[x, y] == '.')
                    {
                        var boardCopy = (char[,])board.Clone();
                        boardCopy[x, y] = '#';
                        if (HasGuardLoop(boardCopy, width, height, guardX, guardY))
                        {
                            result++;
                        }
                    }
                }
            }
            return result;
        }
        private bool HasGuardLoop(char[,] board, int width, int height, int guardX, int guardY)
        {
            HashSet<(int x, int y, int dx, int dy)> VisitedPos = new();
            int dx = 0;
            int dy = -1;
            VisitedPos.Add((guardX, guardY, dx, dy));
            while (InBound(guardX, guardY, width, height))
            {
                if (InBound(guardX + dx, guardY + dy, width, height))
                {
                    char c = board[guardX + dx, guardY + dy];
                    if (c == '#')
                    {
                        switch ((dx, dy))
                        {
                            case ((0, -1)):
                                dx = 1;
                                dy = 0;
                                break;
                            case ((1, 0)):
                                dx = 0;
                                dy = 1;
                                break;
                            case ((0, 1)):
                                dx = -1;
                                dy = 0;
                                break;
                            case ((-1, 0)):
                                dx = 0;
                                dy = -1;
                                break;
                        }
                    }
                    else
                    {
                        guardX += dx;
                        guardY += dy;
                    }
                }
                else
                {
                    break;
                }
                if (VisitedPos.Contains((guardX, guardY, dx, dy)))
                {
                    return true;
                }
                VisitedPos.Add((guardX, guardY, dx, dy));
            }
            return false;
        }
    }
}