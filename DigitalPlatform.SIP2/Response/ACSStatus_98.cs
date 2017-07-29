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
        // 1-char, fixed-length required field:  Y or N.
        public string OnlineStatus_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string CheckinOk_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string CheckoutOk_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string ACSRenewalPolicy_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string StatusUpdateOk_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string OfflineOk_1{ get; set; }

        //3-char, fixed-length required field
        public string TimeoutPeriod_3{ get; set; }

        //3-char, fixed-length required field
        public string RetriesAllowed_3{ get; set; }

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

    }
}
