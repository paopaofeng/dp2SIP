using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2
{
    public class SIPUtility
    {
        public static string NowDateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd    HHmmss");
            }
        }
    }
}
