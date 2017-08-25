
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
using DigitalPlatform.Text;

namespace dp2SIPServer
{
    public class SIP
    {
        public string LoginUserId
        {
            get;
            set;
        }

        public string LoginPassword
        {
            get;
            set;
        }

        public string LocationCode
        {
            get;
            set;
        }

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

        public static MarcRecord MarcXml2MarcRecord(string strMarcXml,
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
        /// To calculate the checksum add each character as an unsigned binary number,
        /// take the lower 16 bits of the total and perform a 2's complement. 
        /// The checksum field is the result represented by four hex digits.
        /// </summary>
        /// <param name="message">
        /// 内容中不包含 校验和(checksum)
        /// </param>
        /// <returns></returns>
        public static string GetChecksum(string message)
        {
            string checksum = "";

            try
            {
                ushort sum = 0;
                foreach (char c in message)
                {
                    sum += c;
                }

                ushort checksum_inverted_plus1 = (ushort)(~sum + 1);

                checksum = checksum_inverted_plus1.ToString("X4");
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
                checksum = null;
            }
            return checksum;
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
                AO_InstitutionId_r = "dp2Library",
                AJ_TitleIdentifier_o = string.Empty,
                AQ_PermanentLocation_r = string.Empty,
            };

            Checkin_09 request = new Checkin_09();

            int nRet = request.parse(message, out strError);
            if (-1 == nRet)
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
                    response.AF_ScreenMessage_o = "成功";
                    response.AG_PrintLine_o = "成功";

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
                            response.AJ_TitleIdentifier_o = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
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
                AJ_TitleIdentifier_r = string.Empty,
                AH_DueDate_r = string.Empty,
            };

            Renew_29 request = new Renew_29();
            int nRet = request.parse(message, out strError);
            if (-1 == nRet)
            {
                response.AF_ScreenMessage_o = strError;
                response.AG_PrintLine_o = strError;
            }
            else
            {
                string strItemIdentifier = request.AB_ItemIdentifier_o;
                string strPatronIdentifier = request.AA_PatronIdentifier_r;

                if (String.IsNullOrEmpty(strItemIdentifier)
                    || String.IsNullOrEmpty(strPatronIdentifier))
                {
                    strError = "读者标识和图书标识都不能是空值";
                    response.AF_ScreenMessage_o = strError;
                    response.AG_PrintLine_o = strError;

                    return response.ToText();
                }

                response.AA_PatronIdentifier_r = strPatronIdentifier;
                response.AB_ItemIdentifier_r = strItemIdentifier;

                string strPatronPassword = request.AD_PatronPassword_o;
                if (!string.IsNullOrEmpty(strPatronPassword))
                {
                    lRet = channel.VerifyReaderPassword(null,
                        strPatronIdentifier,
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
                    strPatronIdentifier,    //读者证条码号
                    strItemIdentifier,     // 册条码号
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
                        strBiblioSummary = strItemIdentifier;

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
                AO_InstitutionId_r = "dp2Library",
                AJ_TitleIdentifier_r = string.Empty,
                AH_DueDate_r = string.Empty,
            };

            Checkout_11 request = new Checkout_11();
            int nRet = request.parse(message, out strError);
            if (-1 == nRet)
            {
                response.AF_ScreenMessage_o = strError;
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

                        response.AJ_TitleIdentifier_r = strBiblioSummary;

                        string strLatestReturnTime = DateTimeUtil.Rfc1123DateTimeStringToLocal(borrow_info.LatestReturnTime, this.DateFormat);
                        response.AH_DueDate_r = strLatestReturnTime;


                        response.AF_ScreenMessage_o = "成功";
                        response.AG_PrintLine_o = "成功";
                    }
                }
            }
            return response.ToText();
        }

