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
                input.Add(line);
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private int Part1(List<string> input)
        {
            return -1;
        }
        private int Part2(List<string> calo)
        {
            return -1;
        }
    }
}