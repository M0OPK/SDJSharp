using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLTV
{
    /// <summary>
    /// Generic type for XML strings with a language identifier. Used a lot in XMLTV files
    /// </summary>
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

    public partial class XmlTV
    {
        public class Channel
        {
            public XmlElement ChanelNode;
            public SortedDictionary<DateTime, XmlElement> programmeNodes;

            public Channel()
            {
                programmeNodes = new SortedDictionary<DateTime, XmlElement>();
            }
        }

        /// <summary>
        /// Structure and helper properties for XMLTV File holder
        /// </summary>
        public class XmlTVData
        {
            public XmlTVDocument rootDocument;
            public XmlElement rootNode;
            public Dictionary<string, Channel> channelData;

            public IEnumerable<XmlElement> channelNodes
            {
                get
                {
                    return channelData.Select(channel => channel.Value.ChanelNode);
                }
            }

            public IEnumerable<XmlElement> programmeNodes(string channel)
            {
                if (channelData.ContainsKey(channel))
                    return channelData[channel].programmeNodes.Values;

                return null;
            }
            /*public XmlNodeList channelNodes
            {
                get
                {
                    return rootNode.SelectNodes("channel");
                }
            }*/
            public IEnumerable<XmlElement> programmeNodes()
            {
                var masterChannelList = new List<XmlElement>();

                foreach (var channelId in channelData.Keys)
                    masterChannelList.AddRange(programmeNodes(channelId));

                return masterChannelList;
            }

            public XmlTVData(XmlTVDocument doc)
            {
                rootDocument = doc;
                channelData = new Dictionary<string, Channel>();
                rootNode = doc.TvElement;

                // Build internal dictionary for channels
                foreach (XmlElement channelNode in rootNode.SelectNodes("channel"))
                {
                    rootNode.RemoveChild(channelNode);
                    if (channelNode.Attributes["id"] == null)
                        continue;

                    var thisChannel = new Channel();
                    thisChannel.ChanelNode = channelNode;
                    channelData.Add(channelNode.Attributes["id"].Value, thisChannel);
                }

                // Build internal dictionary for programmes
                foreach (XmlElement programmeNode in rootNode.SelectNodes("programme"))
                {
                    rootNode.RemoveChild(programmeNode);
                    if (programmeNode.Attributes["start"] == null || programmeNode.Attributes["channel"] == null)
                        continue;

                    // Find channelnode
                    var channelId = programmeNode.Attributes["channel"].Value;
                    if (channelData.ContainsKey(channelId))
                    {
                        // Add programme
                        var start = XMLTV.XmlTV.StringToDate(programmeNode.Attributes["start"].Value).UtcDateTime;
                        if (!channelData[channelId].programmeNodes.ContainsKey(start))
                            channelData[channelId].programmeNodes.Add(start, programmeNode);
                    }
                }
            }
        }

        /// <summary>
        /// Error object, contains information about an error, either exception or user
        /// </summary>
        public class XMLTVError
        {
            public Exception exception;
            public bool isException;
            public int code;
            public string message;
            public string description;
            public string source;
            public ErrorSeverity severity;

            public enum ErrorSeverity
            {
                Info,
                Warning,
                Error,
                Fatal
            }

            public XMLTVError(Exception ex)
            {
                isException = true;
                exception = ex;
                code = ex.HResult;
                message = ex.Message;
                description = ex.StackTrace;
                source = ex.Source;
                severity = ErrorSeverity.Fatal;
            }

            public XMLTVError(int errorcode, string errormessage, ErrorSeverity errorseverity = ErrorSeverity.Error, string errordescription = "", string errorsource = "")
            {
                isException = false;
                exception = null;
                code = errorcode;
                message = errormessage;
                description = errordescription;
                source = errorsource;
                severity = errorseverity;
            }
        }

        public class ProgrammeSearch
        {
            public string Start;
            public string Channel;
        }

        /// <summary>
        /// Configuration structure for XMLTV
        /// </summary>
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
