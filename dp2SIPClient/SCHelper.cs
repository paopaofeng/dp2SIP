using DigitalPlatform;
using DigitalPlatform.SIP2;
using DigitalPlatform.SIP2.Request;
using DigitalPlatform.SIP2.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace dp2SIPClient
{
    public class SCHelper
    {
        private TcpClient _client;
        private NetworkStream _networkStream;

        public bool IsLogin { get; set; }

        #region 单一实例

        static SCHelper _instance;
        private SCHelper()
        {
        }
        private static object _lock = new object();
        static public SCHelper Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_lock)  //线程安全的
                    {
                        _instance = new SCHelper();
                    }
                }
                return _instance;
            }
        }
        #endregion


        public string SIPServerUrl { get; set; }
        public int SIPServerPort { get; set; }

        public bool Connection(string serverUrl, int port,out string error)
        {
            error = "";

            this.SIPServerUrl = serverUrl;
            this.SIPServerPort = port;

            // 先进行关闭
            this.Close();

            try
            {
                IPAddress ipAddress = IPAddress.Parse(this.SIPServerUrl);
                string hostName = Dns.GetHostEntry(ipAddress).HostName;
                _client = new TcpClient(hostName, this.SIPServerPort);

                _networkStream = _client.GetStream();
            }
            catch (Exception ex)
            {
                error= "连接服务器失败:" + ex.Message;
                return false;
            }

            this.IsLogin = false;
            return true;
        }

        // 关闭连接
        public void Close()
        {
            if (_client != null)
            {
                this._networkStream.Close();
                this._client.Close();

                this._networkStream = null;
                this._client = null;
            }
        }

        // 发送数据
        public int SendAndRecvMessage(string requestText, 
            out BaseMessage response, 
            out string responseText, 
            out string error)
        {
            error = "";
            response = null;
            responseText = "";
            int nRet = 0;


            // 校验消息
            BaseMessage request = null;
            nRet = SIPUtility.ParseMessage(requestText, out request, out error);
            if (nRet == -1)
            {
                error = "校验发送消息异常:" + error;
                return -1;
            }

            // 发送消息
            nRet = this.SendMessage(requestText, out error);
            if (nRet == -1)
            {
                error = "发送消息出错:" + error;
                return -1;
            }

            // 接收消息
            nRet = RecvMessage(out responseText, out error);
            if (nRet == -1)
            {
                error = "接收消息出错:" + error;
                return -1;
            }

            //解析返回的消息
            nRet = SIPUtility.ParseMessage(responseText, out response, out error);
            if (nRet == -1)
            {
                error="解析返回的消息异常:" + error +"\r\n"+responseText;
                return-1;
            }

            return 0;
        }

        // 发送数据
        public int SendMessage(string sendMsg,  out string error)
        {
            error = "";

            try
            {

                if (this._networkStream.DataAvailable == true)
                {
                    error = "异常：发送前发现流中有未读的数据!";
                    return -1;
                }

                byte[] baPackage = SIPUtility.Encoding_UTF8.GetBytes(sendMsg);
                this._networkStream.Write(baPackage, 0, baPackage.Length);
                this._networkStream.Flush();//刷新当前数据流中的数据

                return 0;
            }
            catch (Exception ex)
            {
                error =  ex.Message;
                return -1;
            }

        }


        // 接收数据
        public int RecvMessage(out string recvMsg,
            out string strError)
        {
            strError = "";
            recvMsg = "";

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

                    strError = "Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }
                catch (Exception ex)
                {
                    strError = "Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }

                if (wRet == 0)
                {
                    //return 0;
                    strError = "Closed by remote peer";
                    goto ERROR1;
                }

                // 得到包的长度
                if (wRet >= 1 || offset >= 1)
                {
                    //没有找到结束符，继续读
                    int nRet = Array.IndexOf(baPackage, (byte)SIPConst.Terminator);
                    if (nRet != -1)
                    {
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

            recvMsg = SIPUtility.Encoding_UTF8.GetString(baPackage);
            return 0;

        ERROR1:
            this.Close();
            baPackage = null;
            return -1;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="error"></param>
        /// <returns>
        /// 1 登录成功
        /// 0 登录失败
        /// -1 出错
        /// </returns>
        public int Login(string username,string password,out string error)
        {
            error = "";
            int nRet = 0;

            Login_93 request = new Login_93()
            {
                CN_LoginUserId_r=username,
                CO_LoginPassword_r=password,
            };
            request.SetDefaulValue();

            // 发送和接收消息
            string requestText = request.ToText();
            string responseText = "";
            BaseMessage response = null;
            nRet = SCHelper.Instance.SendAndRecvMessage(requestText,
                out response,
                out responseText,
                out error);
            if (nRet == -1)
                return -1;

            LoginResponse_94 response94 = response as LoginResponse_94;
            if (response94 == null)
            {
                error = "返回的不是94消息";
                return -1;
            }

            if (response94.Ok_1 == "0")
            {
                return 0;
            }

            this.IsLogin = true;

            return 1;
        }

        /// <summary>
        /// 借书
        /// </summary>
        /// <param name="patronBarcode"></param>
        /// <param name="itemBarcode"></param>
        /// <param name="error"></param>
        /// <returns>
        /// 1 借书成功
        /// 0 借书失败
        /// -1 出错
        /// -2 尚未登录,需要外部自动测试中断
        /// </returns>
        public int Checkout(string patronBarcode, string itemBarcode, out string error)
        {
            error = "";
            int nRet = 0;

            if (this.IsLogin == false)
            {
                error = "尚未登录ASC系统";
                return -2;
            }

            Checkout_11 request = new Checkout_11()
            {
                TransactionDate_18 = SIPUtility.NowDateTime,
                AA_PatronIdentifier_r = patronBarcode,
                AB_ItemIdentifier_r = itemBarcode,
                AO_InstitutionId_r = SIPConst.AO_Value,
            };
            request.SetDefaulValue();//设置其它默认值

            // 发送和接收消息
            string requestText = request.ToText();
            string responseText = "";
            BaseMessage response = null;
            nRet = SendAndRecvMessage(requestText,
                out response,
                out responseText,
                out error);
            if (nRet == -1)
                return -1;

            CheckoutResponse_12 response12 = response as CheckoutResponse_12;
            if (response12 == null)
            {
                error="返回的不是12消息";
                return -1;
            }

            if (response12.Ok_1 == "0")
            {
                return 0;
            }
            
            return 1;
        }

    }
}
