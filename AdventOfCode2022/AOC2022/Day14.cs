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
            int maxX = 500 + Rocks.MaxValue(i => Math.Max(i.Item1.x, i.Item2.x));
            int maxY = 3 + Rocks.MaxValue(i => Math.Max(i.Item1.y, i.Item2.y));
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
                grid[x, y] = '#';
            }
            int part1 = Part1(grid);
            Console.WriteLine(part1);
            Console.WriteLine(Part2(grid, part1));
        }
        private static (int, int) GetPos(string s)
        {
            var ss = s.Split(',');
            return (Convert.ToInt32(ss[0]), Convert.ToInt32(ss[1]));
        }
        private static int Part1(char[,] grid)
        {
            int sandX = 500;
            int sandY = 0;
            int maxY = grid.GetLength(1) - 1;
            int i = 0;
            while (true)
            {
                if (sandY == maxY)
                {
                    break;
                }
                if (grid[sandX, sandY + 1] == '.')
                {
                    sandY++;
                }
                else if (grid[sandX - 1, sandY + 1] == '.')
                {
                    sandX--;
                    sandY++;
                }
                else if (grid[sandX + 1, sandY + 1] == '.')
                {
                    sandX++;
                    sandY++;
                }
                else
                {
                    grid[sandX, sandY] = 'o';
                    sandX = 500;
                    sandY = 0;
                    i++;
                }
            }
            return i;
        }
        private static void Print(char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                for (int j = 400; j < grid.GetLength(0) && j < 600; j++)
                {
                    Console.Write(Grid(j, i));
                }
                Console.WriteLine();
            }
            char Grid(int x, int y)
            {
                if (y == grid.GetLength(1) - 1) return '#';
                return grid[x, y];
            }
        }
        private static int Part2(char[,] grid, int iterationsPart1)
        {
            int sandX = 500;
            int sandY = 0;
            int maxY = grid.GetLength(1) - 1;
            int i = 0;
            while (true)
            {
                if (grid[500, 0] == 'o') break;
                if (Grid(sandX, sandY + 1) == '.')
                {
                    sandY++;
                }
                else if (Grid(sandX - 1, sandY + 1) == '.')
                {
                    sandX--;
                    sandY++;
                }
                else if (Grid(sandX + 1, sandY + 1) == '.')
                {
                    sandX++;
                    sandY++;
                }
                else
                {
                    grid[sandX, sandY] = 'o';
                    sandX = 500;
                    sandY = 0;
                    i++;
                }
            }
            char Grid(int x, int y)
            {
                if (y == maxY) return '#';
                return grid[x, y];
            }
            return i + iterationsPart1;
        }
    }
}