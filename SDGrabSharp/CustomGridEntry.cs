using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDGrabSharp.Common;
using SchedulesDirect;

namespace SDGrabSharp.UI
{
    public partial class CustomGridEntry : Form
    {
        private SDJson sd;
        private Config config;
        private DataCache cache;
        private Dictionary<string, Config.XmlTVTranslation> localTranslate;

        public CustomGridEntry(SDJson sdJS, Config dataconfig, DataCache datacache,
                               Dictionary<string, Config.XmlTVTranslation> dataLocalTranslate)
        {
            InitializeComponent();

            sd = sdJS;
            config = dataconfig;
            cache = datacache;
            localTranslate = dataLocalTranslate;

            foreach (var item in dataLocalTranslate.Select(line => line.Value).Where(line => line.FieldMode == Config.XmlTVTranslation.TranslateField.Custom))
            {
                var thisData = datacache.GetLineupData(sdJS, item.LineupID).stations.
                    Where(line => line.stationID == item.SDStationID).FirstOrDefault();
                if (thisData == null)
                    continue;
                string[] thisLine = new string[] { item.LineupID, item.SDStationID, thisData.name, item.CustomTranslate };
                dgCustomEntry.Rows.Add(thisLine);
            }

            if (dgCustomEntry.Rows.Count > 0)
                dgCustomEntry.CurrentCell = dgCustomEntry.Rows[0].Cells[3];
        }

        private void dgCustomEntry_CellValidated(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dgCustomEntry.Rows)
            {
                var thisLine = localTranslate.Select(line => line.Value).
                    Where(line => line.LineupID == (string)row.Cells[0].Value && 
                                  line.SDStationID == (string)row.Cells[1].Value).FirstOrDefault();

                if (thisLine != null)
                {
                    thisLine.CustomTranslate = (string)row.Cells[3].Value;
                    thisLine.displayNameHelper = thisLine.CustomTranslate;
                }
            }
            this.Close();
        }
    }
}
