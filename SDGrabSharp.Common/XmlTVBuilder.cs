using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Globalization;
using SchedulesDirect;
using XMLTV;

namespace SDGrabSharp.Common
{
    public partial class XmlTVBuilder
    {
        // Event handlers/wait handles
        public event EventHandler<StatusUpdateArgs> StatusUpdate;
        public event EventHandler<ActivityLogEventArgs> ActivityLogUpdate;
        static EventWaitHandle evSDRequestReady;
        static EventWaitHandle evSDResponseReady;

        // Multi-thread object locks
        private static ReaderWriterLockSlim reqLock;
        private static ReaderWriterLockSlim respLock;

        // Multi thread objects
        private SDRequestQueue requestQueue;
        private SDResponseQueue responseQueue;

        // Schedules direct request thread
        Thread sdRequestThread;

        // Current operation type (used to show what's being worked on
        // for the very short time there's no request or response)
        private SDRequestQueue.RequestType currentRequestOperation;

        // Stop request signal
        static bool _requestStop;

        // Objects
        private Config config;
        private DataCache cache;
        private XmlTV xmlTV;
        SDJson sd;

        // Local keys for Xml Nodes
        Dictionary<string, XmlNode> channelByStationID;
        Dictionary<string, List<XmlNode>> programmeNodesByProgrammeID;

        // Programme object lookup by programme id
        Dictionary<string, SDProgrammeResponse> programmeItemByProgrammeID;

        // SDStation lookup by station id
        Dictionary<string, SDGetLineupResponse.SDLineupStation> SDStationLookup;

        public XmlTVBuilder(Config inputConfig, DataCache inputCache, SDJson sdJs = null)
        {
            config = inputConfig;
            cache = inputCache;

            // If were supplied an instance, use it, else try to make one with the token in cache data if found, 
            // else no token and we'll login later
            sd = sdJs ?? new SDJson(cache.tokenData != null ? cache.tokenData.token : string.Empty);

            // Prime event signals
            evSDRequestReady = new AutoResetEvent(false);
            evSDResponseReady = new AutoResetEvent(false);

            // Create locks
            reqLock = new ReaderWriterLockSlim();
            respLock = new ReaderWriterLockSlim();

            _requestStop = false;
        }

        public void StartThreads()
        {
            sdRequestThread = new Thread(() => SDCommandHandlerThread());
            sdRequestThread.Start();
        }

        public void StopThreads()
        {
            _requestStop = true;
            sdRequestThread.Join();
        }

        public bool LoadXmlTV(string filename)
        {
            xmlTV = new XmlTV(null, "SDGrabSharp", "https://github.com/M0OPK/SDJSharp", "SchedulesDirect");

            if (System.IO.File.Exists(filename))
            {
                if (xmlTV.LoadXmlTV(filename))
                {
                    // If there was a file loaded, update local keys with actual values
                    updateKeys();
                    return true;
                }
            }
            return false;
        }

        public bool mergeXmlTV(string filename)
        {
            return xmlTV.LoadXmlTV(filename, true);
        }

        public IEnumerable<XmlTV.XMLTVError> GetXmlTVErrors()
        {
            return xmlTV.GetRawErrors();
        }

        private bool checkHasItems(SDRequestQueue.RequestType requestType, SDResponseQueue.ResponseType responseType, bool readyItems = false)
        {
            // Check if queue has items (used in where loops) with appropriate reader locking
            if (readyItems)
            {
                bool responseQueueReadyItems = false;
                try
                {
                    respLock.EnterReadLock();
                    responseQueueReadyItems = (responseQueue.items.
                        Where(line => line.sdResponseType == responseType).FirstOrDefault() != null);
                }
                finally
                {
                    respLock.ExitReadLock();
                }
                return responseQueueReadyItems;
            }
            else
            {
                bool hasRequestItem = false;
                try
                {
                    reqLock.EnterReadLock();
                    hasRequestItem = (requestQueue.items.Where(line => line.sdRequestType == requestType).FirstOrDefault() != null);
                }
                finally
                {
                    reqLock.ExitReadLock();
                }

                // If we found something, don't bother with a response lock
                if (hasRequestItem)
                    return true;

                bool hasResponseItem = false;
                try
                {
                    respLock.EnterReadLock();
                    hasResponseItem = (responseQueue.items.Where(line => line.sdResponseType == responseType).FirstOrDefault() != null);
                }
                finally
                {
                    respLock.ExitReadLock();
                }
                if (hasResponseItem)
                    return true;

                return false;
            }
        }

