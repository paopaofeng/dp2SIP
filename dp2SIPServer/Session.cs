#define NEW
#define Default

using DigitalPlatform;
using DigitalPlatform.IO;
using DigitalPlatform.LibraryClient;
using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.Marc;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace dp2SIPServer
{
    public class Session : IDisposable
    {
        public LibraryChannelPool _channelPool = new LibraryChannelPool();

        TcpClient _client = null;

        string _username
        {
            get;
            set;
        }

        // 命令结束符
        char Terminator
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

        string DateFormat
        {
            get
            {
                string strDateFormat = Properties.Settings.Default.DateFormat;
                if (string.IsNullOrEmpty(strDateFormat) || strDateFormat.Length < 8)
                    strDateFormat = "yyyy-MM-dd";
                return strDateFormat;
            }
        }

        MainForm _mainForm = null;
        public MainForm MainForm
        {
            get
            {
                return this._mainForm;
            }
            set
            {
                this._mainForm = value;
            }
        }


        string _dateTimeNow
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd    HHmmss");
            }
        }

        Encoding Encoding
        {
            get
            {
                return this._mainForm.Encoding;
            }
        }

        public void Dispose()
        {
            if (this._client != null)
            {
                try
                {
                    this._client.Close();
                }
                catch
                {
                }
                this._client = null;
            }

            foreach (LibraryChannel channel in this._channelList)
            {
                if (channel != null)
                    channel.Abort();
            }
        }

        internal Session(TcpClient client)
        {
            this._client = client;
        }


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

#if NEW
        public int RecvTcpPackage(out string strPackage,
            out string strError)
        {
            strError = "";

            strPackage = "";

            int nInLen;
            int wRet = 0;

            Debug.Assert(this._client != null, "client为空");

            byte[] baPackage = new byte[1024];
            nInLen = 0;

            int nLen = 1024; //COMM_BUFF_LEN;

            // long lIdleCount = 0;

            while (nInLen < nLen)
            {
                if (this._client == null)
                {
                    strError = "通讯中断";
                    goto ERROR1;
                }

                try
                {
                    wRet = this._client.GetStream().Read(baPackage,
                        nInLen,
                        baPackage.Length - nInLen);
                }
                catch (SocketException ex)
                {
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
                    strError = "Closed by remote peer";
                    goto ERROR1;
                }

                // 得到包的长度

                if (wRet >= 1 || nInLen >= 1)
                {
                    int nRet = Array.IndexOf(baPackage, (byte)this.Terminator);
                    if (nRet != -1)
                    {
                        // nLen = nInLen + wRet;
                        nLen = nRet;
                        break;
                    }

                    if (this._client.GetStream().DataAvailable == false)
                    {
                        nLen = nInLen + wRet;
                        break;
                    }
                }

                nInLen += wRet;
                if (nInLen >= baPackage.Length)
                {
                    // 扩大缓冲区
                    byte[] temp = new byte[baPackage.Length + 1024];
                    Array.Copy(baPackage, 0, temp, 0, nInLen);
                    baPackage = temp;
                    nLen = baPackage.Length;
                }
            }

            // 最后规整缓冲区尺寸，如果必要的话
            if (baPackage.Length > nLen)
            {
                byte[] temp = new byte[nLen];
                Array.Copy(baPackage, 0, temp, 0, nLen);
                baPackage = temp;
            }

            strPackage = this.Encoding.GetString(baPackage);

            LogManager.Logger.Info("Recv:" + strPackage);

            return 0;
            ERROR1:
            // this.CloseSocket();
            baPackage = null;
            return -1;
        }
#endif

