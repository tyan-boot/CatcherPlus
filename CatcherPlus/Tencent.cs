using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace CatcherPlus
{
    public class QQJson
    {
        public string errCode;

        public CmtData data;
    }
    public class CmtData
    {
        public int total = 0;
        public string first;
        public string last;
        public bool hasnext;
        public IList<Cmt> commentid;
    }
    public class Cmt
    {
        public string id;
        public long targetid;
        public string orireplynum;

        public string rootid;
        public string parent;
        public long time;
        public string content;
        public string up;
        //public string location;       早知道就不弄这个了,没啥用,还引起一堆异常,腾讯新闻后台人员搞毛线啊.

        public QQUserInfo userinfo;
    }

    public class QQUserInfo
    {
        public string nick;
        public string region;
    }

    public class QQCmtWithNode : Common.Cmt
    {
        public List<Common.Cmt> replys = new List<Common.Cmt>();
    }

    public class Tencent
    {
        private static Regex reNewID = new Regex("(?<=coral.qq.com\\/)\\w+");

        private string Url;
        private string NewID;
        private int total;
        private bool hasnext;
        private string file;
        private string BaseCmtUrl;
        private string NodeUrl;

        private Dictionary<string, int> QQParm = new Dictionary<string, int>();
        private Dictionary<string, string> NodeParm = new Dictionary<string, string>();

        private List<Common.Cmt> QQCmts = new List<Common.Cmt>();

        private int reqnum = 50;
        private string first = "";

        private MainWin mw;
        private ExcelHelper eh;

        public Tencent(string Url, string file, MainWin mw)
        {
            this.Url = Url;
            this.file = file;
            this.mw = mw;

            QQParm.Add("commentid", 0);
            QQParm.Add("reqnum", 0);

            NodeParm.Add("targetid", "");
            NodeParm.Add("reqnum", "20");
            NodeParm.Add("pageflag", "1");
            NodeParm.Add("parentid", "");

            if (!Init())
                throw new Exception("获取数据失败!");
        }

        private bool Init()
        {
            Match mid = reNewID.Match(Url);

            if (mid.Success)
            {
                NewID = mid.Groups[0].Value;
            }
            else return false;

            BaseCmtUrl = "http://coral.qq.com/article/" + NewID + "/comment?";
            NodeUrl = "http://coral.qq.com/comment/reply/node?";

            QQParm["reqnum"] = 1;
            string CmtUrl = BaseCmtUrl + HttpHelper.arry2urlencoded(QQParm);

            string htmldata = HttpHelper.GetHtml(CmtUrl, "application/json");
            if (htmldata == null) return false;
            QQJson jc = JsonConvert.DeserializeObject<QQJson>(htmldata);

            this.total = jc.data.total;
            this.hasnext = jc.data.hasnext;
            return true;
        }

        private string GetParm()
        {
            string parm = "commentid=" + this.first + "&reqnum=" + this.reqnum.ToString();
            return parm;
        }

        public List<Common.Cmt> GetNodeCmts(string parent)
        {
            List<Common.Cmt> nodecmts = new List<Common.Cmt>();

            return nodecmts;
        }

        public List<Common.Cmt> GetNextCmts()
        {
            if (this.hasnext)
            {
                List<Common.Cmt> lcmts = new List<Common.Cmt>();

                string CmtUrl = BaseCmtUrl + GetParm();
                string htmldata = HttpHelper.GetHtml(CmtUrl, "application/json");
                QQJson jc = JsonConvert.DeserializeObject<QQJson>(htmldata);

                this.first = jc.data.last;
                var cmts = jc.data.commentid;
                this.hasnext = jc.data.hasnext;

                foreach (var cmt in cmts)
                {
                    if (cmt.parent == "0")
                    {
                        Common.Cmt qcmt = new Common.Cmt();
                        qcmt.name = cmt.userinfo.nick;
                        qcmt.date = GetTime(cmt.time.ToString()).ToString();       //这TM是时间戳
                        qcmt.location = cmt.userinfo.region;
                        qcmt.content = cmt.content;
                        qcmt.up = cmt.up;

                        lcmts.Add(qcmt);
                        QQCmts.Add(qcmt);
                    }
                }
                return lcmts;
            }
            return null;
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

            mw.SetProgressBar(0, total);

            var cmts = this.GetNextCmts();

            while (cmts != null)
            {
                foreach (var cmt in cmts)
                {
                    mw.AddPVB(cmt);
                    mw.AddProgressBar1();

                    //if (stop) break;
                }
                //Thread.Sleep(500);
                //if (stop) break;
                cmts = this.GetNextCmts();
            }
            foreach (var cmt in QQCmts)
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
