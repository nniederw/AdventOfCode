namespace AOC2024
{
    class Day11 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<long> stones = new();
            foreach (var stone in lines.First().Split(' '))
            {
                stones.Add(Convert.ToInt64(stone));
            }
            if (lines.Count() > 1) throw new Exception($"Unexpected input file for {nameof(Day11)}");
            Console.WriteLine(Part1(stones));
            Console.WriteLine(Part2(stones));
        }
        private Dictionary<long, long> BlinkOnce(Dictionary<long, long> stones)
        {
            var result = new Dictionary<long, long>();
            foreach (var stone in stones)
            {
                List<(long number, long amount)> toAdd = new();
                string s = stone.Key.ToString();
                if (stone.Key == 0)
                {
                    toAdd.Add((1, stone.Value));
                }
                else if (s.Length % 2 == 0)
                {
                    string nstr1 = s.Substring(0, s.Length / 2);
                    string nstr2 = s.Substring(s.Length / 2, s.Length / 2);
                    long n1 = Convert.ToInt64(nstr1);
                    long n2 = Convert.ToInt64(nstr2);
                    toAdd.Add((n1, stone.Value));
                    toAdd.Add((n2, stone.Value));
                }
                else
                {
                    toAdd.Add((stone.Key * 2024, stone.Value));
                }
                foreach (var i in toAdd)
                {
                    if (result.ContainsKey(i.number))
                    {
                        result[i.number] += i.amount;
                    }
                    else
                    {
                        result.Add(i.number, i.amount);
                    }
                }
            }
            return result;
        }
        private Dictionary<long, long> MultiBlink(List<long> stones, long blinkAmount)
        {
            Dictionary<long, long> stoneDict = new();
            foreach (var stone in stones)
            {
                if (stoneDict.ContainsKey(stone))
                {
                    stoneDict[stone] += 1;
                }
                else
                {
                    stoneDict.Add(stone, 1);
                }
            }
            for (int i = 0; i < blinkAmount; i++)
            {
                stoneDict = BlinkOnce(stoneDict);
            }
            Console.WriteLine($"{stoneDict.Count} total different numbers");
            return stoneDict;
        }
        private long Part1(List<long> stones)
        {
            return MultiBlink(stones, 25).Sum(i => i.Value);
        }
        private long Part2(List<long> stones)
        {
            return MultiBlink(stones, 75).Sum(i => i.Value);
        }
    }
}