using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dp2SIPServer.SIP2
{
    public abstract class SIP2Request : SIP2Message
    {
        protected void RequestInit(string message)
        {
            string strMessage = message.Trim();
            if (string.IsNullOrEmpty(strMessage))
                return;

            string[] fields = strMessage.Split(new string[] 
            {
                VARIABLE_LENGTH_FIELD_TERMINATOR
            }, 
            StringSplitOptions.RemoveEmptyEntries);
            foreach (string field in fields)
            {
                string strField = field.Trim();
                if (string.IsNullOrEmpty(strField) || strField.Length < 2)
                    continue;

                string strFieldID = strField.Substring(0, 2);
                string strFieldData = strField.Substring(2);
                if (string.IsNullOrEmpty(strFieldData))
                    continue;

                switch(strFieldID)
                {
                    case "AA":
                        this._patronIdentifier = strFieldData;
                        break;
                    case "AB":
                        this._itemIdentifier = strFieldData;
                        break;
                    case "AC":
                        this._terminalPassword = strFieldData;
                        break;
                    case "AD":
                        this._patronPassword = strFieldData;
                        break;
                    case "AO":
                        this._institutionId = strFieldData;
                        break;
                    case "AP":
                        this._currentLocation = strFieldData;
                        break;
                    case "AJ":
                        this._titleIdentifier = strFieldData;
                        break;
                    case "AL":
                        this._blockedCardMsg = strFieldData;
                        break;
                    case "BO":
                        this._feeAcknowledged = ParseSIPChar(strFieldData);
                        break;
                    case "BI":
                        this._cancel = ParseSIPChar(strFieldData);
                        break;
                    case "BK":
                        this._transactionId = strFieldData;
                        break;
                    case "BS":
                        this._pickupLocation = strFieldData;
                        break;
                    case "BV":
                        this._feeAmount = strFieldData;
                        break;
                    case "BW":
                        this._expirationDate = strFieldData;
                        break;
                    case "BY":
                        this._holdType = ParseSIPChar(strFieldData);
                        break;
                    case "CH":
                        this._itemProperties = strFieldData;
                        break;
                    case "CN":
                        this._loginUserId = strFieldData;
                        break;
                    case "CO":
                        this._loginPassword = strFieldData;
                        break;
                    case "CG":
                        this._feeIdentifier = strFieldData;
                        break;
                    case "CP":
                        this._locationCode = strFieldData;
                        break;
                    case "BP":
                        this._startItem = strFieldData;
                        break;
                    case "BQ":
                        this._endItem = strFieldData;
                        break;
                }
            }
        }
    }
}
