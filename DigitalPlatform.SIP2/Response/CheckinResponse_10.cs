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
    public class CheckinResponse_10 : BaseResponse
    {
        //OK should be set to 1 if the ACS checked in the item. should be set to 0 if the ACS did not check in the item.
        //1-char, fixed-length required field:  0 or 1.
        private string _ok_1 = "";
        public string Ok_1
        {
            get { return _ok_1; }
            set { _ok_1 = value; }
        }

        //Resensitize should be set to Y if the SC should resensitize the article. should be set to N if the SC should not resensitize the article (for example, a closed reserve book, or the checkin was refused).
        //1-char, fixed-length required field:  Y or N.
        private string _resensitize_1 = "";
        public string Resensitize_1
        {
            get { return _resensitize_1; }
            set { _resensitize_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N or U.
        private string _magneticMedia_1 = "";
        public string MagneticMedia_1
        {
            get { return _magneticMedia_1; }
            set { _magneticMedia_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _alert_1 = "";
        public string Alert_1
        {
            get { return _alert_1; }
            set { _alert_1 = value; }
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
        private string _itemIdentifier_AB_r = "";
        public string ItemIdentifier_AB_r
        {
            get { return _itemIdentifier_AB_r; }
            set { _itemIdentifier_AB_r = value; }
        }

        //variable-length required field
        private string _permanentLocation_AQ_r = "";
        public string PermanentLocation_AQ_r
        {
            get { return _permanentLocation_AQ_r; }
            set { _permanentLocation_AQ_r = value; }
        }

        //variable-length optional field
        private string _titleIdentifier_AJ_o = "";
        public string TitleIdentifier_AJ_o
        {
            get { return _titleIdentifier_AJ_o; }
            set { _titleIdentifier_AJ_o = value; }
        }

        //variable-length optional field
        private string _sortBin_CL_o = "";
        public string SortBin_CL_o
        {
            get { return _sortBin_CL_o; }
            set { _sortBin_CL_o = value; }
        }

        //variable-length optional field.  ID of the patron who had the item checked out.
        private string _patronIdentifier_AA_o = "";
        public string PatronIdentifier_AA_o
        {
            get { return _patronIdentifier_AA_o; }
            set { _patronIdentifier_AA_o = value; }
        }

        //3-char, fixed-length optional field
        private string _mediaType_CK_e = "";
        public string MediaType_CK_e
        {
            get { return _mediaType_CK_e; }
            set { _mediaType_CK_e = value; }
        }

        //variable-length optional field
        private string _itemProperties_CH_o = "";
        public string ItemProperties_CH_o
        {
            get { return _itemProperties_CH_o; }
            set { _itemProperties_CH_o = value; }
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
