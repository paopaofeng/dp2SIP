using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dp2SIPServer.SIP2
{
    public abstract class SIP2Message
    {
        public const string VARIABLE_LENGTH_FIELD_TERMINATOR = "|";

        public const string MESSAGE_TERMINATOR = "\n";

        public const string LANGUAGE_UNKNOWN = "000";

        public const string LANGUAGE_ENGLISH = "001";

        public const string LANGUAGE_FRENCH = "002";

        public const string LANGUAGE_GERMAN = "003";

        public const string LANGUAGE_ITALIAN = "004";

        public const string LANGUAGE_DUTCH = "005";

        public const string LANGUAGE_SWEDISH = "006";

        public const string LANGUAGE_FINNISH = "007";

        public const string LANGUAGE_SPANISH = "008";

        public const string LANGUAGE_DANISH = "009";

        public const string LANGUAGE_PORTUGUESE = "010";

        public const string LANGUAGE_CANADIAN_FRENCH = "011";

        public const string LANGUAGE_NORWEGIAN = "012";

        public const string LANGUAGE_HEBREW = "013";

        public const string LANGUAGE_JAPANESE = "014";

        public const string LANGUAGE_RUSSIAN = "015";

        public const string LANGUAGE_ARABIC = "016";

        public const string LANGUAGE_POLISH = "017";

        public const string LANGUAGE_GREEK = "018";

        public const string LANGUAGE_CHINESE = "019";

        public const string LANGUAGE_KOREAN = "020";

        public const string LANGUAGE_NORTH_AMERICAN_SPANISH = "021";

        public const string LANGUAGE_TAMIL = "022";

        public const string LANGUAGE_MALAY = "023";

        public const string LANGUAGE_UNITED_KINGDOM = "024";

        public const string LANGUAGE_ICELANDIC = "025";

        public const string LANGUAGE_BELGIAN = "026";

        public const string LANGUAGE_TAIWANESE = "027";

        public const string MEDIA_TYPE_OTHER = "000";

        public const string MEDIA_TYPE_BOOK = "001";

        public const string MEDIA_TYPE_ZINE = "002";

        public const string MEDIA_TYPE_BOUND_JOURNAL = "003";

        public const string MEDIA_TYPE_AUDIO_TAPE = "004";

        public const string MEDIA_TYPE_VIDEO_TAPE = "005";

        public const string MEDIA_TYPE_CD = "006";

        public const string MEDIA_TYPE_DISKETTE = "007";

        public const string MEDIA_TYPE_BOOK_WITH_DISKETTE = "008";

        public const string MEDIA_TYPE_BOOK_WITH_CD = "009";

        public const string MEDIA_TYPE_BOOK_WITH_AUDIO_TAPE = "010";

        public const string PAYMENT_TYPE_CASH = "00";

        public const string PAYMENT_TYPE_VISA = "01";

        public const string PAYMENT_TYPE_CREDIT_CARD = "02";

        public const string CURRENCY_TYPE_USD = "USD";

        public const string CURRENCY_TYPE_CAD = "CAD";

        public const string CURRENCY_TYPE_GBP = "GBP";

        public const string CURRENCY_TYPE_FRF = "FRF";

        public const string CURRENCY_TYPE_DEM = "DEM";

        public const string CURRENCY_TYPE_ITL = "ITL";

        public const string CURRENCY_TYPE_ESP = "ESP";

        public const string CURRENCY_TYPE_JPY = "JPY";

        public const int MSIP_NO_ERROR = 0;

        public static int ACS_CHAR_SET = 850;


        public static char ParseSIPChar(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Trim().Length == 1)
            {
                return value[0];
            }
            return 'N';
        }

        public string MessageIdentifier;

        protected char _ok;

        protected char _resensitize;

        protected char _magneticMedia;

        protected char _alert;

        protected string _transactionDate;

        protected string _institutionId;

        protected string _itemIdentifier;

        protected string _permanentLocation;

        protected string _titleIdentifier;

        protected string _sortBin;

        protected string _patronIdentifier;

        protected string _mediaType;

        protected string _itemProperties;

        protected string _patronStatus;

        protected string _language;

        protected string _holdItemsCount;

        protected string _holdItems;

        protected string _holdItemsLimit;

        protected string _overdueItemsCount;

        protected string _overdueItems;

        protected string _overdueItemsLimit;

        protected string _chargedItemsCount;

        protected string _chargedItems;

        protected string _chargedItemsLimit;

        protected string _fineItemsCount;

        protected string _fineItems;

        protected string _recallItemsCount;

        protected string _recallItems;

        protected string _unavailableHoldsCount;

        protected string _unavailableHoldItems;

        protected string _personalName;

        protected string _homeAddress;

        protected string _emailAddress;

        protected string _homePhoneNumber;

        protected char _validPatron;

        protected char _validPatronPassword;

        protected string _currencyType;

        protected string _feeAmount;

        protected string _feeLimit;

        protected string _circulationStatus;

        protected string _securityMarker;

        protected string _feeType;

        protected string _holdQueueLength;

        protected string _dueDate;

        protected string _recallDate;

        protected string _holdPickupDate;

        protected string _owner;

        protected string _currentLocation;

        protected char _renewalOk;

        protected char _desensitize;

        protected char _securityInhibit;

        protected char _onlineStatus;
        protected char _checkInOk;
        protected char _checkOutOk;
        protected char _acsRenewalPolicy;
        protected char _statusUpdateOk;
        protected char _offLineOk;
        protected string _timeoutPeriod;
        protected string _retriesAllowed;
        protected string _datetimesync;
        protected string _protocolVersion;
        protected string _libraryName;
        protected string _supportedMessages;
        protected string _terminalLocation;

        protected char _uidAlgorithm;
        protected char _pwdAlgorithm;
        protected string _locationCode;

        protected string _loginPassword;

        protected string _loginUserId;

        protected string _thirdPartyAllowed;
        protected char _noBlock;
        
        protected string _nbDueDate;

        protected string _patronPassword;
        
        protected string _terminalPassword;
        protected char _feeAcknowledged;

        protected char _cancel;
        protected string _transactionId;
        protected string _endItem;
        protected string _startItem;


        protected string _blockedCardMsg;
        protected string _renewedItems;
        protected string _unrenewedItems;
        protected string _expirationDate;
        protected string _queuePosition;
        protected string _pickupLocation;
        protected char _holdType;
        protected string _feeIdentifier;

        protected string _returnDate;

        protected char _sequenceNumber;
        protected string _checksum;

        protected string _screenMessage;

        protected string _printLine;
    }
}
