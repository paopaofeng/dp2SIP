﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    Login
    This message can be used to login to an ACS server program.  
    The ACS should respond with the Login Response message.  
    Whether to use this message or to use some other mechanism to login to the ACS is configurable on the SC.  When this message is used, it will be the first message sent to the ACS.
    93<UID algorithm><PWD algorithm><login user id><login password><location code>
     */
    public class Login_93 : BaseRequest
    {
        // 1-char, fixed-length required field; the algorithm used to encrypt the user id.
        private string _UIDAlgorithm_1 = "";
        public string UIDAlgorithm_1
        {
            get { return _UIDAlgorithm_1; }
            set { _UIDAlgorithm_1 = value; }
        }

        // 1-char, fixed-length required field; the algorithm used to encrypt the password.
        private string _PWDAlgorithm_1 = "";
        public string PWDAlgorithm_1
        {
            get { return _PWDAlgorithm_1; }
            set { _PWDAlgorithm_1 = value; }
        }

        // variable-length required field
        private string _loginUserId_CN_r = null;
        public string LoginUserId_CN_r
        {
            get { return _loginUserId_CN_r; }
            set { _loginUserId_CN_r = value; }
        }

        // variable-length required field
        private string _loginPassword_CO_r = null;
        public string LoginPassword_CO_r
        {
            get { return _loginPassword_CO_r; }
            set { _loginPassword_CO_r = value; }
        }

        // variable-length optional field; the SC location.
        private string _locationCode_CP_o = null;
        public string LocationCode_CP_o
        {
            get { return _locationCode_CP_o; }
            set { _locationCode_CP_o = value; }
        }

        // 构造函数
        public Login_93()
        { }

        public Login_93(string p_UIDAlgorithm_1
            , string p_PWDAlgorithm_1
            , string p_loginUserId_CN_r
            , string p_loginPassword_CO_r
            , string p_locationCode_CP_o)
        {
            if (p_UIDAlgorithm_1.Length != 1)
                throw new Exception("UIDAlgorithm长度必须是1位");
            this.UIDAlgorithm_1 = p_UIDAlgorithm_1;

            if (p_PWDAlgorithm_1.Length != 1)
                throw new Exception("PWDAlgorithm长度必须是1位");
            this.PWDAlgorithm_1 = p_PWDAlgorithm_1;

            if (p_loginUserId_CN_r == null)
                throw new Exception("loginUserId_CN不能为null");
            this.LoginUserId_CN_r = p_loginUserId_CN_r;

            if (p_loginPassword_CO_r == null)
                throw new Exception("loginPassword_CO不能为null");
            this.LoginPassword_CO_r = p_loginPassword_CO_r;

            this.LocationCode_CP_o = p_locationCode_CP_o;
        }

        // 解析字符串命令为对象
        public override bool parse(string text, out string error)
        {
            error = "";

            if (text == null || text.Length == 0)
            {
                error = "命令字符串为null或长度为0。";
                goto ERROR1;
            }

            //处理定长字段
            string rest = text;
            while (rest.Length > 0)
            {
                if (this.UIDAlgorithm_1 == "")
                {
                    this.UIDAlgorithm_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }

                if (this.PWDAlgorithm_1 == "")
                {
                    this.PWDAlgorithm_1 = rest.Substring(0, 1);
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
                    //error = "发现不足2位的字段:" + part;
                    //goto ERROR1;
                }

                string fieldId = part.Substring(0, 2);
                string value = part.Substring(2);
                if (fieldId == "CN")
                {
                    this.LoginUserId_CN_r = value;
                }
                else if (fieldId == "CO")
                {
                    this.LoginPassword_CO_r = value;
                }
                else if (fieldId == "CP")
                {
                    this.LocationCode_CP_o = value;
                }
                else if (fieldId == "AY")
                {
                    this._AYAZ = part;
                }
                else
                {
                    error = "不支持的字段:" + part;
                    goto ERROR1;
                }
            }

            // 校验;
            bool ret = this.Verify(out error);
            if (ret == false)
                return false;


            return true;

        ERROR1:

            return false;
        }

        // 校验对象的各参数是否合法
        public override bool Verify(out string error)
        {
            error = "";
            if (this.UIDAlgorithm_1 == "")
            {
                error = "UIDAlgorithm字段未赋值";
                goto ERROR1;
            }
            if (this.PWDAlgorithm_1 == "")
            {
                error = "PWDAlgorithm字段未赋值";
                goto ERROR1;
            }

            if (this.LoginUserId_CN_r == null)
            {
                error = "缺必备字段CN";
                goto ERROR1;
            }
            if (this.LoginPassword_CO_r == null)
            {
                error = "缺必备字段CO";
                goto ERROR1;
            }
            return true;

        ERROR1:

            return false;
        }

        // 将对象转换字符串命令
        public override string ToText()
        {
            string text = "93";

            text += this.UIDAlgorithm_1;
            text += this.PWDAlgorithm_1;

            if (this.LoginUserId_CN_r != null)
                text += "CN" + this.LoginUserId_CN_r + "|";

            if (this.LoginPassword_CO_r != null)
                text += "CO" + this.LoginPassword_CO_r + "|";

            if (this.LocationCode_CP_o != null)
                text += "CP" + this.LocationCode_CP_o + "|";


            return text;
        }

    }
}