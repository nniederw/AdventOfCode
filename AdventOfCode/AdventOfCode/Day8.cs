using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

public static class Day8
{
    public static int GetP1() => FuncPart1(GetInput());
    public static int GetP2() => FuncPart2(GetInput());
    private static List<(List<List<char>> number, List<List<char>> solutions)> GetInput()
    {
        List<(List<List<char>> number, List<List<char>> solutions)> result = new List<(List<List<char>> number, List<List<char>> solutions)>();
        foreach (var line in File.ReadAllLines("D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day8Small.txt"))
        {
            List<List<char>> numbers = new List<List<char>>();
            List<List<char>> solutions = new List<List<char>>();
            var input = line.Split(" | ");
            input[0].Split(" ").Foreach(i => numbers.Add(i.ToCharArray().ToList()));
            input[1].Split(" ").Foreach(i => solutions.Add(i.ToCharArray().ToList()));
            result.Add((numbers, solutions));
        }
        return result;
    }
    private static int FuncPart1(List<(List<List<char>> number, List<List<char>> solutions)> list) => list.Sum(i => FuncPart1(i.number, i.solutions));

    private static int FuncPart1(List<List<char>> numbers, List<List<char>> solutions)
    {
        return solutions.Sum(i => i.Count == 2 || i.Count == 3 || i.Count == 4 || i.Count == 7 ? 1 : 0);
    }
    private static int FuncPart2(List<(List<List<char>> number, List<List<char>> solutions)> list) => list.Sum(i => FuncPart2(i.number, i.solutions));
    private static int FuncPart2(List<List<char>> numbers, List<List<char>> solutions)
    {
        List<char> Numb1 = numbers.Find(i => i.Count == 2);
        List<char> Numb7 = numbers.Find(i => i.Count == 3);
        List<char> Numb4 = numbers.Find(i => i.Count == 4);
        List<List<char>> Numb235 = numbers.FindAll(i => i.Count == 5);
        List<List<char>> Numb069 = numbers.FindAll(i => i.Count == 6);
        //ignore 8 because it gives us no information
        Dictionary<char, char> translation = new Dictionary<char, char>();
        char a = Numb7.Except(Numb1).First();
        var Numb234without4 = new List<List<char>>();
        Numb235.ForEach(i => Numb234without4.Add(i.Except(Numb4).ToList()));
        char g = Numb234without4.Find(i => i.Count == 2).Except(new[] { a }).First();
        char e = Numb234without4.Find(i => i.Count == 3).Except(new[] { a, g }).First();
        var Numb2 = Numb235.Find(i => i.Contains(e));
        char d = Numb2.Except(Numb7).Except(new[] { e, g }).First();
        var Numb0 = Numb069.Find(i => !i.Contains(d));
        char b = Numb0.Except(Numb7).Except(new[] { e, g }).First();
        var Numb9 = Numb069.Find(i => !i.Contains(e));
        char f = Numb9.Except(Numb2).Except(new[] { b }).First();
        char c = Numb1.Except(new[] { f }).First();
        translation[a] = 'a';
        translation[b] = 'b';
        translation[c] = 'c';
        translation[d] = 'd';
        translation[e] = 'e';
        translation[f] = 'f';
        translation[g] = 'g';

        solutions.ForEach(list =>
        {
            for (int i = 0; i < list.Count; i++)
            { list[i] = translation[list[i]]; }
        });
        List<int> results = new List<int>();
        int res = 0;
        int times10 = 1;
        for (int i = results.Count - 1; i >= 0; i--)
        {
            res = results[i] * times10;
            times10 *= 10;
        }
        return res;
    }
    private static int GetNumberUnsafe(List<char> number)
    {
        number.Sort();
        if (number.Count == 2) { return 1; }
        if (number.Count == 3) { return 7; }
        if (number.Count == 4) { return 4; }
        if (number.Count == 5)
        {
            if (number[3] == 'e') { return 2; }
            if (number[1] == 'c') { return 3; }
            return 5;
        }
        if (number.Count == 6)
        {
            if (number[2] == 'c' && number[3] == 'e') { return 0; }
            if (number[2] == 'd') { return 6; }
            return 9;
        }
        if (number.Count == 7) { return 8; }
        throw new Exception();
    }
}