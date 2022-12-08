namespace AOC2022
{
    class Day7
    {
        public static void Start()
        {
            var input = File.ReadLines($"{Global.InputPath}/Day7.txt");
            var Root = GetRoot(input);
            Console.WriteLine(Part1(Root));
            Console.WriteLine(Part2(Root));
        }
        private static _Directory GetRoot(IEnumerable<string> input)
        {
            var Root = new _Directory("/");
            _Directory current = Root;
            bool readingContent = false;
            foreach (var line in input)
            {
                if (line == "") { continue; }
                if (line[0] == '$')
                {
                    readingContent = false;
                    var l = line.Split(' ');
                    if (l[1] == "ls")
                    {
                        readingContent = true;
                        continue;
                    }
                    if (l[2] == "/")
                    {
                        current = Root;
                        continue;
                    }
                    current = l[2] == ".." ? current.GetParent() : current.GoTo(l[2]);
                }
                else if (readingContent)
                {
                    var l = line.Split(' ');
                    if (l[0] == "dir")
                    {
                        current.Add(new _Directory(l[1], current));
                        continue;
                    }
                    current.Add(new _File(l[1], Convert.ToInt32(l[0])));
                }
            }
            return Root;
        }
        private static int Part1(_Directory Root)
        {
            int sum = 0;
            foreach (var dir in Root.AllDirectoriesBottomToTop())
            {
                if (dir.GetSize() <= 100000)
                {
                    sum += dir.GetSize();
                }
            }
            return sum;
        }
        private static int Part2(_Directory Root)
        {
            int toSave = 30000000 - (70000000 - Root.GetSize());
            int min = int.MaxValue;
            foreach (var dir in Root.AllDirectoriesBottomToTop())
            {
                int size = dir.GetSize();
                if (size <= min && size >= toSave) { min = size; }
            }
            return min;
        }
    }
}
public class _Directory
{
    string Name;
    List<_Directory> Directories = new();
    List<_File> Files = new();
    _Directory? Parent = null;
    public _Directory(string name, _Directory? parent = null)
    {
        Name = name;
        Parent = parent;
    }
    public _Directory GetParent()
    {
        if (Parent == null) { throw new Exception(); }
        return Parent;
    }
    public _Directory GoTo(string name)
    {
        var ret = Directories.Find(x => x.Name == name);
        if (ret == null) { throw new Exception(); }
        return ret;
    }
    public void Add(_Directory dir) => Directories.Add(dir);
    public void Add(_File file) => Files.Add(file);
    public int GetSize()
    {
        int sum = 0;
        foreach (var dir in Directories)
        {
            sum += dir.GetSize();
        }
        foreach (var file in Files)
        {
            sum += file.Size;
        }
        return sum;
    }
    public IEnumerable<_Directory> AllDirectoriesBottomToTop()
    {
        foreach (var dir in Directories)
        {
            foreach (var d in dir.AllDirectoriesBottomToTop())
                yield return d;
        }
        yield return this;
    }
}
public class _File
{
    public string Name;
    public int Size;
    public _File(string name, int size)
    {
        Name = name;
        Size = size;
    }
}