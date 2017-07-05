using DigitalPlatform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        public MainForm()
        {
            InitializeComponent();
        }

        #region 连接 SIP2 Server

        // 连接
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //_serverThread = new Thread(new ThreadStart(Connection));
            //_serverThread.Start();
            this.Connection();
        }

        // 停止
        private void btnDown_Click(object sender, EventArgs e)
        {
            this.SetCtrlState(true);

            //todo
            string exitMsg = "exit";  // 要退出时，发送 exit 信息给服务器
            this.SendData(exitMsg);

            // 刷新界面
            this.PrinteInfo("客户端关闭");

            //释放资源
            this.CloseSocket();
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

            // 保存到一个变量上，方便使用
            _networkStream = _client.GetStream();

            //界面设置
            this.PrinteInfo("客户端成功连接上服务器");
            SetCtrlState(false);
        }

        // 关闭连接
        public void CloseSocket()
        {
            if (_client != null)
            {
                try
                {
                    this._networkStream.Close();
                }
                catch { }

                try
                {
                    this._client.Close();
                }
                catch { }

                this._client = null;
            }
        }

        #endregion


        #region 发送接收

        // 按钮 触发 发送
        private void btnSend_Click(object sender, EventArgs e)
        {
            this.sendCmd();
        }

        // 回车 触发 发送
        private void txtMsg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.sendCmd();
            }
        }

        // 发送包装小函数
        private void sendCmd()
        {
            // 发送信息不允许为空
            if (txtMsg.Text == "")
            {
                MessageBox.Show("发送的信息不能为空!");
                txtMsg.Focus();
                return;
            }

            string text = this.txtMsg.Text;
            SendData(text);
        }


        // 发送数据
        private void SendData(string text)
        {
            try
            {
                byte[] baPackage = this.Encoding.GetBytes(text);
                if (this._networkStream.DataAvailable == true)
                {
                    MessageBox.Show("发送前发现流中有未读的数据!");
                    //this.RecvData();
                    return;

                }

                this._networkStream.Write(baPackage, 0, baPackage.Length);
                this._networkStream.Flush();//刷新当前数据流中的数据

                // 刷新界面
                this.PrinteInfo("send:" + txtMsg.Text);
                txtMsg.Text = "";  // 清空

                // 调接收数据
                string strBack = "";
                string strError = "";
                int nRet = RecvTcpPackage(out strBack, out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(strError);
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Client提示");
            }
        }


        // 接收数据
        public int RecvTcpPackage(out string strPackage,
            out string strError)
        {
            strError = "";
            strPackage = "";

            Debug.Assert(this._client != null, "client为空");

            int offset = 0; //偏移量
            int wRet = 0;


            byte[] baPackage = new byte[1024];
            int nOneLength = 1024; //COMM_BUFF_LEN;

            while (offset < nOneLength)
            {
                if (this._client == null)
                {
                    strError = "通讯中断";
                    goto ERROR1;
                }

                try
                {
                    wRet = this._networkStream.Read(baPackage,
                        offset,
                        baPackage.Length - offset);
                }
                catch (SocketException ex)
                {
                    // ??这个什么错误码
                    if (ex.ErrorCode == 10035)
                    {
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }

                    // bool bRet = this.client.Connected;

                    strError = "[ERROR] Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }
                catch (Exception ex)
                {
                    //bool bRet = this.client.Connected;

                    strError = "[ERROR] Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }

                if (wRet == 0)
                {
                    return 0;
                    //strError = "Closed by remote peer";
                    //goto ERROR1;
                }

                // 得到包的长度
                if (wRet >= 1 || offset >= 1)
                {
                    //没有找到结束符，继续读
                    int nRet = Array.IndexOf(baPackage, (byte)this.Terminator);
                    if (nRet != -1)
                    {
                        // nLen = nInLen + wRet;
                        nOneLength = nRet;
                        break;
                    }

                    if (this._networkStream.DataAvailable == false) //流中没有数据了
                    {
                        nOneLength = offset + wRet;
                        break;
                    }
                }

                offset += wRet;
                if (offset >= baPackage.Length)
                {
                    // 扩大缓冲区
                    byte[] temp = new byte[baPackage.Length + 1024];
                    Array.Copy(baPackage, 0, temp, 0, offset);
                    baPackage = temp;
                    nOneLength = baPackage.Length;
                }
            }

            // 最后规整缓冲区尺寸，如果必要的话
            if (baPackage.Length > nOneLength)
            {
                byte[] temp = new byte[nOneLength];
                Array.Copy(baPackage, 0, temp, 0, nOneLength);
                baPackage = temp;
            }

            strPackage = this.Encoding.GetString(baPackage);
            this.PrinteInfo("Recv:" + strPackage);
            //this.WriteToLog("Recv:" + strPackage);

            return 0;
        ERROR1:
            this.CloseSocket();
            baPackage = null;
            return -1;
        }

        #endregion


        #region 一些参数信息

        public Encoding Encoding
        {
            get
            {
                //string strEndodingName = Properties.Settings.Default.EncodingName;
                //if (string.IsNullOrEmpty(strEndodingName))
                //    strEndodingName = "UTF-8";

                string strEndodingName = "UTF-8";

                return Encoding.GetEncoding(strEndodingName);
            }
        }

        // 命令结束符
        char Terminator
        {
            get
            {
                //string strTerminator = Properties.Settings.Default.Terminator;
                //if (strTerminator == "LF") //NewLine
                //    return (char)10;
                //else // if(strTerminator == "CR") //Return
                return (char)13;
            }
        }
        #endregion


        #region 界面控件

        private void SetCtrlState(bool enabled)
        {
            btnConnect.Enabled = enabled;
            btnDown.Enabled = !enabled;
            btnSend.Enabled = !enabled;
        }


        private void PrinteInfo(string text)
        {
            this.listBox_printer.Items.Add(text);
        }

        #endregion
    }
}
