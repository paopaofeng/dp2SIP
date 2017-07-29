using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{

    /*
    2.00 Fee Paid Response
    The ACS must send this message in response to the Fee Paid message.
    38<payment accepted><transaction date><institution id><patron identifier><transaction id><screen message><print line>
    */
    public class FeePaidResponse_38 : BaseResponse
    {
        //1-char, fixed-length required field:  Y or N.
        private string _paymentAccepted_1 = "";
        public string PaymentAccepted_1
        {
            get { return _paymentAccepted_1; }
            set { _paymentAccepted_1 = value; }
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
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

        //variable-length optional field.  
        //May be assigned by the ACS to acknowledge  that the payment was received.
        private string _transactionId_BK_o = "";
        public string TransactionId_BK_o
        {
            get { return _transactionId_BK_o; }
            set { _transactionId_BK_o = value; }
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
