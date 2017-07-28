using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    Patron Enable
    This message can be used by the SC to re-enable canceled patrons.  It should only be used for system testing and validation.  The ACS should respond with a Patron Enable Response message.
    25<transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class PatronEnable_25 : BaseRequest
    {
        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transaction_Date_18 = "";
        public string Transaction_Date_18
        {
            get { return _transaction_Date_18; }
            set { _transaction_Date_18 = value; }
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
        private string _terminalPassword_AC_o = "";
        public string TerminalPassword_AC_o
        {
            get { return _terminalPassword_AC_o; }
            set { _terminalPassword_AC_o = value; }
        }

        //variable-length optional field
        private string _patronPassword_AD_o = "";
        public string PatronPassword_AD_o
        {
            get { return _patronPassword_AD_o; }
            set { _patronPassword_AD_o = value; }
        }

    }
}
