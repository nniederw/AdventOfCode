namespace AOC2022
{
    class Day3
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day3.txt");
            List<(string, string)> input = new();
            foreach (var line in lines)
            {
                (string, string) s;
                s.Item1 = line.Substring(0, line.Length / 2);
                s.Item2 = line.Substring(line.Length / 2);
                input.Add(s);
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private static int Priority(char c) => c == $"{c}".ToLower()[0] ? (int)c - (int)'a' + 1 : (int)c - (int)'A' + 27;
        private static int Part1(List<(string s1, string s2)> input)
        {
            int sum = 0;
            foreach (var backpack in input)
            {
                foreach (var c in backpack.s1.Distinct())
                {
                    if (backpack.s2.Contains(c))
                    {
                        sum += Priority(c);
                    }
                }
            }
            return sum;
        }
        private static int Part2(List<(string s1, string s2)> input)
        {
            int sum = 0;
            var groups = input.ConvertAll(s => $"{s.s1}{s.s2}").Chunk(3);
            foreach (var group in groups)
            {
                sum += Priority(group[0].Intersect(group[1].Intersect(group[2])).Single());
            }
            return sum;
        }
    }
}