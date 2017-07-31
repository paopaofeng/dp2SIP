using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
    2.00 Item Information Response
    The ACS must send this message in response to the Item Information message. 
    18<circulation status><security marker><fee type><transaction date><hold queue length><due date><recall date><hold pickup date><item identifier><title identifier><owner><currency type><fee amount><media type><permanent location><current location><item properties><screen message><print line>
    18	2-char	2-char	2-char	18-char	CF	AF	CJ	CM	AB AJ	BG	BH	BV	CK	AQ	AP	CH	AF	AG
     */
    public class ItemInformationResponse_18 : BaseMessage
    {
        public ItemInformationResponse_18()
        {
            this.CommandIdentifier = "18";

            //==前面的定长字段
            //<circulation status><security marker><fee type><transaction date>
            //2-char	2-char	2-char	18-char
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_CirculationStatus, 2));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_SecurityMarker, 2));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_BT_FeeType, 2));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_TransactionDate, 18));

            //==后面变长字段
            //<hold queue length><due date><recall date><hold pickup date>
            //CF	AF	CJ	CM
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CF_HoldQueueLength, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AH_DueDate, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CJ_RecallDate, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CM_HoldPickupDate, false ));

            //<item identifier><title identifier><owner><currency type>
            //AB AJ	BG	BH	
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AB_ItemIdentifier, true));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AJ_TitleIdentifier, true));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BG_Owner, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BH_CurrencyType, false ));

            //<fee amount><media type><permanent location><current location>
            //BV	CK	AQ	AP	
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BV_FeeAmount, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CK_MediaType, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AQ_PermanentLocation, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AP_CurrentLocation, false ));

            //<item properties><screen message><print line>
            //CH	AF	AG
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CH_ItemProperties, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AF_ScreenMessage, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AG_PrintLine, false ));
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
