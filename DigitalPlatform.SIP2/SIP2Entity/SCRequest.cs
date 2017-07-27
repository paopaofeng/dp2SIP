using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.SIP2Entity
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
        public string UIDAlgorithm_1 = "";// 1-char, fixed-length required field; the algorithm used to encrypt the user id.
        public string PWDAlgorithm_1 = "";// 1-char, fixed-length required field; the algorithm used to encrypt the password.
        public string loginUserId_CN_r = null;// variable-length required field
        public string loginPassword_CO_r = null;// variable-length required field
        public string locationCode_CP_o = null;// variable-length optional field; the SC location.
       
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
            this.loginUserId_CN_r = p_loginUserId_CN_r;

            if (p_loginPassword_CO_r == null)
                throw new Exception("loginPassword_CO不能为null");
            this.loginPassword_CO_r = p_loginPassword_CO_r;

            this.locationCode_CP_o = p_locationCode_CP_o;
        }

        // 解析字符串命令为对象
        public override bool parse(string text, out string error)
        {
            error = "";

            if (text.Length == 0)
            {
                error = "命令为空字符串。";
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
                    this.loginUserId_CN_r = value;
                }
                else if (fieldId == "CO")
                {
                    this.loginPassword_CO_r = value;
                }
                else if (fieldId == "CP")
                {
                    this.locationCode_CP_o = value;
                }
                else if (fieldId == "AY")
                {
                    this.AYAZ = part;
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

        // 将对象转换字符串命令
        public override string ToText()
        {
            string text = "93";

            text += this.UIDAlgorithm_1;
            text += this.PWDAlgorithm_1;
            
            if (this.loginUserId_CN_r != null)
                text += "CN" + this.loginUserId_CN_r + "|";

            if (this.loginPassword_CO_r != null)
                text += "CO" + this.loginPassword_CO_r + "|";

            if (this.locationCode_CP_o != null)
                text += "CP" + this.locationCode_CP_o + "|";


            return text;
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

            if (this.loginUserId_CN_r == null)
            {
                error = "缺必备字段CN";
                goto ERROR1;
            }
            if (this.loginPassword_CO_r == null)
            {
                error = "缺必备字段CO";
                goto ERROR1;
            }
            return true;

        ERROR1:

            return false;
        }

    }

/*
SC Status
The SC status message sends SC status to the ACS.  It requires an ACS Status Response message reply from the ACS. This message will be the first message sent by the SC to the ACS once a connection has been established (exception: the Login Message may be sent first to login to an ACS server program). The ACS will respond with a message that establishes some of the rules to be followed by the SC and establishes some parameters needed for further communication.
99<status code><max print width><protocol version>
 */
    public class SCStatus_99 : BaseRequest
    {
        public string statusCode_1 = "";// 1-char, fixed-length required field: 0 or 1 or 2
        public string maxPrintWidth_3 = "";// 3-char, fixed-length required field
        public string protocolVersion_4 = "";// 4-char, fixed-length required field:  x.xx

        // 解析字符串命令为对象
        public override bool parse(string text, out string error)
        {
            error = "未实现参数校验";
            bool ret = false;

            return ret;
        }

        // 将对象转换字符串命令
        public override string ToText()
        {
            return "未实现";
        }

        // 校验对象的各参数是否合法
        public override bool Verify(out string error)
        {
            error = "未实现";
            return false;
        }
    }

/*
Checkout
This message is used by the SC to request to check out an item, and also to cancel a Checkin request that did not successfully complete.  The ACS must respond to this command with a Checkout Response message.
11<SC renewal policy><no block><transaction date><nb due date><institution id><patron identifier><item identifier><terminal password><patron password><item properties><fee acknowledged><cancel>
 */
    public class Checkout_11 : BaseRequest
    {
        public string SCRenewalPolicy_1 = ""; //1-char,fixed-length required field:  Y or N.
        public string noBlock_1 = "";             //1-char, fixed-length required field:  Y or N.

        //The date and time that the patron checked out the item at the SC unit.
        public string transactionDate_18 = ""; //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS.  

        //当noBlock为N时，该值为空。
        public string nbDueDate_18 = "";        //18-char,fixed-length required field:  YYYYMMDDZZZZHHMMSS

        //图书馆的机构ID,目前传的dp2Library
        public string institutionId_AO_r = ""; //variable-length required field

        //读者证条码号
        public string patronIdentifier_AA_r = ""; //variable-length required field 

        //册条码号
        public string itemIdentifier_AB_r = "";   //variable-length required field

        //This is the password for the SC unit.  目前该字段传空值
        public string terminalPassword_AC_r = "";//variable-length required field

        //In current applications, this field is not used. 目前不传这个字段
        public string itemProperties_CH_o = "";//variable-length optional field

        // 读者密码，目前不传这个字段
        public string patronPassword_AD_o = "";//variable-length optional field

        //目前传的N
        public string feeAcknowledged_BO_o = "";//1-char, optional field: Y or N

        // 当Checkout此参数传Y时，则取消上一个错误的Checkin
        //当Checkin此参数传Y时，则取消上一个错误的Checkout
        //普通的Checkout与Checkin，此参数都就传N。
        public string cancel_BI_o = ""; //1-char,optional field: Y or N


    }

    /*
     Checkin
     This message is used by the SC to request to check in an item, and also to cancel a Checkout request that did not successfully complete.  
     The ACS must respond to this command with a Checkin Response message.
     09<no block><transaction date><return date><current location><institution id><item identifier><terminal password><item properties><cancel>
     */
    public class Checkin_09 : BaseRequest
    {
        public string noBlock_1 = "";             //1-char, fixed-length required field:  Y or N.
        public string transactionDate_18 = "";  //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string returnDate_18 = "";         //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string currentLocation_AP_r = "";//variable-length required field
        public string institutionId_AO_r = "";     //variable-length required field
        public string itemIdentifier_AB_r = "";     //variable-length required field
        public string terminalPassword_AC_r = "";//variable-length required field
        public string itemProperties_CH_o = "";    //variable-length optional field
        public string cancel_BI_o = "";                 //1-char, optional field: Y or N
    }

/*
2.00 Patron Information
This message is a superset of the Patron Status Request message.  It should be used to request patron information.  The ACS should respond with the Patron Information Response message.
63<language><transaction date><summary><institution id><patron identifier><terminal password><patron password><start item><end item>
 */
    public class PatronInformation_63 : BaseRequest
    {
        public string language_3 = "";//3-char, fixed-length required field
        public string transactionDate_18 = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string summary_10 = "";//10-char, fixed-length required field
        public string institutionId_AO_r = "";// variable-length required field
        public string patronIdentifier_AA_r = "";// variable-length required field
        public string terminalPassword_AC_o = "";// variable-length optional fiel d
        public string patronPassword_AD_o = "";// variable-length optional field
        public string startItem_BP_o = "";// variable-length optional field
        public string endItem_BQ_o = "";//variable-length optional field
    }

    /*
 2.00 End Patron Session
 This message will be sent when a patron has completed all of their transactions.  The ACS may, upon receipt of this command, close any open files or deallocate data structures pertaining to that patron. The ACS should respond with an End Session Response message.
 35<transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class EndPatronSession_35 : BaseRequest
    {
        public string transactionDate_18 = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";// variable-length required field
        public string patronIdentifier_AA_r = "";// variable-length required field.
        public string terminalPassword_AC_o = "";// variable-length optional field
        public string patronPassword_AD_o = "";// variable-length optional field
    }

 /*
 Item Information
 This message may be used to request item information.  The ACS should respond with the Item Information Response message.
 17<transaction date><institution id>< item identifier ><terminal password>
 */
    public class ItemInformation_17 : BaseRequest
    {
        public string transactionDate_18 = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";// variable-length required field
        public string itemIdentifier_AB_r = "";// variable-length required field.
        public string terminalPassword_AC_o = "";// variable-length optional fiel d
    }

 /*
2.00 Renew
This message is used to renew an item.  The ACS should respond with a Renew Response message. Either or both of the “item identifier” and “title identifier” fields must be present for the message to be useful.
29<third party allowed><no block><transaction date><nb due date><institution id><patron identifier><patron password><item identifier><title identifier><terminal password><item properties><fee acknowledged>
 */
    public class Renew_29 : BaseRequest
    {
        public string thirdPartyAllowed_1 = "";//1-char, fixed-length required field:  Y or N.
        public string noBlock_1 = "";//1-char, fixed-length required field:  Y or N.
        public string transactionDate_18 = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string nbDueDate_18 = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";//variable-length required field
        public string patronIdentifier_AA_r = "";//variable-length required field
        public string patronPassword_AD_o = "";//variable-length optional field
        public string itemIdentifier_AB_o = "";//variable-length optional field
        public string titleIdentifier_AJ_o = "";//variable-length optional field
        public string terminalPassword_AC_o = "";//variable-length optional field
        public string itemProperties_CH_o = "";//variable-length optional field
        public string feeAcknowledged_BO_o = "";//1-char, optional field: Y or N.
    }

    /*
 Patron Status Request
 This message is used by the SC to request patron information from the ACS. 
 The ACS must respond to this command with a Patron Status Response message.
 23<language><transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class PatronStatusRequest_23:BaseRequest
    {
        public string language_3 = "";                // 3-char, fixed-length required field，Chinese 019
        public string transactionDate_18 = "";      // 18-char, fixed-length required field，YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";      // variable-length required field
        public string patronIentifier_AA_r = "";      // variable-length required field
        public string terminalPassword_AC_r = ""; // variable-length required field
        public string patronPassword_AD_r = "";   // variable-length required field
    }

    /*
 This message requests that the patron card be blocked by the ACS.  This is, for example, sent when the patron is detected tampering with the SC or when a patron forgets to take their card.  The ACS should invalidate the patron’s card and respond with a Patron Status Response message.  The ACS could also notify the library staff that the card has been blocked.
 01<card retained><transaction date><institution id><blocked card msg><patron identifier><terminal password>
     */
    public class BlockPatron_01 : BaseRequest
    {
        public string cardRetained_1="";// 1-char, fixed-length required field:  Y or N.
        public string transactionDate_18="";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";//variable-length required field
        public string blockedCardMsg_AL_r= "";// variable-length required field
        public string patronIdentifier_AA_r= "";// variable-length required field
        public string terminalPassword_AC_r = "";// variable-length required field
    }

 /*
 Request ACS Resend
 This message requests the ACS to re-transmit its last message.  
 It is sent by the SC to the ACS when the checksum in a received message does not match the value calculated by the SC.  The ACS should respond by re-transmitting its last message,  This message should never include a “sequence number” field, even when error detection is enabled, (see “Checksums and Sequence Numbers” below) but would include a “checksum” field since checksums are in use.
 97
 */
    public class RequestACSResend_97 : BaseRequest
    { }


    /*
     Fee Paid
     This message can be used to notify the ACS that a fee has been collected from the patron. The ACS should record this information in their database and respond with a Fee Paid Response message.
     37<transaction date><fee type><payment type><currency type><fee amount><institution id><patron identifier><terminal password><patron password><fee identifier><transaction id>
     */
    public class FeePaid_37 : BaseRequest
    {
        public string transactionDate_18 = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string feeType_2 = "";// 2-char, fixed-length required field (01 thru 99). identifies a fee type to apply  the payment to.
        public string paymentType_2 = "";// 2-char, fixed-length required field (00 thru 99)
        public string currencyType_3 = "";// 3-char, fixed-length required field
        public string feeAmount_BV_r = "";// variable-length required field; the amount paid.
        public string institutionId_AO_r = "";// variable-length required field
        public string patronIdentifier_AA_r = "";// variable-length required field.
        public string terminalPassword_AC_o = "";// variable-length optional field
        public string patronPassword_AD_o = "";// variable-length optional field
        public string feeIdentifier_CG_o = "";// variable-length optional field; identifies a specific fee to apply the payment to.
        public string transactionId_BK_o = "";// variable-length optional field; a transaction id assigned by the payment device.
    }



    /*
     2.00 Item Status Update
     This message can be used to send item information to the ACS, without having to do a Checkout or Checkin operation.  The item properties could be stored on the ACS’s database.  The ACS should respond with an Item Status Update Response message.
     19<transaction date><institution id><item identifier><terminal password><item properties>
     */
    public class ItemStatusUpdate_19 : BaseRequest
    {
        public string transactionDate_18 = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";// variable-length required field
        public string itemIdentifier_AB_r = "";//  variable-length required field
        public string terminalPassword_AC_o = "";//  variable-length optional field
        public string itemProperties_CH_r = "";//  variable-length required field
    }

    /*
    Patron Enable
    This message can be used by the SC to re-enable canceled patrons.  It should only be used for system testing and validation.  The ACS should respond with a Patron Enable Response message.
    25<transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class PatronEnable_25 : BaseRequest
    {
        public string transaction_Date_18 = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";//variable-length required field
        public string patronIdentifier_AA_r = "";//variable-length required field
        public string terminalPassword_AC_o = "";//variable-length optional field
        public string patronPassword_AD_o = "";//variable-length optional field
    }

    /*
    2.00 Hold
    This message is used to create, modify, or delete a hold.  The ACS should respond with a Hold Response message.  Either or both of the “item identifier” and “title identifier” fields must be present for the message to be useful.
    15<hold mode><transaction date><expiration date><pickup location><hold type><institution id><patron identifier><patron password><item identifier><title identifier><terminal password><fee acknowledged>
     */
    public class Hold_15 : BaseRequest
    {
        public string holdMode_1 = "";// 1-char, fixed-length required field  '+'/'-'/'*'  Add, delete, change
        public string transactionDate_18 = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string expirationDate_BW_18 = "";//18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string pickupLocation_BS_o = "";//variable-length, optional field
        public string holdType_BY_o = "";//1-char, optional field
        public string institutionId_AO_r = "";//variable-length required field
        public string patronIdentifier_AA_r = "";//variable-length required field
        public string patronPassword_AD_o = "";//variable-length optional field
        public string itemIdentifier_AB_o = "";//variable-length optional field
        public string titleIdentifier_AJ_o = "";//variable-length optional field
        public string terminalPassword_AC_o = "";//variable-length optional field
        public string feeAcknowledged_BO_o = "";//1-char, optional field: Y or N.
    }

    /*
    2.00 Renew All
    This message is used to renew all items that the patron has checked out.  The ACS should respond with a Renew All Response message.
    65<transaction date><institution id><patron identifier><patron password><terminal password><fee acknowledged>
    */
    public class RenewAll_65 : BaseRequest
    {
        public string transactionDate_18 = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO_r = "";//variable-length required field
        public string patronIdentifier_AA_r = "";//variable-length required field
        public string patronPassword_AD_o = "";//variable-length optional field
        public string terminalPassword_AC_o = "";//variable-length optional field
        public string feeAcknowledged_BO_o = "";//1-char, optional field: Y or N.
    }
}
