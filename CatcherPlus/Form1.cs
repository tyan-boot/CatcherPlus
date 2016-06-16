using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Threading;

namespace CatcherPlus
{


    public partial class MainWin : Form
    {
        public MainWin()
        {
            InitializeComponent();
            Console.Write("Start");
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            var sitetypes = sitetype.Items;
            sitetypes.AddRange(new object[] {
                //"凤凰",
                //"搜狐",
                "腾讯",
                "网易",
                "新浪"
            });

            //sitetypes.
            sitetype.SelectedIndex = 0;

        }

        private bool CatchLocker = false;
        //private bool stop = false;

        private enum SiteType
        {
            WangYi = 0,
            Tencent = 1,
            Sina = 2,
            SoHu = 3,
            ifeng = 4
        };

        private string[] WYColumns = { "Name", "Date", "Location", "Vote", "Content" };
        private string[] QQColumns = { "Name", "Date", "Location", "Content", "Up" };
        private string[] SinaColumns = { "Name", "Date", "Area", "Content", "Agree" };

        public string f = "完成";
        public string s = "正在抓取";

        delegate void dState(string s);
        private void State(string s)
        {
            stateText.Text = s;
        }

        private void InitPreViewBox(SiteType site)
        {
            switch (site)
            {
                case SiteType.WangYi:
                    PreViewBox.Columns.Clear();
                    PreViewBox.Items.Clear();
                    foreach (var str in WYColumns)
                    {
                        PreViewBox.Columns.Add(str);
                    }
                    break;

                case SiteType.Tencent:
                    PreViewBox.Columns.Clear();
                    PreViewBox.Items.Clear();
                    foreach (var str in QQColumns)
                    {
                        PreViewBox.Columns.Add(str);
                    }
                    break;

                case SiteType.Sina:
                    PreViewBox.Columns.Clear();
                    PreViewBox.Items.Clear();
                    foreach (var str in SinaColumns)
                    {
                        PreViewBox.Columns.Add(str);
                    }
                    break;
            }
            
        }

        delegate void dSetPB(int min, int max);
        public void SetProgressBar(int min, int max)
        {
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;

            progressBar2.Minimum = min;
            progressBar2.Maximum = max;

            NumText.Text = 0 + "/" + max.ToString();

        }

        delegate void dAddPB1();
        public void AddProgressBar1()
        {
            if (progressBar1.Value >= progressBar1.Maximum)
            { progressBar1.Maximum++; }
            progressBar1.Value++;
            NumText.Text = progressBar1.Value + "/" + progressBar1.Maximum;
        }

        delegate void dAddPB2();
        public void AddProgressBar2()
        {
            if (progressBar2.Value >= progressBar2.Maximum)
            { progressBar2.Maximum++; }
            progressBar2.Value++;
        }

        delegate void dAddPVB(List<string> data);

        public void AddPVB(List<string> data)
        {
            int i = 0;
            PreViewBox.BeginUpdate();
            ListViewItem lvi = new ListViewItem();
            lvi.Text = data[0];
            
            foreach (var sd in data)
            {
                if (i == 0)
                {
                    ++i;
                    continue;
                }
                lvi.SubItems.Add(sd);
            }
            PreViewBox.Items.Add(lvi);
            PreViewBox.EndUpdate();
        }

