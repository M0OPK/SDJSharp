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

        public Config()
        {
            TranslationMatrix = new Dictionary<string, XmlTVTranslation>();
            cacheFilename = "persistentcache.xml";
            PersistantCache = true;
            defaultTranslateMode = XmlTVTranslation.TranslateField.StationName;
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

        public void Save(string filename)
        {
            XmlDocument configXml = new XmlDocument();
            var rootXmlNode = configXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            configXml.InsertBefore(rootXmlNode, configXml.DocumentElement);

            XmlElement rootConfigNode = configXml.CreateElement("SDGrabSharpConfig");

            // Set save date attribute
            rootConfigNode.SetAttribute("config-save-date", DateTimeOffset.Now.ToLocalTime().ToString("yyyyMMddHHmmss zzz").Replace(":",""));

            // add Root node to document
            configXml.AppendChild(rootConfigNode);

            // Simple config elements
            AddSimpleXMLElement(ref rootConfigNode, "sd-username", SDUsername);
            AddSimpleXMLElement(ref rootConfigNode, "sd-password", SDPasswordHash);
            AddSimpleXMLElement(ref rootConfigNode, "sd-alwaysask", LoginAlwaysAsk ? "true" : "false");
            AddSimpleXMLElement(ref rootConfigNode, "persistent-cache", PersistantCache ? "true" : "false");
            AddSimpleXMLElement(ref rootConfigNode, "cache-filename", cacheFilename);

            if (TranslationMatrix != null && TranslationMatrix.Count() != 0)
            {
                XmlElement translateRootNode = configXml.CreateElement("TranslationMatrix");
                translateRootNode.SetAttribute("items", TranslationMatrix.Count().ToString());

                foreach (var translation in TranslationMatrix.Where(item => item.Value.isDeleted == false))
                {
                    XmlElement translateNode = configXml.CreateElement("Translate");
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

            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(filename);

            XmlNode rootConfigNode = configDoc.SelectSingleNode("SDGrabSharpConfig");

            XmlNode workNode = null;

            workNode = rootConfigNode.SelectSingleNode("sd-username");
            if (workNode != null)
                SDUsername = workNode.InnerText;

            workNode = rootConfigNode.SelectSingleNode("sd-password");
            if (workNode != null)
                SDPasswordHash = workNode.InnerText;

            workNode = rootConfigNode.SelectSingleNode("sd-alwaysask");
            if (workNode != null)
                LoginAlwaysAsk = (workNode.InnerText == "true");

            workNode = rootConfigNode.SelectSingleNode("persistent-cache");
            if (workNode != null)
                PersistantCache = (workNode.InnerText == "true");

            workNode = rootConfigNode.SelectSingleNode("cache-filename");
            if (workNode != null)
                cacheFilename = workNode.InnerText;

            XmlNode translateListNode = rootConfigNode.SelectSingleNode("TranslationMatrix");
            if (translateListNode != null)
            {
                TranslationMatrix = new Dictionary<string, XmlTVTranslation>();
                foreach (XmlNode translateNode in translateListNode.SelectNodes("Translate"))
                {
                    XmlTVTranslation translateItem = new XmlTVTranslation();
                    if (translateNode.Attributes["lineup-id"] != null)
                        translateItem.LineupID = translateNode.Attributes["lineup-id"].Value;

                    if (translateNode.Attributes["station-id"] != null)
                        translateItem.SDStationID = translateNode.Attributes["station-id"].Value;

                    if (translateNode.Attributes["field-mode"] != null)
                        translateItem.FieldMode = (XmlTVTranslation.TranslateField)int.Parse(translateNode.Attributes["field-mode"].Value);

                    if (translateNode.Attributes["custom-translate"] != null)
                        translateItem.CustomTranslate = translateNode.Attributes["custom-translate"].Value;
                    translateItem.isDeleted = false;

                    TranslationMatrix.Add(string.Format("{0},{1}", translateItem.LineupID, translateItem.SDStationID), translateItem);
                }
            }

            return true;
        }

        private void AddSimpleXMLElement(ref XmlElement parentNode, string nodeKey, string nodeValue)
        {
            XmlElement newNode = parentNode.OwnerDocument.CreateElement(nodeKey);
            newNode.InnerText = nodeValue;
            parentNode.AppendChild(newNode);
        }
    }
}
