namespace AOC2022
{
    class Day9
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day9.txt");
            List<(char, int)> input = new();
            foreach (var line in lines)
            {
                input.Add((line[0], Convert.ToInt32(line.Split(' ')[1])));
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private static int Part1(List<(char, int)> input)
        {
            HashSet<(int, int)> visited = new();
            int x = 0;
            int y = 0;
            int tailx = 0;
            int taily = 0;
            visited.Add((x, y));
            foreach (var line in input)
            {
                for (int i = 0; i < line.Item2; i++)
                {
                    switch (line.Item1)
                    {
                        case 'R':
                            x++;
                            break;
                        case 'L':
                            x--;
                            break;
                        case 'U':
                            y++;
                            break;
                        case 'D':
                            y--;
                            break;
                        default:
                            throw new Exception();
                    }
                    int dx = x - tailx;
                    int dy = y - taily;
                    if (Math.Abs(dx) < 2 && Math.Abs(dy) < 2)
                    {
                        continue;
                    }
                    if (dx == 0)
                    {
                        taily += dy / 2;
                    }
                    else if (dy == 0)
                    {
                        tailx += dx / 2;
                    }
                    else
                    {
                        tailx += Math.Abs(dx) == 1 ? dx : dx / 2;
                        taily += Math.Abs(dy) == 1 ? dy : dy / 2;
                    }
                    visited.Add((tailx, taily));
                }
            }
            return visited.Count;
        }
        private static int Part2(List<(char, int)> input)
        {
            HashSet<(int, int)> visited = new();
            int[] posx = new int[10];
            int[] posy = new int[10];
            foreach (var line in input)
            {
                for (int i = 0; i < line.Item2; i++)
                {
                    switch (line.Item1)
                    {
                        case 'R':
                            posx[0]++;
                            break;
                        case 'L':
                            posx[0]--;
                            break;
                        case 'U':
                            posy[0]++;
                            break;
                        case 'D':
                            posy[0]--;
                            break;
                        default:
                            throw new Exception();
                    }
                    for (int j = 1; j < 10; j++)
                    {
                        int dx = posx[j - 1] - posx[j];
                        int dy = posy[j - 1] - posy[j];
                        if (Math.Abs(dx) < 2 && Math.Abs(dy) < 2)
                        {
                            continue;
                        }
                        if (dx == 0)
                        {
                            posy[j] += dy / 2;
                        }
                        else if (dy == 0)
                        {
                            posx[j] += dx / 2;
                        }
                        else
                        {
                            posx[j] += Math.Abs(dx) == 1 ? dx : dx / 2;
                            posy[j] += Math.Abs(dy) == 1 ? dy : dy / 2;
                        }
                    }
                    visited.Add((posx[9], posy[9]));
                }
            }
            return visited.Count;
        }
    }
}