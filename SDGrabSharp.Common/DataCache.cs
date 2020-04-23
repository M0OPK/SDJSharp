using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchedulesDirect;
using System.Xml;
using System.Globalization;
using System.Runtime.ExceptionServices;

namespace SDGrabSharp.Common
{
    /// <summary>
    /// Storage for running data from Schedules direct (to prevent overuse of service)
    /// </summary>
    ///
    public class DataCache
    {
        public SDTokenResponse tokenData;
        public SDCountries countryData;
        public Dictionary<string, IEnumerable<SDHeadendsResponse>> headendData;
        public Dictionary<string, SDGetLineupResponse> stationMapData;
        public Dictionary<string, IEnumerable<SDPreviewLineupResponse>> previewStationData;
        private readonly int cacheExpiryHours;

        public DataCache(int cacheExpiryHours)
        {
            this.cacheExpiryHours = cacheExpiryHours;
            Clear();
        }

        private static void AddCacheDateAttribute(XmlElement element, DateTime? cacheDate)
        {
            var cacheAttribute = element.OwnerDocument.CreateAttribute("cachedate");

            cacheAttribute.InnerText = (cacheDate ?? DateTime.UtcNow).ToString("yyyyMMddHHmmss");
            element.Attributes.Append(cacheAttribute);
        }

        private static DateTime? GetCacheDate(XmlNode element)
        {
            if (element.Attributes?["cachedate"] == null) return DateTime.MinValue;
            DateTime elementDate;
            if (DateTime.TryParseExact(element.Attributes["cachedate"].Value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out elementDate))
                return elementDate;

            return DateTime.MinValue;
        }

        public IEnumerable<SDHeadendsResponse> GetHeadendData(SDJson sd, string country, string postcode)
        {
            var headendKey = $"{country},{postcode}";
            if (!headendData.ContainsKey(headendKey))
            {
                var headendDataJS = sd.GetHeadends(country, postcode);

                if (headendDataJS != null)
                    headendData.Add(headendKey, headendDataJS);

                return headendDataJS;
            }
            else
            {
                // Validate oldest cached value
                var oldestDate = headendData[headendKey].Where(line => line.cacheDate != null).Select(line => line.cacheDate).Min() ?? DateTime.UtcNow;
                if (oldestDate <= DateTime.UtcNow.AddHours(0 - cacheExpiryHours))
                {
                    // Delete cached value and return new value
                    headendData.Remove(headendKey);
                    return GetHeadendData(sd, country, postcode);
                }

                // Otherwise return from cache
                return headendData[headendKey];
            }
        }

        public SDCountries GetCountryData(SDJson sd)
        {
            if (countryData == null || (countryData.cacheDate ?? DateTime.UtcNow) <= DateTime.UtcNow.AddHours(0 - cacheExpiryHours))
                countryData = sd.GetCountries();

            return countryData;
        }

        public SDGetLineupResponse GetLineupData(SDJson sd, string lineup)
        {
            if (!stationMapData.ContainsKey(lineup))
            {
                var thisMap = sd.GetLineup(lineup, true);
                if (thisMap != null)
                {
                    stationMapData.Add(lineup, thisMap);
                    return thisMap;
                }
            }
            else
            {
                SDGetLineupResponse map = null;
                if (stationMapData.ContainsKey(lineup))
                {
                    map = stationMapData[lineup];

                    // Validate cache is in date. If not replace it fresh
                    if ((map.cacheDate ?? DateTime.MinValue) <= DateTime.UtcNow.AddHours(0 - cacheExpiryHours))
                    {
                        stationMapData.Remove(lineup);
                        return GetLineupData(sd, lineup);
                    }
                }

                return map;
            }
            return null;
        }

