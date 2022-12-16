namespace AOC2022
{
    class Day14
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day14.txt");
            int input = new();
            List<((int x, int y), (int x, int y))> Rocks = new();
            foreach (var line in lines)
            {
                var l = line.Split(" -> ");
                for (int i = 1; i < l.Length; i++)
                {
                    Rocks.Add((GetPos(l[i - 1]), GetPos(l[i])));
                }
            }
            int maxX;//max + 1
            int maxY;// max + 1
            char[,] grid = new char[maxX, maxY];
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    grid[x, y] = '.';
                }
            }
            foreach (var rock in Rocks)
            {
                int dx = rock.Item2.x - rock.Item1.x;
                int dy = rock.Item2.y - rock.Item1.y;
                dx = Math.Clamp(dx, -1, 1);
                dy = Math.Clamp(dy, -1, 1);
                int x = rock.Item1.x;
                int y = rock.Item1.y;
                grid[x, y] = '#';
                while ((x, y) != rock.Item2)
                {
                    grid[x, y] = '#';
                    x += dx;
                    y += dy;
                }
            }
            Console.WriteLine(Part1(grid));
            Console.WriteLine(Part2(grid));
        }
        private static (int, int) GetPos(string s)
        {
            var ss = s.Split(',');
            return (Convert.ToInt32(ss[0]), Convert.ToInt32(ss[1]));
        }
        private static int Part1(char[,] grid)
        {
            return -1;
        }
        private static int Part2(char[,] grid)
        {
            return -1;
        }
    }
}