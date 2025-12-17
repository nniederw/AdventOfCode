namespace AOC2024
{
    class Day01 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<(bool left, ulong turn)> input = new();
            foreach (var line in lines)
            {
                input.Add((line[0] == 'L', Convert.ToUInt64(line.Substring(1))));
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private IEnumerable<long> TurnIntoSInts(IEnumerable<(bool left, ulong turn)> input)
            => input.Select(i => i.left ? -(long)i.turn : (long)i.turn);
        private IEnumerable<ModNumber> TurnIntsIntoModNumbers(IEnumerable<long> longs, int modulo) => longs.Select(i => new ModNumber(i, modulo));
        private IEnumerable<ModNumber> TurnIntoMod(IEnumerable<(bool left, ulong turn)> input)
            => TurnIntsIntoModNumbers(TurnIntoSInts(input), 100);
        private long Part1(List<(bool left, ulong turn)> input)
        {
            long result = 0;
            ModNumber number = new ModNumber(50, 100);
            foreach (var i in TurnIntoMod(input))
            {
                number += i;
                if (number.Number == 0)
                {
                    result++;
                }
            }
            return result;
        }
        private long Part2(List<(bool left, ulong turn)> input)
        {
            long result = 0;
            ModNumber number = new ModNumber(50,100);
            foreach (var line in input)
            {
                var turnMod = new ModNumber(1, 100);
                if (line.left)
                {
                    turnMod = new ModNumber(-1, 100);
                }
                for (ulong i = 0; i < line.turn; i++)
                {
                    number += turnMod;
                    if (number.Number == 0)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
    }
    public struct ModNumber
    {
        private long _Number = 0;
        public long Number
        {
            get { return _Number; }
            set
            {
                _Number = ((value % Modulo) + Modulo) % Modulo;
            }
        }
        public readonly long Modulo = 1;
        public ModNumber(long number, long modulo)
        {
            Modulo = modulo;
            Number = number;
        }
        public static ModNumber operator +(ModNumber a, ModNumber b)
        {
            if (a.Modulo != b.Modulo) { throw new Exception("Modulo has to be equal for numbers to be added."); }
            return new ModNumber(a._Number + b._Number, b.Modulo);
        }
    }
}