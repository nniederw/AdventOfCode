using System.Collections.Immutable;
namespace AOC2024
{
    class Day19 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");

            List<string> TowelsAvailableLines = new();
            List<string> DesiredTowels = new();
            bool firstPart = true;
            foreach (var line in lines)
            {
                if (line == "")
                {
                    firstPart = false;
                    continue;
                }
                if (firstPart)
                {
                    TowelsAvailableLines.Add(line);
                }
                else
                {
                    DesiredTowels.Add(line);
                }
            }
            List<string> TowelsAvailable = new();
            foreach (var towel in TowelsAvailableLines.Aggregate((a, b) => a + b).Split(", "))
            {
                TowelsAvailable.Add(towel);
            }
            Console.WriteLine(Part1(TowelsAvailable, DesiredTowels));
            Console.WriteLine(Part2(TowelsAvailable, DesiredTowels));
        }
        private bool TowelBuildable(string towel, HashSet<string> TowelsAvailable, HashSet<string> NotBuildable)
        {
            if (TowelsAvailable.Contains(towel)) return true;
            if (NotBuildable.Contains(towel)) return false;
            for (int i = 1; i < towel.Length; i++)
            {
                string firstPart = towel.Substring(0, i);
                string secondPart = towel.Substring(i, towel.Length - i);
                if (TowelBuildable(firstPart, TowelsAvailable, NotBuildable) && TowelBuildable(secondPart, TowelsAvailable, NotBuildable))
                {
                    TowelsAvailable.Add(towel);
                    return true;
                }
            }
            NotBuildable.Add(towel);
            return false;
        }
        private long Part1(IReadOnlyList<string> TowelsAvailable, IReadOnlyList<string> DesiredTowels)
        {
            HashSet<string> available = TowelsAvailable.ToHashSet();
            HashSet<string> notBuildable = new();
            return DesiredTowels.Count(i => TowelBuildable(i, available, notBuildable));
        }
        private long TowelBuildPossiblilities(string towel, ImmutableHashSet<string> TowelAvailable, Dictionary<string, long> TowelPossibilites)
        {
            if (towel == "") return 1;
            if (TowelPossibilites.ContainsKey(towel)) return TowelPossibilites[towel];
            long Possibilities = 0;
            for (int i = 1; i < towel.Length + 1; i++)
            {
                string firstPart = towel.Substring(0, i);
                string secondPart = towel.Substring(i, towel.Length - i);
                if (TowelAvailable.Contains(firstPart))
                {
                    Possibilities += TowelBuildPossiblilities(secondPart, TowelAvailable, TowelPossibilites);
                }
            }
            TowelPossibilites.Add(towel, Possibilities);
            return Possibilities;
        }
        private long Part2(IReadOnlyList<string> TowelsAvailable, IReadOnlyList<string> DesiredTowels)
        {
            var available = TowelsAvailable.ToImmutableHashSet();
            Dictionary<string, long> TowelPossibilites = new();
            return DesiredTowels.Sum(i => TowelBuildPossiblilities(i, available, TowelPossibilites));
        }
    }
}