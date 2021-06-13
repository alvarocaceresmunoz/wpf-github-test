using System;
using System.Collections.Generic;
using System.Text;
using WeavrGraphLibrary.DataProcessing;

namespace WeavrGraphLibrary.DataStructures
{
    public class TimeSeries<T>
    {
        public List<TimeSeriesEntry<T>> Entries { get; set; } = new List<TimeSeriesEntry<T>>();

        public bool IsAutoTriggerEnabled { get; set; }

        public TimeSeries()
        {
            IsAutoTriggerEnabled = true;
        }

        public void AddEntry(double time, T value)
        {
            Entries.Add(new TimeSeriesEntry<T>(time, value));
            AutoTrigger();
        }

        public void AddEmptyValue(double time)
        {
            Entries.Add(new TimeSeriesEntry<T>() { IsEmpty = true });
            AutoTrigger();
        }

        protected void AutoTrigger()
        {
            if (IsAutoTriggerEnabled)
            {
                DataWatcher.Trigger(this);
            }
        }
    }

    public class TimeSeriesEntry<T>
    {
        public double Time { get; set; }
        public T Value { get; set; }
        public bool IsEmpty { get; set; }

        public TimeSeriesEntry()
        {

        }

        public TimeSeriesEntry(double time)
        {
            Time = time;
            IsEmpty = true;
        }

        public TimeSeriesEntry(double time, T value)
        {
            Time = time;
            Value = value;
            IsEmpty = false;
        }
    }
}
