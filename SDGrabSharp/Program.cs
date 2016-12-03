using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDGrabSharp.Common;

namespace SDGrabSharp.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DataCache cache = new DataCache();
            Config config = new Config();

            if (System.IO.File.Exists("SDGrabSharp.xml"))
                config.Load("SDGrabSharp.xml");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(ref cache, ref config));
        }
    }
}
