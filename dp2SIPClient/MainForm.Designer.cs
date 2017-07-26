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
            this.label3 = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.btnCheckSum1 = new System.Windows.Forms.Button();
            this.btnCheckSum2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(15, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 24);
            this.label3.TabIndex = 20;
            this.label3.Text = "发送消息：";
            // 
            // txtMsg
            // 
            this.txtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMsg.Location = new System.Drawing.Point(169, 72);
            this.txtMsg.Margin = new System.Windows.Forms.Padding(6);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(938, 88);
            this.txtMsg.TabIndex = 18;
            this.txtMsg.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMsg_KeyUp);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(1119, 72);
            this.btnSend.Margin = new System.Windows.Forms.Padding(6);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(76, 54);
            this.btnSend.TabIndex = 17;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(891, 19);
            this.btnDown.Margin = new System.Windows.Forms.Padding(6);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(116, 41);
            this.btnDown.TabIndex = 16;
            this.btnDown.Text = "停止";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(767, 17);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(6);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(112, 43);
            this.btnConnect.TabIndex = 15;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(465, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "端口：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 24);
            this.label1.TabIndex = 13;
            this.label1.Text = "服务器地址：";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(559, 15);
            this.txtPort.Margin = new System.Windows.Forms.Padding(6);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(196, 35);
            this.txtPort.TabIndex = 12;
            this.txtPort.Text = "2000";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(181, 17);
            this.txtIP.Margin = new System.Windows.Forms.Padding(6);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(272, 35);
            this.txtIP.TabIndex = 11;
            this.txtIP.Text = "127.0.0.1";
            // 
            // txtInfo
            // 
            this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInfo.BackColor = System.Drawing.SystemColors.Info;
            this.txtInfo.Location = new System.Drawing.Point(12, 169);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(1183, 598);
            this.txtInfo.TabIndex = 21;
            // 
            // btnCheckSum1
            // 
            this.btnCheckSum1.Location = new System.Drawing.Point(19, 114);
            this.btnCheckSum1.Margin = new System.Windows.Forms.Padding(6);
            this.btnCheckSum1.Name = "btnCheckSum1";
            this.btnCheckSum1.Size = new System.Drawing.Size(62, 46);
            this.btnCheckSum1.TabIndex = 22;
            this.btnCheckSum1.Text = "校1";
            this.btnCheckSum1.UseVisualStyleBackColor = true;
            this.btnCheckSum1.Click += new System.EventHandler(this.btnCheckSum1_Click);
            // 
            // btnCheckSum2
            // 
            this.btnCheckSum2.Location = new System.Drawing.Point(85, 114);
            this.btnCheckSum2.Margin = new System.Windows.Forms.Padding(6);
            this.btnCheckSum2.Name = "btnCheckSum2";
            this.btnCheckSum2.Size = new System.Drawing.Size(72, 46);
            this.btnCheckSum2.TabIndex = 23;
            this.btnCheckSum2.Text = "校2";
            this.btnCheckSum2.UseVisualStyleBackColor = true;
            this.btnCheckSum2.Click += new System.EventHandler(this.btnCheckSum2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 783);
            this.Controls.Add(this.btnCheckSum2);
            this.Controls.Add(this.btnCheckSum1);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Name = "MainForm";
            this.Text = "SIP2 Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Button btnCheckSum1;
        private System.Windows.Forms.Button btnCheckSum2;
    }
}

