using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

namespace AdventOfCode
{
    public static class Day13
    {
        private static int Height = 15;
        private static int Width = 11;
        public static long GetP1()
        {
            var input = GetInput();
            return FuncPart1(input.Item1, input.Item2);
        }
        public static long GetP2()
        {
            var input = GetInput();
            return FuncPart2(input.Item1, input.Item2);
        }
        private static (List<(int, int)>, List<bool>) GetInput()
        {
            var points = new List<(int, int)>();
            var folds = new List<bool>();
            bool Folding = false;
            Width = 1311;
            // Width = 15;
            Height = 895;
            // Height = 11;
            foreach (var line in File.ReadAllLines(Program.InputFolderPath + "/Day13.txt"))
            {
                if (line == "")
                {
                    Folding = true;
                    continue;
                }
                if (Folding)
                {
                    folds.Add(line[11] == 'x');
                }
                else
                {
                    var point = line.Split(',');
                    points.Add((Convert.ToInt32(point[0]), Convert.ToInt32(point[1])));
                }

            }
            return (points, folds);
        }
        private static long FuncPart1(List<(int, int)> points, List<bool> folds)
        {
            bool[,] holes = new bool[Width, Height];
            points.ForEach(i => holes[i.Item1, i.Item2] = true);
            holes = Fold(holes, folds[0]);
            return holes.GetEnumerator().GetEnumerable<bool>().Sum(i => i ? 1 : 0);
        }
        private static bool[,] Fold(bool[,] array, bool x)
        {
            bool[,] res;
            if (x)
            {
                int lengthX = array.GetLength(0) / 2;
                res = new bool[lengthX, array.GetLength(1)];
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (array[i, j])
                        {
                            if (i < res.GetLength(0))
                            {
                                res[i, j] = true;
                            }
                            else
                            {
                                int index = i - lengthX;
                                index = lengthX - index;
                                res[index, j] = true;
                            }
                        }
                    }
                }
            }
            else
            {
                int lengthY = array.GetLength(1) / 2;
                res = new bool[array.GetLength(0), lengthY];
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (array[i, j])
                        {
                            if (j < res.GetLength(1))
                            {
                                res[i, j] = true;
                            }
                            else
                            {
                                int index = j - lengthY;
                                index = lengthY - index;
                                res[i, index] = true;
                            }
                        }
                    }
                }
            }
            return res;
        }

        private static long FuncPart2(List<(int, int)> points, List<bool> folds)
        {
            bool[,] holes = new bool[Width, Height];
            points.ForEach(i => holes[i.Item1, i.Item2] = true);
            folds.ForEach(i => holes = Fold(holes, i));
            for (int i = 0; i < holes.GetLength(1); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < holes.GetLength(0); j++)
                {
                    Console.Write(holes[j,i]?'#':'.');
                }
            }
            Console.WriteLine();
            return -1;
        }
    }
}