using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dp2SIPClient.SIP2Entity
{
    /*
Patron Status Response
 The ACS must send this message in response to a Patron Status Request message as well as in response to a Block Patron message.
 24<patron status><language><transaction date><institution id><patron identifier><personal name><valid patron><valid patron password><currency type><fee amount><screen message><print line>
     */
    public class PatronStatusResponse_24
    {
        public string patronStatus = "";//14-char, fixed-length required field
        public string language = "";//3-char, fixed-length required field
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string personalName_AE = "";//variable-length required field
        public string validPatron_BL = "";//1-char, optional field: Y or N
        public string validPatronPassword_CQ = "";//1-char, optional field: Y or N
        public string currencyType_BH = "";//3-char, fixed-length optional field
        public string feeAmount_BV = "";//variable-length optional field.  The amount of fees owed by this patron.
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
Checkout Response
 This message must be sent by the ACS in response to a Checkout message from the SC.
 12<ok><renewal ok><magnetic media><desensitize><transaction date><institution id><patron identifier><item identifier><title identifier><due date><fee type><security inhibit><currency type><fee amount><media type><item properties><transaction id><screen message><print line>
     */
    public class CheckoutResponse_12
    {
        //OK should be set to 1 if the ACS checked out the item to the patron. should be set to 0 if the ACS did not check out the item to the patron.
        public string ok = "";//1-char, fixed-length required field:  0 or 1.

        //Renewal OK should be set to Y if the patron requesting to check out the item already has the item checked out should be set to N if the item is not already checked out to the requesting patron. 
        public string renewalOk = "";//1-char, fixed-length required field:  Y or N.
        public string magneticMedia = "";//1-char, fixed-length required field:  Y or N or U.

        // Desensitize should be set to Y if the SC should desensitize the article. should be set to N if the SC should not desensitize the article (for example, a closed reserve book, or the checkout was refused).
        public string desensitize = "";//1-char, fixed-length required field:  Y or N or U.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string itemIdentifier_AB = "";//variable-length required field
        public string titleIdentifier_AJ = "";//variable-length required field
        public string dueDate_AH = "";//variable-length required field
        public string feeType_BT = "";//2-char, fixed-length optional field (01 thru 99).  The type of fee associated with checking out this item.
        public string securityInhibit_CI = "";//1-char, fixed-length optional field:  Y or N.
        public string currencyType_BH = "";//3-char fixed-length optional field

        //Fee Amount should be set to the value of the fee associated with checking out the item should be set to 0 if there is no fee associated with checking out the item.
        public string feeAmount_BV = "";//variable-length optional field.  The amount of the fee associated with checking out this item.
        public string mediaType_CK = "";//3-char, fixed-length optional field
        public string itemProperties_CH = "";//variable-length optional field
        public string transactionId_BK = "";//variable-length optional field.  May be assigned by the ACS when checking out the item involves a fee.
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
Checkin Response
 This message must be sent by the ACS in response to a SC Checkin message.
 10<ok><resensitize><magnetic media><alert><transaction date><institution id><item identifier><permanent location><title identifier><sort bin><patron identifier><media type><item properties><screen message><print line>
     */
    public class CheckinResponse_10
    {
        //OK should be set to 1 if the ACS checked in the item. should be set to 0 if the ACS did not check in the item.
        public string ok = "";//1-char, fixed-length required field:  0 or 1.

        //Resensitize should be set to Y if the SC should resensitize the article. should be set to N if the SC should not resensitize the article (for example, a closed reserve book, or the checkin was refused).
        public string resensitize = "";//1-char, fixed-length required field:  Y or N.
        public string magneticMedia = "";//1-char, fixed-length required field:  Y or N or U.
        public string alert = "";//1-char, fixed-length required field:  Y or N.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string itemIdentifier_AB = "";//variable-length required field
        public string permanentLocation_AQ = "";//variable-length required field
        public string titleIdentifier_AJ = "";//variable-length optional field
        public string sortBin_CL = "";//variable-length optional field
        public string patronIdentifier_AA = "";//variable-length optional field.  ID of the patron who had the item checked out.
        public string mediaType_CK = "";//3-char, fixed-length optional field
        public string itemProperties_CH = "";//variable-length optional field
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
ACS Status
 The ACS must send this message in response to a SC Status message.  This message will be the first message sent by the ACS to the SC, since it establishes some of the rules to be followed by the SC and establishes some parameters needed for further communication (exception: the Login Response Message may be sent first to complete login of the SC).
 98<on-line status><checkin ok><checkout ok><ACS renewal policy><status update ok><off-line ok><timeout period><retries allowed><date / time sync><protocol version><institution id><library name><supported messages ><terminal location><screen message><print line>
     */
    public class ACSStatus_98
    {
        public string onlineStatus = "";// 1-char, fixed-length required field:  Y or N.
        public string checkinOk = "";//1-char, fixed-length required field:  Y or N.
        public string checkoutOk = "";//1-char, fixed-length required field:  Y or N.
        public string ACSRenewalPolicy = "";//1-char, fixed-length required field:  Y or N.
        public string statusUpdateOk = "";//1-char, fixed-length required field:  Y or N.
        public string offlineOk = "";//1-char, fixed-length required field:  Y or N.
        public string timeoutPeriod = "";//3-char, fixed-length required field
        public string retriesAllowed = "";//3-char, fixed-length required field
        public string datetimeSync = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string protocolVersion = "";//4-char, fixed-length required field:  x.xx
        public string institutionId_AO = "";// variable-length required field
        public string libraryName_AM = "";//variable-length optional field
        public string supportedMessages_BX = "";//variable-length required field
        public string terminalLocation_AN = "";//variable-length optional field
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
Request SC Resend
 This message requests the SC to re-transmit its last message.  It is sent by the ACS to the SC when the checksum in a received message does not match the value calculated by the ACS.  The SC should respond by re-transmitting its last message, This message should never include a “sequence number” field, even when error detection is enabled, (see “Checksums and Sequence Numbers” below) but would include a “checksum” field since checksums are in use.
 96
     */
    public class RequestSCResend_96
    { }

    /*
2.00 Login Response
 The ACS should send this message in response to the Login message.  When this message is used, it will be the first message sent to the SC.
 94<ok>
     */
    public class LoginResponse_94
    {
        public string ok = "";//1-char, fixed-length required field:  0 or 1.
    }



    /*
Patron Information Response
     The ACS must send this message in response to the Patron Information message.
     64<patron status><language><transaction date><hold items count><overdue items count>
     <charged items count><fine items count><recall items count><unavailable holds count><institution id>
     <patron identifier><personal name><hold items limit><overdue items limit><charged items limit>
     <valid patron><valid patron password><currency type><fee amount><fee limit><items>
     <home address><e-mail address><home phone number><screen message><print line>
     */
    public class PatronInformationResponse_64
    {
        public string patronStatus = "";//14-char, fixed-length required field
        public string language = "";//3-char, fixed-length required field
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string holdItemsCount = "";//4-char, fixed-length required field
        public string overdueItemsCount = "";//4-char, fixed-length required field
        public string chargedItemsCount = "";//4-char, fixed-length required field
        public string fineItemsCount = "";//4-char, fixed-length required field
        public string recallItemsCount = "";//4-char, fixed-length required field
        public string unavailableHoldsCount = "";//4-char, fixed-length required field
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string personalName_AE = "";//variable-length required field
        public string holdItemsLimit_BZ = "";//4-char, fixed-length optional field
        public string overdueItemsLimit_CA = "";//4-char, fixed-length optional field
        public string chargedItemsLimit_CB = "";//4-char, fixed-length optional field
        public string validPatron_BL = "";//1-char, optional field:  Y or N
        public string validPatronPassword_CQ = "";//1-char, optional field: Y or N
        public string currencyType_BH = "";//3-char fixed-length optional field
        public string feeAmount_BV = "";//variable-length optional field.  The amount of fees owed by this patron.
        public string feeLimit_CC = "";//variable-length optional field.  The fee limit amount.
        //item: zero or more instances of one of the following, based on “summary” field of the Patron Information message:
        public string holdItems_AS = "";//variable-length optional field  (this field should be sent for each hold item).
        public string overdueItems_AT = "";//variable-length optional field  (this field should be sent for each overdue item).
        public string chargedItems_AU = "";//variable-length optional field  (this field should be sent for each charged item).
        public string fineItems_AV = "";//variable-length optional field  (this field should be sent for each fine item).
        public string recallItems_BU = "";//variable-length optional field  (this field should be sent for each recall item). 
        public string unavailableHoldItems_CD = "";//variable-length optional field  (this field should be sent for each unavailable hold item).
        public string homeAddress_BD = "";//variable-length optional field
        public string emailAddress_BE = "";//variable-length optional field
        public string homePhoneNumber_BF = "";//variable-length optional field
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field 
    }

    /*
    2.00 
     The ACS must send this message in response to the End Patron Session message.
     36<end session>< transaction date >< institution id >< patron identifier ><screen message><print line>
     */
    public class EndSessionResponse_36
    {
        public string endSession = "";//1-char, fixed-length required field:  Y or N.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field.
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }


    /*
     2.00 Fee Paid Response
     The ACS must send this message in response to the Fee Paid message.
     38<payment accepted><transaction date><institution id><patron identifier><transaction id><screen message><print line>
     */
    public class FeePaidResponse_38
    {
        public string paymentAccepted = "";//1-char, fixed-length required field:  Y or N.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string transactionId_BK = "";//variable-length optional field.  May be assigned by the ACS to acknowledge  that the payment was received.
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
        2.00 Item Information Response
     The ACS must send this message in response to the Item Information message.
     18<circulation status><hold queue length><security marker><fee type><transaction date><due date><recall date><hold pickup date><item identifier><title identifier><owner><currency type><fee amount><media type><permanent location><current location><item properties><screen message><print line>
     */
    public class ItemInformationResponse_18
    {
        public string circulationStatus = "";//2-char, fixed-length required field (00 thru 99)
        public string securityMarker = "";//2-char, fixed-length required field (00 thru 99)
        public string feeType = "";//2-char, fixed-length required field (01 thru 99).  The type of fee associated with checking out this item.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string holdQueueLength_CF = "";//variable-length optional field
        public string dueDate_AH = "";//variable-length optional field.
        public string recallDate_CJ = "";//18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string holdPickupDate_CM = "";//18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string itemIdentifier_AB = "";//variable-length required field
        public string titleIdentifier_AJ = "";//variable-length required field
        public string owner_BG = "";//variable-length optional field
        public string currencyType_BH = "";//3 char, fixed-length optional field
        public string feeAmount_BV = "";//variable-length optional field.  The amount of the fee associated with this item.
        public string mediaType_CK = "";//3-char, fixed-length optional field
        public string permanentLocation_AQ = "";//variable-length optional field
        public string currentLocation_AP = "";//variable-length optional field
        public string itemProperties_CH = "";//variable-length optional field
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field    
    }

    /*
     2.00 Item Status Update Response
     The ACS must send this message in response to the Item Status Update message.
     20<item properties ok><transaction date><item identifier><title identifier><item properties><screen message><print line>
     */
    public class ItemStatusUpdateResponse_20
    {
        public string itemPropertiesOk = "";//1-char, fixed-length required field:  0 or 1.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string itemIdentifier_AB = "";//variable-length required field
        public string titleIdentifier_AJ = "";//variable-length optional field
        public string itemProperties_CH = "";//variable-length optional field
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
    2.00 Patron Enable Response
     The ACS should send this message in response to the Patron Enable message from the SC.
     26<patron status><language><transaction date><institution id><patron identifier><personal name><valid patron><valid patron password><screen message><print line>
     */
    public class PatronEnableResponse
    {
        public string patronStatus = "";//14-char, fixed-length required field
        public string language = "";//3-char, fixed-length required field
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string personalName_AE = "";//variable-length required field
        public string validPatron_BL = "";//1-char, optional field:  Y or N.
        public string validPatronPassword_CQ = "";//1-char, optional field: Y or N
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
 2.00 Hold Response
 The ACS should send this message in response to the Hold message from the SC.
 16<ok><available><transaction date><expiration date><queue position><pickup location><institution id><patron identifier><item identifier><title identifier><screen message><print line>
     */
    public class HoldResponse
    {
        public string ok = "";//1-char, fixed-length required field:  0 or 1.
        public string available = "";//1-char, fixed-length required field:  Y or N.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string expirationDate_BW = "";//18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string queuePosition_BR = "";//variable-length optional field
        public string pickupLocation_BS = "";//variable-length optional field
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string itemIdentifier_AB = "";//variable-length optional field
        public string titleIdentifier_AJ = "";//variable-length optional field
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }

    /*
2.00 Renew Response
 This message must be sent by the ACS in response to a Renew message by the SC.
 30<ok><renewal ok><magnetic media><desensitize><transaction date><institution id><patron identifier><item identifier><title identifier><due date><fee type><security inhibit><currency type><fee amount><media type><item properties><transaction id><screen message><print line>
See the description of the Checkout Response message for how the ok, renewal ok, desensitize, and fee amount fields will be interpreted.
 */
    public class RenewResponse_30
    {
        public string ok = "";//1-char, fixed-length required field:  0 or 1.
        public string renewalOk = "";//1-char, fixed-length required field:  Y or N.
        public string magneticMedia = "";//1-char, fixed-length required field:  Y or N or U.
        public string desensitize = "";//1-char, fixed-length required field:  Y or N or U.
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string patronIdentifier_AA = "";//variable-length required field
        public string itemIdentifier_AB = "";//variable-length required field
        public string titleIdentifier_AJ = "";//variable-length required field
        public string dueDate_AH = "";//variable-length required field
        public string feeType_BT = "";//2-char, fixed-length optional field (01 thru 99).  The type of fee associated with renewing this item.
        public string securityInhibit_CI = "";//1-char, fixed-length optional field:  Y or N.
        public string currencyType_BH = "";//3-char fixed-length optional field
        public string feeAmount_BV = "";//variable-length optional field.  The amount of the fee associated with this item.
        public string mediaType_CK = "";//3-char, fixed-length optional field
        public string itemProperties_CH = "";//variable-length optional field
        public string transactionId_BK = "";//variable-length optional field.  May be assigned by the ACS when renewing the item involves a fee.
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field    
    }


    /*
 
2.00 Renew All Response
 The ACS should send this message in response to a Renew All message from the SC.
 66<ok ><renewed count><unrenewed count><transaction date><institution id><renewed items><unrenewed items><screen message><print line>
 */
    public class RenewAllResponse_66
    {
        public string ok = "";//1-char, fixed-length required field:  0 or 1
        public string renewedCount = "";//4-char fixed-length required field
        public string unrenewedCount = "";//4-char fixed-length required field
        public string transactionDate = "";//18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string institutionId_AO = "";//variable-length required field
        public string renewedItems_BM = "";//variable-length optional field  (this field sent for each renewed item)
        public string unrenewedItems_BN = "";//variable-length optional field  (this field sent for each unrenewed item)
        public string screenMessage_AF = "";//variable-length optional field
        public string printLine_AG = "";//variable-length optional field
    }
}
