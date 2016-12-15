using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SchedulesDirect;
using XMLTV;

namespace SDGrabSharp.Common
{
    public partial class XmlTVBuilder
    {
        public class ChannelBlock
        {
            public string lineUp;
            public SDGetLineupResponse.SDLineupStation station;
            public Config.XmlTVTranslation stationTranslation;
            public XmlNode stationNode;
            public bool isNew;
        }


    }
}
