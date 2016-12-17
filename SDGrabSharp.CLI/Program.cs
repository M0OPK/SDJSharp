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
        private static string lastStatusUpdate;
        private static string lastChannel;
        private static string lastProgram;
        private static bool needcr;

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
            builder.StatusUpdateReadyAsync += handle_BuilderUpdates;
            lastStatusUpdate = string.Empty;
            lastChannel = string.Empty;
            lastProgram = string.Empty;
            var channels = builder.AddChannels();
            builder.AddProgrammes(channels);
            builder.SaveXmlTV();

            if (needcr)
                Console.WriteLine("");
            Console.WriteLine(Strings.ProcessComplete);
            Environment.Exit(0);
        }

        private static void handle_BuilderUpdates(object sender, EventArgs ev)
        {
            // The async nature makes this inaccurate, but some feedback is better than none.
            var xmlObj = (XmlTVBuilder)sender;
            XmlTVBuilder.StatusUpdate status = xmlObj.statusData;

            if (lastStatusUpdate != status.statusMessage)
            {
                if (needcr)
                    Console.WriteLine("");
                Console.WriteLine(status.statusMessage);
                lastStatusUpdate = status.statusMessage;
                needcr = false;
            }

            if (lastChannel != status.currentChannelID)
            {
                Console.Write(".");
                lastChannel = status.currentChannelID;
                needcr = true;
            }

            if (lastProgram != status.currentProgrammeID)
            {
                Console.Write(".");
                lastProgram = status.currentProgrammeID;
                needcr = true;
            }

            xmlObj.ResetUpdateStatus();

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
