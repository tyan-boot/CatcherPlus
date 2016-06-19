using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CatcherPlus
{
    public class SinaJson
    {
        public SinaResult result;
    }

    public class SinaResult
    {
        public SinaCount count;
        public List<SinaCmt> cmntlist;
    }
    public class SinaCmt
    {
        public string level;
        public string area;
        public string content;
        public string agree;    //zan
        public string time;
        public string config;   //contain name
        public string nick;     //number id?????
    }

    public class SinaCount
    {
        public int show;
    }

    public class SinaCmtSimple
    {
        public string name;
        public string date;
        public string area;
        public string content;
        public string agree;
    }

    class Sina
    {
        public Dictionary<string, string> SinaParm = new Dictionary<string, string>();

        private static Regex reChannel = new Regex("(?<=comment_channel\\:).*?(?=\\;)");
        private static Regex reId = new Regex("(?<=comment_id\\:).*?(?=\\\")");
        private static Regex reName = new Regex("(?<=wb_screen_name=)(.*?)(?=&)");

        private string Url;
        private string channel;
        private string id;
        private string BaseCmtUrl = "http://comment5.news.sina.com.cn/page/info?";
        private int num;
        private int page;

        //private List<SinaCmtSimple> SinaCmts = new List<SinaCmtSimple>();
        private List<Common.Cmt> SinaCmts = new List<Common.Cmt>();

        public Sina(string Url)
        {
            this.Url = Url;
            page = 1;
            Init();
        }

        private bool Init()
        {
            string htmldata = HttpHelper.GetHtml(Url, "application/text");
            if (htmldata != null) return false;

            Match mChannel = reChannel.Match(htmldata);
            Match mId = reId.Match(htmldata);

            if (mChannel.Success && mId.Success)
            {
                channel = mChannel.Groups[0].Value;
                id = mId.Groups[0].Value;
            }

            SinaParm.Add("format", "json");
            SinaParm.Add("channel", channel);
            SinaParm.Add("newsid", id);
            SinaParm.Add("ie", "utf-8");
            SinaParm.Add("oe", "gbk");
            SinaParm.Add("page", "1");
            SinaParm.Add("page_size", "1");

            //test
            string cmturl = BaseCmtUrl + HttpHelper.arry2urlencoded(SinaParm);
            //Console.WriteLine(cmturl);
            htmldata = HttpHelper.GetHtml(cmturl,"application/json");

            var jc = JsonConvert.DeserializeObject<SinaJson>(htmldata);

            //Console.WriteLine(jc.result.cmntlist.Count);
            this.num = jc.result.count.show;

            SinaParm["page_size"] = "200";
            //int i = 0;
            return true;
        }

        public List<Common.Cmt> GetAllCmts()
        {
            return SinaCmts;
        }

        public int GetNum()
        {
            return num;
        }
        
        public List<Common.Cmt> GetNextCmts()
        {
            SinaParm["page"] = this.page.ToString();
            string cmturl = BaseCmtUrl + HttpHelper.arry2urlencoded(SinaParm);
            string htmldata = HttpHelper.GetHtml(cmturl, "application/json");
            var jc = JsonConvert.DeserializeObject<SinaJson>(htmldata);

            //Console.WriteLine(jc.result.cmntlist.Count);
            //if (jc.result.cmntlist.Count == 0) return null;

            //List<SinaCmtSimple> lscs = new List<SinaCmtSimple>();
            //SinaCmtSimple sss = new SinaCmtSimple();
            //lscs.Add(sss);
            //page++;

            if (jc.result.cmntlist.Count != 0)
            {
                List<Common.Cmt> lscs = new List<Common.Cmt>();

                foreach (var cmt in jc.result.cmntlist)
                {
                    Common.Cmt scs = new Common.Cmt();
                    Match mName = reName.Match(cmt.config);
                    if (mName.Success)
                        scs.name = mName.Groups[1].Value;
                    else scs.name = cmt.nick;

                    scs.date = cmt.time;
                    scs.content = cmt.content;
                    scs.up = cmt.agree;
                    scs.location = cmt.area;
                    lscs.Add(scs);
                    SinaCmts.Add(scs);
                }
                page++;
                return lscs;
            }
            else return null;
            //return null;
        }

    }
}
