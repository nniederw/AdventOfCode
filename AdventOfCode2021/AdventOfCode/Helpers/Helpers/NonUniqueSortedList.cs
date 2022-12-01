using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Helpers
{
    [Serializable]
    public class NonUniqueSortedList<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IList<KeyValuePair<TKey, TValue>>
        where TKey : IComparable<TKey>//, , IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary
    {
        private List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
        public int Count => list.Count;
        public bool IsReadOnly => false;
        public KeyValuePair<TKey, TValue> this[int index] { get => list[index]; set => throw new Exception("dont use an index to set in " + nameof(NonUniqueSortedList<TKey, TValue>)); }
        public bool Any() => list.Any();
        public void Add(KeyValuePair<TKey, TValue> item) => InsertIntoSorted(list, item);
        public void Add(TKey key, TValue value) => InsertIntoSorted(list, new KeyValuePair<TKey, TValue>(key, value));
        public void RemoveAt(int index) => list.RemoveAt(index);
        public void RemoveLast() => list.RemoveAt(list.Count - 1);
        public KeyValuePair<TKey, TValue> Last() => list.Last();
        public KeyValuePair<TKey, TValue> First() => list.First();
        public KeyValuePair<TKey, TValue> LastOrDefault() => list.LastOrDefault();
        public KeyValuePair<TKey, TValue> FirstOrDefault() => list.FirstOrDefault();
        private static void InsertIntoSorted(List<KeyValuePair<TKey, TValue>> list, KeyValuePair<TKey, TValue> item)
        {
            var comparer = Comparer<KeyValuePair<TKey, TValue>>.Create((x, y) => x.Key.CompareTo(y.Key));
            var pos = list.BinarySearch(item, comparer);
            list.Insert(pos >= 0 ? pos + 1 : ~pos, item);
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => list.GetEnumerator();
        [Obsolete("use typesafe method")] IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
        public int IndexOf(KeyValuePair<TKey, TValue> item) => list.IndexOf(item);
        /// <summary>
        ///  ignores the index and adds the item
        /// </summary>
        public void Insert(int index, KeyValuePair<TKey, TValue> item) => Add(item);
        public void Clear() => list.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => list.Contains(item);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<TKey, TValue> item) => list.Remove(item);
        public List<KeyValuePair<TKey, TValue>> GetDCopyList()
        {
            var result = new List<KeyValuePair<TKey, TValue>>();
            list.ForEach(result.Add);
            return result;
        }
    }
}
