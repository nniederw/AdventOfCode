namespace AOC2022
{
    class Day1
    {
        private static void Main()
        {
            Console.WriteLine("Test");
            //var lines = File.ReadLines(Consts.InputPath + "Day1.txt");
            var lines = File.ReadLines("D:/WorkExt/Git/nniederw/AdventOfCode/AdventOfCode2022/Input/Day1.txt");

            List<int> calo = new();
            int i = 0;
            calo.Add(i);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    calo[i] += Convert.ToInt32(line);
                }
                else
                {
                    calo.Add(0);
                    i++;
                }
            }
            Console.WriteLine(Part1(calo));
        }
        private static int Part1(List<int> calo)
        {
            int ind = 0;
            int max = 0;
            for (int i = 0; i < calo.Count; i++)
            {
                if (max < calo[i])
                {
                    ind = i;
                    max = calo[i];
                }
            }
            return ind;
        }
        private static int Part2()
        {
            return 0;
        }
    }
}