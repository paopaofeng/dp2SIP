namespace dp2SIPClient
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
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空信息区ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.实用工具ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_send = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_info = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.tabControl_main = new System.Windows.Forms.TabControl();
            this.tabPage_Login93 = new System.Windows.Forms.TabPage();
            this.tabPage_SCStatus99 = new System.Windows.Forms.TabPage();
            this.tabPage_Checkout11 = new System.Windows.Forms.TabPage();
            this.tabPage_Checkin09 = new System.Windows.Forms.TabPage();
            this.tabPage_PatronInformation63 = new System.Windows.Forms.TabPage();
            this.tabPage_EndPatronSession35 = new System.Windows.Forms.TabPage();
            this.tabPage_ItemInformation17 = new System.Windows.Forms.TabPage();
            this.tabPage_Renew29 = new System.Windows.Forms.TabPage();
            this.label_UIDAlgorithm = new System.Windows.Forms.Label();
            this.textBox_Login_UIDAlgorithm = new System.Windows.Forms.TextBox();
            this.textBox_Login_PWDAlgorithm = new System.Windows.Forms.TextBox();
            this.label_PWDAlgorithm = new System.Windows.Forms.Label();
            this.textBox_Login_loginUserId_CN_r = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Login_loginPassword_CO_r = new System.Windows.Forms.TextBox();
            this.label_oginPassword_CO = new System.Windows.Forms.Label();
            this.textBox_Login_locationCode_CP_o = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.tabControl_main.SuspendLayout();
            this.tabPage_Login93.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMsg
            // 
            this.txtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMsg.BackColor = System.Drawing.Color.Silver;
            this.txtMsg.Location = new System.Drawing.Point(7, 0);
            this.txtMsg.Margin = new System.Windows.Forms.Padding(6);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(857, 35);
            this.txtMsg.TabIndex = 18;
            this.txtMsg.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMsg_KeyUp);
            // 
            // txtInfo
            // 
            this.txtInfo.BackColor = System.Drawing.SystemColors.Info;
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Location = new System.Drawing.Point(0, 0);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(678, 705);
            this.txtInfo.TabIndex = 21;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1551, 39);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.参数配置ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(74, 35);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 参数配置ToolStripMenuItem
            // 
            this.参数配置ToolStripMenuItem.Name = "参数配置ToolStripMenuItem";
            this.参数配置ToolStripMenuItem.Size = new System.Drawing.Size(209, 38);
            this.参数配置ToolStripMenuItem.Text = "参数配置";
            this.参数配置ToolStripMenuItem.Click += new System.EventHandler(this.参数配置ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空信息区ToolStripMenuItem,
            this.实用工具ToolStripMenuItem1});
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(74, 35);
            this.关于ToolStripMenuItem.Text = "帮助";
            // 
            // 清空信息区ToolStripMenuItem
            // 
            this.清空信息区ToolStripMenuItem.Name = "清空信息区ToolStripMenuItem";
            this.清空信息区ToolStripMenuItem.Size = new System.Drawing.Size(233, 38);
            this.清空信息区ToolStripMenuItem.Text = "清空信息区";
            this.清空信息区ToolStripMenuItem.Click += new System.EventHandler(this.清空信息区ToolStripMenuItem_Click);
            // 
            // 实用工具ToolStripMenuItem1
            // 
            this.实用工具ToolStripMenuItem1.Name = "实用工具ToolStripMenuItem1";
            this.实用工具ToolStripMenuItem1.Size = new System.Drawing.Size(233, 38);
            this.实用工具ToolStripMenuItem1.Text = "实用工具";
            this.实用工具ToolStripMenuItem1.Click += new System.EventHandler(this.实用工具ToolStripMenuItem1_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_send});
            this.toolStrip1.Location = new System.Drawing.Point(0, 39);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1551, 34);
            this.toolStrip1.TabIndex = 25;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel_send
            // 
            this.toolStripLabel_send.Enabled = false;
            this.toolStripLabel_send.Name = "toolStripLabel_send";
            this.toolStripLabel_send.Size = new System.Drawing.Size(69, 31);
            this.toolStripLabel_send.Text = "send";
            this.toolStripLabel_send.Click += new System.EventHandler(this.toolStripLabel_send_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_info});
            this.statusStrip1.Location = new System.Drawing.Point(0, 778);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1551, 36);
            this.statusStrip1.TabIndex = 26;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_info
            // 
            this.toolStripStatusLabel_info.Name = "toolStripStatusLabel_info";
            this.toolStripStatusLabel_info.Size = new System.Drawing.Size(257, 31);
            this.toolStripStatusLabel_info.Text = "toolStripStatusLabel1";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 73);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.tabControl_main);
            this.splitContainer_main.Panel1.Controls.Add(this.txtMsg);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.txtInfo);
            this.splitContainer_main.Size = new System.Drawing.Size(1551, 705);
            this.splitContainer_main.SplitterDistance = 869;
            this.splitContainer_main.TabIndex = 27;
            // 
            // tabControl_main
            // 
            this.tabControl_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_main.Controls.Add(this.tabPage_Login93);
            this.tabControl_main.Controls.Add(this.tabPage_SCStatus99);
            this.tabControl_main.Controls.Add(this.tabPage_Checkout11);
            this.tabControl_main.Controls.Add(this.tabPage_Checkin09);
            this.tabControl_main.Controls.Add(this.tabPage_PatronInformation63);
            this.tabControl_main.Controls.Add(this.tabPage_ItemInformation17);
            this.tabControl_main.Controls.Add(this.tabPage_Renew29);
            this.tabControl_main.Controls.Add(this.tabPage_EndPatronSession35);
            this.tabControl_main.Location = new System.Drawing.Point(7, 44);
            this.tabControl_main.Name = "tabControl_main";
            this.tabControl_main.SelectedIndex = 0;
            this.tabControl_main.Size = new System.Drawing.Size(859, 658);
            this.tabControl_main.TabIndex = 19;
            // 
            // tabPage_Login93
            // 
            this.tabPage_Login93.Controls.Add(this.textBox_Login_locationCode_CP_o);
            this.tabPage_Login93.Controls.Add(this.label2);
            this.tabPage_Login93.Controls.Add(this.textBox_Login_loginPassword_CO_r);
            this.tabPage_Login93.Controls.Add(this.label_oginPassword_CO);
            this.tabPage_Login93.Controls.Add(this.textBox_Login_loginUserId_CN_r);
            this.tabPage_Login93.Controls.Add(this.label1);
            this.tabPage_Login93.Controls.Add(this.textBox_Login_PWDAlgorithm);
            this.tabPage_Login93.Controls.Add(this.label_PWDAlgorithm);
            this.tabPage_Login93.Controls.Add(this.textBox_Login_UIDAlgorithm);
            this.tabPage_Login93.Controls.Add(this.label_UIDAlgorithm);
            this.tabPage_Login93.Location = new System.Drawing.Point(8, 39);
            this.tabPage_Login93.Name = "tabPage_Login93";
            this.tabPage_Login93.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Login93.Size = new System.Drawing.Size(843, 611);
            this.tabPage_Login93.TabIndex = 0;
            this.tabPage_Login93.Text = "Login93";
            this.tabPage_Login93.UseVisualStyleBackColor = true;
            // 
            // tabPage_SCStatus99
            // 
            this.tabPage_SCStatus99.Location = new System.Drawing.Point(8, 39);
            this.tabPage_SCStatus99.Name = "tabPage_SCStatus99";
            this.tabPage_SCStatus99.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_SCStatus99.Size = new System.Drawing.Size(772, 559);
            this.tabPage_SCStatus99.TabIndex = 1;
            this.tabPage_SCStatus99.Text = "SCStatus99";
            this.tabPage_SCStatus99.UseVisualStyleBackColor = true;
            // 
            // tabPage_Checkout11
            // 
            this.tabPage_Checkout11.Location = new System.Drawing.Point(8, 39);
            this.tabPage_Checkout11.Name = "tabPage_Checkout11";
            this.tabPage_Checkout11.Size = new System.Drawing.Size(772, 559);
            this.tabPage_Checkout11.TabIndex = 2;
            this.tabPage_Checkout11.Text = "Checkout11";
            this.tabPage_Checkout11.UseVisualStyleBackColor = true;
            // 
            // tabPage_Checkin09
            // 
            this.tabPage_Checkin09.Location = new System.Drawing.Point(8, 39);
            this.tabPage_Checkin09.Name = "tabPage_Checkin09";
            this.tabPage_Checkin09.Size = new System.Drawing.Size(772, 559);
            this.tabPage_Checkin09.TabIndex = 3;
            this.tabPage_Checkin09.Text = "Checkin09";
            this.tabPage_Checkin09.UseVisualStyleBackColor = true;
            // 
            // tabPage_PatronInformation63
            // 
            this.tabPage_PatronInformation63.Location = new System.Drawing.Point(8, 39);
            this.tabPage_PatronInformation63.Name = "tabPage_PatronInformation63";
            this.tabPage_PatronInformation63.Size = new System.Drawing.Size(772, 559);
            this.tabPage_PatronInformation63.TabIndex = 4;
            this.tabPage_PatronInformation63.Text = "PatronInformation63";
            this.tabPage_PatronInformation63.UseVisualStyleBackColor = true;
            // 
            // tabPage_EndPatronSession35
            // 
            this.tabPage_EndPatronSession35.Location = new System.Drawing.Point(8, 39);
            this.tabPage_EndPatronSession35.Name = "tabPage_EndPatronSession35";
            this.tabPage_EndPatronSession35.Size = new System.Drawing.Size(772, 559);
            this.tabPage_EndPatronSession35.TabIndex = 5;
            this.tabPage_EndPatronSession35.Text = "EndPatronSession35";
            this.tabPage_EndPatronSession35.UseVisualStyleBackColor = true;
            // 
            // tabPage_ItemInformation17
            // 
            this.tabPage_ItemInformation17.Location = new System.Drawing.Point(8, 39);
            this.tabPage_ItemInformation17.Name = "tabPage_ItemInformation17";
            this.tabPage_ItemInformation17.Size = new System.Drawing.Size(772, 559);
            this.tabPage_ItemInformation17.TabIndex = 6;
            this.tabPage_ItemInformation17.Text = "ItemInformation17";
            this.tabPage_ItemInformation17.UseVisualStyleBackColor = true;
            // 
            // tabPage_Renew29
            // 
            this.tabPage_Renew29.Location = new System.Drawing.Point(8, 39);
            this.tabPage_Renew29.Name = "tabPage_Renew29";
            this.tabPage_Renew29.Size = new System.Drawing.Size(772, 559);
            this.tabPage_Renew29.TabIndex = 7;
            this.tabPage_Renew29.Text = "Renew29";
            this.tabPage_Renew29.UseVisualStyleBackColor = true;
            // 
            // label_UIDAlgorithm
            // 
            this.label_UIDAlgorithm.AutoSize = true;
            this.label_UIDAlgorithm.Location = new System.Drawing.Point(6, 18);
            this.label_UIDAlgorithm.Name = "label_UIDAlgorithm";
            this.label_UIDAlgorithm.Size = new System.Drawing.Size(178, 24);
            this.label_UIDAlgorithm.TabIndex = 0;
            this.label_UIDAlgorithm.Text = "* UIDAlgorithm";
            // 
            // textBox_Login_UIDAlgorithm
            // 
            this.textBox_Login_UIDAlgorithm.Location = new System.Drawing.Point(238, 15);
            this.textBox_Login_UIDAlgorithm.Name = "textBox_Login_UIDAlgorithm";
            this.textBox_Login_UIDAlgorithm.Size = new System.Drawing.Size(84, 35);
            this.textBox_Login_UIDAlgorithm.TabIndex = 1;
            // 
            // textBox_Login_PWDAlgorithm
            // 
            this.textBox_Login_PWDAlgorithm.Location = new System.Drawing.Point(238, 68);
            this.textBox_Login_PWDAlgorithm.Name = "textBox_Login_PWDAlgorithm";
            this.textBox_Login_PWDAlgorithm.Size = new System.Drawing.Size(84, 35);
            this.textBox_Login_PWDAlgorithm.TabIndex = 3;
            // 
            // label_PWDAlgorithm
            // 
            this.label_PWDAlgorithm.AutoSize = true;
            this.label_PWDAlgorithm.Location = new System.Drawing.Point(6, 68);
            this.label_PWDAlgorithm.Name = "label_PWDAlgorithm";
            this.label_PWDAlgorithm.Size = new System.Drawing.Size(178, 24);
            this.label_PWDAlgorithm.TabIndex = 2;
            this.label_PWDAlgorithm.Text = "* PWDAlgorithm";
            // 
            // textBox_Login_loginUserId_CN_r
            // 
            this.textBox_Login_loginUserId_CN_r.Location = new System.Drawing.Point(238, 126);
            this.textBox_Login_loginUserId_CN_r.Name = "textBox_Login_loginUserId_CN_r";
            this.textBox_Login_loginUserId_CN_r.Size = new System.Drawing.Size(271, 35);
            this.textBox_Login_loginUserId_CN_r.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "* loginUserId_CN";
            // 
            // textBox_Login_loginPassword_CO_r
            // 
            this.textBox_Login_loginPassword_CO_r.Location = new System.Drawing.Point(238, 183);
            this.textBox_Login_loginPassword_CO_r.Name = "textBox_Login_loginPassword_CO_r";
            this.textBox_Login_loginPassword_CO_r.Size = new System.Drawing.Size(271, 35);
            this.textBox_Login_loginPassword_CO_r.TabIndex = 7;
            // 
            // label_oginPassword_CO
            // 
            this.label_oginPassword_CO.AutoSize = true;
            this.label_oginPassword_CO.Location = new System.Drawing.Point(6, 186);
            this.label_oginPassword_CO.Name = "label_oginPassword_CO";
            this.label_oginPassword_CO.Size = new System.Drawing.Size(226, 24);
            this.label_oginPassword_CO.TabIndex = 6;
            this.label_oginPassword_CO.Text = "* loginPassword_CO";
            // 
            // textBox_Login_locationCode_CP_o
            // 
            this.textBox_Login_locationCode_CP_o.Location = new System.Drawing.Point(238, 237);
            this.textBox_Login_locationCode_CP_o.Name = "textBox_Login_locationCode_CP_o";
            this.textBox_Login_locationCode_CP_o.Size = new System.Drawing.Size(271, 35);
            this.textBox_Login_locationCode_CP_o.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "  locationCode_CP";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1551, 814);
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "SIP2 Client";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel1.PerformLayout();
            this.splitContainer_main.Panel2.ResumeLayout(false);
            this.splitContainer_main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.tabControl_main.ResumeLayout(false);
            this.tabPage_Login93.ResumeLayout(false);
            this.tabPage_Login93.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 实用工具ToolStripMenuItem1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_send;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_info;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.ToolStripMenuItem 清空信息区ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl_main;
        private System.Windows.Forms.TabPage tabPage_Login93;
        private System.Windows.Forms.TabPage tabPage_SCStatus99;
        private System.Windows.Forms.TabPage tabPage_Checkout11;
        private System.Windows.Forms.TabPage tabPage_Checkin09;
        private System.Windows.Forms.TabPage tabPage_PatronInformation63;
        private System.Windows.Forms.TabPage tabPage_ItemInformation17;
        private System.Windows.Forms.TabPage tabPage_Renew29;
        private System.Windows.Forms.TabPage tabPage_EndPatronSession35;
        private System.Windows.Forms.TextBox textBox_Login_loginUserId_CN_r;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Login_PWDAlgorithm;
        private System.Windows.Forms.Label label_PWDAlgorithm;
        private System.Windows.Forms.TextBox textBox_Login_UIDAlgorithm;
        private System.Windows.Forms.Label label_UIDAlgorithm;
        private System.Windows.Forms.TextBox textBox_Login_loginPassword_CO_r;
        private System.Windows.Forms.Label label_oginPassword_CO;
        private System.Windows.Forms.TextBox textBox_Login_locationCode_CP_o;
        private System.Windows.Forms.Label label2;
    }
}

