namespace AOC2022
{
    class Day12
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day12.txt");
            int[,] input = null;
            foreach (var line in lines)
            {
                if (input == null)
                {
                    input = new int[lines.Count(), line.Length];
                }
            }
            var S = FindS(input);
            var E = FindE(input);
            Console.WriteLine(Part1(input, S, E));
            Console.WriteLine(Part2(input, S, E));
        }
        private static (int, int) FindS(int[,] input)
        {
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    if (input[i, j] == 'S')
                    {
                        input[i, j] = 'a';
                        return (i, j);
                    }
                }
            }
            throw new Exception();
        }
        private static (int, int) FindE(int[,] input)
        {
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    if (input[i, j] == 'E')
                    {
                        input[i, j] = 'z';
                        return (i, j);
                    }
                }
            }
            throw new Exception();
        }
        private static int Part1(int[,] input, (int, int) S, (int, int) E)
        {
            var G = new Graph(input.GetLength(0) * input.GetLength(1));
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(j); j++)
                {
                    int height = input[i, j];
                    try
                    {
                        if (height - 1 <= input[i - 1, j] || input[i - 1, j] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i - 1, j));
                        }
                    }
                    catch (Exception e) { }
                    try
                    {
                        if (height - 1 <= input[i + 1, j] || input[i + 1, j] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i + 1, j));
                        }
                    }
                    catch (Exception e) { }
                    try
                    {
                        if (height - 1 <= input[i, j - 1] || input[i, j - 1] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i, j - 1));
                        }
                    }
                    catch (Exception e) { }
                    try
                    {
                        if (height - 1 <= input[i, j + 1] || input[i, j + 1] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i, j + 1));
                        }
                    }
                    catch (Exception e) { }
                }
                int GetIndex(int i, int j)
                {
                    return i * input.GetLength(1) + j;
                }

            }


            return -1;
        }
        private static int Part2(int[,] input, (int, int) S, (int, int) E)
        {
            return -1;
        }
    }
    class Graph
    {
        public int n;
        public int m;
        public List<int>[] Edges;// edges from i = Edges[i]
        public int[] Distance;
        public Graph(int n)
        {
            Edges = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                Edges[i] = new();
            }
        }
        public void AddEdge(int v, int u)
        {
            Edges[v].Add(u);
        }
        public void BFS(int v)
        {
            Distance = new int[n];
            for (int i = 0; i < n; i++)
            {
                Distance[i] = int.MaxValue;
            }
            Distance[v] = 0;
            HashSet<int> S = new HashSet<int>();
            foreach (var edge in Edges[v])
            {
                S.Add(edge);
            }
            while (S.Any())
            {
                var newS = new HashSet<int>();
                //todo
            }
        }
    }
}