#if OLD
        // 接收请求包
        public int RecvTcpPackage(out string strPackage,
            out string strError)
        {
            strError = "";
            strPackage = "";

            Debug.Assert(client != null, "client为空");

            // StringBuilder sb = new StringBuilder();
            List<byte> list = new List<byte>();
            while (true)
            {
                try
                {
                    NetworkStream stream = client.GetStream();

                    int nRet = stream.ReadByte();
                    if (nRet == -1 || stream.DataAvailable == false)
                        break;
                    /*
                    if (sb.Length > 0)
                        sb.Append(" ");
                    sb.AppendFormat("{0:X2}", nRet);
                    */

                    if (nRet == 0x0a || nRet == 0x0d)
                    {
                        // EndOfCMD += (char)nRet;
                    }
                    else
                    {
                        list.Add((byte)nRet);
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode == 10035)
                    {
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }
                    strError = "Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }
                catch (Exception ex)
                {
                    strError = "Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }
            }

            /*
            if (string.IsNullOrEmpty(EndOfCMD))
                EndOfCMD = "[空]";
            this.WriteToLog("[endof] " + EndOfCMD);
            */

            if (list.Count < 1)
            {
                strError = "[ERROR] Recv出错:接收到的命令为空";
                goto ERROR1;
            }

            byte[] baPackage = new byte[list.Count];
            list.CopyTo(0, baPackage, 0, list.Count);
            strPackage = this.Encoding.GetString(baPackage);
            this.WriteToLog("Recv:" + strPackage);

            // this.WriteToLog("Recv:" + sb.ToString());

            return 0;

            ERROR1:
            strPackage = "";
            return -1;
        }
#endif
        // 发出响应包
        // return:
        //      -1  出错
        //      0   正确发出
        //      1   发出前，发现流中有未读入的数据
        public int SendTcpPackage(string strPackage,
            out string strError)
        {
            strError = "";

            if (_client == null)
            {
                strError = "client尚未初始化";
                return -1;
            }

            StringBuilder msg = new StringBuilder(strPackage);
            char endChar = strPackage[strPackage.Length - 1];
            if (endChar == '|')
                msg.Append("AY4AZ");
            else
                msg.Append("|AY4AZ");

            msg.Append(GetChecksum(strPackage));

            LogManager.Logger.Info("Send:" + msg.ToString());

            msg.Append(this.Terminator);
            strPackage = msg.ToString();
            byte[] baPackage = this.Encoding.GetBytes(strPackage);

            try
            {
                NetworkStream stream = _client.GetStream();
                if (stream.DataAvailable == true)
                {
                    strError = "发送前发现流中有未读的数据";
                    return 1;
                }

                stream.Write(baPackage, 0, baPackage.Length);
                // stream.Flush();

                /*
                StringBuilder sb = new StringBuilder();
                foreach (byte b in baPackage)
                {
                    if (sb.Length > 0)
                        sb.Append(" ");
                    sb.AppendFormat("{0:X2}", b);
                }
                this.WriteToLog("Send:" + sb.ToString());
                */

                return 0;
            }
            catch (Exception ex)
            {
                strError = "Send出错: " + ExceptionUtil.GetDebugText(ex);
                // this.CloseSocket();
                LogManager.Logger.Error(strError);
                return -1;
            }
        }


        string StringToHex(string input)
        {
            StringBuilder sb = new StringBuilder();
            byte[] baPackage = this.Encoding.GetBytes(input);
            foreach (byte b in baPackage)
            {
                if (sb.Length > 0)
                    sb.Append(" ");
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }


        public void CloseSocket()
        {
            if (_client != null)
            {
                try
                {
                    NetworkStream stream = _client.GetStream();
                    stream.Close();
                }
                catch { }

                try
                {
                    _client.Close();
                }
                catch { }

                _client = null;
            }

            foreach (LibraryChannel channel in this._channelList)
            {
                if (channel != null)
                    channel.Abort();
            }
        }

        /// <summary>
        /// Session处理轮回
        /// </summary>
        public void Processing()
        {
            string strPackage = "";
            string strError = "";

            try
            {
                while (true)
                {
                    int nRet = RecvTcpPackage(out strPackage, out strError);
                    if (nRet == -1)
                    {
                        LogManager.Logger.Error(strError);
                        return;
                    }

                    if (strPackage.Length < 2)
                    {
                        LogManager.Logger.Error("命令错误，命令长度不够2位");
                        return;
                    }

                    string strMessageIdentifiers = strPackage.Substring(0, 2);

                    string strReaderBarcode = "";
                    string strItemBarcode = "";
                    string strPassword = "";
                    string[] parts = strPackage.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 1; i < parts.Length; i++)
                    {
                        string part = parts[i];
                        if (part.Length < 2)
                            continue;

                        switch (part.Substring(0, 2))
                        {
                            case "AA":
                                strReaderBarcode = part.Substring(2);
                                break;
                            case "AB":
                                strItemBarcode = part.Substring(2);
                                break;
                            case "AD":
                                strPassword = part.Substring(2);
                                break;
                            default:
                                break;
                        }
                    }
#if DoLogin
                    // 登录到dp2系统
                    nRet = DoLogin(Properties.Settings.Default.Username,
                        Properties.Settings.Default.Password,
                        out strError);
                    if (nRet == -1 || nRet == 0)
                    {
                        LogManager.Logger.Error(strError);
                        return;
                    }
#endif
                    string strBackMsg = "";
                    switch (strMessageIdentifiers)
                    {
                        case "09":
                            {
                                strBackMsg = Return(strItemBarcode);
                                break;
                            }
                        case "11":
                            {
                                strBackMsg = Borrow(false, strReaderBarcode, strItemBarcode, "auto_renew");
                                break;
                            }
                        case "17":
                            {
                                strBackMsg = GetItemInfo(strItemBarcode);
                                break;
                            }
                        case "29":
                            {
                                strBackMsg = Borrow(true, // 续借
                                    "",  //读者条码号为空，续借
                                    strItemBarcode);
                                break;
                            }
                        case "35":
                            {
                                strBackMsg = "36Y" + this._dateTimeNow + "AOdp2Library|AA" + strReaderBarcode;
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
                                strBackMsg = GetReaderInfo(strReaderBarcode, strPassword);
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
                        case "93": // 登录，方便调试，跳过验证终端编号，密码和用户名及用户密码。
                            {
                                strBackMsg = Login(strPackage);
                                break;
                            }
                        case "99":
                            {
                                strBackMsg = "98YYYYYY010003" + this._dateTimeNow + "2.00AOdp2Library|AMdp2Library|BXYYYYYYYYYYYYYYYY|AF连接成功!";
                                break;
                            }
                        default:
                            strBackMsg = "无法识别的命令'" + strMessageIdentifiers + "'";
                            break;
                    }

                    nRet = SendTcpPackage(strBackMsg, out strError);
                    if (nRet == -1)
                    {
                        LogManager.Logger.Error(strError);
                        return;
                    }
                }
            }
            finally
            {
                this.CloseSocket();
            }
        }



        string Login(string strPackage)
        {
            string strBackMsg = "";

            string strUserID = "";
            string strPassword = "";
            string strLocationCode = "";

            string[] parts = strPackage.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                if (part.Length < 2)
                    continue;

                int nRet = part.IndexOf("CN");
                if (nRet != -1)
                {
                    strUserID = part.Substring(nRet + 2);
                }
                else
                {
                    string strFieldID = part.Substring(0, 2);
                    string strContent = part.Substring(2);

                    switch (strFieldID)
                    {
                        case "CO":
                            strPassword = strContent;
                            break;
                        case "CP":
                            strLocationCode = strContent;
                            break;
                        default:
                            break;
                    }
                }
            }

            string strError = "";
            LibraryChannel channel = this.GetChannel(strUserID);
            try
            {
                long lRet = channel.Login(strUserID,
                    strPassword,
                    "type=worker,client=dp2SIPServer|0.01",
                    out strError);
                if (lRet == -1)
                {
                    LogManager.Logger.Error(strError);
                    strBackMsg = "940";
                }
                else if (lRet == 0)
                {
                    LogManager.Logger.Error(strError);
                    strBackMsg = "940";
                }
                else
                {
                    strBackMsg = "941";
                    this._channelPool.BeforeLogin += (sender, e) =>
                    {
                        e.LibraryServerUrl = Properties.Settings.Default.LibraryServerUrl;
                        e.UserName = strUserID;
                        e.Parameters = "type=worker,client=dp2SIPServer|0.01";
                        e.Password = strPassword;
                        e.SavePasswordLong = true;
                    };

                    this._username = strUserID;

                    LogManager.Logger.Info("终端 " + strLocationCode + " : " + strUserID + " 接入");
                }
            }
            finally
            {
                this.ReturnChannel(channel);
            }
            return strBackMsg;
        }

        // 进行登录
        // return:
        //      -1  error
        //      0   登录未成功
        //      1   登录成功
        int DoLogin(string strUserName,
            string strPassword,
            out string strError)
        {
            strError = "";

            LibraryChannel channel = this.GetChannel();

            // return:
            //      -1  error
            //      0   登录未成功
            //      1   登录成功
            long lRet = channel.Login(strUserName,
                strPassword,
                "type=worker,client=dp2SIPServer|0.01",
                out strError);
            if (lRet == -1)
            {
                this.ReturnChannel(channel);
                return -1;
            }
            this._channelPool.BeforeLogin += (sender, e) =>
            {
                e.LibraryServerUrl = Properties.Settings.Default.LibraryServerUrl;
                e.UserName = strUserName;
                e.Parameters = "type=worker,client=dp2SIPServer|0.01";
                e.Password = strPassword;
                e.SavePasswordLong = true;
            };
            this.ReturnChannel(channel);
            return (int)lRet;
        }


        /// <summary>
        /// 借或续借
        /// </summary>
        /// <param name="bRenew">29 命令续借为 true，11 命令续借为 false</param>
        /// <param name="strStyle">[空]或auto_renew 11 命令续借</param>
        /// <param name="strReaderBarcode"></param>
        /// <param name="strItemBarcode"></param>
        /// <param name="strBackMsg"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        string Borrow(bool bRenew,
            string strReaderBarcode,
            string strItemBarcode,
            string strStyle = "")
        {
            string strError = "";

            string strErrorInfo = ""; // dp2Library 接口返回的错误信息。

            if (string.IsNullOrEmpty(strStyle))
                strStyle = "biblio,item";
            else
                strStyle += ",biblio,item";

            int nFlag = 0;

            string strBiblioSummary = "";

            // string strCurrencyType = ""; // 币种
            // string strFeeAmount = "";    // 金额

            // string strLastReturningDate = ""; // 续借前应还日期

            string[] aDupPath = null;
            string[] item_records = null;
            string[] reader_records = null;
            string[] biblio_records = null;
            string strOutputReaderBarcode = "";
            BorrowInfo borrow_info = null;

            LibraryChannel channel = this.GetChannel(this._username);

            long lRet = channel.Borrow(
                null,   // stop,
                bRenew,  // 续借为 true
                strReaderBarcode,    //读者证条码号
                strItemBarcode,     // 册条码号
                null, //strConfirmItemRecPath,
                false,
                null,   // this.OneReaderItemBarcodes,
                strStyle, // auto_renew,biblio,item                   //  "reader,item,biblio", // strStyle,
                "xml:noborrowhistory",  // strItemReturnFormats,
                out item_records,
                "summary",    // strReaderFormatList
                out reader_records,
                "xml",         //strBiblioReturnFormats,
                out biblio_records,
                out aDupPath,
                out strOutputReaderBarcode,
                out borrow_info,
                out strErrorInfo);
            if (lRet == -1)
            {
                nFlag = 0;
                LogManager.Logger.Error("[ERROR] " + strErrorInfo);

                string strBiblioRecPath = "";
                lRet = channel.GetBiblioSummary(null,
                    strItemBarcode,
                    "",
                    "",
                    out strBiblioRecPath,
                    out strBiblioSummary,
                    out strErrorInfo);
                if (lRet == -1)
                    LogManager.Logger.Error(strErrorInfo);
            }
            else
            {
                nFlag = 1;

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(item_records[0]);

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

                    strLastReturningDate = DomUtil.GetElementText(dom.DocumentElement, "lastReturningDate");
                    strLastReturningDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strLastReturningDate, "yyyyMMdd");
                    */
                }
                catch (Exception ex)
                {
                    strError = "册信息解析错误:" + ExceptionUtil.GetExceptionText(ex);
                    LogManager.Logger.Error(strError);
                }

                string strMarcSyntax = "";
                MarcRecord record = MarcXml2MarcRecord(biblio_records[0],
                    out strMarcSyntax,
                    out strError);
                if (record != null)
                {
                    if (strMarcSyntax == "unimarc")
                    {
                        strBiblioSummary = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                    }
                    else if (strMarcSyntax == "usmarc")
                    {
                        strBiblioSummary = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                    }
                }
                else
                {
                    strError = "书目信息解析错误:" + strError;
                    LogManager.Logger.Error(strError);
                }
            }

            strErrorInfo = RegularString(strErrorInfo);

            if (string.IsNullOrEmpty(strBiblioSummary))
                strBiblioSummary = strItemBarcode;

            strBiblioSummary = RegularString(strBiblioSummary); // 替换文件中可能存在的"\r"，"\n"，"|"

            StringBuilder sb = new StringBuilder(1024);
            if (bRenew == false)
                sb.Append("12").Append(nFlag).Append("NNY").Append(this._dateTimeNow).Append("AOdp2Library");
            else
                sb.Append("30").Append(nFlag).Append("YNN").Append(this._dateTimeNow).Append("AOdp2Library");

            sb.Append("|AA").Append(strOutputReaderBarcode).Append("|AB").Append(strItemBarcode);
            if (nFlag == 0)
            {
                if (bRenew == false)
                {
                    // sb.Append("|AF图书《").Append(strBiblioSummary).Append("》借阅失败。").Append(strErrorInfo);
                    sb.Append("|AF借阅失败");// .Append(strErrorInfo);
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】借书：").Append("《").Append(strBiblioSummary).Append("》失败。").Append(strErrorInfo);
                }
                else
                {
                    // sb.Append("|AF图书《").Append(strBiblioSummary).Append("》续借失败。").Append(strErrorInfo);
                    sb.Append("|AF续借失败");// .Append(strErrorInfo);
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】续借图书").Append("《").Append(strBiblioSummary).Append("》失败。").Append(strErrorInfo);
                }
            }
            else // if (nFlag == 1)
            {
                string strLatestReturnTime = DateTimeUtil.Rfc1123DateTimeStringToLocal(borrow_info.LatestReturnTime, this.DateFormat);
                sb.Append("|AJ").Append(strBiblioSummary).Append("|AH").Append(strLatestReturnTime);

                // sb.Append("|BH").Append(strCurrencyType).Append("|BV").Append(strFeeAmount);

                if (bRenew == false)
                {
#if PJST
                    sb.Append("|CHATN").Append("PR").Append(strPrice);
#endif
                    // sb.Append("|AF图书《").Append(strBiblioSummary).Append("》借阅成功。").Append(channel.UserName);
                    sb.Append("|AF借阅成功"); // .Append(channel.UserName);
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】借书 ");
                    sb.Append("《").Append(strBiblioSummary).Append("》成功。");
                }
                else
                {
#if PJST
                    sb.Append("|CM").Append(strLastReturningDate);
#endif
                    // sb.Append("|AF图书《").Append(strBiblioSummary).Append("》续借成功。");
                    sb.Append("|AF续借成功");
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】续借");
                    sb.Append("《").Append(strBiblioSummary).Append("》成功。");
                }
            }

            this.ReturnChannel(channel);

            return sb.ToString();
        }

        /// <summary>
        /// 还书
        /// </summary>
        /// <param name="strItemBarcode"></param>
        /// <param name="strBackMsg"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        string Return(string strItemBarcode)
        {
            int nFlag = 0;

            string strErrorInfo = "";

            string strTitle = "";

            string strLocation = "";
            /*
            string strBookType = "";
            string strPrice = "";

            string strOverduePrice = "";
            */
            string strReturnDate = "";


            string[] item_records = null;
            string[] reader_records = null;
            string[] biblio_records = null;
            string[] aDupPath = null;
            string strOutputReaderBarcode = "";
            ReturnInfo return_info = null;
            string strError = "";

            LibraryChannel channel = this.GetChannel(this._username);
            long lRet = channel.Return(null,
                "return",
                "",    //strReaderBarcode,
                strItemBarcode,
                "", // strConfirmItemRecPath
                false,
                "item,biblio",
                "xml",
                out item_records,
                "xml",
                out reader_records,
                "xml",
                out biblio_records,
                out aDupPath,
                out strOutputReaderBarcode,
                out return_info,
                out strError);
            if (lRet == -1)
            {
                nFlag = 0;
                LogManager.Logger.Error(strError);
                if (channel.ErrorCode == ErrorCode.NotBorrowed)
                    nFlag = 1;

                if (!string.IsNullOrEmpty(strErrorInfo))
                    strErrorInfo += ";";

                strErrorInfo += strError;
            }
            else
            {
                nFlag = 1;
                XmlDocument item_dom = new XmlDocument();
                string strItemXml = item_records[0];
                try
                {
                    item_dom.LoadXml(strItemXml);

                    strLocation = DomUtil.GetElementText(item_dom.DocumentElement, "location");
                    /*
                          strPrice = DomUtil.GetElementText(item_dom.DocumentElement, "price");
                          strBookType = DomUtil.GetElementText(item_dom.DocumentElement, "bookType");
                    */
                    strReturnDate = DomUtil.GetAttr(item_dom.DocumentElement, "borrowHistory/borrower", "returnDate");
                    if (String.IsNullOrEmpty(strReturnDate) == false)
                        strReturnDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strReturnDate, this.DateFormat);
                    else
                        strReturnDate = DateTime.Now.ToString(this.DateFormat);
                }
                catch (Exception ex)
                {
                    strError = "册信息解析错误:" + ExceptionUtil.GetExceptionText(ex);
                    LogManager.Logger.Error(strError);
                }
            }

            /*
                 if (lRet == 1)
                 {
                      XmlDocument overdue_dom = new XmlDocument();
                      try
                      {
                          overdue_dom.LoadXml(return_info.OverdueString);

                          strOverduePrice = DomUtil.GetAttr(overdue_dom.DocumentElement, "price");
                      }
                      catch (Exception ex)
                      {
                           strError = "超期信息解析错误:" + ExceptionUtil.GetExceptionText(ex);
                           this.WriteToLog(strError);
                      }
                }
            */

            if (biblio_records != null && biblio_records.Length > 0)
            {
                string strMarcSyntax = "";
                MarcRecord record = MarcXml2MarcRecord(biblio_records[0], out strMarcSyntax, out strError);
                if (record != null)
                {
                    if (strMarcSyntax == "unimarc")
                        strTitle = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                    else if (strMarcSyntax == "usmarc")
                        strTitle = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                }
                else
                {
                    strError = "书目信息解析错误:" + strError;
                    LogManager.Logger.Error(strError);
                }
            }

            if (lRet != -1 && string.IsNullOrEmpty(strTitle))
            {
                string strBiblioRecPath = "";
                string strSummary = "";
                lRet = channel.GetBiblioSummary(null,
                    strItemBarcode,
                    "",
                    "",
                    out strBiblioRecPath,
                    out strSummary,
                    out strError);
                if (lRet != -1)
                    strTitle = strSummary;
                else
                {
                    if (!string.IsNullOrEmpty(strErrorInfo))
                        strErrorInfo += ";";

                    strErrorInfo += strError;
                }
            }

            strTitle = RegularString(strTitle);

            // ErrorCode code = channel.ErrorCode;

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("10").Append(nFlag.ToString()).Append("YNN").Append(this._dateTimeNow).Append("AOdp2Library");
            sb.Append("|AB").Append(strItemBarcode);

            if (nFlag == 1)
            {
                sb.Append("|AQ").Append(strLocation);
                if (!string.IsNullOrEmpty(strTitle))
                    sb.Append("|AJ").Append(strTitle);
                else
                    sb.Append("|AJ").Append(strItemBarcode);

                sb.Append("|AA").Append(strOutputReaderBarcode);

                // sb.Append("|CK").Append(strBookType);

                // sb.Append("ATN").Append("PR").Append(strPrice);

                // sb.Append("|CLsort bin A1");

                sb.Append("|AF图书《").Append(strTitle).Append("》还回处理成功！").Append(channel.UserName);
                sb.Append("|AG图书《").Append(strTitle).Append("》-- ").Append(strItemBarcode).Append("已于").Append(strReturnDate).Append("归还！");
            }
            else // nFlag == 0
            {
                strErrorInfo = RegularString(strErrorInfo);

                // sb.Append("|CLsort bin A1");
                sb.Append("|AF图书【").Append(strItemBarcode).Append("】");
                if (!string.IsNullOrEmpty(strTitle))
                    sb.Append(" - 《").Append(strTitle).Append("》");
                sb.Append("还回处理错误:").Append(strError);

                sb.Append("|AG图书【").Append(strItemBarcode).Append("】");
                if (!string.IsNullOrEmpty(strTitle))
                    sb.Append(" - 《").Append(strTitle).Append("》");
                sb.Append("还回处理错误:").Append(strError);
            }
            this.ReturnChannel(channel);

            return sb.ToString();
        }

