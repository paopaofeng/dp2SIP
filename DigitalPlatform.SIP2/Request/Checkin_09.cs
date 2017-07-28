using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{

    /*
     Checkin
     This message is used by the SC to request to check in an item, and also to cancel a Checkout request that did not successfully complete.  
     The ACS must respond to this command with a Checkin Response message.
     09<no block><transaction date><return date><current location><institution id><item identifier><terminal password><item properties><cancel>
     */
    public class Checkin_09 : BaseRequest
    {
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
        private string _returnDate_18 = "";         
        public string ReturnDate_18
        {
            get { return _returnDate_18; }
            set { _returnDate_18 = value; }
        }

        //variable-length required field
        private string _currentLocation_AP_r = "";
        public string CurrentLocation_AP_r
        {
            get { return _currentLocation_AP_r; }
            set { _currentLocation_AP_r = value; }
        }

        //variable-length required field
        private string _institutionId_AO_r = "";     
        public string InstitutionId_AO_r
        {
            get { return _institutionId_AO_r; }
            set { _institutionId_AO_r = value; }
        }

        //variable-length required field
        private string _itemIdentifier_AB_r = "";     
        public string ItemIdentifier_AB_r
        {
            get { return _itemIdentifier_AB_r; }
            set { _itemIdentifier_AB_r = value; }
        }

        //variable-length required field
        private string _terminalPassword_AC_r = "";
        public string TerminalPassword_AC_r
        {
            get { return _terminalPassword_AC_r; }
            set { _terminalPassword_AC_r = value; }
        }

        //variable-length optional field
        private string _itemProperties_CH_o = "";    
        public string ItemProperties_CH_o
        {
            get { return _itemProperties_CH_o; }
            set { _itemProperties_CH_o = value; }
        }

        //1-char, optional field: Y or N
        private string _cancel_BI_1_o = "";                 
        public string Cancel_BI_1_o
        {
            get { return _cancel_BI_1_o; }
            set { _cancel_BI_1_o = value; }
        }

        // 构造函数
        public Checkin_09()
        { }

        public Checkin_09(string p_noBlock_1
            , string p_transactionDate_18
            , string p_returnDate_18

            , string p_currentLocation_AP_r
            , string p_institutionId_AO_r
            , string p_itemIdentifier_AB_r

            , string p_terminalPassword_AC_r
            , string p_itemProperties_CH_o
            , string p_cancel_BI_1_o)
        {
            if (p_noBlock_1.Length != 1)
                throw new Exception("noBlock_1字段长度必须是3位");
            this.NoBlock_1 = p_noBlock_1;

            if (p_transactionDate_18.Length != 18)
                throw new Exception("transactionDate_18字段长度必须是4位");
            this.TransactionDate_18 = p_transactionDate_18;

            if (p_returnDate_18.Length != 18)
                throw new Exception("returnDate_18字段长度必须是4位");
            this.ReturnDate_18 = p_returnDate_18;


            if (p_currentLocation_AP_r == null)
                throw new Exception("currentLocation_AP_r不能为null");
            this.CurrentLocation_AP_r = p_currentLocation_AP_r;

            if (p_institutionId_AO_r == null)
                throw new Exception("institutionId_AO_r不能为null");
            this.InstitutionId_AO_r = p_institutionId_AO_r;

            if (p_itemIdentifier_AB_r == null)
                throw new Exception("itemIdentifier_AB_r不能为null");
            this.ItemIdentifier_AB_r = p_itemIdentifier_AB_r;

            if (p_terminalPassword_AC_r == null)
                throw new Exception("terminalPassword_AC_r不能为null");
            this.TerminalPassword_AC_r = p_terminalPassword_AC_r;

            this.ItemProperties_CH_o = p_itemProperties_CH_o;
            this.Cancel_BI_1_o = p_cancel_BI_1_o;
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
                if (this.ReturnDate_18 == "")
                {
                    this.ReturnDate_18 = rest.Substring(0, 18);
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
                }
                //AP	AO	AB	AC	CH	BI
                string fieldId = part.Substring(0, 2);
                string value = part.Substring(2);
                if (fieldId == "AP")
                {
                    this.CurrentLocation_AP_r = value;
                }
                else if (fieldId == "AO")
                {
                    this.InstitutionId_AO_r = value;
                }
                else if (fieldId == "AB")
                {
                    this.ItemIdentifier_AB_r = value;
                }
                else if (fieldId == "AC")
                {
                    this.TerminalPassword_AC_r = value;
                }
                else if (fieldId == "CH")
                {
                    this.ItemProperties_CH_o = value;
                }
                else if (fieldId == "BI")
                {
                    this.Cancel_BI_1_o = value;
                }

                /*
                else
                {
                    error = "不支持的字段:" + part;
                    goto ERROR1;
                }
                */
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

            //1-char 18-char	18-char
            if (this.NoBlock_1 == "")
            {
                error = "NoBlock_1字段未赋值";
                goto ERROR1;
            }

            if (this.TransactionDate_18 == "")
            {
                error = "TransactionDate_18字段未赋值";
                goto ERROR1;
            }

            if (this.ReturnDate_18 == "")
            {
                error = "ReturnDate_18字段未赋值";
                goto ERROR1;
            }
            //AP	AO	AB	AC
            if (this.CurrentLocation_AP_r == null)
            {
                error = "缺必备字段AP";
                goto ERROR1;
            }
            if (this.InstitutionId_AO_r == null)
            {
                error = "缺必备字段AO";
                goto ERROR1;
            }
            if (this.ItemIdentifier_AB_r == null)
            {
                error = "缺必备字段AB";
                goto ERROR1;
            }

            if (this.TerminalPassword_AC_r == null)
            {
                error = "缺必备字段AC";
                goto ERROR1;
            }


            return true;
        ERROR1:

            return false;
        }

        // 将对象转换字符串命令
        public override string ToText()
        {
            string text = "09";

            //1-char	18-char	18-char
            text += this.NoBlock_1;
            text += this.TransactionDate_18;
            text += this.ReturnDate_18;

            //AP	AO	AB	AC	CH	BI
            if (this.CurrentLocation_AP_r != null)
                text += "AP" + this.CurrentLocation_AP_r + "|";

            if (this.InstitutionId_AO_r != null)
                text += "AO" + this.InstitutionId_AO_r + "|";

            if (this.ItemIdentifier_AB_r != null)
                text += "AB" + this.ItemIdentifier_AB_r + "|";

            if (this.TerminalPassword_AC_r != null)
                text += "AC" + this.TerminalPassword_AC_r + "|";

            if (this.ItemProperties_CH_o != null)
                text += "CH" + this.ItemProperties_CH_o + "|";

            if (this.Cancel_BI_1_o != null)
                text += "BI" + this.Cancel_BI_1_o + "|";

            return text;
        }

    }
}
