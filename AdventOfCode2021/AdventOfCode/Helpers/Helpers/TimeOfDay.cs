using System;
namespace Helpers
{
    /// <summary>
    /// Valid range from 00:00:00 - 24:00:00 (= 00:00:00 next Day)
    /// </summary>
    [Serializable]
    public struct TimeOfDay : IComparable<TimeOfDay>, IEquatable<TimeOfDay>
    {
        public long Ticks { get; private set; }
        public int Milliseconds => (int)(Ticks / TimeSpan.TicksPerMillisecond) % 1000;
        public int Seconds => (int)(Ticks / TimeSpan.TicksPerSecond) % 60;
        public int Minutes => (int)(Ticks / TimeSpan.TicksPerMinute) % 60;
        public int Hours => (int)(Ticks / TimeSpan.TicksPerHour);
        public static readonly int MinTickValue = 0;
        public static readonly long MaxTickValue = TimeSpan.TicksPerDay;
        public static readonly TimeOfDay MinValue = new TimeOfDay(MinTickValue);
        public static readonly TimeOfDay MaxValue = new TimeOfDay(MaxTickValue);
        public TimeOfDay(long ticks = 0) { Ticks = ticks; AssertValid(); }
        public TimeOfDay(int hours, int minutes, int seconds = 0, int milliseconds = 0)
            : this(hours * TimeSpan.TicksPerHour
            + minutes * TimeSpan.TicksPerMinute
            + seconds * TimeSpan.TicksPerSecond
            + milliseconds * TimeSpan.TicksPerMillisecond)
        { }
        private void AssertValid()
        {
            if (Ticks > MaxTickValue || Ticks < MinTickValue)
            {
                throw new Exception($"{nameof(TimeOfDay)} (ticks = {Ticks}) is not between {MinTickValue} and {MaxTickValue}");
            }
        }
        public TimeOfDay AddTicks(long ticks) => new TimeOfDay(Ticks + ticks);
        public TimeOfDay AddMilliseconds(int milliseconds) => new TimeOfDay(Ticks + milliseconds * TimeSpan.TicksPerMillisecond);
        public TimeOfDay AddSeconds(int seconds) => new TimeOfDay(Ticks + seconds * TimeSpan.TicksPerSecond);
        public TimeOfDay AddMinutes(int minutes) => new TimeOfDay(Ticks + minutes * TimeSpan.TicksPerMinute);
        public TimeOfDay AddHours(int hours) => new TimeOfDay(Ticks + hours * TimeSpan.TicksPerHour);
        public int CompareTo(TimeOfDay other) => Ticks.CompareTo(other.Ticks);
        public bool Equals(TimeOfDay other) => Ticks == other.Ticks;
        public override bool Equals(object obj) => Ticks == ((TimeOfDay)obj).Ticks;
        public override int GetHashCode() => (int)(Ticks / (TimeSpan.TicksPerDay / Int32.MaxValue + 1));
        public override string ToString() => $"{Hours.ToString("D2")}:{Minutes.ToString("D2")}:{Seconds.ToString("D2")}.{Milliseconds.ToString("D3")}";
        public string ToString(string s) => ToTimeSpan(this).ToString(s);
        public static bool operator <(TimeOfDay t1, TimeOfDay t2) => t1.Ticks < t2.Ticks;
        public static bool operator >(TimeOfDay t1, TimeOfDay t2) => t1.Ticks > t2.Ticks;
        public static bool operator ==(TimeOfDay t1, TimeOfDay t2) => t1.Ticks == t2.Ticks;
        public static bool operator !=(TimeOfDay t1, TimeOfDay t2) => t1.Ticks != t2.Ticks;
        public static TimeOfDay GetTimeOfDay(TimeSpan timeSpan) => new TimeOfDay(timeSpan.Ticks);
        public static TimeOfDay GetTimeOfDay(DateTime dateTime) => GetTimeOfDay(dateTime.TimeOfDay);
        public static DateTime AddTimeOfDay(DateTime dateTime, TimeOfDay timeOfDay) => dateTime.AddTicks(timeOfDay.Ticks);
        public static TimeSpan ToTimeSpan(TimeOfDay timeOfDay) => new TimeSpan(timeOfDay.Ticks);
    }
}