namespace CatcherPlus
{
    partial class MainWin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.Url = new System.Windows.Forms.TextBox();
            this.clearUrl = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.Setting = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clearFileName = new System.Windows.Forms.Button();
            this.FileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PreViewBox = new System.Windows.Forms.ListView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.NumText = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.stateText = new System.Windows.Forms.Label();
            this.sitetype = new System.Windows.Forms.ComboBox();
            this.Setting.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(8, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "地址";
            // 
            // Url
            // 
            this.Url.Location = new System.Drawing.Point(54, 35);
            this.Url.Name = "Url";
            this.Url.Size = new System.Drawing.Size(412, 21);
            this.Url.TabIndex = 1;
            // 
            // clearUrl
            // 
            this.clearUrl.Font = new System.Drawing.Font("宋体", 9F);
            this.clearUrl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.clearUrl.Location = new System.Drawing.Point(558, 32);
            this.clearUrl.Name = "clearUrl";
            this.clearUrl.Size = new System.Drawing.Size(75, 29);
            this.clearUrl.TabIndex = 2;
            this.clearUrl.Text = "清除";
            this.clearUrl.UseVisualStyleBackColor = true;
            this.clearUrl.Click += new System.EventHandler(this.clearUrl_Click);
            // 
            // start
            // 
            this.start.Font = new System.Drawing.Font("宋体", 9F);
            this.start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.start.Location = new System.Drawing.Point(639, 32);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 29);
            this.start.TabIndex = 3;
            this.start.Text = "开始";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // Setting
            // 
            this.Setting.Controls.Add(this.sitetype);
            this.Setting.Controls.Add(this.button1);
            this.Setting.Controls.Add(this.clearFileName);
            this.Setting.Controls.Add(this.FileName);
            this.Setting.Controls.Add(this.label2);
            this.Setting.Controls.Add(this.start);
            this.Setting.Controls.Add(this.clearUrl);
            this.Setting.Controls.Add(this.Url);
            this.Setting.Controls.Add(this.label1);
            this.Setting.Font = new System.Drawing.Font("宋体", 9F);
            this.Setting.Location = new System.Drawing.Point(12, 12);
            this.Setting.Name = "Setting";
            this.Setting.Size = new System.Drawing.Size(731, 132);
            this.Setting.TabIndex = 0;
            this.Setting.TabStop = false;
            this.Setting.Text = "相关设置";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("宋体", 9F);
            this.button1.Location = new System.Drawing.Point(639, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 7;
            this.button1.Text = "停止";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clearFileName
            // 
            this.clearFileName.Font = new System.Drawing.Font("宋体", 9F);
            this.clearFileName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.clearFileName.Location = new System.Drawing.Point(558, 78);
            this.clearFileName.Name = "clearFileName";
            this.clearFileName.Size = new System.Drawing.Size(75, 29);
            this.clearFileName.TabIndex = 6;
            this.clearFileName.Text = "清除";
            this.clearFileName.UseVisualStyleBackColor = true;
            this.clearFileName.Click += new System.EventHandler(this.clearFileName_Click);
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(102, 79);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(450, 21);
            this.FileName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(8, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "保存文件名";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PreViewBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(731, 387);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预览";
            // 
            // PreViewBox
            // 
            this.PreViewBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreViewBox.Location = new System.Drawing.Point(3, 17);
            this.PreViewBox.Name = "PreViewBox";
            this.PreViewBox.Size = new System.Drawing.Size(725, 367);
            this.PreViewBox.TabIndex = 0;
            this.PreViewBox.UseCompatibleStateImageBehavior = false;
            this.PreViewBox.View = System.Windows.Forms.View.Details;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 55);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(719, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // NumText
            // 
            this.NumText.AutoSize = true;
            this.NumText.Location = new System.Drawing.Point(657, 29);
            this.NumText.Name = "NumText";
            this.NumText.Size = new System.Drawing.Size(23, 12);
            this.NumText.TabIndex = 3;
            this.NumText.Text = "0/0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.progressBar2);
            this.groupBox2.Controls.Add(this.stateText);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.NumText);
            this.groupBox2.Location = new System.Drawing.Point(12, 543);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(731, 144);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "写入文件:";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(6, 115);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(719, 23);
            this.progressBar2.TabIndex = 5;
            // 
            // stateText
            // 
            this.stateText.AutoSize = true;
            this.stateText.Location = new System.Drawing.Point(9, 29);
            this.stateText.Name = "stateText";
            this.stateText.Size = new System.Drawing.Size(29, 12);
            this.stateText.TabIndex = 4;
            this.stateText.Text = "空闲";
            // 
            // sitetype
            // 
            this.sitetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sitetype.FormattingEnabled = true;
            this.sitetype.Location = new System.Drawing.Point(472, 36);
            this.sitetype.Name = "sitetype";
            this.sitetype.Size = new System.Drawing.Size(80, 20);
            this.sitetype.Sorted = true;
            this.sitetype.TabIndex = 8;
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 699);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Setting);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWin";
            this.Text = "CatcherPlus";
            this.Setting.ResumeLayout(false);
            this.Setting.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Url;
        private System.Windows.Forms.Button clearUrl;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.GroupBox Setting;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button clearFileName;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label NumText;
        private System.Windows.Forms.ListView PreViewBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label stateText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox sitetype;
    }
}

