using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
     Patron Information Response
     The ACS must send this message in response to the Patron Information message.
     64<patron status><language><transaction date><hold items count><overdue items count><charged items count><fine items count><recall items count><unavailable holds count>
     64	14-char	3-char	18-char	4-char	4-char  4-char	4-char
     <institution id><patron identifier><personal name><hold items limit><overdue items limit>
     AO	AA	AE	    BZ	CA
     <charged items limit><valid patron><valid patron password><currency type><fee amount><fee limit>
     CB	BL	CQ	BH	BV	CC
     <hold items><overdue items><charged items><fine items><recall items><unavailable hold items>
     AS	AT	AU	AV	BU	CD
     <home address><e-mail address><home phone number><screen message><print line>
     BD	BE  BF	AF	AG
    */
    public class PatronInformationResponse_64 : BaseMessage
    {
        public PatronInformationResponse_64()
        {
            this.CommandIdentifier = "64";

            //==前面的定长字段
            //<patron status><language><transaction date><hold items count><overdue items count><charged items count><fine items count><recall items count><unavailable holds count>
            //14-char	3-char	18-char	---4-char	4-char  4-char	---4-char  4-char	4-char	
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_PatronStatus, 14));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_Language, 3));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_TransactionDate, 18));

            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_HoldItemsCount, 4));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_OverdueItemsCount, 4));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_ChargedItemsCount, 4));

            //<fine items count><recall items count><unavailable holds count>
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_FineItemsCount, 4));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_RecallItemsCount, 4));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_UnavailableHoldsCount, 4));

            //==后面变长字段
             //<institution id><patron identifier><personal name><hold items limit><overdue items limit>
             //AO	AA	AE	    BZ	CA
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AO_InstitutionId, true));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AA_PatronIdentifier, true));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AE_PersonalName, true));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BZ_HoldItemsLimit, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CA_OverdueItemsLimit, false ));

            //<charged items limit><valid patron><valid patron password><currency type><fee amount><fee limit>
            //CB	BL	CQ	BH	BV	CC
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CB_ChargedItemsLimit, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BL_ValidPatron, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CQ_ValidPatronPassword, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BH_CurrencyType, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BV_FeeAmount, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CC_FeeLimit, false ));

             //<hold items><overdue items><charged items><fine items><recall items><unavailable hold items>
             //AS	AT	AU	AV	BU	CD
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AS_HoldItems, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AT_OverdueItems, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AU_ChargedItems, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AV_FineItems, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BU_RecallItems, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_CD_UnavailableHoldItems, false ));


             //<home address><e-mail address><home phone number><screen message><print line>
             //BD	BE  BF	AF	AG
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BD_HomeAddress, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BE_EmailAddress, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_BF_HomePhoneNumbers, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AF_ScreenMessage, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AG_PrintLine, false ));


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
