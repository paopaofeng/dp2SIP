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
    public class FeePaidResponse_38 : BaseMessage
    {
        public FeePaidResponse_38()
        {
            this.CommandIdentifier = "38";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
        //1-char, fixed-length required field:  Y or N.
        public string PaymentAccepted_1{ get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r{ get; set; }

        //variable-length optional field.  
        //May be assigned by the ACS to acknowledge  that the payment was received.
        public string TransactionId_BK_o{ get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o{ get; set; }

        //variable-length optional field
        public string PrintLine_AG_o{ get; set; }
        */
    }
}
