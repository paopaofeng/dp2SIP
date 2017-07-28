using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
2.00 End Patron Session
This message will be sent when a patron has completed all of their transactions.  The ACS may, upon receipt of this command, close any open files or deallocate data structures pertaining to that patron. The ACS should respond with an End Session Response message.
35<transaction date><institution id><patron identifier><terminal password><patron password>
 */
    public class EndPatronSession_35 : BaseRequest
    {
        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
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
    }
}
