namespace AOC2022
{
    class Day8
    {
        private static int[,] Grid;
        private static int N;
        private static int M;
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day8.txt");
            int n = lines.Count();
            int m = lines.First().Length;
            int[,] input = new int[n, m];
            int i = 0;
            foreach (var line in lines)
            {
                for (int j = 0; j < m; j++)
                {
                    input[i, j] = line[j];
                }
                i++;
            }
            Grid = input;
            N = n;
            M = m;
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private static bool Visible(int[,] grid, int x, int y)
        {
            int height = grid[x, y];
            bool visible = true;
            for (int i = 0; i < x; i++)
            {
                if (height <= grid[i, y])
                {
                    visible = false;
                }
            }
            if (visible) return true;
            visible = true;
            for (int i = x + 1; i < N; i++)
            {
                if (height <= grid[i, y])
                {
                    visible = false;
                }
            }
            if (visible) return true;
            visible = true;
            for (int i = 0; i < y; i++)
            {
                if (height <= grid[x, i])
                {
                    visible = false;
                }
            }
            if (visible) return true;
            visible = true;
            for (int i = y + 1; i < M; i++)
            {
                if (height <= grid[x, i])
                {
                    visible = false;
                }
            }
            //if (visible) return true;
            return visible;
        }
        private static int Part1(int[,] input)
        {
            int sum = 0;
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < M; y++)
                {
                    if (Visible(input, x, y))
                    {
                        sum++;
                        //Console.WriteLine($"x {x}, y {y}");
                    }
                }
            }
            return sum;
        }
        private static int Score(int x, int y)
        {
            int score = 1;
            int height = Grid[x, y];
            int distance = 0;
            for (int i = x - 1; i >= 0; i--, distance++)
            {
                if (height <= Grid[i, y])
                {
                    distance++;
                    break;
                }
            }
            score *= distance;
            distance = 0;
            for (int i = x + 1; i < N; i++, distance++)
            {
                if (height <= Grid[i, y])
                {
                    distance++;
                    break;
                }
            }
            score *= distance;
            distance = 0;
            for (int i = y - 1; i >= 0; i--, distance++)
            {
                if (height <= Grid[x, i])
                {
                    distance++;
                    break;
                }
            }
            score *= distance;
            distance = 0;
            for (int i = y + 1; i < M; i++, distance++)
            {
                if (height <= Grid[x, i])
                {
                    distance++;
                    break;
                }
            }
            score *= distance;
            return score;
        }
        private static int Part2(int[,] input)
        {
            int max = 0;
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < M; y++)
                {
                    int s = Score(x, y);
                    max = Math.Max(max, s);
                }
            }
            return max;
        }
    }
}