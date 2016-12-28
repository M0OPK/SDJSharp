using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;

namespace XMLTV
{
    public partial class XmlTV
    {
        private List<XMLTVError> localErrors;
        private string _rootGeneratorName;
        private string _rootGeneratorUrl;
        private string _rootDataSourceName;
        private Config config;

        private XmlTVData xmlData;

        public XmlTV(Config configuration = null, string generatorname = "", string generatorurl = "", string datasourcename = "")
        {
            try
            {
                localErrors = new List<XMLTVError>();

                if (generatorname == string.Empty)
                    _rootGeneratorName = "XMLTVSharp 1.0";
                else
                    _rootGeneratorName = generatorname;

                if (generatorurl == string.Empty)
                    _rootGeneratorUrl = "https://github.com/M0OPK/SDJSharp";
                else
                    _rootGeneratorUrl = generatorurl;

                if (datasourcename != string.Empty)
                    _rootDataSourceName = datasourcename;
                else
                    _rootDataSourceName = string.Empty;

                // New document for new data
                XmlDocument newDoc = NewXML(_rootGeneratorName, _rootGeneratorUrl, _rootDataSourceName);
                xmlData = new XmlTVData(newDoc);
                if (configuration == null)
                    config = new Config();
                else
                    config = configuration;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
        }

        /// <summary>
        /// Load an XML TV file, and merge contents into existing data (if any)
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="mergeOnly"></param>
        /// <returns></returns>
        public bool LoadXmlTV(string filename, bool mergeOnly = false)
        {
            try
            {
                XmlDocument xmlTVLoad = new XmlDocument();
                xmlTVLoad.Load(filename);
                XmlTVData localData = new XmlTVData(xmlTVLoad);

                XmlNode localRootTvNode = localData.rootDocument;

                // Check for root TV node
                if (localRootTvNode == null)
                {
                    addError(1002, "root TV node was not found", XMLTVError.ErrorSeverity.Error, "", "LoadXmlTV");
                    return false;
                }

                // Check for dupe channel ids
                if (validateChannel(xmlData, localData))
                    return false;

                if (validateProgramme(xmlData, localData))
                    return false;

                if (mergeOnly)
                    CopyXmlData(ref localData, xmlData, false);
                else
                    CopyXmlData(ref localData, xmlData, true);

                return true;
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
            return false;
        }

        /// <summary>
        /// Clear all existing data and create blank XMLTV workspace
        /// </summary>
        public void Clear()
        {
            try
            {
                localErrors = new List<XMLTVError>();

                // New document for new data
                XmlDocument newDoc = NewXML(_rootGeneratorName, _rootGeneratorUrl, _rootDataSourceName);
                xmlData = new XmlTVData(newDoc);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
        }

        public XmlNode GetChannel(string channelID)
        {
            if (xmlData.channelData.ContainsKey(channelID))
                return xmlData.channelData[channelID].ChanelNode;
            else
                return null;
        }

        public IEnumerable<XmlNode> GetChannelNodes()
        {
            return xmlData.channelNodes;
        }

        public IEnumerable<XmlNode> GetProgrammeNodes(string channelID)
        {
            return xmlData.programmeNodes(channelID);
        }

        public bool renameChannel(string oldChannelId, string newChannelId)
        {
            if (!xmlData.channelData.ContainsKey(oldChannelId))
                return false;

            // Fail if destination already exists
            if (xmlData.channelData.ContainsKey(newChannelId))
                return false;

            // First get all programmes for this channelblock, and the channel data too
            var channelData = xmlData.channelData[oldChannelId];
            var programmes = channelData.programmeNodes.Values;

            // Rename channel id on all the programmes
            foreach(var program in programmes)
            {
                if (program.Attributes["channel"] != null)
                    program.Attributes["channel"].Value = newChannelId;
            }

            // Rename channel in the node
            if (channelData.ChanelNode.Attributes["id"] != null)
                channelData.ChanelNode.Attributes["id"].Value = newChannelId;

            // Replace dictionary entry with correct ID
            xmlData.channelData.Remove(oldChannelId);

            // Rename any clashed destination too
            /*if (xmlData.channelData.ContainsKey(newChannelId))
            {
                var tempChannelData = xmlData.channelData[newChannelId];
                xmlData.channelData.Remove(newChannelId);
                xmlData.channelData.Add(string.Format("{0}_rename", newChannelId), tempChannelData);
            }*/
            xmlData.channelData.Add(newChannelId, channelData);
            return true;
        }

        public IEnumerable<XmlNode> GetProgrammeNodes()
        {
            // This is not ideal
            List<XmlNode> masterList = new List<XmlNode>();
            foreach (var channel in xmlData.channelData)
            {
                if (channel.Value.programmeNodes.Values != null)
                    masterList.AddRange(channel.Value.programmeNodes.Values);
            }
            return masterList.AsEnumerable();
        }

        public XmlDocument GetDocument()
        {
            return xmlData.rootDocument;
        }

        /// <summary>
        /// Add a single channel to the current dataset
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="displayName"></param>
        /// <param name="url"></param>
        /// <param name="iconUrl"></param>
        /// <param name="extraattributes"></param>
        /// <param name="extranodes"></param>
        /// <returns></returns>
        public XmlNode AddChannel(string channelID, XmlLangText[] displayName, string url = null, string iconUrl= null,
                               IEnumerable<XmlAttribute> extraattributes = null, IEnumerable<XmlNode> extranodes = null)
        {
            try
            {
                XmlElement channelNode = buildChannelNode(channelID, displayName, url, iconUrl, extraattributes, extranodes);
                if (channelNode == null)
                    return null;

                if (xmlData.channelData.ContainsKey(channelID))
                    xmlData.channelData[channelID].ChanelNode = channelNode;
                else
                {
                    Channel thisChannel = new Channel();
                    thisChannel.ChanelNode = channelNode;
                    xmlData.channelData.Add(channelID, thisChannel);
                }
                return channelNode;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /*public XmlNode ReplaceChannel(string channelID, XmlLangText[] displayName, string url = null, string iconUrl = null,
                                   IEnumerable<XmlAttribute> extraattributes = null, IEnumerable<XmlNode> extranodes = null)
        {
            try
            {
                XmlNode existingChannel = FindFirstChannel(channelID);
                if (existingChannel == null)
                    return null;

                XmlElement channelNode = buildChannelNode(channelID, displayName, url, iconUrl, extraattributes, extranodes, true);
                if (channelNode == null)
                    return null;

                xmlData.rootNode.ReplaceChild(channelNode, existingChannel);
                return channelNode;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }*/

        private XmlElement buildChannelNode(string channelID, XmlLangText[] displayName, string url = null, string iconUrl = null,
                                            IEnumerable<XmlAttribute> extraattributes = null, IEnumerable<XmlNode> extranodes = null, bool replace = false)
        {
            try
            {
                if (channelID == string.Empty)
                    return null;

                if (!replace)
                {
                    XmlNode collideChannel = FindFirstChannel(channelID);

                    if (collideChannel != null)
                    {
                        addError(1001, "Duplicate channel found", XMLTVError.ErrorSeverity.Error, string.Format("Duplicate channel found:\r\n{0}\r\n", channelID), "AddChannel");
                        return null;
                    }
                }

                XmlElement channelNode = xmlData.rootDocument.CreateElement("channel");

                // Set ID
                channelNode.SetAttribute("id", channelID);

                // Display name(s) if any
                foreach (var thisDisplayName in displayName)
                {
                    XmlElement displayNameNode = xmlData.rootDocument.CreateElement("display-name");
                    displayNameNode.SetAttribute("lang", thisDisplayName.lang);
                    displayNameNode.InnerText = thisDisplayName.text;
                    channelNode.AppendChild(displayNameNode);
                }

                // Icon URL if available
                if (iconUrl != null)
                {
                    XmlElement iconNode = xmlData.rootDocument.CreateElement("icon");
                    iconNode.SetAttribute("src", iconUrl);
                    channelNode.AppendChild(iconNode);
                }

                // Channel URL if available
                if (url != null)
                {
                    XmlElement urlNode = xmlData.rootDocument.CreateElement("url");
                    urlNode.InnerText = url;
                    channelNode.AppendChild(urlNode);
                }

                // Any extra attributes
                if (extraattributes != null)
                {
                    foreach (XmlAttribute extra in extraattributes)
                        channelNode.Attributes.Append((XmlAttribute)extra.Clone());
                }

                // Any extra nodes
                if (extranodes != null)
                {
                    foreach (XmlNode extra in extranodes)
                        channelNode.AppendChild(extra.Clone());
                }
                return channelNode;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        public bool DeleteChannelNode(string channelID)
        {
            try
            {
                if (!xmlData.channelData.ContainsKey(channelID))
                    return false;

                xmlData.channelData.Remove(channelID);
                return true;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return false;
        }

        /// <summary>
        /// Add a single program to the current dataset
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
        public XmlNode AddProgramme(string start, string stop, string channel, XmlLangText title = null, XmlLangText 
                                 subtitle = null, XmlLangText description = null, XmlLangText[] categories = null, 
                                 IEnumerable<XmlAttribute> extraattributes = null, IEnumerable<XmlNode> extranodes = null)
        {
            try
            {
                XmlElement programmelNode = buildProgrammeNode(start, stop, channel, title, subtitle, description,
                                                            categories, extraattributes, extranodes);

                DateTime startTime = XMLTV.XmlTV.StringToDate(start).UtcDateTime;
                if (xmlData.channelData.ContainsKey(channel))
                {
                    if (xmlData.channelData[channel].programmeNodes.ContainsKey(startTime))
                        xmlData.channelData[channel].programmeNodes[startTime] = programmelNode;
                    else
                        xmlData.channelData[channel].programmeNodes.Add(startTime, programmelNode);
                }

                return programmelNode;
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        public XmlElement buildProgrammeNode(string start, string stop, string channel, XmlLangText title = null, 
                                             XmlLangText subtitle = null, XmlLangText description = null,
                                             XmlLangText[] categories = null, IEnumerable<XmlAttribute> extraattributes = null, 
                                             IEnumerable<XmlNode> extranodes = null, bool replace = false)
        {
            try
            {
                // We need at the very least a start/stop time and channel number
                if (start == string.Empty || stop == string.Empty || channel == string.Empty)
                    return null;

                if (!replace)
                {
                    XmlNode collide = FindFirstProgramme(start, channel);

                    if (collide != null)
                    {
                        addError(2001, "Duplicate programme found", XMLTVError.ErrorSeverity.Error,
                                 string.Format("Duplicate programme found:\r\n{0} from {1} to {2}", start, stop, channel), "AddProgramme");
                        return null;
                    }
                }

                XmlElement programmelNode = xmlData.rootDocument.CreateElement("programme");
                programmelNode.SetAttribute("start", start);
                programmelNode.SetAttribute("stop", stop);
                programmelNode.SetAttribute("channel", channel);

                if (title != null)
                {
                    XmlElement titleNode = xmlData.rootDocument.CreateElement("title");
                    titleNode.SetAttribute("lang", title.lang);
                    titleNode.InnerText = title.text;
                    programmelNode.AppendChild(titleNode);
                }

                if (subtitle != null)
                {
                    XmlElement subtitleNode = xmlData.rootDocument.CreateElement("sub-title");
                    subtitleNode.SetAttribute("lang", subtitle.lang);
                    subtitleNode.InnerText = subtitle.text;
                    programmelNode.AppendChild(subtitleNode);
                }

                if (description != null)
                {
                    XmlElement descriptionNode = xmlData.rootDocument.CreateElement("desc");
                    descriptionNode.SetAttribute("lang", description.lang);
                    descriptionNode.InnerText = description.text;
                    programmelNode.AppendChild(descriptionNode);
                }

                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        XmlElement categoryNode = xmlData.rootDocument.CreateElement("category");
                        categoryNode.SetAttribute("lang", category.lang);
                        categoryNode.InnerText = category.text;
                        programmelNode.AppendChild(categoryNode);
                    }
                }

                // Any extra attributes
                if (extraattributes != null)
                {
                    foreach (XmlAttribute extra in extraattributes)
                        programmelNode.Attributes.Append(extra);
                }

                // Any extra nodes
                if (extranodes != null)
                {
                    foreach (XmlNode extra in extranodes)
                        programmelNode.AppendChild(extra);
                }
                return programmelNode;
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        public bool DeleteProgrammeNodeByTimeExact(string start, string channel)
        {
            try
            {
                if (channel == null || start == null || channel == string.Empty || start == string.Empty)
                    return false;

                if (!xmlData.channelData.ContainsKey(channel))
                    return false;

                DateTime startTime = XmlTV.StringToDate(start).UtcDateTime;
                if (!xmlData.channelData[channel].programmeNodes.ContainsKey(startTime))
                    return false;

                xmlData.channelData[channel].programmeNodes.Remove(startTime);
                return true;
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
            return false;
        }

        public IEnumerable<XmlNode> FindMatchingProgrammeNodes(ProgrammeSearch[] searchItems)
        {
            List<XmlNode> programmeNodes = new List<XmlNode>();

            var channelList = xmlData.channelData.Keys.Intersect(searchItems.Select(line => line.Channel));

            foreach(var channelItem in channelList)
            {
                programmeNodes.AddRange
                    (
                        from searchItem in searchItems
                        join programmeNode in xmlData.channelData[channelItem].programmeNodes
                        on XmlTV.StringToDate(searchItem.Start).UtcDateTime equals programmeNode.Key
                        select programmeNode.Value
                    );
            }
            return programmeNodes.AsEnumerable();
        }

        public IEnumerable<XmlNode> FindMatchingChannelNodes(string[] searchItems)
        {
            return
                (
                    from channel in xmlData.channelData
                    join searchItem in searchItems
                        on channel.Key equals searchItem
                    where searchItem != null
                    select channel.Value.ChanelNode
                ).AsEnumerable();
        }

        public string[] DeleteUnmatchingChannelNodes(string[] searchItems)
        {
            var unMatchingItems = xmlData.channelData.
                Where(line => !searchItems.Any(search => search == line.Key)).Select(line => line.Key).ToArray();

            foreach (var deleteItem in unMatchingItems)
                xmlData.channelData.Remove(deleteItem);

            return unMatchingItems;
        }

        public XmlNode[] GetProgrammesOutsideDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            List<XmlNode> nodeList = new List<XmlNode>();
            foreach (var channelInfo in xmlData.channelData)
            {
                // Create list of nodes outside of range (using local time as cutoff points)
                nodeList.AddRange
                (
                    from programme in channelInfo.Value.programmeNodes
                    where programme.Key < startDate.UtcDateTime || programme.Key > endDate.UtcDateTime
                    select programme.Value
                );
            }
            return nodeList.ToArray();
        }

        public void DeleteProgrammesOutsideDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            foreach (var channelInfo in xmlData.channelData)
            {
                // Create list of nodes outside of range (using local time as cutoff points)
                var programmeList =
                (
                    from programme in channelInfo.Value.programmeNodes
                    where programme.Key < startDate.UtcDateTime || programme.Key > endDate.UtcDateTime
                    select programme.Key
                ).ToArray();

                if (programmeList != null)
                {
                    // Delete these nodes
                    foreach (var programme in programmeList)
                        channelInfo.Value.programmeNodes.Remove(programme);
                }
            }
        }

        public static string DateToString(DateTimeOffset inDate, bool utc = false)
        {
            try
            {
                if (!utc)
                    return inDate.ToString("yyyyMMddHHmmss zzzz").Replace(":", "");
                else
                    return inDate.UtcDateTime.ToString("yyyyMMddHHmmss zzzz").Replace(":", "");
            }
            catch
            {
                return null;
            }
        }

        public static DateTimeOffset StringToDate(string inDate, bool dateOnly = false)
        {
            if (dateOnly)
            {
                DateTime tempDate;
                if (DateTime.TryParseExact(inDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out tempDate))
                    return tempDate;
            }

            DateTimeOffset temp;
            if (DateTimeOffset.TryParseExact(inDate, "yyyyMMddHHmmss zzzz", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out temp))
                return temp;

            return DateTimeOffset.ParseExact("19000101", "yyyyMMdd", CultureInfo.InstalledUICulture);
        }

        /// <summary>
        /// Save current data to an XMLTV format file
        /// </summary>
        /// <param name="filename"></param>
        public void SaveXmlTV(string filename)
        {
            try
            {
                // Build up nodes from dictionaries (Channels)
                foreach (XmlNode channelNode in xmlData.channelData.Select(node => node.Value.ChanelNode))
                    xmlData.rootNode.AppendChild(channelNode);

                // Now programmes
                foreach (Channel channelItem in xmlData.channelData.Values)
                {
                    foreach (XmlNode programmeNode in channelItem.programmeNodes.Values)
                        xmlData.rootNode.AppendChild(programmeNode);
                }

                xmlData.rootDocument.Save(filename);
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
        }

        /// <summary>
        /// Return raw error details (if any)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XMLTVError> GetRawErrors()
        {
            return localErrors.AsEnumerable();
        }

        /// <summary>
        /// Retrieve the most recent reported error. If specified pop (remove) after returning
        /// </summary>
        /// <param name="pop"></param>
        /// <returns></returns>
        public XMLTVError GetLastError(bool pop = true)
        {
            var thisError = localErrors.Last();
            if (pop)
                localErrors.Remove(thisError);
            return thisError;
        }

        /// <summary>
        /// Clear any errors
        /// </summary>
        public void ClearErrors()
        {
            localErrors.Clear();
        }

        private void addError(Exception ex)
        {
            localErrors.Add(new XMLTVError(ex));
        }

        private void addError(int errorcode, string errormessage, XMLTVError.ErrorSeverity errorseverity = XMLTVError.ErrorSeverity.Error, string errordescription = "", string errorsource = "")
        {
            localErrors.Add(new XMLTVError(errorcode, errormessage, errorseverity, errordescription, errorsource));
        }

        private XmlDocument NewXML(string generatorname = "", string generatorurl = "", string sourcename = "")
        {
            try
            {
                // Generate out own root node/declaration
                XmlDocument newDoc = new XmlDocument();

                var rootXmlNode = newDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                newDoc.InsertBefore(rootXmlNode, newDoc.DocumentElement);
                XmlElement rootTvNode = newDoc.CreateElement("tv");

                // Set generator attributes
                rootTvNode.SetAttribute("generator-info-name", generatorname);
                rootTvNode.SetAttribute("generator-info-url", generatorurl);
                if (sourcename != string.Empty)
                    rootTvNode.SetAttribute("source-info-name", sourcename);

                // add Root node to document
                newDoc.AppendChild(rootTvNode);

                return newDoc;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        private bool validateChannel(XmlTVData thisXmlFile, XmlTVData localXmlFile)
        {
            // Largely irrelevant with keys @ToDo, fix it up
            try
            {
                IEnumerable<XmlNode> channelNodes = thisXmlFile.channelNodes;
                IEnumerable<XmlNode> localChannelNodes = localXmlFile.channelNodes;

                // Validate local list against self first
                var grouped =
                (
                    from localNode in localChannelNodes
                    group localNode by localNode.Attributes["id"].Value into groupNode
                    where groupNode.Count() > 1
                    select groupNode.Key
                );

                if (grouped != null && grouped.Count() > 0)
                {
                    if (config.RemoveDupeChannels)
                    {
                        // Try to repair
                        foreach (var dupe in grouped)
                        {
                            // Get all matches
                            IEnumerable<XmlNode> matches = localChannelNodes.Cast<XmlNode>().Where(node => node.Attributes["id"].Value == dupe);

                            // Delete all but the first one
                            bool first = true;
                            foreach (XmlElement match in matches)
                            {
                                if (!first)
                                    localXmlFile.channelData.Remove(match.Attributes["id"].Value);

                                first = false;
                            }
                        }
                    }
                    else
                    {
                        string errorMessage = "Duplicate programmes found:\r\n";
                        foreach (var dupe in grouped)
                            errorMessage += string.Format("{0}", dupe);

                        addError(1001, "Duplicate channel(s) found", XMLTVError.ErrorSeverity.Error, errorMessage, "LoadXmlTV");
                    }
                }

                // Find matches in current data
                IEnumerable<XmlNode> dupeChannels =
                (
                    from localNode in localChannelNodes.Cast<XmlNode>()
                    join mainNode in thisXmlFile.channelNodes.Cast<XmlNode>()
                        on localNode.Attributes["id"].Value ?? "" equals mainNode.Attributes["id"].Value ?? ""
                    where mainNode.Attributes["id"] != null && localNode.Attributes["id"] != null
                    select localNode
                );

                if (dupeChannels.Count() != 0)
                {
                    if (config.SkipImportedDupeChannels)
                    {
                        // Remove the duplicates from the input file
                        foreach (XmlNode dupeNode in dupeChannels)
                        {
                            // Get all matches
                            IEnumerable<XmlNode> matches = localChannelNodes.Cast<XmlNode>().Where
                                (node => node.Attributes["id"].Value == dupeNode.Attributes["id"].Value);

                            // Delete all
                            foreach (XmlElement match in matches)
                                localXmlFile.channelData.Remove(match.Attributes["id"].Value);
                        }
                    }
                    else
                    {
                        string errorMessage = "Duplicate channels found:\r\n";
                        foreach (XmlNode dupeNode in dupeChannels)
                            errorMessage += string.Format("{0}\r\n", dupeNode.Attributes["id"].Value ?? "<NULL>");

                        addError(1001, "Duplicate channel(s) found", XMLTVError.ErrorSeverity.Error, errorMessage, "LoadXmlTV");
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }

            return false;
        }

        private bool validateProgramme(XmlTVData thisXmlFile, XmlTVData localXmlFile)
        {
            try
            {
                // Cache this lookup
                var programmeNodes = thisXmlFile.programmeNodes();
                var localProgrammeNodes = localXmlFile.programmeNodes();

                // Validate local list against self first
                var grouped =
                (
                    from localNode in localProgrammeNodes.Cast<XmlNode>()
                    group localNode by new
                    {
                        channelID = localNode.Attributes["channel"].Value,
                        startTime = localNode.Attributes["start"].Value,
                        endTime = localNode.Attributes["stop"].Value
                    } into groupNode
                    where groupNode.Count() > 1
                    select groupNode.Key
                );

                if (grouped != null && grouped.Count() > 0)
                {
                    if (config.RemoveDupeProgrammes)
                    {
                        // Try to repair
                        foreach (var dupe in grouped)
                        {
                            // Get all matches
                            IEnumerable<XmlNode> matches = localProgrammeNodes.Cast<XmlNode>().Where
                                (node => node.Attributes["channel"].Value == dupe.channelID && node.Attributes["start"].Value == dupe.startTime && node.Attributes["stop"].Value == dupe.endTime);

                            // Delete all but the first one
                            bool first = true;
                            foreach (XmlElement match in matches)
                            {
                                if (!first)
                                    localXmlFile.rootNode.RemoveChild(match);

                                first = false;
                            }
                        }
                    }
                    else
                    {
                        string errorMessage = "Duplicate programmes found:\r\n";
                        foreach (var dupe in grouped)
                            errorMessage += string.Format("{0} between {1} and {2}\r\n", dupe.channelID, dupe.startTime, dupe.endTime);

                        addError(2001, "Duplicate programmes(s) found", XMLTVError.ErrorSeverity.Error, errorMessage, "LoadXmlTV");
                    }
                }

                // Find exact matches in current data
                IEnumerable<XmlNode> dupePrograms =
                (
                    from localNode in localProgrammeNodes.Cast<XmlNode>()
                    join mainNode in programmeNodes.Cast<XmlNode>()
                        on new
                        {
                            joinChannel = localNode.Attributes["channel"].Value ?? "",
                            joinStart = localNode.Attributes["start"].Value ?? "",
                            joinEnd = localNode.Attributes["stop"].Value ?? ""
                        }
                        equals new
                        {
                            joinChannel = mainNode.Attributes["channel"].Value ?? "",
                            joinStart = mainNode.Attributes["start"].Value ?? "",
                            joinEnd = mainNode.Attributes["stop"].Value ?? ""
                        }
                    where mainNode.Attributes["channel"] != null && localNode.Attributes["channel"] != null
                    select localNode
                );

                if (dupePrograms.Count() != 0)
                {
                    if (config.SkipImportedDupeProgrammes)
                    {
                        // Remove the duplicates from the input file
                        foreach (XmlNode dupeNode in dupePrograms)
                        {
                            // Get all matches
                            IEnumerable<XmlNode> matches = localProgrammeNodes.Cast<XmlNode>().Where
                                (node => node.Attributes["channel"].Value == dupeNode.Attributes["channel"].Value && 
                                         node.Attributes["start"].Value == dupeNode.Attributes["start"].Value && 
                                         node.Attributes["stop"].Value == dupeNode.Attributes["stop"].Value);

                            // Delete all
                            foreach (XmlElement match in matches)
                                localXmlFile.rootNode.RemoveChild(match);
                        }
                    }
                    else
                    {
                        string errorMessage = "Duplicate programmes found:\r\n";
                        foreach (XmlNode dupeNode in dupePrograms)
                            errorMessage += string.Format("{0} between {1} and {2}\r\n",
                                            dupeNode.Attributes["channel"].Value ?? "<NULL>",
                                            dupeNode.Attributes["start"].Value ?? "<NULL>",
                                            dupeNode.Attributes["stop"].Value ?? "<NULL>");

                        addError(2001, "Duplicate programmes(s) found", XMLTVError.ErrorSeverity.Error, errorMessage, "LoadXmlTV");
                        return true;
                    }
                }

            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
            return false;
        }

        private void CopyXmlData(ref XmlTVData fromData, XmlTVData toData, bool translate = true)
        {
            try
            {
                //XmlNode lastChannel = toData.channelNodes.Cast<XmlNode>().LastOrDefault();

                // Channels
                foreach (var channelNodeData in fromData.channelData)
                {
                    Channel thisChannel = new Channel();
                    thisChannel.ChanelNode = toData.rootDocument.ImportNode(channelNodeData.Value.ChanelNode, true);

                    if (toData.channelData.ContainsKey(channelNodeData.Key))
                        toData.channelData[channelNodeData.Key] = thisChannel;
                    else
                        toData.channelData.Add(channelNodeData.Key, thisChannel);

                    var thisChan = toData.channelData[channelNodeData.Key];
                    // Programmes
                    foreach (var programmeNode in channelNodeData.Value.programmeNodes)
                    {
                        var thisProgrammNode = toData.rootDocument.ImportNode(programmeNode.Value, true);

                        if (thisChan.programmeNodes.ContainsKey(programmeNode.Key))
                            thisChan.programmeNodes[programmeNode.Key] = thisProgrammNode;
                        else
                            thisChan.programmeNodes.Add(programmeNode.Key, thisProgrammNode);
                    }
                }

                /*// Channels only
                foreach (XmlNode channelNode in fromData.channelNodes)
                {
                    // @ToDo some translation here
                    XmlNode thisChannel = toData.rootDocument.ImportNode(channelNode, true);
                    if (lastChannel == null)
                        toData.rootNode.AppendChild(thisChannel);
                    else
                        toData.rootNode.InsertAfter(thisChannel, lastChannel);
                    lastChannel = thisChannel;
                }

                XmlNode lastProgramme = toData.programmeNodes.Cast<XmlNode>().LastOrDefault();

                // Programmes only
                foreach (XmlNode programmenode in fromData.programmeNodes)
                {
                    // @ToDo some translation here
                    XmlNode thisProgramme = toData.rootDocument.ImportNode(programmenode, true);
                    if (lastProgramme == null)
                        toData.rootNode.AppendChild(thisProgramme);
                    else
                        toData.rootNode.InsertAfter(thisProgramme, lastProgramme);
                    lastProgramme = thisProgramme;
                }*/
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            
        }

        private XmlNode FindFirstChannel(string channelID)
        {
            try
            {
                XmlNode channelNode = xmlData.channelNodes.Cast<XmlNode>().Where
                    (chan => chan.Attributes["id"] != null && chan.Attributes["id"].Value == channelID).FirstOrDefault();

                if (channelNode != null)
                    return channelNode;
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        public XmlNode FindFirstProgramme(string start, string channel)
        {
            try
            {
                //XmlNode programmeNode = xmlData.programmeNodes.Cast<XmlNode>().Where
                //    (prog => prog.Attributes["start"].Value == start && prog.Attributes["stop"].Value == stop && prog.Attributes["channel"].Value == channel).FirstOrDefault();
                var startTime = XMLTV.XmlTV.StringToDate(start).UtcDateTime;
                if (!xmlData.channelData.ContainsKey(channel) || !xmlData.channelData[channel].programmeNodes.ContainsKey(startTime))
                    return null;

                return xmlData.channelData[channel].programmeNodes[startTime];
            }
            catch (System.Exception ex)
            {
                addError(ex);
            }
            return null;
        }

    }
}