        public void StartSina()
        {
            CatchLocker = true;
            try
            {
                Sina sina = new Sina(Url.Text);

                string URL = Url.Text;
                string file = FileName.Text;
                ExcelHelper eh = new ExcelHelper(file);
                List<string> title = new List<string>();
                foreach (var str in SinaColumns)
                {
                    title.Add(str);
                }

                eh.SetTitle(title);

                var num = sina.GetNum();
                this.BeginInvoke(new dSetPB(SetProgressBar), new object[] { 0, num });

                var cmts = sina.GetNextCmts();

                while (cmts != null)
                {
                    foreach (var cmt in cmts)
                    {
                        List<string> scmt = new List<string>();

                        scmt.Add(cmt.name);
                        scmt.Add(cmt.date);
                        scmt.Add(cmt.area);
                        scmt.Add(cmt.content);
                        scmt.Add(cmt.agree);
                        this.BeginInvoke(new dAddPVB(AddPVB), scmt);
                        this.BeginInvoke(new dAddPB1(AddProgressBar1));

                        //if (stop) break;
                    }
                    //Thread.Sleep(500);
                    //if (stop) break;
                    cmts = sina.GetNextCmts();
                    Thread.Sleep(500);
                }


                var allcmts = sina.GetAllCmts();
                foreach (var cmt in allcmts)
                {
                    List<string> scmt = new List<string>();

                    scmt.Add(cmt.name);
                    scmt.Add(cmt.date);
                    scmt.Add(cmt.area);
                    scmt.Add(cmt.content);
                    scmt.Add(cmt.agree);
                    eh.AddRow(scmt);
                    this.BeginInvoke(new dAddPB2(AddProgressBar2));
                }

                eh.Save();
                this.BeginInvoke(new dState(State), f);
                MessageBox.Show("抓取完成", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show("出错！错误原因：" + err.Message, "提示信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                CatchLocker = false;
            }
            
            CatchLocker = false;

            //return true;
        }

        public void StartQQ()
        {
            this.CatchLocker = true;
            try
            {
                Tencent qq = new Tencent(Url.Text);

                this.BeginInvoke(new dState(State), s);
                string URL = Url.Text;
                string file = FileName.Text;
                ExcelHelper eh = new ExcelHelper(file);
                List<string> title = new List<string>();
                foreach (var str in QQColumns)
                {
                    title.Add(str);
                }

                eh.SetTitle(title);

                int num = qq.GetNum();
                this.BeginInvoke(new dSetPB(SetProgressBar), new object[] { 0, num });
                var cmts = qq.GetNextCmts();

                while (cmts != null)
                {
                    foreach (var cmt in cmts)
                    {
                        List<string> scmt = new List<string>();

                        scmt.Add(cmt.name);
                        scmt.Add(cmt.date);
                        scmt.Add(cmt.location);
                        scmt.Add(cmt.content);
                        scmt.Add(cmt.up);
                        this.BeginInvoke(new dAddPVB(AddPVB), scmt);
                        this.BeginInvoke(new dAddPB1(AddProgressBar1));

                        //if (stop) break;
                    }
                    //Thread.Sleep(500);
                    //if (stop) break;
                    cmts = qq.GetNextCmts();
                }

                var allcmts = qq.GetAllCmts();
                foreach (var cmt in allcmts)
                {
                    List<string> scmt = new List<string>();

                    scmt.Add(cmt.name);
                    scmt.Add(cmt.date);
                    scmt.Add(cmt.location);
                    scmt.Add(cmt.content);
                    scmt.Add(cmt.up);
                    eh.AddRow(scmt);
                    this.BeginInvoke(new dAddPB2(AddProgressBar2));
                }
                eh.Save();
                this.BeginInvoke(new dState(State), f);
                MessageBox.Show("抓取完成", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception err)
            {
                MessageBox.Show("出错！错误原因：" + err.Message, "提示信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                CatchLocker = false;
            }
            this.CatchLocker = false;
        }

        public void StartWY()
        {
            this.CatchLocker = true;
            try
            {
                this.BeginInvoke(new dState(State), s);
                string URL = Url.Text;
                string file = FileName.Text;
                WangYi.WangYi wy = new WangYi.WangYi(URL);
                ExcelHelper eh = new ExcelHelper(file);
                List<string> title = new List<string>();
                //Set Title
                foreach (var str in WYColumns)
                {
                    title.Add(str);
                }

                eh.SetTitle(title);

                int num = wy.num;
                int round = wy.round;

                this.BeginInvoke(new dSetPB(SetProgressBar), new object[] { 0, num });
                for (int i = 0; i < round; ++i)
                {
                    var cmtlist = wy.GetNextCmtList();

                    foreach (var cmt in cmtlist)
                    {
                        List<string> scmt = new List<string>();

                        scmt.Add(cmt.nickname);
                        scmt.Add(cmt.createTime);
                        scmt.Add(cmt.location);
                        scmt.Add(cmt.vote.ToString());
                        scmt.Add(cmt.content);
                        this.BeginInvoke(new dAddPVB(AddPVB), scmt);
                        this.BeginInvoke(new dAddPB1(AddProgressBar1));
                    }
                }

                var AllCmts = wy.GetAllData();

                foreach (var cmt in AllCmts)
                {
                    List<string> scmt = new List<string>();

                    scmt.Add(cmt.nickname);
                    scmt.Add(cmt.createTime);
                    scmt.Add(cmt.location);
                    scmt.Add(cmt.vote.ToString());
                    scmt.Add(cmt.content);
                    //Console.WriteLine(cmt.nickname);
                    eh.AddRow(scmt);
                    this.BeginInvoke(new dAddPB2(AddProgressBar2));
                }

                eh.Save();

                this.BeginInvoke(new dState(State), f);
                MessageBox.Show("抓取完成", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception err)
            {
                MessageBox.Show("出错！错误原因：" + err.Message, "提示信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.CatchLocker = false;
            }
            this.CatchLocker = false;
        }
        private void clearUrl_Click(object sender, EventArgs e)
        {
            Url.Text = "";
        }

        private void clearFileName_Click(object sender, EventArgs e)
        {
            FileName.Text = "";
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (!this.CatchLocker)
            {
                if (Url.Text == "")
                {
                    MessageBox.Show("请输入地址", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else
                {
                    if (FileName.Text == "")
                    {
                        MessageBox.Show("请设置文件名", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        progressBar1.Value = 0;
                        progressBar2.Value = 0;
                        try
                        {
                            var site = sitetype.SelectedItem.ToString();
                            switch (site)
                            {
                                case "网易":
                                    InitPreViewBox(SiteType.WangYi);
                                    Thread tWY = new Thread(StartWY);
                                    tWY.Start();
                                    break;
                                case "搜狐":
                                    InitPreViewBox(SiteType.SoHu);
                                    Thread tSH = new Thread(StartSina);
                                    break;
                                case "新浪":
                                    InitPreViewBox(SiteType.Sina);
                                    Thread tSA = new Thread(StartSina);
                                    tSA.Start();
                                    break;
                                case "腾讯":
                                    InitPreViewBox(SiteType.Tencent);
                                    Thread tQQ = new Thread(StartQQ);
                                    tQQ.Start();
                                    break;
                                case "凤凰":
                                    InitPreViewBox(SiteType.ifeng);
                                    Thread tFN = new Thread(StartSina);
                                    break;
                                default:
                                    MessageBox.Show("未知类型", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    break;

                            }
                        }catch(Exception err)
                        {
                            MessageBox.Show("出现错误,错误信息:"+err.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //InitPreViewBox(SiteType.Sina);
                        State("GetHtml...");
                    }
                }
                //State("完成");
            }else
            {
                MessageBox.Show("当前正在执行任务,请稍后再试", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
            //Console.WriteLine(jc.comments);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.stop = true;
        }
    }
}
