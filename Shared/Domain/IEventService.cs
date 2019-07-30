using System;
using System.Collections.Generic;
using PPCAndroid.Domain;

namespace PPCAndroid.Shared.Domain
{
    public interface IEventService
    {
        void Add(EventBase eventBase);
        void ClearAllEvents();
        IList<EventBase> GetAll();
        List<EventTuple> GetTuplesFromDay(DateTime day);
        IEnumerable<EventBase> GetEventsFromDay(DateTime day);
    }
}