#if NO
        /// <summary>
        /// 借
        /// </summary>
        /// <param name="bRenew"></param>
        /// <param name="strReaderBarcode"></param>
        /// <param name="strItemBarcode"></param>
        /// <param name="strBackMsg"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        int Borrow(bool bRenew,
            string strReaderBarcode,
            string strItemBarcode,
            out string strBackMsg,
            out string strError)
        {
            strBackMsg = "";
            strError = "";

            int nRet = 0;

            int nFlag = 0;

            string strTitle = "";

            string strPrice = "";

            string strLastReturningDate = ""; // 续借前应还日期

            string[] aDupPath = null;
            string[] item_records = null;
            string[] reader_records = null;
            string[] biblio_records = null;
            string strOutputReaderBarcode = "";
            BorrowInfo borrow_info = null;

            LibraryChannel channel = this.GetChannel();

            long lRet = channel.Borrow(
                null,   // stop,
                bRenew,  // 续借为 true
                 strReaderBarcode,    //读者证条码号
                 strItemBarcode,     // 册条码号
                null, //strConfirmItemRecPath,
                false,
                null,   // this.OneReaderItemBarcodes,
                "biblio,item",//  "reader,item,biblio", // strStyle,
                "xml:noborrowhistory",  // strItemReturnFormats,
                out item_records,
                "summary",    // strReaderFormatList
                out reader_records,
                "xml",         //strBiblioReturnFormats,
                out biblio_records,
                out aDupPath,
                out strOutputReaderBarcode,
                out borrow_info,
                out strError);
            if (lRet == -1)
            {
                nRet = -1;
                nFlag = 0;
            }
            else
            {
                nFlag = 1;

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(item_records[0]);

                    strPrice = DomUtil.GetElementText(dom.DocumentElement, "price");

                    strLastReturningDate = DomUtil.GetElementText(dom.DocumentElement, "lastReturningDate");
                    strLastReturningDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strLastReturningDate, "yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    nRet = -1;
                    strError = "册信息解析错误" + ex.Message;
                }


                if (nRet == 0)
                {
                    string strMarcSyntax = "";
                    MarcRecord record = MarcXml2MarcRecord(biblio_records[0],
                        out strMarcSyntax,
                        out strError);
                    if (record != null)
                    {
                        if (strMarcSyntax == "unimarc")
                        {
                            strTitle = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                        }
                        else if (strMarcSyntax == "usmarc")
                        {
                            strTitle = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                        }
                    }
                    else
                    {
                        nRet = -1;
                        strError = "书目信息解析错误:" + strError;
                    }
                }
            }

            StringBuilder sb = new StringBuilder(1024);
            if (bRenew == false)
                sb.Append("12").Append(nFlag.ToString()).Append("NNY20141027    181545AOdp2Library");
            else
                sb.Append("30").Append(nFlag.ToString()).Append("YNN20141028    082239AOdp2Library");

            sb.Append("|AA").Append(strOutputReaderBarcode).Append("|AB").Append(strItemBarcode);
            if (nFlag == 0)
            {
                if (bRenew == false)
                {
                    sb.Append("|AF 图书【").Append(strItemBarcode).Append("】借阅失败！");
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】借书 ").Append("【").Append(strItemBarcode).Append("】失败！");
                }
                else
                {
                    sb.Append("|AF 图书【").Append(strItemBarcode).Append("】续借失败！");
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】续借图书 ").Append("【").Append(strItemBarcode).Append("】失败！");
                }
            }
            else // if (nFlag == 1)
            {
                string strLatestReturnTime = DateTimeUtil.Rfc1123DateTimeStringToLocal(borrow_info.LatestReturnTime, "yyyy-MM-dd");
                sb.Append("|AJ").Append(strTitle).Append("|AH").Append(strLatestReturnTime);

                if (bRenew == false)
                {
                    sb.Append("|CHATN").Append("PR").Append(strPrice);
                    sb.Append("|AF 图书【").Append(strItemBarcode).Append("】借阅成功！应还日期：").Append(strLatestReturnTime);
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】借书 ").Append(strTitle);
                    sb.Append("【").Append(strItemBarcode).Append("】成功，应还日期：").Append(strLatestReturnTime);
                }
                else
                {
                    sb.Append("|CM").Append(strLastReturningDate);

                    sb.Append("|AF 图书【").Append(strItemBarcode).Append("】续借处理成功，应还日期：").Append(strLatestReturnTime);
                    sb.Append("|AG读者【").Append(strOutputReaderBarcode).Append("】续借处理成功，应还日期：").Append(strLatestReturnTime);
                }
            }
            strBackMsg = sb.ToString();

            this.ReturnChannel(channel);

            return nRet;
        }




        /// <summary>
        /// 还书
        /// </summary>
        /// <param name="strItemBarcode"></param>
        /// <param name="strBackMsg"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        int Return(
            // string strReaderBarcode, 
            string strItemBarcode,
            out string strBackMsg,
            out string strError)
        {
            strBackMsg = "";
            strError = "";

            int nRet = 0;

            int nFlag = 0;


            string strTitle = "";

            string strLocation = "";
            string strBookType = "";
            string strPrice = "";

            string strOverduePrice = "";
            string strReturnDate = "";


            string[] item_records = null;
            string[] reader_records = null;
            string[] biblio_records = null;
            string[] aDupPath = null;
            string strOutputReaderBarcode = "";
            ReturnInfo return_info = null;

            LibraryChannel channel = this.GetChannel();
            long lRet = channel.Return(null,
                "return",
                "",    //strReaderBarcode,
                strItemBarcode,
                "", // strConfirmItemRecPath
                false,
                "item,biblio",
                "xml",
                out item_records,
                "xml",
                out reader_records,
                "xml",
                out biblio_records,
                out aDupPath,
                out strOutputReaderBarcode,
                out return_info,
                out strError);
            if (lRet == -1)
            {
                nRet = -1;
                nFlag = 0;
            }
            else
            {
                nFlag = 1;

                XmlDocument item_dom = new XmlDocument();
                string strItemXml = item_records[0];
                try
                {
                    item_dom.LoadXml(strItemXml);


                    strLocation = DomUtil.GetElementText(item_dom.DocumentElement, "location");
                    strPrice = DomUtil.GetElementText(item_dom.DocumentElement, "price");
                    strBookType = DomUtil.GetElementText(item_dom.DocumentElement, "bookType");

                    strReturnDate = DomUtil.GetAttr(item_dom.DocumentElement, "borrowHistory/borrower", "returnDate");
                    if (String.IsNullOrEmpty(strReturnDate) == false)
                        strReturnDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strReturnDate, "yyyy-MM-dd");
                    else
                        strReturnDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    nRet = -1;
                    strError = "册信息解析错误:" + ex.Message;
                }

                if (nRet == 0 && lRet == 1)
                {
                    XmlDocument overdue_dom = new XmlDocument();
                    try
                    {
                        overdue_dom.LoadXml(return_info.OverdueString);

                        strOverduePrice = DomUtil.GetAttr(overdue_dom.DocumentElement, "price");
                    }
                    catch (Exception ex)
                    {
                        nRet = -1;
                        strError = "超期信息解析错误:" + ex.Message;
                    }
                }


                if (nRet == 0)
                {
                    string strMarcSyntax = "";
                    MarcRecord record = MarcXml2MarcRecord(biblio_records[0], out strMarcSyntax, out strError);
                    if (record != null)
                    {
                        if (strMarcSyntax == "unimarc")
                        {
                            strTitle = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                        }
                        else if (strMarcSyntax == "usmarc")
                        {
                            strTitle = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                        }
                    }
                    else
                    {
                        nRet = -1;
                        strError = "书目信息解析错误:" + strError;
                    }
                }
            }

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("10").Append(nFlag.ToString()).Append("YNN").Append(this._dateTimeNow).Append("AOdp2Library");
            sb.Append("|AB").Append(strItemBarcode);

            if (nFlag == 1)
            
                sb.Append("|AQ").Append(strLocation);
                sb.Append("|AJ").Append(strTitle).Append("|AA").Append(strOutputReaderBarcode);
                sb.Append("|CK").Append(strBookType);
                sb.Append("|CF").Append(strOverduePrice); // 超期还书欠费金额
                sb.Append("|CH").Append(strTitle).Append("ATN").Append("PR").Append(strPrice);
                sb.Append("|CLsort bin A1");

                sb.Append("|AF图书【").Append(strItemBarcode).Append("】还回处理成功！").Append(strReturnDate);
                sb.Append("|AG图书").Append(strTitle).Append("[").Append(strItemBarcode).Append("]已于").Append(strReturnDate).Append("归还！");
            }
            else // nFlag == 0
            {
                sb.Append("|CLsort bin A1");
                sb.Append("|AF图书【").Append(strItemBarcode).Append("】还回处理错误！");
                sb.Append("|AG图书【").Append(strItemBarcode).Append("】还回处理错误！");
            }

            strBackMsg = sb.ToString();

            this.ReturnChannel(channel);

            return nRet;
        }
