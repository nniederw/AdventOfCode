namespace AOC2024
{
    /*
    NumberPad:
    +---+---+---+
    | 7 | 8 | 9 |
    +---+---+---+
    | 4 | 5 | 6 |
    +---+---+---+
    | 1 | 2 | 3 |
    +---+---+---+
        | 0 | A |
        +---+---+

    DirectionPad:
        +---+---+
        | ^ | A |
    +---+---+---+
    | < | v | > |
    +---+---+---+
    */
    class Day21 : IDay
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
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private (int x, int y) GetPositionOfNumber(char number)
        {
            switch (number)
            {
                case '1':
                    return (0, 2);
                case '2':
                    return (1, 2);
                case '3':
                    return (2, 2);
                case '4':
                    return (0, 1);
                case '5':
                    return (1, 1);
                case '6':
                    return (2, 1);
                case '7':
                    return (0, 0);
                case '8':
                    return (1, 0);
                case '9':
                    return (2, 0);
                case '0':
                    return (1, 3);
                case Accept:
                    return (2, 3);
            }
            throw new Exception($"number {number} is not on NumberPad");
        }
        private (int x, int y) ForbiddenNumberPadPosition = (0, 3);
        private (int x, int y) ForbiddenDirectionPadPosition = (0, 0);
        private const char Accept = 'A';
        private const char UpDirection = '^';
        private const char DownDirection = 'v';
        private const char LeftDirection = '<';
        private const char RightDirection = '>';
        private (int x, int y) AcceptDirectionLocation => GetPositionOfDirection(Accept);
        private (int x, int y) AcceptNumberLocation => GetPositionOfNumber(Accept);
        private (int x, int y) GetPositionOfDirection(char direction)
        {
            switch (direction)
            {
                case UpDirection:
                    return (0, 2);
                case Accept:
                    return (1, 2);
                case LeftDirection:
                    return (2, 2);
                case DownDirection:
                    return (0, 1);
                case RightDirection:
                    return (1, 1);
            }
            throw new Exception($"direction {direction} is not on DirectionPad");
        }
        private char GetDirectionChar((int dx, int dy) d)
        {
            switch (d)
            {
                case (1, 0):
                    return RightDirection;
                case (-1, 0):
                    return LeftDirection;
                case (0, 1):
                    return DownDirection;
                case (0, -1):
                    return UpDirection;
            }
            throw new Exception($"d ({d.dx},{d.dy}) is not for GetDirectionChar");
        }
        private IEnumerable<List<T>> Interleaves<T>(List<T> col1, IEnumerable<T> col2)
        {
            if (!col2.Any())
            {
                yield return col1;
                yield break;
            }
            if (!col1.Any())
            {
                yield return col1;
                yield break;
            }
            for (int i = 0; i < col1.Count + 1; i++)
            {
                var copy = col1.ToList();
                copy.Insert(i, col2.First());
                foreach (var variant in Interleaves(copy, col2.Skip(1)))
                {
                    yield return variant;
                }
            }
        }
        private List<char> TranslateToChar(List<(int dx, int dy)> moves)
            => moves.Select(i => GetDirectionChar(i)).ToList();

        private IEnumerable<List<char>> PossibleWaysToMoveToNumber(char numberFrom, char numberTo)
        {
            (int x, int y) from, to;
            from = GetPositionOfNumber(numberFrom);
            to = GetPositionOfNumber(numberTo);
            int totalDY = to.y - from.y;
            int totalDX = to.x - from.x;
            List<(int dx, int dy)> Vertical = new();
            for (int i = 0; i < Math.Abs(totalDY); i++)
            {
                Vertical.Add((0, Math.Sign(totalDY)));
            }
            List<(int dx, int dy)> Horizontal = new();
            for (int i = 0; i < Math.Abs(totalDX); i++)
            {
                Horizontal.Add((Math.Sign(totalDX), 0));
            }
            var possibilities = Interleaves(Horizontal, Vertical).ToList();
            HashSet<string> AlreadyReturned = new();
            foreach (var poss in possibilities)
            {
                var chars = TranslateToChar(poss);
                var str = new string(chars.ToArray());
                if (AlreadyReturned.Contains(str))
                {
                    continue;
                }
                AlreadyReturned.Add(str);
                (int x, int y) testingPos = from;
                bool ForbiddenVersion = false;
                foreach (var move in poss)
                {
                    testingPos = (testingPos.x + move.dx, testingPos.y + move.dy);
                    if (testingPos == ForbiddenNumberPadPosition)
                    {
                        ForbiddenVersion = true;
                    }
                }
                if (!ForbiddenVersion)
                {
                    yield return TranslateToChar(poss);
                }
            }
        }
        private IEnumerable<List<char>> PossibleWaysToDoDirection(char dirFrom, char dirTo)
        {
            (int x, int y) from, to;
            from = GetPositionOfDirection(dirFrom);
            to = GetPositionOfDirection(dirTo);
            int totalDY = to.y - from.y;
            int totalDX = to.x - from.x;
            List<(int dx, int dy)> Vertical = new();
            for (int i = 0; i < Math.Abs(totalDY); i++)
            {
                Vertical.Add((0, Math.Sign(totalDY)));
            }
            List<(int dx, int dy)> Horizontal = new();
            for (int i = 0; i < Math.Abs(totalDX); i++)
            {
                Horizontal.Add((Math.Sign(totalDX), 0));
            }
            var possibilities = Interleaves(Horizontal, Vertical).ToList();
            HashSet<string> AlreadyReturned = new();
            foreach (var poss in possibilities)
            {
                var chars = TranslateToChar(poss);
                var str = new string(chars.ToArray());
                if (AlreadyReturned.Contains(str))
                {
                    continue;
                }
                AlreadyReturned.Add(str);
                (int x, int y) testingPos = from;
                bool ForbiddenVersion = false;
                foreach (var move in poss)
                {
                    testingPos = (testingPos.x + move.dx, testingPos.y + move.dy);
                    if (testingPos == ForbiddenDirectionPadPosition)
                    {
                        ForbiddenVersion = true;
                    }
                }
                if (!ForbiddenVersion)
                {
                    yield return TranslateToChar(poss);
                }
            }
        }
        private IEnumerable<List<char>> ShortestDirectionsForString(string numberToBuild)
        {
            throw new NotImplementedException();
            char lastChar = Accept;

            foreach (var number in numberToBuild)
            {
                var possibleWay = PossibleWaysToMoveToNumber(lastChar, number);
                lastChar = number;
            }
        }
        private int Part1(List<string> input)
        {
            var t = PossibleWaysToMoveToNumber('7', 'A').ToList();
            return -1;
        }
        private int Part2(List<string> calo)
        {
            return -1;
        }
    }
}