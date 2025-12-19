using AOC2025;
var Days = new List<IDay>()
{
    new Day01(), new Day02(), new Day03(), new Day04(), new Day05(), new Day06(), new Day07(), new Day08(), new Day09(), new Day10(),
    new Day11(), new Day12(),
};
foreach (var day in Days)
{
    Console.WriteLine(day.GetType().Name);
    day.Start();
    Console.WriteLine();
}