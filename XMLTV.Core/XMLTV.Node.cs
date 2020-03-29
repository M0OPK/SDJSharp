using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLTV
{
    public class XmlTVDocument : XmlDocument
    {
        private XmlElement _tvTvElement;
        public XmlTVDocument(string generatorname = "", string generatorurl = "", string sourcename = "")
        {
            initTVDoc(generatorname, generatorurl, sourcename);
        }

        private void initTVDoc(string generatorname = "", string generatorurl = "", string sourcename = "")
        {
            // Generate our own root node/declaration
            var rootXmlNode = CreateXmlDeclaration("1.0", "UTF-8", null);
            InsertBefore(rootXmlNode, DocumentElement);
            _tvTvElement = CreateElement("tv");

            // Set generator attributes
            if (generatorname != string.Empty)
                GeneratorName = generatorname;
            if (generatorurl != string.Empty)
                GeneratorURL = generatorurl;
            if (sourcename != string.Empty)
                SourceName = sourcename;

            // add Root node to document
            AppendChild(_tvTvElement);
        }

        public override void Load(string file)
        {
            this.RemoveChild(_tvTvElement);
            base.Load(file);
            _tvTvElement = (XmlElement)SelectSingleNode("//tv");
        }

        /// <summary>
        /// Create new XML "channel" element populated with channel details provided
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="displayName"></param>
        /// <param name="url"></param>
        /// <param name="iconUrl"></param>
        /// <param name="extraattributes"></param>
        /// <param name="extranodes"></param>
        /// <returns></returns>
        public XmlElement CreateChannelElement(string channelID, XmlLangText[] displayName, string url = null, string iconUrl = null,
                                               IEnumerable<XmlAttribute> extraattributes = null, IEnumerable<XmlNode> extranodes = null)
        {
            var channelNode = CreateElement("channel");

            // Set ID
            channelNode.SetAttribute("id", channelID);

            // Display name(s) if any
            foreach (var thisDisplayName in displayName)
            {
                var displayNameNode = CreateElement("display-name");
                displayNameNode.SetAttribute("lang", thisDisplayName.lang);
                displayNameNode.InnerText = thisDisplayName.text;
                channelNode.AppendChild(displayNameNode);
            }

            // Icon URL if available
            if (iconUrl != null)
            {
                var iconNode = CreateElement("icon");
                iconNode.SetAttribute("src", iconUrl);
                channelNode.AppendChild(iconNode);
            }

            // Channel URL if available
            if (url != null)
            {
                var urlNode = CreateElement("url");
                urlNode.InnerText = url;
                channelNode.AppendChild(urlNode);
            }

            // Any extra attributes
            if (extraattributes != null)
            {
                foreach (var extra in extraattributes)
                    channelNode.Attributes.Append((XmlAttribute)extra.Clone());
            }

            // Any extra nodes
            if (extranodes != null)
            {
                foreach (var extra in extranodes)
                    channelNode.AppendChild(extra.Clone());
            }
            return channelNode;
        }

        /// <summary>
        /// Create new XML "channel" element populated with channel details provided
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="channel"></param>
        /// <param name="title"></param>
        /// <param name="subtitle"></param>
        /// <param name="description"></param>
        /// <param name="categories"></param>
        /// <param name="extraattributes"></param>
        /// <param name="extranodes"></param>
        /// <returns></returns>
        public XmlElement CreateProgrammeElement(string start, string stop, string channel, XmlLangText title = null,
            XmlLangText subtitle = null, XmlLangText description = null,
            XmlLangText[] categories = null, IEnumerable<XmlAttribute> extraattributes = null,
            IEnumerable<XmlElement> extranodes = null)
        {
            var programmelNode = CreateElement("programme");
            programmelNode.SetAttribute("start", start);
            programmelNode.SetAttribute("stop", stop);
            programmelNode.SetAttribute("channel", channel);

            if (title != null)
            {
                var titleNode = CreateElement("title");
                titleNode.SetAttribute("lang", title.lang);
                titleNode.InnerText = title.text;
                programmelNode.AppendChild(titleNode);
            }

            if (subtitle != null)
            {
                var subtitleNode = CreateElement("sub-title");
                subtitleNode.SetAttribute("lang", subtitle.lang);
                subtitleNode.InnerText = subtitle.text;
                programmelNode.AppendChild(subtitleNode);
            }

            if (description != null)
            {
                var descriptionNode = CreateElement("desc");
                descriptionNode.SetAttribute("lang", description.lang);
                descriptionNode.InnerText = description.text;
                programmelNode.AppendChild(descriptionNode);
            }

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    var categoryNode = CreateElement("category");
                    categoryNode.SetAttribute("lang", category.lang);
                    categoryNode.InnerText = category.text;
                    programmelNode.AppendChild(categoryNode);
                }
            }

            // Any extra attributes
            if (extraattributes != null)
            {
                foreach (var extra in extraattributes)
                    programmelNode.Attributes.Append(extra);
            }

            // Any extra nodes
            if (extranodes != null)
            {
                foreach (var extra in extranodes)
                    programmelNode.AppendChild(extra);
            }
            return programmelNode;
        }


        public string GeneratorName
        {
            get => _tvTvElement?.GetAttribute("generator-info-name");
            set => _tvTvElement?.SetAttribute("generator-info-name", value);
        }

        public string GeneratorURL
        {
            get => _tvTvElement?.GetAttribute("generator-info-url");
            set => _tvTvElement?.SetAttribute("generator-info-url", value);
        }

        public string SourceName
        {
            get => _tvTvElement?.GetAttribute("source-info-name");
            set => _tvTvElement?.SetAttribute("source-info-name", value);
        }

        public XmlElement TvElement => _tvTvElement;
    }
}
