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
    public class RenewAll_65 : BaseMessage
    {
        public RenewAll_65()
        {
            this.CommandIdentifier = "65";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r{ get; set; }

        //variable-length optional field
        public string PatronPassword_AD_o{ get; set; }

        //variable-length optional field
        public string TerminalPassword_AC_o{ get; set; }

        //1-char, optional field: Y or N.
        public string FeeAcknowledged_BO_o{ get; set; }

    }
}
