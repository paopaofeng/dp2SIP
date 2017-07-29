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
    public class BlockPatron_01 : BaseMessage
    {
        // 1-char, fixed-length required field:  Y or N.
        public string CardRetained_1{ get; set; }


        // 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }


        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }


        // variable-length required field
        public string BlockedCardMsg_AL_r{ get; set; }


        // variable-length required field
        public string PatronIdentifier_AA_r{ get; set; }


        // variable-length required field
        public string TerminalPassword_AC_r{ get; set; }



    }
}
