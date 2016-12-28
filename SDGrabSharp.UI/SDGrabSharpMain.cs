using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using SDGrabSharp.Common;
using SDGrabSharp.UI.Resources;

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

        private void toolStripMenuItemRun_Click(object sender, EventArgs e)
        {
            rtActivityLog.Clear();
            this.Cursor = Cursors.WaitCursor;
            var builder = new XmlTVBuilder(config, cache, null);
            builder.ActivityLogUpdate += updateActivityLog;
            builder.RunProcess();
            this.Cursor = Cursors.Default;
            builder = null;

            /*this.Cursor = Cursors.WaitCursor;
            menuMain.Enabled = false;
            var builder = new XmlTVBuilder(config, cache, null);
            builder.StatusUpdateReady += handle_BuilderUpdates;
            var updateThread = new Thread(() => doCreateXmlTV(builder));
            updateThread.Start(); */
        }

        private void updateActivityLog(object sender, XmlTVBuilder.ActivityLogEventArgs args)
        {
            rtActivityLog.Text += string.Format("{0}\r\n", args.ActivityText);
            rtActivityLog.SelectionStart = rtActivityLog.Text.Length;
            rtActivityLog.ScrollToCaret();
            Application.DoEvents();
        }

        private void updateUI(XmlTVBuilder.StatusUpdate status)
        {
            if (status.statusMessage != null)
                tsStatus.Text = status.statusMessage;

            if (status.CurrentChannel != 0 && status.TotalChannels != 0)
            {
                tsProgress.Maximum = status.TotalChannels;
                tsProgress.Value = status.CurrentChannel < status.TotalChannels ? status.CurrentChannel : status.TotalChannels;
            }
            else if(status.CurrentProgramme != 0 && status.TotalProgrammes != 0)
            {
                tsProgress.Maximum = status.TotalProgrammes;
                tsProgress.Value = status.CurrentProgramme < status.TotalProgrammes ? status.CurrentProgramme : status.TotalProgrammes;
            }
            else
            {
                tsProgress.Maximum = 0;
                tsProgress.Value = 0;
            }
            Application.DoEvents();
        }

        private void builderComplete(XmlTVBuilder.StatusUpdate status)
        {
            updateUI(status);
            this.Cursor = Cursors.Default;
            tsStatus.Text = Strings.tsStatusDefaultText;
            menuMain.Enabled = true;
        }
    }
}
