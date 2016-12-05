using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchedulesDirect;
using System.Xml;
using System.Globalization;

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

        public DataCache()
        {
            Clear();
        }

        public IEnumerable<SDHeadendsResponse> GetHeadendData(SDJson sd, string country, string postcode)
        {
            string headendKey = string.Format("{0},{1}", country, postcode);
            if (!headendData.ContainsKey(headendKey))
            {
                var headendDataJS = sd.GetHeadends(country, postcode);
                if (headendDataJS != null)
                    headendData.Add(headendKey, headendDataJS);

                return headendDataJS;
            }
            else
            {
                return headendData[headendKey];
            }
        }

        public SDCountries GetCountryData(SDJson sd)
        {
            if (countryData == null)
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
                    map = stationMapData[lineup];
                return map;
            }
            return null;
        }

        public void Clear()
        {
            tokenData = null;
            countryData = null;
            stationMapData = new Dictionary<string, SDGetLineupResponse>();
            headendData = new Dictionary<string, IEnumerable<SDHeadendsResponse>>();
        }

        public void Save(string filename)
        {
            XmlDocument cacheXml = new XmlDocument();
            var rootXmlNode = cacheXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            cacheXml.InsertBefore(rootXmlNode, cacheXml.DocumentElement);

            XmlElement rootCacheNode = cacheXml.CreateElement("SDGrabSharpCache");

            // Set save date attribute
            rootCacheNode.SetAttribute("cache-save-date", DateTimeOffset.Now.ToLocalTime().ToString("yyyyMMddHHmmss"));

            // add Root node to document
            cacheXml.AppendChild(rootCacheNode);

            // Write Countries
            if (countryData != null && countryData.continents.Count > 0)
            {
                XmlElement countryRoot = cacheXml.CreateElement("CountryData");
                foreach (var continent in countryData.continents)
                {
                    XmlElement continentNode = cacheXml.CreateElement("continent");
                    continentNode.SetAttribute("name", continent.continentname);

                    foreach (var country in continent.countries)
                    {
                        XmlElement countryNode = cacheXml.CreateElement("country");
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
                XmlElement headEndRoot = cacheXml.CreateElement("HeadEndData");
                foreach (var headendCombo in headendData)
                {
                    string[] splitKey = headendCombo.Key.Split(',');
                    XmlElement headEndListNode = cacheXml.CreateElement("HeadEndList");
                    headEndListNode.SetAttribute("country", splitKey[0]);
                    headEndListNode.SetAttribute("postcode", splitKey[1]);
                    var headendData = headendCombo.Value;
                    foreach (var headEnd in headendCombo.Value)
                    {
                        XmlElement headEndNode = cacheXml.CreateElement("HeadEnd");
                        if (headEnd.headend != null)
                            headEndNode.SetAttribute("headend", headEnd.headend);
                        if (headEnd.location != null)
                            headEndNode.SetAttribute("location", headEnd.location);
                        if (headEnd.transport != null)
                            headEndNode.SetAttribute("transport", headEnd.transport);
                        foreach (var lineup in headEnd.lineups)
                        {
                            XmlElement lineUpNode = cacheXml.CreateElement("LineUp");
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

            // Write stations
            if (stationMapData != null && stationMapData.Count > 0)
            {
                XmlElement stationMapRoot = cacheXml.CreateElement("LineUpData");
                foreach (var stationMapCombo in stationMapData)
                {
                    XmlElement stationMapNode = cacheXml.CreateElement("LineUp");
                    stationMapNode.SetAttribute("lineup", stationMapCombo.Key);

                    // Maps first
                    foreach (var map in stationMapCombo.Value.map)
                    {
                        XmlElement mapNode = cacheXml.CreateElement("Map");
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
                        XmlElement stationNode = cacheXml.CreateElement("Station");
                        stationNode.SetAttribute("affiliate", station.affiliate);
                        stationNode.SetAttribute("id", station.stationID);
                        stationNode.SetAttribute("callsign", station.callsign);
                        stationNode.SetAttribute("name", station.name);
                        foreach (string lang in station.broadcastLanguage)
                        {
                            XmlElement languageNode = cacheXml.CreateElement("BroadcastLanguage");
                            languageNode.InnerText = lang;
                            stationNode.AppendChild(languageNode);
                        }

                        foreach (string lang in station.descriptionLanguage)
                        {
                            XmlElement languageNode = cacheXml.CreateElement("DescriptionLanguage");
                            languageNode.InnerText = lang;
                            stationNode.AppendChild(languageNode);
                        }

                        if (station.broadcaster != null)
                        {
                            XmlElement broadcasterNode = cacheXml.CreateElement("Broadcaster");
                            broadcasterNode.SetAttribute("city", station.broadcaster.city);
                            broadcasterNode.SetAttribute("state", station.broadcaster.state);
                            broadcasterNode.SetAttribute("postalcode", station.broadcaster.postalcode);
                            broadcasterNode.SetAttribute("country", station.broadcaster.country);
                            stationNode.AppendChild(broadcasterNode);
                        }

                        if (station.logo != null)
                        {
                            XmlElement logoNode = cacheXml.CreateElement("Logo");
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
                        XmlElement metaNode = cacheXml.CreateElement("MetaData");
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

        public bool Load(string filename)
        {
            if (!File.Exists(filename))
                return false;

            Clear();
            XmlDocument cacheDoc = new XmlDocument();
            cacheDoc.Load(filename);

            var rootNode = cacheDoc.SelectSingleNode("//SDGrabSharpCache");

            if (rootNode == null)
                return false;

            var countryDataNode = rootNode.SelectSingleNode("CountryData");
            var hendendDataNode = rootNode.SelectSingleNode("HeadEndData");
            var lineupDataNode = rootNode.SelectSingleNode("LineUpData");

            // Country data
            if (countryDataNode != null)
            {
                countryData = new SDCountries();

                var continentNodes = countryDataNode.SelectNodes("continent");
                foreach (XmlNode continentNode in continentNodes)
                {
                    var thisContinent = new SDCountries.Continent();
                    thisContinent.continentname = continentNode.Attributes["name"].Value;

                    var countryNodes = continentNode.SelectNodes("country");
                    foreach (XmlNode countryNode in countryNodes)
                    {
                        var thisCountry = new SDCountries.Country();
                        if (countryNode.Attributes["shortname"] != null)
                            thisCountry.shortName = countryNode.Attributes["shortname"].Value;
                        if (countryNode.Attributes["postalCodeExample"] != null)
                            thisCountry.postalCodeExample = countryNode.Attributes["postalCodeExample"].Value;
                        if (countryNode.Attributes["postalCode"] != null)
                            thisCountry.postalCode = countryNode.Attributes["postalCode"].Value;
                        if (countryNode.Attributes["onePostalCode"] != null)
                            thisCountry.onePostalCode = (countryNode.Attributes["onePostalCode"].Value == "true");
                        if (countryNode.InnerText != null)
                            thisCountry.fullName = countryNode.InnerText;
                        thisContinent.countries.Add(thisCountry);
                    }
                    countryData.continents.Add(thisContinent);
                }
            }

            // Head end data
            if (hendendDataNode != null)
            {
                foreach (XmlNode headEndListNode in hendendDataNode.SelectNodes("HeadEndList"))
                {
                    List<SDHeadendsResponse> headEnds = new List<SDHeadendsResponse>();
                    string thisKey = string.Format("{0},{1}", headEndListNode.Attributes["country"].Value, headEndListNode.Attributes["postcode"].Value);

                    foreach (XmlNode headEndNode in headEndListNode.SelectNodes("HeadEnd"))
                    {
                        List<SDHeadendsResponse.SDLineup> lineUpList = new List<SDHeadendsResponse.SDLineup>();
                        SDHeadendsResponse thisHeadEnd = new SDHeadendsResponse();
                        if (headEndNode.Attributes["headend"] != null)
                            thisHeadEnd.headend = headEndNode.Attributes["headend"].Value;
                        if (headEndNode.Attributes["location"] != null)
                            thisHeadEnd.location = headEndNode.Attributes["location"].Value;
                        if (headEndNode.Attributes["transport"] != null)
                            thisHeadEnd.transport = headEndNode.Attributes["transport"].Value;

                        foreach (XmlNode lineUpNode in headEndNode.SelectNodes("LineUp"))
                        {
                            SDHeadendsResponse.SDLineup thisLineup = new SDHeadendsResponse.SDLineup();
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

            // Map/Station/Metadata
            if (lineupDataNode != null)
            {
                //stationMapData = new Dictionary<string, SDGetLineupResponse>();
                foreach (XmlNode lineUpNode in lineupDataNode.SelectNodes("LineUp"))
                {
                    string thisKey = lineUpNode.Attributes["lineup"].Value;
                    SDGetLineupResponse thisStationMap = new SDGetLineupResponse();

                    // Maps
                    List<SDGetLineupResponse.SDLineupMap> mapList = new List<SDGetLineupResponse.SDLineupMap>();
                    foreach (XmlNode mapNode in lineUpNode.SelectNodes("Map"))
                    {
                        var thisMap = new SDGetLineupResponse.SDLineupMap();
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

                    // Stations
                    List<SDGetLineupResponse.SDLineupStation> stationList = new List<SDGetLineupResponse.SDLineupStation>();
                    foreach (XmlNode stationNode in lineUpNode.SelectNodes("Station"))
                    {
                        var thisStation = new SDGetLineupResponse.SDLineupStation();
                        if (stationNode.Attributes["affiliate"] != null)
                            thisStation.affiliate = stationNode.Attributes["affiliate"].Value;
                        if (stationNode.Attributes["id"] != null)
                            thisStation.stationID = stationNode.Attributes["id"].Value;
                        if (stationNode.Attributes["callsign"] != null)
                            thisStation.callsign = stationNode.Attributes["callsign"].Value;
                        if (stationNode.Attributes["name"] != null)
                            thisStation.name = stationNode.Attributes["name"].Value;

                        List<string> broadcastLanguages = new List<string>();
                        foreach (XmlNode broadcastLanguageNode in stationNode.SelectNodes("BroadcastLanguage"))
                            broadcastLanguages.Add(broadcastLanguageNode.InnerText);
                        thisStation.broadcastLanguage = broadcastLanguages.ToArray();

                        List<string> descriptionLanguages = new List<string>();
                        foreach (XmlNode descriptionLanguageNode in stationNode.SelectNodes("DescriptionLanguage"))
                            descriptionLanguages.Add(descriptionLanguageNode.InnerText);
                        thisStation.descriptionLanguage = descriptionLanguages.ToArray();

                        XmlNode broadcasterNode = stationNode.SelectSingleNode("Broadcaster");
                        if (broadcasterNode != null)
                        {
                            thisStation.broadcaster = new SDGetLineupResponse.SDLineupStation.SDStationBroadcaster();
                            thisStation.broadcaster.city = broadcasterNode.Attributes["city"].Value;
                            thisStation.broadcaster.state = broadcasterNode.Attributes["state"].Value;
                            thisStation.broadcaster.postalcode = broadcasterNode.Attributes["postalcode"].Value;
                            thisStation.broadcaster.country = broadcasterNode.Attributes["country"].Value;
                        }

                        XmlNode logoNode = stationNode.SelectSingleNode("Logo");
                        if (logoNode != null)
                        {
                            thisStation.logo = new SDGetLineupResponse.SDLineupStation.SDStationLogo();
                            thisStation.logo.URL = logoNode.Attributes["url"].Value;
                            thisStation.logo.height = int.Parse(logoNode.Attributes["height"].Value);
                            thisStation.logo.width = int.Parse(logoNode.Attributes["width"].Value);
                            thisStation.logo.md5 = logoNode.Attributes["md5"].Value;
                        }
                        stationList.Add(thisStation);
                    }

                    thisStationMap.map = mapList.ToArray();
                    thisStationMap.stations = stationList.ToArray();

                    // Metadata
                    XmlNode metadataNode = lineUpNode.SelectSingleNode("MetaData");
                    if (metadataNode != null)
                    {
                        thisStationMap.metadata = new SDGetLineupResponse.SDLineupMetadata();
                        thisStationMap.metadata.lineup = metadataNode.Attributes["lineup"].Value;
                        thisStationMap.metadata.modified = DateTime.ParseExact(metadataNode.Attributes["modified"].Value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        thisStationMap.metadata.transport = metadataNode.Attributes["transport"].Value;
                        thisStationMap.metadata.modulation = metadataNode.Attributes["modulation"].Value;
                    }
                    stationMapData.Add(thisKey, thisStationMap);
                }
            }
            return true;
        }
    }
}
