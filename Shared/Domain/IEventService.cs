using System;
using System.Collections.Generic;

namespace PPCAndroid.Shared.Domain
{
    public interface IEventService
    {
        void Add(EventBase eventBase);
        IList<EventBase> GetAll();
        IEnumerable<EventBase> GetEventsFromDay(DateTime day);
        TimeSpan CountWorkTime(DateTime day);
        int CountWorkExits(DateTime day);
    }
}