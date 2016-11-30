using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SchedulesDirect
{
    public static class SDErrors
    {
        public static int OK = 0;
        public static int INVALID_JSON = 1001;
        public static int DEFLATE_REQUIRED = 1002;
        public static int TOKEN_MISSING = 1004;
        public static int UNSUPPORTED_COMMAND = 2000;
        public static int REQUIRED_ACTION_MISSING = 2001;
        public static int REQUIRED_REQUEST_MISSING = 2002;
        public static int REQUIRED_PARAMETER_MISSING_COUNTRY = 2004;
        public static int REQUIRED_PARAMETER_MISSING_POSTALCODE = 2005;
        public static int REQUIRED_PARAMETER_MISSING_MSGID = 2006;
        public static int INVALID_PARAMETER_COUNTRY = 2050;
        public static int INVALID_PARAMETER_POSTALCODE = 2051;
        public static int INVALID_PARAMETER_FETCHTYPE = 2052;
        public static int DUPLICATE_LINEUP = 2100;
        public static int LINEUP_NOT_FOUND = 2101;
        public static int UNKNOWN_LINEUP = 2102;
        public static int INVALID_LINEUP_DELETE = 2103;
        public static int LINEUP_WRONG_FORMAT = 2104;
        public static int INVALID_LINEUP = 2105;
        public static int LINEUP_DELETED = 2106;
        public static int LINEUP_QUEUED = 2107;
        public static int INVALID_COUNTRY = 2108;
        public static int STATIONID_NOT_FOUND = 2200;
        public static int SERVICE_OFFLINE = 3000;
        public static int ACCOUNT_EXPIRED = 4001;
        public static int INVALID_HASH = 4002;
        public static int INVALID_USER = 4003;
        public static int ACCOUNT_LOCKOUT = 4004;
        public static int ACCOUNT_DISABLED = 4005;
        public static int TOKEN_EXPIRED = 4006;
        public static int MAX_LINEUP_CHANGES_REACHED = 4100;
        public static int MAX_LINEUPS = 4101;
        public static int NO_LINEUPS = 4102;
        public static int IMAGE_NOT_FOUND = 5000;
        public static int INVALID_PROGRAMID = 6000;
        public static int PROGRAMID_QUEUED = 6001;
        public static int SCHEDULE_NOT_FOUND = 7000;
        public static int INVALID_SCHEDULE_REQUEST = 7010;
        public static int SCHEDULE_RANGE_EXCEEDED = 7020;
        public static int SCHEDULE_NOT_IN_LINEUP = 7030;
        public static int SCHEDULE_QUEUED = 7100;
        public static int HCF = 9999;
    }

    /// <summary>
    /// Message contains login/hashed password to authenticate
    /// </summary>
    [DataContract]
    public class SDTokenRequest
    {
        [DataMember]
        public string username;
        [DataMember]
        public string password;

        public SDTokenRequest(string Username = "", string Password = "")
        {
            username = Username;
            password = Password;
        }
    }

    /// <summary>
    /// Message response, contains result of authentication request
    /// </summary>
    [DataContract]
    public class SDTokenResponse
    {
        [DataMember]
        public int code;
        [DataMember]
        public string message;
        [DataMember]
        public DateTime? datetime;
        [DataMember]
        public string token;
        [DataMember]
        public string response;
    }

    /// <summary>
    /// Response to Status command, contains information about account
    /// </summary>
    [DataContract]
    public class SDStatusResponse
    {
        [DataMember]
        public SDAccount account;
        [DataMember]
        public SDLineUps[] lineups;
        [DataMember]
        public DateTime? lastDataUpdate;
        [DataMember]
        public string[] notifications;
        [DataMember]
        public SDSystemStatus[] systemStatus;
        [DataMember]
        public string serverID;
        [DataMember]
        public DateTime? datetime;
        [DataMember]
        public int code;

        [DataContract]
        public class SDAccount
        {
            [DataMember]
            public string expires;
            [DataMember]
            public string[] messages;
            [DataMember]
            public int maxLineups;
        }

        [DataContract]
        public class SDLineUps
        {
            [DataMember]
            public string lineup;
            [DataMember]
            public DateTime? modified;
            [DataMember]
            public string uri;
            [DataMember]
            public bool isDeleted;
        }

        [DataContract]
        public class SDSystemStatus
        {
            [DataMember]
            public DateTime? date;
            [DataMember]
            public string status;
            [DataMember]
            public string message;
        }
    }

    /// <summary>
    /// Version response, contains information about specified client program
    /// </summary>
    [DataContract]
    public class SDVersionResponse
    {
        [DataMember]
        public string response;
        [DataMember]
        public int code;
        [DataMember]
        public string client;
        [DataMember]
        public string version;
        [DataMember]
        public string serverID;
        [DataMember]
        public DateTime? datetime;
        [DataMember]
        public string message;
    }

    /// <summary>
    /// Available response. Provide list of available services
    /// </summary>
    [DataContract]
    public class SDAvailableResponse
    {
        [DataMember]
        public string type;
        [DataMember]
        public string description;
        [DataMember]
        public string uri;
    }

    /// <summary>
    /// Countries response. Provides list of continents/countries for which service is available
    /// </summary>
    public class SDCountries
    {
        public List<Continent> continents;

        public SDCountries()
        {
            continents = new List<Continent>();
        }

        public class Continent
        {
            public string continentname;
            public List<Country> countries;

            public Continent()
            {
                countries = new List<Country>();
            }
        }

        public class Country
        {
            public string fullName;
            public string shortName;
            public string postalCodeExample;
            public string postalCode;
            public bool onePostalCode;
        }
    }

    /// <summary>
    /// Transmitter response. Provides a list of transmitters available.
    /// </summary>
    public class SDTransmitter
    {
        public string transmitterArea;
        public string transmitterID;
    }

    /// <summary>
    /// Headends response. Provides a list of headends and lineups
    /// </summary>
    [DataContract]
    public class SDHeadendsResponse
    {
        [DataMember]
        public string headend;
        [DataMember]
        public string transport;
        [DataMember]
        public string location;
        [DataMember]
        public SDLineup[] lineups;

        [DataContract]
        public class SDLineup
        {
            [DataMember]
            public string name;
            [DataMember]
            public string lineup;
            [DataMember]
            public string uri;
        }
    }

    /// <summary>
    /// Lineup response. Success/failure result for adding lineup
    /// </summary>
    [DataContract]
    public class SDAddRemoveLineupResponse
    {
        [DataMember]
        public string response;
        [DataMember]
        public int code;
        [DataMember]
        public string serverID;
        [DataMember]
        public string message;
        [DataMember]
        public string changesRemaining;
        [DataMember]
        public DateTime? datetime;
    }

    /// <summary>
    /// Lineups command response. Provides list of lineups available.
    /// </summary>
    [DataContract]
    public class SDLineupsResponse
    {
        [DataMember]
        public int code;
        [DataMember]
        public string serverID;
        [DataMember]
        public DateTime? datetime;
        [DataMember]
        public SDLineups[] lineups;

        [DataContract]
        public class SDLineups
        {
            [DataMember]
            public string lineup;
            [DataMember]
            public string name;
            [DataMember]
            public string transport;
            [DataMember]
            public string location;
            [DataMember]
            public string uri;
        }
    }

    /// <summary>
    /// Lineup response. Provides list of lineup maps, stations and metadata.
    /// </summary>
    [DataContract]
    public class SDGetLineupResponse
    {
        [DataMember]
        public SDLineupMap[] map;
        [DataMember]
        public SDLineupStation[] stations;
        [DataMember]
        public SDLineupMetadata metadata;

        [DataContract]
        public class SDLineupMap
        {
            [DataMember]
            public string stationID;
            [DataMember]
            public int uhfVhf;
            [DataMember]
            public int atscMajor;
            [DataMember]
            public int atscMinor;
            [DataMember]
            public string channel;
            [DataMember]
            public string providerCallsign;
            [DataMember]
            public string logicalChannelNumber;
            [DataMember]
            public string matchType;
            [DataMember]
            public UInt64 frequencyHz;
            [DataMember]
            public string polarization;
            [DataMember]
            public string deliverySystem;
            [DataMember]
            public string modulationSystem;
            [DataMember]
            public int symbolrate;
            [DataMember]
            public string fec;
            [DataMember]
            public string serviceID;
            [DataMember]
            public string networkID;
            [DataMember]
            public string transportID;
        }

        [DataContract]
        public class SDLineupStation
        {
            [DataMember]
            public string stationID;
            [DataMember]
            public string name;
            [DataMember]
            public string callsign;
            [DataMember]
            public string affiliate;
            [DataMember]
            public string[] broadcastLanguage;
            [DataMember]
            public string[] descriptionLanguage;
            [DataMember]
            public SDStationBroadcaster broadcaster;
            [DataMember]
            bool isCommercialFree;
            [DataMember]
            public SDStationLogo logo;

            [DataContract]
            public class SDStationBroadcaster
            {
                [DataMember]
                public string city;
                [DataMember]
                public string state;
                [DataMember]
                public string postalcode;
                [DataMember]
                public string country;
            }

            [DataContract]
            public class SDStationLogo
            {
                [DataMember]
                public string URL;
                [DataMember]
                public int height;
                [DataMember]
                public int width;
                [DataMember]
                public string md5;
            }
        }

        [DataContract]
        public class SDLineupMetadata
        {
            [DataMember]
            public string lineup;
            [DataMember]
            public DateTime? modified;
            [DataMember]
            public string transport;
            [DataMember]
            public string modulation;
        }
    }

    /// <summary>
    /// Program response. Provides details about requested programs.
    /// </summary>
    [DataContract]
    public class SDProgramResponse
    {
        [DataMember]
        public string programID;
        [DataMember]
        public int code;
        [DataMember]
        public string message;
        [DataMember]
        public SDProgramTitles[] titles;
        [DataMember]
        public SDProgramEventDetails eventDetails;
        [DataMember]
        public SDProgramDescriptions descriptions;
        [DataMember]
        public string originalAirDate;
        [DataMember]
        public string[] genres;
        [DataMember]
        public string episodeTitle150;
        [DataMember]
        public SDProgramMetadata[] metadata;
        [DataMember]
        public SDProgramPerson[] cast;
        [DataMember]
        public SDProgramPerson[] crew;
        [DataMember]
        public string showType;
        [DataMember]
        public bool hasImageArtwork;
        [DataMember]
        public string md5;

        [DataContract]
        public class SDProgramTitles
        {
            [DataMember]
            public string title120;
        }

        [DataContract]
        public class SDProgramEventDetails
        {
            [DataMember]
            public string subType;
        }

        [DataContract]
        public class SDProgramDescriptions
        {
            [DataMember]
            public SDProgramDescription100[] description1000;

            [DataContract]
            public class SDProgramDescription100
            {
                [DataMember]
                public string descriptionLanguage;
                [DataMember]
                public string description;
            }
        }

        [DataContract]
        public class SDProgramMetadata
        {
            [DataMember]
            public SDProgramMetadataGracenote Gracenote;

            [DataContract]
            public class SDProgramMetadataGracenote
            {
                [DataMember]
                public int season;
                [DataMember]
                public int episode;
            }
        }

        [DataContract]
        public class SDProgramPerson
        {
            [DataMember]
            public string personId;
            [DataMember]
            public string nameId;
            [DataMember]
            public string name;
            [DataMember]
            public string role;
            [DataMember]
            public string billingOrder;
        }
    }

    /// <summary>
    /// Description response. Provides extra descriptions for specified program
    /// </summary>
    [DataContract]
    public class SDDescriptionResponse
    {
        [DataMember]
        public string episodeID;
        [DataMember]
        public SDProgramDescription episodeDescription;

        public SDDescriptionResponse()
        {
            episodeDescription = new SDProgramDescription();
        }

        [DataContract]
        public class SDProgramDescription
        {
            [DataMember]
            public int code;
            [DataMember]
            public string description100;
            [DataMember]
            public string description1000;
        }
    }

    /// <summary>
    /// Schedule request. Provides information to server regarding required station ID and date ranges to include program data for.
    /// </summary>
    [DataContract]
    public class SDScheduleRequest
    {
        [DataMember]
        public string stationID;
        [DataMember]
        public string[] date;

        public SDScheduleRequest()
        {
            date = new string[2];
        }

        public SDScheduleRequest(string station, DateTime start, DateTime end)
        {
            stationID = station;
            string dateStart = start.ToString("yyyy-MM-dd");
            string dateEnd = end.ToString("yyyy-MM-dd");
            date = new string[] { dateStart, dateEnd };
        }
    }

    /// <summary>
    /// Schedule response. Provides data regarding program schedules for requested stations
    /// </summary>
    [DataContract]
    public class SDScheduleResponse
    {
        [DataMember]
        public string stationID;
        [DataMember]
        public SDScheduleProgram[] programs;
        [DataMember]
        public SDScheduleMetadata metadata;
        // Possible error fields
        [DataMember]
        public string serverID;
        [DataMember]
        public int code;
        [DataMember]
        public string response;
        [DataMember]
        public DateTime? retryTime;

        [DataContract]
        public class SDScheduleProgram
        {
            [DataMember]
            public string programID;
            [DataMember]
            public DateTime? airDateTime;
            [DataMember]
            public int duration;
            [DataMember]
            public string md5;
            [DataMember(Name="new")]
            public bool isNew;
            [DataMember]
            public bool cableInTheClassroom;
            [DataMember]
            public bool catchup;
            [DataMember]
            public bool continued;
            [DataMember]
            public bool educational;
            [DataMember]
            public bool joinedInProgress;
            [DataMember]
            public bool leftInProgress;
            [DataMember]
            public bool premiere;
            [DataMember]
            public bool programBreak;
            [DataMember]
            public bool repeat;
            [DataMember]
            public bool signed;
            [DataMember]
            public bool subjectToBlackout;
            [DataMember]
            public bool timeApproximate;
            [DataMember]
            public bool free;
            [DataMember]
            public string liveTapeDelay;
            [DataMember]
            public string isPremiereOrFinale;
            [DataMember]
            public string[] audioProperties;
            [DataMember]
            public string[] videoProperties;
            [DataMember]
            public SDScheduleRatings[] ratings;
            [DataMember]
            public SDScheduleMultipart multipart;

            [DataContract]
            public class SDScheduleRatings
            {
                [DataMember]
                public string body;
                [DataMember]
                public string code;
            }

            [DataContract]
            public class SDScheduleMultipart
            {
                [DataMember]
                public int partNumber;
                [DataMember]
                public int totalParts;
            }
        }

        [DataContract]
        public class SDScheduleMetadata
        {
            [DataMember]
            public int code;
            [DataMember]
            public DateTime? modified;
            [DataMember]
            public string md5;
            [DataMember]
            public string startDate;
        }
    }

    /// <summary>
    /// MD5 hash request structure. Contains stations and date ranges to request hash values for
    /// </summary>
    [DataContract]
    public class SDMD5Request
    {
        [DataMember]
        public string stationID;
        [DataMember]
        public string[] date;

        public SDMD5Request()
        {
            date = new string[2];
        }

        public SDMD5Request(string station, DateTime start, DateTime end)
        {
            stationID = station;
            string dateStart = start.ToString("yyyy-MM-dd");
            string dateEnd = end.ToString("yyyy-MM-dd");
            date = new string[] { dateStart, dateEnd };
        }

    }

    /// <summary>
    /// MD5 hash response structure. Generated from dynamic data.
    /// </summary>
    public class SDMD5Response
    {
        public string stationID;
        public SDMD5Day[] md5day;

        public class SDMD5Day
        {
            public string date;
            public SDMD5Data md5data;

            public class SDMD5Data
            {
                public int code;
                public string message;
                public DateTime? lastModified;
                public string md5;
            }

            public SDMD5Day()
            {
                md5data = new SDMD5Data();
            }
        }
    }

    /// <summary>
    /// Delete message response structure. Contains result and messages for this operation
    /// </summary>
    [DataContract]
    public class SDDeleteResponse
    {
        [DataMember]
        public int code;
        [DataMember]
        public string response;
        [DataMember]
        public string serverID;
        [DataMember]
        public string message;
        [DataMember]
        public DateTime? datetime;
    }

    [DataContract]
    public class SDStillRunningResponse
    {
        [DataMember]
        public int code;
        [DataMember]
        public string message;
        [DataMember]
        public string programID;
        [DataMember]
        public string response;
        [DataMember]
        public bool isComplete;
        [DataMember]
        public string serverID;
        [DataMember]
        public DateTime? datetime;
        [DataMember]
        public string eventStartDateTime;
        [DataMember]
        public SDStillRunningResult result;

        [DataContract]
        public class SDStillRunningResult
        {
            [DataMember]
            public SDStillRunningTeamInfo homeTeam;
            [DataMember]
            public SDStillRunningTeamInfo awayTeam;

            [DataContract]
            public class SDStillRunningTeamInfo
            {
                [DataMember]
                public string name;
                [DataMember]
                public string score;
            }
        }
    }

    [DataContract]
    public class SDProgramMetadataResponse
    {
        [DataMember]
        public string programID;
        [DataMember]
        public SDImageData[] data;
    }

    [DataContract]
    public class SDImageData
    {
        [DataMember]
        public string width;
        [DataMember]
        public string height;
        [DataMember]
        public string uri;
        [DataMember]
        public string size;
        [DataMember]
        public string aspect;
        [DataMember]
        public string category;
        [DataMember]
        public string text;
        [DataMember]
        public string primary;
        [DataMember]
        public string tier;
        [DataMember]
        public SDProgramImageCaption caption;

        [DataContract]
        public class SDProgramImageCaption
        {
            [DataMember]
            public string content;
            [DataMember]
            public string lang;
        }
    }

}
