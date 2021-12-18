using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

public static class Day10
{
    private static Dictionary<char, int> GetPoints = new Dictionary<char, int>();
    private static Dictionary<char, int> GetMultPoints = new Dictionary<char, int>();
    private static readonly List<char> OpenChars = new List<char>(new char[] {'(','[','{','<' });
    private static readonly List<char> CloseChars = new List<char>(new char[] {')',']','}','>' });
    private static readonly Dictionary<char,char> GetClosingChars = new Dictionary<char, char>(new KeyValuePair<char,char>[] { KeyValuePair.Create('(', ')') , KeyValuePair.Create('[', ']'), KeyValuePair.Create('{', '}'), KeyValuePair.Create('<', '>') });
    public static int GetP1() => FuncPart1(GetInput());
    public static long GetP2() => FuncPart2(GetInput());
    private static void FillPoints()
    {
        GetPoints.Add(default, 0);
        GetPoints.Add('(', 3);
        GetPoints.Add('[', 57);
        GetPoints.Add('{', 1197);
        GetPoints.Add('<', 25137);
        GetPoints.Add(')', 3);
        GetPoints.Add(']', 57);
        GetPoints.Add('}', 1197);
        GetPoints.Add('>', 25137);
        GetMultPoints.Add(')', 1);
        GetMultPoints.Add(']', 2);
        GetMultPoints.Add('}', 3);
        GetMultPoints.Add('>', 4);
    }
    private static List<string> GetInput()
    {
        FillPoints();
        return File.ReadAllLines("D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day10.txt").ToList();
    }
    private static int FuncPart1(List<string> lines) => lines.Sum(i => { return GetPoints[EvaluateLine(i)]; });
    private static char EvaluateLine(string line)
    {
        List<char> opened = new List<char>();
        foreach (var c in line)
        {
            if (OpenChars.Contains(c))
            {
                opened.Add(c);
            }
            else
            {
                if (GetClosingChars[opened.Last()] != c)
                {
                    return c;
                }
                opened.RemoveAt(opened.Count - 1);
            }
        }
        return default;
    }
    private static long FuncPart2(List<string> lines)
    {
        lines = lines.FindAll(i => EvaluateLine(i) == default);
        var BadLines = new List<List<char>>();
        lines.ForEach(i => BadLines.Add(CloseLine(i)));
        var Scores = new List<long>();
        BadLines.ForEach(i => Scores.Add(CalcScore(i)));
        Scores.Sort();
        return Scores[(Scores.Count-1)/2];
    }
    private static List<char> CloseLine(string line)
    {
        List<char> opened = new List<char>();
        foreach (var c in line)
        {
            if (OpenChars.Contains(c))
            {
                opened.Add(c);
            }
            else
            {
                opened.RemoveAt(opened.Count - 1);
            }
        }
        List<char> result = new List<char>();
        for (int i = opened.Count-1; i >=0; i--)
        {
            result.Add(GetClosingChars[opened[i]]);
        }
        return result;
    }
    private static long CalcScore(List<char> line)
    {
        long result = 0;
        foreach (var c in line)
        {
            result *= 5;
            result += GetMultPoints[c];
        }
        return result;
    }
}