        private void preloadMD5Requests(List<SDScheduleRequest> scheduleRequestList, List<SDMD5Request> md5RequestList, 
                                        ref int md5RequestsSent, ref int scheduleRequestsSent)
        {
            // Process MD5 requests per channel
            foreach (var channel in channelByStationID)
            {
                // Check channel has valid attributes
                var channelNode = channel.Value;
                if (channelNode.Attributes["id"] == null)
                    continue;

                ActivityLog(string.Format("Working on channel {0}", channel.Value.Attributes["id"].Value));

                string stationId = channel.Key;

                // First see what MD5 values we have for this channel
                var channelMD5 = channelMD5List(stationId);

                // Find out which MD5 dates we already have
                List<DateTime> md5Dates = new List<DateTime>();
                List<DateTime> md5ScheduleDates = new List<DateTime>();
                foreach (var date in dateRange)
                {
                    var thisMD5 = channelMD5.
                        Where(line => line.Split(',').First() == date).
                        Select(line => line.Split(',').First()).FirstOrDefault();
                    if (thisMD5 == null)
                        md5ScheduleDates.Add(DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    else
                        md5Dates.Add(DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                }

                // Add any we don't have direct to schedule queue
                if (md5ScheduleDates.Count > 0)
                    scheduleRequestList.Add(new SDScheduleRequest(stationId, md5ScheduleDates.ToArray()));

                if (md5Dates.Count > 0)
                    md5RequestList.Add(new SDMD5Request(stationId, md5Dates.ToArray()));

                // Queue the MD5 list for this channel
            }

            // Queue all the md5 requests, and the initial batch of schedule requests.
            try
            {
                reqLock.EnterWriteLock();
                requestQueue.AddRequest(md5RequestList);
                requestQueue.AddRequest(scheduleRequestList);
            }
            finally
            {
                reqLock.ExitWriteLock();
            }
            md5RequestsSent += md5RequestList.Count();
            scheduleRequestsSent += scheduleRequestList.Count();

            md5RequestList.Clear();
            scheduleRequestList.Clear();
            evSDRequestReady.Set();
        }

        private void processMD5Responses(List<SDScheduleRequest> scheduleRequestList, int md5RequestsSent, 
                                         ref int md5Count, ref int scheduleRequestsSent)
        {
            // Keep processing MD5 responses while there are queued requests 
            while (checkHasItems(SDRequestQueue.RequestType.SDRequestMD5, SDResponseQueue.ResponseType.SDResponseMD5, false)
                || currentRequestOperation == SDRequestQueue.RequestType.SDRequestMD5)
            {
                // Wait to be triggered (scan anyway every second)
                evSDResponseReady.WaitOne(1000);

                // Process all MD5 responses
                while (checkHasItems(SDRequestQueue.RequestType.SDRequestMD5, SDResponseQueue.ResponseType.SDResponseMD5, true))
                {
                    IEnumerable<SDResponseQueue.MD5ResultPair> result = null;

                    // pop top item
                    try
                    {
                        respLock.EnterUpgradeableReadLock();
                        var tempResult = responseQueue.items.
                            Where(line => line.sdResponseType == SDResponseQueue.ResponseType.SDResponseMD5).
                            FirstOrDefault();
                        result = tempResult.md5Response;
                        try
                        {
                            respLock.EnterWriteLock();
                            responseQueue.items.Remove(tempResult);
                        }
                        finally
                        {
                            respLock.ExitWriteLock();
                        }
                    }
                    finally
                    {
                        respLock.ExitUpgradeableReadLock();
                    }

                    if (result == null)
                        continue;

                    // Process each response in this queue item
                    foreach (var thisResponse in result)
                    {
                        if (thisResponse.md5Response.md5day != null)
                        {
                            md5Count++;
                            SendStatusUpdate(null, Math.Min(md5Count, md5RequestsSent), md5RequestsSent);

                            // Check each MD5 against XML result
                            var thisChanDate = new List<DateTime>();
                            foreach (var thisMD5 in thisResponse.md5Response.md5day)
                            {
                                // If error, or MD5 doesn't match, queue this date
                                var md5Value = channelMD5Value(thisResponse.md5Response.stationID, thisMD5.date);
                                if (thisMD5.md5data.code != SDErrors.OK || md5Value != thisMD5.md5data.md5)
                                    thisChanDate.Add(XmlTV.StringToDate(thisMD5.date).LocalDateTime);
                            }

                            // If there are any dates, add this request to the schedule request list
                            if (thisChanDate.Count() != 0)
                            {
                                var thisScheduleRequest = new SDScheduleRequest(thisResponse.md5Response.stationID, thisChanDate.ToArray());
                                scheduleRequestList.Add(thisScheduleRequest);
                            }
                        }
                    }

                    // If the request list isn't empty, add these requests to the queue
                    if (scheduleRequestList.Count() != 0)
                    {
                        try
                        {
                            reqLock.EnterWriteLock();
                            requestQueue.AddRequest(scheduleRequestList);
                        }
                        finally
                        {
                            reqLock.ExitWriteLock();
                        }

                        scheduleRequestsSent += scheduleRequestList.Count();
                        scheduleRequestList.Clear();
                        evSDRequestReady.Set();
                    }
                }
            }
            ActivityLog(string.Format("Processed {0} MD5 responses", md5Count.ToString()));
        }

        private void processScheduleResponses(List<string> programmeRequestList, int scheduleRequestsSent,
                                              ref int scheduleCount, ref int programmeCount, ref int programmeRequestsSent)
        {
            // Keep processing Schedule responses while there are queued requests 
            while (checkHasItems(SDRequestQueue.RequestType.SDRequestSchedule, SDResponseQueue.ResponseType.SDResponseSchedule, false)
                || currentRequestOperation == SDRequestQueue.RequestType.SDRequestSchedule)
            {
                // Wait to be triggered (1 second max)
                evSDResponseReady.WaitOne(1000);

                // Process all MD5 responses
                while (checkHasItems(SDRequestQueue.RequestType.SDRequestSchedule, SDResponseQueue.ResponseType.SDResponseSchedule, true))
                {
                    IEnumerable<SDResponseQueue.ScheduleResultPair> result = null;

                    // pop top item
                    try
                    {
                        respLock.EnterUpgradeableReadLock();
                        var tempResult = responseQueue.items.
                            Where(line => line.sdResponseType == SDResponseQueue.ResponseType.SDResponseSchedule).
                            FirstOrDefault();
                        result = tempResult.scheduleResponse;
                        try
                        {
                            respLock.EnterWriteLock();
                            responseQueue.items.Remove(tempResult);
                        }
                        finally
                        {
                            respLock.ExitWriteLock();
                        }
                    }
                    finally
                    {
                        respLock.ExitUpgradeableReadLock();
                    }

                    if (result == null)
                        continue;

                    // Process each response in this queue item
                    foreach (var thisResponse in result.Where(line => line.scheduleResponse.code == SDErrors.OK))
                    {
                        scheduleCount++;
                        SendStatusUpdate(null, Math.Min(scheduleCount, scheduleRequestsSent), scheduleRequestsSent);
                        // First update/add any MD5 dates for this schedule
                        channelUpdateAddMD5(thisResponse.scheduleResponse.stationID,
                                            thisResponse.scheduleResponse.metadata.startDate,
                                            thisResponse.scheduleResponse.metadata.md5);

                        // Process programmes in this schedule
                        foreach (var programme in thisResponse.scheduleResponse.programs)
                        {
                            programmeCount++;
                            DateTimeOffset startTime = new DateTimeOffset(programme.airDateTime.Value);
                            DateTimeOffset endTime = startTime.AddSeconds(programme.duration);

                            var existProgrammeMD5 = programmeMD5Value(programme.programID);

                            // Check if this programme was already received (it's possible the same program is on
                            // multiple channels)
                            if (programmeItemByProgrammeID.ContainsKey(programme.programID))
                            {
                                // Add programme direct and no need to queue it
                                var thisProgramme = programmeItemByProgrammeID[programme.programID];

                                string title = thisProgramme.titles.FirstOrDefault().title120;
                                string titleLang = null;
                                string description = null;
                                string descLang = null;
                                string subtitle = null;
                                string subtitleLang = null;
                                var categories = new List<XmlTV.XmlLangText>();

                                if (thisProgramme.descriptions != null && thisProgramme.descriptions.description1000 != null
                                 && thisProgramme.descriptions.description1000.FirstOrDefault() != null
                                 && thisProgramme.descriptions.description1000.FirstOrDefault().description != null)
                                {
                                    description = thisProgramme.descriptions.description1000.FirstOrDefault().description;
                                    descLang = fixLang(thisProgramme.descriptions.description1000.FirstOrDefault().descriptionLanguage);
                                    // @Todo: Preferred language
                                    titleLang = descLang;
                                    subtitleLang = descLang;
                                }

                                if (thisProgramme.episodeTitle150 != null)
                                    subtitle = thisProgramme.episodeTitle150;

                                if (thisProgramme.genres != null)
                                {
                                    foreach (var genre in thisProgramme.genres)
                                        categories.Add(new XmlTV.XmlLangText(descLang ?? "en", genre));
                                }

                                addProgramme(startTime, endTime, GetChannelID(thisResponse.scheduleResponse.stationID),
                                             titleLang, title, subtitleLang, subtitle, descLang, description, categories.ToArray(),
                                             thisProgramme.programID, thisProgramme.md5);
                            }
                            else
                            {
                                // Update existing node/Add new skeleton node
                                addProgramme(startTime, endTime, GetChannelID(thisResponse.scheduleResponse.stationID),
                                             null, null, null, null, null, null, null, programme.programID, programme.md5);

                                // Queue programme for retrieval
                                if (existProgrammeMD5 == null || existProgrammeMD5 != programme.md5)
                                    programmeRequestList.Add(programme.programID);
                            }
                        }

                        // Queue all distinct programme id's for this station
                        try
                        {
                            reqLock.EnterWriteLock();
                            requestQueue.AddRequest(programmeRequestList.Distinct().ToArray(),
                                                    thisResponse.scheduleResponse.stationID);
                        }
                        finally
                        {
                            reqLock.ExitWriteLock();
                        }
                        programmeRequestsSent += programmeRequestList.Count();
                        programmeRequestList.Clear();
                    }
                }
            }

            ActivityLog(string.Format("Processed {0} schedules with {1} programmes", scheduleCount.ToString(), programmeCount.ToString()));
        }

        private void processProgrammeResponses(int programRequestsSent, ref int programmeCount)
        {
            // Keep processing programme responses while there are queued requests 
            while (checkHasItems(SDRequestQueue.RequestType.SDRequestProgramme, SDResponseQueue.ResponseType.SDResponseProgramme, false)
                || currentRequestOperation == SDRequestQueue.RequestType.SDRequestProgramme)
            {
                // Wait to be triggered (1 second max)
                evSDResponseReady.WaitOne(1000);

                // Process all programme responses
                while (checkHasItems(SDRequestQueue.RequestType.SDRequestProgramme, SDResponseQueue.ResponseType.SDResponseProgramme, true))
                {
                    IEnumerable<SDResponseQueue.ProgrammeResultPair> result = null;

                    // pop top item
                    try
                    {
                        respLock.EnterUpgradeableReadLock();
                        var tempResult = responseQueue.items.
                            Where(line => line.sdResponseType == SDResponseQueue.ResponseType.SDResponseProgramme).FirstOrDefault();
                        result = tempResult.programmeResponse;
                        try
                        {
                            respLock.EnterWriteLock();
                            responseQueue.items.Remove(tempResult);
                        }
                        finally
                        {
                            respLock.ExitWriteLock();
                        }
                    }
                    finally
                    {
                        respLock.ExitUpgradeableReadLock();
                    }

                    if (result == null)
                        continue;

                    // Process each response in this queue item
                    foreach (var thisResponse in result.Where(line => line.programmeResponse.code == SDErrors.OK))
                    {
                        programmeCount++;
                        SendStatusUpdate(null, Math.Min(programmeCount, programRequestsSent), programRequestsSent);

                        // Add this programme to the local cache
                        if (!programmeItemByProgrammeID.ContainsKey(thisResponse.programmeResponse.programID))
                            programmeItemByProgrammeID.Add(thisResponse.programmeResponse.programID, thisResponse.programmeResponse);

                        var thisProgramme = thisResponse.programmeResponse;

                        string title = thisProgramme.titles.FirstOrDefault().title120;
                        string titleLang = null;
                        string description = null;
                        string descLang = null;
                        string subtitle = null;
                        string subtitleLang = null;
                        var categories = new List<XmlTV.XmlLangText>();

                        if (thisProgramme.descriptions != null && thisProgramme.descriptions.description1000 != null
                         && thisProgramme.descriptions.description1000.FirstOrDefault() != null
                         && thisProgramme.descriptions.description1000.FirstOrDefault().description != null)
                        {
                            description = thisProgramme.descriptions.description1000.FirstOrDefault().description;
                            // @Todo: Preferred language
                            descLang = fixLang(thisProgramme.descriptions.description1000.FirstOrDefault().descriptionLanguage);
                            titleLang = descLang;
                            subtitleLang = descLang;
                        }

                        if (thisProgramme.episodeTitle150 != null)
                            subtitle = thisProgramme.episodeTitle150;

                        if (thisProgramme.genres != null)
                        {
                            foreach (var genre in thisProgramme.genres)
                                categories.Add(new XmlTV.XmlLangText(descLang ?? "en", genre));
                        }

                        updateAllProgrammes(thisProgramme.programID, titleLang, title, subtitleLang, subtitle,
                                            descLang, description, categories.ToArray(), thisProgramme.md5);
                    }
                }
            }
            ActivityLog(string.Format("Processed {0} programme responses", programmeCount.ToString()));
        }

        private void validateCorrectProgrammes()
        {
            // Get all programmes with no title
            var emptyProgrammes = xmlTV.GetProgrammeNodes().
                Where(line => line.SelectNodes("title").Count == 0 && line.Attributes["sd-programmeid"] != null).
                Select(line => line.Attributes["sd-programmeid"].Value).Distinct().ToArray();

            if (emptyProgrammes.Count() == 0)
                return;

            // Request programmes
            requestQueue.AddRequest(emptyProgrammes);

            int dummy = 0;

            // Process responses
            processProgrammeResponses(0, ref dummy);
        }

        public void Init()
        {
            // Lookups for programmes/stations
            channelByStationID = new Dictionary<string, XmlNode>();
            programmeNodesByProgrammeID = new Dictionary<string, List<XmlNode>>();
            SDStationLookup = new Dictionary<string, SDGetLineupResponse.SDLineupStation>();
            programmeItemByProgrammeID = new Dictionary<string, SDProgrammeResponse>();
        }

        public void RunProcess()
        {
            ActivityLog("Starting");

            // Ensure this instance is logged in
            if (!sd.LoggedIn)
                sd.Login(config.SDUsername, config.SDPasswordHash, true);

            ActivityLog("Logged in");

            // Initialize queues
            requestQueue = new SDRequestQueue(config);
            responseQueue = new SDResponseQueue();

            // Begin SD thread
            StartThreads();

            // Create lookup for SDStations
            foreach (var lineup in config.TranslationMatrix.Select(line => line.Value.LineupID).Distinct())
            {
                var sdLineupStations = sd.GetLineup(lineup, true);
                if (sdLineupStations != null)
                {
                    foreach (var sdStation in sdLineupStations.stations)
                    {
                        // Sometimes the same channel is in multiple lineups
                        if (!SDStationLookup.ContainsKey(sdStation.stationID))
                            SDStationLookup.Add(sdStation.stationID, sdStation);
                    }
                }
            }
            ActivityLog("Station lookup created");
            SendStatusUpdate("Loading existing XML");

            ActivityLog("Existing XML loaded");

            SendStatusUpdate("Adding/updating channel nodes");
            // Add/update channel nodes
            doChannels();

            // Get date range from config
            DateTime dateMin = DateTime.Today.Date;
            DateTime dateMax = dateMin.AddDays(config.ProgrammeRetrieveRangeDays);

            if (config.ProgrammeRetrieveYesterday)
                dateMin = dateMin.AddDays(-1.0f);

            // Remove programmes outside of range
            deleteProgrammesOutsideDateRange(dateMin, dateMax);

            ActivityLog("Added channel nodes");

            // Request lists (used by the queuing process)
            List<SDScheduleRequest> scheduleRequestList = new List<SDScheduleRequest>();
            List<SDMD5Request> md5RequestList = new List<SDMD5Request>();
            List<string> programmeRequestList = new List<string>();

            // Counters for requests sent (used by progress reports for UI)
            int md5RequestsSent = 0;
            int scheduleRequestsSent = 0;
            int programmeRequestsSent = 0;

            SendStatusUpdate("Checking/Updating MD5 values");
            ActivityLog("Processing MD5 responses");

            // counters for the responses (for status update purposes)
            int md5Count = 0;
            int scheduleCount = 0;
            int programmeCount = 0;

            // Load MD5 requests
            preloadMD5Requests(scheduleRequestList, md5RequestList, ref md5RequestsSent, ref scheduleRequestsSent);

            // Handle MD5 responses
            processMD5Responses(scheduleRequestList, md5RequestsSent, ref md5Count, ref scheduleRequestsSent);

            ActivityLog("Processing schedule responses");

            SendStatusUpdate("Updating/Adding schedule data");

            // Handle Schedule Responses
            processScheduleResponses(programmeRequestList, scheduleRequestsSent, ref scheduleCount, 
                                     ref programmeCount, ref programmeRequestsSent);

            ActivityLog("Processing programme responses");

            // reset programme counter
            programmeCount = 0;

            SendStatusUpdate("Updating/Adding programme items");

            // Handle Programme Responses
            processProgrammeResponses(programmeRequestsSent, ref programmeCount);

            SendStatusUpdate("Checking/Correcting empty programmes");
            validateCorrectProgrammes();

            SendStatusUpdate("Cleaning up");
            ActivityLog("Finished all activities, stopping threads");
            StopThreads();
        }

        private void updateKeys()
        {
            foreach (var channel in xmlTV.GetChannelNodes())
            {
                if (channel.Attributes["sd-stationid"] != null)
                    channelByStationID.Add(channel.Attributes["sd-stationid"].Value, channel);

                if (channel.Attributes["id"] == null)
                    continue;

                foreach (var programme in xmlTV.GetProgrammeNodes(channel.Attributes["id"].Value))
                {
                    if (programme.Attributes["sd-programmeid"] != null)
                    {
                        if (!programmeNodesByProgrammeID.ContainsKey(programme.Attributes["sd-programmeid"].Value))
                        {
                            var nodeList = new List<XmlNode>();
                            nodeList.Add(programme);
                            programmeNodesByProgrammeID.Add(programme.Attributes["sd-programmeid"].Value, nodeList);
                        }
                        else
                        {
                            programmeNodesByProgrammeID[programme.Attributes["sd-programmeid"].Value].Add(programme);
                        }
                    }
                }
            }
        }

        private void doChannels()
        {
            // Get unique list of lineups from translation matrix (e.g. ones we're interested in)
            var lineupList = config.TranslationMatrix.Select(line => line.Value.LineupID).Distinct();

            List<ChannelBlock> channelDataset = new List<ChannelBlock>();

            // Get date range from config
            DateTime dateMin = DateTime.Today.Date;
            DateTime dateMax = dateMin.AddDays(config.ProgrammeRetrieveRangeDays);

            foreach (var lineup in lineupList)
            {
                var lineupData = cache.GetLineupData(sd, lineup);
                if (lineupData == null)
                    continue;

                if (config.ProgrammeRetrieveYesterday)
                    dateMin = dateMin.AddDays(-1.0f);

                var channelDatasetLineup =
                    (
                        from translate in config.TranslationMatrix
                        join station in lineupData.stations
                            on translate.Key equals station.stationID
                        where station != null
                        select new ChannelBlock()
                        {
                            station = station,
                            lineUp = lineup,
                            stationTranslation = translate.Value
                        }
                    );

                channelDataset.AddRange(channelDatasetLineup);
            }

            // Rename any channels that might have changed ID/Display name
            renameChannelNodes(channelDataset);

            // Remove channels from xml set not fond in current configuration
            deleteUnmatchingChannelNodes(channelDataset);

            // Remove MD5 values outside current range
            channelRemoveMD5OutsideDateRange(dateMin, dateMax);

            foreach (var channel in channelDataset)
                addChannel(channel.station.stationID, null, null, null, channel.station.logo != null ? channel.station.logo.URL : null);
        }

        private void SDCommandHandlerThread()
        {
            currentRequestOperation = SDRequestQueue.RequestType.SDReqeustNone;
            while (!_requestStop)
            {
                // Wait for signal
                evSDRequestReady.WaitOne(1000);

                // If it's time to finish, then exit
                if (_requestStop)
                    return;

                while (requestQueue.items.Count > 0)
                {
                    // Process One queue item
                    SDRequestQueue.SDRequestQueueItem thisItem;
                    try
                    {
                        reqLock.EnterUpgradeableReadLock();
                        var thisOrigItem = requestQueue.items.
                            Where(line => line.retryTimeUtc <= DateTime.UtcNow).
                            OrderBy(line => line.priority).ThenBy(line => line.retryTimeUtc).FirstOrDefault();

                        // If we found nothing, then probably a delayed entry exists.
                        // Delay for a short time and try again
                        if (thisOrigItem == null)
                        {
                            Thread.Sleep(500);
                            continue;
                        }

                        thisItem = (SDRequestQueue.SDRequestQueueItem)thisOrigItem.Clone();
                        currentRequestOperation = thisItem.sdRequestType;
                        try
                        {
                            reqLock.EnterWriteLock();
                            requestQueue.items.Remove(thisOrigItem);
                        }
                        finally
                        {
                            reqLock.ExitWriteLock();
                        }
                    }
                    finally
                    {
                        reqLock.ExitUpgradeableReadLock();
                    }

                    if (thisItem.sdRequestType == SDRequestQueue.RequestType.SDRequestMD5)
                    {
                        var response = sd.GetMD5(thisItem.md5Request);
                        if (response != null)
                        {
                            // Create joined request/response list
                            var resultList =
                                (
                                    from thisResponse in response
                                    join origRequest in thisItem.md5Request
                                        on thisResponse.stationID equals origRequest.stationID
                                    where thisResponse.md5day.Any(any => origRequest.date.Contains(any.date))
                                    select new SDResponseQueue.MD5ResultPair(origRequest, thisResponse)
                                );

                            try
                            {
                                respLock.EnterWriteLock();
                                responseQueue.AddResponse(resultList, thisItem.stationContext);
                            }
                            finally
                            {
                                respLock.ExitWriteLock();
                            }
                            evSDResponseReady.Set();
                        }
                    }
                    else if (thisItem.sdRequestType == SDRequestQueue.RequestType.SDRequestSchedule)
                    {
                        var response = sd.GetSchedules(thisItem.scheduleRequest);
                        if (response != null)
                        {
                            // Process errors (NYI)
                            /*var errorList =
                                (
                                    from thisResponse in response
                                    join origRequest in thisItem.scheduleRequest
                                        on thisResponse.stationID equals origRequest.stationID
                                    where thisResponse.code != SDErrors.OK
                                    select new SDResponseQueue.ScheduleResultPair(origRequest, thisResponse)
                                );*/

                            // Create joined request/response list
                            var resultList =
                                (
                                    from thisResponse in response
                                    join origRequest in thisItem.scheduleRequest
                                        on thisResponse.stationID equals origRequest.stationID
                                    where thisResponse.code == SDErrors.OK && origRequest.date.Contains(thisResponse.metadata.startDate)
                                    select new SDResponseQueue.ScheduleResultPair(origRequest, thisResponse)
                                );

                            // Auto requeue retry nodes
                            var retryResponse = resultList.Where(line => line.scheduleResponse.code == SDErrors.SCHEDULE_QUEUED);
                            if (retryResponse == null || retryResponse.Count() == 0)
                            {
                                // New list, set retry time to now
                                List<SDScheduleRequest> retryList = new List<SDScheduleRequest>();
                                DateTime retryTime = DateTime.UtcNow;

                                foreach (var result in retryResponse)
                                {
                                    // Convert dates to datetime
                                    List<DateTime> originalDates = new List<DateTime>();
                                    foreach (var thisDate in result.scheduleRequest.date)
                                        originalDates.Add(XmlTV.StringToDate(thisDate).UtcDateTime);

                                    // Create new request, add to list
                                    var thisRequest = new SDScheduleRequest(result.scheduleResponse.stationID, originalDates.ToArray());
                                    retryList.Add(thisRequest);

                                    // If this retrytime is later than current, update current
                                    if (result.scheduleResponse.retryTime.HasValue && result.scheduleResponse.retryTime.Value > retryTime)
                                        retryTime = result.scheduleResponse.retryTime.Value;
                                }

                                // Requeue any retries found with the longest retrytime encountered
                                if (retryList.Count() > 0)
                                {
                                    try
                                    {
                                        reqLock.EnterWriteLock();
                                        requestQueue.AddRequest(retryList, thisItem.stationContext, retryTime);
                                    }
                                    finally
                                    {
                                        reqLock.ExitWriteLock();
                                    }
                                }
                            }

                            try
                            {
                                respLock.EnterWriteLock();
                                responseQueue.AddResponse(resultList, thisItem.stationContext);
                            }
                            finally
                            {
                                respLock.ExitWriteLock();
                            }
                            evSDResponseReady.Set();

                        }
                    }
                    else if (thisItem.sdRequestType == SDRequestQueue.RequestType.SDRequestProgramme)
                    {
                        var response = sd.GetProgrammes(thisItem.programmeRequest);
                        if (response != null)
                        {
                            // Create joined request/response list
                            var resultList =
                                (
                                    from thisResponse in response
                                    join origRequest in thisItem.programmeRequest
                                        on thisResponse.programID equals origRequest
                                    where thisResponse.code == SDErrors.OK
                                    select new SDResponseQueue.ProgrammeResultPair(origRequest, thisResponse)
                                );

                            // Auto requeue retry nodes
                            var retryResponse = resultList.Where(line => line.programmeResponse.code == SDErrors.PROGRAMID_QUEUED);
                            if (retryResponse == null || retryResponse.Count() == 0)
                            {
                                // New list, set retry time to now
                                List<string> retryList = new List<string>();
                                DateTime retryTime = DateTime.UtcNow;

                                foreach (var result in retryResponse)
                                {
                                    // Convert dates to datetime
                                    List<DateTime> originalDates = new List<DateTime>();

                                    // Create new request, add to list
                                    var thisRequest = result.programmeResponse.programID;
                                    retryList.Add(thisRequest);

                                    // If this retrytime is later than current, update current
                                    retryTime = DateTime.UtcNow.AddSeconds(10);
                                }

                                // Requeue any retries found with the longest retrytime encountered
                                if (retryList.Count() > 0)
                                {
                                    try
                                    {
                                        reqLock.EnterWriteLock();
                                        requestQueue.AddRequest(retryList.ToArray(), thisItem.stationContext, retryTime);
                                    }
                                    finally
                                    {
                                        reqLock.ExitWriteLock();
                                    }
                                }
                            }

                            try
                            {
                                respLock.EnterWriteLock();
                                responseQueue.AddResponse(resultList, thisItem.stationContext);
                            }
                            finally
                            {
                                respLock.ExitWriteLock();
                            }
                            evSDResponseReady.Set();
                        }
                    }
                    currentRequestOperation = SDRequestQueue.RequestType.SDReqeustNone;
                }
            }
        }

        private IEnumerable<XmlNode> FindProgrammeNodesByID(string sdProgrammeId)
        {
            if (!programmeNodesByProgrammeID.ContainsKey(sdProgrammeId))
                return null;

            return programmeNodesByProgrammeID[sdProgrammeId].AsEnumerable();
        }

        private XmlNode FindChannelByStationID(string sdStationID)
        {
            if (!channelByStationID.ContainsKey(sdStationID))
                return null;

            return channelByStationID[sdStationID];
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

        // Wrapper to xmltv class function of same name
        // Update internal keys also
        private bool addChannel(string stationId, string displayName = null, string displayLang = null, string url = null, string iconUrl = null,
                           string channelId = null)
        {
            // If no channelId supplied, find it via station ID
            if (!config.TranslationMatrix.ContainsKey(stationId))
                return false;

            if (!SDStationLookup.ContainsKey(stationId))
                return false;

            // Fetch translation and station data
            var thisTranslation = config.TranslationMatrix[stationId];
            var sdStation = SDStationLookup[stationId];

            if (channelId == null)
                channelId = GetChannelID(sdStation, thisTranslation);

            if (displayName == null)
                displayName = GetChannelName(sdStation, thisTranslation);

            var displayNameItem = new XmlTV.XmlLangText[] { new XmlTV.XmlLangText(displayLang ?? "en", displayName) };

            // Generate SD ID attribute
            XmlAttribute sdChannelIDAttrib = xmlTV.GetDocument().CreateAttribute("sd-stationid");
            sdChannelIDAttrib.Value = stationId;

            var channelNode = xmlTV.AddChannel(channelId, displayNameItem, url, iconUrl, new XmlAttribute[] { sdChannelIDAttrib });
            if (channelNode == null)
                return false;

            channelByStationID.Add(stationId, channelNode);
            return true;
        }

        private void removeChannel(string stationId, string channelId = null)
        {
            // If we have nothing, there's nothing to do
            if (stationId == null && channelId == null)
                return;

            // If stationId is null, fetch from channelId
            if (stationId == null)
            {
                var channelNode = xmlTV.GetChannel(channelId);
                if (channelNode != null && channelNode.Attributes["sd-station"] != null)
                    stationId = channelNode.Attributes["sd-station"].Value;
            }

            if (channelId == null)
            {
                if (channelByStationID.ContainsKey(stationId))
                {
                    var channelNode = channelByStationID[stationId];
                    if (channelNode != null && channelNode.Attributes["id"] != null)
                        channelId = channelNode.Attributes["id"].Value;
                }
            }

            // Remove from local list
            if (stationId != null && channelByStationID.ContainsKey(stationId))
                channelByStationID.Remove(stationId);

            if (channelId != null)
                xmlTV.DeleteChannelNode(channelId);
        }

        private void deleteUnmatchingChannelNodes(IEnumerable<ChannelBlock> fullChannelList)
        {
            var removed = xmlTV.DeleteUnmatchingChannelNodes(fullChannelList.Select(line =>
                    GetChannelID(line.station, line.stationTranslation)).Distinct().ToArray());

            foreach(var thisRemoved in removed)
            {
                var thisStationIdNode = channelByStationID.
                    Where(line => line.Value.Attributes["id"] != null
                       && line.Value.Attributes["id"].Value == thisRemoved).
                       Select(line => line.Value).FirstOrDefault();

                if (thisStationIdNode != null && thisStationIdNode.Attributes["sd-stationid"] != null)
                    channelByStationID.Remove(thisStationIdNode.Attributes["sd-stationid"].Value);
            }
        }

        private void renameChannelNodes(IEnumerable<ChannelBlock> fullChannelList)
        {
            // Rename channels if the station ID matches, but id/name doesn't

            // Phase 1 - rename to station ID
            foreach (var blockItem in fullChannelList)
            {
                if (!channelByStationID.ContainsKey(blockItem.station.stationID))
                    continue;

                // Check/rename channel IDs
                var channelNode = channelByStationID[blockItem.station.stationID];
                if (channelNode.Attributes["id"] != null 
                 && channelNode.Attributes["id"].Value != GetChannelID(blockItem.station.stationID))
                {
                    // First check for renamed nodes
                    //xmlTV.renameChannel(string.Format("{0}_rename", channelNode.Attributes["id"].Value), GetChannelID(blockItem.station.stationID));

                    // And try normal
                    //xmlTV.renameChannel(channelNode.Attributes["id"].Value, GetChannelID(blockItem.station.stationID));
                    xmlTV.renameChannel(channelNode.Attributes["id"].Value, blockItem.station.stationID);
                }
            }

            // Phase 2 - rename to station ID
            foreach (var blockItem in fullChannelList)
            {
                if (!channelByStationID.ContainsKey(blockItem.station.stationID))
                    continue;

                // Check/rename channel IDs
                var channelNode = channelByStationID[blockItem.station.stationID];
                if (channelNode.Attributes["id"] != null
                 && channelNode.Attributes["id"].Value == blockItem.station.stationID)
                {
                    xmlTV.renameChannel(blockItem.station.stationID, GetChannelID(blockItem.station.stationID));
                }

                // Check/rename display name
                var displayNode = channelNode.SelectNodes("display-name").Cast<XmlNode>().FirstOrDefault();
                if (displayNode != null && displayNode.InnerText != GetChannelName(blockItem.station.stationID))
                    displayNode.InnerText = GetChannelName(blockItem.station.stationID);
            }

        }

        private string[] channelMD5List(string stationId)
        {
            if (!channelByStationID.ContainsKey(stationId))
                return null;

            var thisNode = channelByStationID[stationId];
            if (thisNode == null)
                return null;

            List<string> listMD5 = new List<string>();

            var md5Nodes = thisNode.SelectNodes("sd-md5");
            if (md5Nodes == null)
                return null;

            foreach(XmlNode md5Node in md5Nodes)
            {
                string md5Item = string.Format("{0},{1}", md5Node.Attributes["date"].Value, md5Node.InnerText);
                listMD5.Add(md5Item);
            }

            return listMD5.ToArray();
        }

        private string channelMD5Value(string stationId, string date)
        {
            var md5Result = channelMD5List(stationId);
            if (md5Result == null)
                return null;

            return md5Result.
                Where(line => line.Split(',').First() == date).
                Select(line => line.Split(',').Last()).FirstOrDefault();
        }

        private void channelUpdateAddMD5(string stationId, string date, string md5)
        {
            if (!channelByStationID.ContainsKey(stationId))
                return;

            var thisNode = channelByStationID[stationId];
            if (thisNode == null)
                return;

            var md5Nodes = thisNode.SelectNodes("sd-md5");
            if (md5Nodes == null)
                return;

            var md5Node = md5Nodes.Cast<XmlNode>().
                Where(node => node.Attributes["date"] != null && node.Attributes["date"].Value == date).FirstOrDefault();

            if (md5Node != null)
                md5Node.InnerText = md5;
            else
            {
                XmlElement newMD5 = xmlTV.GetDocument().CreateElement("sd-md5");
                newMD5.SetAttribute("date", date);
                newMD5.InnerText = md5;
                thisNode.AppendChild(newMD5);
            }
        }

        private void channelRemoveMD5(string stationId, string date)
        {
            if (!channelByStationID.ContainsKey(stationId))
                return;

            var thisNode = channelByStationID[stationId];
            if (thisNode == null)
                return;

            var md5Nodes = thisNode.SelectNodes("sd-md5");
            if (md5Nodes == null)
                return;

            var md5Node = md5Nodes.Cast<XmlNode>().
                Where(node => node.Attributes["date"] != null && node.Attributes["date"].Value == date).FirstOrDefault();

            if (md5Node != null)
                thisNode.RemoveChild(md5Node);
        }

        private void channelRemoveMD5OutsideDateRange(DateTime minDate, DateTime maxDate, string stationId)
        {
            if (!channelByStationID.ContainsKey(stationId))
                return;

            var thisNode = channelByStationID[stationId];
            if (thisNode == null)
                return;

            var md5Nodes = thisNode.SelectNodes("sd-md5");
            if (md5Nodes == null)
                return;

            var md5SelectedNodes = md5Nodes.Cast<XmlNode>().
                Where(line => line.Attributes["date"] != null &&
                    (XmlTV.StringToDate(line.Attributes["date"].Value, true).Date < minDate.Date ||
                     XmlTV.StringToDate(line.Attributes["date"].Value, true).Date > maxDate.Date)).ToArray();

            foreach (var md5Node in md5SelectedNodes)
                thisNode.RemoveChild(md5Node);
        }

        private void channelRemoveMD5OutsideDateRange(DateTime minDate, DateTime maxDate)
        {
            foreach (var channel in channelByStationID)
                channelRemoveMD5OutsideDateRange(minDate, maxDate, channel.Key);
        }

        private bool addProgramme(DateTimeOffset start, DateTimeOffset stop, string channel, 
                                  string titleLang = "en", string title = null,
                                  string subtitleLang = "en", string subtitle = null, 
                                  string descriptionLang = "en", string description = null,
                                  XmlTV.XmlLangText[] categories = null, string sdProgrammeId = null, string sdMD5 = null)
        {
            string startString = XmlTV.DateToString(start);
            string stopString = XmlTV.DateToString(stop);
            var programmeNode = (XmlElement)xmlTV.FindFirstProgramme(startString, channel);

            if (programmeNode != null)
            {
                // Update existing node
                programmeNode.SetAttribute("stop", stopString);
                programmeNode.RemoveAttribute("sd-md5");
                programmeNode.RemoveAttribute("sd-programmeid");
                deleteSubNodes(programmeNode, "title");
                deleteSubNodes(programmeNode, "sub-title");
                deleteSubNodes(programmeNode, "desc");
                deleteSubNodes(programmeNode, "category");

                if (sdMD5 != null)
                    programmeNode.SetAttribute("sd-md5", sdMD5);

                if (sdProgrammeId != null)
                    programmeNode.SetAttribute("sd-programmeid", sdProgrammeId);

                if (title != null)
                {
                    XmlElement titleNode = xmlTV.GetDocument().CreateElement("title");
                    titleNode.SetAttribute("lang", titleLang);
                    titleNode.InnerText = title;
                    programmeNode.AppendChild(titleNode);
                }

                if (subtitle != null)
                {
                    XmlElement subtitleNode = xmlTV.GetDocument().CreateElement("sub-title");
                    subtitleNode.SetAttribute("lang", subtitleLang);
                    subtitleNode.InnerText = subtitle;
                    programmeNode.AppendChild(subtitleNode);
                }

                if (description != null)
                {
                    XmlElement descriptionNode = xmlTV.GetDocument().CreateElement("desc");
                    descriptionNode.SetAttribute("lang", descriptionLang);
                    descriptionNode.InnerText = description;
                    programmeNode.AppendChild(descriptionNode);
                }

                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        XmlElement categoryNode = xmlTV.GetDocument().CreateElement("category");
                        categoryNode.SetAttribute("lang", category.lang);
                        categoryNode.InnerText = category.text;
                        programmeNode.AppendChild(categoryNode);
                    }
                }

                // Update local key if not already present
                if (sdProgrammeId != null)
                {
                    // Add new if none already present
                    if (!programmeNodesByProgrammeID.ContainsKey(sdProgrammeId))
                        programmeNodesByProgrammeID.Add(sdProgrammeId, new List<XmlNode>());

                    // Find existing node
                    var existNode = programmeNodesByProgrammeID[sdProgrammeId].
                        Where(line => line.Attributes["start"] != null && line.Attributes["channel"] != null
                           && line.Attributes["start"].Value == startString
                           && line.Attributes["channel"].Value == channel).FirstOrDefault();

                    // If none, add this one
                    if (existNode == null)
                        programmeNodesByProgrammeID[sdProgrammeId].Add(programmeNode);
                }
                return true;
            }

            // Create new programme node
            XmlTV.XmlLangText titleText = null;
            if (title != null)
                titleText = new XmlTV.XmlLangText(titleLang, title);

            XmlTV.XmlLangText subtitleText = null;
            if (subtitle != null)
                subtitleText = new XmlTV.XmlLangText(subtitleLang, subtitle);

            XmlTV.XmlLangText descriptionText = null;
            if (description != null)
                descriptionText = new XmlTV.XmlLangText(descriptionLang, description);

            List<XmlAttribute> attributes = new List<XmlAttribute>();
            XmlAttribute programmeIdAttrib = null;
            XmlAttribute md5Attrib = null;

            // Create attributes for programme id/MD5 hash
            if (sdProgrammeId != null)
            {
                programmeIdAttrib = xmlTV.GetDocument().CreateAttribute("sd-programmeid");
                programmeIdAttrib.Value = sdProgrammeId;
                attributes.Add(programmeIdAttrib);
            }

            if (sdMD5 != null)
            {
                md5Attrib = xmlTV.GetDocument().CreateAttribute("sd-md5");
                md5Attrib.Value = sdMD5;
                attributes.Add(md5Attrib);
            }

            var newProgrammeNode = xmlTV.AddProgramme(startString, stopString, channel, titleText, subtitleText, 
                                                   descriptionText, categories, attributes);

            if (newProgrammeNode == null)
                return false;

            if (sdProgrammeId == null)
                return true;

            // Add entry to programme nodes
            if (!programmeNodesByProgrammeID.ContainsKey(sdProgrammeId))
                programmeNodesByProgrammeID.Add(sdProgrammeId, new List<XmlNode>());

            programmeNodesByProgrammeID[sdProgrammeId].Add(newProgrammeNode);
            return true;
        }

        private void deleteProgrammesOutsideDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            // Delete from local store too
            var outOfRangeNodes = xmlTV.GetProgrammesOutsideDateRange(startDate, endDate);
            foreach (var outOfRangeNode in outOfRangeNodes)
            {
                if (outOfRangeNode.Attributes["sd-programmeid"] == null)
                    continue;

                if (!programmeNodesByProgrammeID.ContainsKey(outOfRangeNode.Attributes["sd-programmeid"].Value))
                    return;

                var programmeEntry = programmeNodesByProgrammeID[outOfRangeNode.Attributes["sd-programmeid"].Value];
                if (programmeEntry.Contains(outOfRangeNode))
                    programmeEntry.Remove(outOfRangeNode);
            }

            xmlTV.DeleteProgrammesOutsideDateRange(startDate, endDate);
        }

