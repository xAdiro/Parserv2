using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GenerateTimetableFromFiles
{
    class Program
    {
        static bool forceDateNow = false;

        static (List<string>, List<string>) LoadFilesFromFolder(string path)
        {
            return (
                Directory.GetFiles(path, "*.*").Where(s => s.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)).ToList(),
                Directory.GetFiles(path, "*.*").Where(s => s.EndsWith(".pwzim", StringComparison.OrdinalIgnoreCase)).ToList()
                );
        }

        static Parsers.TimetableOLD.Models.Timetable Parse(List<string> txts, List<string> pwzims)
        {
            if (txts.Count == 0 && pwzims.Count == 0)
            {
                throw new Exception("Nothing to parse!");
            }


            Parsers.TimetableOLD.Models.Timetable ParseNew()
            {
                //PLIKI .pwzim
                DateTime lastDate = new DateTime();
                List<string> filesContents = new List<string>();
                foreach (string filePath in pwzims)
                {
                    StreamReader streamReader = new StreamReader(filePath);
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.LastWriteTime > lastDate) lastDate = fileInfo.LastWriteTime;
                    filesContents.Add(streamReader.ReadToEnd());
                    streamReader.Close();
                }
                return (Parsers.TimetableOLD.Models.Timetable)Parsers.TimetableNew.Parser.ParseTimetableFiles(filesContents, lastDate);
            }
            Parsers.TimetableOLD.Models.Timetable ParseOld()
            {
                return Parsers.TimetableOLD.Parser.ParseTimetableFiles(txts);
            }

            if (pwzims.Count > 0 && txts.Count > 0)
            {
                Parsers.TimetableOLD.Models.Timetable tOLD = ParseNew();
                Parsers.TimetableOLD.Models.Timetable tOLD2 = ParseOld();
                //merge
                Parsers.TimetableOLD.Models.Timetable tResult = tOLD.MergeTimetables(tOLD2);
                if (forceDateNow) tResult.Date = DateTime.Now;
                return tResult;
            }
            else if (pwzims.Count > 0)
            {
                Parsers.TimetableOLD.Models.Timetable tOLD = ParseNew();
                if (forceDateNow) tOLD.Date = DateTime.Now;
                return tOLD;
            }
            else
            {
                Parsers.TimetableOLD.Models.Timetable tOLD2 = ParseOld();
                if (forceDateNow) tOLD2.Date = DateTime.Now;
                return tOLD2;
            }
        }

        static string GenTimetableJson(Parsers.TimetableOLD.Models.Timetable timetableResult)
        {
            // experimental 1.5 old json format
            Parsers.TimetableOLD2.Models.Timetable t = (Parsers.TimetableOLD2.Models.Timetable)timetableResult;
            if (forceDateNow)
            {
                t.Date = DateTime.Now;
            }
            Parsers.TimetableOLD3.Models.Timetable t3 = (Parsers.TimetableOLD3.Models.Timetable)t;
            return JsonConvert.SerializeObject(t3);
        }

        static void SaveToFile(string contents, string path = "./timetable.json")
        {
            StreamWriter sw = new StreamWriter(
                new FileStream(path, FileMode.Create, FileAccess.ReadWrite),
                Encoding.UTF8
            );
            sw.Write(contents);
            sw.Close();
        }

        static void Main(string[] args)
        {
            List<string> argsList = new List<string>(args);
            if (argsList.Contains("-forceDate") || argsList.Contains("-fd"))
            {
                forceDateNow = true;
                argsList.Remove("-forceDate");
                argsList.Remove("-fd");
            }

            if (argsList.Count == 0)
            {
                Console.WriteLine("Nie podano żadnej ścieżki do plików txt/pwzim");
                return;
            }
            List<string> txts = new List<string>();
            List<string> pwzims = new List<string>();
            foreach (var path in argsList)
            {
                if (Directory.Exists(path))
                {
                var (txt, pwzim) = LoadFilesFromFolder(path);
                txts.AddRange(txt);
                pwzims.AddRange(pwzim);
                }
                else
                {
                    Console.WriteLine("ERROR: {0} does not exist", path);
                }
            }
            SaveToFile(GenTimetableJson(Parse(txts, pwzims)));

        }
    }
}
