using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
 Patron Status Response
 The ACS must send this message in response to a Patron Status Request message as well as in response to a Block Patron message.
 24<patron status><language><transaction date><institution id><patron identifier><personal name><valid patron><valid patron password><currency type><fee amount><screen message><print line>
     */
    public class PatronStatusResponse_24 : BaseMessage
    {
        //14-char, fixed-length required field
        public string PatronStatus_14 { get; set; }

        //3-char, fixed-length required field
        public string Language_3 { get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18 { get; set; }

        //variable-length required field
        public string InstitutionId_AO_r { get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r { get; set; }

        //variable-length required field
        public string PersonalName_AE_r { get; set; }

        //1-char, optional field: Y or N
        public string ValidPatron_BL_o { get; set; }

        //1-char, optional field: Y or N
        public string ValidPatronPassword_CQ_o { get; set; }

        //3-char, fixed-length optional field
        public string CurrencyType_BH_o { get; set; }

        //variable-length optional field.  The amount of fees owed by this patron.
        public string FeeAmount_BV_o { get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o { get; set; }

        //variable-length optional field
        public string PrintLine_AG_o { get; set; }
    }
}