#endif

#if NO
        /// <summary>
        /// 获得读者记录
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <param name="strBackMsg"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        int GetReaderInfo(string strBarcode,
            string strPassword,
            string strStyle,
            out string strBackMsg,
            out string strError)
        {
            strBackMsg = "";
            strError = "";

            int nRet = 0;

            long lRet = 0;

            LibraryChannel channel = this.GetChannel();

            StringBuilder sb = new StringBuilder(1024);

            if (strStyle == "" || strStyle == "readerInfo")
            {
                sb.Append("86").Append(this._dateTimeNow).Append("AOdp2Library");
            }
            else  // strStyle == "borrowInfo"
            {
                sb.Append("64              001").Append(this._dateTimeNow).Append("000600000006000000000008AOdp2Library");
            }
            sb.Append("|AA").Append(strBarcode);

            // strStyle == "readerInfo" && 
            if (String.IsNullOrEmpty(strPassword) == false)
            {
                lRet = channel.VerifyReaderPassword(null,
                    strBarcode,
                    strPassword,
                    out strError);
                if (lRet == -1)
                {
                    sb.Append("|BLN").Append("|CQN");
                    sb.Append("|AF").Append("校验密码过程中发生错误……");
                    sb.Append("|AG").Append("校验密码过程中发生错误……");
                    strBackMsg = sb.ToString();
                    return nRet;
                }
                else if (lRet == 0)
                {
                    sb.Append("|BLN").Append("|CQN");
                    sb.Append("|AF").Append("卡号或密码不正确。");
                    sb.Append("|AG").Append("卡号或密码不正确。");
                    strBackMsg = sb.ToString();
                    return nRet;
                }
            }


            XmlDocument dom = new XmlDocument();
            string[] results = null;
            lRet = channel.GetReaderInfo(
                null,// stop,
                strBarcode, //读者卡号,
                "advancexml",   // this.RenderFormat, // "html",
                out results,
                out strError);
            switch (lRet)
            {
                case -1:
                    nRet = 0;
                    break;
                case 0:
                    nRet = 0;
                    strError = "查无此证";
                    break;
                case 1:
                    {
                        nRet = 1;
                        string strReaderXml = results[0];
                        try
                        {
                            dom.LoadXml(strReaderXml);
                        }
                        catch (Exception ex)
                        {
                            nRet = 0;
                            strError = "读者信息解析错误:" + ExceptionUtil.GetDebugText(ex);
                        }
                        break;
                    }
                default: // lRet > 1
                    nRet = 0;
                    strError = "找到多条读者记录";
                    break;
            }

            if (strStyle == "" || strStyle == "readerInfo")
            {
                sb.Append("|OK").Append(nRet.ToString());
            }

            if (nRet == 0)
            {
                sb.Append("|BLN").Append("|AF查询读者信息失败:").Append(strError).Append("|AG查询读者信息失败!");
            }
            else // nRet == 1
            {
                sb.Append("|AE").Append(DomUtil.GetElementText(dom.DocumentElement, "name"));
                sb.Append("|XT").Append(DomUtil.GetElementText(dom.DocumentElement, "readerType"));

                string strExpireDate = DomUtil.GetElementText(dom.DocumentElement, "expireDate");
                strExpireDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strExpireDate, "yyyyMMdd");
                sb.Append("|XD").Append(strExpireDate);
                if (strStyle == "" || strStyle == "readerInfo")
                {
                    sb.Append("|BP").Append(DomUtil.GetElementText(dom.DocumentElement, "tel"));
                    sb.Append("|BD").Append(DomUtil.GetElementText(dom.DocumentElement, "address"));
                    sb.Append("|XO").Append(DomUtil.GetElementText(dom.DocumentElement, "idCardNumber"));

                    string strCreateDate = DomUtil.GetElementText(dom.DocumentElement, "createDate");
                    strCreateDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strCreateDate, "yyyyMMdd");
                    sb.Append("|XB").Append(strCreateDate);

                    string strDateOfBirth = DomUtil.GetElementText(dom.DocumentElement, "dateOfBirth");
                    strDateOfBirth = DateTimeUtil.Rfc1123DateTimeStringToLocal(strDateOfBirth, "yyyyMMdd");
                    sb.Append("|XH").Append(strDateOfBirth);
                    // sb.Append("|XN"); // 民族
                    sb.Append("|XF").Append(DomUtil.GetElementText(dom.DocumentElement, "comment"));
                    string strGender = DomUtil.GetElementText(dom.DocumentElement, "gender").Trim();
                    if (strGender == "男")
                        strGender = "1";
                    else if (strGender == "女")
                        strGender = "0";
                    else
                        strGender = "1";
                    sb.Append("|XM").Append(strGender).Append("|AF查询读者信息成功!|AG查询读者信息成功!");
                }
                else if (strStyle == "borrowInfo")
                {
                    string strOverduesPrice = "";
                    XmlNodeList nodes = dom.DocumentElement.SelectNodes("overdues/overdue/@price");
                    foreach (XmlNode node in nodes)
                    {
                        string strPrice = node.Value;
                        if (String.IsNullOrEmpty(strPrice))
                            continue;

                        if (String.IsNullOrEmpty(strOverduesPrice) == false)
                            strOverduesPrice += "+";

                        strOverduesPrice += strPrice;
                    }
                    nRet = PriceUtil.SumPrices(strOverduesPrice, out strOverduesPrice, out strError);
                    if (nRet == 0)
                        sb.Append("|CF").Append(strOverduesPrice).Append("|JF").Append(strOverduesPrice);
                    else
                        sb.Append("|CF").Append("CNY0.0").Append("|JF").Append("CNY0.0");

                    // 验证读者：Y表示读者存在，N表示读者不存在
                    sb.Append("|BLY");

                    // CQ验证密码：Y表示读者密码正确，N表示读者密码错误
                    sb.Append("|CQY");

                    // 租金 JE 预付款
                    sb.Append("|JE");

                    // 保证金保证系数
                    sb.Append("|XR");

                    // 所借图书总额
                    string strItemsPrices = "";
                    string barcodes = "";
                    XmlNodeList borrows = dom.DocumentElement.SelectNodes("borrows/borrow");
                    foreach (XmlNode borrow in borrows)
                    {
                        string strItemBarcode = DomUtil.GetAttr(borrow, "barcode");
                        if (String.IsNullOrEmpty(strItemBarcode) == false)
                            barcodes += "|AS" + strItemBarcode;

                        string strPrice = DomUtil.GetAttr(borrow, "price");
                        if (String.IsNullOrEmpty(strPrice) == false)
                        {
                            if (String.IsNullOrEmpty(strItemsPrices) == false)
                                strItemsPrices += "+";

                            strItemsPrices += strPrice;
                        }
                    }

                    sb.Append(barcodes);

                    nRet = PriceUtil.SumPrices(strItemsPrices, out strItemsPrices, out strError);
                    if (nRet == 0)
                        sb.Append("|XC").Append(strItemsPrices);
                    else
                        sb.Append("|XC0.0");

                    // 押金
                    sb.Append("|BV").Append(DomUtil.GetElementText(dom.DocumentElement, "foregift"));

                    // 可借总册数
                    string strCount = DomUtil.GetElementAttr(dom.DocumentElement, "info/item[@name='可借总册数']", "value");
                    sb.Append("|BZ").Append(strCount);

                    string strBorrowsCount = DomUtil.GetElementAttr(dom.DocumentElement, "info/item[@name='当前还可借']", "value");
                    string strMsg = "";
                    if (strBorrowsCount != "0")
                        strMsg += "您在本馆最多可借【" + strCount + "】册，还可以再借【" + strBorrowsCount + "】册。";
                    else
                        strMsg += "您在本馆借书数已达最多可借数【" + strCount + "】，不能继续借了!";
                    sb.Append("|AF").Append(strMsg).Append("|AG").Append(strMsg);
                }
            }

            strBackMsg = sb.ToString();

            this.ReturnChannel(channel);
            return nRet;
        }
