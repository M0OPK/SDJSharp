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

            foreach (var lineup in lineupList)
            {
                // Build channel list
                var channelList =
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
                        select new
                        {
                            channel,
                            channelTranslate
                        }
                    );

                var existingList = xmlTV.FindMatchingChannelNodes(channelList.
                    Select(line => GetChannelID(line.channel, line.channelTranslate)).Distinct().ToArray());

                if (existingList != null)
                {
                    var detailedResults =
                        (
                            from chanList in channelList
                            join existList in existingList
                                on GetChannelID(chanList.channel, chanList.channelTranslate)
                                equals existList.Attributes["id"].Value into existListOuter
                            from fullList in existListOuter.DefaultIfEmpty()
                            select new
                            {
                                isNew = (fullList == null),
                                channel = chanList.channel,
                                channelTranslate = chanList.channelTranslate
                            }
                        );

                    if (detailedResults != null)
                    {
                        // Replace existing first
                        foreach (var channel in detailedResults.Where(line => line.isNew == false))
                        {
                            var displayName =
                                new XmlTV.XmlLangText[] 
                                { new XmlTV.XmlLangText(channel.channel.descriptionLanguage.FirstOrDefault() ?? "en",
                                GetChannelName(channel.channel, channel.channelTranslate)) };

                            xmlTV.ReplaceChannel(GetChannelID(channel.channel, channel.channelTranslate), displayName,
                                null, channel.channel.logo != null ? channel.channel.logo.URL : null);
                        }

                        // New ones now
                        foreach (var channel in detailedResults.Where(line => line.isNew == true))
                        {
                            var displayName =
                                new XmlTV.XmlLangText[]
                                { new XmlTV.XmlLangText(channel.channel.descriptionLanguage.FirstOrDefault() ?? "en",
                                GetChannelName(channel.channel, channel.channelTranslate)) };

                            xmlTV.AddChannel(GetChannelID(channel.channel, channel.channelTranslate), displayName,
                                null, channel.channel.logo != null ? channel.channel.logo.URL : null);
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
