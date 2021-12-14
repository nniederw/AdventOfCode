using System;
using System.Collections.Generic;
using System.Linq;
namespace Helpers
{
    public class SortedMultiArray<TKey, TValue>
    {
        private SortedList<TKey, List<TValue>> slist = new SortedList<TKey, List<TValue>>();
        public void Add(TKey key, TValue value)
        {
            if (slist.ContainsKey(key))
            {
                slist[key].Add(value);
            }
            else
            {
                slist.Add(key, new List<TValue> { value });
            }
        }
        public bool Any() => slist.Any();
        public KeyValuePair<TKey, List<TValue>> First() => slist.First();
        public KeyValuePair<TKey, List<TValue>> Last() => slist.Last();
        public int NrOfKeys { get => slist.Keys.Count; }
        public int NrOfValues { get => slist.Sum(i => i.Value.Count); }
        public List<TValue> GetValues(int index) => slist[slist.Keys[index]];
        public void RemoveKey(TKey key) => slist.Remove(key);
        public void RemoveKeyAt(int i) => slist.RemoveAt(i);
    }
}