        private bool deleteProgrammeNodeByTimeExact(DateTimeOffset start, string channel)
        {
            string startString = XmlTV.DateToString(start);
            var programmeNode = xmlTV.FindFirstProgramme(startString, channel);

            if (programmeNode == null)
                return false;

            if (programmeNode.Attributes["sd-programmeid"] != null 
             && programmeNodesByProgrammeID.ContainsKey(programmeNode.Attributes["sd-programmeid"].Value))
            {
                programmeNodesByProgrammeID.Remove(programmeNode.Attributes["sd-programmeid"].Value);
            }

            return xmlTV.DeleteProgrammeNodeByTimeExact(startString, channel);
        }

        private string programmeMD5Value(string programId)
        {
            if (!programmeNodesByProgrammeID.ContainsKey(programId))
                return null;

            var programmeNode = programmeNodesByProgrammeID[programId].FirstOrDefault();

            if (programmeNode != null && programmeNode.Attributes["sd-md5"] != null)
                return programmeNode.Attributes["sd-md5"].Value;

            return null;
        }

        private void updateAllProgrammes(string sdProgrammeId = null,
                                         string titleLang = "en", string title = null,
                                         string subtitleLang = "en", string subtitle = null,
                                         string descriptionLang = "en", string description = null,
                                         XmlTV.XmlLangText[] categories = null, string sdMD5 = null)
        {
            if (!programmeNodesByProgrammeID.ContainsKey(sdProgrammeId))
                return;

            var programmeNodes = programmeNodesByProgrammeID[sdProgrammeId];

            foreach (var programmeNode in programmeNodes.ToArray())
            {
                if (programmeNode.Attributes["start"] == null || programmeNode.Attributes["stop"] == null
                 || programmeNode.Attributes["channel"] == null)
                    continue;

                // Get channel, start/end times
                DateTimeOffset start = XmlTV.StringToDate(programmeNode.Attributes["start"].Value);
                DateTimeOffset stop = XmlTV.StringToDate(programmeNode.Attributes["stop"].Value);
                string channel = programmeNode.Attributes["channel"].Value;

                // Update program
                addProgramme(start, stop, channel, titleLang, title, subtitleLang, subtitle, descriptionLang,
                             description, categories, sdProgrammeId, sdMD5);
            }
        }

