namespace AOC2022
{
    class Day11
    {
        public static void Start()
        {
            ReadMonkeysIn();
            Console.WriteLine(Part1());
            Monkey.Monkeys = new();
            ReadMonkeysIn();
            Console.WriteLine(Part2());
        }
        private static void ReadMonkeysIn()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day11.txt");
            List<Monkey> monkeys = new();
            int input = new();
            foreach (var line in lines)
            {
                if (line == "") continue;
                var l = line.Split(' ').Where(i => i != "").ToArray();
                switch (l[0])
                {
                    case "Monkey":
                        monkeys.Add(new Monkey());
                        break;
                    case "Starting":
                        {
                            var monk = monkeys.Last();
                            foreach (var item in l.Skip(2))
                            {
                                monk.StartItems.Add(Convert.ToInt32(item.TrimEnd(',')));
                            }
                        }
                        break;
                    case "Operation:":
                        monkeys.Last().Operation = new Operation(l[3], l[4][0], l[5]).Apply;
                        break;
                    case "Test:":
                        int value = Convert.ToInt32(l[3]);
                        monkeys.Last().Test = (long i) => i % value == 0;
                        monkeys.Last().TestValue = value;
                        break;

                    case "If":
                        if (l[1] == "true:")
                        {
                            monkeys.Last().IfTrue = (long item) => Monkey.ThrowTo(item, Convert.ToInt32(l[5]));
                        }
                        else
                        {
                            monkeys.Last().IfFalse = (long item) => Monkey.ThrowTo(item, Convert.ToInt32(l[5]));
                        }
                        break;
                }
            }
            for (int i = 0; i < monkeys.Count; i++)
            {
                if (Monkey.Monkeys[i] != monkeys[i]) throw new Exception();
            }
        }
        private static long Part1()
        {
            var monkeys = Monkey.Monkeys;
            for (int i = 0; i < 20; i++)
            {
                foreach (var monk in monkeys)
                {
                    monk.Act(i => i / 3);
                }
            }
            monkeys.Sort((a, b) => b.ItemsInspected.CompareTo(a.ItemsInspected));
            return monkeys[0].ItemsInspected * monkeys[1].ItemsInspected;
        }
        private static long Part2()
        {
            var monkeys = Monkey.Monkeys;
            int modulo = 1;
            foreach (var monk in monkeys)
            {
                modulo *= monk.TestValue;
            }
            for (int i = 0; i < 10000; i++)
            {
                foreach (var monk in monkeys)
                {
                    monk.Act(i => i % modulo);
                }
            }
            monkeys.Sort((a, b) => b.ItemsInspected.CompareTo(a.ItemsInspected));
            return monkeys[0].ItemsInspected * monkeys[1].ItemsInspected;
        }
    }
    public class Monkey
    {
        public List<long> StartItems = new();
        public Func<long, long>? Operation = null;
        public Predicate<long>? Test = null;
        public int TestValue = 0;
        public Action<long>? IfTrue = null;
        public Action<long>? IfFalse = null;
        public static List<Monkey> Monkeys = new();
        public long ItemsInspected = 0;
        public Monkey()
        {
            Monkeys.Add(this);
        }
        private void ThrowItem(long item)
        {
            if (Test(item))
            {
                IfTrue(item);
            }
            else
            {
                IfFalse(item);
            }
        }
        public static void ThrowTo(long item, int index)
        {
            Monkeys[index].StartItems.Add(item);
        }
        public void Act(Func<long, long> levelChange)
        {
            while (StartItems.Any())
            {
                long item = StartItems[0];
                StartItems.RemoveAt(0);
                item = Operation(item);
                item = levelChange(item);
                ThrowItem(item);
                ItemsInspected++;
            }
        }
    }
    public class Operation
    {
        long value = int.MinValue;
        private enum Identifier { value, old }
        private Func<long, long, long> Op = (long a, long b) => int.MinValue;
        Identifier Left = Identifier.value;
        Identifier Right = Identifier.old;
        public Operation(string identL, char op, string identR)
        {
            switch (op)
            {
                case '+':
                    Op = (long a, long b) => { checked { return a + b; } };
                    break;
                case '*':
                    Op = (long a, long b) => { checked { return a * b; } };
                    break;
                default: throw new NotImplementedException();
            }
            if (identL == "old")
            {
                Left = Identifier.old;
            }
            else
            {
                Left = Identifier.value;
                value = Convert.ToInt32(identL);
            }

            if (identR == "old")
            {
                Right = Identifier.old;
            }
            else
            {
                Right = Identifier.value;
                if (value != int.MinValue) throw new Exception();
                value = Convert.ToInt32(identR);
            }
        }
        public long Apply(long item)
        {
            long left = Left == Identifier.value ? value : item;
            long right = Right == Identifier.value ? value : item;
            return Op(left, right);
        }
    }
}