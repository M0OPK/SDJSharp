using System;
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

        public class StatusUpdate
        {
            public int CurrentChannel;
            public int TotalChannels;
            public int CurrentProgramme;
            public int TotalProgrammes;
            public string currentChannelID;
            public string currentChannelName;
            public string currentProgrammeID;
            public string currentProgrammeTitle;
            public string statusMessage;
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

            public int Count
            {
                get
                {
                    return queueItems.Count();
                }
            }

            public bool ItemsReady
            {
                get
                {
                    return (GetReadyItems().Count() > 0);
                }
            }

            public int DelayTime
            {
                get
                {
                    if (ItemsReady)
                        return 1;
                    else
                    {
                        var nextTime = queueItems.OrderBy(line => line.nextTry).FirstOrDefault().nextTry;
                        TimeSpan span = nextTime - DateTime.UtcNow;
                        return span.Milliseconds;
                    }
                }
            }

            public class RescheduleQueueItem<T>
            {
                public DateTime nextTry;
                public T itemId;

                public RescheduleQueueItem(DateTime nexttry, T itemid)
                {
                    nextTry = nexttry;
                    itemId = itemid;
                }
            }
        }
    }
}
