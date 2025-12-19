namespace AOC2025
{
    class Day02 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<(long start, long end)> input = lines.First().Split(',').Select(i => i.Split('-')).Select(i => (Convert.ToInt64(i[0]), Convert.ToInt64(i[1]))).ToList();
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private bool IsInvalidID(long id)
        {
            string number = id.ToString();
            if (number.Length % 2 != 0)
            {
                return false;
            }
            var middle = number.Length / 2;
            return number.Substring(0, middle) == number.Substring(middle);
        }
        private IEnumerable<long> GetInvalidIDs(List<(long start, long end)> input)
        {
            foreach (var range in input)
            {
                for (long i = range.start; i < range.end + 1; i++)
                {
                    if (IsInvalidID(i))
                    {
                        yield return i;
                    }
                }
            }
        }
        private long Part1(List<(long start, long end)> input)
        {
            return GetInvalidIDs(input).Sum();
        }
        private bool IsInvalidIDP2(long id)
        {
            if (IsInvalidID(id))
            {
                return true;
            }
            string number = id.ToString();
            int length = number.Length;
            for (int k = 1; k < length / 2+1; k++)
            {
                if (length % k != 0)
                {
                    continue;
                }
                var repeat = number.Substring(0, k);
                for (int i = k; i < length; i++)
                {
                    if (repeat[i % k] != number[i])
                    {
                        break;
                    }
                    if (i == length - 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private IEnumerable<long> GetInvalidIDsP2(List<(long start, long end)> input)
        {
            foreach (var range in input)
            {
                for (long i = range.start; i < range.end + 1; i++)
                {
                    if (IsInvalidIDP2(i))
                    {
                        yield return i;
                    }
                }
            }
        }
        private long Part2(List<(long start, long end)> input)
        {
            return GetInvalidIDsP2(input).Sum();
        }
    }
}