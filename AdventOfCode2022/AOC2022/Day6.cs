namespace AOC2022
{
    class Day6
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day6.txt");
            string line = lines.First();
            Console.WriteLine(Part1(line));
            Console.WriteLine(Part2(line));
        }
        private static int Part1(string input)
        {
            for (int i = 0; i < input.Length - 3; i++)
            {
                char[] l = { input[i], input[i + 1], input[i + 2], input[i + 3] };
                if (l.Distinct().Count() == 4)
                {
                    return i + 4;
                }
            }
            return -1;
        }

        private static int Part2(string input)
        {
            for (int i = 0; i < input.Length - 13; i++)
            {
                char[] l = new char[14];
                for (int j = 0; j < 14; j++)
                {
                    l[j] = input[i + j];
                }
                if (l.Distinct().Count() == 14)
                {
                    return i + 14;
                }
            }
            return -1;
        }
    }
}