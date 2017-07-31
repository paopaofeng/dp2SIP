using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
     Checkin Response
     This message must be sent by the ACS in response to a SC Checkin message.
     10<ok><resensitize><magnetic media><alert><transaction date><institution id><item identifier><permanent location><title identifier><sort bin><patron identifier><media type><item properties><screen message><print line>
    */
    public class CheckinResponse_10 : BaseMessage
    {
        public CheckinResponse_10()
        {
            this.CommandIdentifier = "10";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }
        /*
        //OK should be set to 1 if the ACS checked in the item. should be set to 0 if the ACS did not check in the item.
        //1-char, fixed-length required field:  0 or 1.
        public string Ok_1{ get; set; }

        //Resensitize should be set to Y if the SC should resensitize the article. should be set to N if the SC should not resensitize the article (for example, a closed reserve book, or the checkin was refused).
        //1-char, fixed-length required field:  Y or N.
        public string Resensitize_1{ get; set; }

        //1-char, fixed-length required field:  Y or N or U.
        public string MagneticMedia_1{ get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string Alert_1{ get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field
        public string ItemIdentifier_AB_r{ get; set; }

        //variable-length required field
        public string PermanentLocation_AQ_r{ get; set; }

        //variable-length optional field
        public string TitleIdentifier_AJ_o{ get; set; }

        //variable-length optional field
        public string SortBin_CL_o{ get; set; }

        //variable-length optional field.  ID of the patron who had the item checked out.
        public string PatronIdentifier_AA_o{ get; set; }

        //3-char, fixed-length optional field
        public string MediaType_CK_e{ get; set; }

        //variable-length optional field
        public string ItemProperties_CH_o{ get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o{ get; set; }

        //variable-length optional field
        public string PrintLine_AG_o{ get; set; }
         */
    }
}
