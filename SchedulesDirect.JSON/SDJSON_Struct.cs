using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SchedulesDirect
{
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
            public string[] audioProperties;
            [DataMember]
            public string[] videoProperties;
        }
    }
}
