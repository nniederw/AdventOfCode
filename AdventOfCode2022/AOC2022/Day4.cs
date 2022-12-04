namespace AOC2022
{
    class Day4
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day4.txt");
            List<((int min, int max) p1, (int min, int max) p2)> input = new();
            foreach (var line in lines)
            {
                ((int min, int max) p1, (int min, int max) p2) pair;
                var persons = line.Split(',');
                var p1 = persons[0].Split('-');
                var p2 = persons[1].Split('-');
                pair.p1 = (Convert.ToInt32(p1[0]), Convert.ToInt32(p1[1]));
                pair.p2 = (Convert.ToInt32(p2[0]), Convert.ToInt32(p2[1]));
                input.Add(pair);
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private static int Part1(List<((int min, int max) p1, (int min, int max) p2)> input)
        {
            int sum = 0;
            foreach (var pair in input)
            {
                if (pair.p1.min >= pair.p2.min && pair.p1.max <= pair.p2.max)
                {
                    sum++;
                }
                else if (pair.p2.min >= pair.p1.min && pair.p2.max <= pair.p1.max)
                {
                    sum++;
                }
            }
            return sum;
        }
        private static int Part2(List<((int min, int max) p1, (int min, int max) p2)> input)
        {
            int sum = 0;
            foreach (var pair in input)
            {
                List<int> p1 = new();
                for (int i = pair.p1.min; i <= pair.p1.max; i++)
                {
                    p1.Add(i);
                }
                List<int> p2 = new();
                for (int i = pair.p2.min; i <= pair.p2.max; i++)
                {
                    p2.Add(i);
                }
                if (p1.Intersect(p2).Any())
                {
                    sum++;
                }
            }
            return sum;
        }
    }
}