using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    2.00 Hold
    This message is used to create, modify, or delete a hold.  The ACS should respond with a Hold Response message.  Either or both of the “item identifier” and “title identifier” fields must be present for the message to be useful.
    15<hold mode><transaction date><expiration date><pickup location><hold type><institution id><patron identifier><patron password><item identifier><title identifier><terminal password><fee acknowledged>
     */
    public class Hold_15 : BaseRequest
    {
        // 1-char, fixed-length required field  '+'/'-'/'*'  Add, delete, change
        private string _holdMode_1 = "";
        public string HoldMode_1
        {
            get { return _holdMode_1; }
            set { _holdMode_1 = value; }
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

        //variable-length, optional field
        private string _pickupLocation_BS_o = "";
        public string PickupLocation_BS_o
        {
            get { return _pickupLocation_BS_o; }
            set { _pickupLocation_BS_o = value; }
        }

        //1-char, optional field
        private string _holdType_BY_o = "";
        public string HoldType_BY_o
        {
            get { return _holdType_BY_o; }
            set { _holdType_BY_o = value; }
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
        private string _patronPassword_AD_o = "";
        public string PatronPassword_AD_o
        {
            get { return _patronPassword_AD_o; }
            set { _patronPassword_AD_o = value; }
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
        private string _terminalPassword_AC_o = "";
        public string TerminalPassword_AC_o
        {
            get { return _terminalPassword_AC_o; }
            set { _terminalPassword_AC_o = value; }
        }

        //1-char, optional field: Y or N.
        private string _feeAcknowledged_BO_o = "";
        public string FeeAcknowledged_BO_o
        {
            get { return _feeAcknowledged_BO_o; }
            set { _feeAcknowledged_BO_o = value; }
        }
    }
}
