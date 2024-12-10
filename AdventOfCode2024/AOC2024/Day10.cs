namespace AOC2024
{
    class Day10 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            int[,] heightMap = Input.InterpretStringsAs2DIntArray(lines);
            int MapWidth = heightMap.GetLength(0);
            int MapHeight = heightMap.GetLength(1);
            Console.WriteLine(Part1(heightMap, MapWidth, MapHeight));
            Console.WriteLine(Part2(heightMap, MapWidth, MapHeight));
        }
        private bool InBound(int x, int y, int MapWidth, int MapHeight)
           => 0 <= x && x < MapWidth && 0 <= y && y < MapHeight;
        private IEnumerable<(int x, int y)> IncreasingNeighbors(int[,] heightMap, int MapWidth, int MapHeight, int x, int y)
        {
            int height = heightMap[x, y];
            var neighbors = new List<(int x, int y)>() { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
            return neighbors.Where(i => InBound(i.x, i.y, MapWidth, MapHeight) && heightMap[i.x, i.y] == height + 1);
        }
        private IEnumerable<(int x, int y)> ReachablePoints(int[,] heightMap, int MapWidth, int MapHeight, int x, int y)
        {
            HashSet<(int x, int y)> FoundBefore = new();
            List<(int x, int y)> WorkingPoints = new() { (x, y) };
            while (WorkingPoints.Count > 0)
            {
                foreach (var point in WorkingPoints)
                {
                    yield return point;
                    FoundBefore.Add(point);
                }
                List<(int x, int y)> newWorkingPoints = new();
                foreach (var points in WorkingPoints.Select(i => IncreasingNeighbors(heightMap, MapWidth, MapHeight, i.x, i.y)))
                {
                    newWorkingPoints.AddRange(points.Where(i => !FoundBefore.Contains(i)));
                }
                WorkingPoints = newWorkingPoints.Distinct().ToList();
            }
        }
        private int Part1(int[,] heightMap, int MapWidth, int MapHeight)
        {
            int trailheads = 0;
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    int height = heightMap[x, y];
                    if (height == 0)
                    {
                        trailheads += ReachablePoints(heightMap, MapWidth, MapHeight, x, y)
                            .Count(i => heightMap[i.x, i.y] == 9);
                    }
                }
            }
            return trailheads;
        }
        private IEnumerable<IEnumerable<(int x, int y)>> GetAllPaths(int[,] heightMap, int MapWidth, int MapHeight, int x, int y)
        {
            var neighbors = IncreasingNeighbors(heightMap, MapWidth, MapHeight, x, y).ToList();
            if (neighbors.Count == 0)
            {
                yield return new List<(int x, int y)>() { (x, y) };
            }
            else
            {
                foreach (var neighbor in neighbors)
                {
                    var paths = GetAllPaths(heightMap, MapWidth, MapHeight, neighbor.x, neighbor.y);
                    foreach (var path in paths)
                    {
                        yield return path.Prepend((x, y));
                    }
                }
            }
        }
        private int Part2(int[,] heightMap, int MapWidth, int MapHeight)
        {
            int trailheadRatings = 0;
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    int height = heightMap[x, y];
                    if (height == 0)
                    {
                        int newtrailheadRatings = GetAllPaths(heightMap, MapWidth, MapHeight, x, y)
                            .Count(i =>
                            {
                                var last = i.Last();
                                return heightMap[last.x, last.y] == 9;
                            });
                        trailheadRatings += newtrailheadRatings;
                    }
                }
            }
            return trailheadRatings;
        }
    }
}