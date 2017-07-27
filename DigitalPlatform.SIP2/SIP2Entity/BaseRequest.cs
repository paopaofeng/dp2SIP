using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2.SIP2Entity
{
    public class BaseRequest
    {
        public string sequenceNumber_AY = null;// 0-9
        public string checksum_AZ = null;// 4位16进制

        public string AYAZ = "";


        // 校验命令的各参数是否正确
        public virtual bool parse(string text, out string error)
        {
            error = "未实现参数校验";
            bool ret = false;

            return ret;
        }
    }
}
