
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DigitalPlatform.SIP2;
using DigitalPlatform.SIP2.Request;
using DigitalPlatform.SIP2.Response;

using DigitalPlatform.LibraryClient;
using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.Xml;
using DigitalPlatform.IO;
using DigitalPlatform.Marc;
using DigitalPlatform;

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

        /// <summary>
        /// 还书
        /// </summary>
        /// <param name="channel">ILS 通道</param>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string Checkin(LibraryChannel channel, string message)
        {
            string strError = "";
            string strItemBarcode = "";

            CheckinResponse_10 response = new CheckinResponse_10()
            {
                Ok_1 = "0",
                Resensitize_1 = "Y",
                MagneticMedia_1 = "N",
                Alert_1 = "N",
                TransactionDate_18 = SIPUtility.NowDateTime,
                AO_InstitutionId_r = "dp2Library"
            };

            Checkin_09 request = new Checkin_09();

            bool bRet = request.parse(message, out strError);
            if (!bRet)
            {
                response.AF_ScreenMessage_o = strError;
            }
            else
            {
                strItemBarcode = request.AB_ItemIdentifier_r;
            }

            if (!string.IsNullOrEmpty(strItemBarcode))
            {
                response.AB_ItemIdentifier_r = strItemBarcode;

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
                if (-1 == lRet)
                {
                    if (channel.ErrorCode == ErrorCode.NotBorrowed)
                        response.Ok_1 = "1";

                    response.AF_ScreenMessage_o = strError;
                }
                else
                {
                    response.Ok_1 = "1";
                    response.AA_PatronIdentifier_o = strOutputReaderBarcode;
                    response.AF_ScreenMessage_o="成功";
                    response.AG_PrintLine_o="成功";

                    if (itemRecords != null && itemRecords.Length > 0)
                    {
                        XmlDocument dom = new XmlDocument();
                        try
                        {
                            dom.LoadXml(itemRecords[0]);

                            response.AQ_PermanentLocation_r = DomUtil.GetElementText(dom.DocumentElement, "location");

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
                            response.AJ_TitleIdentifier_o=record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                        else if (strMarcSyntax == "usmarc")
                            response.AJ_TitleIdentifier_o = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                    }
                    else
                    {
                        strError = "书目信息解析错误：" + strError;
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
                        response.AJ_TitleIdentifier_o = strSummary;
                }
            }

            return response.ToText();
        }

        /// <summary>
        /// 续借
        /// </summary>
        /// <param name="channel">ILS 通道</param>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string Renew(LibraryChannel channel, string message)
        {
            long lRet = 0;

            string strError = "";

            RenewResponse_30 response = new RenewResponse_30()
            {
                Ok_1 = "0",
                RenewalOk_1 = "N",
                MagneticMedia_1 = "N",
                Desensitize_1 = "N",
                TransactionDate_18 = SIPUtility.NowDateTime,
                AO_InstitutionId_r = "dp2Library",
            };

            Renew_29 request = new Renew_29();
            bool bRet = request.parse(message, out strError);
            if (!bRet)
            {
                response.AF_ScreenMessage_o = strError;
            }
            else
            {
                string strItemBarcode = request.AB_ItemIdentifier_o;
                response.AB_ItemIdentifier_r = strItemBarcode;

                string strReaderBarcode = request.AA_PatronIdentifier_r;
                response.AA_PatronIdentifier_r = strReaderBarcode;

                string strPatronPassword = request.AD_PatronPassword_o;
                if (!string.IsNullOrEmpty(strPatronPassword))
                {
                    lRet = channel.VerifyReaderPassword(null,
                        strReaderBarcode,
                        strPatronPassword,
                        out strError);
                    if (-1 == lRet)
                    {
                        response.AF_ScreenMessage_o = "校验密码发生错误：" + strError;
                    }
                    else if (0 == lRet)
                    {
                        response.AF_ScreenMessage_o = "失败：密码错误";
                    }

                    return response.ToText();
                }


                string[] aDupPath = null;
                string[] item_records = null;
                string[] reader_records = null;
                string[] biblio_records = null;
                string strOutputReaderBarcode = "";
                BorrowInfo borrow_info = null;
                lRet = channel.Borrow(
                    null,   // stop,
                    true,  // 续借为 true
                    strReaderBarcode,    //读者证条码号
                    strItemBarcode,     // 册条码号
                    null, //strConfirmItemRecPath,
                    false,
                    null,   // this.OneReaderItemBarcodes,
                    "auto_renew,biblio,item", // strStyle, // auto_renew,biblio,item                   //  "reader,item,biblio", // strStyle,
                    "xml:noborrowhistory",  // strItemReturnFormats,
                    out item_records,
                    "summary",    // strReaderFormatList
                    out reader_records,
                    "xml",         //strBiblioReturnFormats,
                    out biblio_records,
                    out aDupPath,
                    out strOutputReaderBarcode,
                    out borrow_info,
                    out strError);
                if (-1 == lRet)
                {
                    response.AF_ScreenMessage_o = "失败：" + strError;
                }
                else
                {
                    response.Ok_1 = "1";
                    response.RenewalOk_1 = "Y";

                    string strBiblioSummary = String.Empty;
                    string strMarcSyntax = "";
                    MarcRecord record = MarcXml2MarcRecord(biblio_records[0],
                        out strMarcSyntax,
                        out strError);
                    if (record != null)
                    {
                        if (strMarcSyntax == "unimarc")
                        {
                            strBiblioSummary = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                        }
                        else if (strMarcSyntax == "usmarc")
                        {
                            strBiblioSummary = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                        }
                    }
                    else
                    {
                        strError = "书目信息解析错误：" + strError;
                        LogManager.Logger.Error(strError);
                    }

                    if (String.IsNullOrEmpty(strBiblioSummary))
                        strBiblioSummary = strItemBarcode;

                    response.AJ_TitleIdentifier_r = strBiblioSummary;

                    string strLatestReturnTime = DateTimeUtil.Rfc1123DateTimeStringToLocal(borrow_info.LatestReturnTime, this.DateFormat);
                    response.AH_DueDate_r = strLatestReturnTime;


                    response.AF_ScreenMessage_o = "成功";
                    response.AG_PrintLine_o = "成功";
                }
            }
            return response.ToText();
        }

        /// <summary>
        /// 借书
        /// </summary>
        /// <param name="channel">ILS 通道</param>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string Checkout(LibraryChannel channel, string message)
        {
            long lRet = 0;
            string strError = "";

            CheckoutResponse_12 response = new CheckoutResponse_12()
            {
                Ok_1 = "0",
                RenewalOk_1 = "N",
                MagneticMedia_1 = "N",
                Desensitize_1 = "Y",
                TransactionDate_18 = SIPUtility.NowDateTime,
                AO_InstitutionId_r = "dp2Library"
            };

            Checkout_11 request = new Checkout_11();
            bool bRet = request.parse(message, out strError);
            if (!bRet)
            {
                response.AF_ScreenMessage_o=strError;
            }
            else
            {
                string strItemBarcode = request.AB_ItemIdentifier_r;
                response.AB_ItemIdentifier_r = strItemBarcode;

                string strReaderBarcode = request.AA_PatronIdentifier_r;
                response.AA_PatronIdentifier_r = strReaderBarcode;

                string strCancel = request.BI_Cancel_1_o;
                if (String.IsNullOrEmpty(strCancel) || "N" == strCancel)
                {
                    string strPatronPassword = request.AD_PatronPassword_o;
                    if (!string.IsNullOrEmpty(strPatronPassword))
                    {
                        lRet = channel.VerifyReaderPassword(null,
                            strReaderBarcode,
                            strPatronPassword,
                            out strError);
                        if (-1 == lRet)
                        {
                            response.AF_ScreenMessage_o = "校验密码发生错误：" + strError;
                        }
                        else if (0 == lRet)
                        {
                            response.AF_ScreenMessage_o = "失败：密码错误";
                        }

                        return response.ToText();
                    }

                    string[] aDupPath = null;
                    string[] item_records = null;
                    string[] reader_records = null;
                    string[] biblio_records = null;
                    string strOutputReaderBarcode = "";
                    BorrowInfo borrow_info = null;
                    lRet = channel.Borrow(
                        null,   // stop,
                        false,  // 续借为 true
                        strReaderBarcode,    //读者证条码号
                        strItemBarcode,     // 册条码号
                        null, //strConfirmItemRecPath,
                        false,
                        null,   // this.OneReaderItemBarcodes,
                        "auto_renew,biblio,item", // strStyle, // auto_renew,biblio,item                   //  "reader,item,biblio", // strStyle,
                        "xml:noborrowhistory",  // strItemReturnFormats,
                        out item_records,
                        "summary",    // strReaderFormatList
                        out reader_records,
                        "xml",         //strBiblioReturnFormats,
                        out biblio_records,
                        out aDupPath,
                        out strOutputReaderBarcode,
                        out borrow_info,
                        out strError);
                    if (-1 == lRet)
                    {
                        response.AF_ScreenMessage_o = "失败：" + strError;
                    }
                    else
                    {
                        response.Ok_1 = "1";

                        string strBiblioSummary = String.Empty;
                        string strMarcSyntax = "";
                        MarcRecord record = MarcXml2MarcRecord(biblio_records[0],
                            out strMarcSyntax,
                            out strError);
                        if (record != null)
                        {
                            if (strMarcSyntax == "unimarc")
                            {
                                strBiblioSummary = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                            }
                            else if (strMarcSyntax == "usmarc")
                            {
                                strBiblioSummary = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                            }
                        }
                        else
                        {
                            strError = "书目信息解析错误：" + strError;
                            LogManager.Logger.Error(strError);
                        }

                        if (String.IsNullOrEmpty(strBiblioSummary))
                            strBiblioSummary = strItemBarcode;

                        response.AJ_TitleIdentifier_r=strBiblioSummary;

                        string strLatestReturnTime = DateTimeUtil.Rfc1123DateTimeStringToLocal(borrow_info.LatestReturnTime, this.DateFormat);
                        response.AH_DueDate_r=strLatestReturnTime;


                        response.AF_ScreenMessage_o = "成功";
                        response.AG_PrintLine_o="成功";
                    }
                }
            }
            return response.ToText();
        }
    }
}
