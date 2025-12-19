using System.Collections;

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
            /*foreach (var o in GetOutput(117440, 0,0))
            {
                Console.WriteLine(o);
            }
            var ps = new ProgramState(117440, 0, 0, Program);
            ps.Run();
            Console.WriteLine(ps.StringOutput);*/
            Console.WriteLine(Part1(regA, regB, regC, Program));
            //Console.WriteLine(Part2(regA, regB, regC, Program));
        }
        private string Part1(long RegisterA, long RegisterB, long RegisterC, IReadOnlyList<byte> Program)
        {
            ProgramState program = new ProgramState(RegisterA, RegisterB, RegisterC, Program, 0);
            while (program.ProgramStep()) { }
            return program.StringOutput;
        }
        private bool FirstNumberPossible(long A)
        {
            //return ((((A % 8) ^ 1) ^ 4) ^ (A >> (int)((A % 8) ^ 1))) % 8 == 2;
            return ((A & 7) ^ ((A >> (int)((A & 7) ^ 1)) & 7)) == 7;
            //            return;
            /*B = A % 8;
            B = B ^ 1;
            C = A >> (int)B;
            //A = A >> 3;
            B = B ^ 4;
            B = B ^ C;
            return B % 8 == 2;
            B = A % 8;
            B = (A % 8) ^ 1;
            C = A >> (int)((A % 8) ^ 1);
            //A = A >> 3;
            B = ((A % 8) ^ 1) ^ 4;
            B = (((A % 8) ^ 1) ^ 4) ^ (A >> (int)((A % 8) ^ 1));
            return ((((A % 8) ^ 1) ^ 4) ^ (A >> (int)((A % 8) ^ 1))) % 8 == 2;*/
        }
        private List<long> GetSmallPossibleValues()
        {
            List<long> Possible = new();
            for (int i = 0; i < 1024 + 1; i++)
            {
                if (FirstNumberPossible(i))
                {
                    Possible.Add(i);
                }
            }
            return Possible;
        }
        private long? SearchForRegA(long RegisterAStart, long SearchCount, long RegisterB, long RegisterC, IReadOnlyList<byte> Program)//, IReadOnlyList<long> PossibleValuesUnder1024)
        {
            Console.WriteLine($"Start Search for {RegisterAStart}-{RegisterAStart + SearchCount}");
            //var time = DateTime.Now;
            /*const long increment = 1024;
            const long incrementMask = increment - 1;
            foreach (var possible in PossibleValuesUnder1024)
            {
                long regAstart = RegisterAStart;
                while((regAstart & incrementMask) != possible) { regAstart++; }
//                if ((regAstart & incrementMask) != possible)
                {
  //                  regAstart += increment - (regAstart & incrementMask) + possible;
                }
                for (long i = regAstart; i < SearchCount + RegisterAStart; i += increment)
                {
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
            }*/

            for (long i = RegisterAStart; i < SearchCount + RegisterAStart; i++)
            {
                if (!FirstNumberPossible(i))//, RegisterB, RegisterC))
                { continue; }
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
        private long SearchForRegAFast(long RegisterAStart, long SearchCount, IReadOnlyList<byte> Program)//, IReadOnlyList<long> PossibleValuesUnder1024)
        {
            Console.WriteLine($"Start Search for {RegisterAStart}-{RegisterAStart + SearchCount}");
            int MaxLengthFound = 0;
            for (long regA = RegisterAStart; regA < SearchCount + RegisterAStart; regA++)
            {
                if (!FirstNumberPossible(regA)) { continue; }
                //var output = GetOutput(regA, RegisterB, RegisterC);
                var output = GetOutputFast(regA);
                int lengthMatched = 0;
                foreach (var zip in Program.Zip(output))
                {
                    if (zip.First != zip.Second) { break; }
                    lengthMatched++;
                }
                MaxLengthFound = Math.Max(lengthMatched, MaxLengthFound);
                if (lengthMatched != Program.Count)
                {
                    continue;
                }
                return regA;
            }
            //Console.WriteLine();
            Console.WriteLine($"Ended Search ({RegisterAStart}-{RegisterAStart + SearchCount}) witout finding anything, with max matching length {MaxLengthFound}");
            return -1;
        }
        //searched 0 - 924540000000
        //searched 0 - 20004990000000 with reduced set 
        private long Part2(long RegisterA, long RegisterB, long RegisterC, IReadOnlyList<byte> Program)
        {
            //const long MaxTries = 10000000000000;
            const byte SearchThreads = 16;
            const long StartSearch = 20004990000000;
            //const long StartSearch = (long)1 << 44;
            const long CountPerSearch = 50000000 * 8;
            //const long CountPerSearch = 1 << 26;

            long Start = StartSearch;
            var PossibleSmallNumbers = GetSmallPossibleValues();
            //Console.WriteLine($"Total count: {PossibleSmallNumbers.Count}");
            while (true)
            {
                var time = DateTime.Now;
                List<(long startRegA, long searchCount)> ThreadValues = new();
                for (int i = 0; i < SearchThreads; i++)
                {
                    ThreadValues.Add((Start, CountPerSearch));
                    Start += CountPerSearch;
                }
                List<Task<long>> Tasks = new();
                for (int i = 0; i < ThreadValues.Count; i++)
                {
                    var value = ThreadValues[i];
                    var task = Task.Run(() => SearchForRegAFast(value.startRegA, value.searchCount, Program));//, PossibleSmallNumbers));
                    Tasks.Add(task);
                }
                Task.WhenAll(Tasks).Wait();
                foreach (var task in Tasks)
                {
                    //task.Wait();
                    if (task.Result != -1)
                    {
                        long res = task.Result;
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
        private IEnumerable<byte> GetOutput(long RegA, long RegB, long RegC)
        {
            do
            {
                RegB = RegA & 7;
                RegB = RegB ^ 1;
                RegC = RegA >> (int)RegB;
                RegA = RegA >> 3;
                RegB = RegB ^ 4;
                RegB = RegB ^ RegC;
                yield return (byte)(RegB & 7);
            }
            while (RegA != 0);
        }
        private IEnumerable<byte> GetOutputFast(long RegA)
        {
            long RegB = 0;
            long RegC = 0;
            do
            {
                RegB = (RegA & 7) ^ 1;
                RegC = RegA >> (int)RegB;
                RegA = RegA >> 3;
                yield return (byte)((RegB ^ RegC ^ 4) & 7);
            }
            while (RegA != 0);
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
                //RegisterB = CurrentComboOperand() % 8;
                RegisterB = CurrentComboOperand() & 7;
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
                Output.Add((byte)(CurrentComboOperand() & 7));
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
            public void Run()
            {
                while (ProgramStep()) ;
            }
        }
    }
}