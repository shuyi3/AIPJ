using System.Collections.Generic;

//TODO: Died Minions

namespace HRSim
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;

    public class Helpfunctions
    {

        public event EventHandler<String> LogUpdate;

        //HRSim.MainWindow window = null;
        public string logpath = "C:\\Code\\WpfApplication1\\WpfApplication1\\";
        public string logfile = "UILogg" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + ".txt";
        public string path = "C:\\Code\\WpfApplication1\\WpfApplication1\\";

        public StreamReader stream = null;
        public JArray sources = null;
        public int index = 0;
        public Stopwatch timer = new Stopwatch();
        public JavaScriptSerializer jsonSerializer;


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
            jsonSerializer = new JavaScriptSerializer();
            System.IO.File.WriteAllText(logpath + logfile, "");
        }

        private bool writelogg = true;
        public void loggonoff(bool onoff)
        {
            writelogg = onoff;
        }

        public void startTimer()
        {
            timer.Reset();
            timer.Start();
        }

        public double logTime(String tag = null)
        {
            timer.Stop();
            double timeElapsed = timer.Elapsed.TotalMilliseconds;
            if (tag != null)
                Helpfunctions.Instance.logg(tag + ": " + timeElapsed + " ms");
            timer.Reset();
            timer.Start();
            return timeElapsed;
        }

        public void createNewLoggfile()
        {
            System.IO.File.WriteAllText(logpath + logfile, "");
        }

        public void logg(string s)
        {

            if (!writelogg) return;
            //try
            //{
            //    using (StreamWriter sw = File.AppendText(logpath + logfile))
            //    {
            //        sw.WriteLine(s);
            //    }
            //}
            //catch { }
            Console.WriteLine(s);
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

        public int c(int N, int K)
        {

            int r = 1;
            int d;
            if (K > N) return 0;
            for (d = 1; d <= K; d++)
            {
                r *= N--;
                r /= d;
            }
            return r;
        }

        public int fact(int n)
        {
            if (n == 0)
                return 1;
            else
                return n * fact(n - 1);
        }

        public void WriteResultToFile(string fileName, string result)
        {
            // Set a variable to the My Documents path.
            string mydocpath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(mydocpath + fileName, true))
            {
                outputFile.WriteLine(result);
            }
        }



        public Playfield readJsonFile()
        {
            
            if (sources == null)
            {
                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                StreamReader stream = File.OpenText(mydocpath + @"\picked_states.txt");   
                sources = (JArray)JToken.ReadFrom(new JsonTextReader(stream));

            }

            Console.WriteLine("index = " + index);

            if (index < sources.Count)
            {
                var state = sources[index];

                var ownJson = state["own"];
                var enemyJson = state["enemy"];
                bool own = (bool)state["turn"];

                PlayerKeyInfo p1KeyInfo = ownJson.ToObject<PlayerKeyInfo>();
                PlayerKeyInfo p2KeyInfo = enemyJson.ToObject<PlayerKeyInfo>();
                Playfield parsedPlayfield = new Playfield(1000, own, p1KeyInfo, p2KeyInfo);
                index++;
                return parsedPlayfield;
            }
 
            return null;
        }

        public string convertPlayfieldToJson(Playfield p)
        {
            var json = new JavaScriptSerializer().Serialize(p.playerFirst);
            //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //Playfield ptest =
            //       (Playfield)json_serializer.DeserializeObject(json);
            return json;
        }

        //public string getPlayerKeyInfo(Player p)
        //{
        //    keyInfo.maxMana = Math.Min(10, p.ownMaxMana + 1);
        //    keyInfo.turn = p.ownController;
        //    keyInfo.heroInfo = new MinionKeyInfo(p.ownHero);
        //    List<MinionKeyInfo> minionList = new List<MinionKeyInfo>();
        //    foreach(Minion m in p.ownMinions)
        //    {
        //        MinionKeyInfo minionInfo = new MinionKeyInfo(m);
        //        minionList.Add(minionInfo);
        //    }
        //    keyInfo.minionJsonList = minionList;
        //    List<CardKeyInfo> hcList = new List<CardKeyInfo>();
        //    foreach(Handmanager.Handcard  hc in p.owncards)
        //    {
        //        CardKeyInfo hcInfo = new CardKeyInfo(hc);
        //        hcList.Add(hcInfo);
        //    }
        //    keyInfo.handcardJsonList = hcList;
        //    return new JavaScriptSerializer().Serialize(keyInfo);
        //}

        public string getMinionKeyInfo(Minion m)
        {
            PlayerKeyInfo.MinionKeyInfo keyInfo = new PlayerKeyInfo.MinionKeyInfo(m);
            return new JavaScriptSerializer().Serialize(keyInfo);
        }

        public string getCardKeyInfo(Handmanager.Handcard handcard)
        {
            PlayerKeyInfo.CardKeyInfo keyInfo = new PlayerKeyInfo.CardKeyInfo(handcard);
            return new JavaScriptSerializer().Serialize(keyInfo);
        }

        public string convertMinionToJson(Minion m)
        {
            var json = new JavaScriptSerializer().Serialize(m);
            //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //Playfield ptest =
            //       (Playfield)json_serializer.DeserializeObject(json);
            return json;
        }

        public void combination(int[] elements, int k, int count)
        {

            if (k > count)
            {
                logg("k > n");
                return;
            }
            // calculate the possible combinations
            // e.g. c(4,2)
            c(count, k);

            // get the combination by index 
            // e.g. 01 --> AB , 23 --> CD
            int[] combination = new int[k];

            // position of current index
            //  if (r = 1)              r*
            //  index ==>        0   |   1   |   2
            //  element ==>      A   |   B   |   C
            int r = 0;
            int index = 0;

            while (r >= 0)
            {
                // possible indexes for 1st position "r=0" are "0,1,2" --> "A,B,C"
                // possible indexes for 2nd position "r=1" are "1,2,3" --> "B,C,D"

                // for r = 0 ==> index < (4+ (0 - 2)) = 2
                if (index <= (count + (r - k)))
                {
                    combination[r] = index;

                    if (r == k - 1)
                    {
                        index++;
                    }
                    else
                    {
                        index = combination[r] + 1;
                        r++;
                    }
                }
                else
                {
                    r--;
                    if (r > 0)
                        index = combination[r] + 1;
                    else
                        index = combination[0] + 1;
                }
            }

        }
    }
}