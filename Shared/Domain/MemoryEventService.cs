using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using PPCAndroid.Domain;

namespace PPCAndroid.Shared.Domain
{
    public class MemoryEventService : IEventService
    {
        private static List<EventBase> _events = new List<EventBase>();

        public void Add(EventBase eventBase)
        {
            if (eventBase.EventType == nameof(StartWorkEvent))
                CreateStartWorkEvent();
            else if (eventBase.EventType == nameof(EndWorkEvent))
                CreateEndWorkEvent();
        }

        public void ClearAllEvents()
        {
            _events.Clear();
        }

        public IList<EventBase> GetAll()
        {
            return _events;
        }
        
        public List<EventTuple> GetTuplesFromDay(DateTime day)
        {
            var dayEvents = _events.Where(n => n.When.Date == day.Date).OrderBy(n => n.When).ToList();
            if (dayEvents.Count % 2 != 0)
                throw new Exception("Number of events must be even");
            var tuples = new List<EventTuple>();
            for (var i = 0; i < _events.Count-1; i+=2)
            {
                tuples.Add(new EventTuple((StartWorkEvent)_events[i],(EndWorkEvent)_events[i+1]));
            }

            return tuples;
        }

        public IEnumerable<EventBase> GetEventsFromDay(DateTime day)
        {
            return !_events.Any() ? null : _events.Where(n => n.When.Date == day.Date).OrderBy(n => n.When).ToArray();
        }

        private void CreateStartWorkEvent()
        {
            if(_events.OrderByDescending(n => n.When).FirstOrDefault()?.EventType != nameof(StartWorkEvent))
                _events.Add(new StartWorkEvent(DateTime.Now));
        }

        private void CreateEndWorkEvent()
        {
            if (_events.OrderByDescending(n => n.When).First().EventType != nameof(EndWorkEvent))
                _events.Add(new EndWorkEvent(DateTime.Now));
        } 
    }
}