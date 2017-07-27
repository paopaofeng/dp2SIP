using DigitalPlatform;
using DigitalPlatform.SIP2.SIP2Entity;

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



        public string SIPServerUrl
        {
            get
            {
                return Properties.Settings.Default.SIPServerUrl;
            }
        }


        public int SIPServerPort
        {
            get
            {
                return Properties.Settings.Default.SIPServerPort;
            }
        }

        private void Connection()
        {
            this.Enabled = false;
            try
            {
                IPAddress ipAddress = IPAddress.Parse(this.SIPServerUrl);
                string hostName = Dns.GetHostEntry(ipAddress).HostName;
                _client = new TcpClient(hostName, this.SIPServerPort);


                // 保存到一个变量上，方便使用
                _networkStream = _client.GetStream();

                //界面设置
                this.toolStripStatusLabel_info.Text = "连接服务器成功。";
                toolStripLabel_send.Enabled = true;
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel_info.Text = "连接服务器失败:" + ex.Message;
                return;
            }
            finally
            {
                this.Enabled = true;
            }


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

        //发送消息
        private void toolStripLabel_send_Click(object sender, EventArgs e)
        {
            BaseRequest request = null;
            try
            {
                if (this.tabControl_main.SelectedTab == this.tabPage_Login93)
                {
                    request = new Login_93(this.textBox_Login_UIDAlgorithm.Text,
                       this.textBox_Login_PWDAlgorithm.Text,
                       this.textBox_Login_loginUserId_CN_r.Text == "null" ? null : this.textBox_Login_loginUserId_CN_r.Text,
                       this.textBox_Login_loginPassword_CO_r.Text == "null" ? null : this.textBox_Login_loginPassword_CO_r.Text,
                       this.textBox_Login_locationCode_CP_o.Text == "null" ? null : this.textBox_Login_locationCode_CP_o.Text
                       );
                }



                //发送命令
                this.txtMsg.Text = request.ToText();
                this.sendCmd();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        // 回车 触发 发送
        private void txtMsg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.sendCmd();
                return;
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

            string text = this.txtMsg.Text.Trim();
            SendData(text);
        }


        // 发送数据
        private void SendData(string text)
        {
            this.Enabled = false;
            try
            {
                if (this._networkStream.DataAvailable == true)
                {
                    MessageBox.Show("异常：发送前发现流中有未读的数据!");
                    //this.RecvData();
                    return;
                }

                // 刷新界面
                this.Print("send:" + text);
                //txtMsg.Text = "";  // 清空
                this.txtMsg.SelectAll();


                // 命令参数检查
                BaseRequest request = null;
                string error="";
                bool bRet = SCRequestFactory.ParseRequest(text, out request, out error);
                if (bRet == false)
                {
                    this.Print("error-s:" + error);
                    return;
                }

                byte[] baPackage = this.Encoding.GetBytes(text);
                this._networkStream.Write(baPackage, 0, baPackage.Length);
                this._networkStream.Flush();//刷新当前数据流中的数据



                // 调接收数据
                string strRecv = "";
                string strError = "";
                int nRet = RecvTcpPackage(out strRecv, out strError);
                if (nRet == -1)
                {
                    this.Print("error-r:" + strError);
                    return;
                }
                this.Print("recv:" + strRecv);
                //this.PrinteInfo("recv:" + strPackage);
                //this.WriteToLog("Recv:" + strPackage);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Client提示");
            }
            finally
            {
                this.Enabled = true;
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



        private void Print(string text)
        {
            if (this.txtInfo.Text != "")
                this.txtInfo.Text += "\r\n";
            this.txtInfo.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +" "+ text;

            //this.txtInfo.Text = text;
        }

        #endregion




        private void 参数配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_setting dlg = new Form_setting();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.Connection();
            }
        }

        private void 实用工具ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form_checksum dlg = new Form_checksum();
            dlg.ShowDialog(this);
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.SIPServerUrl))
            {
                Form_setting dlg = new Form_setting();
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    this.Close();
                    return;
                }
            }

            this.Connection();

           // this.toolStripStatusLabel_port.Text = "监听端口：" + this.Port;
           // this.toolStripLabel_send.Enabled = true;
        }

        private void 清空信息区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txtInfo.Text = "";
        }







    }
}
