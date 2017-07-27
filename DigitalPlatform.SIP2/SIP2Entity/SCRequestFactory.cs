using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.SIP2Entity
{
    public class SCRequestFactory
    {
        public static bool ParseRequest(string text,out BaseRequest request,out string error)
        {
             request = new BaseRequest();
             error = "";

             if (text.Length < 2)
             {
                 error="命令长度不够2位";
                 return false;
             }

             string cmdIdentifiers = text.Substring(0, 2);
             text = text.Substring(2);
             switch (cmdIdentifiers)
             {
                 case "93": 
                     {
                         request = new Login_93();
                         return request.parse(text, out error);
                     }
                 case "99":
                     {
                         request = new SCStatus_99();
                         return request.parse(text, out error);
                     }
                 case "09":
                     {
                         request = new Checkin_09();
                         return request.parse(text, out error);
                     }
                 case "11":
                     {
                         request = new Checkout_11();
                         return request.parse(text, out error);
                     }
                 case "63":
                     {
                         request = new PatronInformation_63();
                         return request.parse(text, out error);
                     }
                 case "35":
                     {
                         request = new EndPatronSession_35();
                         return request.parse(text, out error);
                     }
                 case "17":
                     {
                         request = new ItemInformation_17();
                         return request.parse(text, out error);
                     }
                 case "29":
                     {
                         request = new Renew_29();
                         return request.parse(text, out error);
                     }

                 default:
                     error = "无法识别的命令'" + cmdIdentifiers + "'";
                     return false;
             }


            return true;
        }
    }
}
