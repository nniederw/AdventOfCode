namespace AOC2024
{
    class Day12 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            char[,] garden = Input.InterpretStringsAs2DCharArray(lines);
            int width = garden.GetLength(0);
            int height = garden.GetLength(1);
            Console.WriteLine(Part1(garden, width, height));
            Console.WriteLine(Part2(garden, width, height));
        }
        private bool InBound(int x, int y, int width, int height)
           => 0 <= x && x < width && 0 <= y && y < height;
        private IEnumerable<(int x, int y)> GetPlotNeighbors(char[,] garden, int width, int height, int x, int y)
            => GetPlotNeighbors(garden[x, y], garden, width, height, x, y);
        private IEnumerable<(int x, int y)> GetPlotNeighbors(char plantType, char[,] garden, int width, int height, int x, int y)
        {
            var neighbors = new List<(int x, int y)>() { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
            return neighbors.Where(i => InBound(i.x, i.y, width, height) && garden[i.x, i.y] == plantType);
        }
        //private IEnumerable<(char plant, IEnumerable<(int x, int y)> positions)> GetPlantRegions(char[,] garden, int width, int height)
        private IEnumerable<IEnumerable<(int x, int y)>> GetPlantPlots(char[,] garden, int width, int height)
        {
            bool[,] Visited = new bool[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!Visited[x, y])
                    {
                        var region = GetPlantPlot(garden, width, height, Visited, x, y);
                        yield return region;
                    }
                }
            }
        }
        private IEnumerable<(int x, int y)> GetPlantPlot(char[,] garden, int width, int height, bool[,] Visited, int x, int y)
        {
            if (Visited[x, y]) { yield break; }
            Visited[x, y] = true;
            char plantType = garden[x, y];
            HashSet<(int x, int y)> plotPos = new HashSet<(int x, int y)>() { (x, y) };
            while (plotPos.Any())
            {
                HashSet<(int x, int y)> newPlotPos = new();
                var posNeighbors = plotPos.Select(i => GetPlotNeighbors(plantType, garden, width, height, i.x, i.y));
                foreach (var posNeighbor in posNeighbors)
                {
                    foreach (var neighbor in posNeighbor)
                    {
                        if (!Visited[neighbor.x, neighbor.y])
                        {
                            Visited[neighbor.x, neighbor.y] = true;
                            newPlotPos.Add(neighbor);
                        }
                    }
                }
                foreach (var pos in plotPos)
                {
                    yield return pos;
                }
                plotPos = newPlotPos;
            }
        }
        private long Area(IReadOnlyCollection<(int x, int y)> plants)
        {
            return plants.Count;
        }
        private long Perimeter(IReadOnlyCollection<(int x, int y)> plants)
        {
            long MaxPerimeter = plants.Count * 4;
            long Overcounting = 0;
            foreach (var plant in plants)
            {
                int x = plant.x;
                int y = plant.y;
                var possibleNeighbors = new List<(int x, int y)>() { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
                Overcounting += possibleNeighbors.Count(i => plants.Contains(i));
            }
            if (Overcounting % 2 != 0) { throw new Exception($"function {nameof(Perimeter)} in {nameof(Day12)} counts wrong"); }
            return MaxPerimeter - Overcounting;
        }

        private long Cost(IEnumerable<(int x, int y)> Plot)
        {
            var plot = Plot.ToList();
            return Area(plot) * Perimeter(plot);
        }
        private long Part1(char[,] garden, int width, int height)
        {
            return GetPlantPlots(garden, width, height).Sum(Cost);
        }
        private Edges GetEdges(IReadOnlyCollection<(int x, int y)> plants, (int x, int y) pos)
        {
            int x, y;
            (x, y) = pos;
            bool right = /**/!plants.Contains((x + 1, y));
            bool left = /* */!plants.Contains((x - 1, y));
            bool up = /*   */!plants.Contains((x, y - 1));
            bool down = /* */!plants.Contains((x, y + 1));
            return new Edges(right, up, left, down);
        }
        private long ShortPerimeter(IReadOnlyCollection<(int x, int y)> plants)
        {
            if (plants.Count == 1) return 4;
            Dictionary<(int x, int y), Edges> Edges = new();
            foreach (var plant in plants)
            {
                Edges.Add(plant, GetEdges(plants, plant));
            }
            int SideCount = 0;
            int doubleSideCount = 0;
            foreach (var plant in plants.Where(i => Edges[i].HasEdge))
            {
                var edge = Edges[plant];
                if(plant == (0, 0)) { }
                if (edge.HasEdge)
                {
                    if (edge.EdgeRight)
                    {
                        (int x, int y) up = (plant.x, plant.y - 1);
                        while (Edges.ContainsKey(up) && Edges[up].EdgeRight)
                        {
                            var e = Edges[up];
                            Edges.Remove(up);
                            e.EdgeRight = false;
                            Edges.Add(up, e);
                            up = (up.x, up.y - 1);
                        }
                        (int x, int y) down = (plant.x, plant.y + 1);
                        while (Edges.ContainsKey(down) && Edges[down].EdgeRight)
                        {
                            var e = Edges[down];
                            Edges.Remove(down);
                            e.EdgeRight = false;
                            Edges.Add(down, e);
                            down = (down.x, down.y + 1);
                        }
                        //Console.WriteLine($"Counted edge to the right of ({plant.x},{plant.y})");
                        SideCount++;
                    }
                    if (edge.EdgeLeft)
                    {
                        (int x, int y) up = (plant.x, plant.y - 1);
                        while (Edges.ContainsKey(up) && Edges[up].EdgeLeft)
                        {
                            var e = Edges[up];
                            Edges.Remove(up);
                            e.EdgeLeft = false;
                            Edges.Add(up, e);
                            up = (up.x, up.y - 1);
                        }
                        (int x, int y) down = (plant.x, plant.y + 1);
                        while (Edges.ContainsKey(down) && Edges[down].EdgeLeft)
                        {
                            var e = Edges[down];
                            Edges.Remove(down);
                            e.EdgeLeft = false;
                            Edges.Add(down, e);
                            down = (down.x, down.y + 1);
                        }
                        //Console.WriteLine($"Counted edge to the left of ({plant.x},{plant.y})");
                        SideCount++;
                    }
                    if (edge.EdgeUp)
                    {
                        (int x, int y) right = (plant.x + 1, plant.y);
                        while (Edges.ContainsKey(right) && Edges[right].EdgeUp)
                        {
                            var e = Edges[right];
                            Edges.Remove(right);
                            e.EdgeUp = false;
                            Edges.Add(right, e);
                            right = (right.x + 1, right.y);
                        }
                        (int x, int y) left = (plant.x - 1, plant.y);
                        while (Edges.ContainsKey(left) && Edges[left].EdgeUp)
                        {
                            var e = Edges[left];
                            Edges.Remove(left);
                            e.EdgeUp = false;
                            Edges.Add(left, e);
                            left = (left.x - 1, left.y);
                        }
                        //Console.WriteLine($"Counted edge to the up of ({plant.x},{plant.y})");
                        SideCount++;
                    }
                    if (edge.EdgeDown)
                    {
                        (int x, int y) right = (plant.x + 1, plant.y);
                        while (Edges.ContainsKey(right) && Edges[right].EdgeDown)
                        {
                            var e = Edges[right];
                            Edges.Remove(right);
                            e.EdgeDown = false;
                            Edges.Add(right, e);
                            right = (right.x + 1, right.y);
                        }
                        (int x, int y) left = (plant.x - 1, plant.y);
                        while (Edges.ContainsKey(left) && Edges[left].EdgeDown)
                        {
                            var e = Edges[left];
                            Edges.Remove(left);
                            e.EdgeDown = false;
                            Edges.Add(left, e);
                            left = (left.x - 1, left.y);
                        }
                        //Console.WriteLine($"Counted edge to the down of ({plant.x},{plant.y})");
                        SideCount++;
                    }
                }
            }
            //if (doubleSideCount % 2 != 0) { throw new Exception($"function {nameof(ShortPerimeter)} in {nameof(Day12)} counts wrong"); }
            return SideCount;
        }
        private long DiscoutCost(IEnumerable<(int x, int y)> Plot)
        {
            var plot = Plot.ToList();
            long area = Area(plot);
            long peri = ShortPerimeter(plot);
            long result = area * peri;
            return result;
        }
        private long Part2(char[,] garden, int width, int height)
        {
            return GetPlantPlots(garden, width, height).Sum(DiscoutCost);
        }
        private struct Edges
        {
            public bool EdgeRight = false;
            public bool EdgeUp = false;
            public bool EdgeLeft = false;
            public bool EdgeDown = false;
            public Edges(bool right, bool up, bool left, bool down)
            {
                EdgeRight = right;
                EdgeUp = up;
                EdgeLeft = left;
                EdgeDown = down;
            }
            public bool HasEdge => (EdgeRight || EdgeUp) || (EdgeLeft || EdgeDown);
            public int EdgeCount()
            {
                int res = 0;
                if (EdgeRight) { res++; }
                if (EdgeUp) { res++; }
                if (EdgeLeft) { res++; }
                if (EdgeDown) { res++; }
                return res;
            }
        }
    }
}