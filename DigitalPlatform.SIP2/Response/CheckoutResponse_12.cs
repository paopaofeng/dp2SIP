using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
    Checkout Response
    This message must be sent by the ACS in response to a Checkout message from the SC.
    12<ok><renewal ok><magnetic media><desensitize><transaction date><institution id><patron identifier><item identifier><title identifier><due date><fee type><security inhibit><currency type><fee amount><media type><item properties><transaction id><screen message><print line>
    */
    public class CheckoutResponse_12 : BaseResponse
    {
        //OK should be set to 1 if the ACS checked out the item to the patron. should be set to 0 if the ACS did not check out the item to the patron.
        //1-char, fixed-length required field:  0 or 1.
        private string _ok_1 = "";
        public string Ok_1
        {
            get { return _ok_1; }
            set { _ok_1 = value; }
        }

        //Renewal OK should be set to Y if the patron requesting to check out the item already has the item checked out should be set to N if the item is not already checked out to the requesting patron. 
        //1-char, fixed-length required field:  Y or N.
        private string _renewalOk_1 = "";
        public string RenewalOk_1
        {
            get { return _renewalOk_1; }
            set { _renewalOk_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N or U.
        private string _magneticMedia_1 = "";
        public string MagneticMedia_1
        {
            get { return _magneticMedia_1; }
            set { _magneticMedia_1 = value; }
        }

        // Desensitize should be set to Y if the SC should desensitize the article. should be set to N if the SC should not desensitize the article (for example, a closed reserve book, or the checkout was refused).
        //1-char, fixed-length required field:  Y or N or U.
        private string _desensitize_1 = "";
        public string Desensitize_1
        {
            get { return _desensitize_1; }
            set { _desensitize_1 = value; }
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
        }

        //variable-length required field
        private string _institutionId_AO_r = "";
        public string InstitutionId_AO_r
        {
            get { return _institutionId_AO_r; }
            set { _institutionId_AO_r = value; }
        }

        //variable-length required field
        private string _patronIdentifier_AA_r = "";
        public string PatronIdentifier_AA_r
        {
            get { return _patronIdentifier_AA_r; }
            set { _patronIdentifier_AA_r = value; }
        }

        //variable-length required field
        private string _itemIdentifier_AB_r = "";
        public string ItemIdentifier_AB_r
        {
            get { return _itemIdentifier_AB_r; }
            set { _itemIdentifier_AB_r = value; }
        }

        //variable-length required field
        private string _titleIdentifier_AJ_r = "";
        public string TitleIdentifier_AJ_r
        {
            get { return _titleIdentifier_AJ_r; }
            set { _titleIdentifier_AJ_r = value; }
        }

        //variable-length required field
        private string _dueDate_AH_r = "";
        public string DueDate_AH_r
        {
            get { return _dueDate_AH_r; }
            set { _dueDate_AH_r = value; }
        }

        //2-char, fixed-length optional field (01 thru 99).  The type of fee associated with checking out this item.
        private string _feeType_BT_2_o = "";
        public string FeeType_BT_2_o
        {
            get { return _feeType_BT_2_o; }
            set { _feeType_BT_2_o = value; }
        }


        //1-char, fixed-length optional field:  Y or N.
        private string _securityInhibit_CI_1_o = "";
        public string SecurityInhibit_CI_1_o
        {
            get { return _securityInhibit_CI_1_o; }
            set { _securityInhibit_CI_1_o = value; }
        }


        //3-char fixed-length optional field
        private string _currencyType_BH_3_o = "";
        public string CurrencyType_BH_3_o
        {
            get { return _currencyType_BH_3_o; }
            set { _currencyType_BH_3_o = value; }
        }


        //Fee Amount should be set to the value of the fee associated with checking out the item should be set to 0 if there is no fee associated with checking out the item.
        //variable-length optional field.  The amount of the fee associated with checking out this item.
        private string _feeAmount_BV_o = "";
        public string FeeAmount_BV_o
        {
            get { return _feeAmount_BV_o; }
            set { _feeAmount_BV_o = value; }
        }

        //3-char, fixed-length optional field
        private string _mediaType_CK_3_o = "";
        public string MediaType_CK_3_o
        {
            get { return _mediaType_CK_3_o; }
            set { _mediaType_CK_3_o = value; }
        }

        //variable-length optional field
        private string _itemProperties_CH_o = "";
        public string ItemProperties_CH_o
        {
            get { return _itemProperties_CH_o; }
            set { _itemProperties_CH_o = value; }
        }

        //variable-length optional field.  May be assigned by the ACS when checking out the item involves a fee.
        private string _transactionId_BK_o = "";
        public string TransactionId_BK_o
        {
            get { return _transactionId_BK_o; }
            set { _transactionId_BK_o = value; }
        }

        //variable-length optional field
        private string _screenMessage_AF_o = "";
        public string ScreenMessage_AF_o
        {
            get { return _screenMessage_AF_o; }
            set { _screenMessage_AF_o = value; }
        }

        //variable-length optional field
        private string _printLine_AG_o = "";
        public string PrintLine_AG_o
        {
            get { return _printLine_AG_o; }
            set { _printLine_AG_o = value; }
        }
    }
}
