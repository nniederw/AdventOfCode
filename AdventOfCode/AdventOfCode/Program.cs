using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Day3Part2());
        }
        private static int Day1Part1()
        {
            var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day1.txt";
            var enumLines = File.ReadLines(path, Encoding.UTF8);
            List<int> list = new List<int>();
            foreach (var line in enumLines)
            {
                list.Add(Convert.ToInt32(line));
            }
            int[] array = list.ToArray();
            return Day1Part1Func(array);
        }
        private static int Day1Part2()
        {
            var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day1.txt";
            var enumLines = File.ReadLines(path, Encoding.UTF8);
            List<int> list = new List<int>();
            foreach (var line in enumLines)
            {
                list.Add(Convert.ToInt32(line));
            }
            int[] array = list.ToArray();
            return Day1Part2Func(array);
        }
        private static int Day1Part1Func(int[] ints)
        {
            int res = 0;
            for (int i = 1; i < ints.Length; i++)
            {
                if (ints[i - 1] < ints[i])
                {
                    res++;
                }
            }
            return res;
        }
        private static int Day1Part2Func(int[] ints)
        {
            int res = 0;
            for (int i = 1; i < ints.Length - 2; i++)
            {
                if (ints[i - 1] + ints[i] + ints[i + 1] < ints[i] + ints[i + 1] + ints[i + 2])
                {
                    res++;
                }
            }
            return res;
        }
        private static int Day2Part1()
        {
            var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day2.txt";
            var enumLines = File.ReadLines(path, Encoding.UTF8);
            List<int> forward = new List<int>();
            List<int> down = new List<int>();
            List<int> up = new List<int>();
            foreach (var line in enumLines)
            {
                var values = line.Split(" ");
                switch (values[0])
                {
                    case ("forward"):
                        forward.Add(Convert.ToInt32(values[1]));
                        break;
                    case ("down"):
                        down.Add(Convert.ToInt32(values[1]));
                        break;
                    case ("up"):
                        up.Add(Convert.ToInt32(values[1]));
                        break;
                    default:
                        throw new Exception();
                }
            }
            return Day2Part1Func(forward.ToArray(), down.ToArray(), up.ToArray());
        }
        private static int Day2Part1Func(int[] forward, int[] down, int[] up)
        {
            int forwardSum = Sum(forward);
            int downSum = Sum(down);
            int upSum = Sum(up);
            return forwardSum * (downSum - upSum);
        }
        private static int Day2Part2()
        {
            List<(char, int)> values = new List<(char, int)>();
            var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day2.txt";
            var enumLines = File.ReadLines(path, Encoding.UTF8);
            foreach (var line in enumLines)
            {
                var strs = line.Split(" ");
                values.Add((strs[0][0], Convert.ToInt32(strs[1])));
            }
            return Day2Par2Func(values);
        }
        private static int Day2Par2Func(List<(char, int)> values)
        {
            int horizontal = 0;
            int depth = 0;
            int aim = 0;
            foreach (var item in values)
            {
                switch (item.Item1)
                {
                    case 'f':
                        horizontal += item.Item2;
                        depth += aim * item.Item2;
                        break;
                    case 'd':
                        aim += item.Item2;
                        break;
                    case 'u':
                        aim -= item.Item2;
                        break;
                }
            }
            return horizontal * depth;
        }
        private static int Sum(int[] ints)
        {
            int res = 0;
            foreach (var val in ints)
            {
                res += val;
            }
            return res;
        }
        private static int Day3Part1()
        {
            const int BitLength = 12;
            List<BitArray> array = new List<BitArray>();
            var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day3.txt";
            var enumLines = File.ReadLines(path, Encoding.UTF8);
            foreach (var line in enumLines)
            {
                var bitarr = new BitArray(BitLength);
                for (int i = 0; i < bitarr.Length; i++)
                {
                    bitarr.Set(i, line[i] == '1');
                }
                array.Add(bitarr);
            }
            return Day3Part1Func(array);
        }
        private static int Day3Part1Func(List<BitArray> list)
        {
            int length = list[0].Length;
            int[] trues = new int[length];
            for (int j = 0; j < list.Count; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    if (list[j][i]) { trues[i] += 1; }
                }
            }
            BitArray gamma = new BitArray(length);
            BitArray epsilon = new BitArray(length);
            for (int i = 0; i < trues.Length; i++)
            {
                if (trues[i] > list.Count - trues[i])
                {
                    gamma[i] = true;
                    epsilon[i] = false;
                }
                else
                {
                    gamma[i] = false;
                    epsilon[i] = true;
                }
            }
            var intGamma = Convert.ToInt32(ToBinaryString(gamma), 2);
            var intEpsilon = Convert.ToInt32(ToBinaryString(epsilon), 2);
            Console.WriteLine($"gamma {ToBinaryString(gamma)} {intGamma}, epsilon {ToBinaryString(epsilon)} {intEpsilon}");
            return intGamma * intEpsilon;
        }
        private static int Day3Part2()
        {
            const int BitLength = 12;
            List<BitArray> array = new List<BitArray>();
            var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day3.txt";
            var enumLines = File.ReadLines(path, Encoding.UTF8);
            foreach (var line in enumLines)
            {
                var bitarr = new BitArray(BitLength);
                for (int i = 0; i < bitarr.Length; i++)
                {
                    bitarr.Set(i, line[i] == '1');
                }
                array.Add(bitarr);
            }
            return Day3Part2Func(array);
        }
        private static int Day3Part2Func(List<BitArray> list)
        {
            var OxygenList = new List<BitArray>(list);
            int length = list[0].Length;
            for (int i = 0; i < length; i++)
            {
                int ones = 0;
                for (int j = 0; j < OxygenList.Count; j++)
                { if (OxygenList[j][i]) { ones++; } }
                if (ones >= OxygenList.Count - ones) { OxygenList = Keep1At(i, OxygenList); }
                else { OxygenList = Keep0At(i, OxygenList); }
            }
            var CO2List = new List<BitArray>(list);
            for (int i = 0; i < length; i++)
            {
                if (CO2List.Count == 1) { break; }
                int ones = 0;
                for (int j = 0; j < CO2List.Count; j++)
                { if (CO2List[j][i]) { ones++; } }
                if (ones < CO2List.Count - ones) { CO2List = Keep1At(i, CO2List); }
                else { CO2List = Keep0At(i, CO2List); }
            }
            List<BitArray> Keep1At(int index, List<BitArray> bits) => bits.Where(i => i[index]).ToList();
            List<BitArray> Keep0At(int index, List<BitArray> bits) => bits.Where(i => !i[index]).ToList();
            Console.WriteLine($"o2list {OxygenList.Count} co2list {CO2List.Count}");

            var CO2 = Convert.ToInt32(ToBinaryString(CO2List[0]), 2);
            var O2 = Convert.ToInt32(ToBinaryString(OxygenList[0]), 2);
            Console.WriteLine($"O2 {ToBinaryString(OxygenList[0])} CO2 {ToBinaryString(CO2List[0])}");
            Console.WriteLine($"O2 {O2} CO2 {CO2}");
            return CO2 * O2;
        }
        private static string ToBinaryString(BitArray bits)
        {
            string res = "";
            foreach (bool item in bits)
            {
                res += item ? "1" : "0";
            }
            return res;
        }
    }
}