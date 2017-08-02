using DigitalPlatform;
using DigitalPlatform.SIP2;
using DigitalPlatform.SIP2.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace dp2SIPClient
{
    public partial class MainForm : Form
    {
        private TcpClient _client;
        private NetworkStream _networkStream;

        public MainForm()
        {
            InitializeComponent();
        }

        #region 连接 SIP2 Server



        public string SIPServerUrl
        {
            get
            {
                return Properties.Settings.Default.SIPServerUrl;
            }
        }


        public int SIPServerPort
        {
            get
            {
                return Properties.Settings.Default.SIPServerPort;
            }
        }

        private void Connection()
        {
            this.Enabled = false;
            try
            {
                IPAddress ipAddress = IPAddress.Parse(this.SIPServerUrl);
                string hostName = Dns.GetHostEntry(ipAddress).HostName;
                _client = new TcpClient(hostName, this.SIPServerPort);


                // 保存到一个变量上，方便使用
                _networkStream = _client.GetStream();

                //界面设置
                this.toolStripStatusLabel_info.Text = "连接服务器成功。";
                toolStripLabel_send.Enabled = true;
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel_info.Text = "连接服务器失败:" + ex.Message;
                return;
            }
            finally
            {
                this.Enabled = true;
            }


        }

        // 关闭连接
        public void CloseSocket()
        {
            if (_client != null)
            {
                try
                {
                    this._networkStream.Close();
                }
                catch { }

                try
                {
                    this._client.Close();
                }
                catch { }

                this._client = null;
            }
        }

        #endregion


        #region 发送接收

        //发送消息
        private void toolStripLabel_send_Click(object sender, EventArgs e)
        {
            BaseMessage request = null;
            try
            {
                if (this.tabControl_main.SelectedTab == this.tabPage_Login93)
                {
                    request = new Login_93();
                    request.SetFixedFieldValue(SIPConst.F_UIDAlgorithm, this.textBox_Login93_UIDAlgorithm_1.Text);
                    request.SetFixedFieldValue(SIPConst.F_PWDAlgorithm, this.textBox_Login93_PWDAlgorithm_1.Text);

                    request.SetVariableFieldValue(SIPConst.F_CN_LoginUserId, this.textBox_Login93_loginUserId_CN_r.Text == "null" ? null : this.textBox_Login93_loginUserId_CN_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_CO_LoginPassword, this.textBox_Login93_loginPassword_CO_r.Text == "null" ? null : this.textBox_Login93_loginPassword_CO_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_CP_LocationCode, this.textBox_Login93_locationCode_CP_o.Text == "null" ? null : this.textBox_Login93_locationCode_CP_o.Text);
                }
                else if (this.tabControl_main.SelectedTab == this.tabPage_SCStatus99)
                {
                    request = new SCStatus_99();
                    request.SetFixedFieldValue(SIPConst.F_StatusCode, this.textBox_SCStatus99_statusCode_1.Text);
                    request.SetFixedFieldValue(SIPConst.F_MaxPrintWidth, this.textBox_SCStatus99_maxPrintWidth_3.Text);
                    request.SetFixedFieldValue(SIPConst.F_ProtocolVersion, this.textBox_SCStatus99_protocolVersion_4.Text);
                }
                else if (this.tabControl_main.SelectedTab == this.tabPage_Checkout11)
                {
                    request = new Checkout_11();
                    request.SetFixedFieldValue(SIPConst.F_SCRenewalPolicy, this.textBox_Checkout11_SCRenewalPolicy_1.Text);
                    request.SetFixedFieldValue(SIPConst.F_NoBlock, this.textBox_Checkout11_noBlock_1.Text);
                    request.SetFixedFieldValue(SIPConst.F_TransactionDate, this.textBox_Checkout11_transactionDate_18.Text);
                    request.SetFixedFieldValue(SIPConst.F_NbDueDate, this.textBox_Checkout11_nbDueDate_18.Text);

                    request.SetVariableFieldValue(SIPConst.F_AO_InstitutionId, this.textBox_Checkout11_institutionId_AO_r.Text == "null" ? null : this.textBox_Checkout11_institutionId_AO_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_AA_PatronIdentifier, this.textBox_Checkout11_patronIdentifier_AA_r.Text == "null" ? null : this.textBox_Checkout11_patronIdentifier_AA_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_AB_ItemIdentifier, this.textBox_Checkout11_itemIdentifier_AB_r.Text == "null" ? null : this.textBox_Checkout11_itemIdentifier_AB_r.Text);

                    request.SetVariableFieldValue(SIPConst.F_AC_TerminalPassword,  this.textBox_Checkout11_terminalPassword_AC_r.Text == "null" ? null : this.textBox_Checkout11_terminalPassword_AC_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_CH_ItemProperties,this.textBox_Checkout11_itemProperties_CH_o.Text == "null" ? null : this.textBox_Checkout11_itemProperties_CH_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_AD_PatronPassword, this.textBox_Checkout11_patronPassword_AD_o.Text == "null" ? null : this.textBox_Checkout11_patronPassword_AD_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_BO_FeeAcknowledged, this.textBox_Checkout11_feeAcknowledged_BO_1_o.Text == "null" ? null : this.textBox_Checkout11_feeAcknowledged_BO_1_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_BI_Cancel,this.textBox_Checkout11_cancel_BI_1_o.Text == "null" ? null : this.textBox_Checkout11_cancel_BI_1_o.Text);
                }
                else if (this.tabControl_main.SelectedTab == this.tabPage_Checkin09)
                {
                    request = new Checkin_09();
                    
                    request.SetFixedFieldValue(SIPConst.F_NoBlock, this.textBox_Checkin09_noBlock_1.Text);
                    request.SetFixedFieldValue(SIPConst.F_TransactionDate, this.textBox_Checkin09_transactionDate_18.Text);
                    request.SetFixedFieldValue(SIPConst.F_ReturnDate, this.textBox_Checkin09_returnDate_18.Text);

                    request.SetVariableFieldValue(SIPConst.F_AP_CurrentLocation,this.textBox_Checkin09_currentLocation_AP_r.Text == "null" ? null : this.textBox_Checkin09_currentLocation_AP_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_AO_InstitutionId,this.textBox_Checkin09_institutionId_AO_r.Text == "null" ? null : this.textBox_Checkin09_institutionId_AO_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_AB_ItemIdentifier,this.textBox_Checkin09_itemIdentifier_AB_r.Text == "null" ? null : this.textBox_Checkin09_itemIdentifier_AB_r.Text);

                    request.SetVariableFieldValue(SIPConst.F_AC_TerminalPassword,this.textBox_Checkin09_terminalPassword_AC_r.Text == "null" ? null : this.textBox_Checkin09_terminalPassword_AC_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_CH_ItemProperties,this.textBox_Checkin09_itemProperties_CH_o.Text == "null" ? null : this.textBox_Checkin09_itemProperties_CH_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_BI_Cancel, this.textBox_Checkin09_cancel_BI_1_o.Text == "null" ? null : this.textBox_Checkin09_cancel_BI_1_o.Text);
                }
                else if (this.tabControl_main.SelectedTab == this.tabPage_PatronInformation63)
                {
                    request = new PatronInformation_63();
                    request.SetFixedFieldValue(SIPConst.F_Language,  this.textBox_PatronInformation63_language_3.Text);
                    request.SetFixedFieldValue(SIPConst.F_TransactionDate, this.textBox_PatronInformation63_transactionDate_18.Text);
                    request.SetFixedFieldValue(SIPConst.F_Summary,  this.textBox_PatronInformation63_summary_10.Text);

                    request.SetVariableFieldValue(SIPConst.F_AO_InstitutionId,this.textBox_PatronInformation63_institutionId_AO_r.Text == "null" ? null : this.textBox_PatronInformation63_institutionId_AO_r.Text);
                     request.SetVariableFieldValue(SIPConst.F_AA_PatronIdentifier,this.textBox_PatronInformation63_patronIdentifier_AA_r.Text == "null" ? null : this.textBox_PatronInformation63_patronIdentifier_AA_r.Text);
                     request.SetVariableFieldValue(SIPConst.F_AC_TerminalPassword,this.textBox_PatronInformation63_terminalPassword_AC_o.Text == "null" ? null : this.textBox_PatronInformation63_terminalPassword_AC_o.Text);

                     request.SetVariableFieldValue(SIPConst.F_AD_PatronPassword,this.textBox_PatronInformation63_patronPassword_AD_o.Text == "null" ? null : this.textBox_PatronInformation63_patronPassword_AD_o.Text);
                     request.SetVariableFieldValue(SIPConst.F_BP_StartItem,this.textBox_PatronInformation63_startItem_BP_o.Text == "null" ? null : this.textBox_PatronInformation63_startItem_BP_o.Text);
                     request.SetVariableFieldValue(SIPConst.F_BQ_EmailAddress,this.textBox_PatronInformation63_endItem_BQ_o.Text == "null" ? null : this.textBox_PatronInformation63_endItem_BQ_o.Text);

                }
                else if (this.tabControl_main.SelectedTab == this.tabPage_ItemInformation17)
                {
                    
                    request = new ItemInformation_17();

                    request.SetFixedFieldValue(SIPConst.F_TransactionDate, this.textBox_ItemInformation17_transactionDate_18.Text);

                    request.SetVariableFieldValue(SIPConst.F_AO_InstitutionId,this.textBox_ItemInformation17_institutionId_AO_r.Text == "null" ? null : this.textBox_ItemInformation17_institutionId_AO_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_AB_ItemIdentifier,this.textBox_ItemInformation17_itemIdentifier_AB_r.Text == "null" ? null : this.textBox_ItemInformation17_itemIdentifier_AB_r.Text);
                    request.SetVariableFieldValue(SIPConst.F_AC_TerminalPassword, this.textBox_ItemInformation17_terminalPassword_AC_o.Text == "null" ? null : this.textBox_ItemInformation17_terminalPassword_AC_o.Text);

                }
                else if (this.tabControl_main.SelectedTab == this.tabPage_Renew29)
                {
                    
                    request = new Renew_29();
                    
                    request.SetFixedFieldValue(SIPConst.F_ThirdPartyAllowed,this.textBox_Renew29_thirdPartyAllowed_1.Text);
                    request.SetFixedFieldValue(SIPConst.F_NoBlock,this.textBox_Renew29_noBlock_1.Text);
                    request.SetFixedFieldValue(SIPConst.F_TransactionDate,this.textBox_Renew29_transactionDate_18.Text);
                    request.SetFixedFieldValue(SIPConst.F_NbDueDate,this.textBox_Renew29_nbDueDate_18.Text);

                    request.SetVariableFieldValue(SIPConst.F_AO_InstitutionId,this.textBox_Renew29_institutionId_AO_r.Text == "null" ? null : this.textBox_Renew29_institutionId_AO_r.Text);
                     request.SetVariableFieldValue(SIPConst.F_AA_PatronIdentifier,this.textBox_Renew29_patronIdentifier_AA_r.Text == "null" ? null : this.textBox_Renew29_patronIdentifier_AA_r.Text);

                    request.SetVariableFieldValue(SIPConst.F_AD_PatronPassword,this.textBox_Renew29_patronPassword_AD_o.Text == "null" ? null : this.textBox_Renew29_patronPassword_AD_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_AB_ItemIdentifier,this.textBox_Renew29_itemIdentifier_AB_o.Text == "null" ? null : this.textBox_Renew29_itemIdentifier_AB_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_AJ_TitleIdentifier,this.textBox_Renew29_titleIdentifier_AJ_o.Text == "null" ? null : this.textBox_Renew29_titleIdentifier_AJ_o.Text);

                    request.SetVariableFieldValue(SIPConst.F_AC_TerminalPassword,this.textBox_Renew29_terminalPassword_AC_o.Text == "null" ? null : this.textBox_Renew29_terminalPassword_AC_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_CH_ItemProperties,this.textBox_Renew29_itemProperties_CH_o.Text == "null" ? null : this.textBox_Renew29_itemProperties_CH_o.Text);
                    request.SetVariableFieldValue(SIPConst.F_BO_FeeAcknowledged, this.textBox_Renew29_feeAcknowledged_BO_1_o.Text == "null" ? null : this.textBox_Renew29_feeAcknowledged_BO_1_o.Text);

                }
                //发送命令
                this.txtMsg.Text = request.ToText();
                this.sendCmd();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


        }

        private void toolStripLabel_sample_Click(object sender, EventArgs e)
        {
            this.txtMsg.Text = "";

            string error = "";
            bool bRet = false;
            //BaseRequest request = null;
            string text = "";
            if (this.tabControl_main.SelectedTab == this.tabPage_Login93)
            {
                text = "93  CNsupervisor|CO1|CPC00|AY0AZFB58";
                Login_93 request93 = new Login_93();
                bRet = request93.parse(text, out error);
                if (bRet == false)
                    goto ERROR1;

                this.textBox_Login93_UIDAlgorithm_1.Text = request93.GetFixedFieldValue(SIPConst.F_UIDAlgorithm);
                this.textBox_Login93_PWDAlgorithm_1.Text = request93.GetFixedFieldValue(SIPConst.F_PWDAlgorithm);
                this.textBox_Login93_loginUserId_CN_r.Text = request93.GetVariableFieldValue(SIPConst.F_CN_LoginUserId);
                this.textBox_Login93_loginPassword_CO_r.Text = request93.GetVariableFieldValue(SIPConst.F_CO_LoginPassword);
                this.textBox_Login93_locationCode_CP_o.Text = request93.GetVariableFieldValue(SIPConst.F_CP_LocationCode);
            }
            else if (this.tabControl_main.SelectedTab == this.tabPage_SCStatus99)
            {
                text = "9900302.00";
                SCStatus_99 request99 = new SCStatus_99();
                bRet = request99.parse(text, out error);
                if (bRet == false)
                    goto ERROR1;

                this.textBox_SCStatus99_statusCode_1.Text = request99.GetFixedFieldValue(SIPConst.F_StatusCode);
                this.textBox_SCStatus99_maxPrintWidth_3.Text = request99.GetFixedFieldValue(SIPConst.F_MaxPrintWidth);
                this.textBox_SCStatus99_protocolVersion_4.Text = request99.GetFixedFieldValue(SIPConst.F_ProtocolVersion);
            }
            else if (this.tabControl_main.SelectedTab == this.tabPage_Checkout11)
            {
                //20170630    141135
                text = "11YN" + SIPUtility.NowDateTime + "                  AOdp2Library|AAFZXP00001|ABDPB000051|AC|BON|BIN|";
                Checkout_11 request11 = new Checkout_11();
                bRet = request11.parse(text, out error);
                if (bRet == false)
                    goto ERROR1;

                
                this.textBox_Checkout11_SCRenewalPolicy_1.Text = request11.GetFixedFieldValue(SIPConst.F_SCRenewalPolicy);//.SCRenewalPolicy_1;
                this.textBox_Checkout11_noBlock_1.Text = request11.GetFixedFieldValue(SIPConst.F_NoBlock);//.NoBlock_1;
                this.textBox_Checkout11_transactionDate_18.Text = request11.GetFixedFieldValue(SIPConst.F_TransactionDate);//.TransactionDate_18;
                this.textBox_Checkout11_nbDueDate_18.Text = request11.GetFixedFieldValue(SIPConst.F_NbDueDate);//.NbDueDate_18;

                this.textBox_Checkout11_institutionId_AO_r.Text = request11.GetVariableFieldValue(SIPConst.F_AO_InstitutionId);//.InstitutionId_AO_r;
                this.textBox_Checkout11_patronIdentifier_AA_r.Text = request11.GetVariableFieldValue(SIPConst.F_AA_PatronIdentifier);//.PatronIdentifier_AA_r;

                this.textBox_Checkout11_itemIdentifier_AB_r.Text = request11.GetVariableFieldValue(SIPConst.F_AB_ItemIdentifier);//.ItemIdentifier_AB_r;
                this.textBox_Checkout11_terminalPassword_AC_r.Text = request11.GetVariableFieldValue(SIPConst.F_AC_TerminalPassword);//.TerminalPassword_AC_r;
                this.textBox_Checkout11_itemProperties_CH_o.Text = request11.GetVariableFieldValue(SIPConst.F_CH_ItemProperties);//.ItemProperties_CH_o;

                this.textBox_Checkout11_patronPassword_AD_o.Text = request11.GetVariableFieldValue(SIPConst.F_AD_PatronPassword);//.PatronPassword_AD_o;
                this.textBox_Checkout11_feeAcknowledged_BO_1_o.Text = request11.GetVariableFieldValue(SIPConst.F_BO_FeeAcknowledged);//.FeeAcknowledged_BO_1_o;
                this.textBox_Checkout11_cancel_BI_1_o.Text = request11.GetVariableFieldValue(SIPConst.F_BI_Cancel);//Cancel_BI_1_o;
            
                 
            }
            else if (this.tabControl_main.SelectedTab == this.tabPage_Checkin09)
            {
                //20170630    141630
                string transactionDate = SIPUtility.NowDateTime;
                string returnDate = SIPUtility.NowDateTime;
                text = "09N" + transactionDate + returnDate + "AP|AOdp2Library|ABDPB000051|AC|BIN|";
                Checkin_09 request09 = new Checkin_09();
                bRet = request09.parse(text, out error);
                if (bRet == false)
                    goto ERROR1;

                
                this.textBox_Checkin09_noBlock_1.Text = request09.GetFixedFieldValue(SIPConst.F_NoBlock);//.F_no.NoBlock_1;
                this.textBox_Checkin09_transactionDate_18.Text = request09.GetFixedFieldValue(SIPConst.F_TransactionDate);//_18;
                this.textBox_Checkin09_returnDate_18.Text = request09.GetFixedFieldValue(SIPConst.F_ReturnDate);

                this.textBox_Checkin09_currentLocation_AP_r.Text = request09.GetVariableFieldValue(SIPConst.F_AP_CurrentLocation);// _r;
                this.textBox_Checkin09_institutionId_AO_r.Text = request09.GetVariableFieldValue(SIPConst.F_AO_InstitutionId);
                this.textBox_Checkin09_itemIdentifier_AB_r.Text = request09.GetVariableFieldValue(SIPConst.F_AB_ItemIdentifier);

                this.textBox_Checkin09_terminalPassword_AC_r.Text = request09.GetVariableFieldValue(SIPConst.F_AC_TerminalPassword);
                this.textBox_Checkin09_itemProperties_CH_o.Text = request09.GetVariableFieldValue(SIPConst.F_CH_ItemProperties);
                this.textBox_Checkin09_cancel_BI_1_o.Text = request09.GetVariableFieldValue(SIPConst.F_BI_Cancel);
            
                 
            }
            else if (this.tabControl_main.SelectedTab == this.tabPage_PatronInformation63)
            {
                //6301920170630    090808  Y       AOdp2Library|AAA005312|
                string transactionDate = SIPUtility.NowDateTime;
                text = "63019" + transactionDate + "  Y       AOdp2Library|AAFZXP00001|";
                PatronInformation_63 request63 = new PatronInformation_63();
                bRet = request63.parse(text, out error);
                if (bRet == false)
                    goto ERROR1;
                
                this.textBox_PatronInformation63_language_3.Text = request63.GetFixedFieldValue(SIPConst.F_Language);
                this.textBox_PatronInformation63_transactionDate_18.Text = request63.GetFixedFieldValue(SIPConst.F_TransactionDate);//.TransactionDate_18;
                this.textBox_PatronInformation63_summary_10.Text = request63.GetFixedFieldValue(SIPConst.F_Summary);//.Summary_10;

                this.textBox_PatronInformation63_institutionId_AO_r.Text = request63.GetVariableFieldValue(SIPConst.F_AO_InstitutionId);//.InstitutionId_AO_r;
                this.textBox_PatronInformation63_patronIdentifier_AA_r.Text = request63.GetVariableFieldValue(SIPConst.F_AA_PatronIdentifier);//.PatronIdentifier_AA_r;
                this.textBox_PatronInformation63_terminalPassword_AC_o.Text = request63.GetVariableFieldValue(SIPConst.F_AC_TerminalPassword);//.TerminalPassword_AC_o;

                this.textBox_PatronInformation63_patronPassword_AD_o.Text = request63.GetVariableFieldValue(SIPConst.F_AD_PatronPassword);//.PatronPassword_AD_o;
                this.textBox_PatronInformation63_startItem_BP_o.Text = request63.GetVariableFieldValue(SIPConst.F_BP_StartItem);//.StartItem_BP_o;
                this.textBox_PatronInformation63_endItem_BQ_o.Text = request63.GetVariableFieldValue(SIPConst.F_BQ_EmailAddress);//.EndItem_BQ_o;                 
            }
            else if (this.tabControl_main.SelectedTab == this.tabPage_ItemInformation17)
            {
                //1720170623    151645AOdp2Library|AB700635|
                string transactionDate = SIPUtility.NowDateTime;
                text = "17" + transactionDate + "AOdp2Library|ABDPB000051|";
                ItemInformation_17 request17 = new ItemInformation_17();
                bRet = request17.parse(text, out error);
                if (bRet == false)
                    goto ERROR1;

                
                this.textBox_ItemInformation17_transactionDate_18.Text = request17.GetFixedFieldValue(SIPConst.F_TransactionDate);//.TransactionDate_18;

                this.textBox_ItemInformation17_institutionId_AO_r.Text = request17.GetVariableFieldValue(SIPConst.F_AO_InstitutionId);//.InstitutionId_AO_r;
                this.textBox_ItemInformation17_itemIdentifier_AB_r.Text = request17.GetVariableFieldValue(SIPConst.F_AB_ItemIdentifier);//.ItemIdentifier_AB_r;
                this.textBox_ItemInformation17_terminalPassword_AC_o.Text = request17.GetVariableFieldValue(SIPConst.F_AC_TerminalPassword);//.TerminalPassword_AC_o;
            
                 
            }
            else if (this.tabControl_main.SelectedTab == this.tabPage_Renew29)
            {
                //29NN20170630    144419                  AOdp2Library|AAL905071|AB510105|BON|
                string transactionDate = SIPUtility.NowDateTime;
                text = "29NN" + transactionDate + "                  AOdp2Library|AAFZXP00001|ABDPB000051|BON|";
                Renew_29 request29 = new Renew_29();
                bRet = request29.parse(text, out error);
                if (bRet == false)
                    goto ERROR1;

                
                this.textBox_Renew29_thirdPartyAllowed_1.Text = request29.GetFixedFieldValue(SIPConst.F_ThirdPartyAllowed);//.ThirdPartyAllowed_1;
                this.textBox_Renew29_noBlock_1.Text = request29.GetFixedFieldValue(SIPConst.F_NoBlock);//.NoBlock_1;
                this.textBox_Renew29_transactionDate_18.Text = request29.GetFixedFieldValue(SIPConst.F_TransactionDate);//.TransactionDate_18;
                this.textBox_Renew29_nbDueDate_18.Text = request29.GetFixedFieldValue(SIPConst.F_NbDueDate);//.NbDueDate_18;

                this.textBox_Renew29_institutionId_AO_r.Text = request29.GetVariableFieldValue(SIPConst.F_AO_InstitutionId);//.InstitutionId_AO_r;
                this.textBox_Renew29_patronIdentifier_AA_r.Text = request29.GetVariableFieldValue(SIPConst.F_AA_PatronIdentifier);//.PatronIdentifier_AA_r;

                //AD	AB AJ
                this.textBox_Renew29_patronPassword_AD_o.Text = request29.GetVariableFieldValue(SIPConst.F_AD_PatronPassword);//.PatronPassword_AD_o;
                this.textBox_Renew29_itemIdentifier_AB_o.Text = request29.GetVariableFieldValue(SIPConst.F_AB_ItemIdentifier);//.ItemIdentifier_AB_o;
                this.textBox_Renew29_titleIdentifier_AJ_o.Text = request29.GetVariableFieldValue(SIPConst.F_AJ_TitleIdentifier);//.TitleIdentifier_AJ_o;

                //AC	CH	BO
                this.textBox_Renew29_terminalPassword_AC_o.Text = request29.GetVariableFieldValue(SIPConst.F_AC_TerminalPassword);//.TerminalPassword_AC_o;
                this.textBox_Renew29_itemProperties_CH_o.Text = request29.GetVariableFieldValue(SIPConst.F_CH_ItemProperties);//.ItemProperties_CH_o;
                this.textBox_Renew29_feeAcknowledged_BO_1_o.Text = request29.GetVariableFieldValue(SIPConst.F_BO_FeeAcknowledged);//.FeeAcknowledged_BO_1_o;
            
                 
            }
            return;

        ERROR1:
            this.Print("error:" + error);


        }

        // 回车 触发 发送
        private void txtMsg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.sendCmd();
                return;
            }
        }

        // 发送包装小函数
        private void sendCmd()
        {
            // 发送信息不允许为空
            if (txtMsg.Text == "")
            {
                MessageBox.Show("发送的信息不能为空!");
                txtMsg.Focus();
                return;
            }

            string text = this.txtMsg.Text.Trim();
            SendData(text);
        }


        // 发送数据
        private void SendData(string text)
        {
            this.Enabled = false;
            try
            {
                if (this._networkStream.DataAvailable == true)
                {
                    MessageBox.Show("异常：发送前发现流中有未读的数据!");
                    //this.RecvData();
                    return;
                }

                // 刷新界面
                this.Print("send:" + text);
                //txtMsg.Text = "";  // 清空
                this.txtMsg.SelectAll();


                // 命令参数检查
                BaseMessage request = null;
                string error="";
                bool bRet = SIPUtility.ParseRequest(text, out request, out error);
                if (bRet == false)
                {
                    this.Print("error-s:" + error);
                    return;
                }

                byte[] baPackage = this.Encoding.GetBytes(text);
                this._networkStream.Write(baPackage, 0, baPackage.Length);
                this._networkStream.Flush();//刷新当前数据流中的数据



                // 调接收数据
                string strRecv = "";
                string strError = "";
                int nRet = RecvTcpPackage(out strRecv, out strError);
                if (nRet == -1)
                {
                    this.Print("error-r:" + strError);
                    return;
                }
                this.Print("recv:" + strRecv);
                //this.PrinteInfo("recv:" + strPackage);
                //this.WriteToLog("Recv:" + strPackage);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Client提示");
            }
            finally
            {
                this.Enabled = true;
            }

        }


        // 接收数据
        public int RecvTcpPackage(out string strPackage,
            out string strError)
        {
            strError = "";
            strPackage = "";

            Debug.Assert(this._client != null, "client为空");

            int offset = 0; //偏移量
            int wRet = 0;


            byte[] baPackage = new byte[1024];
            int nOneLength = 1024; //COMM_BUFF_LEN;

            while (offset < nOneLength)
            {
                if (this._client == null)
                {
                    strError = "通讯中断";
                    goto ERROR1;
                }

                try
                {
                    wRet = this._networkStream.Read(baPackage,
                        offset,
                        baPackage.Length - offset);
                }
                catch (SocketException ex)
                {
                    // ??这个什么错误码
                    if (ex.ErrorCode == 10035)
                    {
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }

                    // bool bRet = this.client.Connected;

                    strError = "[ERROR] Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }
                catch (Exception ex)
                {
                    //bool bRet = this.client.Connected;

                    strError = "[ERROR] Recv出错: " + ExceptionUtil.GetDebugText(ex);
                    goto ERROR1;
                }

                if (wRet == 0)
                {
                    return 0;
                    //strError = "Closed by remote peer";
                    //goto ERROR1;
                }

                // 得到包的长度
                if (wRet >= 1 || offset >= 1)
                {
                    //没有找到结束符，继续读
                    int nRet = Array.IndexOf(baPackage, (byte)this.Terminator);
                    if (nRet != -1)
                    {
                        // nLen = nInLen + wRet;
                        nOneLength = nRet;
                        break;
                    }

                    if (this._networkStream.DataAvailable == false) //流中没有数据了
                    {
                        nOneLength = offset + wRet;
                        break;
                    }
                }

                offset += wRet;
                if (offset >= baPackage.Length)
                {
                    // 扩大缓冲区
                    byte[] temp = new byte[baPackage.Length + 1024];
                    Array.Copy(baPackage, 0, temp, 0, offset);
                    baPackage = temp;
                    nOneLength = baPackage.Length;
                }
            }

            // 最后规整缓冲区尺寸，如果必要的话
            if (baPackage.Length > nOneLength)
            {
                byte[] temp = new byte[nOneLength];
                Array.Copy(baPackage, 0, temp, 0, nOneLength);
                baPackage = temp;
            }

            strPackage = this.Encoding.GetString(baPackage);
            return 0;

        ERROR1:
            this.CloseSocket();
            baPackage = null;
            return -1;
        }

        #endregion


        #region 一些参数信息

        public Encoding Encoding
        {
            get
            {
                //string strEndodingName = Properties.Settings.Default.EncodingName;
                //if (string.IsNullOrEmpty(strEndodingName))
                //    strEndodingName = "UTF-8";

                string strEndodingName = "UTF-8";

                return Encoding.GetEncoding(strEndodingName);
            }
        }

        // 命令结束符
        char Terminator
        {
            get
            {
                //string strTerminator = Properties.Settings.Default.Terminator;
                //if (strTerminator == "LF") //NewLine
                //    return (char)10;
                //else // if(strTerminator == "CR") //Return
                return (char)13;
            }
        }
        #endregion


        #region 界面控件



        private void Print(string text)
        {
            if (this.txtInfo.Text != "")
                this.txtInfo.Text += "\r\n";
            this.txtInfo.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +" "+ text;

            //this.txtInfo.Text = text;
        }

        #endregion




        private void 参数配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_setting dlg = new Form_setting();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.Connection();
            }
        }

        private void 实用工具ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form_checksum dlg = new Form_checksum();
            dlg.ShowDialog(this);
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.SIPServerUrl))
            {
                Form_setting dlg = new Form_setting();
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    this.Close();
                    return;
                }
            }

            this.Connection();


           // this.toolStripStatusLabel_port.Text = "监听端口：" + this.Port;
           // this.toolStripLabel_send.Enabled = true;
        }

        private void 清空信息区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.txtInfo.Text = "";
        }

        private void 初始化测试环境ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建读者库

            //生成几条读者数据

            //创建书目库

            // 导入marc

            //创建册记录

            //配上相关馆藏地

            //配上流通权限

            Form_test dlg = new Form_test();
            dlg.ShowDialog(this);
        }









    }
}
