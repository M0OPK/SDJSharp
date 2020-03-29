using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDGrabSharp.Common;

namespace SDGrabSharp.UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var config = new Config();

            if (System.IO.File.Exists("SDGrabSharp.xml"))
                config.Load("SDGrabSharp.xml");

            var cache = new DataCache(config.CacheExpiryHours);

            if (config.PersistantCache && config.cacheFilename != string.Empty && System.IO.File.Exists(config.cacheFilename))
                cache.Load(config.cacheFilename);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(cache, config));
        }
    }
}
