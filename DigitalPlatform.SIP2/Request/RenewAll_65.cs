using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    2.00 Renew All
    This message is used to renew all items that the patron has checked out.  The ACS should respond with a Renew All Response message.
    65<transaction date><institution id><patron identifier><patron password><terminal password><fee acknowledged>
    */
    public class RenewAll_65 : BaseRequest
    {
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

        //variable-length optional field
        private string _patronPassword_AD_o = "";
        public string PatronPassword_AD_o
        {
            get { return _patronPassword_AD_o; }
            set { _patronPassword_AD_o = value; }
        }

        //variable-length optional field
        private string _terminalPassword_AC_o = "";
        public string TerminalPassword_AC_o
        {
            get { return _terminalPassword_AC_o; }
            set { _terminalPassword_AC_o = value; }
        }

        //1-char, optional field: Y or N.
        private string _feeAcknowledged_BO_o = "";
        public string FeeAcknowledged_BO_o
        {
            get { return _feeAcknowledged_BO_o; }
            set { _feeAcknowledged_BO_o = value; }
        }
    }
}
