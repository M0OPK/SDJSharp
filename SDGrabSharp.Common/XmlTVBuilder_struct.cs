﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SchedulesDirect;
using XMLTV;

namespace SDGrabSharp.Common
{
    public partial class XmlTVBuilder
    {
        public class ChannelBlock
        {
            public string lineUp;
            public SDGetLineupResponse.SDLineupStation station;
            public Config.XmlTVTranslation stationTranslation;
            public XmlNode stationNode;
            public bool isNew;
        }

        private class SDRequestQueue
        {
            public readonly List<SDRequestQueueItem> items;
            private readonly Config config;

            public class SDRequestQueueItem : ICloneable
            {
                public RequestType sdRequestType;
                public int priority;
                public DateTime retryTimeUtc;
                public string stationContext;
                public IEnumerable<SDMD5Request> md5Request;
                public IEnumerable<SDScheduleRequest> scheduleRequest;
                public string[] programmeRequest;

                public object Clone()
                {
                    return MemberwiseClone();
                }

            }

            public enum RequestType
            {
                SDRequestMD5,
                SDRequestSchedule,
                SDRequestProgramme,
                SDReqeustNone
            }

            public SDRequestQueue(Config configObject)
            {
                items = new List<SDRequestQueueItem>();
                config = configObject;
            }

            public void AddRequest(IEnumerable<SDMD5Request> md5Request, string stationContext = null, 
                                   DateTime? retryTime = null, int priority = 5)
            {
                var splitRequest = splitArray<SDMD5Request>(md5Request.ToArray(), config.ScheduleRetrievalItems);
                foreach (var thisSplit in splitRequest)
                {
                    if (thisSplit.Length > 0)
                    {
                        var thisRequest = new SDRequestQueueItem
                        {
                            sdRequestType = RequestType.SDRequestMD5,
                            md5Request = thisSplit,
                            retryTimeUtc = retryTime ?? DateTime.UtcNow,
                            stationContext = stationContext,
                            priority = priority
                        };

                        items.Add(thisRequest);
                    }
                }
            }

            public void AddRequest(IEnumerable<SDScheduleRequest> scheduleRequest, string stationContext = null,
                                   DateTime? retryTime = null, int priority = 5)
            {
                var splitRequest = splitArray<SDScheduleRequest>(scheduleRequest.ToArray(), config.ScheduleRetrievalItems);
                foreach (var thisSplit in splitRequest)
                {
                    if (thisSplit.Length > 0)
                    {
                        var thisRequest = new SDRequestQueueItem
                        {
                            sdRequestType = RequestType.SDRequestSchedule,
                            scheduleRequest = thisSplit,
                            retryTimeUtc = retryTime ?? DateTime.UtcNow,
                            stationContext = stationContext,
                            priority = priority
                        };

                        items.Add(thisRequest);
                    }
                }
            }

            public void AddRequest(string[] programRequest, string stationContext = null,
                                   DateTime? retryTime = null, int priority = 5)
            {
                var splitRequest = splitArray<string>(programRequest, config.ProgrammeRetrievalItems);
                foreach (var thisSplit in splitRequest)
                {
                    if (thisSplit.Length > 0)
                    {
                        var thisRequest = new SDRequestQueueItem
                        {
                            sdRequestType = RequestType.SDRequestProgramme,
                            programmeRequest = thisSplit,
                            retryTimeUtc = retryTime ?? DateTime.UtcNow,
                            stationContext = stationContext,
                            priority = priority
                        };

                        items.Add(thisRequest);
                    }
                }
            }

            private IEnumerable<T[]> splitArray<T>(T[] items, int nSize)
            {
                var origList = items.ToList();
                var list = new List<T[]>();

                for (var i = 0; i < origList.Count; i += nSize)
                    list.Add(origList.GetRange(i, Math.Min(nSize, origList.Count - i)).ToArray());

                return list;
            }
        }

        private class SDResponseQueue
        {
            public readonly List<SDResponseQueueItem> items;

            public class SDResponseQueueItem : ICloneable
            {
                public ResponseType sdResponseType;
                public string stationContext;
                public IEnumerable<MD5ResultPair> md5Response;
                public IEnumerable<ScheduleResultPair> scheduleResponse;
                public IEnumerable<ProgrammeResultPair> programmeResponse;
                public object Clone()
                {
                    return MemberwiseClone();
                }
            }

