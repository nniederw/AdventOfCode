namespace AOC2025
{
    class Day05 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<(long start, long end)> freshIDs = new();
            List<long> ids = new();
            bool firstPart = true;
            foreach (var line in lines)
            {
                if (line == "" || line == " ")
                {
                    firstPart = false;
                    continue;
                }
                if (firstPart)
                {
                    var split = line.Split('-');
                    freshIDs.Add((Convert.ToInt64(split[0]), Convert.ToInt64(split[1])));
                    continue;
                }
                ids.Add(Convert.ToInt64(line));
            }
            Console.WriteLine(Part1(freshIDs, ids));
            Console.WriteLine(Part2(freshIDs, ids));
        }
        private bool IsFreshID(long id, List<(long start, long end)> freshIDs)
        {
            foreach (var range in freshIDs)
            {
                if (range.start <= id && id <= range.end)
                {
                    return true;
                }
            }
            return false;
        }
        private long Part1(List<(long start, long end)> freshIDs, List<long> ids)
        {
            return ids.Count(i => IsFreshID(i, freshIDs));
        }
        private bool RangeIsOverlapping((long start, long end) range1, (long start, long end) range2, out (long start, long end) merged)
        {
            merged = (-1, -1);
            bool overlapping = false;
            if (range2.start <= range1.start && range1.start <= range2.end)
            {
                merged.start = range2.start;
                overlapping = true;
            }
            if (range1.start <= range2.start && range2.start <= range1.end)
            {
                merged.start = range1.start;
                overlapping = true;
            }
            if (range2.start <= range1.end && range1.end <= range2.end)
            {
                merged.end = range2.end;
                overlapping = true;
            }
            if (range1.start <= range2.end && range2.end <= range1.end)
            {
                merged.end = range1.end;
                overlapping = true;
            }
            return overlapping;
        }
        private long Part2(List<(long start, long end)> freshIDRanges, List<long> ids)
        {
            freshIDRanges = freshIDRanges.ToList();
            bool changed = true;
            while (changed)
            {
                changed = false;
                for (int i = 0; i < freshIDRanges.Count; i++)
                {
                    bool doBreak = false;
                    for (int j = i + 1; j < freshIDRanges.Count; j++)
                    {
                        (long start, long end) merged;
                        if (RangeIsOverlapping(freshIDRanges[i], freshIDRanges[j], out merged))
                        {
                            freshIDRanges.RemoveAt(j);
                            freshIDRanges.RemoveAt(i);
                            freshIDRanges.Add(merged);
                            doBreak = true;
                            changed = true;
                            break;
                        }
                    }
                    if (doBreak)
                    {
                        break;
                    }
                }
            }
            long result = 0;
            foreach (var range in freshIDRanges)
            {
                result += range.end - range.start + 1;
            }
            return result;
        }
    }
}