        /// <summary>
        /// 图书信息
        /// 图书状态
        ///  1		other -- 其他
        ///  2		on order -- 订购中
        ///  3		available -- 可借
        ///  4		charged -- 在借
        /// 12		lost -- 丢失
        /// 13		missing -- 没有找到
        ///  5		charged; not to be recalled until earliest recall date -- 在借
        ///  6		in process -- 
        ///  7		recalled -- 召回
        ///  8		waiting on hold shelf -- 等待上架
        ///  9		waiting to be re-shelved -- 倒架中
        /// 10		in transit between library locations
        /// 11		claimed returned
        /// </summary>
        /// <param name="channel">ILS 通道</param>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string ItemInfo(LibraryChannel channel, string message)
        {
            string strError = "";

            ItemInformationResponse_18 response = new ItemInformationResponse_18()
            {
                CirculationStatus_2 = "01",
                SecurityMarker_2 = "00",
                FeeType_2 = "01",
                TransactionDate_18 = SIPUtility.NowDateTime,
                CK_MediaType_o = SIPConst.MEDIA_TYPE_BOOK,
            };

            ItemInformation_17 request = new ItemInformation_17();
            int nRet = request.parse(message, out strError);
            if (-1 == nRet)
            {
                LogManager.Logger.Error(strError);
                response.AF_ScreenMessage_o = strError;
                response.AG_PrintLine_o = strError;
            }
            else
            {
                string strItemIdentifier = request.AB_ItemIdentifier_r;
                response.AB_ItemIdentifier_r = strItemIdentifier;
                string strItemXml = "";
                string strBiblio = "";
                long lRet = channel.GetItemInfo(null,
                    strItemIdentifier,
                    "xml",
                    out strItemXml,
                    "xml",
                    out strBiblio,
                    out strError);
                if (-1 >= lRet)
                {
                    response.CirculationStatus_2 = "01";

                    strError = "获得'" + strItemIdentifier + "'发生错误: " + strError;
                    response.AF_ScreenMessage_o = strError;
                    response.AG_PrintLine_o = strError;
                }
                else if (0 == lRet)
                {
                    response.CirculationStatus_2 = "13";

                    strError = strItemIdentifier + " 记录不存在";
                    response.AF_ScreenMessage_o = strError;
                    response.AG_PrintLine_o = strError;
                }
                else if (1 < lRet)
                {
                    response.CirculationStatus_2 = "01";
                    strError = strItemIdentifier + " 记录重复，需馆员处理";
                    response.AF_ScreenMessage_o = strError;
                    response.AG_PrintLine_o = strError;
                }
                else if (1 == lRet)
                {
                    XmlDocument dom = new XmlDocument();
                    try
                    {
                        dom.LoadXml(strItemXml);

                        string strItemState = DomUtil.GetElementText(dom.DocumentElement, "state");
                        if (String.IsNullOrEmpty(strItemState))
                        {
                            string strBorrower = DomUtil.GetElementText(dom.DocumentElement, "borrower");
                            response.CirculationStatus_2 = String.IsNullOrEmpty(strBorrower) ? "03" : "04";

                            XmlNodeList reservations = dom.DocumentElement.SelectNodes("reservations/request");
                            if (reservations != null)
                            {
                                response.CF_HoldQueueLength_o = reservations.Count.ToString();
                                /*
                                if (reservations.Count > 0)
                                    response.CirculationStatus_2 = "08"; // 预约保留架
                                */
                            }
                        }
                        else
                        {
                            if (StringUtil.IsInList("丢失", strItemState))
                                response.CirculationStatus_2 = "12";
                        }

                        response.AQ_PermanentLocation_o = DomUtil.GetElementText(dom.DocumentElement, "location");

                        string strBorrowDate = DomUtil.GetElementText(dom.DocumentElement, "borrowDate");
                        if (!String.IsNullOrEmpty(strBorrowDate))
                        {
                            strBorrowDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strBorrowDate, "yyyyMMdd    HHmmss");
                            response.CM_HoldPickupDate_18 = strBorrowDate;
                        }

                        string strReturningDate = DomUtil.GetElementText(dom.DocumentElement, "returningDate");
                        if (!String.IsNullOrEmpty(strReturningDate))
                        {
                            strReturningDate = DateTimeUtil.Rfc1123DateTimeStringToLocal(strReturningDate, this.DateFormat);
                            response.AH_DueDate_o = strReturningDate;
                        }

                        string strMarcSyntax = "";
                        MarcRecord record = MarcXml2MarcRecord(strBiblio, out strMarcSyntax, out strError);
                        if (record != null)
                        {
                            if (strMarcSyntax == "unimarc")
                            {
                                // strISBN = record.select("field[@name='010']/subfield[@name='a']").FirstContent;
                                response.AJ_TitleIdentifier_r = record.select("field[@name='200']/subfield[@name='a']").FirstContent;
                                // strAuthor = record.select("field[@name='200']/subfield[@name='f']").FirstContent;
                            }
                            else if (strMarcSyntax == "usmarc")
                            {
                                // strISBN = record.select("field[@name='020']/subfield[@name='a']").FirstContent;
                                response.AJ_TitleIdentifier_r = record.select("field[@name='245']/subfield[@name='a']").FirstContent;
                                // strAuthor = record.select("field[@name='245']/subfield[@name='c']").FirstContent;
                            }
                        }
                        else
                        {
                            strError = "图书信息解析错误:" + strError;
                            LogManager.Logger.Error(strError);

                            response.AF_ScreenMessage_o = strError;
                            response.AG_PrintLine_o = strError;
                        }
                    }
                    catch (Exception ex)
                    {
                        strError = strItemIdentifier + ":图书解析错误:" + ExceptionUtil.GetDebugText(ex);
                        LogManager.Logger.Error(strError);
                    }
                }
            }

