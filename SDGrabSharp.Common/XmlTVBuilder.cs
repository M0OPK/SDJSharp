using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;
using SchedulesDirect;
using XMLTV;

namespace SDGrabSharp.Common
{
    public partial class XmlTVBuilder
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

        public IEnumerable<ChannelBlock> AddChannels()
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
            List<ChannelBlock> returnList = new List<ChannelBlock>();
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

            // Get date range from config
            DateTime dateMin = DateTime.Today.Date;
            DateTime dateMax = dateMin.AddDays(config.ProgrammeRetrieveRangeDays);

            if (config.ProgrammeRetrieveYesterday)
                dateMin = dateMin.AddDays(-1.0f);

            // Delete any channel md5 entries outside date range
            List<XmlNode> md5NodesToRemove = new List<XmlNode>();
            foreach (var channelNode in xmlTV.GetChannelNodes().Cast<XmlNode>())
            {
                var md5Nodes = channelNode.SelectNodes("sd-md5").Cast<XmlNode>().
                    Where(line => line.Attributes["date"] != null && (xmlTV.StringToDate(line.Attributes["date"].Value) < dateMin || xmlTV.StringToDate(line.Attributes["date"].Value) > dateMax));
                md5NodesToRemove.AddRange(md5Nodes);
            }

            foreach (var md5Node in md5NodesToRemove)
                md5Node.ParentNode.RemoveChild(md5Node);

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
                                stationNode = fullList,
                                lineUp = lineup
                            }
                        );

                    if (detailedResults != null)
                    {
                        returnList.AddRange(detailedResults);
                        // Replace existing first
                        foreach (var channel in detailedResults.Where(line => line.isNew == false))
                        {
                            var displayName =
                                new XmlTV.XmlLangText[] 
                                { new XmlTV.XmlLangText(fixLang(channel.station.descriptionLanguage.FirstOrDefault()) ?? "en",
                                GetChannelName(channel.station, channel.stationTranslation)) };

                            // Retrieve original MD5 nodes, we'll need them
                            var md5Nodes = channel.stationNode.SelectNodes("sd-md5");

                            var newChannel = xmlTV.ReplaceChannel(GetChannelID(channel.station, channel.stationTranslation), displayName,
                                                                  null, channel.station.logo != null ? channel.station.logo.URL : null, null, 
                                                                  md5Nodes != null ? md5Nodes.Cast<XmlNode>() : null) ;

                            if (newChannel != null)
                                channel.stationNode = newChannel;
                        }

                        // Then add new ones
                        foreach (var channel in detailedResults.Where(line => line.isNew == true))
                        {
                            var displayName =
                                new XmlTV.XmlLangText[]
                                { new XmlTV.XmlLangText(fixLang(channel.station.descriptionLanguage.FirstOrDefault()) ?? "en",
                                GetChannelName(channel.station, channel.stationTranslation)) };

                            xmlTV.AddChannel(GetChannelID(channel.station, channel.stationTranslation), displayName,
                                null, channel.station.logo != null ? channel.station.logo.URL : null, null);
                        }
                    }
                }
            }

            // Return so that programme phase has access to channel date/MD5s
            return returnList.AsEnumerable();
        }

        public void AddProgrammes(IEnumerable<ChannelBlock> channelData)
        {
            // Split channel list
            var updatedList = channelData.Where(line => !line.isNew);
            var addedList = channelData.Where(line => line.isNew);
            var fullUpdateList = new List<ChannelBlock>();
            fullUpdateList.AddRange(addedList);

            // Get date range from config
            DateTime dateMin = DateTime.Today.Date;
            DateTime dateMax = dateMin.AddDays(config.ProgrammeRetrieveRangeDays);

            if (config.ProgrammeRetrieveYesterday)
                dateMin = dateMin.AddDays(-1.0f);

            // Delete any existing program nodes outside this range
            xmlTV.DeleteProgrammesOutsideDateRange(dateMin, dateMax);

            // Extract MD5 info from updated list
            var md5List = updatedList.Select(line =>
                new
                {
                    lineUp = line.lineUp,
                    stationId = line.station.stationID,
                    md5List = line.stationNode.SelectNodes("sd-md5").Cast<XmlNode>().Select(md5Line => 
                        new
                        {
                            date = md5Line.Attributes["date"].Value,
                            md5 = md5Line.InnerText
                        }
                    )
                }
            );

            // Build MD5 request block (we don't care how big, we'll split it later)
            var md5Req = new List<SDMD5Request>();
            var scheduleReq = new List<SDScheduleRequest>();
            foreach (var updateItem in updatedList)
            {
                var md5Nodes = updateItem.stationNode.SelectNodes("sd-md5").Cast<XmlNode>();

                // If updated channel, with no MD5 date, update it all
                if (md5Nodes == null)
                {
                    fullUpdateList.Add(updateItem);
                    continue;
                }

                List<DateTime> schedDates = new List<DateTime>();
                List<DateTime> md5Dates = new List<DateTime>();
                foreach (string thisDate in dateRange)
                {
                    var thisMD5 = md5Nodes.Where(line => line.Attributes["date"].Value == thisDate).FirstOrDefault();
                    if (thisMD5 == null)
                        schedDates.Add(DateTime.ParseExact(thisDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    else
                        md5Dates.Add(DateTime.ParseExact(thisDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                }

                // If we have some MD5s to look up
                if (md5Dates.Count > 0)
                    md5Req.Add(new SDMD5Request(updateItem.station.stationID, md5Dates.AsEnumerable()));

                // If we have some schedules to lookup
                if (schedDates.Count > 0)
                    scheduleReq.Add(new SDScheduleRequest(updateItem.station.stationID, schedDates.AsEnumerable()));
            }

            // Actually retrieve MD5 list
            var splitMD5 = splitArray(md5Req.ToArray(), config.ScheduleRetrievalItems);
            List<SDMD5Response> md5Master = new List<SDMD5Response>();
            foreach (var thisMD5 in splitMD5)
            {
                var thisResponse = sd.GetMD5(thisMD5.AsEnumerable());
                if (thisResponse != null)
                    md5Master.AddRange(thisResponse);
            }
            
            // Check all retrieved MD5 hashes against stored
            foreach (var thisMD5 in md5Master)
            {
                var thisItem = updatedList.Where(line => line.station.stationID == thisMD5.stationID).FirstOrDefault();
                List<DateTime> schedDates = new List<DateTime>();
                foreach (var thisDate in thisMD5.md5day)
                {
                    bool addItem = false;
                    if (thisDate.md5data == null || thisDate.md5data.md5 == null)
                    {
                        addItem = true;
                    }
                    else
                    {
                        var thisDateItem = thisItem.stationNode.SelectNodes("sd-md5").Cast<XmlNode>().Where(node => node.Attributes["date"].Value == thisDate.date).FirstOrDefault();
                        if (thisDateItem.InnerText != thisDate.md5data.md5)
                            addItem = true;
                    }
                    if (addItem)
                        schedDates.Add(DateTime.ParseExact(thisDate.date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                }
                // If we have some schedules to lookup
                if (schedDates.Count > 0)
                    scheduleReq.Add(new SDScheduleRequest(thisMD5.stationID, schedDates.AsEnumerable()));
            }

            // Add full update list for all dates
            List<DateTime> allDates = new List<DateTime>();
            foreach (var thisDate in dateRange)
                allDates.Add(DateTime.ParseExact(thisDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));

            foreach (var thisItem in fullUpdateList)
                scheduleReq.Add(new SDScheduleRequest(thisItem.station.stationID, allDates));

            // Schedule all items
            var scheduleQueue = new RescheduleQueue<SDScheduleRequest>();
            scheduleQueue.AddRange(DateTime.UtcNow, scheduleReq);

            // Create program ID list
            List<string> programmeIdList = new List<string>();

            while (scheduleQueue.Count > 0)
            {
                if (!scheduleQueue.ItemsReady)
                {
                    // Update status here to show we're waiting
                    System.Threading.Thread.Sleep(scheduleQueue.DelayTime);
                    continue;
                }

                // Get ready items (right now it'll be all of them)
                var splitSched = splitArray(scheduleQueue.GetReadyItems().ToArray(), config.ScheduleRetrievalItems);

                List<SDScheduleResponse> schedMaster = new List<SDScheduleResponse>();
                foreach (var thisSched in splitSched)
                {
                    var thisResponse = sd.GetSchedules(thisSched.AsEnumerable());
                    if (thisResponse != null)
                    {
                        schedMaster.AddRange(thisResponse);
                    }
                }

                foreach (var schedule in schedMaster)
                {
                    if (schedule.code == SDErrors.SCHEDULE_QUEUED)
                    {
                        // Get original request, remove from queue
                        var originRequest = scheduleQueue.GetReadyItems().Where(line => line.stationID == schedule.stationID).FirstOrDefault();
                        scheduleQueue.RemoveItem(originRequest);

                        // Add once more, with retry time
                        scheduleQueue.AddItem(schedule.retryTime ?? DateTime.UtcNow, originRequest);
                        continue;
                    }

                    if (schedule.code == SDErrors.OK)
                    {
                        // Get original request, remove from queue
                        var originRequest = scheduleQueue.GetReadyItems().Where(line => line.stationID == schedule.stationID).FirstOrDefault();
                        scheduleQueue.RemoveItem(originRequest);

                        // Remove this specific date from request
                        var dateList = originRequest.date.Where(thisdate => thisdate != schedule.metadata.startDate).ToArray();
                        originRequest.date = dateList;

                        // If there are any dates left, reschedule
                        if (dateList.Length > 0)
                            scheduleQueue.AddItem(DateTime.UtcNow, originRequest);

                        // First, update/set channel MD5
                        var channelBlock = channelData.Where(line => line.station.stationID == schedule.stationID).FirstOrDefault();
                        var channelNode = xmlTV.GetChannel(GetChannelID(channelBlock.station, channelBlock.stationTranslation));
                        var existingNode = channelNode.SelectNodes("sd-md5").Cast<XmlNode>().Where(line => line.Attributes["date"].Value == schedule.metadata.startDate).FirstOrDefault();
                        if (existingNode != null)
                            existingNode.InnerText = schedule.metadata.md5;
                        else
                        {
                            var newNode = channelNode.OwnerDocument.CreateElement("sd-md5");
                            newNode.SetAttribute("date", schedule.metadata.startDate);
                            newNode.InnerText = schedule.metadata.md5;
                            channelNode.AppendChild(newNode);
                        }

                        var programmeNodes = xmlTV.GetProgrammeNodes().Cast<XmlNode>();
                        // Create shell node
                        var rootDoc = xmlTV.GetDocument();

                        foreach (var program in schedule.programs)
                        {
                            if (program.airDateTime == null)
                                continue;

                            DateTimeOffset startTime = new DateTimeOffset(program.airDateTime.Value);
                            DateTimeOffset endTime = startTime.AddSeconds(program.duration);

                            var thisProgrammeNode = programmeNodes.
                                Where(line => line.Attributes["sd-programmeid"] != null && line.Attributes["sd-programmeid"].Value == program.programID
                                   && line.Attributes["start"].Value == xmlTV.DateToString(startTime)
                                   && line.Attributes["channel"].Value == schedule.stationID).FirstOrDefault();

                            if (thisProgrammeNode == null)
                            {
                                programmeIdList.Add(program.programID);

                                if (endTime != null)
                                {
                                    List<XmlAttribute> attribs = new List<XmlAttribute>();
                                    var atProgId = rootDoc.CreateAttribute("sd-programmeid");
                                    atProgId.Value = program.programID;
                                    attribs.Add(atProgId);

                                    var atMD5 = rootDoc.CreateAttribute("sd-md5");
                                    atMD5.Value = program.md5;
                                    attribs.Add(atMD5);

                                    xmlTV.AddProgramme(xmlTV.DateToString(startTime), xmlTV.DateToString(endTime),
                                                        GetChannelID(channelBlock.station, channelBlock.stationTranslation), null, null, null, null, attribs);
                                }
                                continue;
                            }

                            if (thisProgrammeNode.Attributes["sd-md5"].Value == program.md5)
                                continue;

                            programmeIdList.Add(program.programID);
                        }
                    }
                }
            }

            if (programmeIdList.Count > 0)
            {
                var programmeSchedule = new RescheduleQueue<string>();

                // First, schedule all programs 
                programmeSchedule.AddRange(DateTime.UtcNow, programmeIdList);

                while (programmeSchedule.Count > 0)
                {
                    // If nothing ready now, then wait until next delay time
                    if (!programmeSchedule.ItemsReady)
                    {
                        System.Threading.Thread.Sleep(programmeSchedule.DelayTime);
                        continue;
                    }

                    // Split list
                    var splitProgrammes = splitArray<string>(programmeSchedule.GetReadyItems().ToArray(), config.ScheduleRetrievalItems);
                    var masterProgrammeList = new List<SDProgramResponse>();

                    // Collect all responses into a master list
                    foreach (var programme in splitProgrammes)
                    {
                        var programmeResponse = sd.GetPrograms(programmeIdList.ToArray());

                        if (programmeResponse != null)
                            masterProgrammeList.AddRange(programmeResponse);
                    }

                    var programmeNodes = xmlTV.GetProgrammeNodes().Cast<XmlNode>();
                    var rootDoc = xmlTV.GetDocument();
                    foreach (var programme in masterProgrammeList)
                    {
                        // Remove this programme
                        programmeSchedule.RemoveItem(programme.programID);

                        // Handle re-queue of request
                        if (programme.code == SDErrors.PROGRAMID_QUEUED)
                        {
                            // Reschedule this one
                            programmeSchedule.AddItem(DateTime.UtcNow.AddSeconds(10), programme.programID);
                            continue;
                        }

                        if (programme.code != SDErrors.OK)
                        {
                            // Handle any errors
                            continue;
                        }

                        // Get all programme nodes for this programme ID (there can be multiple)
                        var thisProgrammeNodes = programmeNodes.
                            Where(line => line.Attributes["sd-programmeid"].Value == programme.programID);
                        if (thisProgrammeNodes == null)
                            continue;

                        // Update each matching node
                        foreach (var thisProgrammeNode in thisProgrammeNodes)
                        {
                            string lang = "en";

                            if (programme.descriptions != null && programme.descriptions.description1000 != null && programme.descriptions.description1000.FirstOrDefault().descriptionLanguage != null)
                                lang = fixLang(programme.descriptions.description1000.FirstOrDefault().descriptionLanguage);

                            if (programme.titles != null && programme.titles.FirstOrDefault().title120 != null)
                            {
                                XmlElement titleNode = rootDoc.CreateElement("title");
                                titleNode.SetAttribute("lang", lang);
                                titleNode.InnerText = programme.titles.FirstOrDefault().title120;
                                thisProgrammeNode.AppendChild(titleNode);
                            }

                            if (programme.episodeTitle150 != null)
                            {
                                XmlElement subtitleNode = rootDoc.CreateElement("sub-title");
                                subtitleNode.SetAttribute("lang", lang);
                                subtitleNode.InnerText = programme.episodeTitle150;
                                thisProgrammeNode.AppendChild(subtitleNode);
                            }

                            if (programme.descriptions != null && programme.descriptions.description1000 != null && programme.descriptions.description1000.FirstOrDefault().description != null)
                            {
                                XmlElement descriptionNode = rootDoc.CreateElement("desc");
                                descriptionNode.SetAttribute("lang", lang);
                                descriptionNode.InnerText = programme.descriptions.description1000.FirstOrDefault().description;
                                thisProgrammeNode.AppendChild(descriptionNode);
                            }

                            if (programme.genres != null)
                            {
                                foreach (var genre in programme.genres)
                                {
                                    XmlElement categoryNode = rootDoc.CreateElement("category");
                                    categoryNode.SetAttribute("lang", lang);
                                    categoryNode.InnerText = genre;
                                    thisProgrammeNode.AppendChild(categoryNode);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Return scheduleplus configured date range as enumerable
        private IEnumerable<string> dateRange
        {
            get
            {
                DateTime dateMin = DateTime.Today.Date;
                DateTime dateMax = dateMin.AddDays(config.ProgrammeRetrieveRangeDays);

                if (config.ProgrammeRetrieveYesterday)
                    dateMin = dateMin.AddDays(-1.0f);

                for (var thisDate = dateMin; thisDate <= dateMax; thisDate = thisDate.AddDays(1.0f))
                    yield return thisDate.ToString("yyyy-MM-dd");
            }
        }

        public void SaveXmlTV()
        {
            xmlTV.SaveXmlTV(config.XmlTVFileName);
        }

        private string fixLang(string lang)
        {
            if (lang == null)
                return "en";

            string newLang = lang;
            switch (lang)
            {
                case "en-GB":
                    newLang = "en";
                    break;
            }
            return newLang;
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

        // Template to create List of various array types with max size
        private IEnumerable<T[]> splitArray<T>(T[] items, int nSize)
        {
            var origList = items.ToList();
            var list = new List<T[]>();

            for (int i = 0; i < origList.Count; i += nSize)
                list.Add(origList.GetRange(i, Math.Min(nSize, origList.Count - i)).ToArray());

            return list;
        }
    }
}
