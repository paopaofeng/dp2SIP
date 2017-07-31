using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
    2.00 Item Information Response
    The ACS must send this message in response to the Item Information message. 
    18<circulation status><hold queue length><security marker><fee type><transaction date><due date><recall date><hold pickup date><item identifier><title identifier><owner><currency type><fee amount><media type><permanent location><current location><item properties><screen message><print line>
    */
    public class ItemInformationResponse_18 : BaseMessage
    {
        public ItemInformationResponse_18()
        {
            this.CommandIdentifier = "18";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
        //2-char, fixed-length required field (00 thru 99)
        public string CirculationStatus_2{ get; set; }

        //2-char, fixed-length required field (00 thru 99)
        public string SecurityMarker_2{ get; set; }

        //2-char, fixed-length required field (01 thru 99).  The type of fee associated with checking out this item.
        public string FeeType_2{ get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS

        //variable-length optional field
        public string HoldQueueLength_CF_o{ get; set; }

        //variable-length optional field.
        public string DueDate_AH_o{ get; set; }


        //18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string RecallDate_CJ_18 { get; set; }


        //18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        public string HoldPickupDate_CM_18{ get; set; }


        //variable-length required field
        public string ItemIdentifier_AB_r{ get; set; }

        //variable-length required field
        public string TitleIdentifier_AJ_r{ get; set; }

        //variable-length optional field
        public string Owner_BG_o{ get; set; }

        //3 char, fixed-length optional field
        public string CurrencyType_BH_o{ get; set; }

        //variable-length optional field.  The amount of the fee associated with this item.
        public string FeeAmount_BV_o{ get; set; }

        //3-char, fixed-length optional field
        public string MediaType_CK_o{ get; set; }

        //variable-length optional field
        public string PermanentLocation_AQ_o{ get; set; }

        //variable-length optional field
        public string CurrentLocation_AP_o{ get; set; }

        //variable-length optional field
        public string ItemProperties_CH_o{ get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o{ get; set; }

        //variable-length optional field    
        public string PrintLine_AG_o{ get; set; }
        */
    }
}
