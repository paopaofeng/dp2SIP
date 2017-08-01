using DigitalPlatform;
using DigitalPlatform.LibraryClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dp2SIPClient
{
    public partial class Form_CreateTestEnv : Form
    {

        public Form_CreateTestEnv()
        {
            InitializeComponent();
        }

        private LibraryChannelPool _channelPool = new LibraryChannelPool();

        List<LibraryChannel> _channelList = new List<LibraryChannel>();

        // parameters:
        //      style    风格。如果为 GUI，表示会自动添加 Idle 事件，并在其中执行 Application.DoEvents
        public LibraryChannel GetChannel(string strUsername = "")
        {
            if (strUsername == "")
                strUsername = this.dp2Username;

            LibraryChannel channel = this._channelPool.GetChannel(this.dp2ServerUrl,
                strUsername);
            channel.Idle += channel_Idle;
            _channelList.Add(channel);
            // TODO: 检查数组是否溢出
            return channel;
        }

        void channel_Idle(object sender, IdleEventArgs e)
        {
            Application.DoEvents();
        }

        public void ReturnChannel(LibraryChannel channel)
        {
            channel.Idle -= channel_Idle;

            this._channelPool.ReturnChannel(channel);
            _channelList.Remove(channel);
        }

        public string dp2ServerUrl
        {
            get
            {
                return this.textBox_dp2serverUrl.Text;
            }

            set
            {
                this.textBox_dp2serverUrl.Text = value;
            }
        }

        public string dp2Username
        {
            get
            {
                return this.textBox_dp2username.Text;
            }

            set
            {
                this.textBox_dp2username.Text = value;
            }
        }

        public string dp2Password
        {
            get
            {
                return this.textBox_dp2password.Text;
            }

            set
            {
                this.textBox_dp2password.Text = value;
            }
        }

        private void Form_CreateTestEnv_Load(object sender, EventArgs e)
        {
            this.dp2ServerUrl = Properties.Settings.Default.dp2ServerUrl;
            this.dp2Username = Properties.Settings.Default.dp2Username;
            this.dp2Password = Properties.Settings.Default.dp2Password;

        }

        private void Form_CreateTestEnv_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.dp2ServerUrl = this.dp2ServerUrl;
            Properties.Settings.Default.dp2Username = this.dp2Username;
            Properties.Settings.Default.dp2Password = this.dp2Password;
        }

        private void button_createPatronDb_Click(object sender, EventArgs e)
        {

        }
    }
}
