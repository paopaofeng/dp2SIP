using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
    2.00 Item Status Update Response
    The ACS must send this message in response to the Item Status Update message.
    20<item properties ok><transaction date><item identifier><title identifier><item properties><screen message><print line>
    */
    public class ItemStatusUpdateResponse_20 : BaseMessage
    {
        public ItemStatusUpdateResponse_20()
        {
            this.CommandIdentifier = "20";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
        //1-char, fixed-length required field:  0 or 1.
        public string ItemPropertiesOk_1 {get;set;}

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18 { get; set; }

        //variable-length required field
        public string ItemIdentifier_AB_r { get; set; }

        //variable-length optional field
        public string TitleIdentifier_AJ_o { get; set; }

        //variable-length optional field
        public string ItemProperties_CH_o { get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o { get; set; }

        //variable-length optional field
        public string PrintLine_AG_o  {get;set;}
         */
    }
}
