using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace dp2analysis
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
        }

        private void dp2服务器配置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Setting dlg = new Form_Setting();
            dlg.ShowDialog(this);
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            string dp2ServerUrl = Properties.Settings.Default.dp2ServerUrl;
            if (string.IsNullOrEmpty(dp2ServerUrl))
            {
                Form_Setting dlg = new Form_Setting();
                dlg.ShowDialog(this);
            }
        }

        private void button_analysis_Click(object sender, EventArgs e)
        {
            // 先加载模块文件
            string appDir = Application.StartupPath;
            string filePath = appDir + "\\" + "analysisTemplate.txt";

            //string 


            SetHtmlString(this.webBrowser1, "test");
        }

        public string GetHtml()
        {
            string html = "<html><body>";



            html += "</body></html>";
            return html;
        }

        static void SetTextString(WebBrowser webBrowser, string strText)
        {
            SetHtmlString(webBrowser, "<pre>" + HttpUtility.HtmlEncode(strText) + "</pre>");
        }
        public static void SetHtmlString(WebBrowser webBrowser,
string strHtml)
        {
            webBrowser.DocumentText = strHtml;
        }
    }
}
