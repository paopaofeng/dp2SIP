using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Request
{
    /*
    SC Status
    The SC status message sends SC status to the ACS.  It requires an ACS Status Response message reply from the ACS. This message will be the first message sent by the SC to the ACS once a connection has been established (exception: the Login Message may be sent first to login to an ACS server program). The ACS will respond with a message that establishes some of the rules to be followed by the SC and establishes some parameters needed for further communication.
    99<status code><max print width><protocol version>
     */
    public class SCStatus_99 : BaseMessage
    {
        // 构造函数
        public SCStatus_99()
        {
            this.CommandIdentifier = "99";
            FixedLengthFields.Add(new FixedLengthField(SIPConst.F_StatusCode,1));// 1-char, fixed-length required field: 0 or 1 or 2
            FixedLengthFields.Add(new FixedLengthField(SIPConst.F_MaxPrintWidth, 3));// 3-char, fixed-length required field
            FixedLengthFields.Add(new FixedLengthField(SIPConst.F_ProtocolVersion, 4));// 4-char, fixed-length required field:  x.xx

            // 校验码相关，todo
            this.VariableLengthFields.Add(new VariableLengthField(SIPConst.F_AY_SequenceNumber, false));

        }


    }
}
