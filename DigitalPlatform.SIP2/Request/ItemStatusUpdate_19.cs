using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
     2.00 Item Status Update
     This message can be used to send item information to the ACS, without having to do a Checkout or Checkin operation.  The item properties could be stored on the ACS’s database.  The ACS should respond with an Item Status Update Response message.
     19<transaction date><institution id><item identifier><terminal password><item properties>
     */
    public class ItemStatusUpdate_19 : BaseMessage
    {
        // 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        // variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //  variable-length required field
        public string ItemIdentifier_AB_r{ get; set; }

        //  variable-length optional field
        public string TerminalPassowrd_AC_o{ get; set; }

        //  variable-length required field
        public string ItemProperties_CH_r{ get; set; }

    }
}
