using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Threading;
using System.Reflection;
using System.IO;

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
                "凤凰",
                "搜狐",
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

        public static string[] Columns = { "Name", "Date", "Location", "Up", "Content" };

        public string f = "完成";
        public string s = "正在抓取";

        delegate void dState(string s);
        public void State(string s)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new dState(State), f);
            } else
            {
                stateText.Text = s;
            }
        }

        private void InitPreViewBox()
        {
            PreViewBox.Columns.Clear();
            PreViewBox.Items.Clear();
            foreach (var str in Columns)
            {
                PreViewBox.Columns.Add(str);
            }
        }

        delegate void dSetPB(int min, int max);
        public void SetProgressBar(int min, int max)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new dSetPB(SetProgressBar), new object[] { min, max });
            }
            else
            {
                progressBar1.Minimum = min;
                progressBar1.Maximum = max;

                progressBar2.Minimum = min;
                progressBar2.Maximum = max;

                NumText.Text = 0 + "/" + max.ToString();
            }


        }

        delegate void dAddPB1();
        public void AddProgressBar1()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new dAddPB1(AddProgressBar1));
            } else
            {
                if (progressBar1.Value >= progressBar1.Maximum)
                { progressBar1.Maximum++; }
                progressBar1.Value++;
                NumText.Text = progressBar1.Value + "/" + progressBar1.Maximum;
            }
        }

        delegate void dAddPB2();
        public void AddProgressBar2()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new dAddPB2(AddProgressBar2));
            }
            else
            {
                if (progressBar2.Value >= progressBar2.Maximum)
                { progressBar2.Maximum++; }
                progressBar2.Value++;
            }
        }

        delegate void dAddPVB(Common.Cmt data);

        public void AddPVB(Common.Cmt data)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new dAddPVB(AddPVB), data);
            }
            else
            {
                //int i = 0;
                PreViewBox.BeginUpdate();
                ListViewItem lvi = new ListViewItem();
                lvi.Text = data.name;
                lvi.SubItems.Add(data.date);
                lvi.SubItems.Add(data.location);
                lvi.SubItems.Add(data.up);
                lvi.SubItems.Add(data.content);

                PreViewBox.Items.Add(lvi);
                PreViewBox.EndUpdate();
            }
        }
        /*
        public void Startifeng()
        {
            this.CatchLocker = true;
            try
            {
                ifeng fn = new ifeng(Url.Text);

                this.BeginInvoke(new dState(State), s);
                string URL = Url.Text;
                string file = FileName.Text;
                ExcelHelper eh = new ExcelHelper(file);
                int num = fn.GetNum();


                this.BeginInvoke(new dSetPB(SetProgressBar), new object[] { 0, num });
                var cmts = fn.GetNextCmts();

                while (cmts != null)
                {
                    foreach (var cmt in cmts)
                    {
                        this.BeginInvoke(new dAddPVB(AddPVB), cmt);
                        this.BeginInvoke(new dAddPB1(AddProgressBar1));
                    }
                    cmts = fn.GetNextCmts();
                }

                var allcmts = fn.GetAllCmts();
                foreach (var cmt in allcmts)
                {
                    eh.AddRow(cmt);
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
        }*/
        /*
        public void StartSoHu()
        {
            this.CatchLocker = true;
            try
            {
                SoHu sh = new SoHu(Url.Text, FileName.Text, this);
                sh.Run();
            }
            catch (Exception err)
            {
                MessageBox.Show("出错！错误原因：" + err.Message, "提示信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                CatchLocker = false;
            }
            this.CatchLocker = false;
        }*/
        /*
        public void StartSina()
        {
            CatchLocker = true;
            try
            {
                Sina sina = new Sina(Url.Text, FileName.Text, this);
                sina.Run();
            }
            catch (Exception err)
            {
                MessageBox.Show("出错！错误原因：" + err.Message, "提示信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                CatchLocker = false;
            }

            CatchLocker = false;

            //return true;
        }*/

        private void Start(object st)
        {
            CatchLocker = true;

            try
            {
                switch (st.ToString())
                {
                    case "网易":
                        var wy = new Common.CatcherWorker<WangYi.WangYi>(new WangYi.WangYi("Ss", "Ss", this));
                        wy.Run();
                        break;
                    case "搜狐":
                        var sh = new Common.CatcherWorker<SoHu>(new SoHu(Url.Text, FileName.Text, this));
                        sh.Run();
                        break;
                    case "新浪":
                        var sa = new Common.CatcherWorker<Sina>(new Sina(Url.Text, FileName.Text, this));
                        sa.Run();
                        break;
                    case "腾讯":
                        var qq = new Common.CatcherWorker<Sina>(new Sina(Url.Text, FileName.Text, this));
                        qq.Run();
                        break;
                    case "凤凰":
                        var fn = new Common.CatcherWorker<ifeng>(new ifeng(Url.Text, FileName.Text, this));
                        fn.Run();
                        break;
                    default:
                        MessageBox.Show("未知类型", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                }
            }
            catch (Exception err)
            {
                MessageBox.Show("出错！错误原因：" + err.Message, "提示信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                CatchLocker = false;
            }


            CatchLocker = false;
        }
        /*
        public void StartQQ()
        {
            this.CatchLocker = true;
            try
            {
                Tencent qq = new Tencent(Url.Text,FileName.Text,this);

                
                
            }
            catch (Exception err)
            {
                MessageBox.Show("出错！错误原因：" + err.Message, "提示信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                CatchLocker = false;
            }
            this.CatchLocker = false;
        }*/
        /*
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

                int num = wy.num;
                int round = wy.round;

                this.BeginInvoke(new dSetPB(SetProgressBar), new object[] { 0, num });
                for (int i = 0; i < round; ++i)
                {
                    var cmtlist = wy.GetNextCmts();

                    foreach (var cmt in cmtlist)
                    {
                        this.BeginInvoke(new dAddPVB(AddPVB), cmt);
                        this.BeginInvoke(new dAddPB1(AddProgressBar1));
                    }
                }

                var AllCmts = wy.GetAllCmts();

                foreach (var cmt in AllCmts)
                {
                    eh.AddRow(cmt);
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
        */
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
                            InitPreViewBox();

                            var site = sitetype.SelectedItem.ToString();
                            Thread t = new Thread(new ParameterizedThreadStart(Start));
                            t.Start(site);
                            /*
                            switch (site)
                            {
                                case "网易":
                                    Thread tWY = new Thread(StartWY);
                                    tWY.Start();
                                    break;
                                case "搜狐":
                                    Thread tSH = new Thread(StartSoHu);
                                    tSH.Start();
                                    break;
                                case "新浪":
                                    //Sina sa = new Sina("sss", "ss", this);
                                    Thread tSA = new Thread(new ParameterizedThreadStart(Start));
                                    tSA.Start(SiteType.Sina);
                                    break;
                                case "腾讯":
                                    Thread tQQ = new Thread(StartQQ);
                                    tQQ.Start();
                                    break;
                                case "凤凰":
                                    Thread tFN = new Thread(Startifeng);
                                    tFN.Start();
                                    break;
                                default:
                                    MessageBox.Show("未知类型", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    break;
                                    
                            }*/
                        }catch(Exception err)
                        {
                            MessageBox.Show("出现错误,错误信息:"+err.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //InitPreViewBox(SiteType.Sina);
                        State("GetHtml...");
                    }
                }
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
