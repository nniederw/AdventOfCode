using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Day6
{
    private const int days = 256;
    private static List<int> GetInput()
    {
        var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day6.txt";
        var enumLines = File.ReadLines(path).First().Split(",");
        var list = new List<int>();
        enumLines.ToList().ForEach(i => list.Add(Convert.ToInt32(i)));
        return list;
    }
    public static int GetP1() => FuncPart1(GetInput());

    private static int FuncPart1(List<int> list)
    {
        var fishs = new List<Laternfish>();
        list.ForEach(i => fishs.Add(new Laternfish(i)));
        var newFish = new List<Laternfish>();
        for (int i = 0; i < days; i++)
        {
            fishs.ForEach(i =>
            {
                var fish = i.Update();
                if (fish != null)
                {
                    newFish.Add(fish);
                }
            });
            newFish.ForEach(fishs.Add);
            newFish.Clear();
        }
        return fishs.Count;
    }

    public static long GetP2() => FuncPart2(GetInput());

    private static long FuncPart2(List<int> list)
    {
        long[] fishCount = new long[9];
        list.ForEach(i => fishCount[i]++);
        for (int i = 0; i < days; i++)
        {
            long[] NextFish = new long[9];
            for (int j = 8; j > 0; j--)
            {
                NextFish[j - 1] = fishCount[j];
            }
            NextFish[6] += fishCount[0];
            NextFish[8] += fishCount[0];
            fishCount = NextFish;
        }
        return fishCount.Sum();
    }
}
public class Laternfish
{
    public int Timer = 6;
    public Laternfish(int timer) => Timer = timer;
    public Laternfish Update()
    {
        Timer--;
        if (Timer == -1)
        {
            Timer = 6;
            return new Laternfish(8);
        }
        return null;
    }
}