using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
     Fee Paid
     This message can be used to notify the ACS that a fee has been collected from the patron. The ACS should record this information in their database and respond with a Fee Paid Response message.
     37<transaction date><fee type><payment type><currency type><fee amount><institution id><patron identifier><terminal password><patron password><fee identifier><transaction id>
     */
    public class FeePaid_37 : BaseRequest
    {
        // 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
        }

        // 2-char, fixed-length required field (01 thru 99). identifies a fee type to apply  the payment to.
        private string _feeType_2 = "";
        public string FeeType_2
        {
            get { return _feeType_2; }
            set { _feeType_2 = value; }
        }

        // 2-char, fixed-length required field (00 thru 99)
        private string _paymentType_2 = "";
        public string PaymentType_2
        {
            get { return _paymentType_2; }
            set { _paymentType_2 = value; }
        }

        // 3-char, fixed-length required field
        private string _currencyType_3 = "";
        public string CurrencyType_3
        {
            get { return _currencyType_3; }
            set { _currencyType_3 = value; }
        }

        // variable-length required field; the amount paid.
        private string _feeAmount_BV_r = "";
        public string FeeAmount_BV_r
        {
            get { return _feeAmount_BV_r; }
            set { _feeAmount_BV_r = value; }
        }

        // variable-length required field
        private string _institutionId_AO_r = "";
        public string InstitutionId_AO_r
        {
            get { return _institutionId_AO_r; }
            set { _institutionId_AO_r = value; }
        }

        // variable-length required field.
        private string _patronIdentifier_AA_r = "";
        public string PatronIdentifier_AA_r
        {
            get { return _patronIdentifier_AA_r; }
            set { _patronIdentifier_AA_r = value; }
        }

        // variable-length optional field
        private string _terminalPassword_AC_o = "";
        public string TerminalPassword_AC_o
        {
            get { return _terminalPassword_AC_o; }
            set { _terminalPassword_AC_o = value; }
        }

        // variable-length optional field
        private string _patronPassword_AD_o = "";
        public string PatronPassword_AD_o
        {
            get { return _patronPassword_AD_o; }
            set { _patronPassword_AD_o = value; }
        }

        // variable-length optional field; identifies a specific fee to apply the payment to.
        private string _feeIdentifier_CG_o = "";
        public string FeeIdentifier_CG_o
        {
            get { return _feeIdentifier_CG_o; }
            set { _feeIdentifier_CG_o = value; }
        }

        // variable-length optional field; a transaction id assigned by the payment device.
        private string _transactionId_BK_o = "";
        public string TransactionId_BK_o
        {
            get { return _transactionId_BK_o; }
            set { _transactionId_BK_o = value; }
        }
    }
}
