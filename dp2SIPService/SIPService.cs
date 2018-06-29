using DigitalPlatform;
using DigitalPlatform.SIP2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dp2SIPService
{
    public partial class SIPService : ServiceBase
    {
        private TcpListener Listener = null;
        private Thread _acceptThread = null;

        private bool isRun = true;

        // 客户端连接Hashtable
        private Hashtable _clientTable = new Hashtable();

        #region 一些配置项

        public string ServerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["LibraryServerUrl"];
            }
        }

        public int Port
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["Port"]);
            }
        }

        public Encoding Encoding
        {
            get
            {
                string strEndodingName = ConfigurationManager.AppSettings["EncodingName"];
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
                string strTerminator = ConfigurationManager.AppSettings["Terminator"];
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
                string strDateFormat = ConfigurationManager.AppSettings["DateFormat"];
                if (string.IsNullOrEmpty(strDateFormat) || strDateFormat.Length < 8)
                    strDateFormat = "yyyy-MM-dd";
                return strDateFormat;
            }
        }

        #endregion

        public SIPService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (string.IsNullOrEmpty(this.ServerUrl))
            {
                return;
            }

            // 启动监听
            DoListen();

            string info = "监听端口：" + this.Port;
        }

        private void DoListen()
        {
            try
            {
                // 启动监听
                Listener = new TcpListener(IPAddress.Any, this.Port);  //IPAddress.Parse("0.0.0.0")
                Listener.Start();

                // 20170809 jane
                LogManager.Logger.Info("启动监听端口成功:" + this.Port);

                //用一个线程 接收请求
                _acceptThread = new Thread(new ThreadStart(DoAccept));
                _acceptThread.IsBackground = true;
                _acceptThread.Start();
            }
            catch (Exception ex)
            {
                // 20170809 jane
                LogManager.Logger.Info("启动监听端口失败:" + ExceptionUtil.GetDebugText(ex));
            }
        }

        // 接收TcpClient，并
        private void DoAccept()
        {
            try
            {
                while (isRun)
                {
                    if (!Listener.Pending())
                        continue;

                    TcpClient client = Listener.AcceptTcpClient();
                    LogManager.Logger.Info("收到一个TcpClient请求：" + client.Client.RemoteEndPoint);

                    // 发现同一台前端，断开后重连，是一个新的TcpClient对象，所有一直不会走进去
                    if (this._clientTable.ContainsKey(client) == true)
                    {
                        LogManager.Logger.Info("~~已存在client对象：" + client.ToString());
                    }

                    // 依据TcpClient创建一个会话对象，即一根通道
                    Session session = new Session(client);
                    session.CloseEvent += session_CloseEvent;
                    LogManager.Logger.Info("初始化一个会话");

                    // 保存到hashtable，停止时统一释放
                    this._clientTable[session] = session;
                }
            }
            catch (ThreadInterruptedException ex)
            {
                LogManager.Logger.Error(ExceptionUtil.GetDebugText(ex));
            }
            catch (SocketException ex)
            {
                LogManager.Logger.Error(ExceptionUtil.GetDebugText(ex));
            }
        }

        protected override void OnStop()
        {
            DoStop();
        }

        // 停止监听
        private void DoStop()
        {
            LogManager.Logger.Info("开始停止监听...");

            isRun = false;

            try
            {
                // 停止Listener
                if (Listener != null)
                {
                    Listener.Stop();
                    LogManager.Logger.Info("监听对象Listener.Stop()");
                }

                // 关闭TcpClient，在里面记日志
                this.CloseClients();
            }
            catch (Exception ex)
            {
                LogManager.Logger.Info("停止监听出错：" + ExceptionUtil.GetDebugText(ex));
            }
            finally
            {
                LogManager.Logger.Info("停止监听完成");
            }
        }

        // 关闭所有TcpClient连接
        private void CloseClients()
        {
            if (this._clientTable.Count > 0)
            {

                foreach (Session client in this._clientTable.Keys)
                {
                    CloseOneSession(client, false);
                }

                this._clientTable.Clear();
                LogManager.Logger.Info("清空TcpClient Hashtable完成");
            }
            else
            {
                LogManager.Logger.Info("TcpClient Hashtable没有成员，不需要进行清理的工作。");
            }
        }

        public void CloseOneSession(Session client, bool bRemove)
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

        void session_CloseEvent(object sender, EventArgs e)
        {
            Session session = sender as Session;
            if (session != null)
            {
                this.CloseOneSession(session, true);
            }
        }
    }
}
