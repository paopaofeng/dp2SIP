using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*
     2.00 End Session Response
     The ACS must send this message in response to the End Patron Session message.
     36<end session>< transaction date >< institution id >< patron identifier ><screen message><print line>
     36	1-char	18-char	AO	AA	AF	AG
     */
    public class EndSessionResponse_36 : BaseMessage
    {
        public EndSessionResponse_36()
        {
            this.CommandIdentifier = "36";

            //==前面的定长字段
            //<end session>< transaction date >
            //1-char	18-char
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_EndSession, 1));
            this.FixedLengthFields.Add(new FixedLengthField(SIPConst.F_TransactionDate, 18));

            //==后面变长字段
            //< institution id >< patron identifier ><screen message><print line>
            //AO	AA	AF	AG
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AO_InstitutionId, true));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AA_PatronIdentifier, true));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AF_ScreenMessage, false ));
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AG_PrintLine, false ));

            // 校验码相关，todo
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AY_SequenceNumber, false));

        }

        /*
        //1-char, fixed-length required field:  Y or N.
        public string EndSession_1{ get; set; }

        //18-char, fixed-length required field:  YYYYMMDDZZZZHHMMSS
        public string TransactionDate_18{ get; set; }

        //variable-length required field
        public string InstitutionId_AO_r{ get; set; }

        //variable-length required field.
        public string PatronIdentifier_AA_r{ get; set; }

        //variable-length optional field
        public string ScreenMessage_AF_o{ get; set; }

        //variable-length optional field
        public string PrintLine_AG_o{ get; set; }
        */
    }
}
