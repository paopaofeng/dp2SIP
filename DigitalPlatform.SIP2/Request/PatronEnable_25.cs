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
    public class PatronEnable_25 : BaseMessage
    {
        public PatronEnable_25()
        {
            this.CommandIdentifier = "25";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string Transaction_Date_18{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r{ get; set; }

        //variable-length optional field
        public string TerminalPassword_AC_o{ get; set; }

        //variable-length optional field
        public string PatronPassword_AD_o{ get; set; }
        */
    }
}