            return response.ToText();
        }


        /// <summary>
        /// 读者信息
        /// </summary>
        /// <param name="channel">ILS 通道</param>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string PatronInfo(LibraryChannel channel, string message)
        {
            char[] patronStatus = new char[14];
            for (int i = 0; i < patronStatus.Length; i++)
            {
                patronStatus[i] = (char)0x20; // 空格
            }

            string strError = "";
            long lRet = 0;

            PatronInformationResponse_64 response = new PatronInformationResponse_64()
            {
                PatronStatus_14 = "              ",
                Language_3 = SIPConst.LANGUAGE_CHINESE,
                TransactionDate_18 = SIPUtility.NowDateTime,
                HoldItemsCount_4 = "0000",
                OverdueItemsCount_4 = "0000",
                ChargedItemsCount_4 = "0000",
                FineItemsCount_4 = "0000",
                RecallItemsCount_4 = "0000",
                UnavailableHoldsCount_4 = "0000",
                AO_InstitutionId_r = "dp2Library",
                AA_PatronIdentifier_r = string.Empty,
                AE_PersonalName_r = string.Empty,

                BL_ValidPatron_o = "N",
                CQ_ValidPatronPassword_o = "N",
            };

            PatronInformation_63 request = new PatronInformation_63();
            int nRet = request.parse(message, out strError);
            if (-1 == nRet)
            {
                LogManager.Logger.Error(strError);
                response.AF_ScreenMessage_o = strError;
                response.AG_PrintLine_o = strError;
            }
            else
            {
                string strPassword = request.AD_PatronPassword_o;
                string strBarcode = request.AA_PatronIdentifier_r;

                if (!string.IsNullOrEmpty(strPassword))
                {
                    lRet = channel.VerifyReaderPassword(null,
                        strBarcode,
                        strPassword,
                        out strError);
                    if (lRet == -1)
                    {
                        response.AF_ScreenMessage_o = "校验密码发生错误：" + strError;
                        response.AG_PrintLine_o = "校验密码发生错误：" + strError;

                        return response.ToText();
                    }
                    else if (lRet == 0)
                    {
                        response.AF_ScreenMessage_o = "卡号或密码不正确";
                        response.AG_PrintLine_o = "卡号或密码不正确";
                        return response.ToText();
                    }

                    response.CQ_ValidPatronPassword_o = "Y";
                }

                string[] results = null;
                lRet = channel.GetReaderInfo(null,
                    strBarcode, //读者卡号,
                    "advancexml",   // this.RenderFormat, // "html",
                    out results,
                    out strError);
                if (lRet <= -1)
                {
                    LogManager.Logger.Error(strError);
                    response.AF_ScreenMessage_o = "查询读者信息失败：" + strError;
                    response.AG_PrintLine_o = "查询读者信息失败：" + strError;
                    return response.ToText();
                }
                else if (lRet == 0)
                {
                    response.AF_ScreenMessage_o = "查无此证";
                    response.AG_PrintLine_o = "查无此证";
                    return response.ToText();
                }
                else if (lRet > 1)
                {
                    response.AF_ScreenMessage_o = "证号重复";
                    response.AG_PrintLine_o = "证号重复";
                    return response.ToText();
                }

                XmlDocument dom = new XmlDocument();
                string strReaderXml = results[0];
                try
                {
                    dom.LoadXml(strReaderXml);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.Error("读者信息解析错误：" + ExceptionUtil.GetDebugText(ex));
                    response.AF_ScreenMessage_o = "读者信息解析错误";
                    response.AG_PrintLine_o = "读者信息解析错误";
                    return response.ToText();
                }

                string strState = DomUtil.GetElementText(dom.DocumentElement, "state");
                if(!String.IsNullOrEmpty(strState))
                {
                    patronStatus[4] = 'Y';
                }


                // hold items count 4 - char, fixed-length required field -- 预约
                XmlNodeList holdItemNodes = dom.DocumentElement.SelectNodes("reservations/request");
                if (holdItemNodes != null)
                {
                    response.HoldItemsCount_4 = holdItemNodes.Count.ToString().PadLeft(4, '0');

                    List<VariableLengthField> holdItems = new List<VariableLengthField>();
                    foreach (XmlNode node in holdItemNodes)
                    {
                        string strItemBarcode = DomUtil.GetAttr(node, "items");
                        if (string.IsNullOrEmpty(strItemBarcode))
                            continue;

                        holdItems.Add(new VariableLengthField(SIPConst.F_AS_HoldItems, false, strItemBarcode));
                    }

                    if (holdItems.Count > 0)
                        response.AS_HoldItems_o = holdItems;
                }

                // overdue items count 4 - char, fixed-length required field  -- 超期
                // charged items count 4 - char, fixed-length required field -- 在借
                XmlNodeList chargedItemNodes = dom.DocumentElement.SelectNodes("borrows/borrow");
                if (chargedItemNodes != null)
                {
                    response.ChargedItemsCount_4 = chargedItemNodes.Count.ToString().PadLeft(4, '0');

                    List<VariableLengthField> chargedItems = new List<VariableLengthField>();
                    List<VariableLengthField> overdueItems = new List<VariableLengthField>();
                    int nOverdueItemsCount = 0;
                    foreach (XmlNode node in chargedItemNodes)
                    {
                        string strItemBarcode = DomUtil.GetAttr(node, "barcode");
                        if (string.IsNullOrEmpty(strItemBarcode))
                            continue;

                        chargedItems.Add(new VariableLengthField(SIPConst.F_AU_ChargedItems, false, strItemBarcode));

                        string strReturningDate = DomUtil.GetAttr(node, "returningDate");
                        if (string.IsNullOrEmpty(strReturningDate))
                            continue;
                        DateTime returningDate = DateTimeUtil.FromRfc1123DateTimeString(strReturningDate);
                        if (returningDate < DateTime.Now)
                        {
                            nOverdueItemsCount++;
                            overdueItems.Add(new VariableLengthField(SIPConst.F_AT_OverdueItems, false, strItemBarcode));
                        }
                    }

                    if (chargedItems.Count > 0)
                        response.AU_ChargedItems_o = chargedItems;
                    if (overdueItems.Count > 0)
                    {
                        patronStatus[6] = 'Y';
                        response.AT_OverdueItems_o = overdueItems;
                    }
                }
                response.PatronStatus_14 = new string(patronStatus);

                response.AA_PatronIdentifier_r = strBarcode;
                response.AE_PersonalName_r = DomUtil.GetElementText(dom.DocumentElement, "name");
                response.BL_ValidPatron_o = "Y";

                string strCount = DomUtil.GetElementAttr(dom.DocumentElement, "info/item[@name='可借总册数']", "value");
                string strBorrowsCount = DomUtil.GetElementAttr(dom.DocumentElement, "info/item[@name='当前还可借']", "value");
                response.BZ_HoldItemsLimit_o = strBorrowsCount.PadLeft(4, '0');

                string strMsg = "";
                if (!string.IsNullOrEmpty(strCount))
                {
                    if (!string.IsNullOrEmpty(strBorrowsCount) && strBorrowsCount != "0")
                        strMsg += "您在本馆最多可借【" + strCount + "】册，还可以再借【" + strBorrowsCount + "】册。";
                    else
                        strMsg += "您在本馆借书数已达最多可借数【" + strCount + "】，不能继续借了!";
                }
                if (!string.IsNullOrEmpty(strMsg))
                {
                    response.AF_ScreenMessage_o = strMsg;
                    response.AG_PrintLine_o = strMsg;
                }
            }
            return response.ToText();
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="channel">ILS 通道</param>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string Login(LibraryChannel channel, string message)
        {
            string strError = "";

            string strUserID = "";
            string strPassword = "";
            string strLocationCode = "";


            LoginResponse_94 response = new LoginResponse_94()
            {
                Ok_1 = "0",
            };

            Login_93 request = new Login_93();
            int nRet = request.parse(message, out strError);
            if (-1 == nRet)
            {
                return response.ToText();
            }
            else
            {
                strUserID = request.CN_LoginUserId_r;
                strPassword = request.CO_LoginPassword_r;
                strLocationCode = request.CP_LocationCode_o;

                long lRet = channel.Login(strUserID,
                    strPassword,
                    "type=worker,client=dp2SIPServer|0.01",
                    out strError);
                if (lRet == -1)
                {
                    LogManager.Logger.Error(strError);
                }
                else if (lRet == 0)
                {
                    LogManager.Logger.Error(strError);
                }
                else
                {
                    response.Ok_1 = "1";

                    this.LocationCode = strLocationCode;
                    this.LoginUserId = strUserID;
                    this.LoginPassword = strPassword;

                    LogManager.Logger.Info("终端 " + strLocationCode + " : " + strUserID + " 接入");
                }
            }

            return response.ToText();
        }

        /// <summary>
        /// 状态查询
        /// </summary>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string SCStatus(string message)
        {
            ACSStatus_98 response = new ACSStatus_98()
            {
                OnlineStatus_1 = "Y",
                CheckinOk_1 = "Y",
                CheckoutOk_1 = "Y",
                ACSRenewalPolicy_1 = "Y",
                StatusUpdateOk_1 = "Y",
                OfflineOk_1 = "Y",
                TimeoutPeriod_3 = "010",
                RetriesAllowed_3 = "003",
                DatetimeSync_18 = SIPUtility.NowDateTime,
                ProtocolVersion_4 = "2.00",
                AO_InstitutionId_r = "dp2Library",
                AM_LibraryName_o = "dp2Library",
                BX_SupportedMessages_r = "YYYYYYYYYYYYYYYY",
                AF_ScreenMessage_o = "连接成功!",
            };

            return response.ToText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">SIP消息</param>
        /// <returns></returns>
        public string EndPatronSession(string message)
        {
            string strError = "";

            EndSessionResponse_36 response = new EndSessionResponse_36()
            {
                EndSession_1 = "N",
                TransactionDate_18 = SIPUtility.NowDateTime,
                AO_InstitutionId_r = "dp2Library",
            };

            EndPatronSession_35 request = new EndPatronSession_35();
            int nRet = request.parse(message, out strError);
            if (-1 == nRet)
            {
                LogManager.Logger.Error(strError);
                response.AF_ScreenMessage_o = strError;
                response.AG_PrintLine_o = strError;
            }
            else
            {
                response.AA_PatronIdentifier_r = request.AA_PatronIdentifier_r;
                response.AF_ScreenMessage_o = "结束操作成功";
                response.AG_PrintLine_o = "结束操作成功";
            }

            return response.ToText();
        }
    }
}
