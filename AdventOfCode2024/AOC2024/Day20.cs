namespace AOC2024
{
    class Day20 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            char[,] map = Input.InterpretStringsAs2DCharArray(lines);
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            //Console.WriteLine(Part1(map, width, height));
            Console.WriteLine(Part2(map, width, height));
        }
        private const char EmptySpace = '.';
        private const char Wall = '#';
        private const char StartChar = 'S';
        private const char EndChar = 'E';
        private bool IsEmptySpace(char c) => c == EmptySpace;
        private bool IsWall(char c) => c == Wall;
        private bool IsStart(char c) => c == StartChar;
        private bool IsEnd(char c) => c == EndChar;
        private ((int x, int y) start, (int x, int y) end) GetStartAndEndPosition(char[,] map, int width, int height)
        {
            (int x, int y) start = (-1, -1);
            (int x, int y) end = (-1, -1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsStart(map[x, y]))
                    {
                        start = (x, y);
                    }
                    if (IsEnd(map[x, y]))
                    {
                        end = (x, y);
                    }
                }
            }
            if ((new[] { start, end }).Contains((-1, -1)))
            {
                throw new Exception("Robot not found");
            }
            return (start, end);
        }
        private bool InBound(int x, int y, int width, int height)
            => 0 <= x && x < width && 0 <= y && y < height;
        private IEnumerable<(int x, int y)> GetInBoundNeighborsIgnoringWalls(char[,] map, int width, int height, int x, int y)
        {
            var neighbors = new List<(int x, int y)>() { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
            return neighbors.Where(i => InBound(i.x, i.y, width, height));
        }
        private IEnumerable<(int x, int y)> GetNeighbors(char[,] map, int width, int height, (int x, int y) pos)
            => GetNeighbors(map, width, height, pos.x, pos.y);
        private IEnumerable<(int x, int y)> GetNeighbors(char[,] map, int width, int height, int x, int y)
            => GetInBoundNeighborsIgnoringWalls(map, width, height, x, y).Where(i => !IsWall(map[i.x, i.y]));
        private IEnumerable<(int x, int y)> GetNeighborsIgnoringWall(char[,] map, int width, int height, int x, int y, (int x, int y) WallToIgnore)
            => GetInBoundNeighborsIgnoringWalls(map, width, height, x, y).Where(i =>
            {
                if (i == WallToIgnore) { return true; }
                return !IsWall(map[i.x, i.y]);
            });
        private IEnumerable<(int x, int y)> GetDoubleNeighborsIgnoringWalls(char[,] map, int width, int height, int x, int y)
        {
            return GetInBoundNeighborsIgnoringWalls(map, width, height, x, y)
                .Select(i => GetInBoundNeighborsIgnoringWalls(map, width, height, i.x, i.y))
                .SelectMany(i => i).Distinct();
        }
        private IEnumerable<(int x, int y)> GetThirdDegreeNeighborsIgnoringWalls(char[,] map, int width, int height, int x, int y)
        {
            return GetDoubleNeighborsIgnoringWalls(map, width, height, x, y)
                .Select(i => GetInBoundNeighborsIgnoringWalls(map, width, height, i.x, i.y))
                .SelectMany(i => i)
                .Distinct();
        }
        public int PicoSeconds(List<(int x, int y, bool BottomLayer)> path)
        {
            int pico = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var cur = path[i];
                var next = path[i + 1];
                if ((cur.x, cur.y) != (next.x, next.y))
                {
                    pico++;
                    if (Math.Abs(cur.x - next.x) + Math.Abs(cur.y - next.y) == 2)
                    {
                        pico++;
                    }
                }
            }
            return pico;
        }
        private bool PathsEqual(List<(int x, int y, bool BottomLayer)> path1, List<(int x, int y, bool BottomLayer)> path2)
        {
            var p1 = path1.Select(i => (i.x, i.y)).Distinct().ToList(); //distinct is to remove cases like (x,y,true) -> (x,y, false)
            var p2 = path2.Select(i => (i.x, i.y)).Distinct().ToList();
            if (p1.Count != p2.Count)
            {
                return false;
            }

            for (int i = 0; i < p1.Count; i++)
            {
                if (p1[i] != p2[i])
                {
                    return false;
                }
            }
            return true;
        }
        private int Distance(char[,] map, int width, int height, (int x, int y) start, (int x, int y) end)
        {
            var nodes = Enumerable.Range(1, width - 2).SelectMany(x => Enumerable.Range(1, height - 2), (x, y) => new { x, y }).Select(i => (i.x, i.y)).ToHashSet();
            var graph = new UnbuiltGraph<(int x, int y)>(nodes, node => GetNeighbors(map, width, height, node).Where(i => nodes.Contains(i)));
            return graph.GetDistance(start, end);
        }
        private int DistanceWhenWallRemoved(char[,] map, int width, int height, (int x, int y) Wall, (int x, int y) start, (int x, int y) end)
        {
            var nodes = Enumerable.Range(1, width - 2).SelectMany(x => Enumerable.Range(1, height - 2), (x, y) => new { x, y }).Select(i => (i.x, i.y)).ToHashSet();
            var graph = new UnbuiltGraph<(int x, int y)>(nodes,
                node =>
                 GetNeighborsIgnoringWall(map, width, height, node.x, node.y, Wall)
                 .Where(i => nodes.Contains(i))
                 );
            return graph.GetDistance(start, end);
            /*map = (char[,])map.Clone();
            map[Wall.x, Wall.y] = EmptySpace;
            return Distance(map, width, height, start, end);*/
        }
        private long Part1(char[,] map, int width, int height)
        {
            const int PicoThreshold = 100;
            (int x, int y) start, end;
            (start, end) = GetStartAndEndPosition(map, width, height);
            int LegitLenght = Distance(map, width, height, start, end);
            Console.WriteLine("Got Legit distance");
            List<int> CheatingPaths = new();
            List<(int x, int y)> WallsToCheck = new();
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (IsWall(map[x, y]))
                    {
                        WallsToCheck.Add((x, y));
                    }
                }
            }
            const int NumberOfThreads = 16;
            List<Task<int>> Tasks = new List<Task<int>>(NumberOfThreads);
            int notStartedIndex = 0;
            for (int i = 0; i < NumberOfThreads && i < WallsToCheck.Count; i++)
            {
                var wall = WallsToCheck[notStartedIndex];
                Console.WriteLine($"Starting Task for Wall ({wall.x},{wall.y})");
                Tasks.Add(Task.Run(() => DistanceWhenWallRemoved(map, width, height, wall, start, end)));
                notStartedIndex++;
            }
            while (notStartedIndex < WallsToCheck.Count)
            {
                var finishedTask = Task.WhenAny(Tasks).Result;
                int ind = Tasks.FindIndex(i => i == finishedTask);
                Console.WriteLine($"One Task finished, {notStartedIndex}/{WallsToCheck.Count} done");
                CheatingPaths.Add(finishedTask.Result);
                var wall = WallsToCheck[notStartedIndex];
                Console.WriteLine($"Starting Task for Wall ({wall.x},{wall.y})");
                Tasks[ind] = Task.Run(() => DistanceWhenWallRemoved(map, width, height, wall, start, end));
                notStartedIndex++;
            }
            foreach (var task in Tasks)
            {
                int dist = task.Result;
                CheatingPaths.Add(dist);
            }
            Console.WriteLine("Finished Distance Calculations");
            CheatingPaths.Sort();
            var grouping = CheatingPaths.GroupBy(i => i).ToList();
            return CheatingPaths.Count(i => LegitLenght - i >= PicoThreshold);
            /*var nodes = Enumerable.Range(1, width - 2).SelectMany(x => Enumerable.Range(1, height - 2), (x, y) => new { x, y }).Select(i => (i.x, i.y));
            Console.WriteLine($"Normal count:{nodes.Count()}");
            var bools = new[] { false, true };
            IEnumerable<(int x, int y, bool BottomLayer)> nodes3D = nodes.SelectMany(pos => bools, (pos, thirdD) => new { pos, thirdD }).Select(i => (i.pos.x, i.pos.y, i.thirdD));
            Console.WriteLine($"3D count:{nodes3D.Count()}");
            var graph = new BuiltGraph<(int x, int y, bool BottomLayer)>(nodes3D);
            (int x, int y) start, end;
            (start, end) = GetStartAndEndPosition(map, width, height);
            //Console.WriteLine($"Start ({start.x},{start.y}), End ({end.x},{end.y})");
            foreach (var pos in nodes)
            {
                foreach (var edge in Enumerable.Repeat(pos, 4).Zip(GetNeighbors(map, width, height, pos)))
                {
                    (int x, int y, bool BottomLayer) first1 = (edge.First.x, edge.First.y, false);
                    (int x, int y, bool BottomLayer) first2 = (edge.First.x, edge.First.y, true);
                    (int x, int y, bool BottomLayer) second1 = (edge.Second.x, edge.Second.y, false);
                    (int x, int y, bool BottomLayer) second2 = (edge.Second.x, edge.Second.y, true);
                    graph.AddEdgeOnedirectional(first1, second1);
                    graph.AddEdgeOnedirectional(first2, second2);
                }
                graph.AddEdgeOnedirectional((pos.x, pos.y, true), (pos.x, pos.y, false));
            }
            int LegitDistance = graph.GetDistance((start.x, start.y, true), (end.x, end.y, true));
            foreach (var pos in nodes)
            {
                foreach (var edge in GetInBoundNeighborsIgnoringWalls(map, width, height, pos.x, pos.y).Where(i => nodes.Contains(i)))
                {
                    graph.AddEdgeOnedirectional((pos.x, pos.y, true), (edge.x, edge.y, false));
                }
            }
            var allpaths = graph.GetAllPaths((start.x, start.y, true), (end.x, end.y, false)).ToList();
            List<List<(int x, int y, bool BottomLayer)>> builtPaths = new();
            foreach (var path in allpaths)
            {
                var builtPath = path.ToList();
                var last = builtPath.Last();
                if (last == (end.x, end.y, false))
                {
                    builtPaths.Add(builtPath);
                    //Console.WriteLine($"Found Path with length {builtPath.Count}");
                }
            }
            var groups = builtPaths.GroupBy(path => PicoSeconds(path)).Where(i => i.Key < LegitDistance).ToList();
            groups.Sort((a, b) => b.Key.CompareTo(a.Key));
            Dictionary<int, List<List<(int x, int y, bool BottomLayer)>>> uniquePaths = new();
            foreach (var group in groups)
            {
                uniquePaths.Add(group.Key, new());
                foreach (var path in group)
                {
                    if (uniquePaths[group.Key].TrueForAll(i => !PathsEqual(i, path)))
                    {
                        uniquePaths[group.Key].Add(path);
                    }
                }
            }*/


            /*
            for (int i = builtPaths.Count - 1; i >= 0; i--)
            {
                bool alreadyContained = false;
                for (int j = 0; j < i; j++)
                {
                    if (PathsEqual(builtPaths[i], builtPaths[j]))
                    {
                        alreadyContained = true;
                        break;
                    }
                }
                if (alreadyContained) { builtPaths.RemoveAt(i); }
            }
            var c = pathsWithLengths.Where(i => i.Item1 < LegitDistance).Count();
            Console.WriteLine(c);
            */
            //return graph.GetAllPaths((start.x, start.y, true)).Where(i => i.Last().x == i.Last().y).Count();
            // return uniquePaths.Where(i => LegitDistance - i.Key >= 100).Sum(i => i.Value.Count);
        }
        private int ManhattanDist((int x, int y) a, (int x, int y) b)
            => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        private IEnumerable<int> GetCheatSavingsLengths(char[,] map, int width, int height, (int x, int y) start, (int x, int y) end, int SkippableLength = 20)
        {
            var nodes = Enumerable.Range(1, width - 2).SelectMany(x => Enumerable.Range(1, height - 2), (x, y) => new { x, y }).Select(i => (i.x, i.y)).ToHashSet();
            var graph = new UnbuiltGraph<(int x, int y)>(nodes, node => GetNeighbors(map, width, height, node).Where(i => nodes.Contains(i)));
            var shortestPath = graph.GetShortestPath(start, end).ToList();
            int LegitLength = shortestPath.Count - 1;
            for (int j = 0; j < shortestPath.Count; j++)
            {
                var v = shortestPath[j];
                for (int i = j + 1; i < shortestPath.Count; i++)
                {
                    //if (i == j) { continue; }
                    var u = shortestPath[i];

                    int manhattandist = ManhattanDist(u, v);
                    if (manhattandist <= SkippableLength)
                    {
                        int savedLength = i - j - manhattandist;
                        yield return savedLength;
                    }
                }
            }
            //return graph.GetDistance(start, end);
        }
        private long Part2(char[,] map, int width, int height)
        {
            const int PicoThreshold = 100;
            (int x, int y) start, end;
            (start, end) = GetStartAndEndPosition(map, width, height);
            /*var t = GetCheatSavingsLengths(map, width, height, start, end, 20).GroupBy(i => i).ToList();
            foreach (var i in t)
            {
                Console.WriteLine($"{i.Key} has a total of {i.Count()}");
            }*/
            return GetCheatSavingsLengths(map, width, height, start, end, 20).Count(i=>i>=PicoThreshold);
        }
    }
}