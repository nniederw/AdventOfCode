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
        var input = File.ReadAllLines("D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day9Small.txt");
        int[,] heightMap = new int[input.Length, input.First().Length];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input.First().Length; j++)
            {
                heightMap[i, j] = Convert.ToInt32("" + input[i][j]);
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
    private static int FuncPart2(int[,] hmap)
    {
        throw new NotImplementedException();
        List<Basin> basins = new List<Basin>();
        for (int i = 0; i < hmap.GetLength(0); i++)
        {
            for (int j = 0; j < hmap.GetLength(1); j++)
            {
                if (hmap[i, j] != 9)
                {
                    if (basins.TrueForAll(b => !b.Contains((i, j))))
                    {
                        var basin = basins.Find(b => b.GetEdges().Contains((i, j)));
                        if (basin != null)
                        {
                            basin.Parts.Add((i, j));
                        }
                        else
                        {
                            basin = new Basin(hmap.GetLength(0), hmap.GetLength(1));
                            basin.Parts.Add((i, j));
                            basins.Add(basin);
                        }
                    }
                }
            }
        }
        {
            int i = 0;
            while (i != basins.Count)
            {
                var basin = basins[i];
                var edges = basin.GetEdges();
                for (int j = i + 1; j < basins.Count; j++)
                {
                    var other = basins[j];
                    if (edges.Exists(b => other.Contains(b)))
                    {
                        basin.MergeWith(other);
                        basins.RemoveAt(j);
                        i--;
                        break;
                    }
                }
                i++;
            }
        }
        return basins.Product(i => i.Parts.Count);
    }
    public static int Product<T>(this IEnumerable<T> list, Func<T, int> getInt)
    {
        int result = 1;
        foreach (var i in list)
        {
            result *= getInt(i);
        }
        return result;
    }
}
public class Basin
{
    public int Height;
    public int Width;
    public List<(int, int)> Parts = new List<(int, int)>();
    public Basin(int height, int width)
    {
        Height = height;
        Width = width;
    }
    public List<(int, int)> GetEdges()
    {
        var neighbors = new List<(int, int)>();
        Parts.ForEach(i =>
        {
            neighbors.Add((i.Item1 - 1, i.Item2));
            neighbors.Add((i.Item1 + 1, i.Item2));
            neighbors.Add((i.Item1, i.Item2 - 1));
            neighbors.Add((i.Item1, i.Item2 + 1));
        });
        return neighbors.FindAll(i => !Parts.Contains(i)).FindAll(i => i.Item1 > 0 && i.Item1 < Height && i.Item2 > 0 && i.Item2 < Width);
    }
    public bool Contains((int, int) point) => Parts.Contains(point);
    public void MergeWith(Basin other)
    {
        Parts = Parts.Concat(other.Parts).ToList();
        other.Parts.Clear();
    }
}