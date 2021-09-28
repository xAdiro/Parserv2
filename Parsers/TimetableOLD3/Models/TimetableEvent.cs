using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.TimetableOLD3.Models
{
    public class TimetableGroup
    {
        public string group_id;
        public string specialization;

        public TimetableGroup(string group_id, string specialization = null)
        {
            this.group_id = group_id;
            this.specialization = specialization;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", specialization, group_id);
        }

        public override bool Equals(object obj)
        {
            if (obj is TimetableGroup group)
            {
                if (group.group_id == group_id && group.specialization == specialization)
                    return true;
            }
            return false;
        }

        public override int GetHashCode() => (
            group_id,
            specialization
            ).GetHashCode();
    }
    public class TimetableEvent
    {
        public static Dictionary<string, int> eventTypes = new Dictionary<string, int>()
        {
            { "w", 0 },
            { "wykład", 0 },
            { "wyklad", 0 },
            { "ćw", 1 },
            { "cw", 1 },
            { "ćwiczenia", 1 },
            { "cwiczenia", 1 },
            { "lab", 2 },
            { "laboratorium", 2 },
            { "f", 3 },
            { "faq", 3 },
            { "fak", 3 },
            { "fakultet", 3 },
        };

        public string name;
        public string department;
        public string field_of_study;
        public string degree;
        public string semester;
        public string mode;
        public string start_time;
        public string end_time;
        public bool is_online;
        public string building;
        public string room;
        public List<string> lecturers;
        public string remarks;
        public int day_of_week;
        public int type;
        public string custom_type;
        public List<TimetableGroup> groups;

        public static explicit operator TimetableEvent(TimetableOLD2.Models.TimetableEvent timetableOldEvent)
        {
            TimetableEvent e = new TimetableEvent();
            e.name = timetableOldEvent.Name;
            e.department = timetableOldEvent.Department;
            e.field_of_study = timetableOldEvent.FieldOfStudy;
            e.degree = timetableOldEvent.Degree;
            e.semester = timetableOldEvent.Semester;
            e.mode = timetableOldEvent.Mode;
            e.start_time = timetableOldEvent.StartTime;
            e.end_time = timetableOldEvent.EndTime;
            if(timetableOldEvent.Building.Trim() == "" && timetableOldEvent.Room.Trim().ToLower() == "zdalne")
            {
                e.is_online = true;
                e.building = null;
                e.room = null;
            }
            else
            {
                e.building = timetableOldEvent.Building.Trim();
                e.room = timetableOldEvent.Room.Trim();
                if (e.room != "" && e.building == "")
                    e.building = "34";
            }
            e.lecturers = timetableOldEvent.Lecturers.ToList();
            e.remarks = timetableOldEvent.Remarks;
            e.day_of_week = timetableOldEvent.DayOfWeek;

            if (eventTypes.ContainsKey(timetableOldEvent.Type.ToLower()))
            {
                e.type = eventTypes[timetableOldEvent.Type.ToLower()];
                e.custom_type = null;
            }
            else
            {
                e.type = -1;
                e.custom_type = timetableOldEvent.Type;
            }
            e.groups = new List<TimetableGroup>();
            foreach (var g in timetableOldEvent.Group)
            {
                if(g.Specialization != "")
                    e.groups.Add(new TimetableGroup(g.Group, g.Specialization));
                else
                    e.groups.Add(new TimetableGroup(g.Group));
            }
            return e;
        }                   
    }                       
}                           
                            
                            