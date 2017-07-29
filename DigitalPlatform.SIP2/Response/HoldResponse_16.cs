using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
 2.00 Hold Response
 The ACS should send this message in response to the Hold message from the SC.
 16<ok><available><transaction date><expiration date><queue position><pickup location><institution id><patron identifier><item identifier><title identifier><screen message><print line>
     */
    public class HoldResponse_16 : BaseResponse
    {
        //1-char, fixed-length required field:  0 or 1.
        private string _ok_1 = "";
        public string Ok_1
        {
            get { return _ok_1; }
            set { _ok_1 = value; }
        }

        //1-char, fixed-length required field:  Y or N.
        private string _available_1 = "";
        public string Available_1
        {
            get { return _available_1; }
            set { _available_1 = value; }
        }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
        }

        //18-char, fixed-length optional field:  YYYYMMDDZZZZHHMMSS
        private string _expirationDate_BW_18 = "";
        public string ExpirationDate_BW_18
        {
            get { return _expirationDate_BW_18; }
            set { _expirationDate_BW_18 = value; }
        }

        //variable-length optional field
        private string _queuePosition_BR_o = "";
        public string QueuePosition_BR_o
        {
            get { return _queuePosition_BR_o; }
            set { _queuePosition_BR_o = value; }
        }

        //variable-length optional field
        private string _pickupLocation_BS_o = "";
        public string PickupLocation_BS_o
        {
            get { return _pickupLocation_BS_o; }
            set { _pickupLocation_BS_o = value; }
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

        //variable-length optional field
        private string _itemIdentifier_AB_o = "";
        public string ItemIdentifier_AB_o
        {
            get { return _itemIdentifier_AB_o; }
            set { _itemIdentifier_AB_o = value; }
        }

        //variable-length optional field
        private string _titleIdentifier_AJ_o = "";
        public string TitleIdentifier_AJ_o
        {
            get { return _titleIdentifier_AJ_o; }
            set { _titleIdentifier_AJ_o = value; }
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
