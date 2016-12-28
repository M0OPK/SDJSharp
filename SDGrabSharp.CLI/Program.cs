using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDGrabSharp.CLI.Resources;
using SchedulesDirect;
using SDGrabSharp.Common;

namespace SDGrabSharp.CLI
{
    class Program
    {
        private static Config config;
        private static DataCache cache;
        private static XmlTVBuilder builder;
        private static bool needcr;
        private static int lastPercent;

        static void Main(string[] args)
        {
            // Validate arguments
            if (args.Length > 1)
            {
                Console.WriteLine(Strings.TooManyArgs);
                Environment.Exit(1);
            }

            // Initialize objects
            config = new Config();
            cache = new DataCache();

            // Load configuration
            if (args.Length == 1)
                LoadConfig(args[0]);
            else
                LoadConfig();

            // Initialize XMLTV builder
            builder = new XmlTVBuilder(config, cache);
            builder.ActivityLogUpdate += updateActivityLog;
            needcr = false;
            lastPercent = 0;
            builder.RunProcess();

            if (needcr)
                Console.WriteLine("");
            Console.WriteLine(Strings.ProcessComplete);
            Environment.Exit(0);
        }
        private static void updateActivityLog(object sender, XmlTVBuilder.ActivityLogEventArgs args)
        {
            if (needcr)
                Console.WriteLine(string.Empty);
            Console.WriteLine(args.ActivityText);
            needcr = false;
        }

        private static void LoadConfig(string filename = null)
        {
            string fileName = filename != null ? filename : "SDGrabSharp.xml";
            if (!config.Load(fileName))
            {
                Console.WriteLine(string.Format(Strings.TooManyArgs, fileName));
                Environment.Exit(1);
            }
        }
    }
}
