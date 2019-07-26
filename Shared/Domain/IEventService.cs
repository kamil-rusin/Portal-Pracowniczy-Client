using System;
using System.Collections.Generic;

namespace PPCAndroid.Shared.Domain
{
    public interface IEventService
    {
        void Add(EventBase eventBase);
        void ClearAllEvents();
        IList<EventBase> GetAll();
        IEnumerable<EventBase> GetEventsFromDay(DateTime day);
    }
}