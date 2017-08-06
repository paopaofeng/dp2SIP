using DigitalPlatform;
using DigitalPlatform.CirculationClient;
using DigitalPlatform.LibraryClient;
using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.Marc;
using DigitalPlatform.SIP2;
using DigitalPlatform.SIP2.Request;
using DigitalPlatform.SIP2.Response;
using DigitalPlatform.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace dp2SIPClient
{
    public partial class Form_test : Form
    {

        public Form_test()
        {
            InitializeComponent();
        }

        private LibraryChannelPool _channelPool = new LibraryChannelPool();

        List<LibraryChannel> _channelList = new List<LibraryChannel>();

        // parameters:
        //      style    风格。如果为 GUI，表示会自动添加 Idle 事件，并在其中执行 Application.DoEvents
        public LibraryChannel GetChannel()
        {
            LibraryChannel channel = this._channelPool.GetChannel(this.dp2ServerUrl,
                this.dp2Username);
            channel.Idle += channel_Idle;
            _channelList.Add(channel);

            // TODO: 检查数组是否溢出
            return channel;
        }

        void channel_Idle(object sender, IdleEventArgs e)
        {
            Application.DoEvents();
        }

        public void ReturnChannel(LibraryChannel channel)
        {
            channel.Idle -= channel_Idle;

            this._channelPool.ReturnChannel(channel);
            _channelList.Remove(channel);
        }

        public string dp2ServerUrl
        {
            get
            {
                return Properties.Settings.Default.dp2ServerUrl;
            }
        }

        public string dp2Username
        {
            get
            {
                return Properties.Settings.Default.dp2Username;
            }
        }

        public string dp2Password
        {
            get
            {
                return Properties.Settings.Default.dp2Password;
            }
        }

        private void Form_CreateTestEnv_Load(object sender, EventArgs e)
        {
           if (string.IsNullOrEmpty(this.dp2ServerUrl)==true
               || string.IsNullOrEmpty(this.dp2Username) == true)
            {
                MessageBox.Show("尚未配置dp2系统登录信息");
                this.Close();
            }

            this._channelPool.BeforeLogin += (sender1, e1) =>
            {
                e1.LibraryServerUrl = this.dp2ServerUrl;
                e1.UserName = this.dp2Username;
                e1.Parameters = "type=worker,client=dp2SIPClient|0.01";
                e1.Password = this.dp2Password;
                e1.SavePasswordLong = true;                
            };

            // Stop初始化
            stopManager.Initial(null,
                (object)this.toolStripStatusLabel1,
                (object)this.toolStripProgressBar1);

            stop = new DigitalPlatform.Stop();
            stop.Register(this.stopManager, true);	// 和容器关联
        }

        /// <summary>
        /// 允许或者禁止界面控件。在长操作前，一般需要禁止界面控件；操作完成后再允许
        /// </summary>
        /// <param name="bEnable">是否允许界面控件。true 为允许， false 为禁止</param>
        public void EnableControls(bool bEnable)
        {
            this.button_createTestEnv.Enabled = bEnable;
        }
        /// <summary>
        /// Stop 管理器
        /// </summary>
        public DigitalPlatform.StopManager stopManager = new DigitalPlatform.StopManager();
        internal DigitalPlatform.Stop stop = null;

        /// <summary>
        /// 进度条和停止按钮
        /// </summary>
        public Stop Progress
        {
            get
            {
                return this.stop;
            }
        }

        public void DoStop(object sender, StopEventArgs e)
        {
            foreach (LibraryChannel channel in _channelList)
            {
                if (channel != null)
                    channel.Abort();
            }
        }

        // 获得流通读者权限相关定义
        int GetRightsTableInfo(LibraryChannel channel, 
            out string strRightsTableXml,
            out string strError)
        {
            strError = "";
            strRightsTableXml = "";


            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("正在获取读者流通权限定义 ...");
            stop.BeginLoop();

            try
            {
                long lRet = channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTable",
                    out strRightsTableXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

            }

        ERROR1:
            return -1;
        }

        // 保存流通读者权限相关定义
        // parameters:
        //      strRightsTableXml   流通读者权限定义XML。注意，没有根元素
        int SetRightsTableDef( LibraryChannel channel,
            string strRightsTableXml,
            out string strError)
        {
            strError = "";


            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("正在保存读者流通权限定义 ...");
            stop.BeginLoop();

            try
            {
                long lRet = channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTable",
                    strRightsTableXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

            }

        ERROR1:
            return -1;
        }

        /*
        void UpdateDom(string strLibraryCode)
        {
            // string strFilter = "";
            XmlNode root = null;
            if (string.IsNullOrEmpty(strLibraryCode) == false)
            {
                // descendant
                XmlNode temp = this._dom.SelectSingleNode("//descendant-or-self::library[@code='" + strLibraryCode + "']");
                if (temp == null)
                {
                    temp = this._dom.CreateElement("library");
                    this._dom.DocumentElement.AppendChild(temp);
                    DomUtil.SetAttr(temp, "code", strLibraryCode);
                }
                root = temp;
            }
            else
            {
                root = this._dom.DocumentElement;
                // strFilter = "[count(ancestor::library) = 0]";
            }

            // 删除原来的下级元素
            XmlNodeList nodes = root.SelectNodes("child::*[not(self::library)]");
            foreach (XmlNode node in nodes)
            {
                node.ParentNode.RemoveChild(node);
            }

            XmlNode insert_pos = root.SelectSingleNode("library");

            List<string> reader_types = new List<string>();

            foreach (Row row in this._rows)
            {
                reader_types.Add(row.ReaderType);

                XmlNode reader = this._dom.CreateElement("type");
                if (insert_pos != null)
                    root.InsertBefore(reader, insert_pos);
                else
                    root.AppendChild(reader);
                DomUtil.SetAttr(reader, "reader", row.ReaderType);

                foreach (string strName in LoanParam.reader_d_paramnames)
                {
                    string strValue = row.PatronPolicyCell.GetValue(strName);
                    XmlNode param = this._dom.CreateElement("param");
                    reader.AppendChild(param);
                    DomUtil.SetAttr(param, "name", strName);
                    DomUtil.SetAttr(param, "value", strValue);
                }

                for (int j = 0; j < this._bookTypes.Count; j++)
                {
                    string strBookType = this._bookTypes[j];



                    XmlNode book = this._dom.CreateElement("type");
                    reader.AppendChild(book);
                    DomUtil.SetAttr(book, "book", strBookType);

                    foreach (string strName in LoanParam.two_d_paramnames)
                    {
                        string strValue = row.Cells[j].GetValue(strName);
                        XmlNode param = this._dom.CreateElement("param");
                        book.AppendChild(param);
                        DomUtil.SetAttr(param, "name", strName);
                        DomUtil.SetAttr(param, "value", strValue);
                    }
                }

            }


            // TODO: 最好插入在兄弟 <library> 元素以前
            XmlNode readertypes_node = this._dom.CreateElement("readerTypes");
            if (insert_pos != null)
                root.InsertBefore(readertypes_node, insert_pos);
            else
                root.AppendChild(readertypes_node);
            foreach (string s in reader_types)
            {
                XmlNode node = this._dom.CreateElement("item");
                readertypes_node.AppendChild(node);
                node.InnerText = s;
            }

            XmlNode booktypes_node = this._dom.CreateElement("bookTypes");
            if (insert_pos != null)
                root.InsertBefore(booktypes_node, insert_pos);
            else
                root.AppendChild(booktypes_node);
            foreach (string s in this._bookTypes)
            {
                XmlNode node = this._dom.CreateElement("item");
                booktypes_node.AppendChild(node);
                node.InnerText = s;
            }

            this._domChanged = true;
        }
        */

        public string C_ReaderDbName = "_测试读者";
        private void button_createTestEnv_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            LibraryChannel channel = this.GetChannel();
            TimeSpan old_timeout = channel.Timeout;
            channel.Timeout = TimeSpan.FromMinutes(10);

            Progress.Style = StopStyle.EnableHalfStop;
            Progress.OnStop += new StopEventHandler(this.DoStop);
            Progress.Initial("正在进行测试 ...");
            Progress.BeginLoop();


            EnableControls(false);
            try
            {
                // 创建测试所需的书目库

                string strBiblioDbName = "_测试用中文图书";

                // 如果测试用的书目库以前就存在，要先删除。删除前最好警告一下
                Progress.SetMessage("正在删除测试用书目库 ...");
                string strOutputInfo = "";
                long lRet = channel.ManageDatabase(
                    stop,
                    "delete",
                    strBiblioDbName,    // strDatabaseNames,
                    "",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                {
                    if (channel.ErrorCode != DigitalPlatform.LibraryClient.localhost.ErrorCode.NotFound)
                        goto ERROR1;
                }

                Progress.SetMessage("正在创建测试用书目库 ...");
                // 创建一个书目库
                // parameters:
                // return:
                //      -1  出错
                //      0   没有必要创建，或者操作者放弃创建。原因在 strError 中
                //      1   成功创建
                nRet = ManageHelper.CreateBiblioDatabase(
                    channel,
                    this.Progress,
                    strBiblioDbName,
                    "book",
                    "unimarc",
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                // 创建书目库的定义
                Progress.SetMessage("正在删除测试用读者库 ...");
                 lRet = channel.ManageDatabase(
                    stop,
                    "delete",
                    C_ReaderDbName,    // strDatabaseNames,
                    "",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                {
                    if (channel.ErrorCode != DigitalPlatform.LibraryClient.localhost.ErrorCode.NotFound)
                        goto ERROR1;
                }

                Progress.SetMessage("正在创建测试用读者库 ...");

                XmlDocument database_dom = new XmlDocument();
                database_dom.LoadXml("<root />");
                // 创建读者库
                CreateReaderDatabaseNode(database_dom,
                    C_ReaderDbName,
                    "",
                    true);
                lRet = channel.ManageDatabase(
                    this.stop,
                    "create",
                    "",
                    database_dom.OuterXml,
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                Progress.SetMessage("正在创建测试读者记录 ...");
                lRet = this.CreateReaderRecord(channel, out strError);
                if (lRet == -1)
                    goto ERROR1;

                Progress.SetMessage("正在定义测试所需的馆藏地 ...");
                // *** 定义测试所需的馆藏地
                List<DigitalPlatform.CirculationClient.ManageHelper.LocationItem> items = new List<DigitalPlatform.CirculationClient.ManageHelper.LocationItem>();
                items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem("", "_测试阅览室", true, true));
                items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem("", "_测试流通库", true, true));

                // 为系统添加新的馆藏地定义
                nRet = ManageHelper.AddLocationTypes(
                    channel,
                    this.Progress,
                    "add",
                    items,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                Progress.SetMessage("正在配置工作日历 ...");

                CalenderInfo info = new CalenderInfo();
                info.Name = C_CalenderName;
                info.Range = "20170101-20191231";
                info.Comment = "";
                info.Content = "";

                //先删除
                lRet = channel.SetCalendar(
                   stop,
                   "delete",
                   info,
                   out strError);
                if (lRet == -1)
                    goto ERROR1;

                lRet = channel.SetCalendar(
                   stop,
                   "new",
                   info,
                   out strError);
                if (lRet == -1)
                    goto ERROR1;

                Progress.SetMessage("正在配置测试所需的流通权限 ...");

                // 先删除原来测试自动增加权限
                nRet = this.RemoveTestRightsTable(channel, null, out strError);
                if (nRet == -1)
                    goto ERROR1;

                //增加测试用权限
                nRet = this.AddTestRightsTable(channel, null, out strError);
                if (nRet == -1)
                    goto ERROR1;


                Progress.SetMessage("正在创建书目记录和册记录 ...");
                // 创建书目
                nRet = this.CreateBiblioRecord(channel, strBiblioDbName, out strError);
                if (nRet == -1)
                    goto ERROR1;

                
                /*
                // 删除测试用的书目库、馆藏地定义
                Progress.SetMessage("正在删除测试用书目库 ...");
                lRet = channel.ManageDatabase(
                    stop,
                    "delete",
                    strBiblioDbName,    // strDatabaseNames,
                    "",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                Progress.SetMessage("正在删除测试用的馆藏地 ...");
                nRet = ManageHelper.AddLocationTypes(
                    channel,
                    this.Progress,
                    "remove",
                    items,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;
                */

                return;
            }
            //catch (Exception ex)
            //{
            //    strError = "MoveBiblioRecord() Exception: " + ExceptionUtil.GetExceptionText(ex);
            //    goto ERROR1;
            //}
            finally
            {
                Progress.EndLoop();
                Progress.OnStop -= new StopEventHandler(this.DoStop);
                Progress.Initial("");
                Progress.HideProgress();


                EnableControls(true);

                channel.Timeout = old_timeout;
                this.ReturnChannel(channel);
            }
        ERROR1:
            this.Invoke((Action)(() => MessageBox.Show(this, strError)));
            
        }

        public const string C_CalenderName = "_测试日历";

        int CreateReaderRecord(LibraryChannel channel,
            out string strError)
        {
            strError="";

            // 创建10条测试用读者记录
            string strTargetRecPath=C_ReaderDbName+"/?";
            for (int i = 0; i < 10; i++)
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml("<root />");
                XmlNode root = dom.DocumentElement;

                string barcode = "_P" + i.ToString().PadLeft(3, '0');
                DomUtil.SetElementText(root, "barcode", barcode);

                string name = "SIP2测试" + i.ToString().PadLeft(3, '0'); ;
                DomUtil.SetElementText(root, "name", name);

                DomUtil.SetElementText(root, "readerType", C_PatronType);

                //DomUtil.SetElementText(root,"createDate", this.CreateDate);
                string strExistingXml="";
                string strSavedXml="";
                string strSavedPath="";
                byte[] baNewTimestamp=null;
                ErrorCodeValue kernel_errorcode=ErrorCodeValue.NoError;
                 long lRet = channel.SetReaderInfo(
                    stop,
                    "new",  // this.m_strSetAction,
                    strTargetRecPath,
                    dom.OuterXml,
                    null,
                    null,

                    out strExistingXml,
                    out strSavedXml,
                    out strSavedPath,
                    out baNewTimestamp,
                    out kernel_errorcode,
                    out strError);
                 if (lRet == -1)
                 {
                     return -1;
                 }
            }

            return 0;

        }

        /*
        internal override void RefreshDom()
        {
            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "barcode", this.Barcode);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "cardNumber", this.CardNumber);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "state", this.State);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "comment", this.Comment);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "readerType", this.ReaderType);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "createDate", this.CreateDate);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "expireDate", this.ExpireDate);

            // 2007/6/15
            XmlNode nodeHire = null;
            nodeHire = this._dataDom.DocumentElement.SelectSingleNode("hire");
            if (nodeHire == null)
            {
                nodeHire = this._dataDom.CreateElement("hire");
                this._dataDom.DocumentElement.AppendChild(nodeHire);
            }
            DomUtil.SetAttr(nodeHire, "expireDate", this.HireExpireDate);
            DomUtil.SetAttr(nodeHire, "period", this.HirePeriod);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "foregift", this.Foregift);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "name", this.NameString);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
    "namePinyin", this.NamePinyin);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "gender", this.Gender);

            // 2012/4/11
            // 根据记录中是否已经有<dateOfBirth>元素来决定是否使用这个元素，以免对旧的dp2Library版本写记录过程中丢失<dateOfBirth>元素
            XmlNode nodeExistBirthdate = this._dataDom.DocumentElement.SelectSingleNode("dateOfBirth");    // BUG 2012/5/3 原先少了.DocumentElement
            if (nodeExistBirthdate == null)
                DomUtil.SetElementText(this._dataDom.DocumentElement,
                    "birthday", this.DateOfBirth);
            else
                DomUtil.SetElementText(this._dataDom.DocumentElement,
                    "dateOfBirth", this.DateOfBirth);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "idCardNumber", this.IdCardNumber);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "department", this.Department);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "post", this.Post);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "address", this.Address);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "tel", this.Tel);

            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "email", this.Email);
            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "rights", this.Rights);
            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "personalLibrary", this.PersonalLibrary);
            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "access", this.Access);
            DomUtil.SetElementText(this._dataDom.DocumentElement,
                "friends", this.Friends);
            DomUtil.SetElementText(this._dataDom.DocumentElement,
    "refID", this.RefID);

            base.RefreshDom();
        }
         */

        // 创建读者库的定义结点
        static XmlNode CreateReaderDatabaseNode(XmlDocument dom,
            string strDatabaseName,
            string strLibraryCode,
            bool bInCirculation)
        {
            XmlNode nodeDatabase = dom.CreateElement("database");
            dom.DocumentElement.AppendChild(nodeDatabase);

            // type
            DomUtil.SetAttr(nodeDatabase, "type", "reader");

            // inCirculation
            string strInCirculation = "true";
            if (bInCirculation == true)
                strInCirculation = "true";
            else
                strInCirculation = "false";

            DomUtil.SetAttr(nodeDatabase, "inCirculation", strInCirculation);

            DomUtil.SetAttr(nodeDatabase, "name", strDatabaseName);

            DomUtil.SetAttr(nodeDatabase, "libraryCode",
                strLibraryCode);

            return nodeDatabase;
        }

        // 所创建的书目记录和下属册记录信息。用于最后核对验证
        class BiblioCreationInfo
        {
            public string BiblioRecPath { get; set; }
            public List<string> ItemRefIDs { get; set; }
        }

        int CreateBiblioRecord(LibraryChannel channel,
            string strBiblioDbName, 
            out string strError)
        {
            strError = "";

            int barcordStart = 1;

            for (int i = 0; i < 10; i++)
            {
                string strTitle = "测试题名-" + i;

                MarcRecord record = new MarcRecord();
                record.add(new MarcField('$', "200  $a" + strTitle));
                record.add(new MarcField('$', "690  $aI247.5"));
                record.add(new MarcField('$', "701  $a测试著者"));
                string strMARC = record.Text;

                string strMarcSyntax = "unimarc";
                string strXml = "";
                int nRet = MarcUtil.Marc2Xml(strMARC,
                    strMarcSyntax,
                    out strXml,
                    out strError);
                if (nRet == -1)
                    return -1;

                string strPath = strBiblioDbName + "/?";
                byte[] baTimestamp = null;
                byte[] baNewTimestamp = null;
                string strOutputPath = "";

                long lRet = channel.SetBiblioInfo(
                    stop,
                    "new",
                    strPath,
                    "xml",
                    strXml,
                    baTimestamp,
                    "",
                    out strOutputPath,
                    out baNewTimestamp,
                    out strError);
                if (lRet == -1)
                {
                    strError = "保存书目记录 '" + strPath + "' 时出错: " + strError;
                    return -1;
                }

                //// 创建册记录
                //List<string> refids = CreateEntityRecords(entity_form, 10);
                EntityInfo[] entities = null;
                entities = new EntityInfo[5];
                for (int j = 0; j < entities.Length; j++)
                {
                    EntityInfo info = new EntityInfo();
                    info.RefID = Guid.NewGuid().ToString();
                    info.Action = "new";
                    info.Style = "";

                    info.OldRecPath = "";
                    info.OldRecord = "";
                    info.OldTimestamp = null;

                    info.NewRecPath = "";
                    info.NewRecord = "";
                    info.NewTimestamp = null;

                    entities[j] = info;

                    /*
<dprms:item path="中文图书实体/87" timestamp="2a3a427665d5d4080000000000000010" xmlns:dprms="http://dp2003.com/dprms">
  <parent>43</parent> 
  <refID>8e05d74b-650e-42f8-99cc-45442150c115</refID> 
  <barcode>DPB000051</barcode> 
  <location>方洲小学/图书馆</location> 
  <seller>新华书店</seller> 
  <source>本馆经费</source> 
  <price>CNY10.00</price> 
  <batchNo>201707</batchNo> 
  <accessNo>I17(198.4)/Y498</accessNo> 
  <bookType>普通</bookType>
</dprms:item>
                     */
                    XmlDocument itemDom = new XmlDocument();
                    itemDom.LoadXml("<root />");
                    XmlNode root=itemDom.DocumentElement;

                    string strTargetBiblioRecID = GetRecordID(strOutputPath);
                    DomUtil.SetElementText(root,"parent", strTargetBiblioRecID);

                    string barcode = "_B" + barcordStart.ToString().PadLeft(6, '0');// i.ToString().PadLeft(2, '0')+j.ToString().PadLeft(3,'0');
                    DomUtil.SetElementText(root, "barcode", barcode);
                    DomUtil.SetElementText(root, "location", C_Location);
                    DomUtil.SetElementText(root, "batchNo", "test001");
                    DomUtil.SetElementText(root, "bookType", C_BookType);


                    info.NewRecord = itemDom.DocumentElement.OuterXml;

                    barcordStart++;
                }

                EntityInfo[] errorinfos = null;

                lRet = channel.SetEntities(
                     this.stop,   // this.BiblioStatisForm.stop,
                     strOutputPath,
                     entities,
                     out errorinfos,
                     out strError);

                if (lRet == -1)
                    return -1;


            }


            return 0;

        }

        public static string GetRecordID(string strPath)
        {
            int nRet = strPath.LastIndexOf("/");
            if (nRet == -1)
                return "";

            return strPath.Substring(nRet + 1).Trim();
        }

        public string C_Location = "_测试流通库";
        public string C_PatronType = "测试-读者类型";
        public string C_BookType = "测试-图书类型";

        // 删除测试加的流通权限
        public int RemoveTestRightsTable(LibraryChannel channel,
            XmlDocument dom, 
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // 获取流通权限
            if (dom == null)
            {
                string strRightsTableXml = "";
                nRet = GetRightsTableInfo(channel, out strRightsTableXml, out strError);
                if (nRet == -1)
                    goto ERROR1;
                strRightsTableXml = "<rightsTable>" + strRightsTableXml + "</rightsTable>";
                dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strRightsTableXml);
                }
                catch (Exception ex)
                {
                    strError = "strRightsTableXml装入XMLDOM时发生错误：" + ex.Message;
                    goto ERROR1;
                }
            }

            XmlNode node = dom.DocumentElement.SelectSingleNode("type[@reader='" + C_PatronType + "']");
            if (node != null)
            {
                dom.DocumentElement.RemoveChild(node);
            }

            XmlNodeList list = dom.DocumentElement.SelectNodes("//type[@book='" + C_BookType + "']");
            foreach (XmlNode n in list)
            {
                n.ParentNode.RemoveChild(n);
            }

            node = dom.DocumentElement.SelectSingleNode("readerTypes/item[text()='" + C_PatronType + "']");
            if (node != null)
            {
                node.ParentNode.RemoveChild(node);
            }
            node = dom.DocumentElement.SelectSingleNode("bookTypes/item[text()='" + C_BookType + "']");
            if (node != null)
            {
                node.ParentNode.RemoveChild(node);
            }

            // 保存到系统
            nRet = this.SetRightsTableDef(channel, dom.DocumentElement.InnerXml, out strError);
            if (nRet == -1)
                goto ERROR1;

            return 0;

        ERROR1:
            return -1;
        }

        /*
         
                    string newXml = @"<type reader='_测试读者类型'>
                                                    <param name='可借总册数' value='10' />
                                                    <param name='可预约册数' value='5' />
                                                    <param name='以停代金因子' value='' />
                                                    <param name='工作日历名' value='' />
                                                    <type book='_测试图书类型'>
                                                        <param name='可借册数' value='10' />
                                                        <param name='借期' value='31day,15day' />
                                                        <param name='超期违约金因子' value='' />
                                                        <param name='丢失违约金因子' value='1.5' />
                                                    </type>
                                                    </type>
                                                    <readerTypes>
                                                    <item>_测试读者类型</item>
                                                    </readerTypes>
                                                    <bookTypes>
                                                    <item>_测试图书类型</item>
                                                    </bookTypes>";
 */
        // 增加测试用的流通权限
        public int AddTestRightsTable(LibraryChannel channel,
            XmlDocument dom,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // 获取流通权限
            if (dom == null)
            {
                string strRightsTableXml = "";
                nRet = GetRightsTableInfo(channel, out strRightsTableXml, out strError);
                if (nRet == -1)
                    goto ERROR1;
                strRightsTableXml = "<rightsTable>" + strRightsTableXml + "</rightsTable>";
                dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strRightsTableXml);
                }
                catch (Exception ex)
                {
                    strError = "strRightsTableXml装入XMLDOM时发生错误：" + ex.Message;
                    goto ERROR1;
                }
            }


            XmlNode node = dom.DocumentElement.SelectSingleNode("type[@reader='" + C_PatronType + "']");
            // 增加测试用权限
            if (node == null)
            {
                node = dom.CreateElement("type");
                DomUtil.SetAttr(node, "reader", C_PatronType);
                node.InnerXml = @"<param name='可借总册数' value='10' />
                                                <param name='可预约册数' value='5' />
                                                <param name='以停代金因子' value='' />
                                                <param name='工作日历名' value='"+C_CalenderName+@"' />
                                                <type book='" + C_BookType + @"'>
                                                  <param name='可借册数' value='10' />
                                                  <param name='借期' value='31day,60day' />
                                                  <param name='超期违约金因子' value='' />
                                                  <param name='丢失违约金因子' value='1.5' />
                                                </type>";
                dom.DocumentElement.AppendChild(node);


                XmlNode readerTypesNode = dom.DocumentElement.SelectSingleNode("readerTypes");
                if (readerTypesNode == null)
                {
                    readerTypesNode = dom.CreateElement("readerTypes");
                    dom.DocumentElement.AppendChild(readerTypesNode);
                }
                node = dom.CreateElement("item");
                node.InnerText = C_PatronType;
                readerTypesNode.AppendChild(node);

                //    node.InnerXml = "<item>" + C_PatronType + "</item>";
                //dom.DocumentElement.AppendChild(node);

                XmlNode bookTypesNode = dom.DocumentElement.SelectSingleNode("bookTypes");
                if (bookTypesNode == null)
                {
                    bookTypesNode = dom.CreateElement("bookTypes");
                    dom.DocumentElement.AppendChild(bookTypesNode);
                }
                node = dom.CreateElement("item");
                node.InnerText = C_BookType;
                bookTypesNode.AppendChild(node);

                //node = dom.CreateElement("bookTypes");
                //node.InnerXml = "<item>" + C_BookType + "</item>";
                //dom.DocumentElement.AppendChild(node);
            }
            // 保存
            nRet = this.SetRightsTableDef(channel, dom.DocumentElement.InnerXml, out strError);
            if (nRet == -1)
                goto ERROR1;

            return 0;
        ERROR1:
            return -1;
        }
                    

        private void button_createRightsTable_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCheckinCheckout_Click(object sender, EventArgs e)
        {
            string error="";
            int nRet=0;


            REDO:

            for (int i = 0; i < 10; i++)
            {
                string patronBarcode = "_P" + i.ToString().PadLeft(3, '0');

                for (int j = 1; j <= 50; j++)
                {
                    Application.DoEvents();

                    string itemBarcode = "_B" + j.ToString().PadLeft(6, '0');


                    nRet = SCHelper.Instance.Checkout(patronBarcode, itemBarcode, out error);
                    if (nRet == -2) //尚未登录的情况
                    {

                        nRet = SCHelper.Instance.Login("supervisor", "1", out error);
                        if (nRet == 1)
                        {
                            goto REDO;
                        }

                        goto ERROR1;
                    }

                    this.Print(patronBarcode + "借" + itemBarcode + "...");


                    if (nRet == -1)
                    {
                        Print("出错:" + error);
                        continue;
                    }

                    if (nRet == 0)
                    {
                        Print("借出失败:" + error);
                        continue;
                    }

                    this.Print("借书成功");
                }

            }

        ERROR1:
            this.Print(error);

        }


        private void Print(string text)
        {
            if (this.txtInfo.Text != "")
                this.txtInfo.Text += "\r\n";
            this.txtInfo.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + text;

            //this.txtInfo.Text = text;
        }

        // 清空消息
        public void ClearInfo()
        {
            this.txtInfo.Text = "";
        }

        private void 创建流通权限ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;
            LibraryChannel channel = this.GetChannel();
            EnableControls(false);
            try
            {
                // 获取流通权限
                string strRightsTableXml = "";
                nRet = GetRightsTableInfo(channel, out strRightsTableXml, out strError);
                if (nRet == -1)
                    goto ERROR1;
                strRightsTableXml = "<rightsTable>" + strRightsTableXml + "</rightsTable>";
                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strRightsTableXml);
                }
                catch (Exception ex)
                {
                    strError = "strRightsTableXml装入XMLDOM时发生错误：" + ex.Message;
                    goto ERROR1;
                }

                // 先删除原来测试自动增加权限
                nRet = this.RemoveTestRightsTable(channel, dom, out strError);
                if (nRet == -1)
                    goto ERROR1;

                //增加测试用权限
                nRet = this.AddTestRightsTable(channel, dom, out strError);
                if (nRet == -1)
                    goto ERROR1;
            }
            finally
            {
                EnableControls(true);
                this.ReturnChannel(channel);
            }
        ERROR1:
            MessageBox.Show(this, strError);
        }

        //登录
        private void button_login_Click(object sender, EventArgs e)
        {
            string error = "";
            int nRet = SCHelper.Instance.Login("supervisor", "1", out error);
            if (nRet == -1)
            {
                MessageBox.Show(this, "登录出错：" + error);
                return;
            }
            if (nRet==0)
            {
                MessageBox.Show(this, "登录失败：" + error);
                return;
            }
            MessageBox.Show(this, "登录成功");
        }

        //SC status
        private void button_SCStatus_Click(object sender, EventArgs e)
        {
            string error = "";
            ACSStatus_98 response98 = null;
            string responseText = "";
            int nRet = SCHelper.Instance.SCStatus(out response98,
                out responseText, 
                out error);
            if (nRet == -1)
            {
                MessageBox.Show(this, "出错：" + error);
                return;
            }
            if (nRet == 0)
            {
                MessageBox.Show(this, "ACS不在线");
                return;
            }
            MessageBox.Show(this, "ACS在线状态");
        }

        public void CustomerCheckoutAndCheckin(int patrontNum, 
            int itemNum,
            int checkoutNum, 
            int checkinNum)
        {
            //借还
            if (checkoutNum > 0 && checkinNum > 0)
            {

            }
            else if (checkoutNum > 0 && checkinNum==0)
            {
 
            }
            else if (checkoutNum == 0 && checkinNum > 0)
            {
 
            }

        }

        //借书，或者借还书
        public void CheckoutAndCheckin(int patrontNum,
            int itemNum,
            int checkoutNum,
            int checkinNum)
        {
            string error = "";
            int nRet = 0;

            // 清空输出信息
            this.ClearInfo();

        REDO:
            // 循环读者
            for (int i = 0; i < patrontNum; i++)
            {
                string patronBarcode = "_P" + i.ToString().PadLeft(3, '0');

                // 每人 itemNum 册
                for (int j = (i + 1) * itemNum - (itemNum-1); j <= (i + 1) * itemNum; j++)
                {
                    Application.DoEvents();
                    string itemBarcode = "_B" + j.ToString().PadLeft(6, '0');

                    //执行借书
                    for (int a = 0; a < checkoutNum; a++)
                    {
                        nRet = SCHelper.Instance.Checkout(patronBarcode, itemBarcode, out error);
                        if (nRet == -2) //尚未登录的情况
                        {
                            nRet = SCHelper.Instance.Login("supervisor", "1", out error);
                            if (nRet == 1) //登录成功，重新执行
                                goto REDO;

                            MessageBox.Show(this, "登录失败：" + error);
                            return;
                        }
                        this.Print(patronBarcode + "借" + itemBarcode + "...");
                        if (nRet == -1)
                        {
                            Print("出错:" + error);
                            continue;
                        }
                        if (nRet == 0)
                        {
                            Print("借出失败:" + error);
                            continue;
                        }
                        this.Print("借书成功");
                    }

                    //执行还书
                    for (int a = 0; a < checkinNum; a++)
                    {
                        nRet = SCHelper.Instance.Checkin(itemBarcode, out error);
                        this.Print("还" + itemBarcode + "...");
                        if (nRet == -1)
                        {
                            Print("出错:" + error);
                            continue;
                        }
                        if (nRet == 0)
                        {
                            Print("还书失败:" + error);
                            continue;
                        }
                        this.Print("还书成功");
                    }

                }
            }

            return;
        }

        public void Checkin(int itemNum, int checkinNum)
        {
            string error = "";
            int nRet = 0;

            // 清空输出信息
            this.ClearInfo();

            for (int j = 1; j <= itemNum; j++)
            {
                Application.DoEvents();
                string itemBarcode = "_B" + j.ToString().PadLeft(6, '0');

                //执行还书
                for (int a = 0; a < checkinNum; a++)
                {
                    nRet = SCHelper.Instance.Checkin(itemBarcode, out error);
                    if (nRet == -2) //尚未登录的情况
                    {
                        MessageBox.Show(this, "尚未登录");
                        return;
                    }

                    this.Print("还" + itemBarcode + "...");
                    if (nRet == -1)
                    {
                        Print("出错:" + error);
                        continue;
                    }
                    if (nRet == 0)
                    {
                        Print("还书失败:" + error);
                        continue;
                    }
                    this.Print("还书成功");
                }
            }

        }



        //借书 10人*5册*1借
        private void button_checkout_Click(object sender, EventArgs e)
        {
            this.CheckoutAndCheckin(10, 5, 1,0);
            /*
            string error = "";
            int nRet = 0;

            // 清空输出信息
            this.ClearInfo();

        REDO:
            // 10人
            for (int i = 0; i < 10; i++)
            {
                string patronBarcode = "_P" + i.ToString().PadLeft(3, '0');

                // 每人5册
                for (int j = (i+1)*5-4; j <= (i+1)*5; j++)
                {
                    Application.DoEvents();

                    string itemBarcode = "_B" + j.ToString().PadLeft(6, '0');

                    nRet = SCHelper.Instance.Checkout(patronBarcode, itemBarcode, out error);
                    if (nRet == -2) //尚未登录的情况
                    {

                        nRet = SCHelper.Instance.Login("supervisor", "1", out error);
                        if (nRet == 1)
                        {
                            goto REDO;
                        }

                        goto ERROR1;
                    }

                    this.Print(patronBarcode + "借" + itemBarcode + "...");


                    if (nRet == -1)
                    {
                        Print("出错:" + error);
                        continue;
                    }

                    if (nRet == 0)
                    {
                        Print("借出失败:" + error);
                        continue;
                    }
                    this.Print("借书成功");
                }
            }

        return;

        ERROR1:
            this.Print(error);
             */
        }

        //还书 50册*1还
        private void button_checkin_Click(object sender, EventArgs e)
        {
            this.Checkin(50,1);
        }

        // 借还 10人*2册*1借*1还
        private void button_checkoutin_Click(object sender, EventArgs e)
        {
            this.CheckoutAndCheckin(10, 2, 1, 1);

        }

        // 重复还 10册*2次
        private void button_checkin_dup_Click(object sender, EventArgs e)
        {
            this.Checkin(10, 3);
        }

        // 重复借 10人*2册*3借
        private void button_checkout_dup_Click(object sender, EventArgs e)
        {
            this.CheckoutAndCheckin(10, 2, 3, 0);
        }

        // 自定义借还
        private void button_checkoutin_customer_Click(object sender, EventArgs e)
        {
            int patrontNum=0;
            if (this.textBox_checkinout_patronNum.Text != "")
            {
                try
                {
                    patrontNum = Convert.ToInt32(this.textBox_checkinout_patronNum.Text);
                }
                catch
                {
                    MessageBox.Show(this, "读者人数必须是数字");
                    return;
                }
            }
            int itemNum=0;
            if (this.textBox_checkinout_itemNum.Text != "")
            {
                try
                {
                    itemNum = Convert.ToInt32(this.textBox_checkinout_itemNum.Text);
                }
                catch
                {
                    MessageBox.Show(this, "图书册数必须是数字");
                    return;
                }
            }
            int checkoutNum=0;
            if (this.textBox_checkinout_outNum.Text != "")
            {
                try
                {
                    checkoutNum = Convert.ToInt32(this.textBox_checkinout_outNum.Text);
                }
                catch
                {
                    MessageBox.Show(this, "借书次数必须是数字");
                    return;
                }
            }
            int checkinNum = 0;
            if (this.textBox_checkinout_inNum.Text != "")
            {
                try
                {
                    checkinNum = Convert.ToInt32(this.textBox_checkinout_inNum.Text);
                }
                catch
                {
                    MessageBox.Show(this, "还书次数必须是数字");
                    return;
                }
            }

            // 只还书
            if (checkoutNum == 0 && checkinNum > 0)
            {
                if (itemNum == 0)
                {
                    MessageBox.Show(this, "请输入还书的册数");
                    return;
                }
                this.Checkin(itemNum, checkinNum);
                return;
            }

            //借书
            if (checkoutNum > 0)
            {
                if (patrontNum == 0)
                {
                    MessageBox.Show(this, "请输入读者数量");
                    return;
                }
                if (itemNum == 0)
                {
                    MessageBox.Show(this, "请输入图书册数量");
                    return;
                }
                this.CheckoutAndCheckin(patrontNum,
                    itemNum,
                    checkoutNum,
                    checkinNum);
                return;
            }

            MessageBox.Show(this, "请输入借书次数，还书次数");
        }

        // 续借
        private void button_renew_Click(object sender, EventArgs e)
        {
            this.CheckoutAndRenew(10, 2, 1, 2);
        }

        //借书，或者借还书
        public void CheckoutAndRenew(int patrontNum,
            int itemNum,
            int checkoutNum,
            int renewNum)
        {
            string error = "";
            int nRet = 0;

            // 清空输出信息
            this.ClearInfo();

        REDO:
            // 循环读者
            for (int i = 0; i < patrontNum; i++)
            {
                string patronBarcode = "_P" + i.ToString().PadLeft(3, '0');

                // 每人 itemNum 册
                for (int j = (i + 1) * itemNum - (itemNum - 1); j <= (i + 1) * itemNum; j++)
                {
                    Application.DoEvents();
                    string itemBarcode = "_B" + j.ToString().PadLeft(6, '0');

                    //执行借书
                    for (int a = 0; a < checkoutNum; a++)
                    {
                        nRet = SCHelper.Instance.Checkout(patronBarcode, itemBarcode, out error);
                        if (nRet == -2) //尚未登录的情况
                        {
                            nRet = SCHelper.Instance.Login("supervisor", "1", out error);
                            if (nRet == 1) //登录成功，重新执行
                                goto REDO;

                            MessageBox.Show(this, "登录失败：" + error);
                            return;
                        }
                        this.Print(patronBarcode + "借" + itemBarcode + "...");
                        if (nRet == -1)
                        {
                            Print("出错:" + error);
                            continue;
                        }
                        if (nRet == 0)
                        {
                            Print("借出失败:" + error);
                            continue;
                        }
                        this.Print("借书成功");
                    }

                    //执行续借
                    for (int a = 0; a < renewNum; a++)
                    {
                        nRet = SCHelper.Instance.Renew(patronBarcode, itemBarcode, out error);
                        this.Print(patronBarcode + "续借" + itemBarcode + "...");

                        if (nRet == -1)
                        {
                            Print("出错:" + error);
                            continue;
                        }
                        if (nRet == 0)
                        {
                            Print("续借失败:" + error);
                            continue;
                        }
                        this.Print("续借成功");
                    }

                }
            }

            return;
        }

        // 获取读者信息 
        private void button_patronInfo_Click(object sender, EventArgs e)
        {
            this.GetPatronInfo(10);
        }

        public void GetPatronInfo(int patrontNum)
        {
            string error = "";
            int nRet = 0;

            // 清空输出信息
            this.ClearInfo();
            // 循环读者
            for (int i = 0; i < patrontNum; i++)
            {
                string patronBarcode = "_P" + i.ToString().PadLeft(3, '0');
                PatronInformationResponse_64 response64 = null;
                string responseText = "";
                nRet = SCHelper.Instance.GetPatronInformation(patronBarcode,
                    out response64,
                    out responseText,
                    out error);
                if (nRet == -2) //尚未登录的情况
                {
                    MessageBox.Show(this, "登录失败：" + error);
                    return;
                }
                this.Print("获取读者" + patronBarcode + "...");
                if (nRet == -1)
                {
                    Print("出错:" + error);
                    continue;
                }
                this.Print("成功："+responseText);
            }
        }

        //获取册信息
        private void button_itemInfo_Click(object sender, EventArgs e)
        {
            this.GetItemInfo(10);
        }

        public void GetItemInfo(int itemNum)
        {
            string error = "";
            int nRet = 0;

            // 清空输出信息
            this.ClearInfo();
            // 循环读者
            for (int i = 1; i <= itemNum; i++)
            {
                Application.DoEvents();

                string itemBarcode = "_B" + i.ToString().PadLeft(6, '0');

                ItemInformationResponse_18 response18 = null;
                string responseText = "";
                nRet = SCHelper.Instance.GetItemInformation(itemBarcode,
                    out response18,
                    out responseText,
                    out error);
                if (nRet == -2) //尚未登录的情况
                {
                    MessageBox.Show(this, "登录失败：" + error);
                    return;
                }
                this.Print("获取图书" + itemBarcode + "...");
                if (nRet == -1)
                {
                    Print("出错:" + error);
                    continue;
                }
                this.Print("成功：" + responseText);
            }
        }
    }
}
