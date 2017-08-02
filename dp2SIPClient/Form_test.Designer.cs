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
            this.button_createRightsTable = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_createTestEnv
            // 
            this.button_createTestEnv.Location = new System.Drawing.Point(12, 23);
            this.button_createTestEnv.Name = "button_createTestEnv";
            this.button_createTestEnv.Size = new System.Drawing.Size(265, 49);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 649);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(948, 38);
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
            // button_createRightsTable
            // 
            this.button_createRightsTable.Location = new System.Drawing.Point(12, 556);
            this.button_createRightsTable.Name = "button_createRightsTable";
            this.button_createRightsTable.Size = new System.Drawing.Size(190, 46);
            this.button_createRightsTable.TabIndex = 2;
            this.button_createRightsTable.Text = "创建流通权限";
            this.button_createRightsTable.UseVisualStyleBackColor = true;
            this.button_createRightsTable.Visible = false;
            this.button_createRightsTable.Click += new System.EventHandler(this.button_createRightsTable_Click);
            // 
            // Form_test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 687);
            this.Controls.Add(this.button_createRightsTable);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button_createTestEnv);
            this.Name = "Form_test";
            this.Text = "Form_test";
            this.Load += new System.EventHandler(this.Form_CreateTestEnv_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_createTestEnv;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button button_createRightsTable;

    }
}