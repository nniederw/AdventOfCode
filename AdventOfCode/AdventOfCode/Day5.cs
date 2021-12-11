using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;

public static class Day5
{
    private const int BiggestNumber = 1000;
    public static int Get()
    {
        var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day5.txt";
        var enumLines = File.ReadLines(path);
        var list = new List<((int x, int y) start, (int x, int y) end)>();
        foreach (var InputLine in enumLines)
        {
            var str = InputLine.Split(" -> ");
            ((int x, int y) start, (int x, int y) end) line;
            var start = str[0].Split(",");
            var end = str[1].Split(",");
            line.start = (Convert.ToInt32(start[0]), Convert.ToInt32(start[1]));
            line.end = (Convert.ToInt32(end[0]), Convert.ToInt32(end[1]));
            list.Add(line);
        }
        return FuncPart2(list);
    }
    private static int FuncPart1(List<((int x, int y) start, (int x, int y) end)> lines)
    {
        var Points = new int[BiggestNumber, BiggestNumber];
        lines.FindAll(line => line.start.x == line.end.x || line.start.y == line.end.y)
        .ForEach(line =>
        {
            if (line.start.x == line.end.x)
            {
                var x = line.start.x;
                for (int y = Math.Min(line.start.y, line.end.y); y <= Math.Max(line.start.y, line.end.y); y++)
                { Points[x, y]++; }
            }
            else
            {
                var y = line.start.y;
                for (int x = Math.Min(line.start.x, line.end.x); x <= Math.Max(line.start.x, line.end.x); x++)
                { Points[x, y]++; }
            }
        });
        return Points.Cast<int>().Sum(i => i > 1 ? 1 : 0);
    }
    private static int FuncPart2(List<((int x, int y) start, (int x, int y) end)> lines)
    {
        var Points = new int[BiggestNumber, BiggestNumber];
        lines.ForEach(line =>
        {
            if (line.start.x == line.end.x || line.start.y == line.end.y)
            {
                if (line.start.x == line.end.x)
                {
                    var x = line.start.x;
                    for (int y = Math.Min(line.start.y, line.end.y); y <= Math.Max(line.start.y, line.end.y); y++)
                    { Points[x, y]++; }
                }
                else
                {
                    var y = line.start.y;
                    for (int x = Math.Min(line.start.x, line.end.x); x <= Math.Max(line.start.x, line.end.x); x++)
                    { Points[x, y]++; }
                }
            }
            else
            {
                int xmin = Math.Min(line.start.x, line.end.x);
                int xmax = Math.Max(line.start.x, line.end.x);
                int ymin = Math.Min(line.start.y, line.end.y);
                int ymax = Math.Max(line.start.y, line.end.y);
                if (xmin == line.start.x && ymin != line.start.y)
                {
                    for (int x = xmin, y = ymax; x <= xmax && y >= ymin; x++, y--)
                    {
                        Points[x, y]++;
                    }
                }
                else if (xmin != line.start.x && ymin == line.start.y)
                {
                    for (int x = xmax, y = ymin; x >= xmin && y <= ymax; x--, y++)
                    {
                        Points[x, y]++;
                    }
                }
                else
                {
                    for (int x = xmin, y = ymin; x <= xmax && y <= ymax; x++, y++)
                    {
                        Points[x, y]++;
                    }
                }
            }
        });
        return Points.Cast<int>().Sum(i => i > 1 ? 1 : 0);
    }
}