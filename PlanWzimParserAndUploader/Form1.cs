using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanWzimParserAndUploader
{
    public partial class Form1 : Form
    {
        private string Version { get; } = "v1.0.0";
        private bool forceDateNow = false;
        public Form1()
        {
            InitializeComponent();
        }
        private async Task<bool> CheckLatestRelease()
        {
            GitHubClient ghClient = new GitHubClient(new Octokit.ProductHeaderValue("matyjb"));
            try
            {
                Release latest = await ghClient.Repository.Release.GetLatest(161249677);
                if(latest.TagName.CompareTo(Version) <= 0)
                {
                    if (MessageBox.Show("Wykryto nową wersje aplikacji\nCzy chcesz ją pobrać?","Aktualizacja", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://github.com/matyjb/Parserv2/releases");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "version: " + Version;
            //spr czy jest nowa wersja apki
            CheckLatestRelease();
            //download date planu z serwera
            RefreshTimetableDate();
        }

        private void LbOldFiles_KeyDown(object sender, KeyEventArgs e)
        {
            //na delete usunąć zaznaczoną pozycje
            if(e.KeyCode == Keys.Delete)
            {
                ListBox.SelectedObjectCollection selectedItems = lbOldFiles.SelectedItems;

                if (lbOldFiles.SelectedIndex != -1)
                {
                    for (int i = selectedItems.Count - 1; i >= 0; i--)
                        lbOldFiles.Items.Remove(selectedItems[i]);
                }
            }
        }

        private void LbNewFiles_KeyDown(object sender, KeyEventArgs e)
        {
            //na delete usunąć zaznaczoną pozycje
            if (e.KeyCode == Keys.Delete)
            {
                ListBox.SelectedObjectCollection selectedItems = lbNewFiles.SelectedItems;

                if (lbNewFiles.SelectedIndex != -1)
                {
                    for (int i = selectedItems.Count - 1; i >= 0; i--)
                        lbNewFiles.Items.Remove(selectedItems[i]);
                }
            }
        }

        private void BFilesLoad_Click(object sender, EventArgs e)
        {
            // okienko przyjmujące pliki .txt i .pwzim
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "txt or pwzim files|*.txt;*.pwzim",
                Title = "Wybierz pliki do załadowania",
                Multiselect = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog1.FileNames)
                {
                    if (file.Substring(file.LastIndexOf('.'))==".txt")
                    {
                        lbOldFiles.Items.Add(file);
                    }
                    if (file.Substring(file.LastIndexOf('.')) == ".pwzim")
                    {
                        lbNewFiles.Items.Add(file);
                    }
                }
            }
            // refresh counters
            label6.Text = lbOldFiles.Items.Count.ToString();
            label7.Text = lbNewFiles.Items.Count.ToString();
        }

        private void BParse_Click(object sender, EventArgs e)
        {
            // parsuj pliki z listboxów na typ stary Timetable
            string output;
            bUpload.Enabled = true;
            //try
            //{
                Parsers.TimetableOLD.Models.Timetable ParseNew()
                {
                    //PLIKI .pwzim
                    DateTime lastDate = new DateTime();
                    List<string> filesContents = new List<string>();
                    foreach (string filePath in lbNewFiles.Items)
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
                    //PLIKI .txt
                    List<string> filepaths = new List<string>();
                    foreach (string fp in lbOldFiles.Items)
                    {
                        filepaths.Add(fp);
                    }
                    return Parsers.TimetableOLD.Parser.ParseTimetableFiles(filepaths);
                }

                if(lbNewFiles.Items.Count > 0 && lbOldFiles.Items.Count > 0)
                {
                    Parsers.TimetableOLD.Models.Timetable tOLD = ParseNew();
                    Parsers.TimetableOLD.Models.Timetable tOLD2 = ParseOld();
                    //merge
                    Parsers.TimetableOLD.Models.Timetable tResult = tOLD.MergeTimetables(tOLD2);
                    if (forceDateNow) tResult.Date = DateTime.Now;
                    output = JsonConvert.SerializeObject(tResult);
                }
                else if(lbNewFiles.Items.Count > 0)
                {
                    Parsers.TimetableOLD.Models.Timetable tOLD = ParseNew();
                    if (forceDateNow) tOLD.Date = DateTime.Now;
                    output = JsonConvert.SerializeObject(tOLD);
                }
                else if (lbOldFiles.Items.Count > 0)
                {
                    Parsers.TimetableOLD.Models.Timetable tOLD2 = ParseOld();
                    if (forceDateNow) tOLD2.Date = DateTime.Now;
                    output = JsonConvert.SerializeObject(tOLD2);
                }
                else
                {
                    output = "Brak plików";
                    bUpload.Enabled = false;
                }
            //}
            //catch (Exception ex)
            //{
            //    // jak są errory to przycisk upload na szaro
            //    output = ex.Message;
            //    bUpload.Enabled = false;
            //}
            // cały output (albo errory) do richboxa 
            rtbOutput.Text = output;
            // zmiana label pochodzenie jsona na "z plików"
            label2.Text = "Json wygenereowany z plików:";
        }

        private void BLoadJson_Click(object sender, EventArgs e)
        {
            // okienko przyjmujące pliki .json
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "JSON|*.json",
                Title = "Wybierz plik json do załadowania"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // zmiana label pochodzenie jsona na "z jsona"
                label2.Text = "Json załadowany z pliku:";
                // zakładamy ze json jest poprawny, wiec przycisk upload na active
                bUpload.Enabled = true;
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                rtbOutput.Text = sr.ReadToEnd();
            }
        }

        private void BDownloadJson_Click(object sender, EventArgs e)
        {
            // pobranie aktualnego planu z serwera
            string timetableJson = PlanWzimServices.FetchTimetable();
            // okienko do zapisu pliku .json
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "JSON|*.json",
                Title = "Zapisz plik json planu zajęć"
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter fs = new StreamWriter(saveFileDialog1.OpenFile());
                fs.Write(timetableJson);
                fs.Close();
            }
        }

        private void BUpload_Click(object sender, EventArgs e)
        {
            // upload
            PlanWzimServices.PutJson(rtbOutput.Text);
            // refresh date
            RefreshTimetableDate();
        }

        private void RefreshTimetableDate()
        {
            string date = PlanWzimServices.GetDate();
            label3.Text = label3.Text.Remove(label3.Text.IndexOf(':'));
            label3.Text += ": " + date;
        }

        private async void BCheckUpdate_Click(object sender, EventArgs e)
        {
            bCheckUpdate.Enabled = false;
            bool isUpdate = await CheckLatestRelease();
            if (!isUpdate)
            {
                MessageBox.Show("Brak aktualizacji");
            }
            bCheckUpdate.Enabled = true;
        }

        private void CheckBox1_Click(object sender, EventArgs e)
        {
            forceDateNow = checkBox1.Checked;
        }
    }
    public static class PlanWzimServices
    {
        public static string GetDate()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://plan.silver.sggw.pl/api/timetable/date");//
            request.Method = "Get";
            request.KeepAlive = true;
            request.ContentType = "appication/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd();
        }
        public static async void PutJson(string json)
        {
            //tmp fix (bad time on server)
            for (int i = 0; i < 10; i++)
            {
                int hours = DateTime.Now.Hour;
                int minutes = DateTime.Now.Minute - i;
                if (minutes < 0)
                {
                    hours--;
                    if (hours < 0) hours = 24;
                    minutes += 60;
                }

                //

                string permission = $"GHGZ0-{minutes}-{hours}-mzqZ934-dxd9";
                string uri = "https://plan.silver.sggw.pl/api/timetable/list/" + permission;
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync(
                        uri,
                        new StringContent(json, Encoding.UTF8, "application/json")
                    );
              
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        MessageBox.Show("Plan zaktualizowano");
                        break;
                    }
                    else
                    {
                        MessageBox.Show(response.ToString() + "\nretries left: "+(11-i));
                    }
                }
            }
        }
            public static string FetchTimetable()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://plan.silver.sggw.pl/api/timetable/");
            request.Method = "Get";
            request.KeepAlive = true;
            request.ContentType = "appication/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd();
        }
    }
}
