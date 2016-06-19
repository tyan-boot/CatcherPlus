using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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

    public class QQCmtSimple
    {
        public string name;
        public string date;
        public string location;
        public string content;
        public string up;
    }

    public class Tencent
    {
        private static Regex reNewID = new Regex("(?<=coral.qq.com\\/)\\w+");

        private string Url;
        private string NewID;
        private int total;
        private bool hasnext;

        private string BaseCmtUrl;
        private Dictionary<string, int> QQParm = new Dictionary<string, int>();
        //private List<QQCmtSimple> QQCmts = new List<QQCmtSimple>();
        private List<Common.Cmt> QQCmts = new List<Common.Cmt>();

        private int reqnum = 50;
        //private string commentid;
        private string first;
        //private string last;

        public Tencent(string Url)
        {
            this.Url = Url;

            QQParm.Add("commentid", 0);
            QQParm.Add("reqnum", 0);

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

            QQParm["reqnum"] = 1;
            string CmtUrl = BaseCmtUrl + HttpHelper.arry2urlencoded(QQParm);
            //Console.WriteLine(CmtUrl);

            string htmldata = HttpHelper.GetHtml(CmtUrl, "application/json");
            if (htmldata == null) return false;
            QQJson jc = JsonConvert.DeserializeObject<QQJson>(htmldata);

            this.total = jc.data.total;
            this.hasnext = jc.data.hasnext;
            this.first = jc.data.first;

            //Console.WriteLine(htmldata);
            return true;
        }
        public List<Common.Cmt> GetAllCmts()
        {
            return QQCmts;
        }

        public int GetNum()
        {
            return total;
        }

        private string GetParm()
        {
            string parm = "commentid=" + this.first + "&reqnum=" + this.reqnum.ToString();
            return parm;
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
    }
}
