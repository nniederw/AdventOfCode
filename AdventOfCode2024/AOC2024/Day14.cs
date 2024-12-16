namespace AOC2024
{
    class Day14 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        private const long Width = 101;
        //private const long Width = 11;
        //private const long Height = 7;
        private const long Height = 103;
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<Robot> robots = new();
            foreach (var line in lines)
            {
                var s = line.Substring(2);
                var split = s.Split(" v=");
                var xy = split[0].Split(',');
                var dxy = split[1].Split(',');
                if (split.Length != 2 || xy.Length != 2 || dxy.Length != 2)
                {
                    throw new Exception($"Input for day 14 was in a wrong format");
                }
                long x = long.Parse(xy[0]);
                long y = long.Parse(xy[1]);
                long dx = long.Parse(dxy[0]);
                long dy = long.Parse(dxy[1]);
                robots.Add(new Robot((x, y), (dx, dy)));
            }
            Console.WriteLine(Part1(robots));
            Console.WriteLine($"{6355} (Precomputed)");
            //Console.WriteLine(Part2(robots));
        }
        private static (long x, long y) GetLoopAroundPosition((long x, long y) pos, long width, long height)
        {
            while (pos.x < 0)
            {
                pos = (pos.x + width, pos.y);
            }
            while (pos.x >= width)
            {
                pos = (pos.x - width, pos.y);
            } while (pos.y < 0)
            {
                pos = (pos.x, pos.y + height);
            }
            while (pos.y >= height)
            {
                pos = (pos.x, pos.y - height);
            }
            return pos;
        }
        private void MoveRobots(List<Robot> robots, int amount)
        {
            for (int j = 0; j < amount; j++)
            {
                for (int i = 0; i < robots.Count; i++)
                {
                    robots[i] = robots[i].MoveOnce();
                }
            }
        }
        private void PrintMap(int[,] map, long width, long height)
        {
            Console.WriteLine();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = map[x, y];
                    string s;
                    if (i == 0)
                    {
                        s = ".";
                    }
                    else
                    {
                        s = map[x, y].ToString();
                    }
                    Console.Write(s);
                }
                Console.WriteLine();
                if (y % 2 == 0)
                {
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// All values inclusive
        /// </summary>
        private long RobotsInRange(int[,] map, long XStart, long XEnd, long YStart, long YEnd)
        {
            long count = 0;
            for (long x = XStart; x <= XEnd; x++)
            {
                for (long y = YStart; y <= YEnd; y++)
                {
                    count += map[x, y];
                }
            }
            return count;
        }
        private long RobotsInUpLeftQuadrant(int[,] map)
            => RobotsInRange(map, 0, Width / 2 - 1, 0, Height / 2 - 1);
        private long RobotsInUpRightQuadrant(int[,] map)
            => RobotsInRange(map, Width / 2 + 1, Width - 1, 0, Height / 2 - 1);
        private long RobotsInDownRightQuadrant(int[,] map)
            => RobotsInRange(map, Width / 2 + 1, Width - 1, Height / 2 + 1, Height - 1);
        private long RobotsInDownLeftQuadrant(int[,] map)
            => RobotsInRange(map, 0, Width / 2 - 1, Height / 2 + 1, Height - 1);
        private long SafetyFactor(int[,] map)
            => RobotsInUpLeftQuadrant(map) * RobotsInUpRightQuadrant(map) * RobotsInDownRightQuadrant(map) * RobotsInDownLeftQuadrant(map);
        private long Part1(IReadOnlyList<Robot> robots)
        {
            var ActiveRobots = robots.ToList();
            MoveRobots(ActiveRobots, 100);
            int[,] map = new int[Width, Height];
            ActiveRobots.ForEach(i => map[i.Position.x, i.Position.y]++);
            return SafetyFactor(map);
        }
        public static IEnumerable<T> ToEnumerable<T>(T[,] target)
        {
            foreach (var item in target)
                yield return item;
        }
        private bool LooksSpecial(int[,] map, long width, long height)
        {
            const long Threshold = 20;
            long specialCols = 0;
            for (int x = 0; x < width; x++)
            {
                long sum = 0;
                for (int y = 0; y < height; y++)
                {
                    sum += map[x, y];
                }
                if (sum >= Threshold)
                {
                    specialCols++;
                }
            }
            long specialRows = 0;
            for (int y = 0; y < height; y++)
            {
                long sum = 0;
                for (int x = 0; x < width; x++)
                {
                    sum += map[x, y];
                }
                if (sum >= Threshold)
                {
                    specialRows++;
                }
            }
            return specialCols >= 2 && specialRows >= 2;
        }
        //already searched 0-508
        private long Part2(List<Robot> robots)
        {
            int skip = 6350;
            var ActiveRobots = robots.ToList();
            MoveRobots(ActiveRobots, skip);
            for (int i = 0; i < 10; i++) //assumtion is that it is in less than 20
            {
                Console.WriteLine($"{i + skip} seconds");
                int[,] map = new int[Width, Height];
                ActiveRobots.ForEach(i => map[i.Position.x, i.Position.y]++);
                MoveRobots(ActiveRobots, 1);
                //if (LooksSpecial(map, Width, Height))s
                {
                    PrintMap(map, Width, Height);
                }
            }
            return -1;
        }
        private struct Robot
        {
            public (long x, long y) Position;
            public (long dx, long dy) Velocity;
            public Robot((long x, long y) position, (long dx, long dy) velocity)
            {
                Position = position;
                Velocity = velocity;
            }
            public Robot MoveOnce()
            {
                (long x, long y) newPos = (Position.x + Velocity.dx, Position.y + Velocity.dy);
                return new Robot(GetLoopAroundPosition(newPos, Width, Height), Velocity);
            }
        }
    }
}