        private void deleteSubNodes(XmlNode parentNode, string childNodeKey)
        {
            var childNodes = parentNode.SelectNodes(childNodeKey).Cast<XmlElement>().ToArray();
            foreach (var node in childNodes)
                parentNode.RemoveChild(node);
        }

        public void SaveXmlTV()
        {
            xmlTV.SaveXmlTV(config.XmlTVFileName);
        }


        private string fixLang(string lang)
        {
            // @Todo: Better way to do this, resources file maybe?
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

        public string GetChannelID(string stationId)
        {
            if (!SDStationLookup.ContainsKey(stationId) || !config.TranslationMatrix.ContainsKey(stationId))
                return null;

            var sdStation = SDStationLookup[stationId];
            var translateStation = config.TranslationMatrix[stationId];
            return GetChannelID(sdStation, translateStation);
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

        public string GetChannelName(string stationId)
        {
            if (!SDStationLookup.ContainsKey(stationId) || !config.TranslationMatrix.ContainsKey(stationId))
                return null;

            var sdStation = SDStationLookup[stationId];
            var translateStation = config.TranslationMatrix[stationId];
            return GetChannelName(sdStation, translateStation);
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

        private void ActivityLog(string text)
        {
            EventHandler<ActivityLogEventArgs> logHandler = ActivityLogUpdate;
            var args = new ActivityLogEventArgs();
            args.ActivityText = string.Format("{0}: {1}", DateTime.Now.ToString("HH:mm:ss.ffffff"), text);
            if (logHandler != null)
                logHandler.Invoke(this, args);
        }

        private void SendStatusUpdate(string statusMessage = null, int progressMin = -1, int progressMax = -1,
                                      string currenChannelID = null,  string currentChannelName = null,
                                      string currentProgrammeID = null, string currentProgrammeName = null)
        {
            var args = new StatusUpdateArgs();
            args.progressValue = progressMin;
            args.progressMax = progressMax;
            args.statusMessage = statusMessage;
            args.currentChannelID = currenChannelID;
            args.currentChannelName = currentChannelName;
            args.currentProgrammeID = currentProgrammeID;
            args.currentProgrammeTitle = currentProgrammeName;
            EventHandler<StatusUpdateArgs> statusHandler = StatusUpdate;
            if (statusHandler != null)
                statusHandler.Invoke(this, args);
        }
    }
}
