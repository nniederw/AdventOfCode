namespace AOC2024
{
    class Day17 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<string> input = lines.ToList();
            if (input.Count != 5) { throw new Exception("Input was in the wrong format in Day 17"); }
            long regA = Convert.ToInt64(input[0].Substring(12));
            long regB = Convert.ToInt64(input[1].Substring(12));
            long regC = Convert.ToInt64(input[2].Substring(12));
            List<byte> Program = new();
            foreach (var split in input[4].Substring(9).Split(','))
            {
                Program.Add(Convert.ToByte(split));
            }
            Console.WriteLine(Part1(regA, regB, regC, Program));
            Console.WriteLine(Part2(regA, regB, regC, Program));
        }
        private string Part1(long RegisterA, long RegisterB, long RegisterC, IReadOnlyList<byte> Program)
        {
            ProgramState program = new ProgramState(RegisterA, RegisterB, RegisterC, Program, 0);
            while (program.ProgramStep()) { }
            return program.StringOutput;
        }
        private long? SearchForRegA(long RegisterAStart, long SearchCount, long RegisterB, long RegisterC, IReadOnlyList<byte> Program)
        {
            Console.WriteLine($"Start Search for {RegisterAStart}-{RegisterAStart+SearchCount}");
            //var time = DateTime.Now;
            for (long i = RegisterAStart; i < SearchCount + RegisterAStart; i++)
            {
                if (i % 2000000 == 0)
                {
                    //Console.WriteLine($"Searched Rounds {RegisterAStart}-{i - 1}, step talking {(DateTime.Now - time).TotalMilliseconds}ms");
                    //time = DateTime.Now;
                }
                ProgramState program = new ProgramState(i, RegisterB, RegisterC, Program, 0);
                int lastCount = 0;
                while (program.ProgramStep())
                {
                    if (program.Output.Count == lastCount + 1)
                    {
                        if (program.Output.Count > Program.Count)
                        {
                            break;
                        }
                        if (program.Output[lastCount] != Program[lastCount])
                        {
                            break;
                        }
                        lastCount++;
                    }
                }
                if (Program.Count != program.Output.Count)
                {
                    continue;
                }
                if (Program.Zip(program.Output).Any(i => i.First != i.Second))
                {
                    continue;
                }
                return i;
            }
            Console.WriteLine($"Ended Search ({RegisterAStart}-{RegisterAStart + SearchCount}) witout finding anything");
            return null;
        }
        //searched 0 - 316240000000
        private long Part2(long RegisterA, long RegisterB, long RegisterC, IReadOnlyList<byte> Program)
        {
            //const long MaxTries = 10000000000000;
            const byte SearchThreads = 16;
            const long StartSearch = 316240000000;
            const long CountPerSearch = 50000000;

            long Start = StartSearch;
            while (true)
            {
                var time = DateTime.Now;
                List<(long startRegA, long searchCount)> ThreadValues = new();
                for (int i = 0; i < SearchThreads; i++)
                {
                    ThreadValues.Add((Start, CountPerSearch));
                    Start += CountPerSearch;
                }
                List<Task<long?>> Tasks = new();
                for (int i = 0; i < ThreadValues.Count; i++)
                {
                    var value = ThreadValues[i];
                    var task = Task.Run(() => SearchForRegA(value.startRegA, value.searchCount, RegisterB, RegisterC, Program));
                    Tasks.Add(task);
                }
                Task.WhenAll(Tasks).Wait();
                foreach (var task in Tasks)
                {
                    //task.Wait();
                    if (task.Result != null)
                    {
                        long res = task.Result.Value;
                        Console.WriteLine("----------------------------------------------------------------------------");
                        Console.WriteLine($"Found Result: {res}");
                        Console.WriteLine("----------------------------------------------------------------------------");
                        return res;
                    }
                }
                Console.WriteLine($"Ran {SearchThreads} total threads in {(DateTime.Now - time).TotalMilliseconds}ms");
            }
            /*var time = DateTime.Now;
            for (long i = 266890000000; i < MaxTries; i++)
            {
                if (i % 2000000 == 0)
                {
                    Console.WriteLine($"Testing round {i}, talking {(DateTime.Now - time).TotalMilliseconds}ms");
                    time = DateTime.Now;
                }
                ProgramState program = new ProgramState(i, RegisterB, RegisterC, Program, 0);
                int lastCount = 0;
                while (program.ProgramStep())
                {
                    if (program.Output.Count == lastCount + 1)
                    {
                        if (program.Output.Count > Program.Count)
                        {
                            break;
                        }
                        if (program.Output[lastCount] != Program[lastCount])
                        {
                            break;
                        }
                        lastCount++;
                    }
                }
                if (Program.Count != program.Output.Count)
                {
                    continue;
                }
                if (Program.Zip(program.Output).Any(i => i.First != i.Second))
                {
                    continue;
                }
                return i;
            }*/
            //throw new Exception($"Wasn't able to find the solution of day 17 part 2");
        }
        private class ProgramState
        {
            public long RegisterA = 0;
            public long RegisterB = 0;
            public long RegisterC = 0;
            public int InstructionCounter = 0;
            public IReadOnlyList<byte> Instructions;
            public string StringOutput => Output.Select(i => i.ToString()).Aggregate((a, b) => $"{a},{b}");
            public List<byte> Output = new List<byte>();
            public bool HasHalted = false;
            public ProgramState(long regA, long regB, long regC, IReadOnlyList<byte> instructions, int instrCounter = 0)
            {
                RegisterA = regA;
                RegisterB = regB;
                RegisterC = regC;
                InstructionCounter = instrCounter;
                Instructions = instructions;
            }
            //private bool HasHalted => InstructionCounter > Instructions.Count;
            private byte CurrentOpcode => Instructions[InstructionCounter];
            private byte CurrentOperand => Instructions[InstructionCounter + 1];
            private long CurrentComboOperand()
            {
                var operand = CurrentOperand;
                if (operand <= 3)
                {
                    return operand;
                }
                switch (operand)
                {
                    case 4:
                        return RegisterA;
                    case 5:
                        return RegisterB;
                    case 6:
                        return RegisterC;
                }
                throw new Exception($"Unsupported combo operand found");
            }
            private long PowerOfTwo(long exp)
            {
                return (long)1 << (int)exp;
            }
            private void IncreaseInstructionPointer()
            {
                InstructionCounter += 2;
            }
            private void adv()
            {
                //RegisterA = RegisterA / PowerOfTwo(CurrentComboOperand());
                RegisterA = RegisterA >> (int)CurrentComboOperand();
                IncreaseInstructionPointer();
            }
            private void bxl()
            {
                RegisterB = RegisterB ^ CurrentOperand;
                IncreaseInstructionPointer();
            }
            private void bst()
            {
                RegisterB = CurrentComboOperand() % 8;
                IncreaseInstructionPointer();
            }
            private void jnz()
            {
                if (RegisterA == 0)
                {
                    IncreaseInstructionPointer();
                    return;
                }
                InstructionCounter = CurrentOperand;
            }
            private void bxc()
            {
                byte operand = CurrentOperand;//for throwing
                RegisterB = RegisterB ^ RegisterC;
                IncreaseInstructionPointer();
            }
            private void OUT()
            {
                var combo = CurrentComboOperand();
                combo %= 8;
                Output.Add((byte)combo);
                IncreaseInstructionPointer();
            }
            private void bdv()
            {
                RegisterB = RegisterA >> (int)CurrentComboOperand();
                //RegisterB = RegisterA / PowerOfTwo(CurrentComboOperand());
                IncreaseInstructionPointer();
            }
            private void cdv()
            {
                RegisterC = RegisterA >> (int)CurrentComboOperand();
                //RegisterC = RegisterA / PowerOfTwo(CurrentComboOperand());
                IncreaseInstructionPointer();
            }
            //public bool RunUptoNextOutput()            {                            }
            public bool ProgramStep()
            {
                if (HasHalted) { return false; }
                try
                {
                    switch (CurrentOpcode)
                    {
                        case 0:
                            adv();
                            break;
                        case 1:
                            bxl();
                            break;
                        case 2:
                            bst();
                            break;
                        case 3:
                            jnz();
                            break;
                        case 4:
                            bxc();
                            break;
                        case 5:
                            OUT();
                            break;
                        case 6:
                            bdv();
                            break;
                        case 7:
                            cdv();
                            break;
                    }
                }
                catch
                {
                    HasHalted = true;
                }
                return !HasHalted;
            }
        }
    }
}