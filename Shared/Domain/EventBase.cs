using System;

namespace PPCAndroid.Shared.Domain
{
    public class EventBase
    {
        protected EventBase(DateTime when)
        {
            When = when;
        }

        public DateTime When { get; set; }
        public string EventType;
    }
}