using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ParserApp
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Reading from input folder");
            Console.WriteLine("Searching for txt files...");
            string[] filesPathsArray = Directory.GetFiles(@".\input\", "*.txt");
            Console.WriteLine("Found " + filesPathsArray.Length + " txt file" + (filesPathsArray.Length == 1 ? "" : "s"));

            //Console.WriteLine("Reading files");
            //DateTime lastDate = new DateTime();
            //List<string> filesContents = new List<string>();
            //foreach (var filePath in filesPathsArray)
            //{
            //    StreamReader streamReader = new StreamReader(filePath);
            //    FileInfo fileInfo = new FileInfo(filePath);
            //    if (fileInfo.LastWriteTime > lastDate) lastDate = fileInfo.LastWriteTime;
            //    filesContents.Add(streamReader.ReadToEnd());
            //    streamReader.Close();
            //}
            
            string[] odps = { "1", "2", "3"};
            string odp;
            do
            {
                Console.WriteLine("Co chcesz zrobić?:");
                Console.WriteLine("1. nowe pliki -> JSONy");
                Console.WriteLine("2. stare pliki -> JSONy");
                Console.WriteLine("3. nowe pliki -> stare pliki");
                odp = Console.ReadKey().KeyChar.ToString();
            } while (!odps.Contains(odp));

            if(odp is "1")
            {
                Console.WriteLine("Reading files");
                DateTime lastDate = new DateTime();
                List<string> filesContents = new List<string>();
                foreach (var filePath in filesPathsArray)
                {
                    StreamReader streamReader = new StreamReader(filePath);
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.LastWriteTime > lastDate) lastDate = fileInfo.LastWriteTime;
                    filesContents.Add(streamReader.ReadToEnd());
                    streamReader.Close();
                }

                Console.WriteLine("\nGenerowanie jsonów");
                Parsers.TimetableNew.Models.Timetable tn = Parsers.TimetableNew.Parser.ParseTimetableFiles(filesContents, lastDate);

                Console.WriteLine("Writing to timetable.json");
                StreamWriter streamWriter = new StreamWriter("output/timetable.json");
                streamWriter.Write(JsonConvert.SerializeObject(tn));
                streamWriter.Close();

                Parsers.TimetableOLD.Models.Timetable tOLD = (Parsers.TimetableOLD.Models.Timetable)tn;

                Console.WriteLine("Writing to timetableOLD.json");
                StreamWriter streamWriter2 = new StreamWriter("output/timetableOLD.json");
                streamWriter2.Write(JsonConvert.SerializeObject(tOLD));
                streamWriter2.Close();

                //counting events
                int tEventsNumber = 0;
                int tmEventsNumber = ((List<Parsers.TimetableOLD.Models.TimetableEvent>)tOLD.Events).Count;
                foreach (var item in tn.AcademicYears)
                    foreach (var item2 in item.Modes)
                        foreach (var item3 in item2.Fields)
                            foreach (var item4 in item3.Semesters)
                                foreach (var item5 in item4.Days)
                                    tEventsNumber += item5.Events.Count;

                Console.WriteLine("Done");
                Console.WriteLine("new json has: " + tEventsNumber + " events");
                Console.WriteLine("old json has: " + tmEventsNumber + " events");
            }
            else if (odp is "2")
            {
                Console.WriteLine("Reading files");
                List<string> filepaths = filesPathsArray.ToList();

                Console.WriteLine("\nGenerowanie jsonów");
                Parsers.TimetableOLD.Models.Timetable tOLD = Parsers.TimetableOLD.Parser.ParseTimetableFiles(filepaths);

                Console.WriteLine("Writing to timetableOLD.json");
                StreamWriter streamWriter = new StreamWriter("output/timetableOLD.json");
                streamWriter.Write(JsonConvert.SerializeObject(tOLD));
                streamWriter.Close();

                Parsers.TimetableNew.Models.Timetable tn = (Parsers.TimetableNew.Models.Timetable)tOLD;

                Console.WriteLine("Writing to timetable.json");
                StreamWriter streamWriter2 = new StreamWriter("output/timetable.json");
                streamWriter2.Write(JsonConvert.SerializeObject(tn));
                streamWriter2.Close();

                //counting events
                int tEventsNumber = 0;
                int tmEventsNumber = ((List<Parsers.TimetableOLD.Models.TimetableEvent>)tOLD.Events).Count;
                foreach (var item in tn.AcademicYears)
                    foreach (var item2 in item.Modes)
                        foreach (var item3 in item2.Fields)
                            foreach (var item4 in item3.Semesters)
                                foreach (var item5 in item4.Days)
                                    tEventsNumber += item5.Events.Count;

                Console.WriteLine("Done");
                Console.WriteLine("new json has: " + tEventsNumber + " events");
                Console.WriteLine("old json has: " + tmEventsNumber + " events");
            }
            else
            {
                Console.WriteLine("Reading files");
                DateTime lastDate = new DateTime();
                List<string> filesContents = new List<string>();
                foreach (var filePath in filesPathsArray)
                {
                    StreamReader streamReader = new StreamReader(filePath);
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.LastWriteTime > lastDate) lastDate = fileInfo.LastWriteTime;
                    filesContents.Add(streamReader.ReadToEnd());
                    streamReader.Close();
                }
                Console.WriteLine("\nGenerowanie starych plików");
                Console.WriteLine("Parsing");
                Parsers.TimetableNew.Models.Timetable tn = Parsers.TimetableNew.Parser.ParseTimetableFiles(filesContents, lastDate);
                Parsers.TimetableOLD.Models.Timetable tOLD = (Parsers.TimetableOLD.Models.Timetable)tn;
                Parsers.TimetableOLD.Parser.SaveTimetableToFiles(tOLD, "output");

                Console.WriteLine("Done");
            }
            Console.ReadLine();
        }
    }
}
