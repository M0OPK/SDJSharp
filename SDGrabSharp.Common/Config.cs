using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDGrabSharp.Common
{
    public class Config
    {
        public string SDUsername;
        public string SDPasswordHash;
        public bool LoginAlwaysAsk;
        public string cacheFilename;
        public bool PersistantCache;
        public XmlTVTranslation.TranslateField defaultTranslateMode;
        public Dictionary<string, XmlTVTranslation> TranslationMatrix;
        public bool XmlTVLogicalChannelNumber;
        public bool XmlTVStationAfilliate;
        public bool XmlTVStationID;
        public bool XmlTVShowType;
        public bool XmlTVStationName;
        public bool XmlTVStationCallsign;
        public bool ProgrammeRetrieveYesterday;
        public int ProgrammeRetrieveRangeDays;
        public int ScheduleRetrievalItems;
        public int ProgrammeRetrievalItems;
        public DisplayNameMode XmlTVDisplayNameMode;
        public string XmlTVFileName;
        public int CacheExpiryHours;

        public Config()
        {
            TranslationMatrix = new Dictionary<string, XmlTVTranslation>();
            cacheFilename = "persistentcache.xml";
            XmlTVFileName = "guide.xml";
            PersistantCache = true;
            defaultTranslateMode = XmlTVTranslation.TranslateField.StationName;

            ProgrammeRetrieveRangeDays = 3;
            ScheduleRetrievalItems = 3000;
            ProgrammeRetrievalItems = 3000;
            CacheExpiryHours = 24;
        }

        public class XmlTVTranslation
        {
            public string LineupID;
            public string SDStationID;
            public TranslateField FieldMode;
            public string CustomTranslate;
            
            // Don't save these
            public bool isDeleted;
            public string displayNameHelper;

            public enum TranslateField
            {
                StationID,
                StationName,
                StationAffiliate,
                StationCallsign,
                Custom
            }
        }

        public enum DisplayNameMode
        {
            MatchChannelID,
            StationID,
            StationName,
            StationAffiliate,
            StationCallsign
        }

        public void Save(string filename)
        {
            var configXml = new XmlDocument();
            var rootXmlNode = configXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            configXml.InsertBefore(rootXmlNode, configXml.DocumentElement);

            var rootConfigNode = configXml.CreateElement("SDGrabSharpConfig");

            // Set save date attribute
            rootConfigNode.SetAttribute("config-save-date", DateTimeOffset.Now.ToLocalTime().ToString("yyyyMMddHHmmss zzz").Replace(":",""));

            // add Root node to document
            configXml.AppendChild(rootConfigNode);

            // Simple config elements
            AddSimpleXMLElement(rootConfigNode, "sd-username", SDUsername);
            AddSimpleXMLElement(rootConfigNode, "sd-password", SDPasswordHash);
            AddSimpleXMLElement(rootConfigNode, "sd-alwaysask", LoginAlwaysAsk ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "persistent-cache", PersistantCache ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "cache-filename", cacheFilename);
            AddSimpleXMLElement(rootConfigNode, "attrib-logicalnumber", XmlTVLogicalChannelNumber ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "attrib-showtype", XmlTVShowType ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "attrib-afiliate", XmlTVStationAfilliate ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "attrib-callsgn", XmlTVStationCallsign ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "attrib-stationid", XmlTVStationID ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "attrib-stationname", XmlTVStationName ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "programme-retrieveyesterday", ProgrammeRetrieveYesterday ? "true" : "false");
            AddSimpleXMLElement(rootConfigNode, "programme-retrieverangedays", ProgrammeRetrieveRangeDays.ToString());
            AddSimpleXMLElement(rootConfigNode, "schedule-maxitems", ScheduleRetrievalItems.ToString());
            AddSimpleXMLElement(rootConfigNode, "programme-maxitems", ProgrammeRetrievalItems.ToString());
            AddSimpleXMLElement(rootConfigNode, "displayname-mode", ((int)XmlTVDisplayNameMode).ToString());
            AddSimpleXMLElement(rootConfigNode, "xmltv-filename", XmlTVFileName);
            AddSimpleXMLElement(rootConfigNode, "cache-expirehours", CacheExpiryHours.ToString());

            if (TranslationMatrix != null && TranslationMatrix.Count != 0)
            {
                var translateRootNode = configXml.CreateElement("TranslationMatrix");
                translateRootNode.SetAttribute("items", TranslationMatrix.Count.ToString());

                foreach (var translation in TranslationMatrix.Where(item => item.Value.isDeleted == false))
                {
                    var translateNode = configXml.CreateElement("Translate");
                    translateNode.SetAttribute("lineup-id", translation.Value.LineupID);
                    translateNode.SetAttribute("station-id", translation.Value.SDStationID);
                    translateNode.SetAttribute("field-mode", ((int)translation.Value.FieldMode).ToString());
                    if (translation.Value.FieldMode == XmlTVTranslation.TranslateField.Custom)
                        translateNode.SetAttribute("custom-translate", translation.Value.CustomTranslate ?? string.Empty);

                    translateRootNode.AppendChild(translateNode);
                }
                rootConfigNode.AppendChild(translateRootNode);
            }

            configXml.Save(filename);

        }

        public bool Load(string filename)
        {
            if (!System.IO.File.Exists(filename))
                return false;

            var configDoc = new XmlDocument();
            configDoc.Load(filename);

            var rootConfigNode = configDoc.SelectSingleNode("SDGrabSharpConfig");

            XmlNode workNode = null;

            workNode = rootConfigNode.SelectSingleNode("sd-username");
            if (workNode != null)
                SDUsername = workNode.InnerText;

            workNode = rootConfigNode.SelectSingleNode("sd-password");
            if (workNode != null)
                SDPasswordHash = workNode.InnerText;

            workNode = rootConfigNode.SelectSingleNode("sd-alwaysask");
            if (workNode != null)
                LoginAlwaysAsk = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("persistent-cache");
            if (workNode != null)
                PersistantCache = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("cache-filename");
            if (workNode != null)
                cacheFilename = workNode.InnerText;

            workNode = rootConfigNode.SelectSingleNode("attrib-logicalnumber");
            if (workNode != null)
                XmlTVLogicalChannelNumber = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("attrib-showtype");
            if (workNode != null)
                XmlTVShowType = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("attrib-afiliate");
            if (workNode != null)
                XmlTVStationAfilliate = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("attrib-callsgn");
            if (workNode != null)
                XmlTVStationCallsign = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("attrib-stationid");
            if (workNode != null)
                XmlTVStationID = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("attrib-stationname");
            if (workNode != null)
                XmlTVStationName = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("programme-retrieveyesterday");
            if (workNode != null)
                ProgrammeRetrieveYesterday = workNode.InnerText == "true";

            workNode = rootConfigNode.SelectSingleNode("programme-retrieverangedays");
            if (workNode != null)
                try { ProgrammeRetrieveRangeDays = int.Parse(workNode.InnerText); } catch { }

            workNode = rootConfigNode.SelectSingleNode("schedule-maxitems");
            if (workNode != null)
                try { ScheduleRetrievalItems = int.Parse(workNode.InnerText); } catch { }

            workNode = rootConfigNode.SelectSingleNode("programme-maxitems");
            if (workNode != null)
                try { ProgrammeRetrievalItems = int.Parse(workNode.InnerText); } catch { }

            workNode = rootConfigNode.SelectSingleNode("displayname-mode");
            if (workNode != null)
                try { XmlTVDisplayNameMode = (DisplayNameMode)int.Parse(workNode.InnerText); } catch { }

            workNode = rootConfigNode.SelectSingleNode("xmltv-filename");
            if (workNode != null)
                XmlTVFileName = workNode.InnerText;

            workNode = rootConfigNode.SelectSingleNode("cache-expirehours");
            if (workNode != null)
                try { CacheExpiryHours = int.Parse(workNode.InnerText); } catch { }

            var translateListNode = rootConfigNode.SelectSingleNode("TranslationMatrix");
            if (translateListNode != null)
            {
                TranslationMatrix = new Dictionary<string, XmlTVTranslation>();
                foreach (XmlNode translateNode in translateListNode.SelectNodes("Translate"))
                {
                    var translateItem = new XmlTVTranslation();
                    if (translateNode.Attributes["lineup-id"] != null)
                        translateItem.LineupID = translateNode.Attributes["lineup-id"].Value;

                    if (translateNode.Attributes["station-id"] != null)
                        translateItem.SDStationID = translateNode.Attributes["station-id"].Value;

                    if (translateNode.Attributes["field-mode"] != null)
                        translateItem.FieldMode = (XmlTVTranslation.TranslateField)int.Parse(translateNode.Attributes["field-mode"].Value);

                    if (translateNode.Attributes["custom-translate"] != null)
                        translateItem.CustomTranslate = translateNode.Attributes["custom-translate"].Value;
                    translateItem.isDeleted = false;

                    TranslationMatrix.Add(translateItem.SDStationID, translateItem);
                }
            }

            return true;
        }

        private void AddSimpleXMLElement(XmlElement parentNode, string nodeKey, string nodeValue)
        {
            var newNode = parentNode.OwnerDocument.CreateElement(nodeKey);
            newNode.InnerText = nodeValue;
            parentNode.AppendChild(newNode);
        }
    }
}
