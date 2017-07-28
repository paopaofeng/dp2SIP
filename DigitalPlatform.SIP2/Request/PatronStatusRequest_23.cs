using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
 Patron Status Request
 This message is used by the SC to request patron information from the ACS. 
 The ACS must respond to this command with a Patron Status Response message.
 23<language><transaction date><institution id><patron identifier><terminal password><patron password>
     */
    public class PatronStatusRequest_23 : BaseRequest
    {
        // 3-char, fixed-length required field，Chinese 019
        private string _language_3 = "";                
        public string Language_3
        {
            get { return _language_3; }
            set { _language_3 = value; }
        }

        // 18-char, fixed-length required field，YYYYMMDDZZZZHHMMSS
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

        // variable-length required field
        private string _patronIentifier_AA_r = "";      
        public string PatronIentifier_AA_r
        {
            get { return _patronIentifier_AA_r; }
            set { _patronIentifier_AA_r = value; }
        }

        // variable-length required field
        private string _terminalPassword_AC_r = ""; 
        public string TerminalPassword_AC_r
        {
            get { return _terminalPassword_AC_r; }
            set { _terminalPassword_AC_r = value; }
        }

        // variable-length required field
        private string _patronPassword_AD_r = "";   
        public string PatronPassword_AD_r
        {
            get { return _patronPassword_AD_r; }
            set { _patronPassword_AD_r = value; }
        }
    }
}
