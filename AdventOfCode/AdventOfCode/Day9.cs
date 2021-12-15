using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

public static class Day9
{
    public static int GetP1() => FuncPart1(GetInput());
    public static int GetP2() => FuncPart2(GetInput());
    private static int[,] GetInput()
    {
        var input = File.ReadAllLines("D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day9.txt");
        int[,] heightMap = new int[input.Length, input.First().Length];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input.First().Length; j++)
            {
                heightMap[i, j] =Convert.ToInt32(""+input[i][j]);
            }
        }
        return heightMap;
    }
    private static int FuncPart1(int[,] hmap)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < hmap.GetLength(0); i++)
        {
            for (int j = 0; j < hmap.GetLength(1); j++)
            {
                int top = j > 0 ? hmap[i, j - 1] : int.MaxValue;
                int bottom = j < hmap.GetLength(1) - 1 ? hmap[i, j + 1] : int.MaxValue;
                int left = i > 0 ? hmap[i - 1, j] : int.MaxValue;
                int right = i < hmap.GetLength(0) - 1 ? hmap[i + 1, j] : int.MaxValue;
                if (SmallesOf(hmap[i, j], top, bottom, left, right))
                {
                    result.Add(hmap[i, j] + 1);
                }
            }
        }
        return result.Sum();
    }
    public static bool SmallesOf(int target, int a, int b)
             => target < a && target < b;
    public static bool SmallesOf(int target, int a, int b, int c)
            => target < a && target < b && target < c;
    public static bool SmallesOf(int target, int a, int b, int c, int d)
            => target < a && target < b && target < c && target < d;
    private static int FuncPart2(int[,] heightMap)
    {
        return 0;
    }
}