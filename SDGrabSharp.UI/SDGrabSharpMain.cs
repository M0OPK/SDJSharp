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
            toolStripMenuItemLoadConfig.Text = Strings.toolStripMenuItemLoadConfig;
            toolStripMenuItemRun.Text = Strings.toolStripMenuItemRun;
            toolStripMenuItemExit.Text = Strings.toolStripMenuItemExit;
            toolStripMenuItemTools.Text = Strings.toolStripMenuItemTools;
            toolStripMenuItemOptions.Text = Strings.toolStripMenuItemOptions;
            toolStripMenuItemHelp.Text = Strings.toolStripMenuItemHelp;
            toolStripMenuItemAbout.Text = Strings.toolStripMenuItemAbout;
            gbActivityLog.Text = Strings.gbActivityLog;
            gbProgress.Text = Strings.gbProgress;
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
            builder.StatusUpdate += statusUpdate;
            builder.Init();
            builder.LoadXmlTV(config.XmlTVFileName);
            builder.RunProcess();
            builder.SaveXmlTV();
            this.Cursor = Cursors.Default;
            builder = null;
            tsProgress.Value = 0;
            tsStatus.Text = Strings.tsStatusDefaultText;
        }

        private void updateActivityLog(object sender, XmlTVBuilder.ActivityLogEventArgs args)
        {
            if (rtActivityLog.InvokeRequired)
            {
                rtActivityLog.BeginInvoke(new Action(delegate
                {
                    updateActivityLog(sender, args);
                }));
                return;
            }
            rtActivityLog.AppendText(string.Format("{0}\r\n", args.ActivityText));
            rtActivityLog.SelectionStart = rtActivityLog.Text.Length;
            rtActivityLog.ScrollToCaret();
            Application.DoEvents();
        }

        private void statusUpdate(object sender, XmlTVBuilder.StatusUpdateArgs args)
        {
            if (args.statusMessage != null)
                tsStatus.Text = args.statusMessage;

            if (args.progressValue >= 0 && args.progressMax >= 0)
            {
                tsProgress.Maximum = args.progressMax;
                tsProgress.Value = args.progressValue;
            }
            Application.DoEvents();
        }
    }
}
