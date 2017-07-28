using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
     2.00 Item Status Update
     This message can be used to send item information to the ACS, without having to do a Checkout or Checkin operation.  The item properties could be stored on the ACS’s database.  The ACS should respond with an Item Status Update Response message.
     19<transaction date><institution id><item identifier><terminal password><item properties>
     */
    public class ItemStatusUpdate_19 : BaseRequest
    {
        // 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        private string _transactionDate_18 = "";
        public string TransactionDate_18
        {
            get { return _transactionDate_18; }
            set { _transactionDate_18 = value; }
        }

        // variable-length required field
        private string _institutionId_AO_r = "";
        public string InstitutionId_AO_r
        {
            get { return _institutionId_AO_r; }
            set { _institutionId_AO_r = value; }
        }

        //  variable-length required field
        private string _itemIdentifier_AB_r = "";
        public string ItemIdentifier_AB_r
        {
            get { return _itemIdentifier_AB_r; }
            set { _itemIdentifier_AB_r = value; }
        }

        //  variable-length optional field
        private string _terminalPassowrd_AC_o = "";
        public string TerminalPassowrd_AC_o
        {
            get { return _terminalPassowrd_AC_o; }
            set { _terminalPassowrd_AC_o = value; }
        }

        //  variable-length required field
        private string _itemProperties_CH_r = "";
        public string ItemProperties_CH_r
        {
            get { return _itemProperties_CH_r; }
            set { _itemProperties_CH_r = value; }
        }
    }
}
