using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
    2.00 Renew All Response
    The ACS should send this message in response to a Renew All message from the SC.
    66<ok ><renewed count><unrenewed count><transaction date><institution id><renewed items><unrenewed items><screen message><print line>
    */
    public class RenewAllResponse_66 : BaseMessage
    {
        //1-char, fixed-length required field:  0 or 1
        public string Ok_1 { get; set; }

        //4-char fixed-length required field
        public string RenewedCount_14 { get; set; }

        //4-char fixed-length required field
        public string UnrenewedCount_14 { get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18 { get; set; }

        //variable-length required field
        public string InstitutionId_AO_r { get; set; }

        //variable-length optional field  (this field sent for each renewed item)
        public string RenewedItems_BM_o { get; set; }

        //variable-length optional field  (this field sent for each unrenewed item)
        public string UnrenewedItems_BN_o { get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o { get; set; }

        //variable-length optional field
        public string PrintLine_AG_o { get; set; }
    }
}
