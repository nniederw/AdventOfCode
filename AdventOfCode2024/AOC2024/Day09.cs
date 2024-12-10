using System.Runtime;

namespace AOC2024
{
    class Day09 : IDay
    {
        private string TextFileName => $"{GetType().Name}.txt";
        public void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/{TextFileName}");
            var input = Input.InterpretStringAs1DIntList(lines.First());
            //Console.WriteLine(Part1(input));
            Console.WriteLine($"{6448989155953} (Precomputed)");
            //Console.WriteLine(Part2(input));
            Console.WriteLine($"{6476642796832} (Precomputed)");
        }
        private List<int> Expand(List<int> compressedFiles)
        {
            List<int> result = new();
            bool file = true;
            int id = 0;
            for (int i = 0; i < compressedFiles.Count; i++)
            {
                int compNumber = compressedFiles[i];
                if (file)
                {
                    Enumerable.Range(0, compNumber).ToList().ForEach(j => result.Add(id));
                    id++;
                }
                else
                {
                    Enumerable.Range(0, compNumber).ToList().ForEach(j => result.Add(-1));
                }
                file = !file;
            }
            return result;
        }
        private long Checksum(List<int> files)
        {
            long sum = 0;
            for (int i = 0; i < files.Count; i++)
            {
                long file = files[i];
                if (file == -1) { continue; }
                sum += file * i;
            }
            return sum;
        }
        private bool RearangeOnce(List<int> files)
        {
            int indexLastUsedBlock = -1;
            for (int i = files.Count - 1; i > 0; i--)
            {
                if (files[i] != -1)
                {
                    indexLastUsedBlock = i;
                    break;
                }
            }
            if (indexLastUsedBlock == -1) { return false; }
            int indexFirstFreeBlock = -1;
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] == -1)
                {
                    indexFirstFreeBlock = i;
                    break;
                }
            }
            if (indexFirstFreeBlock == -1) { return false; }
            if (indexFirstFreeBlock > indexLastUsedBlock) { return false; }
            files[indexFirstFreeBlock] = files[indexLastUsedBlock];
            files[indexLastUsedBlock] = -1;
            return true;
        }
        private long Part1(List<int> files)
        {
            files = Expand(files);
            while (RearangeOnce(files)) { }
            return Checksum(files);
        }
        private void PrintFiles(List<int> files)
        {
            string list = "";
            files.ForEach(f => { if (f != -1) { list += $"{f}|"; } else { list += ".|"; } });
            Console.WriteLine(list);
        }
        private List<(int Id, int StartIndex, int Length)> GetAllBlocks(List<int> files)
        {
            List<(int Id, int StartIndex, int Length)> result = new();
            int currentBlock = files.First();
            int currentLength = -1;
            int startIndex = 0;
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] == currentBlock)
                {
                    currentLength++;
                }
                else
                {
                    result.Add((currentBlock, startIndex, currentLength + 1));
                    currentBlock = files[i];
                    currentLength = 0;
                    startIndex = i;
                }
            }
            if (currentLength > 0)
            {
                result.Add((currentBlock, startIndex, currentLength + 1));
            }
            return result;
        }
        private List<(int StartIndex, int Length)> GetUsedBlocks(List<int> files)
            => GetAllBlocks(files).Where(i => i.Id != -1).Select(i => (i.StartIndex, i.Length)).ToList();
        private List<(int StartIndex, int Length)> GetFreeBlocks(List<int> files)
            => GetAllBlocks(files).Where(i => i.Id == -1).Select(i => (i.StartIndex, i.Length)).ToList();
        private bool TryRearangeBlockOnce(List<int> files, int BlockStartIndex, int BlockLength)
        {
            var freeBlocks = GetFreeBlocks(files);
            foreach (var i in freeBlocks)
            {
                if (i.Length >= BlockLength && i.StartIndex < BlockStartIndex)
                {
                    RearangeBlock(files, BlockStartIndex, BlockLength, i.StartIndex);
                    return true;
                }
            }
            return false;
        }
        private void RearangeBlock(List<int> files, int BlockStartIndex, int BlockLength, int newIndex)
        {
            for (int i = 0; i < BlockLength; i++)
            {
                files[newIndex + i] = files[BlockStartIndex + i];
                files[BlockStartIndex + i] = -1;
            }
        }
        private long Part2(List<int> files)
        {
            files = Expand(files);
            var blocks = GetUsedBlocks(files);
            blocks.Reverse();
            foreach (var block in blocks)
            {
                TryRearangeBlockOnce(files, block.StartIndex, block.Length);
            }
            //PrintFiles(files);
            return Checksum(files);
        }
    }
}