using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
 This message requests that the patron card be blocked by the ACS.  This is, for example, sent when the patron is detected tampering with the SC or when a patron forgets to take their card.  The ACS should invalidate the patron’s card and respond with a Patron Status Response message.  The ACS could also notify the library staff that the card has been blocked.
 01<card retained><transaction date><institution id><blocked card msg><patron identifier><terminal password>
 01	1-char	18-char	AO	AL  AA	AC
     */
    public class BlockPatron_01 : BaseRequest
    {
        // 1-char, fixed-length required field:  Y or N.
        private string _cardRetained_1 = "";
        public string CardRetained_1
        {
            get { return _cardRetained_1; }
            set { _cardRetained_1 = value; }
        }

        // 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
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

        // variable-length required field
        private string _blockedCardMsg_AL_r = "";
        public string BlockedCardMsg_AL_r
        {
            get { return _blockedCardMsg_AL_r; }
            set { _blockedCardMsg_AL_r = value; }
        }

        // variable-length required field
        private string _patronIdentifier_AA_r = "";
        public string PatronIdentifier_AA_r
        {
            get { return _patronIdentifier_AA_r; }
            set { _patronIdentifier_AA_r = value; }
        }

        // variable-length required field
        private string _terminalPassword_AC_r = "";
        public string TerminalPassword_AC_r
        {
            get { return _terminalPassword_AC_r; }
            set { _terminalPassword_AC_r = value; }
        }


    }
}
