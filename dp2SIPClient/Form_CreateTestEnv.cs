using DigitalPlatform;
using DigitalPlatform.CirculationClient;
using DigitalPlatform.LibraryClient;
using DigitalPlatform.SIP2;
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
        public LibraryChannel GetChannel()
        {
            LibraryChannel channel = this._channelPool.GetChannel(this.dp2ServerUrl,
                this.dp2Username);
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

            this._channelPool.BeforeLogin += (sender1, e1) =>
            {
                e1.LibraryServerUrl = this.dp2ServerUrl;
                e1.UserName = this.dp2Username;
                e1.Parameters = "type=worker,client=dp2SIPClient|0.01";
                e1.Password = this.dp2Password;
                e1.SavePasswordLong = true;
                
            };

        }

        private void Form_CreateTestEnv_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.dp2ServerUrl = this.dp2ServerUrl;
            Properties.Settings.Default.dp2Username = this.dp2Username;
            Properties.Settings.Default.dp2Password = this.dp2Password;
        }

        private void button_createPatronDb_Click(object sender, EventArgs e)
        {
            string strOutputInfo = "";
            string strError = "";

            LibraryChannel channel = this.GetChannel();
            try
            {


                string strBiblioDbName = "_test";

                // 创建一个书目库
                // parameters:
                // return:
                //      -1  出错
                //      0   没有必要创建，或者操作者放弃创建。原因在 strError 中
                //      1   成功创建
                long nRet = ManageHelper.CreateBiblioDatabase(
                    channel,
                    null,//this.Progress,
                    strBiblioDbName,
                    "book",
                    "unimarc",
                    out strError);
                if (nRet == -1)
                    goto ERROR1;



                MessageBox.Show(strOutputInfo);

            }
            finally
            {
                this.ReturnChannel(channel);
            }
            ERROR1:
            MessageBox.Show("出错：" + strError);
            return;
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            LibraryChannel channel = this.GetChannel();
            try
            {
                string strError="";
                    long lRet = channel.Login(this.dp2Username,
                    this.dp2Password,
                    "type=worker,client=dp2SIPClient|0.01",
                    out strError);
                    if (lRet == -1 || lRet == 0)
                    {
                        //SIPUtility.Logger
                        MessageBox.Show("登录失败：" + strError);
                        return;
                    }
                    else
                    {
                        //this._channelPool.BeforeLogin += (sender, e) =>
                        //{
                        //    e.LibraryServerUrl = Properties.Settings.Default.LibraryServerUrl;
                        //    e.UserName = strUserID;
                        //    e.Parameters = "type=worker,client=dp2SIPServer|0.01";
                        //    e.Password = strPassword;
                        //    e.SavePasswordLong = true;
                        //};

                        //this._username = strUserID;

                        //LogManager.Logger.Info("终端 " + strLocationCode + " : " + strUserID + " 接入");

                        MessageBox.Show("登录成功");
                    }
            }
            finally
            {
                this.ReturnChannel(channel);
            }
        }


    }
}
