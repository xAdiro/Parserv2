using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsers.TimetableOLD2.Models
{
    public class Timetable
    {
        public DateTime Date { get; set; }
        public IEnumerable<TimetableEvent> Events { get; set; }

        public Timetable()
        {
             Events = new List<TimetableEvent>();
        }


        public static explicit operator Timetable(TimetableOLD.Models.Timetable timetableOld)
        {
            Timetable t = new Timetable() { Date = timetableOld.Date, Events = new List<TimetableEvent>() };
            string[] dni = { "PN", "WT", "ŚR", "CZW", "PT", "SO", "NIE" };

            foreach (TimetableOLD.Models.TimetableEvent e in timetableOld.Events)
            {
                TimetableEvent ev = (TimetableEvent)e;
                t.Events = t.Events.Append(ev);
                Console.WriteLine("hello");
            }
            List<TimetableEvent> finalEvents = t.Events.ToList();
            for (int i = 0; i < finalEvents.Count; i++)
            {
                for (int j = i+1; j < finalEvents.Count;)
                {
                    if (finalEvents[i].Equals(finalEvents[j]))
                    {
                        finalEvents[i].Group.AddRange(finalEvents[j].Group);
                        finalEvents.RemoveAt(j);
                    }
                    else
                    {
                        j++;
                    }
                }
            }
            t.Events = finalEvents;

            t.SortTimetable();
            return t;
        }

        public Timetable MergeTimetables(Timetable t)
        {
            Timetable result = new Timetable
            {
                Date = t.Date > Date ? t.Date : Date,
                Events = Events.Union(t.Events)
            };
            result.SortTimetable();
            return result;
        }

        public void SortTimetable()
        {
            Events = Events.OrderBy(i => i.AcademicYear)
                .ThenBy(i => i.Department)
                .ThenBy(i => i.Mode)
                .ThenBy(i => i.FieldOfStudy)
                .ThenBy(i => i.Degree)
                .ThenBy(i => i.Semester)
                .ThenBy(i => i.DayOfWeek)
                .ThenBy(i => i.StartTime)
                .ThenBy(i => i.EndTime).ToList();
        }
    }
}
