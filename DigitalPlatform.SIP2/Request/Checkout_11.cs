using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    Checkout
    This message is used by the SC to request to check out an item, and also to cancel a Checkin request that did not successfully complete.  The ACS must respond to this command with a Checkout Response message.
    11<SC renewal policy><no block><transaction date><nb due date><institution id><patron identifier><item identifier><terminal password><patron password><item properties><fee acknowledged><cancel>
11	1-char	1-char	18-char	18-char	AO	AA	AB	AC AD	CH	BO	BI
     */
    public class Checkout_11 : BaseMessage
    {
        // 构造函数
        public Checkout_11()
        {
            this.CommandIdentifier = "11";

            //==前面的定长字段
            //this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            //this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
        //1-char,fixed-length required field:  Y or N.
        public string SCRenewalPolicy_1{ get; set; }


        //1-char, fixed-length required field:  Y or N.
        public string NoBlock_1{ get; set; }

        //The date and time that the patron checked out the item at the SC unit.
        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS.  
        public string TransactionDate_18{ get; set; }

        //18-char,fixed-length required field:  YYYYMMDDZZZZHHMMSS
        //当noBlock为N时，该值为空。
        public string NbDueDate_18{ get; set; }

        //图书馆的机构ID,目前传的dp2Library
        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //读者证条码号
        //variable-length required field 
        private string _patronIdentifier_AA_r = ""; 
        public string PatronIdentifier_AA_r
        {
            get { return _patronIdentifier_AA_r; }
            set { _patronIdentifier_AA_r = value; }
        }

        //册条码号
        //variable-length required field
        public string ItemIdentifier_AB_r{ get; set; }

        //This is the password for the SC unit.  目前该字段传空值
        //variable-length required field
        public string TerminalPassword_AC_r{ get; set; }

        //In current applications, this field is not used. 目前不传这个字段
        //variable-length optional field
        public string ItemProperties_CH_o{ get; set; }

        // 读者密码，目前不传这个字段
        //variable-length optional field
        public string PatronPassword_AD_o{ get; set; }

        //目前传的N
        //1-char, optional field: Y or N
        public string FeeAcknowledged_BO_1_o{ get; set; }

        // 当Checkout此参数传Y时，则取消上一个错误的Checkin
        //当Checkin此参数传Y时，则取消上一个错误的Checkout
        //普通的Checkout与Checkin，此参数都就传N。
        //1-char,optional field: Y or N
        public string Cancel_BI_1_o{ get; set; }




        public Checkout_11(string p_SCRenewalPolicy_1
            , string p_noBlock_1
            , string p_transactionDate_18

            , string p_nbDueDate_18
            , string p_institutionId_AO_r
            , string p_patronIdentifier_AA_r

            , string p_itemIdentifier_AB_r
            , string p_terminalPassword_AC_r
            , string p_itemProperties_CH_o

            , string p_patronPassword_AD_o
            , string p_feeAcknowledged_BO_1_o
            , string p_cancel_BI_1_o)
        {
            if (p_SCRenewalPolicy_1.Length != 1)
                throw new Exception("SCRenewalPolicy_1字段长度必须是1位");
            this.SCRenewalPolicy_1 = p_SCRenewalPolicy_1;

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



            if (p_itemIdentifier_AB_r == null)
                throw new Exception("itemIdentifier_AB_r不能为null");
            this.ItemIdentifier_AB_r = p_itemIdentifier_AB_r;

            if (p_terminalPassword_AC_r == null)
                throw new Exception("terminalPassword_AC_r不能为null");
            this.TerminalPassword_AC_r = p_terminalPassword_AC_r;

            this.ItemProperties_CH_o = p_itemProperties_CH_o;


            this.PatronPassword_AD_o = p_patronPassword_AD_o;
            this.FeeAcknowledged_BO_1_o = p_feeAcknowledged_BO_1_o;
            this.Cancel_BI_1_o = p_cancel_BI_1_o;
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
                if (String.IsNullOrEmpty(this.SCRenewalPolicy_1)==true)
                {
                    this.SCRenewalPolicy_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.NoBlock_1)==true)
                {
                    this.NoBlock_1 = rest.Substring(0, 1);
                    rest = rest.Substring(1);
                    continue;
                }
                if (String.IsNullOrEmpty(this.TransactionDate_18)==true)
                {
                    this.TransactionDate_18 = rest.Substring(0, 18);
                    rest = rest.Substring(18);
                    continue;
                }
                if (String.IsNullOrEmpty(this.NbDueDate_18)==true)
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
                //AO	AA	AB	AC  AD	CH	BO	BI
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
                else if (fieldId == "AB")
                {
                    this.ItemIdentifier_AB_r = value;
                }
                else if (fieldId == "AC")
                {
                    this.TerminalPassword_AC_r = value;
                }
                else if (fieldId == "AD")
                {
                    this.PatronPassword_AD_o = value;
                }
                else if (fieldId == "CH")
                {
                    this.ItemProperties_CH_o = value;
                }
                else if (fieldId == "BO")
                {
                    this.FeeAcknowledged_BO_1_o = value;
                }
                else if (fieldId == "BI")
                {
                    this.Cancel_BI_1_o = value;
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

        // 校验对象的各参数是否合法
        public override bool Verify(out string error)
        {
            error = "";

            //1-char	1-char	18-char	18-char
            if (this.SCRenewalPolicy_1 == "")
            {
                error = "SCRenewalPolicy_1字段未赋值";
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
            //AO	AA	AB	AC 
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
            string text = "11";

            //1-char	1-char	18-char	18-char
            text += this.SCRenewalPolicy_1;
            text += this.NoBlock_1;
            text += this.TransactionDate_18;
            text += this.NbDueDate_18;

            //AO	AA	AB	AC  AD	CH	BO	BI
            if (this.InstitutionId_AO_r != null)
                text += "AO" + this.InstitutionId_AO_r + "|";

            if (this.PatronIdentifier_AA_r != null)
                text += "AA" + this.PatronIdentifier_AA_r + "|";

            if (this.ItemIdentifier_AB_r != null)
                text += "AB" + this.ItemIdentifier_AB_r + "|";

            if (this.TerminalPassword_AC_r != null)
                text += "AC" + this.TerminalPassword_AC_r + "|";

            if (this.PatronPassword_AD_o != null)
                text += "AD" + this.PatronPassword_AD_o + "|";

            if (this.ItemProperties_CH_o != null)
                text += "CH" + this.ItemProperties_CH_o + "|";

            if (this.FeeAcknowledged_BO_1_o != null)
                text += "BO" + this.FeeAcknowledged_BO_1_o + "|";

            if (this.Cancel_BI_1_o != null)
                text += "BI" + this.Cancel_BI_1_o + "|";

            return text;
        }
        */
    }
}
