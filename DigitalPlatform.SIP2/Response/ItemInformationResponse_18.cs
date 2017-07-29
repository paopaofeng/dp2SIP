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
    public class ItemInformationResponse_18 : BaseResponse
    {
        //2-char, fixed-length required field (00 thru 99)
        private string _circulationStatus_2 = "";
        public string CirculationStatus_2
        {
            get { return _circulationStatus_2; }
            set { _circulationStatus_2 = value; }
        }

        //2-char, fixed-length required field (00 thru 99)
        private string _securityMarker_2 = "";
        public string SecurityMarker_2
        {
            get { return _securityMarker_2; }
            set { _securityMarker_2 = value; }
        }

        //2-char, fixed-length required field (01 thru 99).  The type of fee associated with checking out this item.
        private string _feeType_2 = "";
        public string FeeType_2
        {
            get { return _feeType_2; }
            set { _feeType_2 = value; }
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
        }

        //variable-length optional field
        private string _holdQueueLength_CF_o = "";
        public string HoldQueueLength_CF_o
        {
            get { return _holdQueueLength_CF_o; }
            set { _holdQueueLength_CF_o = value; }
        }

        //variable-length optional field.
        private string _dueDate_AH_o = "";
        public string DueDate_AH_o
        {
            get { return _dueDate_AH_o; }
            set { _dueDate_AH_o = value; }
        }

        //18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        private string _recallDate_CJ_18 = "";
        public string RecallDate_CJ_18
        {
            get { return _recallDate_CJ_18; }
            set { _recallDate_CJ_18 = value; }
        }

        //18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        private string _holdPickupDate_CM_18 = "";
        public string HoldPickupDate_CM_18
        {
            get { return _holdPickupDate_CM_18; }
            set { _holdPickupDate_CM_18 = value; }
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

        //variable-length optional field
        private string _owner_BG_o = "";
        public string Owner_BG_o
        {
            get { return _owner_BG_o; }
            set { _owner_BG_o = value; }
        }

        //3 char, fixed-length optional field
        private string _currencyType_BH_o = "";
        public string CurrencyType_BH_o
        {
            get { return _currencyType_BH_o; }
            set { _currencyType_BH_o = value; }
        }

        //variable-length optional field.  The amount of the fee associated with this item.
        private string _feeAmount_BV_o = "";
        public string FeeAmount_BV_o
        {
            get { return _feeAmount_BV_o; }
            set { _feeAmount_BV_o = value; }
        }

        //3-char, fixed-length optional field
        private string _mediaType_CK_o = "";
        public string MediaType_CK_o
        {
            get { return _mediaType_CK_o; }
            set { _mediaType_CK_o = value; }
        }

        //variable-length optional field
        private string _permanentLocation_AQ_o = "";
        public string PermanentLocation_AQ_o
        {
            get { return _permanentLocation_AQ_o; }
            set { _permanentLocation_AQ_o = value; }
        }

        //variable-length optional field
        private string _currentLocation_AP_o = "";
        public string CurrentLocation_AP_o
        {
            get { return _currentLocation_AP_o; }
            set { _currentLocation_AP_o = value; }
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
