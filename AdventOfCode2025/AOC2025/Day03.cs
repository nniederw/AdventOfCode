namespace AOC2025
{
    class Day03 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<List<long>> input = new();
            foreach (var line in lines)
            {
                input.Add(line.Select(i => Convert.ToInt64(i.ToString())).ToList());
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private long MaxJoltage(List<long> batteries)
        {
            long max = 0;
            for (int j = 0; j < batteries.Count; j++)
            {
                for (int i = j + 1; i < batteries.Count; i++)
                {
                    long joltage = batteries[j] * 10 + batteries[i];
                    max = Math.Max(max, joltage);
                }
            }
            return max;
        }
        private long Part1(List<List<long>> input)
        {
            return input.Select(i => MaxJoltage(i)).Sum();
        }
        private long MaxJoltageFast(List<long> batteries, long turningCount)
        {
            List<long> batteriesLeft = batteries.ToList();
            List<long> turnedOn = new List<long>();
            for (int digit = 0; digit < turningCount; digit++)
            {
                var digitsLeft = turningCount - digit;
                var count = batteriesLeft.Count;
                long max = 0;
                int maxIndex = -1;
                for (int i = 0; i <= count - digitsLeft; i++)
                {
                    if (batteriesLeft[i] > max)
                    {
                        max = batteriesLeft[i];
                        maxIndex = i;
                    }
                }
                turnedOn.Add(max);
                for (int i = maxIndex; i >= 0; i--)
                {
                    batteriesLeft.RemoveAt(i);
                }
            }
            return Convert.ToInt64(turnedOn.Select(i => i.ToString()).Aggregate((a, b) => a + b));
        }
        private long Part2(List<List<long>> input)
        {
            return input.Select(i => MaxJoltageFast(i,12)).Sum();
        }
    }
}