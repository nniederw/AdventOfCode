namespace AOC2022
{
    class Day12
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day12.txt");
            int[,] input = null;
            int linenumber = 0;
            var n = lines.Count();
            lines = File.ReadLines($"{Global.InputPath}/Day12.txt");
            foreach (var line in lines)
            {
                if (input == null)
                {
                    input = new int[n, line.Length];
                }
                for (int i = 0; i < line.Length; i++)
                {
                    input[linenumber, i] = line[i];
                }
                linenumber++;
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
        private static Graph GenerateGraph(int[,] input)
        {
            var G = new Graph(input.GetLength(0) * input.GetLength(1));
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    int height = input[i, j];
                    try
                    {
                        if (input[i - 1, j] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i - 1, j));
                        }
                    }
                    catch (Exception e) { }
                    try
                    {
                        if (input[i + 1, j] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i + 1, j));
                        }
                    }
                    catch (Exception e) { }
                    try
                    {
                        if (input[i, j - 1] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i, j - 1));
                        }
                    }
                    catch (Exception e) { }
                    try
                    {
                        if (input[i, j + 1] <= height + 1)
                        {
                            G.AddEdge(GetIndex(i, j), GetIndex(i, j + 1));
                        }
                    }
                    catch (Exception e) { }
                }
            }
            return G;
            int GetIndex(int i, int j) => i * input.GetLength(1) + j;
        }
        private static int Part1(int[,] input, (int, int) S, (int, int) E)
        {
            Graph G = GenerateGraph(input);
            GlobalGraph = G;
            G.BFS(GetIndex(S.Item1, S.Item2));
            return G.Distance[GetIndex(E.Item1, E.Item2)];
            int GetIndex(int i, int j) => i * input.GetLength(1) + j;
            int GetIndexT((int, int) ind) => GetIndex(ind.Item1, ind.Item2);
        }
        static Graph GlobalGraph;
        private static int Part2(int[,] input, (int, int) S, (int, int) E)
        {
            Graph G = GlobalGraph.GetReverseEdges();
            G.BFS(GetIndexT(E));
            int min = int.MaxValue;
            for (int j = 0; j < input.GetLength(0); j++)
            {
                for (int i = 0; i < input.GetLength(1); i++)
                {
                    if (input[j, i] == 'a')
                    {
                        min = Math.Min(min, G.Distance[GetIndex(j, i)]);
                    }
                }
            }
            return min;
            int GetIndex(int i, int j) => i * input.GetLength(1) + j;
            int GetIndexT((int, int) ind) => GetIndex(ind.Item1, ind.Item2);
        }
    }
    class Graph
    {
        public int N;
        public int M;
        public List<int>[] Edges;// edges from i = Edges[i]
        public int[] Distance;
        public bool[] Visited;
        public Graph(int n)
        {
            this.N = n;
            Edges = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                Edges[i] = new();
            }
            Visited = new bool[n];
        }
        public Graph GetReverseEdges()
        {
            var G = new Graph(N);
            for (int i = 0; i < N; i++)
            {
                foreach (var edge in Edges[i])
                {
                    G.AddEdge(edge, i);
                }
            }
            return G;
        }
        public void AddEdge(int v, int u)
        {
            Edges[v].Add(u);
            M++;
        }
        public void BFS(int v)
        {
            Distance = new int[N];
            for (int i = 0; i < N; i++)
            {
                Distance[i] = int.MaxValue;
            }
            Distance[v] = 0;
            HashSet<int> S = new HashSet<int>();
            foreach (var edge in Edges[v])
            {
                S.Add(edge);
            }
            int distance = 1;
            while (S.Any())
            {
                var newS = new HashSet<int>();
                foreach (var vertex in S)
                {
                    if (Distance[vertex] > distance)
                    {
                        Distance[vertex] = distance;
                        foreach (var edge in Edges[vertex])
                        {
                            newS.Add(edge);
                        }
                    }
                }
                distance++;
                S = newS;
            }
        }
        public void DFS(int v)
        {
            Visited[v] = true;
            foreach (var edge in Edges[v])
            {
                if (!Visited[edge])
                {
                    DFS(edge);
                }
            }
        }
    }
}