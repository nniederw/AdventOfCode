namespace AOC2024
{
    class Day03 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            string input = "";
            foreach (var line in lines)
            {
                input += line;
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private int Part1(string input)
        {
            const int MinLength = 8;
            const int MaxLength = 12;
            List<string> ValidSyntax = new();
            for (int i = 0; i < input.Length - MinLength; i++)
            {
                for (int j = MinLength; j <= MaxLength; j++)
                {
                    if (i + j < input.Length)
                    {
                        string sub = input.Substring(i, j);
                        if (IsValidMultiplySyntax(sub))
                        {
                            ValidSyntax.Add(sub);
                        }
                    }
                }
            }
            return ValidSyntax.Sum(i => Multiply(i));
        }
        private bool IsValidSyntax(string instruction)
        {
            return IsValidDoDontSyntax(instruction) || IsValidMultiplySyntax(instruction);
        }
        private bool IsValidMultiplySyntax(string instruction)
        {
            if (instruction == null) return false;
            if (instruction.Length < 8) return false;
            if (instruction.Length > 12) return false;
            if (instruction.Substring(0, 4) != "mul(") return false;
            if (instruction.Last() != ')') return false;
            instruction = instruction.Substring(4);
            instruction = instruction.Substring(0, instruction.Length - 1);
            var split = instruction.Split(',');
            if (split.Length != 2) return false;
            if (!ContainsOnlyNumbers(split[0])) return false;
            if (!ContainsOnlyNumbers(split[1])) return false;
            return true;
        }
        private bool IsValidDoDontSyntax(string instruction)
        {
            if (instruction == null) return false;
            if (instruction == "do()") return true;
            if (instruction == "don't()") return true;
            return false;
        }
        private enum Instruction { Do, Dont, MUL }
        private int Multiply(string command)
        {
            command = command.Substring(4);
            command = command.Substring(0, command.Length - 1);
            var split = command.Split(',');
            int n1 = Convert.ToInt32(split[0]);
            int n2 = Convert.ToInt32(split[1]);
            return n1 * n2;
        }
        private Instruction GetInstruction(string instruction)
        {
            if (instruction == "do()") return Instruction.Do;
            if (instruction == "don't()") return Instruction.Dont;
            if (!IsValidMultiplySyntax(instruction)) throw new Exception();
            return Instruction.MUL;
        }
        private bool ContainsOnlyNumbers(string str)
        {
            string numbers = "0123456789";
            return str.All(i => numbers.Contains(i));
        }
        private int Part2(string input)
        {
            const int MinLength = 4;
            const int MaxLength = 12;
            List<string> ValidSyntax = new();
            for (int i = 0; i < input.Length - MinLength; i++)
            {
                for (int j = MinLength; j <= MaxLength; j++)
                {
                    if (i + j < input.Length)
                    {
                        string sub = input.Substring(i, j);
                        if (IsValidSyntax(sub))
                        {
                            ValidSyntax.Add(sub);
                        }
                    }
                }
            }
            int result = 0;
            bool enabled = true;
            foreach (string str in ValidSyntax)
            {
                switch (GetInstruction(str))
                {
                    case Instruction.Do:
                        enabled = true; break;
                    case Instruction.Dont:
                        enabled = false; break;
                    case Instruction.MUL:
                        if (enabled)
                        {
                            result += Multiply(str);
                        }
                        break;
                }
            }
            return result;// ValidSyntax.Sum(i => Multiply(i));
        }
    }
}