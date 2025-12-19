namespace AOC2025
{
    class Day04 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        private const char RollOfPaper = '@';
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            char[,] input = Input.InterpretStringsAs2DCharArray(lines);
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private long Part1(char[,] input)
        {
            long result = 0;
            int sizeX = input.GetLength(0);
            int sizeY = input.GetLength(1);
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (input[x, y] == RollOfPaper)
                    {
                        int nei = input.Get8Neighbors(x, y).Count(i => i == RollOfPaper);
                        if (nei < 4)
                        {
                            result++;
                        }
                    }
                }
            }
            return result;
        }
        private long Part2(char[,] input)
        {
            long result = 0;
            int sizeX = input.GetLength(0);
            int sizeY = input.GetLength(1);
            bool removedSomething = true;
            while (removedSomething)
            {
                removedSomething = false;
                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        if (input[x, y] == RollOfPaper)
                        {
                            int nei = input.Get8Neighbors(x, y).Count(i => i == RollOfPaper);
                            if (nei < 4)
                            {
                                result++;
                                removedSomething = true;
                                input[x, y] = '.';
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}