            public enum ResponseType
            {
                SDResponseMD5,
                SDResponseSchedule,
                SDResponseProgramme
            }
            public class MD5ResultPair
            {
                public SDMD5Request md5Request;
                public readonly SDMD5Response md5Response;

                public MD5ResultPair(SDMD5Request request, SDMD5Response response)
                {
                    md5Request = request;
                    md5Response = response;
                }
            }

            public class ScheduleResultPair
            {
                public readonly SDScheduleRequest scheduleRequest;
                public readonly SDScheduleResponse scheduleResponse;

                public ScheduleResultPair(SDScheduleRequest request, SDScheduleResponse response)
                {
                    scheduleRequest = request;
                    scheduleResponse = response;
                }
            }

            public class ProgrammeResultPair
            {
                public readonly string programmeRequest;
                public readonly SDProgrammeResponse programmeResponse;
                public readonly bool isCached;

                public ProgrammeResultPair(string request, SDProgrammeResponse response, bool cached = false)
                {
                    programmeRequest = request;
                    programmeResponse = response;
                    isCached = cached;
                }
            }

            public SDResponseQueue()
            {
                items = new List<SDResponseQueueItem>();
            }

            public void AddResponse(IEnumerable<MD5ResultPair> md5Response, string stationContext = null)
            {
                var thisRequest = new SDResponseQueueItem
                {
                    md5Response = md5Response,
                    sdResponseType = ResponseType.SDResponseMD5,
                    stationContext = stationContext
                };

                items.Add(thisRequest);
            }

            public void AddResponse(IEnumerable<ScheduleResultPair> schedule5Response, string stationContext = null)
            {
                var thisRequest = new SDResponseQueueItem
                {
                    scheduleResponse = schedule5Response,
                    sdResponseType = ResponseType.SDResponseSchedule,
                    stationContext = stationContext
                };
                items.Add(thisRequest);
            }
            public void AddResponse(IEnumerable<ProgrammeResultPair> programmeResponse, string stationContext = null)
            {
                var thisRequest = new SDResponseQueueItem
                {
                    programmeResponse = programmeResponse,
                    sdResponseType = ResponseType.SDResponseProgramme,
                    stationContext = stationContext
                };
                items.Add(thisRequest);
            }
        }

        public class RescheduleQueue<T>
        {
            public List<RescheduleQueueItem<T>> queueItems;

            public RescheduleQueue()
            {
                queueItems = new List<RescheduleQueueItem<T>>();
            }

            public void AddItem(DateTime nexttry, T itemid)
            {
                queueItems.Add(new RescheduleQueueItem<T>(nexttry, itemid));
            }

            public void AddRange(DateTime nexttry, IEnumerable<T> items)
            {
                foreach (var item in items)
                    AddItem(nexttry, item);
            }

            public void RemoveItem(T itemid)
            {
                var removeItems = queueItems.Where(line => line.itemId.Equals(itemid)).ToArray();

                foreach (var removeItem in removeItems)
                    queueItems.Remove(removeItem);
            }

            public IEnumerable<T> GetReadyItems()
            {
                return queueItems.Where(line => line.nextTry <= DateTime.UtcNow).AsEnumerable().OrderBy(line => line.nextTry).Select(line => line.itemId);
            }

            public int Count => queueItems.Count;

            public bool ItemsReady => GetReadyItems().Any();

            public int DelayTime
            {
                get
                {
                    if (ItemsReady)
                        return 1;
                    else
                    {
                        var nextTime = queueItems.OrderBy(line => line.nextTry).FirstOrDefault().nextTry;
                        var span = nextTime - DateTime.UtcNow;
                        return span.Milliseconds;
                    }
                }
            }

            public class RescheduleQueueItem<S>
            {
                public DateTime nextTry;
                public S itemId;

                public RescheduleQueueItem(DateTime nexttry, S itemid)
                {
                    nextTry = nexttry;
                    itemId = itemid;
                }
            }
        }
        public class ActivityLogEventArgs : EventArgs
        {
            public string ActivityText;
        }

        public class StatusUpdateArgs : EventArgs
        {
            public int progressValue;
            public int progressMax;
            public string currentChannelID;
            public string currentChannelName;
            public string currentProgrammeID;
            public string currentProgrammeTitle;
            public string statusMessage;
        }
    }

}
