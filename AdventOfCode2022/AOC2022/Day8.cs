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
            for (int i = 0; i < ; i++)
            {

            }

            return false;
        }
        private static int Part1(int[,] input)
        {
            return -1;
        }

        private static int Part2(int[,] input)
        {
            return -1;
        }
    }
}