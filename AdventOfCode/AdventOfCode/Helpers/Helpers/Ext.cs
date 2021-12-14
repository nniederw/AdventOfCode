using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Helpers
{
    public static class Ext
    {
        public static float Positive(float value) => value < 0 ? value * -1 : value;
        public static int Abs(this int i) => i < 0 ? -i : i;
        public static int Sqr(int a) => a * a;
        public static Int64 Sqr(Int64 a) => a * a;
        public static float Sqr(float a) => a * a;
        public static double Sqr(double a) => a * a;
        // public static Temperature Sqr(this Temperature temperature) => new Temperature(temperature.ValueInKelvin * temperature.ValueInKelvin, Temperature.Unit.Kelvin);
        public static List<int> FindAllIndexes<T>(this IList<T> list, Predicate<T> match)
        {
            var ret = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i])) { ret.Add(i); }
            }
            return ret;
        }
        public static List<KeyValuePair<int, T>> FindAllIndexesAndValues<T>(this IList<T> list, Predicate<T> match)
        {
            var ret = new List<KeyValuePair<int, T>>();
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i])) { ret.Add(new KeyValuePair<int, T>(i, list[i])); }
            }
            return ret;
        }
        [DebuggerHidden]
        public static void Assert(bool condition, string errorMessage = "")
        {
            if (!condition) { throw new Exception(errorMessage); }
        }
        public static TimeSpan AddDays(this TimeSpan timeSpan, int days) => timeSpan.AddTicks(days * TimeSpan.TicksPerDay);
        public static TimeSpan AddHours(this TimeSpan timeSpan, int hours) => timeSpan.AddTicks(hours * TimeSpan.TicksPerHour);
        public static TimeSpan AddMinutes(this TimeSpan timeSpan, int minutes) => timeSpan.AddTicks(minutes * TimeSpan.TicksPerMinute);
        public static TimeSpan AddSeconds(this TimeSpan timeSpan, int seconds) => timeSpan.AddTicks(seconds * TimeSpan.TicksPerSecond);
        public static TimeSpan AddMilliseconds(this TimeSpan timeSpan, int millisecond) => timeSpan.AddTicks(millisecond * TimeSpan.TicksPerMillisecond);
        public static TimeSpan AddTicks(this TimeSpan timeSpan, long ticks) => timeSpan.Add(new TimeSpan(ticks));
        public static List<T> SortList<T>(this List<T> list)
        {
            list.Sort();
            return list;
        }
        public static List<T> SortList<T>(this List<T> list, Comparison<T> comparison)
        {
            list.Sort(comparison);
            return list;
        }
        public static void DoThrow(string message = "") => throw new Exception(message);
        public static T DoThrow<T>(string message = "") => throw new Exception(message);
        public static bool IsValid(this float f) => (!float.IsNaN(f)) && (!float.IsInfinity(f));
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }
            return list;
        }
        public static List<T> Foreach<T>(this List<T> list, Action<T> action) => (List<T>)list.AsEnumerable().Foreach(action);
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
        public static T2 MinValue<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> getvalue) where T2 : IComparable<T2>
        {
            if (list.Any())
            {
                T2 max = getvalue(list.First());
                list.Foreach(i =>
                {
                    if (max.CompareTo(getvalue(i)) > 0) { max = getvalue(i); }
                });
                return max;
            }
            return default;
        }
        public static T1 MaxOfValue<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> getvalue) where T2 : IComparable<T2>
        {
            if (list.Any())
            {
                T1 max = list.First();
                list.Foreach(i =>
                {
                    if (getvalue(max).CompareTo(getvalue(i)) < 0) { max = i; }
                });
                return max;
            }
            return default;
        }
        public static T1 MinOfValue<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> getvalue) where T2 : IComparable<T2>
        {
            if (list.Any())
            {
                T1 max = list.First();
                list.Foreach(i =>
                {
                    if (getvalue(max).CompareTo(getvalue(i)) > 0) { max = i; }
                });
                return max;
            }
            return default;
        }
    }
}