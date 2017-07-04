using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace dp2SIPClient
{
    public partial class MainForm : Form
    {
        private TcpClient _client;

        private NetworkStream _networkStream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;

        private Thread _recvThread;   // 接收信息线程
        //private Thread _sendThread;   // 发送信息线程
        private Thread _serverThread;// 服务线程

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void Connection()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(txtIP.Text);
                Int32 port = Int32.Parse(txtPort.Text);
                string hostName = Dns.GetHostEntry(ipAddress).HostName;
                _client = new TcpClient(hostName, port);
            }
            catch
            {
                MessageBox.Show("没有连接到服务器!");
                return;
            }

            this.SetText("客户端成功连接上服务器");

            SetCtrlState(false);

            _networkStream = _client.GetStream();
            _streamReader = new StreamReader(_networkStream);
            _streamWriter = new StreamWriter(_networkStream);

            // 创建接收信息线程，并启动
            _recvThread = new Thread(new ThreadStart(RecvData));
            _recvThread.Start();
        }

        // 接收数据
        private void RecvData()
        {
            string s = _streamReader.ReadLine();
            if (s == null)
                return;

            // 如果没接到服务器退出的消息，则继续接收信息
            while (s !=null && !s.Equals("exit"))
            {
                this.SetText("收到信息:" + s);
                s = _streamReader.ReadLine();
            }


            // 收到服务器退出的消息，
            this.SetText("服务器关闭");
            this.SetText("客户端关闭");

            // 设置按钮状态
            this.SetCtrlState(true);

            //释放资源
            ReleaseResouce();
        }

        // 释放资源
        private void ReleaseResouce()
        {
            _networkStream.Close();
            _streamReader.Close();
            _streamWriter.Close();

            //_sendThread.Abort();
            _recvThread.Abort();
            _serverThread.Abort();

            _client.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _serverThread = new Thread(new ThreadStart(Connection));
            _serverThread.Start();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            this.SetCtrlState(true);
            //this.btnConnect.Enabled = true;  // 按了停止之后，“连接”按钮可以用，“发送”不能用
            //this.btnDown.Enabled = false;
            //this.btnSend.Enabled = false;


            string exitMsg = "exit";  // 要退出时，发送 exit 信息给服务器
            _streamWriter.WriteLine(exitMsg);
            //刷新当前数据流中的数据
            _streamWriter.Flush();

            // 刷新界面
            this.SetText("客户端关闭");

            //释放资源
            ReleaseResouce();
        }
        delegate void SetTextCallback(string text);

        delegate void SetCtrlStateCallback(bool enabled);
        private void SetCtrlState(bool enabled)
        {
            if (this.btnConnect.InvokeRequired)
            {
                SetCtrlStateCallback d = new SetCtrlStateCallback(SetCtrlState);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                btnConnect.Enabled = enabled;
                btnDown.Enabled = !enabled;
                btnSend.Enabled = !enabled;
            }
        }


        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox_printer.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listBox_printer.Items.Add(text);
               // this.textBox1.Text = text;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //// 启动发送线程
            //_sendThread = new Thread(new ThreadStart(SendData));
            //_sendThread.Start();

            SendData();
        }

        // 发送数据
        private void SendData()
        {
            // 发送信息不允许为空
            if (txtMsg.Text == "")
            {
                MessageBox.Show("发送的信息不能为空!");
                txtMsg.Focus();
                return;
            }

            try
            {
                //往当前的数据流中写入一行字符串
                _streamWriter.WriteLine(txtMsg.Text);
                
                //刷新当前数据流中的数据
                _streamWriter.Flush();

                // 刷新界面
                this.SetText("发送信息:" + txtMsg.Text);
                txtMsg.Text = "";  // 清空
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Client提示");
            }
        }

        private void txtMsg_Enter(object sender, EventArgs e)
        {
            this.SendData();
        }

        private void txtMsg_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtMsg_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SendData();
            }
        }

    }
}
