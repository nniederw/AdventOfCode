namespace AOC2024
{
    public class UnbuiltGraph<T>
    {
        private Func<T, IEnumerable<T>> GetOutgoingEdges = (i => Enumerable.Empty<T>());
        private IEnumerable<int> GetEdgesIndexed(int ind) => GetOutgoingEdges(Nodes[ind]).Select(node => GetIndex[node]);
        private T[] Nodes;
        private Dictionary<T, int> GetIndex = new();
        private int[] Distances = null;
        public UnbuiltGraph(IEnumerable<T> nodes, Func<T, IEnumerable<T>> getOutgoingEdges)
        {
            Nodes = nodes.ToArray();
            GetOutgoingEdges = getOutgoingEdges;
            for (int i = 0; i < Nodes.Length; i++)
            {
                GetIndex.Add(Nodes[i], i);
            }
        }
        public IEnumerable<T> BFSEnumerable(T start)
            => BFSEnumerableIndexed(GetIndex[start]).Select(i => Nodes[i]);
        private IEnumerable<int> BFSEnumerableIndexed(int start)
        {
            Distances = new int[Nodes.Length];
            HashSet<int> visited = new HashSet<int>();
            List<int> runningNodes = new() { start };
            int distance = 0;
            while (runningNodes.Count > 0)
            {
                foreach (var node in runningNodes)
                {
                    yield return node;
                    visited.Add(node);
                    Distances[node] = distance;
                }
                List<int> newNodes = new();
                foreach (var node in runningNodes)
                {
                    foreach (var edge in GetEdgesIndexed(node).Where(i => !visited.Contains(i)))
                    {
                        newNodes.Add(edge);
                    }
                }
                runningNodes = newNodes;
                distance++;
            }
        }
        public void BFS(T start)
            => BFSIndexed(GetIndex[start]);
        private void BFSIndexed(int start)
        {
            Distances = new int[Nodes.Length];
            HashSet<int> visited = new HashSet<int>();
            List<int> runningNodes = new() { start };
            int distance = 0;
            while (runningNodes.Count > 0)
            {
                foreach (var node in runningNodes)
                {
                    visited.Add(node);
                    Distances[node] = distance;
                }
                List<int> newNodes = new();
                foreach (var node in runningNodes)
                {
                    foreach (var edge in GetEdgesIndexed(node).Where(i => !visited.Contains(i)))
                    {
                        newNodes.Add(edge);
                    }
                }
                runningNodes = newNodes;
                distance++;
            }
        }
        public bool IsReachable(T start, T end)
            => IsReachableIndexed(GetIndex[start], GetIndex[end]);
        private bool IsReachableIndexed(int start, int end)
            => BFSEnumerableIndexed(start).Any(i => i == end);
        public int GetDistance(T start, T end)
            => GetDistanceIndexed(GetIndex[start], GetIndex[end]);
        private int GetDistanceIndexed(int start, int end)
        {
            BFSIndexed(start);
            return Distances[end];
        }
        public IEnumerable<T> GetShortestPath(T start, T end)
            => MapToNodes(GetShortestPathIndexed(GetIndex[start], GetIndex[end]));
        private List<int> GetShortestPathIndexed(int start, int end)
        {
            bool foundEnd = false;
            var result = BFSEnumerableIndexed(start).TakeWhile(i =>
            {
                if (foundEnd)
                {
                    return false;
                }
                if (i == end)
                {
                    foundEnd = true;
                }
                return true;
            }).ToList();
            if (!foundEnd) { throw new Exception($"{nameof(GetShortestPathIndexed)} was called on {nameof(UnbuiltGraph<T>)}, but the start and end were not connected"); }
            return result;
        }
        private IEnumerable<T> MapToNodes(IEnumerable<int> indexes)
            => indexes.Select(i => Nodes[i]);
        /*public IEnumerable<IEnumerable<T>> GetAllPaths(T start, T end)
            => GetAllPaths(GetIndex[start], GetIndex[end], new()).Select(i => MapToNodes(i));
        private IEnumerable<IEnumerable<int>> GetAllPaths(int start, int end, HashSet<int> visited)
        {
            if (visited.Contains(start)) yield break;
            var neighbors = GetEdgesIndexed(start).Where(i => !visited.Contains(i)).ToList();
            if (start == end)
            {
                yield return new List<int>() { start };
            }
            else
            {
                HashSet<int> newVisited = visited.ToHashSet();
                newVisited.Add(start);
                foreach (var neighbor in neighbors)
                {
                    var paths = GetAllPaths(neighbor, end, newVisited);
                    foreach (var path in paths)
                    {
                        yield return path.Prepend(start);
                    }
                }
            }
        }*/
    }
}
public class BuiltGraph<T>
{
    private HashSet<int>[] Edges; //Edges[i] are the outgoing edges of node at index i
    private T[] Nodes;
    private Dictionary<T, int> GetIndex = new();
    private int[] Distances = null;
    public BuiltGraph(IEnumerable<T> nodes)
    {
        Nodes = nodes.ToArray();
        Edges = new HashSet<int>[Nodes.Length];
        for (int i = 0; i < Nodes.Length; i++)
        {
            GetIndex.Add(Nodes[i], i);
            Edges[i] = new();
        }
    }
    public void AddEdgeRange(IEnumerable<(T v, T u)> edges)
    {
        AddEdgeRangeOnedirectional(edges);
        AddEdgeRangeOnedirectional(edges.Select(i => (i.u, i.v)));
    }
    public void AddEdge(T v, T u)
    {
        AddEdgeOnedirectional(v, u);
        AddEdgeOnedirectional(u, v);
    }
    private void AddEdgeIndexed(int v, int u)
    {
        AddEdgeOnedirectionalIndexed(v, u);
        AddEdgeOnedirectionalIndexed(u, v);
    }
    public void AddEdgeRangeOnedirectional(IEnumerable<(T from, T to)> edges)
    {
        foreach (var edge in edges)
        {
            AddEdgeOnedirectional(edge.from, edge.to);
        }
    }
    public void AddEdgeOnedirectional(T from, T to)
    {
        AddEdgeOnedirectionalIndexed(GetIndex[from], GetIndex[to]);
    }
    private void AddEdgeOnedirectionalIndexed(int from, int to)
    {
        Edges[from].Add(to);
    }
    public IEnumerable<T> BFSEnumerable(T start)
        => BFSEnumerableIndexed(GetIndex[start]).Select(i => Nodes[i]);
    private IEnumerable<int> BFSEnumerableIndexed(int start)
    {
        Distances = new int[Nodes.Length];
        HashSet<int> visited = new HashSet<int>();
        List<int> runningNodes = new() { start };
        int distance = 0;
        while (runningNodes.Count > 0)
        {
            foreach (var node in runningNodes)
            {
                yield return node;
                visited.Add(node);
                Distances[node] = distance;
            }
            List<int> newNodes = new();
            foreach (var node in runningNodes)
            {
                foreach (var edge in Edges[node].Where(i => !visited.Contains(i)))
                {
                    newNodes.Add(edge);
                }
            }
            runningNodes = newNodes;
            distance++;
        }
    }
    public void BFS(T start)
        => BFSIndexed(GetIndex[start]);
    private void BFSIndexed(int start)
    {
        Distances = new int[Nodes.Length];
        HashSet<int> visited = new HashSet<int>();
        List<int> runningNodes = new() { start };
        int distance = 0;
        while (runningNodes.Count > 0)
        {
            foreach (var node in runningNodes)
            {
                visited.Add(node);
                Distances[node] = distance;
            }
            List<int> newNodes = new();
            foreach (var node in runningNodes)
            {
                foreach (var edge in Edges[node].Where(i => !visited.Contains(i)))
                {
                    newNodes.Add(edge);
                }
            }
            runningNodes = newNodes;
            distance++;
        }
    }
    public bool IsReachable(T start, T end)
        => IsReachableIndexed(GetIndex[start], GetIndex[end]);
    private bool IsReachableIndexed(int start, int end)
        => BFSEnumerableIndexed(start).Any(i => i == end);
    public int GetDistance(T start, T end)
        => GetDistanceIndexed(GetIndex[start], GetIndex[end]);
    private int GetDistanceIndexed(int start, int end)
    {
        BFSIndexed(start);
        return Distances[end];
    }
    private IEnumerable<T> MapToNodes(IEnumerable<int> indexes)
        => indexes.Select(i => Nodes[i]);
    /*public IEnumerable<IEnumerable<T>> GetAllPaths(T start, T end)
        => GetAllPaths(GetIndex[start], GetIndex[end], new()).Select(i => MapToNodes(i));
    private IEnumerable<IEnumerable<int>> GetAllPaths(int start, int end, HashSet<int> visited)
    {
        if (visited.Contains(start)) yield break;
        var neighbors = Edges[start].Where(i => !visited.Contains(i)).ToList();
        if (start == end)
        {
            yield return new List<int>() { start };
        }
        else
        {
            HashSet<int> newVisited = visited.ToHashSet();
            newVisited.Add(start);
            foreach (var neighbor in neighbors)
            {
                var paths = GetAllPaths(neighbor, end, newVisited);
                foreach (var path in paths)
                {
                    yield return path.Prepend(start);
                }
            }
        }
    }*/
}