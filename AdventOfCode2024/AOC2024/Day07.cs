using System.Collections.Generic;
using System.Linq;

namespace AOC2024
{
    class Day07 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            List<Equation> equations = new();
            foreach (var line in lines)
            {
                var split = line.Split(": ");
                long res = Convert.ToInt64(split[0]);
                List<long> numbers = new();
                foreach (var numb in split[1].Split(' '))
                {
                    numbers.Add(Convert.ToInt64(numb));
                }
                equations.Add(new Equation(res, numbers));
            }
            //Console.WriteLine(Part1(equations));
            Console.WriteLine($"Precomputed: {303766880536}");
            //Console.WriteLine(Part2(equations));
            Console.WriteLine($"Precomputed: {337041851384440}");
        }
        private long Part1(List<Equation> equations)
        {
            return equations.Where(EquationPossible).Sum(i => i.Result);
        }
        private bool EquationPossible(Equation equation)
        {
            return EquationPossible(equation.Numbers, equation.Result);
        }
        private bool EquationPossible(IEnumerable<long> numbers, long result)
        {
            if (numbers.Count() == 1) return numbers.First() == result;
            int length = numbers.Count();
            var bools = Posibilities(length - 1);
            foreach (var b in bools)
            {
                if (CalcEquation(numbers, b) == result)
                {
                    return true;
                }
            }
            return false;
        }
        private long CalcEquation(IEnumerable<long> numbers, IEnumerable<bool> PlusOrMinus)
        {
            long result = numbers.First();
            numbers = numbers.Skip(1);
            for (int i = 0; i < 100000; i++)
            {
                if (!numbers.Any())
                {
                    return result;
                }
                result = PlusOrMinus.First() ? result + numbers.First() : result * numbers.First();
                numbers = numbers.Skip(1);
                PlusOrMinus = PlusOrMinus.Skip(1);
            }
            throw new Exception($"Too long calculation in {nameof(Day07)}");
        }
        private IEnumerable<long> ConcatNumbers(IEnumerable<long> numbers, IEnumerable<bool> yes)
        {
            var numb = numbers.ToList();
            var bools = yes.ToList();
            while (numbers.Any())
            {
                if (numbers.Skip(1).Any() && yes.First())
                {
                    long l1 = numbers.First();
                    numbers = numbers.Skip(1);
                    long l2 = numbers.First();
                    yield return Concatenate(l1, l2);
                }
                else
                {
                    yield return numbers.First();
                }
                numbers = numbers.Skip(1);
                yes = yes.Skip(1);
            }
        }
        private long Concatenate(long l1, long l2)
            => Convert.ToInt64(l1.ToString() + l2.ToString());
        private List<List<bool>> GetBools(int length)
        {
            List<List<bool>> result = new();
            foreach (var pos in Posibilities(length))
            {
                result.Add(pos.ToList());
            }
            return result;
        }
        private IEnumerable<IEnumerable<bool>> Posibilities(int length)
        {
            if (length == 1)
            {
                yield return new List<bool>() { true };
                yield return new List<bool>() { false };
            }
            if (length > 1)
            {
                foreach (var posibl in Posibilities(length - 1))
                {
                    yield return posibl.Prepend(true);
                    yield return posibl.Prepend(false);
                }
            }
        }
        private IEnumerable<IEnumerable<byte>> PosibilitiesTertiary(int length)
        {
            if (length == 1)
            {
                yield return new List<byte>() { 0 };
                yield return new List<byte>() { 1 };
                yield return new List<byte>() { 2 };
            }
            if (length > 1)
            {
                foreach (var posibl in PosibilitiesTertiary(length - 1))
                {
                    yield return posibl.Prepend<byte>(0);
                    yield return posibl.Prepend<byte>(1);
                    yield return posibl.Prepend<byte>(2);
                }
            }
        }
        private long Part2(List<Equation> equations)
        {
            return equations.Where(EquationPossibleWithConcat).Sum(i => i.Result);
        }
        private bool EquationPossibleWithConcat(Equation equation)
            => EquationPossibleWithConcat(equation.Numbers, equation.Result);
        private bool EquationPossibleWithConcat(IEnumerable<long> numbers, long result)
        {
            if (numbers.Count() == 1) return numbers.First() == result;
            int length = numbers.Count();
            var tertiaries = PosibilitiesTertiary(length - 1);
            foreach (var t in tertiaries)
            {
                if (CalcEquationWithConcat(numbers, t) == result)
                {
                    return true;
                }
            }
            return false;
        }
        private long CalcEquationWithConcat(IEnumerable<long> numbers, IEnumerable<byte> PlusOrMinusOrConcat)
        {
            long result = numbers.First();
            numbers = numbers.Skip(1);
            for (int i = 0; i < 100000; i++)
            {
                if (!numbers.Any())
                {
                    return result;
                }
                switch (PlusOrMinusOrConcat.First())
                {
                    case 0:
                        result += numbers.First();
                        break;
                    case 1:
                        result *= numbers.First();
                        break;
                    case 2:
                        result = Concatenate(result, numbers.First());
                        break;
                }
                numbers = numbers.Skip(1);
                PlusOrMinusOrConcat = PlusOrMinusOrConcat.Skip(1);
            }
            throw new Exception($"Too long calculation in {nameof(Day07)}");
        }
        private class Equation
        {
            public long Result;
            public List<long> Numbers = new();
            public Equation(long result, IEnumerable<long> numbers)
            {
                Result = result;
                Numbers.AddRange(numbers);
            }
        }

    }
}