#endif

        private string GetReaderInfo(string strBarcode,
            string strPassword)
        {
            long lRet = 0;

            string strBackMsg = "";
            string strError = "";

            LibraryChannel channel = this.GetChannel(this._username);

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("64              001").Append(this._dateTimeNow);

            try
            {
                if (String.IsNullOrEmpty(strPassword) == false)
                {
                    lRet = channel.VerifyReaderPassword(null,
                        strBarcode,
                        strPassword,
                        out strError);
                    if (lRet == -1)
                    {
                        sb.Append("|BLN").Append("|CQN");
                        sb.Append("|AF").Append("校验密码过程中发生错误……");
                        sb.Append("|AG").Append("校验密码过程中发生错误……");
                        strBackMsg = sb.ToString();
                    }
                    else if (lRet == 0)
                    {
                        sb.Append("|BLN").Append("|CQN");
                        sb.Append("|AF").Append("卡号或密码不正确。");
                        sb.Append("|AG").Append("卡号或密码不正确。");
                        strBackMsg = sb.ToString();
                    }
                    return strBackMsg;
                }

                string[] results = null;
                lRet = channel.GetReaderInfo(null,
                    strBarcode, //读者卡号,
                    "advancexml",   // this.RenderFormat, // "html",
                    out results,
                    out strError);
                if (lRet <= -1)
                {
                    LogManager.Logger.Error(strError);
                    sb.Append("|AF查询读者信息失败:系统错误").Append("|AG查询读者信息失败!");
                    return sb.ToString();
                }
                else if (lRet == 0)
                {
                    sb.Append("|BLN");
                    sb.Append("|AF查无此证").Append("|AG查询读者信息失败!");
                    return sb.ToString();
                }
                else if (lRet > 1)
                {
                    sb.Append("|AF证号重复").Append("|AG查询读者信息失败!");
                    return sb.ToString();
                }

                // if(lRet == 1)
                XmlDocument dom = new XmlDocument();
                string strReaderXml = results[0];
                try
                {
                    dom.LoadXml(strReaderXml);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.Error("读者信息解析错误:" + ExceptionUtil.GetDebugText(ex));
                    sb.Append("|AF查询读者信息失败:系统错误").Append("|AG查询读者信息失败!");
                    return sb.ToString();
                }

                // hold items count 4 - char, fixed-length required field -- 预约
                int nHoldItemsCount = 0;
                string holdItems = "";
                XmlNodeList holdItemNodes = dom.DocumentElement.SelectNodes("reservations/request");
                if (holdItemNodes != null)
                    nHoldItemsCount = holdItemNodes.Count;

                sb.Append(nHoldItemsCount.ToString().PadLeft(4, '0'));
                foreach (XmlNode node in holdItemNodes)
                {
                    string strItemBarcode = DomUtil.GetAttr(node, "items");
                    if (string.IsNullOrEmpty(strItemBarcode))
                        continue;

#if Default
                    strItemBarcode = strItemBarcode.Replace(",", "|AS");
                    holdItems += "|AS" + strItemBarcode;
#endif
#if TEST
                    if (!string.IsNullOrEmpty(holdItems))
                        holdItems += ",";

                    holdItems += strItemBarcode;
#endif
                }

                // overdue items count 4 - char, fixed-length required field  -- 超期
                // charged items count 4 - char, fixed-length required field -- 在借
                // int nOverdueItemsCount = 0;
                int nChargedItemsCount = 0;
                XmlNodeList chargedItemNodes = dom.DocumentElement.SelectNodes("borrows/borrow");
                if (chargedItemNodes != null)
                    nChargedItemsCount = chargedItemNodes.Count;

                string chargedItems = "";
                // string overdueItems = "";
                foreach (XmlNode item in chargedItemNodes)
                {
                    string strItemBarcode = DomUtil.GetAttr(item, "barcode");
                    if (string.IsNullOrEmpty(strItemBarcode))
                        continue;
#if Default
                    if (!string.IsNullOrEmpty(strItemBarcode))
                        chargedItems += "|AU" + strItemBarcode;
#endif

#if TEST
                    if (!string.IsNullOrEmpty(chargedItems))
                        chargedItems += ",";

                    chargedItems += strItemBarcode;
#endif
                    /*
                    string strReturningDate = DomUtil.GetAttr(item, "returningDate");
                    DateTime returningDate = DateTimeUtil.FromRfc1123DateTimeString(strReturningDate);
                    if (returningDate > DateTime.Now)
                    {
                        nOverdueItemsCount++;
                        overdueItems += "|AT" + strItemBarcode;
                    }
                    */
                }
                // sb.Append(nOverdueItemsCount.ToString().PadLeft(4, '0'));
                sb.Append("0000");
                sb.Append(nChargedItemsCount.ToString().PadLeft(4, '0'));

                // fine items count 4 - char, fixed-length required field
                sb.Append("0000");
                // recall items count 4 - char, fixed-length required field
                sb.Append("0000");
                // unavailable holds count 4 - char, fixed-length required field
                sb.Append("0000");

                sb.Append("AOdp2Library");
                sb.Append("|AA").Append(strBarcode);
                sb.Append("|AE").Append(DomUtil.GetElementText(dom.DocumentElement, "name"));
#if Default
                if (!string.IsNullOrEmpty(holdItems))
                    sb.Append(holdItems);
                if (!string.IsNullOrEmpty(chargedItems))
                    sb.Append(chargedItems);
#endif

#if TEST

                if (!string.IsNullOrEmpty(holdItems))
                    sb.Append("|AU").Append(holdItems);
                if (!string.IsNullOrEmpty(chargedItems))
                    sb.Append("|AS").Append(chargedItems);

                /*
                sb.Append("|BZ0010");
                sb.Append("|CA0002");
                sb.Append("|CB0003");
                */

                // sb.Append("|CQN");
                // sb.Append("|BV0");
                // sb.Append("|CC0"); // 安徽望湖小学
                // sb.Append("|CA0020");
                // sb.Append("|CB0001");
#endif
                sb.Append("|BLY");

                /*
                if (!string.IsNullOrEmpty(overdueItems))
                    sb.Append(overdueItems);
                */
                // 押金
                // sb.Append("|BV").Append(DomUtil.GetElementText(dom.DocumentElement, "foregift"));

                // 可借总册数
                string strCount = DomUtil.GetElementAttr(dom.DocumentElement, "info/item[@name='可借总册数']", "value");
                string strBorrowsCount = DomUtil.GetElementAttr(dom.DocumentElement, "info/item[@name='当前还可借']", "value");
                sb.Append("|BZ").Append(strBorrowsCount.PadLeft(4, '0'));
                string strMsg = "";
                if (strBorrowsCount != "0")
                    strMsg += "您在本馆最多可借【" + strCount + "】册，还可以再借【" + strBorrowsCount + "】册。";
                else
                    strMsg += "您在本馆借书数已达最多可借数【" + strCount + "】，不能继续借了!";
                sb.Append("|AF").Append(strMsg).Append("|AG").Append(strMsg);
            }
            finally
            {
                this.ReturnChannel(channel);
            }
            return sb.ToString();
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
            LibraryChannel channel = this.GetChannel(this._username);
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
                        MarcRecord record = MarcXml2MarcRecord(strBiblio, out strMarcSyntax, out strError);
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

                sb.Append("18").Append(strBookState).Append("0001").Append(this._dateTimeNow);
                if (nReservations > 0)
                    sb.Append("CF").Append(nReservations);
#if PJST
            if (String.IsNullOrEmpty(strReaderBarcode) == false)
                sb.Append("|AA").Append(strReaderBarcode);
#endif
                sb.Append("|AB").Append(strBarcode);
                if (nRet == 0)
                {
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

#if NO
        /// <summary>
        /// 获得图书信息
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <param name="strBackMsg"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        int GetItemInfo(string strBarcode,
            out string strBackMsg,
            out string strError)
        {
            strBackMsg = "";
            strError = "";

            int nRet = 0;

            string strBookState = "";
            string strReaderBarcode = "";
            string strTitle = "";
            string strAuthor = "";
            string strISBN = "";
            string strIsArrived = "";
            string strBookType = "";
            string strPrice = "";
            string strAccessNo = "";
            // string strDocType = ""; // 文献类型
            string strLocation = "";
            string strReturningDate = "";


            string strItemXml = "";
            string strBiblio = "";
            LibraryChannel channel = this.GetChannel();
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
                        if (String.IsNullOrEmpty(strReaderBarcode))
                            strBookState = "02";
                        else
                            strBookState = "03";
                    }
                    else
                    {
                        if (StringUtil.IsInList("丢失", strItemState))
                            strBookState = "04";

                        if (StringUtil.IsInList("#预约", strItemState))
                            strIsArrived = "1";
                        else
                            strIsArrived = "0";
                    }

                    strPrice = DomUtil.GetElementText(dom.DocumentElement, "price");
                    strBookType = DomUtil.GetElementText(dom.DocumentElement, "bookType");
                    strAccessNo = DomUtil.GetElementText(dom.DocumentElement, "accessNo");
                    strLocation = DomUtil.GetElementText(dom.DocumentElement, "location");
                    strReturningDate = DomUtil.GetElementText(dom.DocumentElement, "returningDate");
                    strReturningDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strReturningDate, "yyyy-MM-dd");


                    string strMarcSyntax = "";
                    MarcRecord record = MarcXml2MarcRecord(strBiblio, out strMarcSyntax, out strError);
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
                    strError = strBarcode + ":读者记录解析错误:" + ex.Message;
                }
            }

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("18").Append(strBookState).Append("0001").Append(this._dateTimeNow).Append("CF00000");

            if (String.IsNullOrEmpty(strReaderBarcode) == false)
                sb.Append("|AA").Append(strReaderBarcode);

            sb.Append("|AB").Append(strBarcode);
            if (nRet == 0)
            {
                sb.Append("|AF").Append("获得图书信息发生错误！").Append("|AG").Append("获得图书信息发生错误！");
            }
            else
            {
                sb.Append("|AJ").Append(strTitle).Append("|AW").Append(strAuthor).Append("|AK").Append(strISBN);
                sb.Append("|RE").Append(strIsArrived).Append("|CK").Append(strBookType).Append("|CH").Append(strPrice);
                sb.Append("|AH").Append(strReturningDate);
                sb.Append("|KC").Append(strAccessNo).Append("|AQ").Append(strLocation).Append("|AF").Append("|AG");
            }

            strBackMsg = sb.ToString();
            this.ReturnChannel(channel);
            return nRet;
        }

