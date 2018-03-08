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
using DigitalPlatform.SIP2;

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

            string info="监听端口：" + this.Port;
            this.toolStripStatusLabel_port.Text =info ;

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
            LogManager.Logger.Info("开始停止监听...");
            try
            {
                // 中止接收线程
                if (_acceptThread != null)
                {
                    _acceptThread.Abort();
                    LogManager.Logger.Info("接收线程_acceptThread.Abort()");
                }

                // 关闭TcpClient，在里面记日志
                this.CloseClients();

                // 停止Listener
                if (Listener != null)
                {
                    Listener.Stop();
                    LogManager.Logger.Info("监听对象Listener.Stop()");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.Info("停止监听出错：" + ExceptionUtil.GetDebugText(ex));
            }
            finally
            {
                this.toolStripStatusLabel1.Text = "监听已停止";
                LogManager.Logger.Info("停止监听完成");
            }
        }

        public void CloseOneSession(Session client,bool bRemove)
        {

            string remoteEndPoint = client.RemoteEndPoint;

            Session session = (Session)this._clientTable[client];
            if (session == null)
            {
                LogManager.Logger.Warn("从hashtable中没有找到Session：" + remoteEndPoint + "");             
                return;
            }
            LogManager.Logger.Info("开始关闭Session：" + remoteEndPoint + "...");             
            session.Close();

            LogManager.Logger.Info("关闭Session：" + remoteEndPoint + "完成");

            if (bRemove == true)
            {
                this._clientTable.Remove(client);
                LogManager.Logger.Info("从hashtable中移动session[" + remoteEndPoint + "]完成，目前数量" + this._clientTable.Count);
            }

        }

        // 关闭所有TcpClient连接
        private void CloseClients()
        {
            if (this._clientTable.Count > 0)
            {

                foreach (Session client in this._clientTable.Keys)
                {
                    CloseOneSession(client,false);
                }

                this._clientTable.Clear();
                LogManager.Logger.Info("清空TcpClient Hashtable完成");
            }
            else
            {
                LogManager.Logger.Info("TcpClient Hashtable没有成员，不需要进行清理的工作。");
            }
        }



        // 启动监听端口
        private void DoListen()
        {
            try
            {
                // 启动监听
                Listener = new TcpListener(IPAddress.Any, this.Port);  //IPAddress.Parse("0.0.0.0")
                Listener.Start();

                this.toolStripStatusLabel1.Text = "正在监听...";
                WriteHtml("启动成功");

                // 20170809 jane
                LogManager.Logger.Info("启动监听端口成功:"+this.Port);


                //用一个线程 接收请求
                _acceptThread = new Thread(new ThreadStart(DoAccept));
                _acceptThread.Start();
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = "监听失败!";
                WriteHtml("启动失败" + ExceptionUtil.GetDebugText(ex));

                // 20170809 jane
                LogManager.Logger.Info("启动监听端口失败:" + ExceptionUtil.GetDebugText(ex));
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
                    LogManager.Logger.Info("收到一个TcpClient请求：" + client.Client.RemoteEndPoint);

                    // 发现同一台前端，断开后重连，是一个新的TcpClient对象，所有一直不会走进去
                    if (this._clientTable.ContainsKey(client) == true)
                    {
                        LogManager.Logger.Info("~~已存在client对象：" + client.ToString());
                    }

                    // 依据TcpClient创建一个会话对象，即一根通道
                    Session session= new Session(client);
                    session.CloseEvent += session_CloseEvent;
                    LogManager.Logger.Info("初始化一个会话");

                    // 保存到hashtable，停止时统一释放
                    this._clientTable[session] = session;
                }
            }
            catch(ThreadInterruptedException)
            {
                Thread.CurrentThread.Abort();
                LogManager.Logger.Info("ThreadInterruptedException：.....Thread.CurrentThread.Abort()");
            }
            catch (SocketException ex)
            {
                LogManager.Logger.Info(ExceptionUtil.GetDebugText(ex));
                Thread.CurrentThread.Abort();
                LogManager.Logger.Info("SocketException：.....Thread.CurrentThread.Abort()");
            }
        }

        void session_CloseEvent(object sender, EventArgs e)
        {
            Session session = sender as Session;
            if (session != null)
            {
                this.CloseOneSession(session,true);
            }
        }


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
