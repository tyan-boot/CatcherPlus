using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CatcherPlus
{
    class FNJson
    {
        public int count;
        public List<FNCmt> comments;
    }
    class FNCmt
    {
        public string uname;
        public string ip_from;
        public string create_time;
        public string comment_contents;
        public int uptimes;
    }

    class ifeng
    {
        private string CmtUrl = "http://comment.ifeng.com/get.php?orderby=DESC&format=json&job=1&pageSize=20&";

        private int page = 1;
        //private HttpParm pt = new HttpParm();
        private Dictionary<string, string> FNParm = new Dictionary<string, string>();

        private string Url;
        private int num;
        private int round;
        private int currentRound = 0;

        private List<Common.Cmt> Cmts = new List<Common.Cmt>();

        public ifeng(string Url)
        {
            this.Url = Url;

            FNParm.Add("p", page.ToString());
            FNParm.Add("docurl", Url);

            Init();
        }

        private void Init()
        {
 
            //Console.WriteLine(url);
            string htmldata = HttpHelper.GetHtml(CmtUrl + HttpHelper.arry2urlencoded(FNParm),"application/json");
            var jc = JsonConvert.DeserializeObject<FNJson>(htmldata);
            num = jc.count;
            round = num / 20;
            round++;
            Console.WriteLine("1");
        }

        public List<Common.Cmt> GetNextCmts()
        {
            if (currentRound >= round) return null;

            List<Common.Cmt> cmts = new List<Common.Cmt>();
            FNParm["p"] = page.ToString();
            string htmldata = HttpHelper.GetHtml(CmtUrl + HttpHelper.arry2urlencoded(FNParm), "application/json");
            var jc = JsonConvert.DeserializeObject<FNJson>(htmldata);

            foreach (var cmt in jc.comments)
            {
                Common.Cmt fncmt = new Common.Cmt();
                fncmt.name = cmt.uname;
                fncmt.date = GetTime(cmt.create_time).ToString();
                fncmt.location = cmt.ip_from;
                fncmt.up = cmt.uptimes.ToString();
                fncmt.content = cmt.comment_contents;

                cmts.Add(fncmt);
                Cmts.Add(fncmt);
            }

            currentRound++;
            return cmts;
        }

        public int GetNum()
        {
            return num;
        }

        public List<Common.Cmt> GetAllCmts()
        {
            return Cmts;
        }

        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
