using DigitalPlatform.SIP2.Request;
using DigitalPlatform.SIP2.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2
{
    public class SIPUtility
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        public static ILog Logger = LogManager.GetLogger("dp2SIP2");


        /// <summary>
        /// 将消息字符串 解析 成对应的消息对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="message"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ParseMessage(string cmdText, out BaseMessage message, out string error)
        {
            message = new BaseMessage();
            error = "";

            if (cmdText.Length < 2)
            {
                error = "命令长度不够2位";
                return false;
            }

            string cmdIdentifiers = cmdText.Substring(0, 2);
            //text = text.Substring(2);
            switch (cmdIdentifiers)
            {
                case "93":
                    {
                        message = new Login_93();
                        break;
                    }
                case "94":
                    {
                        message = new LoginResponse_94();
                        break;
                    }
                case "99":
                    {
                        message = new SCStatus_99();
                        break;
                    }
                case "98":
                    {
                        message = new ACSStatus_98();
                        break;
                    }
                case "11":
                    {
                        message = new Checkout_11();
                        break;
                    }
                case "12":
                    {
                        message = new CheckoutResponse_12();
                        break;
                    }
                case "09":
                    {
                        message = new Checkin_09();
                        break;
                    }
                case "10":
                    {
                        message = new CheckinResponse_10();
                        break;
                    }
                case "63":
                    {
                        message = new PatronInformation_63();
                        break;
                    }
                case "64":
                    {
                        message = new  PatronInformationResponse_64 ();
                        break;
                    }
                case "35":
                    {
                        message = new EndPatronSession_35();
                        break;
                    }
                case "36":
                    {
                        message = new EndSessionResponse_36();
                        break;
                    }
                case "17":
                    {
                        message = new ItemInformation_17();
                        break;
                    }
                case "18":
                    {
                        message = new ItemInformationResponse_18();
                        break;
                    }
                case "29":
                    {
                        message = new Renew_29();
                        break;
                    }
                case "30":
                    {
                        message = new RenewResponse_30();
                        break;
                    }
                default:
                    error = "不支持的命令'" + cmdIdentifiers + "'";
                    return false;
            }

            return message.parse(cmdText, out error);

        }


        //public int Borrow()

        #region 通用函数

        /// <summary>
        /// 当前时间
        /// </summary>
        public static string NowDateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd    HHmmss");
            }
        }

        #endregion

    }
}
