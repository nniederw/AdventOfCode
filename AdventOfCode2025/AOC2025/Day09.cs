namespace AOC2025
{
    class Day09 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<(long x, long y)> input = new();
            foreach (var line in lines)
            {
                var split = line.Split(',');
                input.Add((Convert.ToInt64(split[0]), Convert.ToInt64(split[1])));
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private long GetArea((long x, long y) p1, (long x, long y) p2)
        {
            long dx = Math.Abs(p1.x - p2.x) + 1;
            long dy = Math.Abs(p1.y - p2.y) + 1;
            return dx * dy;
        }
        private long Part1(List<(long x, long y)> input)
        {
            long result = 0;
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = i + 1; j < input.Count; j++)
                {
                    result = Math.Max(result, GetArea(input[i], input[j]));
                }
            }
            return result;
        }
        private (long minx, long maxX, long minY, long maxY) CalcBoundingBox(List<(long x, long y)> input)
        {
            long minX, minY, maxX, maxY;
            minX = input.Select(i => i.x).Min();
            maxX = input.Select(i => i.x).Max();
            minY = input.Select(i => i.y).Min();
            maxY = input.Select(i => i.y).Max();
            return (minX, maxX, minY, maxY);
        }
        private bool IsInsideBox((long x, long y) boxC1, (long x, long y) boxC2, (long x, long y) point)
        {
            long minX = Math.Min(boxC1.x, boxC2.x);
            long maxX = Math.Max(boxC1.x, boxC2.x);
            long minY = Math.Min(boxC1.y, boxC2.y);
            long maxY = Math.Max(boxC1.y, boxC2.y);
            return minX <= point.x && point.x <= maxX && minY <= point.y && point.y <= maxY;
        }
        private long Part2(List<(long x, long y)> input)
        {
            input = input.ToList();
            long minX, minY, maxX, maxY;
            (minX, maxX, minY, maxY) = CalcBoundingBox(input);
            List<(long x, long y)> loopInput = input.ToList();
            loopInput.Add(input.First());
            List<Line> linesForward = new();
            List<Line> linesReverse = new();
            bool? forwardCorrect = null;
            for (int i = 1; i < loopInput.Count; i++)
            {
                var p1 = loopInput[i - 1];
                var p2 = loopInput[i];
                linesForward.Add(new Line(p1, p2));
                linesReverse.Add(new Line(p2, p1));
                if (forwardCorrect == null && OnBoundingBox(p1.x, p1.y) && OnBoundingBox(p2.x, p2.y))
                {
                    if (p1.x == p2.x)
                    {
                        if (p1.x == minX)
                        {
                            forwardCorrect = p1.y > p2.y;
                        }
                        else // x == maxX
                        {
                            forwardCorrect = p1.y < p2.y;
                        }
                    }
                    else // y == y
                    {
                        if (p1.y == minY)
                        {
                            forwardCorrect = p1.x < p2.x;
                        }
                        else // y == maxY
                        {
                            forwardCorrect = p1.x > p2.x;
                        }
                    }
                }
            }
            List<Line> lines = forwardCorrect.Value ? linesForward : linesReverse;
            //Console.WriteLine($"({minX},{minY}) - ({maxX},{maxY})");
            bool OnBoundingBox(long x, long y)
                => x == minX || x == maxX || y == minY || y == maxY;

            HashSet<(long x, long y)> EdgePointsInside = new();
            List<(long x, long y)> NotInsidePoints = new();
            //Console.WriteLine(lines.Select(i => Math.Abs(i.StartX - i.EndX) + Math.Abs(i.StartY - i.EndY) + 1).Sum());
            Console.WriteLine("Lines built");
            foreach (Line line in lines)
            {
                foreach (var point in line.LinePoints())
                {
                    EdgePointsInside.Add(point);
                }
            }
            Console.WriteLine("Line points inside built");
            foreach (Line line in lines)
            {
                foreach (var point in line.LinePointsOutside())
                {
                    if (!EdgePointsInside.Contains(point))
                    {
                        NotInsidePoints.Add(point);
                    }
                }
            }
            Console.WriteLine("Edge line points outside built");
            long maxArea = 0;
            for (int i = 0; i < input.Count; i++)
            {
                Console.WriteLine($"i: {i}");
                for (int j = i + 1; j < input.Count; j++)
                {
                    var p1 = input[i];
                    var p2 = input[j];
                    long area = GetArea(p1, p2);
                    if (area > maxArea)
                    {
                        bool valid = true;
                        foreach (var point in NotInsidePoints)
                        {
                            if (IsInsideBox(p1, p2, point))
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (valid)
                        {
                            maxArea = area;
                        }
                    }
                }
            }
            return maxArea;
        }
        private class Line
        {
            //Outside should be on the right when going from start to end
            //Only supports x or y axis alligned lines
            public long StartX;
            public long StartY;
            public long EndX;
            public long EndY;
            public long DX => Math.Sign(EndX - StartX);
            public long DY => Math.Sign(EndY - StartY);
            public (long x, long y) Start => (StartX, StartY);
            public (long x, long y) End => (EndX, EndY);
            public (long x, long y) D => (DX, DY);
            public long PDXOutside => DX == 0 ? DY : 0;
            public long PDYOutside => DY == 0 ? -DX : 0;
            public (long x, long y) PDOutside => (PDXOutside, PDYOutside);
            public Line(long startX, long startY, long endX, long endY)
            {
                StartX = startX;
                StartY = startY;
                EndX = endX;
                EndY = endY;
            }
            public Line((long x, long y) start, (long x, long y) end)
            {
                StartX = start.x;
                StartY = start.y;
                EndX = end.x;
                EndY = end.y;
            }
            public IEnumerable<(long x, long y)> LinePoints()
            {
                long x, y;
                (x, y) = Start;
                while (x != EndX || y != EndY)
                {
                    yield return (x, y);
                    x += DX;
                    y += DY;
                }
                yield return (x, y);
            }
            public IEnumerable<(long x, long y)> LinePointsOutside()
                => LinePoints().Select(i => (i.x + PDXOutside, i.y + PDYOutside));
        }
    }
}