using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
     Fee Paid
     This message can be used to notify the ACS that a fee has been collected from the patron. The ACS should record this information in their database and respond with a Fee Paid Response message.
     37<transaction date><fee type><payment type><currency type><fee amount><institution id><patron identifier><terminal password><patron password><fee identifier><transaction id>
     */
    public class FeePaid_37 : BaseMessage
    {
        // 18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        // 2-char, fixed-length required field (01 thru 99). identifies a fee type to apply  the payment to.
        public string FeeType_2{ get; set; }

        // 2-char, fixed-length required field (00 thru 99)
        public string PaymentType_2{ get; set; }

        // 3-char, fixed-length required field
        public string CurrencyType_3{ get; set; }

        // variable-length required field; the amount paid.
        public string FeeAmount_BV_r{ get; set; }

        // variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        // variable-length required field.
        public string PatronIdentifier_AA_r{ get; set; }

        // variable-length optional field
        public string TerminalPassword_AC_o{ get; set; }

        // variable-length optional field
        public string PatronPassword_AD_o{ get; set; }

        // variable-length optional field; identifies a specific fee to apply the payment to.
        public string FeeIdentifier_CG_o{ get; set; }

        // variable-length optional field; a transaction id assigned by the payment device.
        public string TransactionId_BK_o{ get; set; }

    }
}
