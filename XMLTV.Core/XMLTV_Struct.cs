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
        /// <summary>
        /// Structure and helper properties for XMLTV File holder
        /// </summary>
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
            public string Stop;
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
