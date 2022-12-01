using System;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

namespace AdventOfCode
{
    public static class Day14
    {
        public static long GetP1() => FuncPart1(GetInput());
        public static long GetP2() => FuncPart2(GetInput());
        private static (string, Dictionary<string, char>) GetInput()
        {
            string sequence = "";
            Dictionary<string, char> translation = new Dictionary<string, char>();
            bool first= true;
            foreach (var line in File.ReadAllLines(Program.InputFolderPath + "/Day14.txt"))
            {
                if (first)
                {
                    sequence = line;
                    first = false;
                    continue;
                }
                if (line == "") { continue; }
                var trans = line.Split(" -> ");
                translation.Add(trans[0], trans[1][0]);
            }
            return (sequence,translation);
        }
        private static long FuncPart1((string, Dictionary<string, char>) input)
        {
            string sequence = input.Item1;
            Dictionary<string, char> translation = input.Item2;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < sequence.Length-1; j++)
                {
                    char c = translation[sequence[j] + "" + sequence[j + 1]];
                    sequence = sequence.Substring(0, j+1) +c+sequence.Substring(j + 1, sequence.Length - j-1);
                    j++;
                }
            }
            Dictionary<char, int> Characters = new Dictionary<char, int>();
            sequence.Foreach(i =>
            {
                if (Characters.ContainsKey(i))
                {
                    Characters[i]++;
                }
                else
                {
                    Characters.Add(i, 1);
                }
            });
            long max = Characters[Characters.MaxOfValue(i => i.Value).Key];
            long min = Characters[Characters.MinOfValue(i => i.Value).Key];
            return max-min;
        }

        private static long FuncPart2((string, Dictionary<string, char>) input)
        {
            return default;
        }
    }
}