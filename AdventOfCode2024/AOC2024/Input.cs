using System.Linq;

namespace AOC2024
{
    public static class Input
    {
        public static List<TResult> InterpretAs1DList<TSource, TResult>(IEnumerable<TSource> input, Func<TSource, TResult> Interpret)
            => input.Select(Interpret).ToList();
        public static List<T> InterpretStringAs1DList<T>(string input, Func<char, T> Interpret)
            => InterpretAs1DList(input.AsEnumerable(), Interpret);
        public static List<int> InterpretStringAs1DIntList(string input)
            => InterpretStringAs1DList(input, i => Convert.ToInt32(i.ToString()));
        public static TResult[,] InterpretAs2DArray<TSource, TResult>(IEnumerable<IEnumerable<TSource>> input, Func<TSource, TResult> Interpret)
        {
            var builtInput = input.Select(i => i.ToList()).ToList();
            int height = builtInput.Count;
            int width = builtInput.First().Count;
            if (!builtInput.TrueForAll(i => i.Count == width))
            {
                throw new Exception("Input isn't in a rectanguar shape, can't convert it to a 2D array");
            }
            TResult[,] result = new TResult[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result[y, x] = Interpret(builtInput[x][y]);
                }
            }
            return result;
        }
        public static T[,] InterpretStringsAs2DArray<T>(IEnumerable<string> input, Func<char, T> Interpret)
            => InterpretAs2DArray(input.Select(i => i.AsEnumerable()), Interpret);
        public static int[,] InterpretStringsAs2DIntArray(IEnumerable<string> input)
            => InterpretStringsAs2DArray(input, i => Convert.ToInt32(i.ToString()));
        public static char[,] InterpretStringsAs2DCharArray(IEnumerable<string> input)
            => InterpretStringsAs2DArray(input, i => i);
    }
}