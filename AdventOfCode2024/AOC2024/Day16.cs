using System.ComponentModel.DataAnnotations;

namespace AOC2024
{
    class Day16 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        private const long TurnCost = 1000;
        private const long MoveCost = 1;
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            char[,] map = Input.InterpretStringsAs2DCharArray(lines);
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            Console.WriteLine(Part1(map, width, height));
            //Console.WriteLine(Part2(map, width, height));
            Console.WriteLine($"{527} (Precomputed)");
        }
        private (int x, int y) GetPosOfChar(char[,] map, int width, int height, char c)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] == c)
                    {
                        return (x, y);
                    }
                }
            }
            throw new Exception($"Char {c} wasn't found with the function {nameof(GetPosOfChar)}");
        }
        private (int x, int y) GetStartPos(char[,] map, int width, int height) => GetPosOfChar(map, width, height, 'S');
        private (int x, int y) GetEndPos(char[,] map, int width, int height) => GetPosOfChar(map, width, height, 'E');
        private const char Wall = '#';
        private bool IsWall(char c) => c == Wall;
        private (int dx, int dy) Rotate((int dx, int dy) dir)
        {
            switch (dir)
            {
                case (1, 0):
                    return (0, -1);
                case (0, -1):
                    return (-1, 0);
                case (-1, 0):
                    return (0, 1);
                case (0, 1):
                    return (1, 0);
            }
            throw new NotImplementedException();
        }
        private IEnumerable<((int x, int y) pos, (int dx, int dy) dir, long score)> GetNeighbors(char[,] map, (int x, int y) pos, (int dx, int dy) dir)
        {

            if (!IsWall(map[pos.x + dir.dx, pos.y + dir.dy]))
            {
                yield return ((pos.x + dir.dx, pos.y + dir.dy), dir, MoveCost);
            }
            dir = Rotate(dir);
            if (!IsWall(map[pos.x + dir.dx, pos.y + dir.dy]))
            {
                yield return ((pos.x + dir.dx, pos.y + dir.dy), dir, MoveCost + TurnCost);
            }
            dir = Rotate(dir);
            if (!IsWall(map[pos.x + dir.dx, pos.y + dir.dy]))
            {
                yield return ((pos.x + dir.dx, pos.y + dir.dy), dir, MoveCost + TurnCost * 2);
            }
            dir = Rotate(dir);
            if (!IsWall(map[pos.x + dir.dx, pos.y + dir.dy]))
            {
                yield return ((pos.x + dir.dx, pos.y + dir.dy), dir, MoveCost + TurnCost);
            }
        }
        private long GetLowestScore(char[,] map, (int x, int y) start, (int dx, int dy) startDir, (int x, int y) end)
        {

            Dictionary<((int x, int y) pos, (int dx, int dy) dir), long> Costs = new();
            PriorityQueue<((int x, int y) pos, (int dx, int dy) dir, long score), long> running = new();
            running.Enqueue((start, startDir, 0), 0);
            Costs.Add((start, startDir), 0);
            while (running.Count > 0)
            {
                var last = running.Dequeue();
                IEnumerable<((int x, int y) pos, (int dx, int dy) dir, long score)> neighbors = GetNeighbors(map, last.pos, last.dir).Select(i => (i.pos, i.dir, i.score + last.score));
                foreach (var neighbor in neighbors)
                {
                    var key = (neighbor.pos, neighbor.dir);
                    if (Costs.ContainsKey(key))
                    {
                        if (Costs[key] > neighbor.score)
                        {
                            Costs[key] = neighbor.score;
                            running.Enqueue(neighbor, neighbor.score);
                        }
                    }
                    else
                    {
                        Costs.Add(key, neighbor.score);
                        running.Enqueue(neighbor, neighbor.score);
                    }
                }
            }
            return Costs.Where(i => i.Key.pos == end).Min(i => i.Value);
        }
        private long Part1(char[,] map, int width, int height)
        {
            int startX, startY;
            int endX, endY;
            (startX, startY) = GetStartPos(map, width, height);
            (endX, endY) = GetEndPos(map, width, height);
            int dx = 1;
            int dy = 0;
            return GetLowestScore(map, (startX, startY), (dx, dy), (endX, endY));
        }
        private IEnumerable<IEnumerable<(int x, int y)>> GetLowestScorePaths(char[,] map, (int x, int y) start, (int dx, int dy) startDir, (int x, int y) end)
        {
            Dictionary<((int x, int y) pos, (int dx, int dy) dir), long> Costs = new();
            Dictionary<((int x, int y) pos, (int dx, int dy) dir), List<List<(int x, int y)>>> PreviousNodes = new();
            PriorityQueue<((int x, int y) pos, (int dx, int dy) dir, long score), long> running = new();
            List<List<(int x, int y)>> emptyStart = new();
            emptyStart.Add(new());
            PreviousNodes.Add((start, startDir), emptyStart);
            running.Enqueue((start, startDir, 0), 0);
            Costs.Add((start, startDir), 0);
            while (running.Count > 0)
            {
                var last = running.Dequeue();
                IEnumerable<((int x, int y) pos, (int dx, int dy) dir, long score)> neighbors = GetNeighbors(map, last.pos, last.dir).Select(i => (i.pos, i.dir, i.score + last.score));
                foreach (var neighbor in neighbors)
                {
                    var key = (neighbor.pos, neighbor.dir);
                    if (Costs.ContainsKey(key))
                    {
                        if (Costs[key] > neighbor.score)
                        {
                            Costs[key] = neighbor.score;
                            PreviousNodes[key].Clear();
                            PreviousNodes[key].AddRange(PreviousNodes[(last.pos, last.dir)].Select(i => i.Append(last.pos).ToList()));
                            running.Enqueue(neighbor, neighbor.score);
                        }
                        if (Costs[key] == neighbor.score)
                        {
                            PreviousNodes[key].AddRange(PreviousNodes[(last.pos, last.dir)].Select(i => i.Append(last.pos).ToList()));
                        }
                    }
                    else
                    {
                        Costs.Add(key, neighbor.score);
                        PreviousNodes.Add(key, new List<List<(int x, int y)>>(PreviousNodes[(last.pos, last.dir)].Select(i => i.Append(last.pos).ToList())));
                        running.Enqueue(neighbor, neighbor.score);
                    }
                }
            }
            var endPaths = PreviousNodes.Where(i => i.Key.pos == end).ToList();
            long minscore = endPaths.Min(i => Costs[i.Key]);
            foreach (var path in endPaths.Where(i => Costs[i.Key] == minscore))
            {
                foreach (var nodes in path.Value)
                {
                    yield return nodes;
                }
            }
        }
        private long Part2(char[,] map, int width, int height)
        {
            int startX, startY;
            int endX, endY;
            (startX, startY) = GetStartPos(map, width, height);
            (endX, endY) = GetEndPos(map, width, height);
            int dx = 1;
            int dy = 0;
            var paths = GetLowestScorePaths(map, (startX, startY), (dx, dy), (endX, endY));
            HashSet<(int x, int y)> Visited = new();
            foreach (var path in paths)
            {
                foreach (var node in path)
                {
                    Visited.Add(node);
                }
            }
            Visited.Add((endX, endY));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (Visited.Contains((x, y)))
                    {
                        Console.Write('0');
                        continue;
                    }
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
            return Visited.Count;
        }
    }
}