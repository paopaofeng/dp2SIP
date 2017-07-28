﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{

    /*
   2.00 Renew
   This message is used to renew an item.  The ACS should respond with a Renew Response message. Either or both of the “item identifier” and “title identifier” fields must be present for the message to be useful.
   29<third party allowed><no block><transaction date><nb due date><institution id><patron identifier><patron password><item identifier><title identifier><terminal password><item properties><fee acknowledged>
    */
    public class Renew_29 : BaseRequest
    {
        //1-char, fixed-length required field:  Y or N.
        private string _thirdPartyAllowed_1 = "";
        public string ThirdPartyAllowed_1
        {
            get { return _thirdPartyAllowed_1; }
            set { _thirdPartyAllowed_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _noBlock_1 = "";
        public string NoBlock_1
        {
            get { return _noBlock_1; }
            set { _noBlock_1 = value; }
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _nbDueDate_18 = "";
        public string NbDueDate_18
        {
            get { return _nbDueDate_18; }
            set { _nbDueDate_18 = value; }
        }

        //variable-length required field
        private string _institutionId_AO_r = "";
        public string InstitutionId_AO_r
        {
            get { return _institutionId_AO_r; }
            set { _institutionId_AO_r = value; }
        }

        //variable-length required field
        private string _patronIdentifier_AA_r = "";
        public string PatronIdentifier_AA_r
        {
            get { return _patronIdentifier_AA_r; }
            set { _patronIdentifier_AA_r = value; }
        }


        //variable-length optional field
        private string _patronPassword_AD_o = "";
        public string PatronPassword_AD_o
        {
            get { return _patronPassword_AD_o; }
            set { _patronPassword_AD_o = value; }
        }

        //variable-length optional field
        private string _itemIdentifier_AB_o = "";
        public string ItemIdentifier_AB_o
        {
            get { return _itemIdentifier_AB_o; }
            set { _itemIdentifier_AB_o = value; }
        }

        //variable-length optional field
        private string _titleIdentifier_AJ_o = "";
        public string TitleIdentifier_AJ_o
        {
            get { return _titleIdentifier_AJ_o; }
            set { _titleIdentifier_AJ_o = value; }
        }

        //variable-length optional field
        private string _terminalPassword_AC_o = "";
        public string TerminalPassword_AC_o
        {
            get { return _terminalPassword_AC_o; }
            set { _terminalPassword_AC_o = value; }
        }

        //variable-length optional field
        private string _itemProperties_CH_o = "";
        public string ItemProperties_CH_o
        {
            get { return _itemProperties_CH_o; }
            set { _itemProperties_CH_o = value; }
        }

        //1-char, optional field: Y or N.
        private string _feeAcknowledged_BO_1_o = "";
        public string FeeAcknowledged_BO_1_o
        {
            get { return _feeAcknowledged_BO_1_o; }
            set { _feeAcknowledged_BO_1_o = value; }
        }

        // 构造函数
        public Renew_29()
        { }

        public Renew_29(string p_thirdPartyAllowed_1
            , string p_noBlock_1
            , string p_transactionDate_18

            , string p_nbDueDate_18
            , string p_institutionId_AO_r
            , string p_patronIdentifier_AA_r

            , string p_patronPassword_AD_o
            , string p_itemIdentifier_AB_o
            , string p_titleIdentifier_AJ_o

            , string p_terminalPassword_AC_o
            , string p_itemProperties_CH_o
            , string p_feeAcknowledged_BO_1_o
            )
        {
            if (p_thirdPartyAllowed_1.Length != 1)
                throw new Exception("p_thirdPartyAllowed_1字段长度必须是1位");
            this.ThirdPartyAllowed_1 = p_thirdPartyAllowed_1;

            if (p_noBlock_1.Length != 1)
                throw new Exception("noBlock_1字段长度必须是3位");
            this.NoBlock_1 = p_noBlock_1;

            if (p_transactionDate_18.Length != 18)
                throw new Exception("transactionDate_18字段长度必须是4位");
            this.TransactionDate_18 = p_transactionDate_18;



            if (p_nbDueDate_18.Length != 18)
                throw new Exception("nbDueDate_18字段长度必须是4位");
            this.NbDueDate_18 = p_nbDueDate_18;

            if (p_institutionId_AO_r == null)
                throw new Exception("institutionId_AO_r不能为null");
            this.InstitutionId_AO_r = p_institutionId_AO_r;

            if (p_patronIdentifier_AA_r == null)
                throw new Exception("patronIdentifier_AA_r不能为null");
            this.PatronIdentifier_AA_r = p_patronIdentifier_AA_r;


            this.PatronPassword_AD_o = p_patronPassword_AD_o;
            this.ItemIdentifier_AB_o = p_itemIdentifier_AB_o;
            this.TitleIdentifier_AJ_o = p_titleIdentifier_AJ_o;

            this.TerminalPassword_AC_o = p_terminalPassword_AC_o;
            this.ItemProperties_CH_o = p_itemProperties_CH_o;
            this.FeeAcknowledged_BO_1_o = p_feeAcknowledged_BO_1_o;
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
                if (this.ThirdPartyAllowed_1 == "")
                {
                    this.ThirdPartyAllowed_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (this.NoBlock_1 == "")
                {
                    this.NoBlock_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (this.TransactionDate_18 == "")
                {
                    this.TransactionDate_18 = rest.Substring(0, 18);
                    rest = rest.Substring(18);
                    continue;
                }
                if (this.NbDueDate_18 == "")
                {
                    this.NbDueDate_18 = rest.Substring(0, 18);
                    rest = rest.Substring(18);
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
                //AO	AA AD	AB AJ	AC	CH	BO
                string fieldId = part.Substring(0, 2);
                string value = part.Substring(2);
                if (fieldId == "AO")
                {
                    this.InstitutionId_AO_r = value;
                }
                else if (fieldId == "AA")
                {
                    this.PatronIdentifier_AA_r = value;
                }

                else if (fieldId == "AD")
                {
                    this.PatronPassword_AD_o = value;
                }
                else if (fieldId == "AB")
                {
                    this.ItemIdentifier_AB_o = value;
                }
                else if (fieldId == "AJ")
                {
                    this.TitleIdentifier_AJ_o = value;
                }
                //===
                else if (fieldId == "AC")
                {
                    this.TerminalPassword_AC_o = value;
                }
                else if (fieldId == "CH")
                {
                    this.ItemProperties_CH_o = value;
                }
                else if (fieldId == "BO")
                {
                    this.FeeAcknowledged_BO_1_o = value;
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

            //1-char	1-char	18-char	18-char
            if (this.ThirdPartyAllowed_1 == "")
            {
                error = "thirdPartyAllowed_1字段未赋值";
                goto ERROR1;
            }
            if (this.NoBlock_1 == "")
            {
                error = "noBlock_1字段未赋值";
                goto ERROR1;
            }

            if (this.TransactionDate_18 == "")
            {
                error = "transactionDate_18字段未赋值";
                goto ERROR1;
            }

            if (this.NbDueDate_18 == "")
            {
                error = "nbDueDate_18字段未赋值";
                goto ERROR1;
            }
            //AO	AA
            if (this.InstitutionId_AO_r == null)
            {
                error = "缺必备字段AO";
                goto ERROR1;
            }

            if (this.PatronIdentifier_AA_r == null)
            {
                error = "缺必备字段AA";
                goto ERROR1;
            }

            return true;
        ERROR1:

            return false;
        }

        // 将对象转换字符串命令
        public override string ToText()
        {
            string text = "29";

            //1-char	1-char	18-char	18-char
            text += this.ThirdPartyAllowed_1;
            text += this.NoBlock_1;
            text += this.TransactionDate_18;
            text += this.NbDueDate_18;

            //AO	AA AD	AB AJ	AC	CH	BO
            if (this.InstitutionId_AO_r != null)
                text += "AO" + this.InstitutionId_AO_r + "|";

            if (this.PatronIdentifier_AA_r != null)
                text += "AA" + this.PatronIdentifier_AA_r + "|";

            if (this.PatronPassword_AD_o != null)
                text += "AD" + this.PatronPassword_AD_o + "|";
            if (this.ItemIdentifier_AB_o != null)
                text += "AB" + this.ItemIdentifier_AB_o + "|";
            if (this.TitleIdentifier_AJ_o != null)
                text += "AJ" + this.TitleIdentifier_AJ_o + "|";


            if (this.TerminalPassword_AC_o != null)
                text += "AC" + this.TerminalPassword_AC_o + "|";
            if (this.ItemProperties_CH_o != null)
                text += "CH" + this.ItemProperties_CH_o + "|";
            if (this.FeeAcknowledged_BO_1_o != null)
                text += "BO" + this.FeeAcknowledged_BO_1_o + "|";



            return text;
        }
    }
}