using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XMLTV;

namespace XMLTVDemoUI
{
    public partial class frmXMLTVDemo : Form
    {
        public frmXMLTVDemo()
        {
            InitializeComponent();
        }

        private void btnXmlLoad_Click(object sender, EventArgs e)
        {
            XmlTV xmlTV = new XmlTV();
            Cursor.Current = Cursors.WaitCursor;
            //xmlTV.LoadXmlTV("F:\\SchedulesDirect\\guide16-12-01.xml");
            //xmlTV.LoadXmlTV("F:\\SchedulesDirect\\guide.xml", true);
            //xmlTV.LoadXmlTV("F:\\SchedulesDirect\\Romania.xml");
            xmlTV.AddChannel("Channel01", new XmlLangText[] { new XmlLangText("en", "Channel 001"), new XmlLangText("ro", "Kanel Una") }, "http://channel1.com", "http://icons.com/c1.png" );
            xmlTV.AddChannel("Channel02", new XmlLangText[] { new XmlLangText("en", "Channel 002") }, "http://channel2.com", null);
            xmlTV.AddProgramme("20161201225000 +0000", "20161201230000 +0000", "Channel01", new XmlLangText("en", "Test program"), null,
                               new XmlLangText("en", "The test program"), new XmlLangText[] { new XmlLangText("en", "Test category"),
                               new XmlLangText("en", "Second Category") }, null, null);
            xmlTV.SaveXmlTV("F:\\SchedulesDirect\\SD-Demo1.xml");
            Cursor.Current = Cursors.Default;
            ShowXmlTVErrors(ref xmlTV);
        }

        private void ShowXmlTVErrors(ref XmlTV xmlTV)
        {
            string errorLine = string.Empty;
            foreach (XmlTV.XMLTVError thisError in xmlTV.GetRawErrors())
            {
                errorLine += $"{thisError.message} ({thisError.code.ToString()})\r\n";
                if (thisError.description != string.Empty)
                    errorLine += $"{thisError.description}\r\n";
            }

            if (errorLine != string.Empty)
                MessageBox.Show(this, errorLine, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            xmlTV.ClearErrors();
        }
    }
}
