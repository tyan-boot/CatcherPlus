using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;

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

        private Dictionary<string, string> FNParm = new Dictionary<string, string>();

        private string Url;
        private int num;
        private int round;
        private int currentRound = 0;
        private string file;

        private List<Common.Cmt> Cmts = new List<Common.Cmt>();

        private MainWin mw;
        private ExcelHelper eh;

        public ifeng(string Url, string file, MainWin mw)
        {
            this.Url = Url;
            this.file = file;
            this.mw = mw;

            FNParm.Add("p", page.ToString());
            FNParm.Add("docurl", Url);

            Init();
        }

        private void Init()
        {
            string htmldata = HttpHelper.GetHtml(CmtUrl + HttpHelper.arry2urlencoded(FNParm), "application/json");
            var jc = JsonConvert.DeserializeObject<FNJson>(htmldata);
            num = jc.count;
            round = num / 20;
            round++;
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
            page++;
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

        public void Run()
        {
            mw.State("正在抓取");
            eh = new ExcelHelper(file);

            mw.SetProgressBar(0, num);

            var cmts = this.GetNextCmts();

            while (cmts != null)
            {
                foreach (var cmt in cmts)
                {
                    mw.AddPVB(cmt);
                    mw.AddProgressBar1();
                }
                cmts = this.GetNextCmts();
            }

            foreach (var cmt in Cmts)
            {
                eh.AddRow(cmt);
                mw.AddProgressBar2();
            }
            eh.Save();
            mw.State("完成");
            MessageBox.Show("抓取完成", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
