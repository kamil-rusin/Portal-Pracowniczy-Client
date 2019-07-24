using System;

namespace PPCAndroid.Shared.Domain
{
    public class StartWorkEvent : EventBase
    {
        public StartWorkEvent(DateTime when) : base(when)
        {
            EventType = nameof(StartWorkEvent);
        }
    }
}