#define NEW
#define Default

using DigitalPlatform;
using DigitalPlatform.IO;
using DigitalPlatform.LibraryClient;
using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.Marc;
using DigitalPlatform.SIP2;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace dp2SIPServer
{
    public class Session : IDisposable //不清楚去掉IDisposable可以吗？jane 20170808
    {
        //TcpClient包装类，其中的TcpClient是监听收到的对象
        TcpClientWrapper _client = null;

        // 处理接收消息与发送消息的线程
        Thread _clientThread = null;
        //SIP消息服务类
        SIP _sip = new SIP();

        // 登录dp2系统的帐户
        string _dp2username = "";
        string _dp2password = "";
        // 3M设备工作台号
        string _locationCode = "";

        public string RemoteEndPoint = "";

        internal Session(TcpClient client)
        {
            this.RemoteEndPoint = client.Client.RemoteEndPoint.ToString();
            this._client = new TcpClientWrapper(client) 
            {
                Encoding=this.Encoding,
                MessageTerminator=this.Terminator
            };

            // 挂上按需登录回调事件
            this._channelPool.BeforeLogin += _channelPool_BeforeLogin;

            // 开一个线程来接收和发送消息
            this._clientThread = new Thread(new ThreadStart(this.Processing));
            this._clientThread.Start();// Start proccessing
        }

        // 关闭通道
        public void Close()
        {
            LogManager.Logger.Info("走进Session的Close");
            try
            {


                // 关闭TcpClient
                if (_client != null)
                {
                    this._client.Close();
                    LogManager.Logger.Info("关闭TcpClient完成。工作台为[" + this._locationCode + "],登录帐户为[" + this._dp2username + "]");
                }
                else
                {
                    LogManager.Logger.Warn("此Session的_client对象为null");
                }

                // 关闭dp2通道
                if (this._channelList.Count > 0)
                {
                    foreach (LibraryChannel channel in this._channelList)
                    {
                        if (channel != null)
                        {
                            string changeUserName = channel.UserName;
                            channel.Abort();
                            LogManager.Logger.Info("断开dp2 channel完成。channel.UserName为[" + changeUserName + "]");
                        }
                    }

                    this._channelList.Clear();
                    LogManager.Logger.Info("清空dp2 channel数组完成");
                }
                else
                {
                    LogManager.Logger.Warn("此Session里没有dp2 channel通道");
                }

                // 中止接收线程
                if (this._clientThread != null)
                {
                    _clientThread.Abort();
                    LogManager.Logger.Info("中止Session中处理消息线程_clientThread.Abort()");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.Error("Session的Close异常：" + ex.Message);
            }
            finally
            {
                LogManager.Logger.Info("结束Session的Close");
            }

        }

        // 如果调的地方都处理好关闭，还需要加这个Dispose函数吗？
        public void Dispose()
        {
            LogManager.Logger.Info("!!!走到Session的Dispose()");

            this.Close();

            LogManager.Logger.Info("!!!完成Session的Dispose()");
        }

        #region 关于dp2通道

        void _channelPool_BeforeLogin(object sender, BeforeLoginEventArgs e)
        {
            if (string.IsNullOrEmpty(this._dp2username))
            {
                e.Cancel = true;
                e.ErrorInfo = "尚未登录";
            }

            e.LibraryServerUrl = Properties.Settings.Default.LibraryServerUrl;
            e.UserName = this._dp2username;
            e.Parameters = "type=worker,client=dp2SIPServer|0.01";
            e.Password = this._dp2password;
            e.SavePasswordLong = true;
        }

        public LibraryChannelPool _channelPool = new LibraryChannelPool();
        List<LibraryChannel> _channelList = new List<LibraryChannel>();

        // parameters:
        //      style    风格。如果为 GUI，表示会自动添加 Idle 事件，并在其中执行 Application.DoEvents
        public LibraryChannel GetChannel(string strUsername = "")
        {
            if (strUsername == "")
                strUsername = Properties.Settings.Default.Username;

            LibraryChannel channel = this._channelPool.GetChannel(Properties.Settings.Default.LibraryServerUrl,
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

        #endregion

        #region 接收 和 发送消息

        /// <summary>
        /// Session处理轮回
        /// </summary>
        public void Processing()
        {
            LogManager.Logger.Info("开始会话里的消息处理线程");
            string strPackage = "";
            string strError = "";
            if (this._client == null)
            {
                strError = "client尚未初始化";
                LogManager.Logger.Warn("Session中的Client不可能为null");
                return;
            }

            try
            {
                while (true)
                {
                    // 先接收消息
                    int nRet = this._client.RecvMessage(out strPackage, out strError);//RecvTcpPackage(out strPackage, out strError);
                    if (nRet == -1)
                    {
                        strError = "接收消息出错：" + strError;
                        goto ERROR1;
                    }
                    LogManager.Logger.Info("Recv:" + strPackage);

                    // 处理消息
                    string strBackMsg = "";
                    nRet = this.HandleOneMessage(strPackage, out strBackMsg, out strError);
                    if (nRet == -1)
                    {
                        strError = "处理消息出错：" + strError;
                        goto ERROR1;
                    }

                    // 发送返回的消息
                    nRet = this._client.SendMessage(strBackMsg, out strError);
                    if (nRet == -1)
                    {
                        strError = "发送消息出错：" + strError;
                        goto ERROR1;
                    }
                    LogManager.Logger.Info("Send:" + strBackMsg);

                }

            ERROR1:
                LogManager.Logger.Error(strError);
                return;
            }
            catch (ThreadAbortException ex)
            {
                LogManager.Logger.Info("session处理消息的线程终止："+ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.Logger.Error("Session里的Process函数异常:" + ExceptionUtil.GetDebugText(ex));
            }
            finally
            {
                LogManager.Logger.Info("走到Session的Process()的finally,触发CloseEvent事件。");
                //this.Close();

                if (CloseEvent != null)
                {
                    object sender = this;
                    EventArgs e=new EventArgs();
                    CloseEvent(sender,e);
                }

            }


        }
        public event CloseEventHandle CloseEvent;
        void SendCloseEvent(object sender,
            EventArgs e)
        {
            if (this.CloseEvent != null)
                this.CloseEvent(sender, e);
        }

        public delegate void CloseEventHandle(object sender,
        EventArgs e);

        // 处理一条消息
        private int HandleOneMessage(string strPackage, out string strBackMsg, out string strError)
        {
            strBackMsg = "";
            strError = "";
            int nRet = 0;

            if (strPackage.Length < 2)
            {
                strError="命令错误，命令长度不够2位";
                return -1;
            }

            // 消息分隔符
            string strMessageIdentifiers = strPackage.Substring(0, 2);

            // 处理消息
            switch (strMessageIdentifiers)
            {
                case "09":
                    {
                        /*
                         strBackMsg = Return(strItemBarcode);
                        */
                        LibraryChannel channel = this.GetChannel(this._dp2username);
                        try
                        {
                            strBackMsg = this._sip.Checkin(channel, strPackage);
                        }
                        finally
                        {
                            this.ReturnChannel(channel);
                        }
                        break;
                    }
                case "11":
                    {
                        /*
                        strBackMsg = Borrow(false, strReaderBarcode, strItemBarcode, "auto_renew");
                        */
                        LibraryChannel channel = this.GetChannel(this._dp2username);
                        try
                        {
                            strBackMsg = this._sip.Checkout(channel, strPackage);
                        }
                        finally
                        {
                            this.ReturnChannel(channel);
                        }
                        break;
                    }
                case "17":
                    {
                        /*
                        strBackMsg = GetItemInfo(strItemBarcode);
                        */
                        LibraryChannel channel = this.GetChannel(this._dp2username);
                        try
                        {
                            strBackMsg = this._sip.ItemInfo(channel, strPackage);
                        }
                        finally
                        {
                            this.ReturnChannel(channel);
                        }
                        break;
                    }
                case "29":
                    {
                        /*
                        strBackMsg = Borrow(true, // 续借
                            "",  // 读者条码号为空，续借
                            strItemBarcode);
                        */
                        LibraryChannel channel = this.GetChannel(this._dp2username);
                        try
                        {
                            strBackMsg = this._sip.Renew(channel, strPackage);
                        }
                        finally
                        {
                            this.ReturnChannel(channel);
                        }
                        break;
                    }
                case "35":
                    {
                        strBackMsg = this._sip.EndPatronSession(strPackage);
                        break;
                    }
                case "37":
                    {
                        LibraryChannel channel = this.GetChannel(this._dp2username);
                        try
                        {
                            strBackMsg = this._sip.Amerce(channel, strPackage);
                        }
                        finally
                        {
                            this.ReturnChannel(channel);
                        }
                        break;
                    }
                case "85":
                    {
                        /*
                        nRet = GetReaderInfo(strReaderBarcode,
                            strPassword,
                            "readerInfo",
                            out strBackMsg,
                            out strError);
                        if (nRet == 0)
                            this.WriteToLog(strError);
                        */
                        break;
                    }
                case "63":
                    {
                        // strBackMsg = GetReaderInfo(strReaderBarcode, strPassword);
                        LibraryChannel channel = this.GetChannel(this._dp2username);
                        try
                        {
                            strBackMsg = this._sip.PatronInfo(channel, strPackage);
                        }
                        finally
                        {
                            this.ReturnChannel(channel);
                        }
                        break;
                    }
                case "81":
                    {
                        nRet = SetReaderInfo(strPackage, out strBackMsg, out strError);
                        if (nRet == 0)
                        {
                            if (String.IsNullOrEmpty(strError) == false)
                                LogManager.Logger.Error(strError);
                        }
                        break;
                    }
                case "91":
                    {
                        nRet = CheckDupReaderInfo(strPackage, out strBackMsg, out strError);
                        if (nRet == 0)
                        {
                            if (String.IsNullOrEmpty(strError) == false)
                                LogManager.Logger.Error(strError);
                        }
                        break;
                    }
                case "93":
                    {
                        // strBackMsg = Login(strPackage);

                        LibraryChannel channel = this.GetChannel();
                        try
                        {
                            strBackMsg = this._sip.Login(channel, strPackage);
                            if ("941" == strBackMsg)
                            {
                                this._dp2username = this._sip.LoginUserId;
                                this._dp2password = this._sip.LoginPassword;
                                this._locationCode = this._sip.LocationCode;
                            }
                        }
                        finally
                        {
                            this.ReturnChannel(channel);
                        }
                        break;
                    }
                case "99":
                    {
                        strBackMsg = this._sip.SCStatus(strPackage);
                        break;
                    }
                default:
                    strBackMsg = "无法识别的命令'" + strMessageIdentifiers + "'";
                    break;
            }

            // 加校验码
            strBackMsg = this.AddChecksumForMessage(strBackMsg);
            return 0;
        }

        #endregion

        #region 一些配置项


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

        #region 一些内部函数

        // 加校验码
        private string AddChecksumForMessage(string strPackage)
        {
            // 加校验码
            StringBuilder msg = new StringBuilder(strPackage);
            char endChar = strPackage[strPackage.Length - 1];
            if (endChar == '|')
                msg.Append("AY4AZ");
            else
                msg.Append("|AY4AZ");
            msg.Append(SIP.GetChecksum(strPackage));

            // 写日志
            //LogManager.Logger.Info("Send:" + msg.ToString());

            // 加消息结束符
            msg.Append(this.Terminator);
            return msg.ToString();
        }

        /// <summary>
        /// 获得图书信息
        /// 图书状态
        ///  1		other -- 其他
        ///  2		on order -- 订购中
        ///  3		available -- 可借
        ///  4		charged -- 在借
        /// 12		lost -- 丢失
        /// 13		missing -- 没有找到

        ///  5		charged; not to be recalled until earliest recall date -- 在借
        ///  6		in process -- 
        ///  7		recalled -- 召回
        ///  8		waiting on hold shelf -- 等待上架
        ///  9		waiting to be re-shelved -- 倒架中
        /// 10		in transit between library locations
        /// 11		claimed returned
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <param name="strBackMsg"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        string GetItemInfo(string strBarcode)
        {
            string strError = "";

            int nRet = 0;

            string strBookState = "";
            string strReaderBarcode = "";
            string strTitle = "";
            string strAuthor = "";
            string strISBN = "";

            // string strIsArrived = "";

            // string strBookType = "";

            // 000    other
            // 001    book
            // 002    magazine
            // 003    bound journal
            // 004    audio tape
            // 005    video tape
            // 006    CD/CDROM
            // 007    diskette
            // 008    book with diskette
            // 009    book with CD
            // 010    book with audio tape
            string strMediatype = "001";

            // string strPrice = "";
            /*
            string strCurrencyType = ""; // 币种
            string strFeeAmount = "";  // 费用金额
            */

            string strAccessNo = "";

            int nReservations = 0; // 预约请求人数

            // string strDocType = ""; // 文献类型
            string strLocation = "";
            string strReturningDate = "";
            string strBorrowDate = "";

            string strItemXml = "";
            string strBiblio = "";
            LibraryChannel channel = this.GetChannel(this._dp2username);
            StringBuilder sb = new StringBuilder(1024);
            try
            {
                long lRet = channel.GetItemInfo(null,
                    strBarcode,
                    "xml",
                    out strItemXml,
                    "xml",
                    out strBiblio,
                    out strError);
                switch (lRet)
                {
                    case -1:
                        nRet = 0;
                        strBookState = "01";
                        strError = "获得'" + strBarcode + "'发生错误:" + strError;
                        break;
                    case 0:
                        nRet = 0;
                        strBookState = "13";
                        strError = strBarcode + " 册记录不存在";
                        break;
                    case 1:
                        nRet = 1;
                        break;
                    default: // lRet > 1
                        nRet = 0;
                        strBookState = "01";
                        strError = strBarcode + " 找到多条册记录，条码重复";
                        break;
                }

#if PJST
            switch (lRet)
            {
                case -1:
                    nRet = 0;
                    strBookState = "05";
                    strError = "获得'" + strBarcode + "'发生错误:" + strError;
                    break;
                case 0:
                    nRet = 0;
                    strBookState = "00";
                    strError = strBarcode + " 册记录不存在";
                    break;
                case 1:
                    nRet = 1;
                    break;
                default: // lRet > 1
                    nRet = 0;
                    strBookState = "05";
                    strError = strBarcode + " 找到多条册记录，条码重复";
                    break;
            }
#endif
                XmlDocument dom = new XmlDocument();
                if (nRet == 1)
                {
                    try
                    {
                        dom.LoadXml(strItemXml);

                        string strItemState = DomUtil.GetElementText(dom.DocumentElement, "state");
                        if (String.IsNullOrEmpty(strItemState))
                        {
                            strReaderBarcode = DomUtil.GetElementText(dom.DocumentElement, "borrower");
#if PJST
                        strBookState = String.IsNullOrEmpty(strReaderBarcode) ? "02" : "03"; // 2在馆 3借出
#endif
                            strBookState = String.IsNullOrEmpty(strReaderBarcode) ? "03" : "04";

                            XmlNodeList nodesReservationRequest = dom.DocumentElement.SelectNodes("reservations/request");
                            if (nodesReservationRequest != null)
                                nReservations = nodesReservationRequest.Count;
                        }
                        else
                        {
#if PJST
                        if (StringUtil.IsInList("丢失", strItemState))
                            strBookState = "04";
                        if (StringUtil.IsInList("#reservation", strItemState))
                            strIsArrived = "1";
                        else
                            strIsArrived = "0";
#endif
                            if (StringUtil.IsInList("丢失", strItemState))
                                strBookState = "12";
                        }
                        /*
                                            string strPrice = DomUtil.GetElementText(dom.DocumentElement, "price");
                                            CurrencyItem currencyItem = null;
                                            if (!string.IsNullOrEmpty(strPrice))
                                            {
                                                int nRet1 = PriceUtil.ParseSinglePrice(strPrice, out currencyItem, out strError);
                                                if (nRet1 == 0)
                                                {
                                                    strCurrencyType = currencyItem.Prefix;
                                                    strFeeAmount = currencyItem.Value.ToString();
                                                }
                                                if (string.IsNullOrEmpty(strCurrencyType) == false && strCurrencyType.Length != 3)
                                                    strCurrencyType = "CNY";
                                            }

                        */
                        // strBookType = DomUtil.GetElementText(dom.DocumentElement, "bookType");
                        strAccessNo = DomUtil.GetElementText(dom.DocumentElement, "accessNo");
                        strLocation = DomUtil.GetElementText(dom.DocumentElement, "location");

                        strBorrowDate = DomUtil.GetElementText(dom.DocumentElement, "borrowDate");
                        strBorrowDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strBorrowDate, "yyyyMMdd    HHmmss");

                        strReturningDate = DomUtil.GetElementText(dom.DocumentElement, "returningDate");
                        strReturningDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strReturningDate, this.DateFormat);


                        string strMarcSyntax = "";
                        MarcRecord record = SIP.MarcXml2MarcRecord(strBiblio, out strMarcSyntax, out strError);
                        if (record != null)
                        {
                            if (strMarcSyntax == "unimarc")
                            {
                                strISBN = record.select("field[@name='010']/subfield[@name='a']").FirstContent;
                                strTitle = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                                strAuthor = record.select("field[@name='200']/subfield[@name='f']").FirstContent;
                            }
                            else if (strMarcSyntax == "usmarc")
                            {
                                strISBN = record.select("field[@name='020']/subfield[@name='a']").FirstContent;
                                strTitle = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                                strAuthor = record.select("field[@name='245']/subfield[@name='c']").FirstContent;
                            }
                        }
                        else
                        {
                            nRet = 0;
                            strError = "书目信息解析错误:" + strError;
                        }
                    }
                    catch (Exception ex)
                    {
                        nRet = 0;
                        strError = strBarcode + ":读者记录解析错误:" + ExceptionUtil.GetDebugText(ex);
                    }
                }

                strError = RegularString(strError);

                sb.Append("18").Append(strBookState).Append("0001").Append(SIPUtility.NowDateTime);
                if (nReservations > 0)
                    sb.Append("CF").Append(nReservations);
#if PJST
            if (String.IsNullOrEmpty(strReaderBarcode) == false)
                sb.Append("|AA").Append(strReaderBarcode);
#endif
                sb.Append("|AB").Append(strBarcode);
                if (nRet == 0)
                {
                    sb.Append("|AJ");//AJ是必备字段
                    sb.Append("|AF").Append("获得图书信息发生错误！").Append(strError).Append("|AG").Append("获得图书信息发生错误！");
                    LogManager.Logger.Error(strError);
                }
                else
                {
                    // sb.Append("|CJ"); // 续借日期
                    sb.Append("|AJ").Append(strTitle);

                    /*
                    if (!string.IsNullOrEmpty(strCurrencyType))
                        sb.Append("|BH").Append(strCurrencyType);
                    if (!string.IsNullOrEmpty(strFeeAmount))
                        sb.Append("|BV").Append(strFeeAmount);
                    */

                    if (!string.IsNullOrEmpty(strReaderBarcode))
                        sb.Append("|BG").Append(strReaderBarcode);
                    if (!string.IsNullOrEmpty(strBorrowDate))
                        sb.Append("|CM").Append(strBorrowDate); // 借阅日期
                    if (!string.IsNullOrEmpty(strReturningDate))
                        sb.Append("|AH").Append(strReturningDate);

#if PJST
                sb.Append("|AW").Append(strAuthor).Append("|AK").Append(strISBN);
                sb.Append("|RE").Append(strIsArrived).Append("|KC").Append(strAccessNo);
                sb.Append("|CH").Append(strPrice);
#endif
                    sb.Append("|CK").Append(strMediatype);
                    sb.Append("|AP").Append(strLocation);

                    if (nReservations > 0)
                        sb.Append("|AF此书被预约").Append("|AG此书被预约");
                }
            }
            finally
            {
                this.ReturnChannel(channel);
            }

            return sb.ToString();
        }


        int SetReaderInfo(string strSIP2Package,
            out string strBackMsg,
            out string strError)
        {
            strBackMsg = "";
            strError = "";

            int nRet = 0;

            string strBarcode = "";
            string strOperation = "";
            string[] parts = strSIP2Package.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length < 2)
                    continue;

                string strValue = part.Substring(2);
                string str = part.Substring(0, 2);

                if (str == "AA")
                    strBarcode = strValue;
                else if (str == "XK")
                    strOperation = strValue;
                else
                    continue;

                if (String.IsNullOrEmpty(strBarcode) == false
                    && String.IsNullOrEmpty(strOperation) == false)
                    break;
            }

            if (String.IsNullOrEmpty(strOperation))
            {
                nRet = 0;
                strBackMsg = "82" + SIPUtility.NowDateTime + "AOdp2Library|AA" + strBarcode +
                    "|XK" + strOperation +
                    "|OK0|AF修改读者记录发生错误，命令不对。|AG修改读者记录发生错误，命令不对。";
            }
            else if (strOperation == "01"
                || strOperation == "11"
                || strOperation == "02")
            {
                nRet = DoSetReaderInfo(strSIP2Package, out strBackMsg, out strError);
            }
            else if (strOperation == "14")
            {
                nRet = ChangePassword(strSIP2Package, out strBackMsg, out strError);
            }

            return nRet;
        }

        int DoSetReaderInfo(string strSIP2Package,
            out string strBackMsg,
            out string strError)
        {
            strBackMsg = "";
            strError = "";

            long lRetValue = 0;

            string strMsg = ""; // 返回给SIP2的错误信息

            string strAction = "";
            string strReaderBarcode = "";
            string strIDCardNumber = ""; // 身份证号

            bool bForegift = false; // 是否创建押金
            string strForegiftValue = ""; // 押金金额

            string strPassword = "";

            string strOperation = "";

            LibraryChannel channel = this.GetChannel(this._dp2username);

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("82").Append(SIPUtility.NowDateTime).Append("AOdp2Library");

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<root />");

            #region 处理SIP2通讯包，构造读者dom
            string[] parts = strSIP2Package.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length < 2)
                    continue;

                string strValue = part.Substring(2);
                switch (part.Substring(0, 2))
                {
                    case "AA":
                        {
                            if (String.IsNullOrEmpty(strValue))
                            {
                                strMsg = "办证时读者证号不能为空";
                                goto ERROR1;
                            }
                            strReaderBarcode = strValue;
                            DomUtil.SetElementText(dom.DocumentElement, "barcode", strValue);
                            break;
                        }
                    case "XO":
                        {
                            if (String.IsNullOrEmpty(strValue))
                            {
                                strMsg = "办证时身份证号不能为空";
                                goto ERROR1;
                            }
                            strIDCardNumber = strValue;
                            DomUtil.SetElementText(dom.DocumentElement, "idCardNumber", strValue);
                            break;
                        }
                    case "AD":
                        {
                            strPassword = strValue;
                            break;
                        }
                    case "XF":
                        DomUtil.SetElementText(dom.DocumentElement, "comment", strValue);
                        break;
                    case "XT":
                        DomUtil.SetElementText(dom.DocumentElement, "readerType", strValue);
                        break;
                    case "BV":
                        {
                            strForegiftValue = strValue;
                            break;
                        }
                    case "AM": // 开户馆
                        // DomUtil.SetElementText(dom.DocumentElement, "readerType", strValue);
                        break;
                    case "BD":
                        DomUtil.SetElementText(dom.DocumentElement, "address", strValue);
                        break;
                    case "XM":
                        {
                            if (strValue == "0")
                                strValue = "女";
                            else
                                strValue = "男";
                            DomUtil.SetElementText(dom.DocumentElement, "gender", strValue);
                            break;
                        }
                    case "MP":
                        DomUtil.SetElementText(dom.DocumentElement, "tel", strValue);
                        break;
                    case "XH":
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(strValue) && strValue != "00010101")
                                {
                                    DateTime dt = DateTimeUtil.Long8ToDateTime(strValue);

                                    strValue = DateTimeUtil.Rfc1123DateTimeStringEx(dt);

                                    // dt = DateTimeUtil.FromRfc1123DateTimeString(strValue).ToLocalTime();

                                    DomUtil.SetElementText(dom.DocumentElement, "dateOfBirth", strValue);
                                }
                            }
                            catch { }
                            break;
                        }
                    case "AE":
                        DomUtil.SetElementText(dom.DocumentElement, "name", strValue);
                        break;
                    case "XN": // 民族
                        DomUtil.SetElementText(dom.DocumentElement, "station", strValue);
                        break;
                    case "XK": // 操作类型，01 办证操作 11办证但不处理押金
                        if (strValue == "01")
                        {
                            strAction = "new";
                            bForegift = true;
                            // DomUtil.SetElementText(dom.DocumentElement, "state", "暂停");
                        }
                        else if (strValue == "11")
                        {
                            strAction = "new";
                            bForegift = false;
                        }
                        else if (strValue == "02")
                        {
                            strAction = "change";
                        }
                        strOperation = strValue;
                        break;
                    default:
                        break;
                }
            } // end of for
            #endregion

            string strOldXml = "";

            string[] results = null;



            #region 根据卡号检索读者记录是否存在
            long lRet = channel.SearchReader(null,
                "<all>",
                strReaderBarcode,
                -1,
                "Barcode",
                "exact",
                "en",
                "default",
                "keycount",
                out strError);
            if (lRet == -1)
            {
                strMsg = "办证失败！按证号查找读者记录发生错误。";
                goto ERROR1;
            }
            else if (lRet >= 1)
            {
                strMsg = "办证失败！卡号为【" + strReaderBarcode + "】的读者已存在。";
                goto ERROR1;
            }
            #endregion

            #region 根据身份证号获得读者记录
            byte[] baTimestamp = null;
            string strRecPath = "";
            lRet = channel.GetReaderInfo(null,
                strIDCardNumber,
                "xml",
                out results,
                out strRecPath,
                out baTimestamp,
                out strError);
            switch (lRet)
            {
                case -1:
                    strMsg = "办证失败！按身份证号查找读者记录发生错误。";
                    goto ERROR1;
                case 0:
                    strRecPath = "读者/?";
                    break;
                case 1:
                    {
                        if (strAction == "change")
                        {
                            strOldXml = results[0];
                        }
                        else // strAction == "new"
                        {
                            XmlDocument result_dom = new XmlDocument();
                            result_dom.LoadXml(results[0]);
                            string strBarcode = DomUtil.GetElementText(result_dom.DocumentElement, "barcode");
                            strMsg = "办证失败！您已经有一张卡号为【" + strBarcode + "】的读者证，不能再办证。如读者证丢失，需补证请到柜台办理。";
                            goto ERROR1;
                        }
                        break;
                    }
                default: // lRet > 1
                    strMsg = "办证失败！身份证号为【" + strIDCardNumber + "】的读者已存在多条记录。";
                    goto ERROR1;
            }
            #endregion


            string strExistingXml = "";
            string strSavedXml = "";
            string strSavedRecPath = "";
            byte[] baNewTimestamp = null;
            ErrorCodeValue kernel_errorcode = ErrorCodeValue.NoError;
            lRet = channel.SetReaderInfo(null,
                strAction,
                strRecPath,
                dom.DocumentElement.OuterXml, // strNewXml
                strOldXml,
                baTimestamp,
                out strExistingXml,
                out strSavedXml,
                out strSavedRecPath,
                out baNewTimestamp,
                out kernel_errorcode,
                out strError);
            if (lRet == -1)
            {
                strMsg = strAction == "new" ? "办证失败！创建读者记录发生错误。" : "修改读者信息发生错误。";
                goto ERROR1;
            }

            lRetValue = lRet;

            if (bForegift == true
                && String.IsNullOrEmpty(strForegiftValue) == false)
            {
                // 创建交费请求
                string strReaderXml = "";
                string strOverdueID = "";
                lRet = channel.Foregift(null,
                   "foregift",
                   strReaderBarcode,
                    out strReaderXml,
                    out strOverdueID,
                   out strError);
                if (lRet == -1)
                {
                    lRet = DeleteReader(strSavedRecPath, baNewTimestamp, out strError);
                    if (lRet == -1)
                        strError = "办证过程中交费发生错误（回滚失败）：" + strError;
                    else
                        strError = "办证过程中交费发生错误（回滚成功）";


                    strMsg = "办证交费过程中创建交费请求失败，办证失败，请重新操作。";
                    goto ERROR1;
                }

                int nRet = DoAmerce(strReaderBarcode,
                    strForegiftValue,
                    out strMsg,
                    out strError);
                if (nRet == 0)
                    goto ERROR1;
            }

            if (String.IsNullOrEmpty(strPassword) == false
                && strAction == "new")
            {
                lRet = channel.ChangeReaderPassword(null,
                    strReaderBarcode,
                    "", // strOldReaderPassword
                    strPassword,
                    out strError);
                if (lRet != 1)
                {
                    strMsg = "设置密码不成功，可用[生日]登录后再修改密码。";
                }
            }
            sb.Append("|AA").Append(strReaderBarcode).Append("|XD").Append("|OK1");
            if (lRetValue == 0)
            {
                strMsg = strAction == "new" ? "办理新证成功！" + strMsg : "读者信息修改成功！";
            }
            else // if (lRetValue == 1)
            {
                strMsg = strAction == "new" ? "办理新证成功！但部分内容被拒绝。" + strMsg : "读者信息修改成功！但部分内容被拒绝。";
            }
            sb.Append("|XK").Append(strOperation);
            sb.Append("|AF").Append(strMsg).Append("|AG").Append(strMsg);
            strBackMsg = sb.ToString();
            this.ReturnChannel(channel);
            return 1;

            ERROR1:
            sb.Append("|XK").Append(strOperation).Append("|OK0").Append("|AF").Append(strMsg).Append("|AG").Append(strMsg);
            strBackMsg = sb.ToString();
            this.ReturnChannel(channel);
            return 0;
        }


        int CheckDupReaderInfo(string strSIP2Package,
             out string strBackMsg,
             out string strError)
        {
            strBackMsg = "";
            strError = "";

            LibraryChannel channel = this.GetChannel(this._dp2username);

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("9220141021 100511AOdp2Library");

            string strBarcode = "";
            string strIDCardNumber = "";
            string strOperation = "";
            string[] parts = strSIP2Package.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length < 2)
                    continue;

                string strValue = part.Substring(2);
                string str = part.Substring(0, 2);

                if (str == "AA")
                    strBarcode = strValue;
                if (str == "XO")
                    strIDCardNumber = strValue;
                else if (str == "XK")
                    strOperation = strValue;
                else
                    continue;
            }

            sb.Append("|AA").Append(strBarcode);
            sb.Append("|XO").Append(strIDCardNumber);

            if ((strOperation == "0" && String.IsNullOrEmpty(strBarcode))
                || (strOperation == "1" && String.IsNullOrEmpty(strIDCardNumber)))
            {
                goto ERROR1;
            }

            #region 根据借书证号或身份证号获得读者记录
            string[] results = null;
            long lRet = channel.GetReaderInfo(null,
                strOperation == "0" ? strBarcode : strIDCardNumber,
                "xml",
                out results,
                out strError);
            switch (lRet)
            {
                case -1:
                    goto ERROR1;
                case 0:
                    sb.Append("|AC0");
                    break;
                case 1:
                    sb.Append("|AC1");
                    break;
                default: // lRet > 1
                    sb.Append("|AC1");
                    break;
            }
            #endregion

            strBackMsg = sb.ToString();
            this.ReturnChannel(channel);
            return 1;

            ERROR1:
            sb.Append("|XK").Append(strOperation).Append("|OK0");
            strBackMsg = sb.ToString();
            this.ReturnChannel(channel);
            return 0;
        }


        #region 交押金
        int DoAmerce(string strReaderBarcode,
            string strForegiftValue,
            out string strMsg,
            out string strError)
        {
            strMsg = "";
            strError = "";

            int nRet = 0;

            LibraryChannel channel = this.GetChannel();

            byte[] baTimestamp = null;
            string strRecPath = "";
            string[] results = null;
            long lRet = channel.GetReaderInfo(null,
                strReaderBarcode,
                "xml",
                out results,
                out strRecPath,
                out baTimestamp,
                out strError);
            if (lRet == -1)
            {
                strMsg = "办证交押金时获得读者记录发生错误，办证失败，请重新操作。";
            }
            else if (lRet == 0)
            {
                strMsg = "办证交押金时发现卡号读者竟不存在，办证失败，请重新操作。";
            }
            else if (lRet == 1)
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(results[0]);

                string strId = DomUtil.GetAttr(dom.DocumentElement, "overdues/overdue[@reason='押金。']", "id");
                if (String.IsNullOrEmpty(strId))
                {
                    strMsg = "办证交押金时发现费信息竟不存在，办证失败，请到柜台办理。";
                    goto UNDO;
                }

                string strValue = DomUtil.GetAttr(dom.DocumentElement, "overdues/overdue[@reason='押金。']", "price");
                strValue = PriceUtil.OldGetPurePrice(strValue);
                float value = float.Parse(strValue);

                float foregiftValue = float.Parse(strForegiftValue);
                if (value != foregiftValue)
                {
                    strMsg = "您放入的金额是：" + strForegiftValue + "，而您需要交的押金金额为：" + value.ToString();
                    goto UNDO;
                }


                AmerceItem item = new AmerceItem();
                item.ID = strId;
                AmerceItem[] amerce_items = { item };

                AmerceItem[] failed_items = null;
                string strReaderXml = "";
                lRet = channel.Amerce(null,
                    "amerce",
                    strReaderBarcode,
                    amerce_items,
                    out failed_items,
                    out strReaderXml,
                    out strError);
                if (lRet == -1 && lRet == 1)
                {
                    strMsg = "办证时收押金失败，办证失败，请到柜台办理。";
                    goto UNDO;
                }

                nRet = 1;
            }
            else // lRet > 1
            {
                strMsg = "办证交押金时发现卡号为【" + strReaderBarcode + "】读者存在多条，办证失败，请到柜台办理。";
            }

            this.ReturnChannel(channel);
            return nRet;
            UNDO:
            lRet = DeleteReader(strRecPath, baTimestamp, out strError);
            if (lRet == -1)
                strError = "办证过程中交费发生错误（回滚失败）：" + strError;
            else
                strError = "办证过程中交费发生错误（回滚成功）";
            this.ReturnChannel(channel);
            return nRet;
        }
        #endregion

        long DeleteReader(string strRecPath,
            byte[] baTimestamp,
            out string strError)
        {
            LibraryChannel channel = this.GetChannel(this._dp2username);

            string strExistingXml = "";
            string strSavedXml = "";
            string strSavedRecPath = "";
            byte[] baNewTimestamp = null;
            ErrorCodeValue kernel_errorcode = ErrorCodeValue.NoError;
            long lRet = channel.SetReaderInfo(null,
                "forcedelete",
                strRecPath,
                "", // strNewXml, 
                "", // strOldXml, 
                baTimestamp,
                out strExistingXml,
                out strSavedXml,
                out strSavedRecPath,
                out baNewTimestamp,
                out kernel_errorcode,
                out strError);

            this.ReturnChannel(channel);

            return lRet;
        }

        int ChangePassword(string strSIP2Package,
            out string strBackMsg,
            out string strError)
        {
            strError = "";

            int nRet = 0;

            string strBarcode = "";
            string strOldPassword = "";
            string strNewPassword = "";
            string strOperation = "";

            string[] parts = strSIP2Package.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length < 2)
                    continue;

                string strValue = part.Substring(2);
                switch (part.Substring(0, 2))
                {
                    case "AA":
                        strBarcode = strValue;
                        break;
                    case "AD":
                        strOldPassword = strValue;
                        break;
                    case "KD":
                        strNewPassword = strValue;
                        break;
                    case "XK":
                        strOperation = strValue;
                        break;
                }
            }
            StringBuilder sb = new StringBuilder(1024);
            sb.Append("82").Append(SIPUtility.NowDateTime).Append("AOdp2Library");
            sb.Append("|AA").Append(strBarcode);
            sb.Append("|XK").Append(strOperation);

            LibraryChannel channel = this.GetChannel(this._dp2username);
            string strMsg = "";
            long lRet = channel.ChangeReaderPassword(null,
                strBarcode,
                strOldPassword,
                strNewPassword,
                out strError);
            if (lRet == -1)
            {
                nRet = 0;
                strMsg = "修改密码过程中发生错误，请稍后再试。";
            }
            else if (lRet == 0)
            {
                nRet = 0;
                strMsg = "旧密码输入错误，请重新输入。";
            }
            else
            {
                nRet = 1;
                strMsg = "读者修改密码成功！";
            }
            sb.Append("|OK").Append(nRet.ToString());
            sb.Append("|AF").Append(strMsg).Append("|AG").Append(strMsg);
            strBackMsg = sb.ToString();

            this.ReturnChannel(channel);
            return nRet;
        }


        static string RegularString(string input)
        {
            string strResult = input;
            strResult = strResult.Replace("\r", "*").Replace("\n", "*").Replace("|", "*");
            return strResult;
        }

        #endregion
    }
}
