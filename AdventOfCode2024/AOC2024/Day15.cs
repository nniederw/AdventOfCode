namespace AOC2024
{
    class Day15 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");

            List<string> mapInput = new();
            List<char> RobotMovements = new();
            bool firstPart = true;
            foreach (var line in lines)
            {
                if (line == "")
                {
                    firstPart = false;
                    continue;
                }
                if (firstPart)
                {
                    mapInput.Add(line);
                }
                else
                {
                    RobotMovements.AddRange(line);
                }
            }
            char[,] map = Input.InterpretStringsAs2DCharArray(mapInput);
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            Console.WriteLine(Part1(map, width, height, RobotMovements));
            Console.WriteLine(Part2(map, width, height, RobotMovements));
        }
        private void PrintMap(char[,] map, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
        }
        private (int dx, int dy) GetDirection(char Movement)
        {
            switch (Movement)
            {
                case 'v': return (0, 1);
                case '^': return (0, -1);
                case '<': return (-1, 0);
                case '>': return (1, 0);
            }
            throw new Exception($"Not supported char in {nameof(GetDirection)}");
        }
        private const char EmptySpace = '.';
        private const char Box = 'O';
        private const char Wall = '#';
        private const char Robot = '@';
        private bool IsEmptySpace(char c) => c == EmptySpace;
        private bool IsBox(char c) => c == Box;
        private bool IsWall(char c) => c == Wall;
        private bool IsRobot(char c) => c == Robot;
        private bool InBound(int x, int y, int width, int height)
          => 0 <= x && x < width && 0 <= y && y < height;
        private IEnumerable<(int x, int y)> NextPositions(char[,] map, int width, int height, int x, int y, int dx, int dy)
        {
            x += dx;
            y += dy;
            while (InBound(x, y, width, height))
            {
                yield return (x, y);
                x += dx;
                y += dy;
            }
        }
        private char[,] MoveRobotOnce(char[,] map, int width, int height, int RobotX, int RobotY, char Movement, out int RobotNewX, out int RobotNewY)
        {
            char[,] resultMap = (char[,])map.Clone();
            int dx, dy;
            (dx, dy) = GetDirection(Movement);
            char c = resultMap[RobotX + dx, RobotY + dy];
            if (IsWall(c))
            {
                RobotNewX = RobotX;
                RobotNewY = RobotY;
                return resultMap;
            }
            if (IsEmptySpace(c))
            {
                resultMap[RobotX, RobotY] = EmptySpace;
                resultMap[RobotX + dx, RobotY + dy] = Robot;
                RobotNewX = RobotX + dx;
                RobotNewY = RobotY + dy;
                return resultMap;
            }
            List<(int x, int y)> NextPos = new();
            foreach (var pos in NextPositions(map, width, height, RobotX, RobotY, dx, dy))
            {
                if (IsBox(map[pos.x, pos.y]))
                {
                    NextPos.Add(pos);
                }
                else
                {
                    NextPos.Add(pos);
                    break;
                }
            }
            var last = NextPos.Last();
            if (IsWall(resultMap[last.x, last.y]))
            {
                RobotNewX = RobotX;
                RobotNewY = RobotY;
                return resultMap;
            }
            if (IsEmptySpace(resultMap[last.x, last.y]))
            {
                resultMap[RobotX, RobotY] = EmptySpace;
                RobotNewX = RobotX + dx;
                RobotNewY = RobotY + dy;
                resultMap[RobotNewX, RobotNewY] = Robot;
                resultMap[last.x, last.y] = Box;
                return resultMap;
            }
            throw new Exception($"Unexpected events in {MoveRobotOnce} of day 15");
        }
        private (int x, int y) GetRobotPosition(char[,] map, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsRobot(map[x, y]))
                    {
                        return (x, y);
                    }
                }
            }
            throw new Exception("Robot not found");
        }
        private long GPS(int x, int y) => 100 * y + x;
        private long Part1(char[,] map, int width, int height, IReadOnlyList<char> RobotMovements)
        {
            int RobotX = 0;
            int RobotY = 0;
            (RobotX, RobotY) = GetRobotPosition(map, width, height);
            char[,] activeMap = (char[,])map.Clone();
            foreach (var movement in RobotMovements)
            {
                activeMap = MoveRobotOnce(activeMap, width, height, RobotX, RobotY, movement, out RobotX, out RobotY);
            }
            long gpsSum = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsBox(activeMap[x, y]))
                    {
                        gpsSum += GPS(x, y);
                    }
                }
            }
            return gpsSum;
        }
        private const char BoxLeft = '[';
        private const char BoxRight = ']';
        private bool IsBoxLeft(char c) => c == BoxLeft;
        private bool IsBoxRight(char c) => c == BoxRight;
        private bool IsBigBox(char c) => IsBoxLeft(c) || IsBoxRight(c);
        private IEnumerable<(int x, int y)> GetBoxes(char[,] map, int x, int y, int dx, int dy)
        {
            if (!IsBigBox(map[x + dx, y + dy]))
            {
                yield break;
            }
            yield return (x + dx, y + dy);
            if (dx != 0)
            {
                yield return (x + dx + dx, y + dy + dy);
                foreach (var box in GetBoxes(map, x + dx + dx, y + dy + dy, dx, dy))
                {
                    yield return box;
                }
            }
            else
            {
                (int x, int y) other;
                if (IsBoxLeft(map[x + dx, y + dy]))
                {
                    other = (x + dx + 1, y + dy);
                }
                else
                {
                    other = (x + dx - 1, y + dy);
                }
                yield return other;
                foreach (var orig in GetBoxes(map, x + dx, y + dy, dx, dy))
                {
                    yield return orig;
                }
                foreach (var oth in GetBoxes(map, other.x, other.y, dx, dy))
                {
                    yield return oth;
                }
            }
        }
        private bool BoxesMovable(char[,] map, int dx, int dy, IReadOnlyCollection<(int x, int y)> boxes)
        {
            foreach (var box in boxes)
            {
                if (IsWall(map[box.x + dx, box.y + dy]))
                {
                    return false;
                }
            }
            return true;
        }
        private char[,] MoveBoxes(char[,] map, int dx, int dy, IReadOnlyCollection<(int x, int y)> boxes)
        {
            char[,] result = (char[,])map.Clone();
            foreach (var box in boxes)
            {
                result[box.x, box.y] = EmptySpace;
            }
            foreach (var box in boxes)
            {
                result[box.x + dx, box.y + dy] = map[box.x, box.y];
            }
            return result;
        }
        private char[,] MoveRobotOnceBigBoxes(char[,] map, int RobotX, int RobotY, char Movement, out int RobotNewX, out int RobotNewY)
        {
            char[,] resultMap = (char[,])map.Clone();
            int dx, dy;
            (dx, dy) = GetDirection(Movement);
            char c = resultMap[RobotX + dx, RobotY + dy];
            if (IsWall(c))
            {
                RobotNewX = RobotX;
                RobotNewY = RobotY;
                return resultMap;
            }
            if (IsEmptySpace(c))
            {
                resultMap[RobotX, RobotY] = EmptySpace;
                resultMap[RobotX + dx, RobotY + dy] = Robot;
                RobotNewX = RobotX + dx;
                RobotNewY = RobotY + dy;
                return resultMap;
            }
            if (IsBigBox(c))
            {
                var boxes = GetBoxes(resultMap, RobotX, RobotY, dx, dy).ToList();
                if (!BoxesMovable(resultMap, dx, dy, boxes))
                {
                    RobotNewX = RobotX;
                    RobotNewY = RobotY;
                    return resultMap;
                }
                RobotNewX = RobotX + dx;
                RobotNewY = RobotY + dy;
                resultMap = MoveBoxes(resultMap, dx, dy, boxes);
                resultMap[RobotX, RobotY] = EmptySpace;
                resultMap[RobotX + dx, RobotY + dy] = Robot;
                return resultMap;
            }
            throw new Exception($"Unexpected events with char {c} in {MoveRobotOnce} of day 15");
        }
        private char[,] ExpandMap(char[,] map, int width, int height)
        {
            char[,] result = new char[width * 2, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char c = map[x, y];
                    if (IsWall(c) || IsEmptySpace(c))
                    {
                        result[x * 2, y] = c;
                        result[x * 2 + 1, y] = c;
                    }
                    else if (IsRobot(c))
                    {
                        result[x * 2, y] = c;
                        result[x * 2 + 1, y] = EmptySpace;
                    }
                    else if (IsBox(c))
                    {
                        result[x * 2, y] = BoxLeft;
                        result[x * 2 + 1, y] = BoxRight;
                    }
                }
            }
            return result;
        }
        private long BigBoxGPS(int x, int y) => 100 * y + x;
        private bool MapCorrect(char[,] map, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsBoxLeft(map[x, y]) && !IsBoxRight(map[x + 1, y]))
                    {
                        return false;
                    }
                    if (IsBoxRight(map[x, y]) && !IsBoxLeft(map[x - 1, y]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private long Part2(char[,] map, int width, int height, List<char> RobotMovements)
        {
            int RobotX = 0;
            int RobotY = 0;
            char[,] activeMap = ExpandMap(map, width, height);
            width *= 2;
            (RobotX, RobotY) = GetRobotPosition(activeMap, width, height);
            int iteration = 0;
            foreach (var movement in RobotMovements)
            {
                //PrintMap(activeMap, width, height);
                activeMap = MoveRobotOnceBigBoxes(activeMap, RobotX, RobotY, movement, out RobotX, out RobotY);
                if (!MapCorrect(activeMap, width, height))
                {
                    PrintMap(activeMap, width, height);
                    throw new Exception("Map now incorrect");
                }
                iteration++;
            }
            long gpsSum = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsBoxLeft(activeMap[x, y]))
                    {
                        gpsSum += BigBoxGPS(x, y);
                    }
                }
            }
            return gpsSum;
        }
    }
}