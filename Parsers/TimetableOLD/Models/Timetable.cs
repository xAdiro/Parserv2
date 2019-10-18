using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsers.TimetableOLD.Models
{
    public class Timetable
    {
        public DateTime Date { get; set; }
        public IEnumerable<TimetableEvent> Events { get; set; }

        public Timetable()
        {
             Events = new List<TimetableEvent>();
        }


        public static explicit operator Timetable(TimetableNew.Models.Timetable timetableNew)
        {
            Timetable t = new Timetable() { Date = timetableNew.Date, Events = new List<TimetableEvent>() };
            string[] dni = { "PN", "WT", "ŚR", "CZW", "PT", "SO", "NIE" };
            foreach (var department in timetableNew.Departments)
            {
                foreach (var mode in department.Modes)
                    foreach (var fieldOfStudy in mode.Fields)
                        foreach (var semester in fieldOfStudy.Semesters)
                        {
                            List<TimetableNew.Models.TimetableGroup> groups = new List<TimetableNew.Models.TimetableGroup>();
                            foreach (var day in semester.Days)
                            {
                                foreach (var e in day.Events)
                                {
                                    groups = groups.Union(e.Groups).ToList();
                                }
                            }
                            groups = groups.OrderBy(item => item.Group).ToList();
                            List<string> specs = new List<string>();
                            foreach (var group in groups)
                            {
                                specs.Add(group.Specialization);
                            }
                            List<bool> duplicates = new List<bool>();
                            int i = 0;
                            foreach (var spec in specs)
                            {
                                duplicates.Add(false);
                                int count = 0;
                                foreach (var item in specs)
                                {
                                    if (item == spec && item != "") count++;
                                }
                                duplicates[i] = count > 1;
                                i++;
                            }
                            for (int j = 0; j < duplicates.Count; j++)
                            {
                                if (duplicates[j])
                                {
                                    string duplicatedSpec = specs[j];
                                    int k = 1;
                                    for (int l = 0; l < specs.Count; l++)
                                    {
                                        if (duplicatedSpec == specs[l])
                                        {
                                            duplicates[l] = false;
                                            specs[l] += "-" + k;
                                            k++;
                                        }
                                    }
                                }
                            }


                            foreach (var day in semester.Days)

                                foreach (var e in day.Events)
                                    foreach (var group in e.Groups)
                                    {
                                        //dictionaries
                                        string field = fieldOfStudy.FieldOfStudy.ToUpper();
                                        string mode2 = mode.Mode.ToUpper();
                                        string type = e.Type.ToUpper();
                                        if (Dictionaries.FieldsDictionary.ContainsKey(field)) field = Dictionaries.FieldsDictionary[field];
                                        if (Dictionaries.ModesDictionary.ContainsKey(mode2)) mode2 = Dictionaries.ModesDictionary[mode2];
                                        if (Dictionaries.TypesOfEventDictionary.ContainsKey(type)) type = Dictionaries.TypesOfEventDictionary[type];




                                        TimetableEvent eventOLD = new TimetableEvent()
                                        {
                                            AcademicYear = "2018/2019",
                                            Building = e.Building,
                                            DayOfWeek = dni[(int)day.DayOfWeek],
                                            Degree = semester.Degree,
                                            Department = department.Department,
                                            EndTime = e.EndTime.ToString("HH:mm"),
                                            StartTime = e.StartTime.ToString("HH:mm"),
                                            FacultyGroup = "",
                                            FieldOfStudy = field,
                                            Group = group.Group.ToString(),
                                            Specialization = specs[group.Group - 1],
                                            Semester = semester.Semester.ToString(),
                                            IsFaculty = false,
                                            Lecturers = e.Lecturers.ToArray(),
                                            Mode = mode2,
                                            Name = e.Name,
                                            Remarks = e.Remarks,
                                            Room = e.Room,
                                            Type = type,
                                            Year = semester.Year.ToString(),
                                        };
                                        ((List<TimetableEvent>)t.Events).Add(eventOLD);
                                    }
                        }
            }
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
            Events = Events.OrderBy(i => i.Department)
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
