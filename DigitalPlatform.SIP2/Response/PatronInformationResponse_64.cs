using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
     Patron Information Response
     The ACS must send this message in response to the Patron Information message.
     64<patron status><language><transaction date><hold items count><overdue items count>
     <charged items count><fine items count><recall items count><unavailable holds count><institution id>
     <patron identifier><personal name><hold items limit><overdue items limit><charged items limit>
     <valid patron><valid patron password><currency type><fee amount><fee limit><items>
     <home address><e-mail address><home phone number><screen message><print line>
    */
    public class PatronInformationResponse_64 : BaseMessage
    {
        public PatronInformationResponse_64()
        {
            this.CommandIdentifier = "64";

            //==前面的定长字段
            this.FixedLengthFields.Add(new FixedLengthField("", 1));

            //==后面变长字段
            this.VariableLengthFields.Add(new VariableLengthField("", true));
        }

        /*
        //14-char, fixed-length required field
        public string PatronStatus_14 { get; set; }

        //3-char, fixed-length required field
        public string Language_3 { get; set; }
        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18 { get; set; }

        //4-char, fixed-length required field
        public string HoldItemsCount_4 { get; set; }

        //4-char, fixed-length required field
        public string OverdueItemsCount_4 { get; set; }

        //4-char, fixed-length required field
        public string ChargedItemsCount_4 { get; set; }

        //4-char, fixed-length required field
        public string FineItemsCount_4 { get; set; }

        //4-char, fixed-length required field
        public string RecallItemsCount_4 { get; set; }

        //4-char, fixed-length required field
        public string UnavailableHoldsCount_4 { get; set; }

        //variable-length required field
        public string InstitutionId_AO_r { get; set; }

        //variable-length required field
        public string PatronIdentifier_AA_r  {get;set;}

        //variable-length required field
        public string PersonalName_AE_r  {get;set;}

        //4-char, fixed-length optional field
        public string HoldItemsLimit_BZ_o { get; set; }

        //4-char, fixed-length optional field
        public string OverdueItemsLimit_CA_o { get; set; }

        //4-char, fixed-length optional field
        public string ChargedItemsLimit_CB_o { get; set; }

        //1-char, optional field:  Y or N
        public string ValidPatron_BL_o { get; set; }

        //1-char, optional field: Y or N
        public string ValidPatronPassword_CQ_o { get; set; }

        //3-char fixed-length optional field
        public string CurrencyType_BH_3 { get; set; }

        //variable-length optional field.  The amount of fees owed by this patron.
        public string feeAmount_BV_o { get; set; }

        //variable-length optional field.  The fee limit amount.
        public string FeeLimit_CC_o { get; set; }


        //item: zero or more instances of one of the following, based on “summary” field of the Patron Information message:
        //variable-length optional field  (this field should be sent for each hold item).
        public string HoldItems_AS_o { get; set; }

        //variable-length optional field  (this field should be sent for each overdue item).
        public string OverdueItems_AT_o { get; set; }

        //variable-length optional field  (this field should be sent for each charged item).
        public string ChargedItems_AU_o { get; set; }

        //variable-length optional field  (this field should be sent for each fine item).
        public string FineItems_AV_o { get; set; }

        //variable-length optional field  (this field should be sent for each recall item). 
        public string RecallItems_BU_o { get; set; }

        //variable-length optional field  (this field should be sent for each unavailable hold item).
        public string UnavailableHoldItems_CD_o { get; set; }

        //variable-length optional field
        public string HomeAddress_BD_o { get; set; }

        //variable-length optional field
        public string EmailAddress_BE_o { get; set; }

        //variable-length optional field
        public string HomePhoneNumber_BF_o { get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o { get; set; }

        //variable-length optional field
        public string PrintLine_AG_o { get; set; }
         */
    }
}
