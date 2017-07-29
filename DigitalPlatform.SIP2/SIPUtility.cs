using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalPlatform.SIP2
{
    public class SIPUtility
    {
        #region 常量

        // 变长字段分隔符
        public const string C_FIELD_TERMINATOR = "|";

        //消息结束符
        public const string C_MESSAGE_TERMINATOR = "\n";

        // language
        public const string C_LANGUAGE_UNKNOWN = "000";
        public const string C_LANGUAGE_ENGLISH = "001";
        public const string C_LANGUAGE_CHINESE = "019";

        //media type
        public const string C_MEDIA_TYPE_OTHER = "000";
        public const string C_MEDIA_TYPE_BOOK = "001";
        public const string C_MEDIA_TYPE_ZINE = "002";
        public const string C_MEDIA_TYPE_BOUND_JOURNAL = "003";
        public const string C_MEDIA_TYPE_AUDIO_TAPE = "004";
        public const string C_MEDIA_TYPE_VIDEO_TAPE = "005";
        public const string C_MEDIA_TYPE_CD = "006";
        public const string C_MEDIA_TYPE_DISKETTE = "007";
        public const string C_MEDIA_TYPE_BOOK_WITH_DISKETTE = "008";
        public const string C_MEDIA_TYPE_BOOK_WITH_CD = "009";
        public const string C_MEDIA_TYPE_BOOK_WITH_AUDIO_TAPE = "010";

        //payment type
        public const string C_PAYMENT_TYPE_CASH = "00";
        public const string C_PAYMENT_TYPE_VISA = "01";
        public const string C_PAYMENT_TYPE_CREDIT_CARD = "02";

        // currency type
        public const string C_CURRENCY_TYPE_USD = "USD";
        public const string C_CURRENCY_TYPE_CAD = "CAD";
        public const string C_CURRENCY_TYPE_GBP = "GBP";
        public const string C_CURRENCY_TYPE_FRF = "FRF";
        public const string C_CURRENCY_TYPE_DEM = "DEM";
        public const string C_CURRENCY_TYPE_ITL = "ITL";
        public const string C_CURRENCY_TYPE_ESP = "ESP";
        public const string C_CURRENCY_TYPE_JPY = "JPY";

        // 无错误
        public const int C_MSIP_NO_ERROR = 0;

        // ACS字符集
        public static int C_ACS_CHAR_SET = 850;

        #endregion

        public static string NowDateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd    HHmmss");
            }
        }
    }
}
