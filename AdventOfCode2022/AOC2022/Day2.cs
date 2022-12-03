namespace AOC2022
{
    class Day2
    {
        private enum Shape { Rock, Paper, Scissors }
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day2.txt");
            List<(Shape, Shape)> input = new();
            foreach (var line in lines)
            {
                var l0 = line[0];
                var l2 = line[2];
                (Shape s1, Shape s2) s;
                s.s1 = l0 == 'A' ? Shape.Rock : l0 == 'B' ? Shape.Paper : Shape.Scissors;
                s.s2 = l2 == 'X' ? Shape.Rock : l2 == 'Y' ? Shape.Paper : Shape.Scissors;
                input.Add(s);
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private static int Score(Shape opponent, Shape choice)
        {
            int score = choice == Shape.Rock ? 1 : choice == Shape.Paper ? 2 : 3;
            if (opponent == choice) { score += 3; }
            else if (((int)opponent + 1) % 3 == (int)choice % 3)
            { score += 6; }
            else
            { score += 0; }
            return score;
        }
        private static int Part1(List<(Shape, Shape)> input)
        {
            int score = 0;
            foreach (var ss in input)
            {
                score += Score(ss.Item1, ss.Item2);
            }
            return score;
        }
        private static int Part2(List<(Shape, Shape)> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].Item2 == Shape.Rock)
                {
                    input[i] = new(input[i].Item1, Loose(input[i].Item1));
                }
                else if (input[i].Item2 == Shape.Paper)
                {
                    input[i] = new(input[i].Item1, input[i].Item1);
                }
                else
                {
                    input[i] = new(input[i].Item1, Win(input[i].Item1));
                }
            }
            return Part1(input);
        }
        private static Shape Win(Shape opp)
        {
            return (Shape)((((int)opp) + 1) % 3);
        }
        private static Shape Loose(Shape opp)
        {
            return (Shape)((((int)opp) + 2) % 3);
        }
    }
}