using System;
using System.Collections.Generic;

namespace PPCAndroid.Shared.Domain
{
    public interface IEventStore
    {        
        void Add(EventBase eventBase);
        IList<EventBase> GetAll();
        IEnumerable<EventBase> GetEventsFromDay(DateTime day);
    }
}