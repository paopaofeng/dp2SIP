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
    public class PatronStatusRequest_23 : BaseMessage
    {
        // 3-char, fixed-length required field，Chinese 019
        public string Language_3{ get; set; }

        // 18-char, fixed-length required field，YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        // variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        // variable-length required field
        public string PatronIentifier_AA_r{ get; set; }

        // variable-length required field
        public string TerminalPassword_AC_r{ get; set; }

        // variable-length required field
        public string PatronPassword_AD_r{ get; set; }
    }
}
