namespace AOC2024
{
    class Day01 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<int> list1 = new();
            List<int> list2 = new();
            foreach (var line in lines)
            {
                var values = line.Split("   ");
                list1.Add(Convert.ToInt32(values[0]));
                list2.Add(Convert.ToInt32(values[1]));
            }
            Console.WriteLine(Part1(list1.ToList(), list2.ToList()));
            Console.WriteLine(Part2(list1.ToList(), list2.ToList()));
        }
        private int Part1(List<int> list1, List<int> list2)
        {
            list1.Sort();
            list2.Sort();
            int distance = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                distance += Distance(list1[i], list2[i]);
            }
            return distance;
        }
        private int Distance(int x, int y) => Math.Abs(x - y);
        private int Part2(List<int> list1, List<int> list2)
        {
            int similarityScore = 0;
            list1.Sort(); list2.Sort();
            foreach (var location in list1)
            {
                similarityScore += location * list2.Where(i => i == location).Count();
            }
            return similarityScore;
        }
    }
}