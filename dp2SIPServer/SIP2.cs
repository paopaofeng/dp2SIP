#define OLD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using dp2SIPServer.SIP2;
using DigitalPlatform.SIP2.SIP2Entity;

using DigitalPlatform.LibraryClient;
using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.Xml;
using DigitalPlatform.IO;
using DigitalPlatform.Marc;

namespace dp2SIPServer
{
    public class SIP
    {
        string DateFormat
        {
            get
            {
                string strDateFormat = Properties.Settings.Default.DateFormat;
                if (string.IsNullOrEmpty(strDateFormat) || strDateFormat.Length < 8)
                    strDateFormat = "yyyy-MM-dd";
                return strDateFormat;
            }
        }

        static MarcRecord MarcXml2MarcRecord(string strMarcXml,
            out string strOutMarcSyntax,
            out string strError)
        {
            MarcRecord record = null;

            strError = "";
            strOutMarcSyntax = "";

            string strMARC = "";
            int nRet = MarcUtil.Xml2Marc(strMarcXml,
                false,
                "",
                out strOutMarcSyntax,
                out strMARC,
                out strError);
            if (nRet == 0)
                record = new MarcRecord(strMARC);
            else
                strError = "MarcXml转换错误:" + strError;

            return record;
        }

        public string CheckIn(LibraryChannel channel, string message)
        {
            string strError = "";
            string strItemBarcode = "";

            CheckInResponse response = new CheckInResponse();

#if OLD
            CheckInRequest request = new CheckInRequest(message);
            strItemBarcode = request.ItemIdentifier;
#else
            BaseRequest request = null;
            bool bRet = SCRequestFactory.ParseRequest(message, out request, out strError);
            if(!bRet)
            {
                LogManager.Logger.Error(strError);
                response.ScreenMessage = strError;
                return response.GetMessage();
            }

            Checkin_09 checkinRequest = request as Checkin_09;
            if (checkinRequest != null)
                strItemBarcode = checkinRequest.itemIdentifier_AB_r;
#endif
            if(!string.IsNullOrEmpty(strItemBarcode))
            {
                response.ItemIdentifier = strItemBarcode;

                string[] itemRecords = null;
                string[] readerRecords = null;
                string[] biblioRecords = null;
                string[] aDupPath = null;
                string strOutputReaderBarcode = "";
                ReturnInfo return_info = null;
                long lRet = channel.Return(null,
                    "return",
                    "",    //strReaderBarcode,
                    strItemBarcode,
                    "", // strConfirmItemRecPath
                    false,
                    "item,biblio",
                    "xml",
                    out itemRecords,
                    "xml",
                    out readerRecords,
                    "xml",
                    out biblioRecords,
                    out aDupPath,
                    out strOutputReaderBarcode,
                    out return_info,
                    out strError);
                if(-1 == lRet)
                {
                    response.OK = '0';
                    if (channel.ErrorCode == ErrorCode.NotBorrowed)
                        response.OK = '1';

                    response.ScreenMessage = strError;
                    response.PrintLine = strError;
                }
                else
                {
                    response.OK = '1';
                    response.PatronIdentifier = strOutputReaderBarcode;
                    response.ScreenMessage = "还书成功";
                    response.PrintLine = "还书成功";

                    if(itemRecords != null && itemRecords.Length > 0)
                    {
                        XmlDocument dom = new XmlDocument();
                        try
                        {
                            dom.LoadXml(itemRecords[0]);

                            response.PermanentLocation = DomUtil.GetElementText(dom.DocumentElement, "location");

                            /*
                             strPrice = DomUtil.GetElementText(item_dom.DocumentElement, "price");
                             strBookType = DomUtil.GetElementText(item_dom.DocumentElement, "bookType");
                             string strReturnDate = DomUtil.GetAttr(dom.DocumentElement, "borrowHistory/borrower", "returnDate");
                             if (String.IsNullOrEmpty(strReturnDate) == false)
                                 strReturnDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strReturnDate, this.DateFormat);
                             else
                                strReturnDate = DateTime.Now.ToString(this.DateFormat);
                            */
                        }
                        catch (Exception ex)
                        {
                            LogManager.Logger.Error(ex.Message);
                        }
                    }
                }

                if (biblioRecords != null && biblioRecords.Length > 0)
                {
                    string strTitle = String.Empty;
                    string strMarcSyntax = "";
                    MarcRecord record = MarcXml2MarcRecord(biblioRecords[0], out strMarcSyntax, out strError);
                    if (record != null)
                    {
                        if (strMarcSyntax == "unimarc")
                            response.TitleIdentifier = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                        else if (strMarcSyntax == "usmarc")
                            response.TitleIdentifier = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                    }
                    else
                    {
                        strError = "书目信息解析错误:" + strError;
                        LogManager.Logger.Error(strError);
                    }
                }
                else
                {
                    string strBiblioRecPath = "";
                    string strSummary = "";
                    lRet = channel.GetBiblioSummary(null,
                        strItemBarcode,
                        "",
                        "",
                        out strBiblioRecPath,
                        out strSummary,
                        out strError);
                    if (-1 != lRet)
                        response.TitleIdentifier = strSummary;
                }
            }

            return response.GetMessage();
        }
    }
}
