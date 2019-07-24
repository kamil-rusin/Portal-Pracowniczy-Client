using System;

namespace PPCAndroid.Shared.Domain
{
    public class EndWorkEvent : EventBase
    {
        public EndWorkEvent(DateTime when) : base(when)
        {
            EventType = nameof(EndWorkEvent);
        }
    }
}