using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dp2SIPClient.SIP2Entity
{
    /*
 Patron Status Request
 This message is used by the SC to request patron information from the ACS. 
 The ACS must respond to this command with a Patron Status Response message.
 23<language><transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class PatronStatusRequest_23
    {
        public string language = "";                // 3-char, fixed-length required field，Chinese 019
        public string transactionDate = "";      // 18-char, fixed-length required field，YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";      // variable-length required field
        public string patronIentifier_AA = "";      // variable-length required field
        public string terminalPassword_AC = ""; // variable-length required field
        public string patronPassword_AD = "";   // variable-length required field
    }

    /*
 Checkout
 This message is used by the SC to request to check out an item, and also to cancel a Checkin request that did not successfully complete.  The ACS must respond to this command with a Checkout Response message.
 11<SC renewal policy><no block><transaction date><nb due date><institution id><patron identifier><item identifier><terminal password><patron password><item properties><fee acknowledged><cancel>
     */
    public class Checkout_11
    {
        public string SCRenewalPolicy = ""; //1-char,fixed-length required field:  Y or N.
        public string noBlock = "";             //1-char, fixed-length required field:  Y or N.

        //The date and time that the patron checked out the item at the SC unit.
        public string transactionDate = ""; //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS.  

        //当noBlock为N时，该值为空。
        public string nbDueDate = "";        //18-char,fixed-length required field:  YYYYMMDDZZZZHHMMSS

        //图书馆的机构ID,目前传的dp2Library
        public string institutionId_AO = ""; //variable-length required field

        //读者证条码号
        public string patronIdentifier_AA = ""; //variable-length required field 

        //册条码号
        public string itemIdentifier_AB = "";   //variable-length required field

        //This is the password for the SC unit.  目前该字段传空值
        public string terminalPassword_AC = "";//variable-length required field

        //In current applications, this field is not used. 目前不传这个字段
        public string itemProperties_CH = "";//variable-length optional field

        // 读者密码，目前不传这个字段
        public string patronPassword_AD = "";//variable-length optional field

        //目前传的N
        public string feeAcknowledged_BO = "";//1-char, optional field: Y or N

        // 当Checkout此参数传Y时，则取消上一个错误的Checkin
        //当Checkin此参数传Y时，则取消上一个错误的Checkout
        //普通的Checkout与Checkin，此参数都就传N。
        public string cancel_BI = ""; //1-char,optional field: Y or N


    }

    /*
     Checkin
     This message is used by the SC to request to check in an item, and also to cancel a Checkout request that did not successfully complete.  
     The ACS must respond to this command with a Checkin Response message.
     09<no block><transaction date><return date><current location><institution id><item identifier><terminal password><item properties><cancel>
     */
    public class Checkin_09
    {
        public string noBlock = "";             //1-char, fixed-length required field:  Y or N.
        public string transactionDate ="";  //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string returnDate ="";         //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string currentLocation_AP ="";//variable-length required field
        public string institutionId_AO ="";     //variable-length required field
        public string itemIdentifier_AB="";     //variable-length required field
        public string terminalPassword_AC ="";//variable-length required field
        public string itemProperties_CH ="";    //variable-length optional field
        public string cancel_BI ="";                 //1-char, optional field: Y or N
    }

    /*
 
 This message requests that the patron card be blocked by the ACS.  This is, for example, sent when the patron is detected tampering with the SC or when a patron forgets to take their card.  The ACS should invalidate the patron’s card and respond with a Patron Status Response message.  The ACS could also notify the library staff that the card has been blocked.
 01<card retained><transaction date><institution id><blocked card msg><patron identifier><terminal password>
     */
    public class BlockPatron_01
    {
        public string cardRetained="";// 1-char, fixed-length required field:  Y or N.
        public string transactionDate="";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO ="";//variable-length required field
        public string blockedCardMsg_AL="";// variable-length required field
        public string patronIdentifier_AA="";// variable-length required field
        public string terminalPassword_AC ="";// variable-length required field
    }


    /*
SC Status
 The SC status message sends SC status to the ACS.  It requires an ACS Status Response message reply from the ACS. This message will be the first message sent by the SC to the ACS once a connection has been established (exception: the Login Message may be sent first to login to an ACS server program). The ACS will respond with a message that establishes some of the rules to be followed by the SC and establishes some parameters needed for further communication.
 99<status code><max print width><protocol version>
     */
    public class SCStatus_99
    {
        public string statusCode = "";// 1-char, fixed-length required field: 0 or 1 or 2
        public string maxPrintWidth = "";// 3-char, fixed-length required field
        public string protocolVersion = "";// 4-char, fixed-length required field:  x.xx
    }

    /*
 Request ACS Resend
 This message requests the ACS to re-transmit its last message.  
 It is sent by the SC to the ACS when the checksum in a received message does not match the value calculated by the SC.  The ACS should respond by re-transmitting its last message,  This message should never include a “sequence number” field, even when error detection is enabled, (see “Checksums and Sequence Numbers” below) but would include a “checksum” field since checksums are in use.
 97
     */
    public class RequestACSResend_97
    { }

    /*
      Login
This message can be used to login to an ACS server program.  
The ACS should respond with the Login Response message.  
Whether to use this message or to use some other mechanism to login to the ACS is configurable on the SC.  When this message is used, it will be the first message sent to the ACS.
 93<UID algorithm><PWD algorithm><login user id><login password><location code>
     */
    public class Login_93
    {
        public string UIDAlgorithm = "";// 1-char, fixed-length required field; the algorithm used to encrypt the user id.
        public string PWDAlgorithm = "";// 1-char, fixed-length required field; the algorithm used to encrypt the password.
        public string loginUserId_CN = "";// variable-length required field
        public string loginPassword_CO = "";// variable-length required field
        public string locationCode_CP = "";// variable-length optional field; the SC location.
    }

    /*
 2.00 Patron Information
 This message is a superset of the Patron Status Request message.  It should be used to request patron information.  The ACS should respond with the Patron Information Response message.
 63<language><transaction date><summary><institution id><patron identifier><terminal password><patron password><start item><end item>
     */
    public class PatronInformation_63
    {
        public string language = "";//3-char, fixed-length required field
        public string transactionDate = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string summary = "";//10-char, fixed-length required field
        public string institutionId_AO = "";// variable-length required field
        public string patronIdentifier_AA = "";// variable-length required field
        public string terminalPassword_AC = "";// variable-length optional fiel d
        public string patronPassword_AD = "";// variable-length optional field
        public string startItem_BP = "";// variable-length optional field
        public string endItem_BQ = "";//variable-length optional field
    }

    /*
 2.00 End Patron Session
 This message will be sent when a patron has completed all of their transactions.  The ACS may, upon receipt of this command, close any open files or deallocate data structures pertaining to that patron. The ACS should respond with an End Session Response message.
 35<transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class EndPatronSession_35
    {
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";// variable-length required field
        public string patronIdentifier_AA = "";// variable-length required field.
        public string terminalPassword_AC = "";// variable-length optional field
        public string patronPassword_AD = "";// variable-length optional field
    }

    /*
 Fee Paid
 This message can be used to notify the ACS that a fee has been collected from the patron. The ACS should record this information in their database and respond with a Fee Paid Response message.
 37<transaction date><fee type><payment type><currency type><fee amount><institution id><patron identifier><terminal password><patron password><fee identifier><transaction id>
     */
    public class FeePaid_37
    {
        public string transactionDate = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string feeType = "";// 2-char, fixed-length required field (01 thru 99). identifies a fee type to apply  the payment to.
        public string paymentType = "";// 2-char, fixed-length required field (00 thru 99)
        public string currencyType = "";// 3-char, fixed-length required field
        public string feeAmount_BV = "";// variable-length required field; the amount paid.
        public string institutionId_AO = "";// variable-length required field
        public string patronIdentifier_AA = "";// variable-length required field.
        public string terminalPassword_AC = "";// variable-length optional field
        public string patronPassword_AD = "";// variable-length optional field
        public string feeIdentifier_CG = "";// variable-length optional field; identifies a specific fee to apply the payment to.
        public string transactionId_BK = "";// variable-length optional field; a transaction id assigned by the payment device.
    }

    /*
     Item Information
 This message may be used to request item information.  The ACS should respond with the Item Information Response message.
 17<transaction date><institution id>< item identifier ><terminal password>
     */
    public class ItemInformation_17
    {
        public string transactionDate = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";// variable-length required field
        public string itemIdentifier_AB = "";// variable-length required field.
        public string terminalPassword_AC = "";// variable-length optional fiel d
    }

    /*
 2.00 Item Status Update
 This message can be used to send item information to the ACS, without having to do a Checkout or Checkin operation.  The item properties could be stored on the ACS’s database.  The ACS should respond with an Item Status Update Response message.
 19<transaction date><institution id><item identifier><terminal password><item properties>
     */
    public class ItemStatusUpdate_19
    {
        public string transactionDate = "";// 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";// variable-length required field
        public string itemIdentifier_AB = "";//  variable-length required field
        public string terminalPassword_AC = "";//  variable-length optional field
        public string itemProperties_CH = "";//  variable-length required field
    }

    /*
Patron Enable
 This message can be used by the SC to re-enable canceled patrons.  It should only be used for system testing and validation.  The ACS should respond with a Patron Enable Response message.
 25<transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class PatronEnable_25
    {
        public string transaction_Date = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string terminalPassword_AC = "";//variable-length optional field
        public string patronPassword_AD = "";//variable-length optional field
    }

    /*
     2.00 Hold
 This message is used to create, modify, or delete a hold.  The ACS should respond with a Hold Response message.  Either or both of the “item identifier” and “title identifier” fields must be present for the message to be useful.
 15<hold mode><transaction date><expiration date><pickup location><hold type><institution id><patron identifier><patron password><item identifier><title identifier><terminal password><fee acknowledged>
     */
    public class Hold_15
    {
        public string holdMode = "";// 1-char, fixed-length required field  '+'/'-'/'*'  Add, delete, change
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string expirationDate_BW = "";//18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string pickupLocation_BS = "";//variable-length, optional field
        public string holdType_BY = "";//1-char, optional field
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string patronPassword_AD = "";//variable-length optional field
        public string itemIdentifier_AB = "";//variable-length optional field
        public string titleIdentifier_AJ = "";//variable-length optional field
        public string terminalPassword_AC = "";//variable-length optional field
        public string feeAcknowledged_BO = "";//1-char, optional field: Y or N.
    }

    /*
     2.00 Renew
 This message is used to renew an item.  The ACS should respond with a Renew Response message. Either or both of the “item identifier” and “title identifier” fields must be present for the message to be useful.
 29<third party allowed><no block><transaction date><nb due date><institution id><patron identifier><patron password><item identifier><title identifier><terminal password><item properties><fee acknowledged>
     */
    public class Renew_29
    {
        public string thirdPartyAllowed = "";//1-char, fixed-length required field:  Y or N.
        public string noBlock = "";//1-char, fixed-length required field:  Y or N.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string nbDueDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string patronPassword_AD = "";//variable-length optional field
        public string itemIdentifier_AB = "";//variable-length optional field
        public string titleIdentifier_AJ = "";//variable-length optional field
        public string terminalPassword_AC = "";//variable-length optional field
        public string itemProperties_CH = "";//variable-length optional field
        public string feeAcknowledged_BO = "";//1-char, optional field: Y or N.
    }

    /*
 2.00 Renew All
 This message is used to renew all items that the patron has checked out.  The ACS should respond with a Renew All Response message.
 65<transaction date><institution id><patron identifier><patron password><terminal password><fee acknowledged>
     */
    public class RenewAll_65
    {
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string patronPassword_AD = "";//variable-length optional field
        public string terminalPassword_AC = "";//variable-length optional field
        public string feeAcknowledged_BO = "";//1-char, optional field: Y or N.
    }
}
