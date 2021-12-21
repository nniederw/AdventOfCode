using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

namespace AdventOfCode
{
    public static class Day11
    {
        private const int Height = 10;
        private const int Width = 10;
        private const int Iterations = 100;
        public static int GetP1() => FuncPart1(GetInput());
        public static long GetP2() => FuncPart2(GetInput());
        private static int[,] GetInput()
        {
            int[,] res = new int[Height, Width];
            int i = 0;
            foreach (var line in File.ReadAllLines(Program.InputFolderPath + "/Day11.txt"))
            {
                for (int j = 0; j < Width; j++)
                {
                    res[i, j] = Convert.ToInt32("" + line[j]);
                }
                i++;
            }
            return res;
        }
        private static int FuncPart1(int[,] input)
        {
            int flashes = 0;
            Oktopus[,] oktos = new Oktopus[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    oktos[i, j] = new Oktopus(input[i, j]);
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    List<Oktopus> neighbors = new List<Oktopus>();
                    neighbors.Add(i > 0 ? oktos[i - 1, j] : null);
                    neighbors.Add(i > 0 && j < Width - 1 ? oktos[i - 1, j + 1] : null);
                    neighbors.Add(j < Width - 1 ? oktos[i, j + 1] : null);
                    neighbors.Add(i < Height - 1 && j < Width - 1 ? oktos[i + 1, j + 1] : null);
                    neighbors.Add(i < Height - 1 ? oktos[i + 1, j] : null);
                    neighbors.Add(i < Height - 1 && j > 0 ? oktos[i + 1, j - 1] : null);
                    neighbors.Add(j > 0 ? oktos[i, j - 1] : null);
                    neighbors.Add(i > 0 && j > 0 ? oktos[i - 1, j - 1] : null);
                    oktos[i, j].Neighbors = neighbors.FindAll(i => i != null);
                }
            }
            var Oktopuses = new List<Oktopus>();
            oktos.Foreach(i => Oktopuses.Add(i));
            for (int i = 0; i < Iterations; i++)
            {
                /*  for (int j = 0; j < Oktopuses.Count; j++)
                  {
                      if (j % 10 == 0) { Console.WriteLine(); }
                      Console.Write(Oktopuses[j].EnergyLevel);
                  }
                  Console.WriteLine();*/
                Oktopuses.ForEach(i => i.IncrementEnergy());
                //Console.WriteLine(Oktopuses.Sum(i => i.HasHighEnergy() ? 1 : 0));
                while (!Oktopuses.TrueForAll(i => !i.HasHighEnergy()))
                {
                    flashes += Oktopuses.FindAll(i => i.HasHighEnergy()).Sum(i => i.Update());
                }
                Oktopuses.ForEach(i => i.ResetFlash());
            }
            return flashes;
        }

        private static long FuncPart2(int[,] input)
        {
            int flashes = 0;
            Oktopus[,] oktos = new Oktopus[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    oktos[i, j] = new Oktopus(input[i, j]);
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    List<Oktopus> neighbors = new List<Oktopus>();
                    neighbors.Add(i > 0 ? oktos[i - 1, j] : null);
                    neighbors.Add(i > 0 && j < Width - 1 ? oktos[i - 1, j + 1] : null);
                    neighbors.Add(j < Width - 1 ? oktos[i, j + 1] : null);
                    neighbors.Add(i < Height - 1 && j < Width - 1 ? oktos[i + 1, j + 1] : null);
                    neighbors.Add(i < Height - 1 ? oktos[i + 1, j] : null);
                    neighbors.Add(i < Height - 1 && j > 0 ? oktos[i + 1, j - 1] : null);
                    neighbors.Add(j > 0 ? oktos[i, j - 1] : null);
                    neighbors.Add(i > 0 && j > 0 ? oktos[i - 1, j - 1] : null);
                    oktos[i, j].Neighbors = neighbors.FindAll(i => i != null);
                }
            }
            var Oktopuses = new List<Oktopus>();
            oktos.Foreach(i => Oktopuses.Add(i));
            for (int i = 0; i < 10000; i++)
            {
                /*  for (int j = 0; j < Oktopuses.Count; j++)
                  {
                      if (j % 10 == 0) { Console.WriteLine(); }
                      Console.Write(Oktopuses[j].EnergyLevel);
                  }
                  Console.WriteLine();*/
                Oktopuses.ForEach(i => i.IncrementEnergy());
                //Console.WriteLine(Oktopuses.Sum(i => i.HasHighEnergy() ? 1 : 0));
                while (!Oktopuses.TrueForAll(i => !i.HasHighEnergy()))
                {
                    flashes += Oktopuses.FindAll(i => i.HasHighEnergy()).Sum(i => i.Update());
                }
                Oktopuses.ForEach(i => i.ResetFlash());
                if (Oktopuses.TrueForAll(i => i.EnergyLevel == 0))
                {
                    return i + 1;
                }
            }
            throw new Exception();
        }
    }
    class Oktopus
    {
        public int EnergyLevel = 0;
        public List<Oktopus> Neighbors = new List<Oktopus>();
        public bool Flashed = false;
        public Oktopus(int energyLevel)
        {
            EnergyLevel = energyLevel;
        }
        public void IncrementEnergy() => EnergyLevel++;
        public int Update()
        {
            if (!Flashed && HasHighEnergy())
            {
                Flash();
                return 1;
            }
            return 0;
        }
        public bool HasHighEnergy() => EnergyLevel > 9;
        private void Flash()
        {
            if (!Flashed)
            {
                Neighbors.ForEach(i => i.IncrementEnergy());
                Flashed = true;
                EnergyLevel = int.MinValue;
            }
        }
        public void ResetFlash()
        {
            if (Flashed)
            {
                EnergyLevel = 0;
                Flashed = false;
            }
        }
    }
}