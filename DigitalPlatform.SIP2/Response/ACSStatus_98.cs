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
    public class ACSStatus_98 : BaseResponse
    {
        // 1-char, fixed-length required field:  Y or N.
        private string _onlineStatus_1 = "";// 1-char, fixed-length required field:  Y or N.
        public string OnlineStatus_1
        {
            get { return _onlineStatus_1; }
            set { _onlineStatus_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _checkinOk_1 = "";
        public string CheckinOk_1
        {
            get { return _checkinOk_1; }
            set { _checkinOk_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _checkoutOk_1 = "";
        public string CheckoutOk_1
        {
            get { return _checkoutOk_1; }
            set { _checkoutOk_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _ACSRenewalPolicy_1 = "";
        public string ACSRenewalPolicy_1
        {
            get { return _ACSRenewalPolicy_1; }
            set { _ACSRenewalPolicy_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _statusUpdateOk_1 = "";
        public string StatusUpdateOk_1
        {
            get { return _statusUpdateOk_1; }
            set { _statusUpdateOk_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _offlineOk_1 = "";
        public string OfflineOk_1
        {
            get { return _offlineOk_1; }
            set { _offlineOk_1 = value; }
        }

        //3-char, fixed-length required field
        private string _timeoutPeriod_3 = "";
        public string TimeoutPeriod_3
        {
            get { return _timeoutPeriod_3; }
            set { _timeoutPeriod_3 = value; }
        }

        //3-char, fixed-length required field
        private string _retriesAllowed_3 = "";
        public string RetriesAllowed_3
        {
            get { return _retriesAllowed_3; }
            set { _retriesAllowed_3 = value; }
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _datetimeSync_18 = "";
        public string DatetimeSync_18
        {
            get { return _datetimeSync_18; }
            set { _datetimeSync_18 = value; }
        }

        //4-char, fixed-length required field:  x.xx
        private string _protocolVersion_4 = "";
        public string ProtocolVersion_4
        {
            get { return _protocolVersion_4; }
            set { _protocolVersion_4 = value; }
        }

        // variable-length required field
        private string _institutionId_AO_r = "";
        public string InstitutionId_AO_r
        {
            get { return _institutionId_AO_r; }
            set { _institutionId_AO_r = value; }
        }

        //variable-length optional field
        private string _libraryName_AM_o = "";
        public string LibraryName_AM_o
        {
            get { return _libraryName_AM_o; }
            set { _libraryName_AM_o = value; }
        }

        //variable-length required field
        private string _supportedMessages_BX_r = "";
        public string SupportedMessages_BX_r
        {
            get { return _supportedMessages_BX_r; }
            set { _supportedMessages_BX_r = value; }
        }

        //variable-length optional field
        private string _terminalLocation_AN_o = "";
        public string TerminalLocation_AN_o
        {
            get { return _terminalLocation_AN_o; }
            set { _terminalLocation_AN_o = value; }
        }

        //variable-length optional field
        private string _screenMessage_AF_o = "";
        public string ScreenMessage_AF_o
        {
            get { return _screenMessage_AF_o; }
            set { _screenMessage_AF_o = value; }
        }

        //variable-length optional field
        private string _printLine_AG_o = "";
        public string PrintLine_AG_o
        {
            get { return _printLine_AG_o; }
            set { _printLine_AG_o = value; }
        }
    }
}
