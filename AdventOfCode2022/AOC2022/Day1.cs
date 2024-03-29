﻿namespace AOC2022
{
    class Day1
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day1.txt");
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
            Console.WriteLine(Part2(calo));
        }
        private static int Part1(List<int> calo)
        {
            return calo.Max();
        }
        private static int Part2(List<int> calo)
        {
            calo.Sort();
            var n = calo.Count;
            return calo[n - 1] + calo[n - 2] + calo[n - 3];
        }
    }
}