using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLTV
{
    public partial class XmlTV
    {
        public class XmlTVData
        {
            public XmlDocument rootDocument;
            public XmlNode rootNode;
            public XmlNodeList channelNodes
            {
                get
                {
                    return rootNode.SelectNodes("channel");
                }
            }
            public XmlNodeList programmeNodes
            {
                get
                {
                    return rootNode.SelectNodes("programme");
                }
            }

            public XmlTVData(XmlDocument doc)
            {
                rootDocument = doc;
                rootNode = doc.SelectSingleNode("//tv");
            }
        }

        public class XmlLangText
        {
            public string lang;
            public string text;

            public XmlLangText(string language, string langtext)
            {
                lang = language;
                text = langtext;
            }
        }

        public class Config
        {
            public bool RemoveDupeChannels;
            public bool RemoveDupeProgrammes;
            public bool SkipImportedDupeChannels;
            public bool SkipImportedDupeProgrammes;

            public Config()
            {
                RemoveDupeChannels = false;
                RemoveDupeProgrammes = false;
                SkipImportedDupeChannels = false;
                SkipImportedDupeProgrammes = false;
            }
        }
    }
}
