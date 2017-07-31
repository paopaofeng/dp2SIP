using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{


    /*
     * ACS Status
     * The ACS must send this message in response to a SC Status message.This message will be the first message sent by the ACS to the SC, since it establishes some of the rules to be followed by the SC and establishes some parameters needed for further communication (exception: the Login Response Message may be sent first to complete login of the SC).
     * 98<on-line status><checkin ok><checkout ok><ACS renewal policy><status update ok><off-line ok><timeout period><retries allowed><date / time sync><protocol version><institution id><library name><supported messages ><terminal location><screen message><print line>
     */
    public class ACSStatus_98 : BaseMessage
    {

        public ACSStatus_98()
        {
            this.CommandIdentifier = "98";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
        // 1-char, fixed-length required field:  Y or N.
        public string OnlineStatus_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string CheckinOk_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string CheckoutOk_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string ACSRenewalPolicy_1{ get; set; }

        //===

        //1-char, fixed-length required field:  Y or N.
        public string StatusUpdateOk_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string OfflineOk_1{ get; set; }

        //3-char, fixed-length required field
        public string TimeoutPeriod_3{ get; set; }

        //3-char, fixed-length required field
        public string RetriesAllowed_3{ get; set; }

        //===
        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string DatetimeSync_18{ get; set; }

        //4-char, fixed-length required field:  x.xx
        public string ProtocolVersion_4{ get; set; }

        // variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length optional field
        public string LibraryName_AM_o{ get; set; }

        //variable-length required field
        public string SupportedMessages_BX_r{ get; set; }

        //variable-length optional field
        public string TerminalLocation_AN_o{ get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o{ get; set; }

        //variable-length optional field
        public string PrintLine_AG_o{ get; set; }


         // 构造函数
        public ACSStatus_98()
        {
        }

        //98<on-line status><checkin ok><checkout ok><ACS renewal policy><status update ok><off-line ok><timeout period><retries allowed>
        public ACSStatus_98(string onlineStatus_1
            , string checkinOk_1
            , string checkoutOk_1
            , string aCSRenewalPolicy_1

            , string statusUpdateOk_1
            , string offlineOk_1
            , string timeoutPeriod_3
            , string retriesAllowed_3

            , string datetimeSync_18
            , string protocolVersion_4
            , string institutionId_AO_r
            , string libraryName_AM_o

            , string supportedMessages_BX_r
            , string terminalLocation_AN_o
            , string screenMessage_AF_o
            , string printLine_AG_o
            )
        {
            if (onlineStatus_1.Length != 1)
                throw new Exception("onlineStatus_1字段长度必须是1位");
            this.OnlineStatus_1 = onlineStatus_1;

            if (checkinOk_1.Length != 1)
                throw new Exception("checkinOk_1字段长度必须是1位");
            this.CheckinOk_1 = checkinOk_1;

            if (checkoutOk_1.Length != 1)
                throw new Exception("checkoutOk_1字段长度必须是1位");
            this.CheckoutOk_1 = checkoutOk_1;

            if (aCSRenewalPolicy_1.Length != 1)
                throw new Exception("aCSRenewalPolicy_1字段长度必须是1位");
            this.ACSRenewalPolicy_1 = aCSRenewalPolicy_1;

            //===
            if (statusUpdateOk_1.Length != 1)
                throw new Exception("statusUpdateOk_1字段长度必须是1位");
            this.StatusUpdateOk_1 = statusUpdateOk_1;

            if (offlineOk_1.Length != 1)
                throw new Exception("offlineOk_1字段长度必须是1位");
            this.OfflineOk_1 = offlineOk_1;

            if (timeoutPeriod_3.Length != 3)
                throw new Exception("timeoutPeriod_3字段长度必须是3位");
            this.TimeoutPeriod_3 = timeoutPeriod_3;

            if (retriesAllowed_3.Length != 3)
                throw new Exception("retriesAllowed_3字段长度必须是3位");
            this.RetriesAllowed_3 = retriesAllowed_3;


            //===
            if (datetimeSync_18.Length != 1)
                throw new Exception("datetimeSync_18字段长度必须是18位");
            this.DatetimeSync_18 = datetimeSync_18;

            if (protocolVersion_4.Length != 1)
                throw new Exception("protocolVersion_4字段长度必须是1位");
            this.ProtocolVersion_4 = protocolVersion_4;

            if (institutionId_AO_r == null)
                throw new Exception("institutionId_AO_r不能为null");
            this.InstitutionId_AO_r = institutionId_AO_r;

            this.LibraryName_AM_o = libraryName_AM_o;

            //===
            if (supportedMessages_BX_r == null)
                throw new Exception("supportedMessages_BX_r不能为null");
            this.SupportedMessages_BX_r = supportedMessages_BX_r;

            this.TerminalLocation_AN_o = terminalLocation_AN_o;
            this.ScreenMessage_AF_o = screenMessage_AF_o;
            this.PrintLine_AG_o = printLine_AG_o;
        }

        // 解析字符串命令为对象
        public override bool parse(string text, out string error)
        {
            error = "";

            if (text == null || text.Length < 2)
            {
                error = "命令字符串为null或长度小于2位";
                return false;
            }
            this.CommandIdentifier = text.Substring(0, 2);  //命令指示符
            string content = text.Substring(2); //内容

            if (this.CommandIdentifier != "98")
            {
                error = "命令指示符必须为98";
                return false;
            }

            string rest = content;
            //处理定长字段
            while (rest.Length > 0)
            {
                if (String.IsNullOrEmpty(this.OnlineStatus_1) == true)
                {
                    this.OnlineStatus_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.CheckinOk_1) == true)
                {
                    this.CheckinOk_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.CheckoutOk_1) == true)
                {
                    this.CheckoutOk_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.ACSRenewalPolicy_1) == true)
                {
                    this.ACSRenewalPolicy_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }

                //===
                if (String.IsNullOrEmpty(this.StatusUpdateOk_1) == true)
                {
                    this.StatusUpdateOk_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.OfflineOk_1) == true)
                {
                    this.OfflineOk_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.TimeoutPeriod_3) == true)
                {
                    this.TimeoutPeriod_3 = rest.Substring(0, 3);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.RetriesAllowed_3) == true)
                {
                    this.RetriesAllowed_3 = rest.Substring(0, 3);
                    rest = rest.Substring(1);
                    continue;
                }

                //==
                if (String.IsNullOrEmpty(this.DatetimeSync_18) == true)
                {
                    this.DatetimeSync_18 = rest.Substring(0, 18);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.ProtocolVersion_4) == true)
                {
                    this.ProtocolVersion_4 = rest.Substring(0, 4);
                    rest = rest.Substring(1);
                    continue;
                }
                
                break;
            }

            //处理变长字段
            string[] parts = rest.Split(new char[] { '|' });
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length < 2)
                {
                    continue;
                }

                //AO	AM	BX	AN	AF	    AG
                string fieldId = part.Substring(0, 2);
                string value = part.Substring(2);
                if (fieldId == "AO")
                {
                    this.InstitutionId_AO_r = value;
                }
                else if (fieldId == "AM")
                {
                    this.LibraryName_AM_o = value;
                }
                else if (fieldId == "BX")
                {
                    this.SupportedMessages_BX_r = value;
                }
                else if (fieldId == "AN")
                {
                    this.TerminalLocation_AN_o = value;
                }
                else if (fieldId == "AF")
                {
                    this.ScreenMessage_AF_o = value;
                }
                else if (fieldId == "AG")
                {
                    this.PrintLine_AG_o = value;
                }                
                else
                {
                    error = "不支持的字段:" + part;
                    return false;
                }
            }

            // 校验;
            bool ret = this.Verify(out error);
            if (ret == false)
                return false;

            return true;

        }
        */


    }
}
