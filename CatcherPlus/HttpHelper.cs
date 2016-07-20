using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace CatcherPlus
{

    class HttpParm
    {
        private string PostField = "";
        private int num = 0;

        public void AddArg(string key, string content)
        {
            if (num != 0)
            {
                PostField += "&" + key + "=" + content;
            }
            else
            {
                PostField += key + "=" + content;
                num++;
            }
        }

        public string Get()
        {
            return PostField;
        }
    }

    class HttpHelper
    {
        public static string GetHtml(string Url, string ContentType)
        {
            string HtmlContent = "";

            try
            {
                HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(Url);

                wReq.AllowAutoRedirect = false;
                wReq.ContentType = ContentType;
                wReq.Method = "GET";

                HttpWebResponse wRes = (HttpWebResponse)wReq.GetResponse();

                StreamReader sr = new StreamReader(wRes.GetResponseStream());

                HtmlContent = sr.ReadToEnd();

                wRes.Close();
                sr.Close();
                return HtmlContent;

            }
            catch
            {
                //Console.WriteLine(err);
                throw;
            }

            //return null;
        }

        public static string PostTo(string Url, HttpParm pt)
        {
            try
            {
                HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(Url);
                wReq.Method = "POST";
                wReq.ContentType = "application/x-www-form-urlencoded";

                string PostData = pt.Get();
                byte[] PostDataByte = Encoding.UTF8.GetBytes(PostData);

                wReq.ContentLength = PostDataByte.Length;
                wReq.KeepAlive = true;
                wReq.AllowAutoRedirect = false;

                Stream stream = wReq.GetRequestStream();
                stream.Write(PostDataByte, 0, PostDataByte.Length);
                stream.Close();

                HttpWebResponse wRes;
                wRes = (HttpWebResponse)wReq.GetResponse();

                StreamReader sr = new StreamReader(wRes.GetResponseStream());
                string rtn = sr.ReadToEnd();
                return rtn;
            }
            catch
            {
                //Console.WriteLine(err.Message);
                throw;
                //return null;
            }

        }

        public static string arry2urlencoded(Dictionary<string, int> data)
        {
            string u = "";
            foreach (var d in data)
            {
                u = u + d.Key + "=" + d.Value + "&";
            }

            u = u.Substring(0, u.Length - 1);
            return u;
        }

        public static string arry2urlencoded(Dictionary<string, string> data)
        {
            string u = "";
            foreach (var d in data)
            {
                u = u + d.Key + "=" + d.Value + "&";
            }

            u = u.Substring(0, u.Length - 1);
            return u;
        }
    }
}
