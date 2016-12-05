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
using SDGrabSharp.Resources;

namespace SDGrabSharp.UI
{
    public partial class frmMain : Form
    {
        DataCache cache;
        Config config;
        public frmMain(DataCache datacache, Config dataconfig)
        {
            InitializeComponent();
            Localize();
            tsStatus.Text = Strings.tsStatusDefaultText;
            cache = datacache;
            config = dataconfig;
        }

        private void Localize()
        {
            toolStripMenuItemFile.Text = Strings.toolStripMenuItemFile;
            toolStripMenuItemRun.Text = Strings.toolStripMenuItemRun;
            toolStripMenuItemExit.Text = Strings.toolStripMenuItemExit;
            toolStripMenuItemTools.Text = Strings.toolStripMenuItemTools;
            toolStripMenuItemOptions.Text = Strings.toolStripMenuItemOptions;
            toolStripMenuItemHelp.Text = Strings.toolStripMenuItemHelp;
            toolStripMenuItemAbout.Text = Strings.toolStripMenuItemAbout;
            this.Text = string.Format(Strings.frmMainTitle, typeof(SDGrabSharp.UI.Program).Assembly.GetName().Version);
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
