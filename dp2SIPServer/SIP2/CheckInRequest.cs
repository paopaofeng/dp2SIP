using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dp2SIPServer.SIP2
{
    public class CheckInRequest : SIP2Request
    {
        public char NoBlock
        {
            get
            {
                return this._noBlock;
            }
        }

        public string TransactionDate
        {
            get
            {
                return this._transactionDate;
            }
        }

        public string ReturnDate
        {
            get
            {
                return this._returnDate;
            }
        }

        public string CurrentLocation
        {
            get
            {
                return this._currentLocation;
            }
        }

        public string InstitutionId
        {
            get
            {
                return this._institutionId;
            }
        }

        public string ItemIdentifier
        {
            get
            {
                return this._itemIdentifier;
            }
        }

        public string TerminalPassword
        {
            get
            {
                return this._terminalPassword;
            }
        }

        public string ItemProperties
        {
            get
            {
                return this._itemProperties;
            }
        }

        public char Cancel
        {
            get
            {
                return this._cancel;
            }
        }

        public CheckInRequest(string message)
        {
            if(!string.IsNullOrEmpty(message) 
                && message.StartsWith("09") 
                && message.Length >= 39)
            {
                int nIndex = 0;
                this.MessageIdentifier = message.Substring(nIndex, 2);

                nIndex += 2;
                this._noBlock = message[nIndex];

                nIndex++;
                this._transactionDate = message.Substring(nIndex, 18);

                nIndex += 18;
                this._returnDate = message.Substring(nIndex, 18);

                nIndex += 18;
                base.RequestInit(message.Substring(nIndex));
            }
        }
    }
}
