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
using System.Threading.Tasks;
using System.Windows.Forms;

using DigitalPlatform.LibraryClient;
using DigitalPlatform;
using System.IO;

namespace dp2SIPServer
{
    public partial class MainForm : Form
    {
        public LibraryChannelPool _channelPool = new LibraryChannelPool();

        private Thread acceptThread = null;

        Thread clientThread = null;

        private TcpListener Listener = null;

        Session _session = null;

        public MainForm()
        {
            InitializeComponent();
        }

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

        List<LibraryChannel> _channelList = new List<LibraryChannel>();

        // parameters:
        //      style    风格。如果为 GUI，表示会自动添加 Idle 事件，并在其中执行 Application.DoEvents
        public LibraryChannel GetChannel()
        {
            LibraryChannel channel = this._channelPool.GetChannel(Properties.Settings.Default.LibraryServerUrl,
                Properties.Settings.Default.Username);
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


        private void MainForm_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(this.ServerUrl))
            {
                SettingsForm dlg = new SettingsForm();
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    this.Close();
                    return;
                }
            }

            DoListen();
            this.toolStripStatusLabel_port.Text = "监听端口：" + this.Port;
            this.button_start.Enabled = false;
        }

        public void WriteHtml(string strHtml)
        {
            strHtml = String.Format("{0}  {1}<br />", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), strHtml);
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoStop();

            if (_session != null)
            {
                _session.CloseSocket();
            }
        }

        private void DoListen()
        {
            try
            {
                Listener = new TcpListener(IPAddress.Any, Port);
                Listener.Start();
                this.toolStripStatusLabel1.Text = "正在监听...";
                WriteHtml("启动成功");

                acceptThread = new Thread(new ThreadStart(DoAccept));
                acceptThread.Start();
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = "侦听失败!";
                WriteHtml("启动失败" + ExceptionUtil.GetDebugText(ex));
            }
        }

        private void DoStop()
        {
            try
            {
                if (Listener != null)
                    Listener.Stop();

                if (acceptThread != null)
                    acceptThread.Abort();

                if (clientThread != null)
                    clientThread.Abort();
            }
            catch (Exception ex)
            {
                this.WriteToLog(ExceptionUtil.GetDebugText(ex));
            }
            finally
            {
                foreach (LibraryChannel channel in _channelList)
                {
                    if (channel != null)
                        channel.Abort();
                }
                this.toolStripStatusLabel1.Text = "监听已停止";
            }
        }


        private void DoAccept()
        {
            try
            {
                while (true)
                {
                    TcpClient client = Listener.AcceptTcpClient();
                    _session = new Session(client) { MainForm = this };

                    clientThread = new Thread(new ThreadStart(_session.Processing));

                    // Start proccessing
                    clientThread.Start();
                }
            }
            catch(ThreadInterruptedException)
            {
                Thread.CurrentThread.Abort();
            }
            catch (SocketException ex)
            {
                this.WriteToLog(ExceptionUtil.GetDebugText(ex));
            }
        }

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

        private void button_start_Click(object sender, EventArgs e)
        {
            DoListen();
            this.button_start.Enabled = false;
            this.button_stop.Enabled = true;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            DoStop();
            this.button_start.Enabled = true;
            this.button_stop.Enabled = false;
        }

        public void WriteToLog(string strText)
        {
            string strOperLogPath = Application.StartupPath + "\\operlog";

            DirectoryInfo dirInfo = new DirectoryInfo(strOperLogPath);
            if (!dirInfo.Exists)
                dirInfo = Directory.CreateDirectory(strOperLogPath);

            if (String.IsNullOrEmpty(strText) == true)
                return;

            strText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + strText;

            try
            {
                string strFilename = dirInfo.FullName + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                StreamWriter sw = new StreamWriter(strFilename,
                    true,	// append
                    Encoding.UTF8);

                sw.WriteLine(strText);
                sw.Close();
            }
            catch (Exception ex)
            {
                this.WriteHtml("写入日志文件发生错误：" + ExceptionUtil.GetDebugText(ex) + "\r\n");
            }
        }
    }
}
