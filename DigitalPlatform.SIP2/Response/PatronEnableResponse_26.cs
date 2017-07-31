using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
    2.00 Patron Enable Response
     The ACS should send this message in response to the Patron Enable message from the SC.
     26<patron status><language><transaction date><institution id><patron identifier><personal name><valid patron><valid patron password><screen message><print line>
     */
    public class PatronEnableResponse_26 : BaseMessage
    {
        public PatronEnableResponse_26()
        {
            this.CommandIdentifier = "26";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
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

        //1-char, optional field:  Y or N.
        public string ValidPatron_BL_o { get; set; }

        //1-char, optional field: Y or N
        public string ValidPatronPassword_CQ_o { get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o { get; set; }

        //variable-length optional field
        public string PrintLine_AG_o { get; set; }
         */
    }
}
