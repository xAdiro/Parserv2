using System;
using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableDay
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<TimetableEvent> Events { get; set; } = new List<TimetableEvent>();

        public List<TimetableEvent> this[string EventName]    // Indexer declaration  
        {
            get
            {
                return Events.FindAll(i => i.Name == EventName);
            }
        }
    }
}