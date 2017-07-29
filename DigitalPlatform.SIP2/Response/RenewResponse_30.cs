using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
    2.00 Renew Response
    This message must be sent by the ACS in response to a Renew message by the SC.
    30<ok><renewal ok><magnetic media><desensitize><transaction date><institution id><patron identifier><item identifier><title identifier><due date><fee type><security inhibit><currency type><fee amount><media type><item properties><transaction id><screen message><print line>
    See the description of the Checkout Response message for how the ok, renewal ok, desensitize, and fee amount fields will be interpreted.
    */
    public class RenewResponse_30 : BaseMessage
    {
        //1-char, fixed-length required field:  0 or 1.
        public string Ok_1 { get; set; }

        //1-char, fixed-length required field:  Y or N.
        public string RenewalOk_1 { get; set; }

        //1-char, fixed-length required field:  Y or N or U.
        public string MagneticMedia_1 { get; set; }

        //1-char, fixed-length required field:  Y or N or U.
        public string Desensitize_1 { get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18 { get; set; }

        //variable-length required field
        public string InstitutionId_AO_r { get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r { get; set; }

        //variable-length required field
        public string ItemIdentifier_AB_r { get; set; }

        //variable-length required field
        public string TitleIdentifier_AJ_r { get; set; }

        //variable-length required field
        public string DueDate_AH_r { get; set; }

        //2-char, fixed-length optional field (01 thru 99).  The type of fee associated with renewing this item.
        public string FeeType_BT_2_o { get; set; }

        //1-char, fixed-length optional field:  Y or N.
        public string SecurityInhibit_CI_1_o { get; set; }

        //3-char fixed-length optional field
        public string CurrencyType_BH_3_o { get; set; }

        //variable-length optional field.  The amount of the fee associated with this item.
        public string FeeAmount_BV_o { get; set; }

        //3-char, fixed-length optional field
        public string MediaType_CK_3_o { get; set; }

        //variable-length optional field
        public string ItemProperties_CH_o { get; set; }

        //variable-length optional field.  May be assigned by the ACS when renewing the item involves a fee.
        public string TransactionId_BK_o { get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o { get; set; }

        //variable-length optional field  
        public string PrintLine_AG_o { get; set; } 
    }
}
