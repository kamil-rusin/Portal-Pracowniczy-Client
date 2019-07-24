using System;
using System.Collections.Generic;
using System.Linq;
using Java.Util;

namespace PPCAndroid.Shared.Domain
{
    public class MemoryEventStore : IEventStore
    {
        private IList<EventBase> _events;

        public void Add(EventBase eventBase)
        {
            if (eventBase.EventType == nameof(StartWorkEvent))
                CreateStartWorkEvent();
            else if (eventBase.EventType == nameof(EndWorkEvent)) 
                CreateEndWorkEvent();
        }

        public IList<EventBase> GetAll()
        {
            return  _events;
        }

        public EventBase[] GetEventsFromDay(DateTime day)
        {
            return _events.Where(n => n.When.Date == day.Date).OrderBy(n => n.When).ToArray();
        }

        private void CreateStartWorkEvent()
        {
            if(_events.OrderByDescending(n => n.When).First().EventType != nameof(StartWorkEvent))
                _events.Add(new StartWorkEvent(DateTime.Now));
        }

        private void CreateEndWorkEvent()
        { 
            if(_events.OrderByDescending(n => n.When).First().EventType != nameof(EndWorkEvent))
                _events.Add(new EndWorkEvent(DateTime.Now));
        }
        
        private TimeSpan CountWorkTime(DateTime day)
        {
            var dayEvents = _events.Where(n => n.When.Date == day.Date).OrderBy(n=>n.When).ToArray();
            var workTime = TimeSpan.Zero;
            for (var i = 0; i < dayEvents.Length - 1; i++)
            {
                workTime += dayEvents[i + 1].When - dayEvents[i].When;
            }
            return workTime;
        }
    }
}