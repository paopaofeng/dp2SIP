namespace dp2analysis
{
    partial class Form_Main
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dp2服务器配置SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_patron = new System.Windows.Forms.TextBox();
            this.button_analysis = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1028, 39);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dp2服务器配置SToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(103, 35);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // dp2服务器配置SToolStripMenuItem
            // 
            this.dp2服务器配置SToolStripMenuItem.Name = "dp2服务器配置SToolStripMenuItem";
            this.dp2服务器配置SToolStripMenuItem.Size = new System.Drawing.Size(306, 38);
            this.dp2服务器配置SToolStripMenuItem.Text = "dp2服务器配置(&S)";
            this.dp2服务器配置SToolStripMenuItem.Click += new System.EventHandler(this.dp2服务器配置SToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "证条码号";
            // 
            // textBox_patron
            // 
            this.textBox_patron.Location = new System.Drawing.Point(125, 59);
            this.textBox_patron.Name = "textBox_patron";
            this.textBox_patron.Size = new System.Drawing.Size(398, 35);
            this.textBox_patron.TabIndex = 2;
            // 
            // button_analysis
            // 
            this.button_analysis.Location = new System.Drawing.Point(547, 59);
            this.button_analysis.Name = "button_analysis";
            this.button_analysis.Size = new System.Drawing.Size(111, 46);
            this.button_analysis.TabIndex = 3;
            this.button_analysis.Text = "分析";
            this.button_analysis.UseVisualStyleBackColor = true;
            this.button_analysis.Click += new System.EventHandler(this.button_analysis_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(17, 126);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(999, 527);
            this.webBrowser1.TabIndex = 4;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 680);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button_analysis);
            this.Controls.Add(this.textBox_patron);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dp2服务器配置SToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_patron;
        private System.Windows.Forms.Button button_analysis;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}

