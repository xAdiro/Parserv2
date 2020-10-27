using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Parsers.TimetableNew.Models;

namespace Parsers.TimetableNew
{
    public class Parser
    {
        public static Timetable ParseTimetableFiles(IEnumerable<string> filesContents, DateTime date)
        {
            string[] dni = { "PN", "WT", "ŚR", "CZW", "PT", "SO", "NIE" };
            Timetable timetable = new Timetable() { Date = date };
            foreach (var content in filesContents)
            {
                var lines = Regex.Split(content, Environment.NewLine);
                Dictionary<string, string> groups = new Dictionary<string, string>();
                Dictionary<string, string> degrees = TimetableOLD.Models.Dictionaries.DegreesDictionary2;
                TimetableInfo currentInfo = new TimetableInfo();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].ToUpper().Trim() == "GROUPS")
                    {
                        string[] grs = lines.Skip(i + 1).TakeWhile(l => l.ToLower().Trim() != "end_groups.").ToArray();
                        foreach (var gr in grs)
                        {
                            string[] g = gr.Split(';');
                            if (!groups.ContainsKey(g[0].Trim()))
                            {
                                groups.Add(g[0].Trim(), g.Length > 1 ? g[1].Trim() : "");
                            }
                        }
                        i += grs.Length + 1;
                    }
                    else if (lines[i].ToUpper().Trim() == "INFO")
                    {
                        string[] infoLines = lines.Skip(i + 1).TakeWhile(l => l.ToLower().Trim() != "end_info.").ToArray();
                        string[] infos = infoLines[0].Split(';');
                        currentInfo = new TimetableInfo()
                        {
                            AcademicYear = infos[0].Trim(),
                            Department = infos[1].Trim(),
                            Mode = infos[2].Trim(),
                            Field = infos[3].Trim(),
                            Degree = degrees.ContainsKey(infos[4].Trim().ToUpper()) ? degrees[infos[4].Trim().ToUpper()] : infos[4].Trim(),
                            Semester = Convert.ToInt32(infos[5].Trim()),
                            Year = Convert.ToInt32(infos[6].Trim()),
                        };
                        i += infoLines.Length + 1;
                    }
                    else if (dni.Contains(lines[i].ToUpper().Trim()))
                    {
                        DayOfWeek day = 0;
                        for (int j = 0; j < dni.Length; j++)
                        {
                            if (lines[i].ToUpper().Trim() == dni[j]) day = (DayOfWeek)j;
                        }

                        string[] dayLines = lines.Skip(i + 1).TakeWhile(l => l.ToLower().Trim() != "end_day.").ToArray();
                        for (int j = 0; j < dayLines.Length;)
                        {
                            string[] eventLines = lines.Skip(i + j + 1).TakeWhile(l => l.ToLower().Trim() != "end_event.").ToArray();
                            string name, type, room, building, remarks;
                            List<string> lecturers = new List<string>(), facultyGroups = new List<string>();
                            List<string> groupsNrs = new List<string>();
                            DateTime startTime, endTime;
                            name = eventLines[0].Split(';')[0].Trim();
                            try
                            {
                                type = eventLines[0].Split(';')[1].Trim();
                            }
                            catch(Exception ex)
                            {
                                type = "?";
                            }
                            //TODO: add facultygroups
                            foreach (string s in eventLines[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                groupsNrs.Add(s.Trim());
                            }
                            List<TimetableGroup> eventGroups = new List<TimetableGroup>();
                            foreach (var item in groupsNrs)
                            {
                                eventGroups.Add(new TimetableGroup() { Group = item, Specialization = groups.ContainsKey(item) ? groups[item] : "" });
                            }

                            startTime = DateTime.ParseExact(eventLines[2].Split('-')[0], new[] { "HH:mm", "H:mm" }, null, DateTimeStyles.AllowWhiteSpaces);
                            endTime = DateTime.ParseExact(eventLines[2].Split('-')[1], new[] { "HH:mm", "H:mm" }, null, DateTimeStyles.AllowWhiteSpaces);

                            string[] where = eventLines[3].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            room = where[0].Trim();
                            building = where.Length > 1 ? where[1].Trim() : "";

                            foreach (string l in eventLines[4].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                lecturers.Add(l.Trim());
                            }

                            remarks = eventLines[5].Trim();

                            TimetableEvent ev = new TimetableEvent()
                            {
                                Name = name,
                                Type = type,
                                Groups = eventGroups,
                                FacultyGroups = new List<string>(),
                                StartTime = startTime,
                                EndTime = endTime,
                                Room = room,
                                Building = building,
                                Lecturers = lecturers,
                                Remarks = remarks
                            };

                            TimetableAcademicYear k = MakeTimetableAcademicYear(currentInfo.AcademicYear, currentInfo.Department, currentInfo.Mode, currentInfo.Field, currentInfo.Semester, currentInfo.Degree, currentInfo.Year, day, ev);

                            MergeToTimetable(timetable, k);


                            j += eventLines.Length + 1;
                        }
                    }
                }
            }

            //sorting all events
            SortTimetable(timetable);

