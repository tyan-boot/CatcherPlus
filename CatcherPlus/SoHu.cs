using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CatcherPlus
{
    class SHJson1
    {
        public ListData listData;

    }
    class SHJson2
    {

        public List<SHCmt> comments;
    }
    class ListData
    {
        public int cmt_num;
        public long topic_id;
    }

    class SHCmt
    {
        public string content;
        public long create_time;
        public string ip_location;

        public UserInfo passport;
        public int support_count;
    }
    class UserInfo
    {
        public string nickname;
    }

    class SoHu
    {
        private static Regex reEId = new Regex("(?<=entityId\\s=\\s)\\d+");
        private static Regex reIDs = new Regex("(pinglun)/(.*)/(.*)\"");
        private static Regex reQuan = new Regex("(?<=href=\").+?(?=\">)");

        private string Url;
        private string EId;
        private string appid;
        private string topicsid;
        private int num;
        private long topic_id;
        private int page = 1;
        private int round = 0;
        private int currendRound = 0;

        private Dictionary<string, string> SHParm = new Dictionary<string, string>();
        private Dictionary<string, string> CmtParm = new Dictionary<string, string>();
        private List<Common.Cmt> cmts = new List<Common.Cmt>();

        public SoHu(string Url)
        {
            this.Url = Url;
            SHParm.Add("appid","");
            SHParm.Add("client_id", "");
            SHParm.Add("topicurl", "");
            SHParm.Add("topicsid", "");

            CmtParm.Add("client_id", "");
            CmtParm.Add("topic_id", "");
            CmtParm.Add("page_size", "20");
            CmtParm.Add("page_no", "1");

            Init();
        }

        private void Init()
        {
            string htmldata = HttpHelper.GetHtml(this.Url, "application/text");
            Match mEId = reEId.Match(htmldata);
            if(mEId.Success)
            {
                EId = mEId.Groups[0].Value; 
            }

            string QUrl = "http://pinglun.sohu.com/s" + EId + ".html";
            htmldata = HttpHelper.GetHtml(QUrl,"application/text");
            Match mIDs = reIDs.Match(htmldata);
            Match mQuan = reQuan.Match(htmldata);
            
            appid = mIDs.Groups[2].Value;
            topicsid = mIDs.Groups[3].Value;
            
            SHParm["appid"] = appid;
            SHParm["client_id"] = appid;
            SHParm["topicurl"] = mQuan.Groups[0].Value;
            SHParm["topicsid"] = topicsid;
            
            string parm = HttpHelper.arry2urlencoded(SHParm);
            string CmtUrl1 = "http://changyan.sohu.com/node/html?" + parm;

            htmldata = HttpHelper.GetHtml(CmtUrl1,"application/json");
            var jc = JsonConvert.DeserializeObject<SHJson1>(htmldata);

            num = jc.listData.cmt_num;

            round = num / 20;
            round++;

            topic_id = jc.listData.topic_id;
            CmtParm["client_id"] = appid;
            CmtParm["topic_id"] = topic_id.ToString();

        }

        public List<Common.Cmt> GetNextCmts()
        {
            if (currendRound >= round) return null;

            List<Common.Cmt> SHCmts = new List<Common.Cmt>();

            CmtParm["page_no"] = page.ToString();
            string CmtUrl2 = "http://changyan.sohu.com/api/2/topic/comments?" + HttpHelper.arry2urlencoded(CmtParm);

            string htmldata = HttpHelper.GetHtml(CmtUrl2,"application/json");
            var jc = JsonConvert.DeserializeObject<SHJson2>(htmldata);

            foreach (var cmt in jc.comments)
            {
                Common.Cmt shcmt = new Common.Cmt();
                shcmt.name = cmt.passport.nickname;
                shcmt.date = GetTime(cmt.create_time.ToString()).ToString();
                shcmt.up = cmt.support_count.ToString();
                shcmt.location = cmt.ip_location;
                shcmt.content = cmt.content;

                SHCmts.Add(shcmt);
                cmts.Add(shcmt);
            }
            currendRound++;
            return SHCmts;
        }

        public List<Common.Cmt> GetAllCmts()
        {
            return cmts;

        }

        public int GetNum()
        {
            return num;

        }

        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
