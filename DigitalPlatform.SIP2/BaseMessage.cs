using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2
{
    public class BaseMessage
    {
        // 命令指示符
        public string CommandIdentifier { get; set; }

        //The sequence number is a single ASCII digit, '0' to '9'.  
        //When error detection is enabled, the SC will increment the sequence number field for each new message it transmits. 
        //The ACS should verify that the sequence numbers increment as new messages are received from the 3M SelfCheck system.  
        //When error detection is enabled, the ACS response to a message should include a sequence number field also, where the sequence number field’s value matches the sequence number value from the message being responded to.
        private string _sequenceNumber_AY { get; set; }

        //The checksum is four ASCII character digits representing the binary sum of the characters including the first character of the transmission and up to and including the checksum field identifier characters.
        //To calculate the checksum add each character as an unsigned binary number, take the lower 16 bits of the total and perform a 2's complement.  The checksum field is the result represented by four hex digits.
        //To verify the correct checksum on received data, simply add all the hex values including the checksum.  It should equal zero.
        // 4位16进制
        private string _checksum_AZ { get; set; }

        // 定长字段数组
        public List<FixedLengthField> FixedLengthFields = new List<FixedLengthField>();
        
        // 变长字段数组
        public List<VariableLengthField> VariableLengthFields = new List<VariableLengthField>();

        #region 定长字段

        // 获取某个定长字段
        public FixedLengthField GetFixedField(string name)
        {
            foreach (FixedLengthField field in this.FixedLengthFields)
            {
                if (field.Name == name)
                    return field;
            }
            return null;
        }

        public string GetFixedFieldValue(string name)
        {
            FixedLengthField field = this.GetFixedField(name);
            if (field == null)
                throw new Exception("未定义定长字段" + name);

            return field.Value;
        }

        // 设置某个定长字段的值
        public void SetFixedFieldValue(string name, string value)
        {
            FixedLengthField field = this.GetFixedField(name);
            if (field == null)
                throw new Exception("未定义定长字段" + name);

            field.Value = value;
        }
        #endregion

        #region 变长字段

        // 获取某个定长字段
        public VariableLengthField GetVariableField(string id)
        {
            foreach (VariableLengthField field in this.VariableLengthFields)
            {
                if (field.ID == id)
                    return field;
            }
            return null;
        }

        public string GetVariableFieldValue(string id)
        {
            VariableLengthField field = this.GetVariableField(id);
            if (field == null)
                throw new Exception("未定义变长字段" + id);

            return field.Value;
        }

        // 设置某个定长字段的值
        public void SetVariableFieldValue(string id, string value)
        {
            VariableLengthField field = this.GetVariableField(id);
            if (field == null)
                throw new Exception("未定义变长字段" + id);

            field.Value = value;
        }
        #endregion

        // 解析字符串命令为对象
        public virtual bool parse(string text, out string error)
        {
            error = "";

            if (text == null || text.Length < 2)
            {
                error = "命令字符串为null或长度小于2位";
                return false;
            }
            this.CommandIdentifier = text.Substring(0, 2);  //命令指示符
            string conent = text.Substring(2); //内容

            // 给定长字段赋值
            int start = 0;
            foreach (FixedLengthField field in this.FixedLengthFields)
            {
                field.Value = conent.Substring(start, field.Length);
                start += field.Length;
            }


            //处理后面的变长字段
            string rest = conent.Substring(start);
            string[] parts = rest.Split(new char[] { '|' });
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length < 2)
                {
                    continue;
                }

                string fieldId = part.Substring(0, 2);
                string value = part.Substring(2);

                this.SetVariableFieldValue(fieldId, value);
            }

            // 校验;
            bool ret = this.Verify(out error);
            if (ret == false)
                return false;

            return true;
        }

        // 将对象转换字符串命令
        public virtual string ToText()
        {
            Debug.Assert(String.IsNullOrEmpty(this.CommandIdentifier) == false, "命令指示符未赋值");
            string text = this.CommandIdentifier;

            foreach (FixedLengthField field in this.FixedLengthFields)
            {
                if (field.Value == null || field.Value.Length != field.Length)
                    throw new Exception("定长字段[" + field.Name + "]的值为null或者长度不符合定义");
                text += field.Value;
            }

            foreach (VariableLengthField field in this.VariableLengthFields)
            {
                if (field.Value != null)
                {
                    text += field.ID + field.Value + SIPConst.FIELD_TERMINATOR;
                }
            }

            return text;
        }

        // 校验对象的各参数是否合法
        public virtual bool Verify(out string error)
        {
            error = "";

            // 校验定长字段
            foreach (FixedLengthField field in this.FixedLengthFields)
            {
                if (field.Value == null || field.Value.Length != field.Length)
                {
                    error = field.Name + "的值为null或者长度不符合要求的长度";
                    return false;
                }
            }

            foreach (VariableLengthField field in this.VariableLengthFields)
            {
                if (field.IsRequired==true &&  field.Value == null)
                {
                    error = field.ID + "是必备字段，必须有值";
                    return false;
                }
            }

            return true;
        }
    }
}
