using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Forms;

using DigitalPlatform;

namespace dp2SIPServer
{

    public partial class MainForm : Form
    {
        private TcpListener Listener = null;
        private Thread _acceptThread = null;

        // 客户端连接Hashtable
        private Hashtable _clientTable = new Hashtable();

        public MainForm()
        {
            InitializeComponent();
        }

        #region 一些配置项

        public string ServerUrl
        {
            get
            {
                return Properties.Settings.Default.LibraryServerUrl;
            }
        }

        public int Port
        {
            get
            {
                return Properties.Settings.Default.Port;
            }
        }

        public Encoding Encoding
        {
            get
            {
                string strEndodingName = Properties.Settings.Default.EncodingName;
                if (string.IsNullOrEmpty(strEndodingName))
                    strEndodingName = "UTF-8";

                return Encoding.GetEncoding(strEndodingName);
            }
        }

        // 命令结束符
        public char Terminator
        {
            get
            {
                string strTerminator = Properties.Settings.Default.Terminator;
                if (strTerminator == "LF")
                    return (char)10;
                else // if(strTerminator == "CR")
                    return (char)13;
            }
        }

        public string DateFormat
        {
            get
            {
                string strDateFormat = Properties.Settings.Default.DateFormat;
                if (string.IsNullOrEmpty(strDateFormat) || strDateFormat.Length < 8)
                    strDateFormat = "yyyy-MM-dd";
                return strDateFormat;
            }
        }

        #endregion

        #region 启动 和 停止 监听

        //窗体加载
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ServerUrl))
            {
                SettingsForm dlg = new SettingsForm();
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    this.Close();
                    return;
                }
            }

            // 启动监听
            DoListen();

            this.toolStripStatusLabel_port.Text = "监听端口：" + this.Port;
            this.button_start.Enabled = false;
        }

        // 窗体关闭
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoStop();
        }

        // 启动监听
        private void button_start_Click(object sender, EventArgs e)
        {
            DoListen();

            this.button_start.Enabled = false;
            this.button_stop.Enabled = true;
        }

        // 停止监听
        private void button_stop_Click(object sender, EventArgs e)
        {
            DoStop();
            this.button_start.Enabled = true;
            this.button_stop.Enabled = false;
        }

        // 设置
        private void toolStripMenuItem_settings_Click(object sender, EventArgs e)
        {
            SettingsForm dlg = new SettingsForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                DoStop();
                DoListen();
                this.toolStripStatusLabel_port.Text = "监听端口：" + this.Port;
            }
        }

        // 停止监听
        private void DoStop()
        {
            try
            {
                if (_acceptThread != null)
                {
                    _acceptThread.Abort();
                    this.WriteToLog(".....acceptThread.Abort()");
                }

                // 关闭TcpClient
                this.CloseClients();

                if (Listener != null)
                {
                    Listener.Stop();
                    this.WriteToLog(".....Listener.Stop()");
                }
            }
            catch (Exception ex)
            {
                this.WriteToLog(ExceptionUtil.GetDebugText(ex));
            }
            finally
            {
                this.toolStripStatusLabel1.Text = "监听已停止";
            }
        }

        // 关闭所有TcpClient连接
        private void CloseClients()
        {
            foreach (TcpClient client in this._clientTable.Keys)
            {
                Session session = (Session)this._clientTable[client];
                session.Close();
            }
            this._clientTable.Clear();
        }

        // 启动监听端口
        private void DoListen()
        {
            try
            {
                // 启动监听
                Listener = new TcpListener(IPAddress.Any, this.Port);
                Listener.Start();
                this.toolStripStatusLabel1.Text = "正在监听...";
                WriteHtml("启动成功");

                //用一个线程 接收请求
                _acceptThread = new Thread(new ThreadStart(DoAccept));
                _acceptThread.Start();
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = "侦听失败!";
                WriteHtml("启动失败" + ExceptionUtil.GetDebugText(ex));
            }
        }




        #endregion


        // 接收TcpClient，并
        private void DoAccept()
        {
            try
            {
                while (true)
                {
                    TcpClient client = Listener.AcceptTcpClient();
                    this.WriteToLog("==收到一个TcpClient请求：" + client.Client.RemoteEndPoint);

                    // 发现同一台前端，断开后重连，是一个新的TcpClient对象，所有一直不会走进去
                    if (this._clientTable.ContainsKey(client) == true)
                    {
                        this.WriteToLog("~~已存在client对象：" + client.ToString());
                    }

                    // 依据TcpClient创建一个会话对象，即一根通道
                    Session session= new Session(client, this );

                    // 保存到hashtable，停止时统一释放
                    this._clientTable[client] = session;
                }
            }
            catch(ThreadInterruptedException)
            {
                Thread.CurrentThread.Abort();
                this.WriteToLog("ThreadInterruptedException：.....Thread.CurrentThread.Abort()");
            }
            catch (SocketException ex)
            {
                this.WriteToLog(ExceptionUtil.GetDebugText(ex));
                Thread.CurrentThread.Abort();
                this.WriteToLog("SocketException：.....Thread.CurrentThread.Abort()");
            }
        }


        #region 写日志

        public void WriteToLog(string strText)
        {
            LogManager.Logger.Info(strText);
        }

        #endregion

        #region 输出信息
        public void WriteHtml(string strHtml)
        {
            strHtml = String.Format("{0}  {1}<br />", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strHtml);
            WriteHtml(this.webBrowser1,
                strHtml);
        }


        // 不支持异步调用
        public static void WriteHtml(WebBrowser webBrowser,
            string strHtml)
        {

            HtmlDocument doc = webBrowser.Document;

            if (doc == null)
            {
                webBrowser.Navigate("about:blank");
                doc = webBrowser.Document;
            }

            // doc = doc.OpenNew(true);
            doc.Write("<pre>");
            doc.Write(strHtml);

            // 保持末行可见
            // ScrollToEnd(webBrowser);
        }

        #endregion
    }

}
