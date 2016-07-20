using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatcherPlus;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WangYi
{
    public class WangYiData
    {
        public IList<string> commentIds;

        public Dictionary<long, WYComment> comments;
        public int newListSize;
    }

    public class WYUser
    {
        public string avatar;
        public string id;
        public string location;
        public string nickname;
        public long userID;
    }
    public class WYComment
    {
        public string against;
        public bool anonymous;
        public int buildLevel;
        public long commentId;
        public string content;
        public string createTime;
        public int favCount;
        public string ip;
        public bool isDel;
        public string postId;
        public string productKey;
        public int shareCount;
        public string siteName;
        public string source;
        public bool unionState { get; set; }
        public WYUser user;
        public int vote;
    }

    public class WYCommentSimple
    {
        public string against;
        public bool anonymous;
        public int buildLevel;
        public string content;
        public string createTime;
        public int favCount;

        public string nickname;
        public string location;

        public int vote;
    }
    public class WangYi
    {
        private static Regex reproductKey = new Regex("\"productKey\"\\s*:\\s*\"([0-9a-fA-F]{32})\"");
        private static Regex redocId = new Regex("\"docId\"\\s*:\\s*\"([0-9a-zA-Z]*?)\"");

        private string file;
        private string productKey;
        private string docId;
        //评论总数
        public int num { get; set; } = 0;
        //当前已获取到的评论总数
        private int currentGet = 0;
        //每次获取数量
        private int limit = 30;

        //private int currentPoint = 0;
        //总共需要抓取次数
        public int round { get; set; } = 0;
        private string NewsUrl;

        private List<Common.Cmt> WYCmts = new List<Common.Cmt>();
        private MainWin mw;
        private ExcelHelper eh;

        public WangYi(string Url, string file, MainWin mw)
        {
            this.NewsUrl = Url;
            this.file = file;
            this.mw = mw;
            Init();
        }


        private string GetCmtUrl()
        {
            return "http://comment.news.163.com/api/v1/products/" + productKey + "/threads/" + docId + "/comments/newList?offset=0&limit=1";
        }
        private string GetNextCmtUrl()
        {
            string u = "http://comment.news.163.com/api/v1/products/" + productKey + "/threads/" + docId + "/comments/newList?offset=" + currentGet + "&limit=" + limit;
            currentGet += limit;
            return u;
        }

        private bool Init()
        {
            //Get Total num
            string NewsPageData = HttpHelper.GetHtml(this.NewsUrl, "application/text");
            if (NewsPageData == null) return false;
            Match mpk = reproductKey.Match(NewsPageData);
            Match mdd = redocId.Match(NewsPageData);

            if (mpk.Success)
            {
                productKey = mpk.Groups[1].Value;
            }
            else return false;

            if (mdd.Success)
            {
                docId = mdd.Groups[1].Value;
            }
            else return false;

            string cmtUrl = GetCmtUrl();
            string tmpData = HttpHelper.GetHtml(cmtUrl, "application/json");

            WangYiData jc = JsonConvert.DeserializeObject<WangYiData>(tmpData);
            this.num = jc.newListSize;

            int round = this.num / limit;
            if (round == 0)
            {
                this.round = 1;
            }
            else this.round = round + 1;

            return true;
        }

        public List<Common.Cmt> GetNextCmts()
        {
            List<Common.Cmt> lwys = new List<Common.Cmt>();
            string Url = GetNextCmtUrl();

            string RawJsonData = HttpHelper.GetHtml(Url, "application/json");
            var JsonData = JsonConvert.DeserializeObject<WangYiData>(RawJsonData);
            var cmts = JsonData.comments;

            foreach (var cmt in cmts)
            {
                if (cmt.Value.buildLevel == 1)
                {
                    Common.Cmt wys = new Common.Cmt();
                    //wys.up = cmt.Value.against;
                    //wys.against = cmt.Value.against;
                    //wys.anonymous = cmt.Value.anonymous;
                    //wys.buildLevel = cmt.Value.buildLevel;
                    wys.content = cmt.Value.content;

                    wys.date = cmt.Value.createTime;

                    wys.location = cmt.Value.user.location;
                    wys.name = cmt.Value.user.nickname;
                    wys.up = cmt.Value.vote.ToString();

                    lwys.Add(wys);
                    WYCmts.Add(wys);
                }
            }

            return lwys;
        }

        public void Run()
        {
            eh = new ExcelHelper(file);

            mw.SetProgressBar(0, num);

            for (int i = 0; i < round; ++i)
            {
                var cmtlist = this.GetNextCmts();

                foreach (var cmt in cmtlist)
                {
                    mw.AddPVB(cmt);
                    mw.AddProgressBar1();
                }
            }

            foreach (var cmt in WYCmts)
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