using Microsoft.VisualBasic;

namespace AOC2024
{
    class Day05 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<string> input1 = new();
            List<string> input2 = new();
            bool secondpart = false;
            foreach (var line in lines)
            {
                if (line == "")
                {
                    secondpart = true;
                    continue;
                }
                if (secondpart)
                {
                    input2.Add(line);
                }
                else
                {
                    input1.Add(line);
                }
            }
            List<(int, int)> PageOrderRules = new();
            foreach (var line in input1)
            {
                var split = line.Split('|');
                if (split.Length != 2) { throw new Exception($"Unexpected line: {line}"); }
                int n1 = Convert.ToInt32(split[0]);
                int n2 = Convert.ToInt32(split[1]);
                PageOrderRules.Add((n1, n2));
            }
            List<List<int>> PageUpdates = new();
            foreach (var line in input2)
            {
                var split = line.Split(',');
                List<int> update = new();
                foreach (var i in split)
                {
                    update.Add(Convert.ToInt32(i));
                }
                PageUpdates.Add(update);
            }
            Console.WriteLine(Part1(PageOrderRules, PageUpdates));
            Console.WriteLine(Part2(PageOrderRules, PageUpdates));
        }
        private int Part1(List<(int, int)> rules, List<List<int>> updates)
        {
            var correctUpdates = updates.Where(i => RightOrdering(rules, i));
            return correctUpdates.Sum(GetMiddle);
        }
        private int GetMiddle(List<int> list)
        {
            if (list.Count % 2 == 0) { throw new Exception($"Unexpected line length found"); }
            return list[list.Count / 2];
        }
        private bool RightOrdering(List<(int, int)> rules, List<int> updates)
        {
            HashSet<int> seenPages = new();
            foreach (var page in updates)
            {
                var prev = rules.FindAll(i => updates.Contains(i.Item1) && i.Item2 == page);
                if (prev.Count > 0)
                {
                    if (!prev.TrueForAll(i => seenPages.Contains(i.Item1)))
                    {
                        return false;
                    }
                }
                seenPages.Add(page);
            }
            return true;
        }
        private int Part2(List<(int, int)> rules, List<List<int>> updates)
        {
            var IncorrectOrders = updates.Where(i => !RightOrdering(rules, i)).ToList();
            foreach (var incor in IncorrectOrders)
            {
                FixOrdering(rules, incor);
            }
            return IncorrectOrders.Sum(GetMiddle);
        }
        private void FixOrdering(List<(int, int)> rules, List<int> updates)
        {
            rules = rules.Where(i => updates.Contains(i.Item1) && updates.Contains(i.Item2)).ToList();
            updates.Sort(
                    (a, b) =>
                    rules.Contains((a, b)) ? -1 :
                    rules.Contains((b, a)) ? 1 : 0
                    );
        }
    }
}