            return timetable;
        }

        public static Timetable SortTimetable(Timetable t)
        {
            t.AcademicYears = t.AcademicYears.OrderBy(i => i.AcademicYear).ToList();
            foreach (var item0 in t.AcademicYears)
            {
                item0.Departments = item0.Departments.OrderBy(i => i.Department).ToList();
                foreach (var item1 in item0.Departments)
                {
                    item1.Modes = item1.Modes.OrderBy(i => i.Mode).ToList();
                    foreach (var item2 in item1.Modes)
                    {
                        item2.Fields = item2.Fields.OrderBy(i => i.FieldOfStudy).ToList();
                        foreach (var item3 in item2.Fields)
                        {
                            item3.Semesters = item3.Semesters.OrderBy(i => i.Semester).ToList();
                            foreach (var item4 in item3.Semesters)
                            {
                                item4.Days = item4.Days.OrderBy(i => i.DayOfWeek).ToList();
                                foreach (var item5 in item4.Days)
                                {
                                    item5.Events = item5.Events.OrderBy(i => i.StartTime).ToList();
                                    foreach (var item6 in item5.Events)
                                    {
                                        item6.Groups = item6.Groups.OrderBy(i => i.Group).ToList();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return t;
        }

        public static TimetableAcademicYear MakeTimetableAcademicYear(string academicYear, string department, string mode, string field, int semester, string degree, int year, DayOfWeek dayOfWeek, TimetableEvent e)
        {
            return new TimetableAcademicYear()
            {
                AcademicYear = academicYear,
                Departments = new List<TimetableDepartment>()
                {
                    new TimetableDepartment()
                    {
                        Department = department,
                        Modes = new List<TimetableMode>()
                        {
                            new TimetableMode()
                            {
                                Mode = mode,
                                Fields = new List<TimetableField>()
                                {
                                    new TimetableField()
                                    {
                                        FieldOfStudy = field,
                                        Semesters = new List<TimetableSemester>()
                                        {
                                            new TimetableSemester()
                                            {
                                                Semester = semester,
                                                Degree = degree,
                                                Year = year,
                                                Days = new List<TimetableDay>()
                                                {
                                                    new TimetableDay()
                                                    {
                                                        DayOfWeek = dayOfWeek,
                                                        Events = new List<TimetableEvent>()
                                                        {
                                                            e
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public static Timetable MergeToTimetable(Timetable t, TimetableAcademicYear d)
        {
            //adding event to the timetable tree
            if (t.AcademicYears.Exists(e => e.AcademicYear == d.AcademicYear))
            {
                TimetableAcademicYear nextLayerTimetable = t[d.AcademicYear].First();
                TimetableDepartment nextLayerK0 = d.Departments[0];
                if (nextLayerTimetable.Departments.Exists(e => e.Department == nextLayerK0.Department))
                {
                    TimetableDepartment nextLayerTimetable1 = nextLayerTimetable[nextLayerK0.Department].First();
                    TimetableMode nextLayerK1 = nextLayerK0.Modes[0];
                    if (nextLayerTimetable1.Modes.Exists(e => e.Mode == nextLayerK1.Mode))
                    {
                        TimetableMode nextLayerTimetable2 = nextLayerTimetable1[nextLayerK1.Mode].First();
                        TimetableField nextLayerK2 = nextLayerK1.Fields[0];
                        if (nextLayerTimetable2.Fields.Exists(e => e.FieldOfStudy == nextLayerK2.FieldOfStudy))
                        {
                            TimetableField nextLayerTimetable3 = nextLayerTimetable2[nextLayerK2.FieldOfStudy].First();
                            TimetableSemester nextLayerK3 = nextLayerK2.Semesters[0];
                            if (nextLayerTimetable3.Semesters.Exists(e => e.Semester == nextLayerK3.Semester && e.Degree == nextLayerK3.Degree))
                            {
                                TimetableSemester nextLayerTimetable4 = nextLayerTimetable3[nextLayerK3.Semester, nextLayerK3.Degree, nextLayerK3.Year].First();
                                TimetableDay nextLayerK4 = nextLayerK3.Days[0];
                                if (nextLayerTimetable4.Days.Exists(e => e.DayOfWeek == nextLayerK4.DayOfWeek))
                                {
                                    //TimetableDay nextLayerTimetable5 = nextLayerTimetable4[nextLayerK4.DayOfWeek].First();
                                    //nextLayerTimetable5.Events.AddRange(nextLayerK4.Events);
                                    TimetableDay nextLayerTimetable5 = nextLayerTimetable4[nextLayerK4.DayOfWeek].First();
                                    TimetableEvent nextLayerK5 = nextLayerK4.Events[0];
                                    if (nextLayerTimetable5.Events.Any(e => e.EqualsExceptGroups(nextLayerK5)))
                                    {
                                        TimetableEvent nextLayerTimetable6 = nextLayerTimetable5.Events.Find(e => e.EqualsExceptGroups(nextLayerK5));
                                        nextLayerTimetable6.Groups = nextLayerTimetable6.Groups.Union(nextLayerK5.Groups).ToList();
                                        nextLayerTimetable6.FacultyGroups = nextLayerTimetable6.FacultyGroups.Union(nextLayerK5.FacultyGroups).ToList();
                                    }
                                    else
                                    {
                                        nextLayerTimetable5.Events.Add(nextLayerK5);
                                    }
                                }
                                else
                                {
                                    nextLayerTimetable4.Days.Add(nextLayerK4);
                                }
                            }
                            else
                            {
                                nextLayerTimetable3.Semesters.Add(nextLayerK3);
                            }
                        }
                        else
                        {
                            nextLayerTimetable2.Fields.Add(nextLayerK2);
                        }
                    }
                    else
                    {
                        nextLayerTimetable1.Modes.Add(nextLayerK1);
                    }
                }
                else
                {
                    nextLayerTimetable.Departments.Add(nextLayerK0);
                }
            }
            else
            {
                t.AcademicYears.Add(d);
            }

            return t;
        }

    }
}
