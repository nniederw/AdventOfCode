namespace AOC2022
{
    public static class Ext
    {
        public static T2 MaxValue<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> getvalue) where T2 : IComparable<T2>
        {
            if (list.Any())
            {
                T2 max = getvalue(list.First());
                list.Foreach(i =>
                {
                    if (max.CompareTo(getvalue(i)) < 0) { max = getvalue(i); }
                });
                return max;
            }
            return default;
        }
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }
            return list;
        }
    }
}