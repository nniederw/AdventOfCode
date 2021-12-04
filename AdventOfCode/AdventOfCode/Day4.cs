using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class Day4
{
    public static int Get()
    {
        List<int> numbers = new List<int>();
        List<BingoBoard> boards = new List<BingoBoard>();

        var path = "D:/WorkGit/nniederw/AdventOfCode2021/AdventOfCode/Input/Day4.txt";
        var enumLines = File.ReadLines(path);
        bool first = true;
        List<List<int>> bingo = new List<List<int>>();
        foreach (var line in enumLines)
        {
            if (first)
            {
                foreach (var n in line.Split(','))
                {
                    numbers.Add(Convert.ToInt32(n));
                }
                first = false;
                continue;
            }
            if (line == "")
            {
                MakeBoard();
                continue;
            }
            bingo.Add(new List<int>());
            foreach (var n in line.Split(' '))
            {
                if (n != "")
                {
                    bingo.Last().Add(Convert.ToInt32(n));
                }
            }
        }
        MakeBoard();
        return FuncPart2(numbers, boards);

        void MakeBoard()
        {
            if (bingo.Count != 0)
            {
                int[,] board = new int[bingo.Count, bingo[0].Count];
                for (int i = 0; i < bingo.Count; i++)
                {
                    for (int j = 0; j < bingo[i].Count; j++)
                    {
                        board[i, j] = bingo[i][j];
                    }
                }
                boards.Add(new BingoBoard(board));
            }
            bingo = new List<List<int>>();
        }
    }
    private static int FuncPart1(List<int> numbers, List<BingoBoard> boards)
    {
        foreach (var n in numbers)
        {
            foreach (var board in boards)
            {
                if (board.AddNumber(n))
                {
                    Console.WriteLine($"Sum {board.SumOfAllUnmarked()}, n {n}");
                    return board.SumOfAllUnmarked() * n;
                }
            }
        }
        return 0;
    }
    private static int FuncPart2(List<int> numbers, List<BingoBoard> boards)
    {
        int BoardsLeftToWin = boards.Count;
        foreach (var n in numbers)
        {
            foreach (var board in boards)
            {
                if ((!board.AlreadyWon) && board.AddNumber(n))
                {
                    if (BoardsLeftToWin == 1)
                    {
                        board.winingLine.ToList().ForEach(i => Console.Write($"{i}, "));
                        Console.WriteLine($"Sum {board.SumOfAllUnmarked()}, n {n}");
                        return board.SumOfAllUnmarked() * n;
                    }
                    else
                    {
                        BoardsLeftToWin--;
                    }
                }
            }
        }
        return 0;
    }
}

internal class BingoBoard
{
    private int[,] values = new int[0, 0];
    private bool[,] marked = new bool[0, 0];
    public int[] winingLine = null;
    public bool AlreadyWon = false;
    public BingoBoard(int[,] board)
    {
        values = board;
        marked = new bool[board.GetLength(0), board.GetLength(1)];
    }
    public bool AddNumber(int number)
    {
        var list = values.Contains(number);
        list.ForEach(i => marked[i.Item1, i.Item2] = true);
        return HasWon();
    }
    public int SumOfAllUnmarked()
    {
        int res = 0;
        for (int i = 0; i < marked.GetLength(0); i++)
        {
            for (int j = 0; j < marked.GetLength(1); j++)
            {
                if (!marked[i, j])
                {
                    res += values[i, j];
                }
            }
        }
        return res;
    }

    private bool HasWon()
    {
        for (int i = 0; i < marked.GetLength(0); i++)
        {
            bool b = true;
            for (int j = 0; j < marked.GetLength(1); j++)
            {
                if (!marked[i, j])
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
                winingLine = new int[marked.GetLength(1)];
                for (int j = 0; j < marked.GetLength(1); j++)
                {
                    winingLine[j] = values[i, j];
                }
                AlreadyWon = true;
                return true;
            }
        }
        for (int i = 0; i < marked.GetLength(1); i++)
        {
            bool b = true;
            for (int j = 0; j < marked.GetLength(0); j++)
            {
                if (!marked[j, i])
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
                winingLine = new int[marked.GetLength(0)];
                for (int j = 0; j < marked.GetLength(0); j++)
                {
                    winingLine[j] = values[j, i];
                }
                AlreadyWon = true;
                return true;
            }
        }
        return false;
    }
}
public static class Ext
{
    public static List<(int, int)> Contains<T>(this T[,] array, T item)
    {
        var res = new List<(int, int)>();
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j].Equals(item))
                {
                    res.Add((i, j));
                }
            }
        }
        return res;
    }
}