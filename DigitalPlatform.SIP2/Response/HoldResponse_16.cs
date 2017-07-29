using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
 2.00 Hold Response
 The ACS should send this message in response to the Hold message from the SC.
 16<ok><available><transaction date><expiration date><queue position><pickup location><institution id><patron identifier><item identifier><title identifier><screen message><print line>
     */
    public class HoldResponse_16 : BaseMessage
    {
        //1-char, fixed-length required field:  0 or 1.
        public string Ok_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string Available_1{ get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        //18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string ExpirationDate_BW_18{ get; set; }

        //variable-length optional field
        public string QueuePosition_BR_o{ get; set; }

        //variable-length optional field
        public string PickupLocation_BS_o{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r{ get; set; }

        //variable-length optional field
        public string ItemIdentifier_AB_o{ get; set; }

        //variable-length optional field
        public string TitleIdentifier_AJ_o{ get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o{ get; set; }

        //variable-length optional field
        public string PrintLine_AG_o{ get; set; }

    }
}
