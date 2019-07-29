using System;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.Domain
{
    public class EventTuple
    {
        public StartWorkEvent StartWorkEvent { get; }
        public EndWorkEvent EndWorkEvent { get; }

        public EventTuple(StartWorkEvent startWorkEvent, EndWorkEvent endWorkEvent)
        {
            StartWorkEvent = startWorkEvent;
            EndWorkEvent = endWorkEvent;
        }

        public TimeSpan CountDifference()
        {
            return EndWorkEvent.When - StartWorkEvent.When;
        }
    }
}