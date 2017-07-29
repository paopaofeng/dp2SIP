using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    2.00 Hold
    This message is used to create, modify, or delete a hold.  The ACS should respond with a Hold Response message.  Either or both of the “item identifier” and “title identifier” fields must be present for the message to be useful.
    15<hold mode><transaction date><expiration date><pickup location><hold type><institution id><patron identifier><patron password><item identifier><title identifier><terminal password><fee acknowledged>
     */
    public class Hold_15 : BaseMessage
    {
        // 1-char, fixed-length required field  '+'/'-'/'*'  Add, delete, change
        public string HoldMode_1{ get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        //18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string ExpirationDate_BW_18{ get; set; }

        //variable-length, optional field
        public string PickupLocation_BS_o{ get; set; }

        //1-char, optional field
        public string HoldType_BY_o{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r{ get; set; }

        //variable-length optional field
        public string PatronPassword_AD_o{ get; set; }

        //variable-length optional field
        public string ItemIdentifier_AB_o{ get; set; }

        //variable-length optional field
        public string TitleIdentifier_AJ_o{ get; set; }

        //variable-length optional field
        public string TerminalPassword_AC_o{ get; set; }

        //1-char, optional field: Y or N.
        public string FeeAcknowledged_BO_o{ get; set; }

    }
}