#endif

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
                strBackMsg = "82" + this._dateTimeNow + "AOdp2Library|AA" + strBarcode +
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

            LibraryChannel channel = this.GetChannel(this._username);

            StringBuilder sb = new StringBuilder(1024);
            sb.Append("82").Append(this._dateTimeNow).Append("AOdp2Library");

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

            LibraryChannel channel = this.GetChannel(this._username);

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
            LibraryChannel channel = this.GetChannel(this._username);

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
            sb.Append("82").Append(this._dateTimeNow).Append("AOdp2Library");
            sb.Append("|AA").Append(strBarcode);
            sb.Append("|XK").Append(strOperation);

            LibraryChannel channel = this.GetChannel(this._username);
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

        static MarcRecord MarcXml2MarcRecord(string strMarcXml,
            out string strOutMarcSyntax,
            out string strError)
        {
            MarcRecord record = null;

            strError = "";
            strOutMarcSyntax = "";

            string strMARC = "";
            int nRet = MarcUtil.Xml2Marc(strMarcXml,
                false,
                "",
                out strOutMarcSyntax,
                out strMARC,
                out strError);
            if (nRet == 0)
                record = new MarcRecord(strMARC);
            else
                strError = "MarcXml转换错误:" + strError;

            return record;
        }


        #region SIP2
        /// <summary>
        /// To calculate the checksum add each character as an unsigned binary number,
        /// take the lower 16 bits of the total and perform a 2's complement. 
        /// The checksum field is the result represented by four hex digits.
        /// </summary>
        /// <param name="message">
        /// 内容中不包含 校验和(checksum)
        /// </param>
        /// <returns></returns>
        private string GetChecksum(string message)
        {
            string checksum = "";

            try
            {
                ushort sum = 0;
                foreach (char c in message)
                {
                    sum += c;
                }

                ushort checksum_inverted_plus1 = (ushort)(~sum + 1);

                checksum = checksum_inverted_plus1.ToString("X4");
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                checksum = null;
            }
            return checksum;
        }
        #endregion

        static string RegularString(string input)
        {
            string strResult = input;
            strResult = strResult.Replace("\r", "*").Replace("\n", "*").Replace("|", "*");
            return strResult;
        }
    }
}
