namespace AOC2022
{
    class Day13
    {
        public static void Start()
        {
            var lines = File.ReadLines($"{Global.InputPath}/Day13.txt");
            List<(Element left, Element right)> input = new();
            input.Add((null, null));
            foreach (var line in lines)
            {
                if (line == "")
                {
                    input.Add((null, null));
                    continue;
                }
                if (input.Last().left == null)
                {
                    input[input.Count - 1] = (StringToElement(line), null);
                }
                else
                {
                    input[input.Count - 1] = (input[input.Count - 1].left, StringToElement(line));
                }
            }
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }
        private static Element StringToElement(string s)
        {
            if (s == "") throw new Exception();
            if (s[0] == '[')
            {
                if (s.Last() != ']') throw new Exception();
                var list = s.Substring(1, s.Length - 2);
                if (list.Length == 0) return new _List();
                var _list = new _List();
                var element = "";
                int openBrackets = 0;
                foreach (var c in list)
                {
                    switch (c)
                    {
                        case '[':
                            openBrackets++;
                            element += c;
                            break;
                        case ']':
                            openBrackets--;
                            element += c;
                            break;
                        case ',':
                            if (openBrackets == 0)
                            {
                                _list.Elements.Add(StringToElement(element));
                                element = "";
                            }
                            else { element += c; }
                            break;
                        default:
                            element += c;
                            break;
                    }
                }
                _list.Elements.Add(StringToElement(element));
                return _list;
            }
            else
            {
                return new _Int(Convert.ToInt32(s));
            }
        }
        private static int Part1(List<(Element left, Element right)> input)
        {
            int sum = 0;
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].left.CompareTo(input[i].right) >= 0)
                {
                    sum += i + 1;
                }
            }
            return sum;
        }
        private static int Part2(List<(Element left, Element right)> input)
        {
            return -1;
        }
    }
    class _List : Element
    {
        public List<Element> Elements = new();
        public _List() { }
        public _List(_Int _int)
        {
            Elements.Add(_int);
        }
        public int CompareTo(_List other)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (other.Elements.Count == i) return -1;
                int comp = Elements[i].CompareTo(other.Elements[i]);
                if (comp != 0) return comp;
            }
            return 0;
        }

        public int CompareTo(Element other)
        {
            if (other.GetType() == typeof(_List))
            {
                return CompareTo((_List)other);
            }
            return CompareTo(new _List((_Int)other));
        }
    }
    class _Int : Element
    {
        int Value = -1;
        public _Int(int value) => Value = value;
        public int CompareTo(_Int other) => other.Value.CompareTo(Value);
        public int CompareTo(Element other)
        {
            if (other.GetType() == typeof(_Int))
            {
                return CompareTo((_Int)other);
            }
            return new _List(this).CompareTo((_List)other);
        }
    }
    interface Element
    {
        public int CompareTo(Element other);
    }
}