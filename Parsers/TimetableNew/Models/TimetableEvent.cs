using System;
using System.Collections.Generic;

namespace Parsers.TimetableNew.Models
{
    public class TimetableEvent
    {
        public string AcademicYear { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public bool IsFaculty { get; set; }
        public List<TimetableGroup> Groups { get; set; } = new List<TimetableGroup>();//CHANGED FROM GROUP
        public List<string> FacultyGroups { get; set; } = new List<string>();//CHANGE FROM GROUP
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Building { get; set; } = "";
        public string Room { get; set; } = "";
        public List<string> Lecturers { get; set; } = new List<string>();
        public string Remarks { get; set; } = "";

        public bool EqualsExceptGroups(TimetableEvent e)
        {
            if (!AcademicYear.Equals(e.AcademicYear)) return false;
            if (!Name.Equals(e.Name)) return false;
            if (!Type.Equals(e.Type)) return false;
            if (!IsFaculty.Equals(e.IsFaculty)) return false;
            if (!StartTime.Equals(e.StartTime)) return false;
            if (!EndTime.Equals(e.EndTime)) return false;
            if (!Building.Equals(e.Building)) return false;
            if (!Room.Equals(e.Room)) return false;
            //if (!Lecturers.Equals(e.Lecturers)) return false; //TODO: this always return false
            if (!Remarks.Equals(e.Remarks)) return false;
            return true;
        }
    }
}