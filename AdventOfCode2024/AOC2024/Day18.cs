using System.Runtime.CompilerServices;

namespace AOC2024
{
    class Day18 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        private const int GridSize = 71;
        private const int FirstFewBytes = 1024;
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<(int x, int y)> FallingBytes = new();
            foreach (var line in lines)
            {
                var split = line.Split(',');
                FallingBytes.Add((Convert.ToInt32(split[0]), Convert.ToInt32(split[1])));
            }
            Console.WriteLine(Part1(FallingBytes));
            Console.WriteLine(Part2(FallingBytes));
        }
        private bool InBound(int x, int y, int width, int height)
          => 0 <= x && x < width && 0 <= y && y < height;
        private IEnumerable<(int x, int y)> GetNeighbors(bool[,] map, int width, int height, int x, int y)
        {
            var neighbors = new List<(int x, int y)>() { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
            return neighbors.Where(i => InBound(i.x, i.y, width, height)).Where(i => !map[i.x, i.y]);
        }
        private int? GetShortestDistance(bool[,] map, int width, int height, (int x, int y) start, (int x, int y) end)
        {
            HashSet<(int x, int y)> Visited = new();
            Dictionary<(int x, int y), int> Distances = new();
            List<(int x, int y)> WorkingPos = new List<(int x, int y)>() { start };
            int distance = 0;
            while (WorkingPos.Any())
            {
                foreach (var pos in WorkingPos)
                {
                    Visited.Add(pos);
                    Distances.Add(pos, distance);
                }
                List<(int x, int y)> NewWorkingPos = new();
                var neighborss = WorkingPos.Select(i => GetNeighbors(map, width, height, i.x, i.y));
                foreach (var neighbors in neighborss)
                {
                    foreach (var neighbor in neighbors)
                    {
                        if (!Visited.Contains(neighbor))
                        {
                            Visited.Add(neighbor);
                            NewWorkingPos.Add(neighbor);
                        }
                    }
                }
                WorkingPos = NewWorkingPos;
                distance++;
            }
            if (Distances.ContainsKey(end))
            {
                return Distances[end];
            }
            return null;
        }
        private void PrintMap(bool[,] map, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(map[x, y] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }
        private int Part1(IReadOnlyList<(int x, int y)> FallingBytes)
        {
            bool[,] map = new bool[GridSize, GridSize];
            foreach (var pos in FallingBytes.Take(FirstFewBytes))
            {
                map[pos.x, pos.y] = true;
            }
            PrintMap(map, GridSize, GridSize);
            return GetShortestDistance(map, GridSize, GridSize, (0, 0), (GridSize - 1, GridSize - 1)).Value;
        }
        private string Part2(IReadOnlyList<(int x, int y)> FallingBytes)
        {
            bool[,] map = new bool[GridSize, GridSize];
            foreach (var pos in FallingBytes.Take(FirstFewBytes))
            {
                map[pos.x, pos.y] = true;
            }
            for (int i = 0; i < FallingBytes.Count; i++)
            {
                var pos = FallingBytes[i];
                map[pos.x, pos.y] = true;
                var dist = GetShortestDistance(map, GridSize, GridSize, (0, 0), (GridSize - 1, GridSize - 1));
                if (dist == null)
                {
                    return $"{pos.x},{pos.y}";
                }
            }
            return "Not found";
        }
    }
}