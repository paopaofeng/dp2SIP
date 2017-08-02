namespace dp2SIPClient
{
    partial class Form_CreateTestEnv
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
            this.button_createPatronDb = new System.Windows.Forms.Button();
            this.button_createPatronRecord = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_createEntity = new System.Windows.Forms.Button();
            this.button_createBiblioDb = new System.Windows.Forms.Button();
            this.button_importBiblio = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_dp2password = new System.Windows.Forms.TextBox();
            this.textBox_dp2username = new System.Windows.Forms.TextBox();
            this.textBox_dp2serverUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_login = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_createPatronDb
            // 
            this.button_createPatronDb.Location = new System.Drawing.Point(20, 34);
            this.button_createPatronDb.Name = "button_createPatronDb";
            this.button_createPatronDb.Size = new System.Drawing.Size(190, 46);
            this.button_createPatronDb.TabIndex = 0;
            this.button_createPatronDb.Text = "创建读者库";
            this.button_createPatronDb.UseVisualStyleBackColor = true;
            this.button_createPatronDb.Click += new System.EventHandler(this.button_createPatronDb_Click);
            // 
            // button_createPatronRecord
            // 
            this.button_createPatronRecord.Location = new System.Drawing.Point(247, 34);
            this.button_createPatronRecord.Name = "button_createPatronRecord";
            this.button_createPatronRecord.Size = new System.Drawing.Size(190, 46);
            this.button_createPatronRecord.TabIndex = 1;
            this.button_createPatronRecord.Text = "生成读者记录";
            this.button_createPatronRecord.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_createPatronDb);
            this.groupBox1.Controls.Add(this.button_createPatronRecord);
            this.groupBox1.Location = new System.Drawing.Point(12, 340);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(669, 96);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_createEntity);
            this.groupBox2.Controls.Add(this.button_createBiblioDb);
            this.groupBox2.Controls.Add(this.button_importBiblio);
            this.groupBox2.Location = new System.Drawing.Point(12, 447);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(669, 122);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // button_createEntity
            // 
            this.button_createEntity.Location = new System.Drawing.Point(473, 34);
            this.button_createEntity.Name = "button_createEntity";
            this.button_createEntity.Size = new System.Drawing.Size(190, 46);
            this.button_createEntity.TabIndex = 2;
            this.button_createEntity.Text = "创建册记录";
            this.button_createEntity.UseVisualStyleBackColor = true;
            // 
            // button_createBiblioDb
            // 
            this.button_createBiblioDb.Location = new System.Drawing.Point(20, 34);
            this.button_createBiblioDb.Name = "button_createBiblioDb";
            this.button_createBiblioDb.Size = new System.Drawing.Size(190, 46);
            this.button_createBiblioDb.TabIndex = 0;
            this.button_createBiblioDb.Text = "创建书目库";
            this.button_createBiblioDb.UseVisualStyleBackColor = true;
            // 
            // button_importBiblio
            // 
            this.button_importBiblio.Location = new System.Drawing.Point(247, 34);
            this.button_importBiblio.Name = "button_importBiblio";
            this.button_importBiblio.Size = new System.Drawing.Size(190, 46);
            this.button_importBiblio.TabIndex = 1;
            this.button_importBiblio.Text = "导入书目记录";
            this.button_importBiblio.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Location = new System.Drawing.Point(12, 237);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(669, 97);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(190, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "创建馆藏地";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(247, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 46);
            this.button2.TabIndex = 1;
            this.button2.Text = "创建流通权限";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button_login);
            this.groupBox4.Controls.Add(this.textBox_dp2password);
            this.groupBox4.Controls.Add(this.textBox_dp2username);
            this.groupBox4.Controls.Add(this.textBox_dp2serverUrl);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(12, 15);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox4.Size = new System.Drawing.Size(802, 205);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "图书馆应用服务器配置";
            // 
            // textBox_dp2password
            // 
            this.textBox_dp2password.Location = new System.Drawing.Point(178, 150);
            this.textBox_dp2password.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_dp2password.Name = "textBox_dp2password";
            this.textBox_dp2password.PasswordChar = '*';
            this.textBox_dp2password.Size = new System.Drawing.Size(298, 35);
            this.textBox_dp2password.TabIndex = 5;
            // 
            // textBox_dp2username
            // 
            this.textBox_dp2username.Location = new System.Drawing.Point(178, 96);
            this.textBox_dp2username.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_dp2username.Name = "textBox_dp2username";
            this.textBox_dp2username.Size = new System.Drawing.Size(298, 35);
            this.textBox_dp2username.TabIndex = 4;
            // 
            // textBox_dp2serverUrl
            // 
            this.textBox_dp2serverUrl.Location = new System.Drawing.Point(178, 42);
            this.textBox_dp2serverUrl.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_dp2serverUrl.Name = "textBox_dp2serverUrl";
            this.textBox_dp2serverUrl.Size = new System.Drawing.Size(608, 35);
            this.textBox_dp2serverUrl.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 156);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "密  码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器地址：";
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(485, 139);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(135, 46);
            this.button_login.TabIndex = 6;
            this.button_login.Text = "登录";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // Form_CreateTestEnv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 687);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_CreateTestEnv";
            this.Text = "Form_CreateTestEnv";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CreateTestEnv_FormClosing);
            this.Load += new System.EventHandler(this.Form_CreateTestEnv_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_createPatronDb;
        private System.Windows.Forms.Button button_createPatronRecord;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_createEntity;
        private System.Windows.Forms.Button button_createBiblioDb;
        private System.Windows.Forms.Button button_importBiblio;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox_dp2password;
        private System.Windows.Forms.TextBox textBox_dp2username;
        private System.Windows.Forms.TextBox textBox_dp2serverUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_login;
    }
}