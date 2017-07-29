using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2
{
    public class BaseMessage
    {
        //The sequence number is a single ASCII digit, '0' to '9'.  
        //When error detection is enabled, the SC will increment the sequence number field for each new message it transmits. 
        //The ACS should verify that the sequence numbers increment as new messages are received from the 3M SelfCheck system.  
        //When error detection is enabled, the ACS response to a message should include a sequence number field also, where the sequence number field’s value matches the sequence number value from the message being responded to.
        private string _sequenceNumber_AY = null;// 0-9
        public string SequenceNumber_AY
        {
            get { return _sequenceNumber_AY; }
            set { _sequenceNumber_AY = value; }
        }

        //The checksum is four ASCII character digits representing the binary sum of the characters including the first character of the transmission and up to and including the checksum field identifier characters.
        //To calculate the checksum add each character as an unsigned binary number, take the lower 16 bits of the total and perform a 2's complement.  The checksum field is the result represented by four hex digits.
        //To verify the correct checksum on received data, simply add all the hex values including the checksum.  It should equal zero.
        private string _checksum_AZ = null;// 4位16进制
        public string Checksum_AZ
        {
            get { return _checksum_AZ; }
            set { _checksum_AZ = value; }
        }

        //完整的AYAZ字段
        public string _AYAZ = "";


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
