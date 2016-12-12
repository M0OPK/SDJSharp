using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchedulesDirect;
using XMLTV;

namespace SDGrabSharp.Common
{
    public class XmlTVBuilder
    {
        private Config config;
        private DataCache cache;
        private XmlTV xmlTV;
        SDJson sd;

        public XmlTVBuilder(Config inputConfig, DataCache inputCache, SDJson sdJs = null)
        {
            config = inputConfig;
            cache = inputCache;
            // If were supplied an instance, use it, else try to make one with the token in cache data if found, 
            // else no token and we'll login later
            sd = sdJs ?? new SDJson(cache.tokenData != null ? cache.tokenData.token : string.Empty);
        }

        public class ChannelBlock
        {
            public string lineUp;
            public SDGetLineupResponse.SDLineupStation station;
            public Config.XmlTVTranslation stationTranslation;
            public bool isNew;
        }

        public void AddChannels()
        {
            xmlTV = new XmlTV(null, "SDGrabSharp", "https://github.com/M0OPK/SDJSharp", "SchedulesDirect");

            if (System.IO.File.Exists(config.XmlTVFileName))
                xmlTV.LoadXmlTV(config.XmlTVFileName);

            // Ensure this instance is logged in
            if (!sd.LoggedIn)
                sd.Login(config.SDUsername, config.SDPasswordHash, true);

            // Get unique list of lineups from translation matrix (e.g. ones we're interested in)
            var lineupList = config.TranslationMatrix.Select(line => line.Value.LineupID).Distinct();

            // Build a full channel list (all lineups)
            List<ChannelBlock> fullChannelList = new List<ChannelBlock>();
            foreach (var lineup in lineupList)
            {
                fullChannelList.AddRange(
                    (
                            from channelTranslate in config.TranslationMatrix.Select(line => line.Value)
                            join channel in cache.GetLineupData(sd, lineup).stations
                                on new
                                {
                                    joinLineup = channelTranslate.LineupID,
                                    joinChannel = channelTranslate.SDStationID
                                }
                                equals new
                                {
                                    joinLineup = lineup,
                                    joinChannel = channel.stationID
                                }
                            where channelTranslate.isDeleted == false
                            select new ChannelBlock()
                            {
                                lineUp = lineup,
                                station = channel,
                                stationTranslation = channelTranslate,
                                isNew = false
                            }
                    ));
            }

            // Delete anything in the XML file not in this list
            xmlTV.DeleteUnmatchingChannelNodes(fullChannelList.Select(line =>
                GetChannelID(line.station, line.stationTranslation)).Distinct().ToArray());

            foreach (var lineup in lineupList)
            {
                // Build channel list
                var channelList = fullChannelList.Where(line => line.lineUp == lineup);

                var existingNodeList = xmlTV.FindMatchingChannelNodes(channelList.Select(line => 
                    GetChannelID(line.station, line.stationTranslation)).Distinct().ToArray());

                if (existingNodeList != null)
                {
                    var detailedResults =
                        (
                            from chanList in channelList
                            join existList in existingNodeList
                                on GetChannelID(chanList.station, chanList.stationTranslation)
                                equals existList.Attributes["id"].Value into existListOuter
                            from fullList in existListOuter.DefaultIfEmpty()
                            select new ChannelBlock()
                            {
                                isNew = (fullList == null),
                                station = chanList.station,
                                stationTranslation = chanList.stationTranslation,
                                lineUp = lineup
                            }
                        );

                    if (detailedResults != null)
                    {
                        // Replace existing first
                        foreach (var channel in detailedResults.Where(line => line.isNew == false))
                        {
                            var displayName =
                                new XmlTV.XmlLangText[] 
                                { new XmlTV.XmlLangText(channel.station.descriptionLanguage.FirstOrDefault() ?? "en",
                                GetChannelName(channel.station, channel.stationTranslation)) };

                            xmlTV.ReplaceChannel(GetChannelID(channel.station, channel.stationTranslation), displayName,
                                null, channel.station.logo != null ? channel.station.logo.URL : null);
                        }

                        // New ones now
                        foreach (var channel in detailedResults.Where(line => line.isNew == true))
                        {
                            var displayName =
                                new XmlTV.XmlLangText[]
                                { new XmlTV.XmlLangText(channel.station.descriptionLanguage.FirstOrDefault() ?? "en",
                                GetChannelName(channel.station, channel.stationTranslation)) };

                            xmlTV.AddChannel(GetChannelID(channel.station, channel.stationTranslation), displayName,
                                null, channel.station.logo != null ? channel.station.logo.URL : null);
                        }
                    }
                }
            }
            xmlTV.SaveXmlTV(config.XmlTVFileName);
        }

        public string GetChannelID(SDGetLineupResponse.SDLineupStation station, Config.XmlTVTranslation translateStation)
        {
            switch (translateStation.FieldMode)
            {
                case Config.XmlTVTranslation.TranslateField.StationID:
                    return station.stationID;
                case Config.XmlTVTranslation.TranslateField.StationName:
                    return station.name;
                case Config.XmlTVTranslation.TranslateField.StationAffiliate:
                    return station.affiliate;
                case Config.XmlTVTranslation.TranslateField.StationCallsign:
                    return station.callsign;
                case Config.XmlTVTranslation.TranslateField.Custom:
                    return translateStation.CustomTranslate;
                default:
                    return station.name;
            }
        }

        public string GetChannelName(SDGetLineupResponse.SDLineupStation station, Config.XmlTVTranslation translateStation)
        {
            switch (config.XmlTVDisplayNameMode)
            {
                case Config.DisplayNameMode.MatchChannelID:
                    return GetChannelID(station, translateStation);
                case Config.DisplayNameMode.StationID:
                    return station.stationID;
                case Config.DisplayNameMode.StationName:
                    return station.name;
                case Config.DisplayNameMode.StationAffiliate:
                    return station.affiliate;
                case Config.DisplayNameMode.StationCallsign:
                    return station.callsign;
                default:
                    return GetChannelID(station, translateStation);
            }
        }

    }
}
