using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public static class Day7
{
    public static int GetP1() => FuncPart1(GetInput());
    public static int GetP2() => FuncPart2(GetInput());
    private static List<int> GetInput()
    {
        List<int> list = new List<int>();
        foreach (var value in File.ReadAllLines("D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day7.txt").First().Split(","))
        {
            list.Add(Convert.ToInt32(value));
        }
        return list;
    }
    private static int FuncPart1(List<int> list)
    {
        List<int> fuels = new List<int>();
        for (int position = 0; position < 10000; position++)
        {
            int fuel = 0;
            foreach (var item in list)
            {
                fuel += Math.Abs(item - position);
            }
            if (fuels.Count > 0 && fuels[position - 1] < fuel)
            {
                return fuels.Last();
            }
            fuels.Add(fuel);
        }
        throw new Exception();
    }
    private static int FuncPart2(List<int> list)
    {
        List<int> fuels = new List<int>();
        for (int position = 0; position < 10000; position++)
        {
            int fuel = 0;
            foreach (var item in list)
            {
            fuel += SumFormula(Math.Abs(item - position));
            }
            if (fuels.Count > 0 && fuels[position - 1] < fuel)
            {
                return fuels.Last();
            }
            fuels.Add(fuel);
        }
        throw new Exception();
    }
    private static int SumFormula(int n) => n * (n + 1) / 2;
}