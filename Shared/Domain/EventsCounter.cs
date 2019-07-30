using System;
using System.Collections.Generic;
using System.Linq;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid.Domain
{
    public static class EventsCounter
    {
        public static TimeSpan CountWorkTime(IEnumerable<EventBase> events,DateTime day)
        {
            var dayEvents = events.Where(n => n.When.Date == day.Date).OrderBy(n => n.When).ToArray();
            var workTime = TimeSpan.Zero;
            for (var i = 0; i < dayEvents.Length - 1; i+=2)
            {
                workTime += dayEvents[i + 1].When - dayEvents[i].When;
            }

            return workTime;
        }
        
        public static TimeSpan CountWorkTime(IEnumerable<EventTuple> eventTuples)
        {
            var tuples = eventTuples.OrderBy(n => n.EndWorkEvent.When);
            return tuples.Aggregate(TimeSpan.Zero, (current, tuple) => current + tuple.CountDifference());
        }

        public static int CountWorkExits(IEventService eventService, DateTime day)
        {
            var events = eventService.GetEventsFromDay(day);
            //if only 2 kind of events Start and End
            return (events.Count() / 2) - 1;
        }
    }
}