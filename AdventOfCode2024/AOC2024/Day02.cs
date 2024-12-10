namespace AOC2024
{
    class Day02 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<List<int>> lists = Input.InterpretAs1DList(lines, line => line.Split(' ').Select(i => Convert.ToInt32(i)).ToList());
            Console.WriteLine(Part1(lists));
            Console.WriteLine(Part2(lists));
        }
        private int Part1(List<List<int>> lists)
        {
            return lists.Count(Safe);
        }
        private bool Safe(List<int> list)
        {
            return IncreasingSafe(list) || DecreasingSafe(list);
        }
        private bool IncreasingSafe(List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1] >= list[i])
                {
                    return false;
                }
                if (list[i] - list[i - 1] > 3)
                {
                    return false;
                }
            }
            return true;
        }
        private bool DecreasingSafe(List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1] <= list[i])
                {
                    return false;
                }
                if (list[i - 1] - list[i] > 3)
                {
                    return false;
                }
            }
            return true;
        }
        private bool ProblemDampenerSafe(List<int> list)
        {
            List<List<int>> lists = new();
            lists.Add(list);
            for (int i = 0; i < list.Count; i++)
            {
                var newlist = list.ToList();
                newlist.RemoveAt(i);
                lists.Add(newlist);
            }
            return !lists.TrueForAll(i => !Safe(i));
        }
        private int Part2(List<List<int>> lists)
        {
            return lists.Count(ProblemDampenerSafe);
        }
    }
}