using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
     2.00 End Session Response
     The ACS must send this message in response to the End Patron Session message.
     36<end session>< transaction date >< institution id >< patron identifier ><screen message><print line>
     */
    public class EndSessionResponse_36 : BaseMessage
    {
        //1-char, fixed-length required field:  Y or N.
        public string EndSession_1{ get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field.
        public string PatronIdentifier_AA_r{ get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o{ get; set; }

        //variable-length optional field
        public string PrintLine_AG_o{ get; set; }

    }
}
