namespace AOC2024
{
    class Day06 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            var board = Input.InterpretAs2DCharArray(lines);
            int width = board.GetLength(0);
            int height = board.GetLength(1);
            Console.WriteLine(Part1(board, width, height));
            //Console.WriteLine(Part2(board, width, height));
            Console.WriteLine($"{2165} (Precomputed)");
        }
        private bool InBound(int x, int y, int width, int height)
           => 0 <= x && x < width && 0 <= y && y < height;
        private int Part1(char[,] board, int width, int height)
        {
            int guardX;
            int guardY;
            (guardX, guardY) = GetGuardPosition(board, width, height);
            return GetGuardVisits(board, width, height, guardX, guardY).Count();
        }
        private HashSet<(int x, int y)> GetGuardVisits(char[,] board, int width, int height, int guardX, int guardY)
        {
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
            return VisitedPos;
        }
        private (int guardX, int guardY) GetGuardPosition(char[,] board, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (board[x, y] == '^')
                    {
                        return (x, y);
                    }
                }
            }
            return (-1, -1);
        }
        private int Part2(char[,] board, int width, int height)
        {
            int guardX;
            int guardY;
            (guardX, guardY) = GetGuardPosition(board, width, height);
            var GuardVisits = GetGuardVisits(board, width, height, guardX, guardY);
            int result = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == guardX && y == guardY)
                    {
                        continue;
                    }
                    if (GuardVisits.Contains((x, y)) || board[x, y] == '.')
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