namespace AOC2024
{
    class Day08 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            char[,] map = Input.InterpretStringsAs2DCharArray(lines);
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            //Console.WriteLine(Part1(map, width, height));
            Console.WriteLine($"{369} (Precomputed)");
            //Console.WriteLine(Part2(map, width, height));
            Console.WriteLine($"{1169} (Precomputed)");
        }
        private Dictionary<char,List<(int x,int y)>> GetAntennas(char[,] map, int width, int height)
        {
            char ignoreChar = '.';
            Dictionary<char, List<(int x, int y)>> Antennas = new();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char c = map[x, y];
                    if (c != ignoreChar)
                    {
                        if (!Antennas.ContainsKey(c))
                        {
                            Antennas[c] = new List<(int x, int y)>();
                        }
                        Antennas[c].Add((x, y));
                    }
                }
            }
            return Antennas;
        }
        private int Part1(char[,] map, int width, int height)
        {
            char ignoreChar = '.';
            Dictionary<char, List<(int x, int y)>> Antennas = GetAntennas(map, width, height);
            int result = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool hasAntinode = !Antennas.Select(i => i.Value).ToList().TrueForAll(i => !AntinodeOfAntenna(i, x, y));
                    if (hasAntinode)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
        private bool AntinodeOfAntenna(List<(int x, int y)> antennas, int x, int y)
        {
            foreach (var ant1 in antennas)
            {
                foreach (var ant2 in antennas)
                {
                    if (ant1 == ant2) continue;
                    if (AntinodeOfAntennas(ant1, ant2, (x, y)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool AntinodeOfAntennas((int x, int y) ant1, (int x, int y) ant2, (int x, int y) pos)
        {
            var dist1 = DirectionV1ToV2(ant1, pos);
            var dist1x2 = (dist1.x * 2, dist1.y * 2);
            var dist2 = DirectionV1ToV2(ant2, pos);
            var dist2x2 = (dist2.x * 2, dist2.y * 2);
            return (dist1 == dist2x2 || dist2 == dist1x2);
        }
        /*
        private bool InLine((int x, int y) v1, (int x, int y) v2, (int x, int y) v3)
        {
            (int x, int y) dif12 = (v2.x - v1.x, v2.y - v1.y);
            (int x, int y) dif23 = (v3.x - v2.x, v3.y - v2.y);
            var sign12 = Math.Sign(dif12.x);
            dif12 = (dif12.x * sign12, dif12.y * sign12);
            var sign23 = Math.Sign(dif23.x);
            dif23 = (dif23.x * sign23, dif23.y * sign23);
            var ratio12x23x = new Rational(dif12.x, dif23.x);
            var ratio12y23y = new Rational(dif12.y, dif23.y);
            return ratio12x23x == ratio12y23y;
        }*/
        private (int x, int y) DirectionV1ToV2((int x, int y) v1, (int x, int y) v2)
            => (v2.x - v1.x, v2.y - v1.y);
        private bool TAlignedAntenna((int x, int y) ant1, (int x, int y) ant2, (int x, int y) pos)
        {
            if (pos == ant1 || pos == ant2) return true;
            var dist12 = DirectionV1ToV2(ant1, ant2);
            var dist2Pos = DirectionV1ToV2(ant2, pos);
            int gcd12 = Gcd(dist12.x, dist12.y);
            (int x, int y) direcAnt = (dist12.x / gcd12, dist12.y / gcd12);
            if (dist2Pos.x % direcAnt.x != 0 || dist2Pos.y % direcAnt.y != 0) return false;
            int multx = dist2Pos.x / direcAnt.x;
            return direcAnt.y * multx == dist2Pos.y;
        }
        private bool TAlignedAntenna(List<(int x, int y)> antennas, int x, int y)
        {
            foreach (var ant1 in antennas)
            {
                foreach (var ant2 in antennas)
                {
                    if (ant1 == ant2) continue;
                    if (TAlignedAntenna(ant1, ant2, (x, y)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private static int Gcd(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            int min = Math.Min(a, b);
            int max = Math.Max(a, b);
            if (min == 0) return max;
            if (min == 1)
            {
                return 1;
            }
            max = Math.Max(a, b);
            int mod = max % min;
            return mod == 0 ? min : Gcd(min, mod);
        }
        private int Part2(char[,] map, int width, int height)
        {
            var Antennas = GetAntennas(map, width, height);
            int result = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool hasTAligned = !Antennas.Select(i => i.Value).ToList().TrueForAll(i => !TAlignedAntenna(i, x, y));
                    if (hasTAligned)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
    }
}