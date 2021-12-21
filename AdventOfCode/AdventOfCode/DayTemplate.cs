using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Helpers;

namespace AdventOfCode
{
    public static class DayTemplate<InputType>
    {
        public static long GetP1() => FuncPart1(GetInput());
        public static long GetP2() => FuncPart2(GetInput());
        private static InputType GetInput()
        {
            //foreach (var line in File.ReadAllLines(Program.InputFolderPath + "/Day11.txt"))
            return default;
        }
        private static long FuncPart1(InputType input)
        {
            return default;
        }

        private static long FuncPart2(InputType input)
        {
            return default;
        }
    }
}