namespace AOC2022
{
    class Day10
    {
        private const string Noop = "noop";
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day10.txt");
            List<(string, int)> input = new();
            foreach (var line in lines)
            {
                var l = line.Split(' ');
                input.Add((l[0], l[0] == Noop ? 0 : Convert.ToInt32(l[1])));
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private static int Part1(List<(string inst, int i)> input)
        {
            for (int i = input.Count - 1; i >= 0; i--)
            {
                if (input[i].inst != Noop)
                {
                    input.Insert(i, (Noop, 0));
                }
            }
            int sum = 0;
            int x = 1;
            int clock = 0;
            int toadd = 0;
            foreach (var line in input)
            {
                if (line.inst != Noop)
                {
                    toadd = line.i;
                }
                IncrementClock();
                x += toadd;
                toadd = 0;
            }
            return sum;
            void IncrementClock()
            {
                clock++;
                if ((clock + 20) % 40 == 0)
                {
                    sum += clock * x;
                }
            }
        }
        private static string Part2(List<(string inst, int i)> input)
        {
            //already modified by Part1
            /*for (int i = input.Count - 1; i >= 0; i--)
            {
                if (input[i].inst != Noop)
                {
                    input.Insert(i, (Noop, 0));
                }
            }*/
            bool[] image = new bool[6 * 40];
            int x = 1;
            int clock = 0;
            int toadd = 0;
            foreach (var line in input)
            {
                if (line.inst != Noop)
                {
                    toadd = line.i;
                }
                Draw(clock);
                clock++;
                x += toadd;
                toadd = 0;
            }
            string result = "";
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 40; i++)
                {
                    result += image[i + 40 * j] ? "#" : ".";
                }
                result += "\n";
            }
            return result;
            void Draw(int i)
            {
                int y = i % 40;
                image[i] = y == x - 1 || y == x || y == x + 1 ? true : false;
            }
        }
    }
}