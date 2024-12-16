namespace AOC2024
{
    class Day13 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<string> input = new();
            foreach (var line in lines)
            {
                if (line != "")
                {
                    input.Add(line);
                }
            }
            List<SlotMachine> slotMachines = new();
            if (input.Count % 3 != 0) { throw new Exception($"Input of {nameof(Day13)} was in a wrong format"); }
            for (int i = 0; i < input.Count; i += 3)
            {
                var s1 = input[i].Substring(12).Split(", Y+");
                var s2 = input[i + 1].Substring(12).Split(", Y+");
                var s3 = input[i + 2].Substring(9).Split(", Y=");
                (int x, int y) a = (Convert.ToInt32(s1[0]), Convert.ToInt32(s1[1]));
                (int x, int y) b = (Convert.ToInt32(s2[0]), Convert.ToInt32(s2[1]));
                (int x, int y) prize = (Convert.ToInt32(s3[0]), Convert.ToInt32(s3[1]));
                slotMachines.Add(new SlotMachine(a, b, prize));
            }
            Console.WriteLine(Part1(slotMachines));
            Console.WriteLine(Part2(slotMachines));
        }
        private bool MutliSolution(SlotMachine machine)
        {
            var A = machine.A;
            var B = machine.B;
            var P = machine.Prize;
            var RatA = new LongRational(A.x, A.y);
            var RatB = new LongRational(B.x, B.y);
            var RatP = new LongRational(P.x, P.y);
            return (RatA == RatB) && (RatB == RatP);
        }
        private bool HasZeroInIt(SlotMachine machine)
        {
            return machine.A.x == 0 || machine.A.y == 0 || machine.B.x == 0 || machine.B.y == 0 || machine.Prize.x == 0 || machine.Prize.y == 0;
        }
        private long? TokensNeeded(SlotMachine machine, bool CheckUnder100 = true)
        {
            if (HasZeroInIt(machine)) { return null; } //these cases aren't handled currently
            var A = machine.A;
            var B = machine.B;
            var P = machine.Prize;
            long ax = A.x;
            long ay = A.y;
            long bx = B.x;
            long by = B.y;
            long px = P.x;
            long py = P.y;
            //try to make k*A + m*B = P
            long mtop = py * ax - ay * px;
            long mbot = by * ax - ay * bx;
            if (mbot == 0) { return null; }
            var RatM = new LongRational(mtop, mbot);
            if (!RatM.IsInteger()) { return null; }
            long m = RatM.Numerator;
            var RatK = new LongRational(px - m * bx, ax);
            if (!RatK.IsInteger()) { return null; }
            long k = RatK.Numerator;
            if (CheckUnder100 && (k > 100 || m > 100)) { return null; }
            if (k < 0 || m < 0) { return null; }
            return k * 3 + m;
        }
        private long Part1(IReadOnlyList<SlotMachine> SlotMachines)
        {
            if (SlotMachines.Where(MutliSolution).Any()) throw new Exception($"Multisolutions aren't currently handled in Day13");
            if (SlotMachines.Where(HasZeroInIt).Any()) throw new Exception($"Slotmachines can't contain zeros currently handled in Day13");
            return SlotMachines.Select(i => TokensNeeded(i)).Where(i => (i != null)).Select(i => i.Value).Sum();
        }

        private long Part2(IReadOnlyList<SlotMachine> SlotMachines)
        {
            long error = 10000000000000;
            var CorrectedMachines = SlotMachines.Select(i => new SlotMachine(i.A, i.B, (i.Prize.x + error, i.Prize.y + error))).ToList();
            if (CorrectedMachines.Where(MutliSolution).Any()) throw new Exception($"Multisolutions aren't currently handled in Day13");
            if (CorrectedMachines.Where(HasZeroInIt).Any()) throw new Exception($"Slotmachines can't contain zeros currently handled in Day13");
            return CorrectedMachines.Select(i => TokensNeeded(i, false)).Where(i => (i != null)).Select(i => i.Value).Sum(); ;
        }
        private struct SlotMachine
        {
            public (long x, long y) A;
            public (long x, long y) B;
            public (long x, long y) Prize;
            public SlotMachine((long x, long y) a, (long x, long y) b, (long x, long y) prize)
            {
                A = a;
                B = b;
                Prize = prize;
            }
        }
    }
}