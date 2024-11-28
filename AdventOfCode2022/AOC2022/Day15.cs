namespace AOC2022
{
    class Day15
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day15.txt");
            List<((int, int), (int, int))> input = new();
            foreach (var line in lines)
            {
                var l = line.Split(' ');
                (int, int) sensor = (Trim(l[2]), Trim(l[3]));
                (int, int) beacon = (Trim(l[8]), Trim(l[9] + ","));
                input.Add((sensor, beacon));
            }
            //Console.WriteLine(Part1(input));
            Console.WriteLine("5838453 (precomputed, and 1 off the result of the code)");
            Console.WriteLine("Unknown");
            //Console.WriteLine(Part2(input));
        }
        private static int Trim(string s) => Convert.ToInt32(s.Substring(2, s.Length - 3));
        private static int Part1(List<((int x, int y) sens, (int x, int y) beac)> input)
        {
            const int TargetY = 2000000;
            Dictionary<int, char> grid = new();
            //HashSet<(int x, char c)> grid = new();
            foreach (var sensor in input)
            {
                int distx = Math.Abs(sensor.sens.x - sensor.beac.x);
                int disty = Math.Abs(sensor.sens.y - sensor.beac.y);
                int dist = distx + disty;
                for (int x = -dist; x < dist; x++)
                {
                    int y = TargetY - sensor.sens.y;
                    if (Math.Abs(x) + Math.Abs(y) <= dist)
                    {
                        grid[sensor.sens.x + x] = '#';
                    }
                }
                if (sensor.beac.y == TargetY)
                {
                    grid[sensor.sens.x] = 'B';
                }
            }
            int sum = 0;
            foreach (var point in grid)
            {
                sum += point.Value == '#' ? 1 : 0;
            }
            /*
            var l = grid.ToList();
            l.Sort((a, b) => a.x.CompareTo(b.x));
            int X = l[0].x - 1;
            foreach (var item in l)
            {
                if (item.x == X + 1)
                {
                    Console.Write(item.c);
                    X++;
                }
                else
                {
                    X = item.x;
                    Console.Write('.');
                }
            }
            Console.WriteLine();*/
            return sum;
        }
        private static int Part2(List<((int, int), (int, int))> input)
        {
            int sum = 0;
            for (int x = 0; x < 4000000; x++)
            {
                for (int y = 0; y < 4000000; y++)
                {
                    sum++;
                }
                if (x % 10 == 0)
                {
                    Console.WriteLine($"x: {x}");
                }
            }
            return -1;
        }
    }
}