        public IEnumerable<SDPreviewLineupResponse> GetLineupPreview(SDJson sd, string lineup)
        {
            if (!previewStationData.ContainsKey(lineup))
            {
                var thisPreview = sd.GetLineupPreview(lineup);
                if (thisPreview != null)
                {
                    previewStationData.Add(lineup, thisPreview);
                    return thisPreview;
                }
            }
            else
            {
                var preview = previewStationData[lineup];

                // Validate cache is in date. If not replace it fresh
                var oldestDate = preview.Where(line => line.cacheDate != null).Select(line => line.cacheDate).Min() ?? DateTime.UtcNow;
                if (oldestDate <= DateTime.UtcNow.AddHours(0 - cacheExpiryHours))
                {
                    previewStationData.Remove(lineup);
                    return GetLineupPreview(sd, lineup);
                }

                return preview;
            }            
            return null;
        }

        public void Clear()
        {
            tokenData = null;
            countryData = null;
            stationMapData = new Dictionary<string, SDGetLineupResponse>();
            headendData = new Dictionary<string, IEnumerable<SDHeadendsResponse>>();
            previewStationData = new Dictionary<string, IEnumerable<SDPreviewLineupResponse>>();
        }

        public void Save(string filename)
        {
            var cacheXml = new XmlDocument();
            var rootXmlNode = cacheXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            cacheXml.InsertBefore(rootXmlNode, cacheXml.DocumentElement);

            var rootCacheNode = cacheXml.CreateElement("SDGrabSharpCache");

            // Set save date attribute
            rootCacheNode.SetAttribute("cache-save-date", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

            // add Root node to document
            cacheXml.AppendChild(rootCacheNode);

            // Write Countries
            if (countryData != null && countryData.continents.Count > 0)
            {
                var countryRoot = cacheXml.CreateElement("CountryData");
                AddCacheDateAttribute(countryRoot, countryData.cacheDate);

                foreach (var continent in countryData.continents)
                {
                    var continentNode = cacheXml.CreateElement("continent");
                    continentNode.SetAttribute("name", continent.continentname);
                    AddCacheDateAttribute(continentNode, continent.cacheDate);

                    foreach (var country in continent.countries)
                    {
                        var countryNode = cacheXml.CreateElement("country");
                        AddCacheDateAttribute(countryNode, country.cacheDate);
                        if (country.shortName != null)
                            countryNode.SetAttribute("shortname", country.shortName);
                        if (country.postalCodeExample != null)
                            countryNode.SetAttribute("postalCodeExample", country.postalCodeExample);
                        if (country.postalCode != null)
                            countryNode.SetAttribute("postalCode", country.postalCode);
                        countryNode.SetAttribute("onePostalCode", country.onePostalCode ? "true" : "false");
                        if (country.fullName != null)
                            countryNode.InnerText = country.fullName;

                        continentNode.AppendChild(countryNode);
                    }
                    countryRoot.AppendChild(continentNode);
                }
                rootCacheNode.AppendChild(countryRoot);
            }

            // Write headend Data
            if (headendData != null && headendData.Count > 0)
            {
                var headEndRoot = cacheXml.CreateElement("HeadEndData");
                foreach (var headendCombo in headendData)
                {
                    var splitKey = headendCombo.Key.Split(',');
                    var headEndListNode = cacheXml.CreateElement("HeadEndList");
                    headEndListNode.SetAttribute("country", splitKey[0]);
                    headEndListNode.SetAttribute("postcode", splitKey[1]);
                    var headendData = headendCombo.Value;
                    foreach (var headEnd in headendCombo.Value)
                    {
                        var headEndNode = cacheXml.CreateElement("HeadEnd");
                        AddCacheDateAttribute(headEndNode, headEnd.cacheDate);
                        if (headEnd.headend != null)
                            headEndNode.SetAttribute("headend", headEnd.headend);
                        if (headEnd.location != null)
                            headEndNode.SetAttribute("location", headEnd.location);
                        if (headEnd.transport != null)
                            headEndNode.SetAttribute("transport", headEnd.transport);
                        foreach (var lineup in headEnd.lineups)
                        {
                            var lineUpNode = cacheXml.CreateElement("LineUp");
                            AddCacheDateAttribute(lineUpNode, lineup.cacheDate);
                            if (lineup.lineup != null)
                                lineUpNode.SetAttribute("lineup", lineup.lineup);
                            if (lineup.uri != null)
                                lineUpNode.SetAttribute("uri", lineup.uri);
                            if (lineup.name != null)
                                lineUpNode.InnerText = lineup.name;

                            headEndNode.AppendChild(lineUpNode);
                        }
                        headEndListNode.AppendChild(headEndNode);
                    }
                    headEndRoot.AppendChild(headEndListNode);
                }
                rootCacheNode.AppendChild(headEndRoot);
            }

            // Write Preview stations
            if (previewStationData != null && previewStationData.Count > 0)
            {
                var lineupPreviewRoot = cacheXml.CreateElement("PreviewLineupData");
                foreach (var lineup in previewStationData.Select(line => line))
                {
                    var lineupNode = cacheXml.CreateElement("PreviewLineup");
                    var oldestDate = lineup.Value.Where(line => line.cacheDate != null).Select(line => line.cacheDate).Min();

                    AddCacheDateAttribute(lineupNode, oldestDate);
                    lineupNode.SetAttribute("lineup", lineup.Key);
                    foreach (var channel in lineup.Value)
                    {
                        var channelNode = cacheXml.CreateElement("Channel");
                        channelNode.SetAttribute("channel", channel.channel);
                        channelNode.SetAttribute("callsign", channel.callsign);
                        channelNode.InnerText = channel.name;
                        lineupNode.AppendChild(channelNode);
                    }

                    lineupPreviewRoot.AppendChild(lineupNode);
                }

                rootCacheNode.AppendChild(lineupPreviewRoot);
            }

            // Write stations
            if (stationMapData != null && stationMapData.Count > 0)
            {
                var stationMapRoot = cacheXml.CreateElement("LineUpData");
                foreach (var stationMapCombo in stationMapData)
                {
                    var stationMapNode = cacheXml.CreateElement("LineUp");
                    AddCacheDateAttribute(stationMapNode, stationMapCombo.Value.cacheDate);
                    stationMapNode.SetAttribute("lineup", stationMapCombo.Key);

                    // Maps first
                    foreach (var map in stationMapCombo.Value.map)
                    {
                        var mapNode = cacheXml.CreateElement("Map");
                        AddCacheDateAttribute(mapNode, map.cacheDate);
                        mapNode.SetAttribute("atscMajor", map.atscMajor.ToString());
                        mapNode.SetAttribute("atscMinor", map.atscMinor.ToString());
                        if (map.channel != null)
                            mapNode.SetAttribute("channel", map.channel);
                        if (map.deliverySystem != null)
                            mapNode.SetAttribute("deliverySystem", map.deliverySystem);
                        if (map.fec != null)
                            mapNode.SetAttribute("fec", map.fec);
                        mapNode.SetAttribute("frequencyHz", map.frequencyHz.ToString());
                        if (map.logicalChannelNumber != null)
                            mapNode.SetAttribute("logicalChannelNumber", map.logicalChannelNumber);
                        if (map.matchType != null)
                            mapNode.SetAttribute("matchType", map.matchType);
                        if (map.modulationSystem != null)
                            mapNode.SetAttribute("modulationSystem", map.modulationSystem);
                        if (map.networkID != null)
                            mapNode.SetAttribute("networkID", map.networkID);
                        if (map.polarization != null)
                            mapNode.SetAttribute("polarization", map.polarization);
                        if (map.providerCallsign != null)
                            mapNode.SetAttribute("providerCallsign", map.providerCallsign);
                        if (map.serviceID != null)
                            mapNode.SetAttribute("serviceID", map.serviceID);
                        if (map.stationID != null)
                            mapNode.SetAttribute("stationID", map.stationID);
                        mapNode.SetAttribute("symbolrate", map.symbolrate.ToString());
                        if (map.transportID != null)
                            mapNode.SetAttribute("transportID", map.transportID);
                        mapNode.SetAttribute("uhfVhf", map.uhfVhf.ToString());
                        stationMapNode.AppendChild(mapNode);
                    }

                    // Station
                    foreach (var station in stationMapCombo.Value.stations)
                    {
                        var stationNode = cacheXml.CreateElement("Station");
                        AddCacheDateAttribute(stationNode, station.cacheDate);
                        stationNode.SetAttribute("affiliate", station.affiliate);
                        stationNode.SetAttribute("id", station.stationID);
                        stationNode.SetAttribute("callsign", station.callsign);
                        stationNode.SetAttribute("name", station.name);
                        foreach (var lang in station.broadcastLanguage)
                        {
                            var languageNode = cacheXml.CreateElement("BroadcastLanguage");
                            languageNode.InnerText = lang;
                            stationNode.AppendChild(languageNode);
                        }

                        foreach (var lang in station.descriptionLanguage)
                        {
                            var languageNode = cacheXml.CreateElement("DescriptionLanguage");
                            languageNode.InnerText = lang;
                            stationNode.AppendChild(languageNode);
                        }

                        if (station.broadcaster != null)
                        {
                            var broadcasterNode = cacheXml.CreateElement("Broadcaster");
                            broadcasterNode.SetAttribute("city", station.broadcaster.city);
                            broadcasterNode.SetAttribute("state", station.broadcaster.state);
                            broadcasterNode.SetAttribute("postalcode", station.broadcaster.postalcode);
                            broadcasterNode.SetAttribute("country", station.broadcaster.country);
                            stationNode.AppendChild(broadcasterNode);
                        }

                        if (station.logo != null)
                        {
                            var logoNode = cacheXml.CreateElement("Logo");
                            logoNode.SetAttribute("url", station.logo.URL);
                            logoNode.SetAttribute("height", station.logo.height.ToString());
                            logoNode.SetAttribute("width", station.logo.width.ToString());
                            logoNode.SetAttribute("md5", station.logo.md5);
                            stationNode.AppendChild(logoNode);
                        }
                        stationMapNode.AppendChild(stationNode);
                    }

                    // Metadata
                    if (stationMapCombo.Value.metadata != null)
                    {
                        var metaNode = cacheXml.CreateElement("MetaData");
                        metaNode.SetAttribute("lineup", stationMapCombo.Value.metadata.lineup);
                        metaNode.SetAttribute("modified", stationMapCombo.Value.metadata.modified.GetValueOrDefault().ToString("yyyyMMddHHmmss"));
                        metaNode.SetAttribute("transport", stationMapCombo.Value.metadata.transport);
                        metaNode.SetAttribute("modulation", stationMapCombo.Value.metadata.modulation);
                        stationMapNode.AppendChild(metaNode);
                    }
                    stationMapRoot.AppendChild(stationMapNode);
                }
                rootCacheNode.AppendChild(stationMapRoot);
            }

            // Save
            cacheXml.Save(filename);
        }

        private bool validateCacheDate(DateTime? cacheDateTime)
        {
            if (cacheDateTime == null || cacheDateTime.Value.AddHours(cacheExpiryHours) >= DateTime.UtcNow)
                return true;

            return false;
        }

        public bool Load(string filename)
        {
            if (!File.Exists(filename))
                return false;

            Clear();
            var cacheDoc = new XmlDocument();
            cacheDoc.Load(filename);

            var rootNode = cacheDoc.SelectSingleNode("//SDGrabSharpCache");

            if (rootNode == null)
                return false;

            var countryDataNode = rootNode.SelectSingleNode("CountryData");
            var headendDataNode = rootNode.SelectSingleNode("HeadEndData");
            var lineupDataNode = rootNode.SelectSingleNode("LineUpData");
            var previewDataNode = rootNode.SelectSingleNode("PreviewLineupData");

            // Country data
            if (countryDataNode != null)
            {
                countryData = new SDCountries {cacheDate = GetCacheDate(countryDataNode)};

                if (validateCacheDate(countryData.cacheDate))
                {
                    var continentNodes = countryDataNode.SelectNodes("continent");
                    foreach (XmlNode continentNode in continentNodes)
                    {
                        var thisContinent = new SDCountries.Continent
                        {
                            cacheDate = GetCacheDate(continentNode),
                            continentname = continentNode.Attributes["name"].Value
                        };

                        if (validateCacheDate(thisContinent.cacheDate))
                        {
                            var countryNodes = continentNode.SelectNodes("country");
                            foreach (XmlNode countryNode in countryNodes)
                            {
                                var thisCountry = new SDCountries.Country {cacheDate = GetCacheDate(countryNode)};
                                if (validateCacheDate(thisCountry.cacheDate))
                                {
                                    if (countryNode.Attributes["shortname"] != null)
                                        thisCountry.shortName = countryNode.Attributes["shortname"].Value;
                                    if (countryNode.Attributes["postalCodeExample"] != null)
                                        thisCountry.postalCodeExample =
                                            countryNode.Attributes["postalCodeExample"].Value;
                                    if (countryNode.Attributes["postalCode"] != null)
                                        thisCountry.postalCode = countryNode.Attributes["postalCode"].Value;
                                    if (countryNode.Attributes["onePostalCode"] != null)
                                        thisCountry.onePostalCode =
                                            countryNode.Attributes["onePostalCode"].Value == "true";
                                    if (countryNode.InnerText != null)
                                        thisCountry.fullName = countryNode.InnerText;
                                    thisContinent.countries.Add(thisCountry);
                                }
                            }

                            countryData.continents.Add(thisContinent);
                        }
                    }
                }
            }

            // Head end data
            if (headendDataNode != null)
            {
                foreach (XmlNode headEndListNode in headendDataNode.SelectNodes("HeadEndList"))
                {
                    var headEnds = new List<SDHeadendsResponse>();
                    var thisKey =
                        $"{headEndListNode.Attributes["country"].Value},{headEndListNode.Attributes["postcode"].Value}";

                    foreach (XmlNode headEndNode in headEndListNode.SelectNodes("HeadEnd"))
                    {
                        var lineUpList = new List<SDHeadendsResponse.SDLineup>();
                        var thisHeadEnd = new SDHeadendsResponse {cacheDate = GetCacheDate(headEndNode)};
                        if (!validateCacheDate(thisHeadEnd.cacheDate)) continue;
                        if (headEndNode.Attributes["headend"] != null)
                            thisHeadEnd.headend = headEndNode.Attributes["headend"].Value;
                        if (headEndNode.Attributes["location"] != null)
                            thisHeadEnd.location = headEndNode.Attributes["location"].Value;
                        if (headEndNode.Attributes["transport"] != null)
                            thisHeadEnd.transport = headEndNode.Attributes["transport"].Value;

                        foreach (XmlNode lineUpNode in headEndNode.SelectNodes("LineUp"))
                        {
                            var thisLineup = new SDHeadendsResponse.SDLineup {cacheDate = GetCacheDate(lineUpNode)};
                            if (!validateCacheDate(thisLineup.cacheDate)) continue;
                            if (lineUpNode.Attributes["lineup"] != null)
                                thisLineup.lineup = lineUpNode.Attributes["lineup"].Value;
                            if (lineUpNode.Attributes["uri"] != null)
                                thisLineup.uri = lineUpNode.Attributes["uri"].Value;
                            if (lineUpNode.InnerText != null)
                                thisLineup.name = lineUpNode.InnerText;
                            lineUpList.Add(thisLineup);
                        }

                        thisHeadEnd.lineups = lineUpList.ToArray();
                        headEnds.Add(thisHeadEnd);
                    }

                    headendData.Add(thisKey, headEnds);
                }
            }

            if (previewDataNode != null)
            {
                foreach (XmlNode previewLineupNode in previewDataNode.SelectNodes("PreviewLineup"))
                {
                    var lineup = previewLineupNode?.Attributes["lineup"]?.Value ?? String.Empty;
                    var previewList = new List<SDPreviewLineupResponse>();
                    foreach (XmlNode channelNode in previewLineupNode.SelectNodes("Channel"))
                    {
                        var channel = new SDPreviewLineupResponse {cacheDate = GetCacheDate(previewLineupNode)};
                        if (validateCacheDate(channel.cacheDate))
                        {
                            channel.channel = channelNode?.Attributes["channel"]?.Value ?? string.Empty;
                            channel.callsign = channelNode?.Attributes["callsign"]?.Value ?? string.Empty;
                            channel.name = channelNode.InnerText ?? string.Empty;
                            previewList.Add(channel);
                        }
                    }
                    previewStationData.Add(lineup, previewList);
                }
            }

            // Map/Station/Metadata
            if (lineupDataNode != null)
            {
                //stationMapData = new Dictionary<string, SDGetLineupResponse>();
                foreach (XmlNode lineUpNode in lineupDataNode.SelectNodes("LineUp"))
                {
                    var thisKey = lineUpNode.Attributes["lineup"].Value;
                    var thisStationMap = new SDGetLineupResponse {cacheDate = GetCacheDate(lineUpNode)};

                    if (validateCacheDate(thisStationMap.cacheDate))
                    {
                        // Maps
                        var mapList = new List<SDGetLineupResponse.SDLineupMap>();
                        foreach (XmlNode mapNode in lineUpNode.SelectNodes("Map"))
                        {
                            var thisMap = new SDGetLineupResponse.SDLineupMap {cacheDate = GetCacheDate(mapNode)};
                            if (validateCacheDate(thisMap.cacheDate))
                            {
                                if (mapNode.Attributes["atscMajor"] != null)
                                    thisMap.atscMajor = int.Parse(mapNode.Attributes["atscMajor"].Value);
                                if (mapNode.Attributes["atscMinor"] != null)
                                    thisMap.atscMinor = int.Parse(mapNode.Attributes["atscMinor"].Value);
                                if (mapNode.Attributes["channel"] != null)
                                    thisMap.channel = mapNode.Attributes["channel"].Value;
                                if (mapNode.Attributes["deliverySystem"] != null)
                                    thisMap.deliverySystem = mapNode.Attributes["deliverySystem"].Value;
                                if (mapNode.Attributes["fec"] != null)
                                    thisMap.fec = mapNode.Attributes["fec"].Value;
                                if (mapNode.Attributes["frequencyHz"] != null)
                                    thisMap.frequencyHz = UInt64.Parse(mapNode.Attributes["frequencyHz"].Value);
                                if (mapNode.Attributes["logicalChannelNumber"] != null)
                                    thisMap.logicalChannelNumber = mapNode.Attributes["logicalChannelNumber"].Value;
                                if (mapNode.Attributes["matchType"] != null)
                                    thisMap.matchType = mapNode.Attributes["matchType"].Value;
                                if (mapNode.Attributes["modulationSystem"] != null)
                                    thisMap.modulationSystem = mapNode.Attributes["modulationSystem"].Value;
                                if (mapNode.Attributes["networkID"] != null)
                                    thisMap.networkID = mapNode.Attributes["networkID"].Value;
                                if (mapNode.Attributes["polarization"] != null)
                                    thisMap.polarization = mapNode.Attributes["polarization"].Value;
                                if (mapNode.Attributes["providerCallsign"] != null)
                                    thisMap.providerCallsign = mapNode.Attributes["providerCallsign"].Value;
                                if (mapNode.Attributes["serviceID"] != null)
                                    thisMap.serviceID = mapNode.Attributes["serviceID"].Value;
                                if (mapNode.Attributes["stationID"] != null)
                                    thisMap.stationID = mapNode.Attributes["stationID"].Value;
                                if (mapNode.Attributes["symbolrate"] != null)
                                    thisMap.symbolrate = int.Parse(mapNode.Attributes["symbolrate"].Value);
                                if (mapNode.Attributes["transportID"] != null)
                                    thisMap.transportID = mapNode.Attributes["transportID"].Value;
                                if (mapNode.Attributes["uhfVhf"] != null)
                                    thisMap.uhfVhf = int.Parse(mapNode.Attributes["uhfVhf"].Value);
                                mapList.Add(thisMap);
                            }
                        }

                        // Stations
                        var stationList =
                            new List<SDGetLineupResponse.SDLineupStation>();
                        foreach (XmlNode stationNode in lineUpNode.SelectNodes("Station"))
                        {
                            var thisStation = new SDGetLineupResponse.SDLineupStation
                            {
                                cacheDate = GetCacheDate(stationNode)
                            };
                            if (validateCacheDate(thisStation.cacheDate))
                            {
                                if (stationNode.Attributes["affiliate"] != null)
                                    thisStation.affiliate = stationNode.Attributes["affiliate"].Value;
                                if (stationNode.Attributes["id"] != null)
                                    thisStation.stationID = stationNode.Attributes["id"].Value;
                                if (stationNode.Attributes["callsign"] != null)
                                    thisStation.callsign = stationNode.Attributes["callsign"].Value;
                                if (stationNode.Attributes["name"] != null)
                                    thisStation.name = stationNode.Attributes["name"].Value;

                                var broadcastLanguages = new List<string>();
                                foreach (XmlNode broadcastLanguageNode in stationNode.SelectNodes("BroadcastLanguage"))
                                    broadcastLanguages.Add(broadcastLanguageNode.InnerText);
                                thisStation.broadcastLanguage = broadcastLanguages.ToArray();

                                var descriptionLanguages = new List<string>();
                                foreach (XmlNode descriptionLanguageNode in stationNode.SelectNodes(
                                    "DescriptionLanguage"))
                                    descriptionLanguages.Add(descriptionLanguageNode.InnerText);
                                thisStation.descriptionLanguage = descriptionLanguages.ToArray();

                                var broadcasterNode = stationNode.SelectSingleNode("Broadcaster");
                                if (broadcasterNode != null)
                                {
                                    thisStation.broadcaster =
                                        new SDGetLineupResponse.SDLineupStation.SDStationBroadcaster
                                        {
                                            city = broadcasterNode.Attributes["city"].Value,
                                            state = broadcasterNode.Attributes["state"].Value,
                                            postalcode = broadcasterNode.Attributes["postalcode"].Value,
                                            country = broadcasterNode.Attributes["country"].Value
                                        };
                                }

                                var logoNode = stationNode.SelectSingleNode("Logo");
                                if (logoNode != null)
                                {
                                    thisStation.logo = new SDGetLineupResponse.SDLineupStation.SDStationLogo
                                    {
                                        URL = logoNode.Attributes["url"].Value,
                                        height = int.Parse(logoNode.Attributes["height"].Value),
                                        width = int.Parse(logoNode.Attributes["width"].Value),
                                        md5 = logoNode.Attributes["md5"].Value
                                    };
                                }

                                stationList.Add(thisStation);
                            }
                        }

                        thisStationMap.map = mapList.ToArray();
                        thisStationMap.stations = stationList.ToArray();

                        // Metadata
                        var metadataNode = lineUpNode.SelectSingleNode("MetaData");
                        if (metadataNode != null)
                        {
                            thisStationMap.metadata = new SDGetLineupResponse.SDLineupMetadata
                            {
                                lineup = metadataNode.Attributes["lineup"].Value,
                                modified = DateTime.ParseExact(
                                    metadataNode.Attributes["modified"].Value, "yyyyMMddHHmmss",
                                    CultureInfo.InvariantCulture),
                                transport = metadataNode.Attributes["transport"].Value,
                                modulation = metadataNode.Attributes["modulation"].Value
                            };
                        }

                        stationMapData.Add(thisKey, thisStationMap);
                    }
                }
            }
            return true;
        }
    }
}
