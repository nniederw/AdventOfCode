namespace AOC2022
{
    class Day5
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day5.txt");
            Stack<char>[] stacks = new Stack<char>[9];
            for (int i = 0; i < 9; i++)
            {
                stacks[i] = new Stack<char>();
            }
            List<(int, int, int)> instruc = new();
            bool instructions = false;
            foreach (var line in lines)
            {
                if (line == "" || line == " 1   2   3   4   5   6   7   8   9 ")
                {
                    instructions = true;
                    continue;
                }
                if (!instructions)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        char c = line[i * 4 + 1];
                        if (c != ' ')
                        {
                            stacks[i].Push(c);
                        }
                    }
                }
                else
                {   //move 1 from 5 to 6
                    var l = line.Split(' ');
                    instruc.Add((Convert.ToInt32(l[1]), Convert.ToInt32(l[3]), Convert.ToInt32(l[5])));
                }
            }
            for (int i = 0; i < 9; i++)
            {
                var s = new Stack<char>();
                foreach (var c in stacks[i])
                {
                    s.Push(c);
                }
                stacks[i] = s;
            }
            var stackcopy = new Stack<char>[9];
            for (int i = 0; i < 9; i++)
            {
                stackcopy[i] = new Stack<char>(new Stack<char>(stacks[i]));
            }
            Console.WriteLine(Part1(stackcopy, instruc));
            Console.WriteLine(Part2(stacks, instruc));
        }
        private static string Part1(Stack<char>[] stacks, List<(int, int, int)> instructions)
        {
            foreach (var inst in instructions)
            {
                for (int i = 0; i < inst.Item1; i++)
                {
                    stacks[inst.Item3 - 1].Push(stacks[inst.Item2 - 1].Pop());
                }
            }
            string res = "";
            foreach (var stack in stacks)
            {
                res += stack.Peek();
            }
            return res;
        }
        private static string Part2(Stack<char>[] stacks, List<(int, int, int)> instructions)
        {
            foreach (var inst in instructions)
            {
                var s = new Stack<char>();
                for (int i = 0; i < inst.Item1; i++)
                {
                    s.Push(stacks[inst.Item2 - 1].Pop());
                }
                for (int i = 0; i < inst.Item1; i++)
                {
                    stacks[inst.Item3 - 1].Push(s.Pop());
                }
            }
            string res = "";
            foreach (var stack in stacks)
            {
                res += stack.Peek();
            }
            return res;
        }
    }
}