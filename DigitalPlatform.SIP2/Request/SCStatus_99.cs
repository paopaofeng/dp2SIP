﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    SC Status
    The SC status message sends SC status to the ACS.  It requires an ACS Status Response message reply from the ACS. This message will be the first message sent by the SC to the ACS once a connection has been established (exception: the Login Message may be sent first to login to an ACS server program). The ACS will respond with a message that establishes some of the rules to be followed by the SC and establishes some parameters needed for further communication.
    99<status code><max print width><protocol version>
     */
    public class SCStatus_99 : BaseMessage
    {
        // 1-char, fixed-length required field: 0 or 1 or 2
        public string StatusCode_1{ get; set; }

        // 3-char, fixed-length required field
        public string MaxPrintWidth_3{ get; set; }

        // 4-char, fixed-length required field:  x.xx
        public string ProtocolVersion_4{ get; set; }

        // 构造函数
        public SCStatus_99()
        { }

        public SCStatus_99(string p_statusCode_1
            , string p_maxPrintWidth_3
            , string p_protocolVersion_4)
        {
            if (p_statusCode_1.Length != 1)
                throw new Exception("statusCode字段长度必须是1位");
            this.StatusCode_1 = p_statusCode_1;

            if (p_maxPrintWidth_3.Length != 3)
                throw new Exception("maxPrintWidth字段长度必须是3位");
            this.MaxPrintWidth_3 = p_maxPrintWidth_3;

            if (p_protocolVersion_4.Length != 4)
                throw new Exception("protocolVersion_4字段长度必须是4位");
            this.ProtocolVersion_4 = p_protocolVersion_4;

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
            string cmdIdentifiers = text.Substring(0, 2);
            text = text.Substring(2);

            //处理定长字段
            string rest = text;
            while (rest.Length > 0)
            {
                if (String.IsNullOrEmpty(this.StatusCode_1)==true)
                {
                    this.StatusCode_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }

                if (String.IsNullOrEmpty(this.MaxPrintWidth_3)==true)
                {
                    this.MaxPrintWidth_3 = rest.Substring(0, 3);
                    rest = rest.Substring(3);
                    continue;
                }

                if (String.IsNullOrEmpty(this.ProtocolVersion_4 )==true)
                {
                    this.ProtocolVersion_4 = rest.Substring(0, 4);
                    rest = rest.Substring(4);
                    continue;
                }

                break;
            }

            // 校验;
            bool ret = this.Verify(out error);
            if (ret == false)
                return false;

            return true;

        }

        // 校验对象的各参数是否合法
        public override bool Verify(out string error)
        {
            error = "";
            if (this.StatusCode_1 == "")
            {
                error = "statusCode字段未赋值";
                goto ERROR1;
            }
            if (this.MaxPrintWidth_3 == "")
            {
                error = "maxPrintWidth字段未赋值";
                goto ERROR1;
            }

            if (this.ProtocolVersion_4 == "")
            {
                error = "protocolVersion字段未赋值";
                goto ERROR1;
            }

            return true;
        ERROR1:

            return false;
        }

        // 将对象转换字符串命令
        public override string ToText()
        {
            string text = "99";

            text += this.StatusCode_1;
            text += this.MaxPrintWidth_3;
            text += this.ProtocolVersion_4;

            return text;
        }


    }
}
