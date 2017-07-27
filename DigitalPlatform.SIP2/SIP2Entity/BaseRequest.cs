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


        // 解析字符串命令为对象
        public virtual bool parse(string text, out string error)
        {
            error = "未实现参数校验";
            bool ret = false;

            return ret;
        }

        // 将对象转换字符串命令
        public virtual string ToText()
        {
            return "未实现";
        }

        // 校验对象的各参数是否合法
        public virtual bool Verify(out string error)
        {
            error = "未实现";
            return false;
        }
    }
}
