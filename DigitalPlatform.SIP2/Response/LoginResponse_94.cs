using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.Response
{
    /*<summary>
     * 2.00 Login Response
     * The ACS should send this message in response to the Login message.When this message is used, it will be the first message sent to the SC.
     * 94<ok>
     */
    public class LoginResponse_94 : BaseResponse
    {
        //1-char, fixed-length required field:  0 or 1.
        public string Ok_1 { get; set; }
    }
}
