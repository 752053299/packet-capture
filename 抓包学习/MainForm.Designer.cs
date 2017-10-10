namespace 抓包学习
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.portNameBox = new System.Windows.Forms.ToolStripComboBox();
            this.StartButton = new System.Windows.Forms.ToolStripButton();
            this.portStatePicLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.clearButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.portStateLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.LabelReceiveNum = new System.Windows.Forms.ToolStripLabel();
            this.LabelDataNum = new System.Windows.Forms.ToolStripLabel();
            this.LabelOrderNum = new System.Windows.Forms.ToolStripLabel();
            this.LabelResponseNum = new System.Windows.Forms.ToolStripLabel();
            this.LabelWarningNum = new System.Windows.Forms.ToolStripLabel();
            this.LabelUnknowNum = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.portNameBox,
            this.StartButton,
            this.portStatePicLabel,
            this.toolStripSeparator4,
            this.clearButton,
            this.toolStripLabel2,
            this.toolStripTextBox1,
            this.portStateLabel,
            this.toolStripLabel3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1387, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(69, 28);
            this.toolStripLabel1.Text = "串口连接";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // portNameBox
            // 
            this.portNameBox.BackColor = System.Drawing.SystemColors.Window;
            this.portNameBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portNameBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.portNameBox.Name = "portNameBox";
            this.portNameBox.Size = new System.Drawing.Size(99, 31);
            this.portNameBox.DropDown += new System.EventHandler(this.portNameBox_DropDown);
            // 
            // StartButton
            // 
            this.StartButton.AutoSize = false;
            this.StartButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.StartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.StartButton.Image = ((System.Drawing.Image)(resources.GetObject("StartButton.Image")));
            this.StartButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StartButton.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(69, 28);
            this.StartButton.Text = "开始";
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // portStatePicLabel
            // 
            this.portStatePicLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.portStatePicLabel.Image = global::抓包学习.Properties.Resources.off;
            this.portStatePicLabel.Margin = new System.Windows.Forms.Padding(10, 1, 10, 0);
            this.portStatePicLabel.Name = "portStatePicLabel";
            this.portStatePicLabel.Size = new System.Drawing.Size(16, 30);
            this.portStatePicLabel.Text = "toolStripLabel4";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // clearButton
            // 
            this.clearButton.AutoSize = false;
            this.clearButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearButton.Image = ((System.Drawing.Image)(resources.GetObject("clearButton.Image")));
            this.clearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(69, 28);
            this.clearButton.Text = "清空显示";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(69, 28);
            this.toolStripLabel2.Text = "筛选节点";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(69, 31);
            // 
            // portStateLabel
            // 
            this.portStateLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.portStateLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.portStateLabel.Name = "portStateLabel";
            this.portStateLabel.Size = new System.Drawing.Size(69, 28);
            this.portStateLabel.Text = "串口状态";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(0, 28);
            this.toolStripLabel3.Text = "toolStripLabel3";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 31);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1387, 535);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelReceiveNum,
            this.LabelDataNum,
            this.LabelOrderNum,
            this.LabelResponseNum,
            this.LabelWarningNum,
            this.LabelUnknowNum});
            this.toolStrip2.Location = new System.Drawing.Point(0, 541);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1387, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // LabelReceiveNum
            // 
            this.LabelReceiveNum.Name = "LabelReceiveNum";
            this.LabelReceiveNum.Size = new System.Drawing.Size(93, 22);
            this.LabelReceiveNum.Text = "收到的帧：0";
            // 
            // LabelDataNum
            // 
            this.LabelDataNum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.LabelDataNum.Name = "LabelDataNum";
            this.LabelDataNum.Size = new System.Drawing.Size(78, 22);
            this.LabelDataNum.Text = "数据帧：0";
            // 
            // LabelOrderNum
            // 
            this.LabelOrderNum.ForeColor = System.Drawing.Color.SteelBlue;
            this.LabelOrderNum.Name = "LabelOrderNum";
            this.LabelOrderNum.Size = new System.Drawing.Size(78, 22);
            this.LabelOrderNum.Text = "命令帧：0";
            // 
            // LabelResponseNum
            // 
            this.LabelResponseNum.ForeColor = System.Drawing.Color.Fuchsia;
            this.LabelResponseNum.Name = "LabelResponseNum";
            this.LabelResponseNum.Size = new System.Drawing.Size(78, 22);
            this.LabelResponseNum.Text = "应答帧：0";
            // 
            // LabelWarningNum
            // 
            this.LabelWarningNum.ForeColor = System.Drawing.Color.Red;
            this.LabelWarningNum.Name = "LabelWarningNum";
            this.LabelWarningNum.Size = new System.Drawing.Size(78, 22);
            this.LabelWarningNum.Text = "告警帧：0";
            // 
            // LabelUnknowNum
            // 
            this.LabelUnknowNum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.LabelUnknowNum.Name = "LabelUnknowNum";
            this.LabelUnknowNum.Size = new System.Drawing.Size(78, 22);
            this.LabelUnknowNum.Text = "未知帧：0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1387, 566);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "抓包学习";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox portNameBox;
        private System.Windows.Forms.ToolStripButton StartButton;
        private System.Windows.Forms.ToolStripButton clearButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ToolStripLabel portStateLabel;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel portStatePicLabel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel LabelReceiveNum;
        private System.Windows.Forms.ToolStripLabel LabelDataNum;
        private System.Windows.Forms.ToolStripLabel LabelOrderNum;
        private System.Windows.Forms.ToolStripLabel LabelResponseNum;
        private System.Windows.Forms.ToolStripLabel LabelWarningNum;
        private System.Windows.Forms.ToolStripLabel LabelUnknowNum;

        public System.IO.Ports.SerialPort SerialPort1 { get { return serialPort1;} }
    }
}

