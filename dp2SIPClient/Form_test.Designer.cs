namespace dp2SIPClient
{
    partial class Form_test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_createTestEnv = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.btnCheckinCheckout = new System.Windows.Forms.Button();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.更多ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.创建流通权限ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_login = new System.Windows.Forms.Button();
            this.button_SCStatus = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_checkout = new System.Windows.Forms.Button();
            this.button_checkin = new System.Windows.Forms.Button();
            this.button_checkoutin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_checkin_dup = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button_checkout_dup = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button_checkoutin_customer = new System.Windows.Forms.Button();
            this.textBox_checkinout_patronNum = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_checkinout_itemNum = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_checkinout_inNum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_checkinout_outNum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button_renew = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_17_patronNum = new System.Windows.Forms.TextBox();
            this.button_patronInfo = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_63_itemNum = new System.Windows.Forms.TextBox();
            this.button_itemInfo = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_createTestEnv
            // 
            this.button_createTestEnv.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_createTestEnv.Location = new System.Drawing.Point(20, 12);
            this.button_createTestEnv.Name = "button_createTestEnv";
            this.button_createTestEnv.Size = new System.Drawing.Size(250, 50);
            this.button_createTestEnv.TabIndex = 0;
            this.button_createTestEnv.Text = "一键初始化测试环境";
            this.button_createTestEnv.UseVisualStyleBackColor = true;
            this.button_createTestEnv.Click += new System.EventHandler(this.button_createTestEnv_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 901);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1404, 38);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(257, 33);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 32);
            // 
            // btnCheckinCheckout
            // 
            this.btnCheckinCheckout.Location = new System.Drawing.Point(403, 756);
            this.btnCheckinCheckout.Name = "btnCheckinCheckout";
            this.btnCheckinCheckout.Size = new System.Drawing.Size(179, 39);
            this.btnCheckinCheckout.TabIndex = 3;
            this.btnCheckinCheckout.Text = "自动测试借还";
            this.btnCheckinCheckout.UseVisualStyleBackColor = true;
            this.btnCheckinCheckout.Visible = false;
            this.btnCheckinCheckout.Click += new System.EventHandler(this.btnCheckinCheckout_Click);
            // 
            // txtInfo
            // 
            this.txtInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.HideSelection = false;
            this.txtInfo.Location = new System.Drawing.Point(0, 0);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(778, 862);
            this.txtInfo.TabIndex = 22;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 39);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox_63_itemNum);
            this.splitContainer1.Panel1.Controls.Add(this.button_itemInfo);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_17_patronNum);
            this.splitContainer1.Panel1.Controls.Add(this.button_patronInfo);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.button_SCStatus);
            this.splitContainer1.Panel1.Controls.Add(this.button_login);
            this.splitContainer1.Panel1.Controls.Add(this.button_createTestEnv);
            this.splitContainer1.Panel1.Controls.Add(this.btnCheckinCheckout);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtInfo);
            this.splitContainer1.Size = new System.Drawing.Size(1404, 862);
            this.splitContainer1.SplitterDistance = 622;
            this.splitContainer1.TabIndex = 23;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.更多ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1404, 39);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 更多ToolStripMenuItem
            // 
            this.更多ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.创建流通权限ToolStripMenuItem});
            this.更多ToolStripMenuItem.Name = "更多ToolStripMenuItem";
            this.更多ToolStripMenuItem.Size = new System.Drawing.Size(74, 35);
            this.更多ToolStripMenuItem.Text = "更多";
            // 
            // 创建流通权限ToolStripMenuItem
            // 
            this.创建流通权限ToolStripMenuItem.Name = "创建流通权限ToolStripMenuItem";
            this.创建流通权限ToolStripMenuItem.Size = new System.Drawing.Size(269, 38);
            this.创建流通权限ToolStripMenuItem.Text = "创建流通权限";
            this.创建流通权限ToolStripMenuItem.Click += new System.EventHandler(this.创建流通权限ToolStripMenuItem_Click);
            // 
            // button_login
            // 
            this.button_login.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_login.Location = new System.Drawing.Point(20, 91);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(268, 43);
            this.button_login.TabIndex = 4;
            this.button_login.Text = " 第一步 登录 93/94";
            this.button_login.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // button_SCStatus
            // 
            this.button_SCStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_SCStatus.Location = new System.Drawing.Point(20, 146);
            this.button_SCStatus.Name = "button_SCStatus";
            this.button_SCStatus.Size = new System.Drawing.Size(268, 50);
            this.button_SCStatus.TabIndex = 5;
            this.button_SCStatus.Text = " 第二步 SC状态 99/98";
            this.button_SCStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_SCStatus.UseVisualStyleBackColor = true;
            this.button_SCStatus.Click += new System.EventHandler(this.button_SCStatus_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_renew);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.textBox_checkinout_inNum);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox_checkinout_outNum);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox_checkinout_itemNum);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_checkinout_patronNum);
            this.groupBox1.Controls.Add(this.button_checkoutin_customer);
            this.groupBox1.Controls.Add(this.button_checkout_dup);
            this.groupBox1.Controls.Add(this.button_checkin_dup);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button_checkoutin);
            this.groupBox1.Controls.Add(this.button_checkin);
            this.groupBox1.Controls.Add(this.button_checkout);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(19, 212);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(563, 470);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // button_checkout
            // 
            this.button_checkout.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_checkout.Location = new System.Drawing.Point(15, 34);
            this.button_checkout.Name = "button_checkout";
            this.button_checkout.Size = new System.Drawing.Size(104, 45);
            this.button_checkout.TabIndex = 7;
            this.button_checkout.Text = "借";
            this.button_checkout.UseVisualStyleBackColor = true;
            this.button_checkout.Click += new System.EventHandler(this.button_checkout_Click);
            // 
            // button_checkin
            // 
            this.button_checkin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_checkin.Location = new System.Drawing.Point(15, 91);
            this.button_checkin.Name = "button_checkin";
            this.button_checkin.Size = new System.Drawing.Size(104, 45);
            this.button_checkin.TabIndex = 8;
            this.button_checkin.Text = "还";
            this.button_checkin.UseVisualStyleBackColor = true;
            this.button_checkin.Click += new System.EventHandler(this.button_checkin_Click);
            // 
            // button_checkoutin
            // 
            this.button_checkoutin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_checkoutin.Location = new System.Drawing.Point(15, 148);
            this.button_checkoutin.Name = "button_checkoutin";
            this.button_checkoutin.Size = new System.Drawing.Size(104, 45);
            this.button_checkoutin.TabIndex = 9;
            this.button_checkoutin.Text = "借还";
            this.button_checkoutin.UseVisualStyleBackColor = true;
            this.button_checkoutin.Click += new System.EventHandler(this.button_checkoutin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "（10人*2册*1借*1还）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(109, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 26);
            this.label2.TabIndex = 11;
            this.label2.Text = "（50册*1还）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "（10人*5册*1借）";
            // 
            // button_checkin_dup
            // 
            this.button_checkin_dup.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_checkin_dup.Location = new System.Drawing.Point(15, 205);
            this.button_checkin_dup.Name = "button_checkin_dup";
            this.button_checkin_dup.Size = new System.Drawing.Size(104, 47);
            this.button_checkin_dup.TabIndex = 13;
            this.button_checkin_dup.Text = "重复还";
            this.button_checkin_dup.UseVisualStyleBackColor = true;
            this.button_checkin_dup.Click += new System.EventHandler(this.button_checkin_dup_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(109, 214);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 24);
            this.label4.TabIndex = 14;
            this.label4.Text = "（10册*3还）";
            // 
            // button_checkout_dup
            // 
            this.button_checkout_dup.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_checkout_dup.Location = new System.Drawing.Point(15, 265);
            this.button_checkout_dup.Name = "button_checkout_dup";
            this.button_checkout_dup.Size = new System.Drawing.Size(104, 47);
            this.button_checkout_dup.TabIndex = 15;
            this.button_checkout_dup.Text = "重复借";
            this.button_checkout_dup.UseVisualStyleBackColor = true;
            this.button_checkout_dup.Click += new System.EventHandler(this.button_checkout_dup_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(110, 277);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "（10人*2册*3借）";
            // 
            // button_checkoutin_customer
            // 
            this.button_checkoutin_customer.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_checkoutin_customer.Location = new System.Drawing.Point(15, 324);
            this.button_checkoutin_customer.Name = "button_checkoutin_customer";
            this.button_checkoutin_customer.Size = new System.Drawing.Size(152, 51);
            this.button_checkoutin_customer.TabIndex = 17;
            this.button_checkoutin_customer.Text = "自定义借还";
            this.button_checkoutin_customer.UseVisualStyleBackColor = true;
            this.button_checkoutin_customer.Click += new System.EventHandler(this.button_checkoutin_customer_Click);
            // 
            // textBox_checkinout_patronNum
            // 
            this.textBox_checkinout_patronNum.Location = new System.Drawing.Point(177, 332);
            this.textBox_checkinout_patronNum.Name = "textBox_checkinout_patronNum";
            this.textBox_checkinout_patronNum.Size = new System.Drawing.Size(37, 35);
            this.textBox_checkinout_patronNum.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(213, 340);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 24);
            this.label6.TabIndex = 18;
            this.label6.Text = "人*";
            // 
            // textBox_checkinout_itemNum
            // 
            this.textBox_checkinout_itemNum.Location = new System.Drawing.Point(261, 332);
            this.textBox_checkinout_itemNum.Name = "textBox_checkinout_itemNum";
            this.textBox_checkinout_itemNum.Size = new System.Drawing.Size(37, 35);
            this.textBox_checkinout_itemNum.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(300, 340);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 24);
            this.label7.TabIndex = 20;
            this.label7.Text = "册*";
            // 
            // textBox_checkinout_inNum
            // 
            this.textBox_checkinout_inNum.Location = new System.Drawing.Point(434, 332);
            this.textBox_checkinout_inNum.Name = "textBox_checkinout_inNum";
            this.textBox_checkinout_inNum.Size = new System.Drawing.Size(37, 35);
            this.textBox_checkinout_inNum.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(473, 340);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 24);
            this.label8.TabIndex = 24;
            this.label8.Text = "还";
            // 
            // textBox_checkinout_outNum
            // 
            this.textBox_checkinout_outNum.Location = new System.Drawing.Point(351, 332);
            this.textBox_checkinout_outNum.Name = "textBox_checkinout_outNum";
            this.textBox_checkinout_outNum.Size = new System.Drawing.Size(37, 35);
            this.textBox_checkinout_outNum.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(390, 340);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 24);
            this.label9.TabIndex = 22;
            this.label9.Text = "借*";
            // 
            // button_renew
            // 
            this.button_renew.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_renew.Location = new System.Drawing.Point(15, 406);
            this.button_renew.Name = "button_renew";
            this.button_renew.Size = new System.Drawing.Size(104, 47);
            this.button_renew.TabIndex = 25;
            this.button_renew.Text = "续借";
            this.button_renew.UseVisualStyleBackColor = true;
            this.button_renew.Click += new System.EventHandler(this.button_renew_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(110, 418);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(202, 24);
            this.label10.TabIndex = 26;
            this.label10.Text = "（10人*2册*3借）";
            // 
            // textBox_17_patronNum
            // 
            this.textBox_17_patronNum.Location = new System.Drawing.Point(303, 707);
            this.textBox_17_patronNum.Name = "textBox_17_patronNum";
            this.textBox_17_patronNum.Size = new System.Drawing.Size(37, 35);
            this.textBox_17_patronNum.TabIndex = 19;
            this.textBox_17_patronNum.Text = "10";
            // 
            // button_patronInfo
            // 
            this.button_patronInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_patronInfo.Location = new System.Drawing.Point(19, 700);
            this.button_patronInfo.Name = "button_patronInfo";
            this.button_patronInfo.Size = new System.Drawing.Size(269, 56);
            this.button_patronInfo.TabIndex = 20;
            this.button_patronInfo.Text = " 获取读者信息 63/64";
            this.button_patronInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_patronInfo.UseVisualStyleBackColor = true;
            this.button_patronInfo.Click += new System.EventHandler(this.button_patronInfo_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(345, 720);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 24);
            this.label11.TabIndex = 21;
            this.label11.Text = "人";
            // 
            // textBox_63_itemNum
            // 
            this.textBox_63_itemNum.Location = new System.Drawing.Point(303, 781);
            this.textBox_63_itemNum.Name = "textBox_63_itemNum";
            this.textBox_63_itemNum.Size = new System.Drawing.Size(37, 35);
            this.textBox_63_itemNum.TabIndex = 22;
            this.textBox_63_itemNum.Text = "10";
            // 
            // button_itemInfo
            // 
            this.button_itemInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_itemInfo.Location = new System.Drawing.Point(19, 775);
            this.button_itemInfo.Name = "button_itemInfo";
            this.button_itemInfo.Size = new System.Drawing.Size(269, 54);
            this.button_itemInfo.TabIndex = 23;
            this.button_itemInfo.Text = " 获取册信息 17/18";
            this.button_itemInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_itemInfo.UseVisualStyleBackColor = true;
            this.button_itemInfo.Click += new System.EventHandler(this.button_itemInfo_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(345, 794);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 24);
            this.label12.TabIndex = 24;
            this.label12.Text = "册";
            // 
            // Form_test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1404, 939);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_test";
            this.Text = "Form_test";
            this.Load += new System.EventHandler(this.Form_CreateTestEnv_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_createTestEnv;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button btnCheckinCheckout;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 更多ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 创建流通权限ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_checkin_dup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_checkoutin;
        private System.Windows.Forms.Button button_checkin;
        private System.Windows.Forms.Button button_checkout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_SCStatus;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Button button_checkout_dup;
        private System.Windows.Forms.TextBox textBox_checkinout_inNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_checkinout_outNum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_checkinout_itemNum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_checkinout_patronNum;
        private System.Windows.Forms.Button button_checkoutin_customer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_renew;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_63_itemNum;
        private System.Windows.Forms.Button button_itemInfo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_17_patronNum;
        private System.Windows.Forms.Button button_patronInfo;
        private System.Windows.Forms.Label label11;

    }
}