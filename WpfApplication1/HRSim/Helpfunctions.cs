using System.Collections.Generic;

//TODO: Died Minions

namespace HRSim
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;

    public class Helpfunctions
    {

        public event EventHandler<String> LogUpdate;

        //HRSim.MainWindow window = null;
        public string logpath = "C:\\Code\\ConsoleApplication1\\ConsoleApplication1\\";
        public string logfile = "UILogg" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + ".txt";
        public string path = "C:\\Code\\ConsoleApplication1\\ConsoleApplication1\\";

        public Stopwatch timer = new Stopwatch();

        public static List<T> TakeList<T>(IEnumerable<T> source, int limit)
        {
            List<T> retlist = new List<T>();
            int i = 0;

            foreach (T item in source)
            {
                retlist.Add(item);
                i++;

                if (i >= limit) break;
            }
            return retlist;
        }


        public bool runningbot = false;

        private static Helpfunctions instance;

        public static Helpfunctions Instance
        {
            get
            {
                return instance ?? (instance = new Helpfunctions());
            }
        }

        private Helpfunctions()
        {
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(HRSim.MainWindow))
            //    {
            //        this.window = (HRSim.MainWindow)window;
            //    }
            //}
            System.IO.File.WriteAllText(logpath + logfile, "");
        }

        private bool writelogg = true;
        public void loggonoff(bool onoff)
        {
            writelogg = onoff;
        }

        public void startTimer() {
            timer.Reset();
            timer.Start();
        }

        public void logTime(String tag)
        {
            timer.Stop();
            Helpfunctions.Instance.logg(tag + ": " + timer.ElapsedTicks.ToString());
            timer.Reset();
            timer.Start();
        }

        public void createNewLoggfile()
        {
            System.IO.File.WriteAllText(logpath + logfile, "");
        }

        public void logg(string s)
        {

            if (!writelogg) return;
            try
            {
                using (StreamWriter sw = File.AppendText(logpath + logfile))
                {
                    sw.WriteLine(s);
                }
            }
            catch { }
            //Console.WriteLine(s);
            ErrorLog(s);
        }

        public DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void ErrorLog(string s)
        {
            //HREngine.API.Utilities.HRLog.Write(s);
            //Console.WriteLine(s);
            Debug.WriteLine(s);
            //window.appendLog(s);
            var handler = LogUpdate;
            if (handler != null)
            {
                handler(this, s);
            }
        }

        string sendbuffer = "";
        public void resetBuffer()
        {
            this.sendbuffer = "";
        }

        public void writeToBuffer(string data)
        {
            this.sendbuffer += "\r\n" + data;
        }

        public void writeBufferToFile()
        {
            bool writed = true;
            this.sendbuffer += "<EoF>";
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(path + "crrntbrd.txt", this.sendbuffer);
                    writed = false;
                }
                catch
                {
                    writed = true;
                }
            }
            this.sendbuffer = "";
        }

        public void writeBufferToActionFile()
        {
            bool writed = true;
            this.sendbuffer += "<EoF>";
            this.ErrorLog("write to action file: " + sendbuffer);
            while (writed)
            {
                try
                {
                    System.IO.File.WriteAllText(path + "actionstodo.txt", this.sendbuffer);
                    writed = false;
                }
                catch
                {
                    writed = true;
                }
            }
            this.sendbuffer = "";

        }

    }
    
}