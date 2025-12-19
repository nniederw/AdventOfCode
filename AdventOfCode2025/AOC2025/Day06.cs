using System.Text.RegularExpressions;

namespace AOC2025
{
    class Day06 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}").ToList();
            List<List<long>> numbers = new();
            List<char> operations = new();
            foreach (var line in lines)
            {
                if (line.Contains('+') || line.Contains('*'))
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        char c = line[i];
                        if (c == '+' || c == '*')
                        {
                            operations.Add(c);
                        }
                    }
                    continue;
                }
                List<long> ns = new();
                var stripped = Regex.Replace(line, @"\s+", " ").Trim();
                var split = stripped.Split(' ');
                foreach (var s in split)
                {
                    ns.Add(Convert.ToInt64(s));
                }
                numbers.Add(ns);
            }
            Console.WriteLine(Part1(numbers, operations));
            char[,] rawArray = Input.InterpretStringsAs2DCharArray(lines);
            Console.WriteLine(Part2(rawArray));
        }
        private long Part1(List<List<long>> numbers, List<char> operations)
        {
            long result = 0;
            for (int i = 0; i < operations.Count; i++)
            {
                if (operations[i] == '+')
                {
                    result += numbers.Select(j => j[i]).Sum();
                }
                else if (operations[i] == '*')
                {
                    result += numbers.Select(j => j[i]).Aggregate((a, b) => a * b);
                }
                else
                {
                    throw new Exception("Unsupported operation in day 6");
                }
            }
            return result;
        }
        private long Part2(char[,] rawArray)
        {
            var operationY = rawArray.GetLength(1) - 1;
            int sizeX = rawArray.GetLength(0);
            List<(int start, int end, char operation)> operationRanges = new(); //end exclusive
            int start = 0;
            char operation = rawArray[0, operationY];
            for (int x = 1; x < sizeX; x++)
            {
                char c = rawArray[x, operationY];
                if (c != ' ')
                {
                    operationRanges.Add((start, x, operation));
                    start = x;
                    operation = c;
                }
            }
            operationRanges.Add((start, sizeX, operation));
            long result = 0;
            foreach (var range in operationRanges)
            {
                List<long> numbers = new();
                for (int i = range.start; i < range.end; i++)
                {
                    string digit = "";
                    for (int y = 0; y < operationY; y++)
                    {
                        char c = rawArray[i, y];
                        if (c != ' ')
                        {
                            digit += c;
                        }
                    }
                    if(digit != "")
                    {
                        numbers.Add(Convert.ToInt64(digit));
                    }
                }
                if (range.operation == '+')
                {
                    result += numbers.Sum();
                }
                else if (range.operation == '*')
                {
                    result += numbers.Aggregate((a, b) => a * b);
                }
                else
                {
                    throw new Exception("Unsupported operation in day 6");
                }
            }
            return result;
        }
    }
}