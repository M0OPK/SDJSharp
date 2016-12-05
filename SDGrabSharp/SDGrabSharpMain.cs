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

namespace SDGrabSharp.UI
{
    public partial class frmMain : Form
    {
        DataCache cache;
        Config config;
        public frmMain(DataCache datacache, Config dataconfig)
        {
            InitializeComponent();
            tsStatus.Text = "Ready";
            cache = datacache;
            config = dataconfig;
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItemOptions_Click(object sender, EventArgs e)
        {
            frmOptions opts = new frmOptions(cache, config);
            opts.ShowDialog(this);
        }

        private void toolStripMenuItemLoadConfig_Click(object sender, EventArgs e)
        {
            cache.Load("F:\\SchedulesDirect\\persistentcache.xml");
        }
    }
}
