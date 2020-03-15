using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Net;
using System.Web.Script.Serialization;
using System.Globalization;

namespace SchedulesDirect
{
    public partial class SDJson
    {
#if DEBUG
        private static readonly string DEBUG_FILE = "SDGrabSharp.debug.txt";
#endif
        private string loginToken;
        private List<SDJsonError> localErrors;
        private static string urlBase = "https://json.schedulesdirect.org/20141201/";
        private static string userAgentDefault = "SDJSharp JSON C# Library/1.0 (https://github.com/M0OPK/SDJSharp)";
        private static string userAgentShort = "SDJSharp JSON C# Library/1.0";
        private static string userAgentFull;

        public SDJson(string clientUserAgent = "", string token = "")
        {
            localErrors = new List<SDJsonError>();
            userAgentFull = (clientUserAgent == string.Empty) ? userAgentDefault : string.Format("{0} ({1})", userAgentShort, clientUserAgent);

            // If token supplied, use it
            if (token != string.Empty)
                loginToken = token;
        }

        /// <summary>
        /// Return raw error details (if any)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SDJsonError> GetRawErrors()
        {
            return localErrors.AsEnumerable();
        }

        /// <summary>
        /// Return true if there are errors to report
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return (localErrors.Count > 0);
            }
        }

        /// <summary>
        /// Retrieve the most recent reported error. If specified pop (remove) after returning
        /// </summary>
        /// <param name="pop"></param>
        /// <returns></returns>
        public SDJsonError GetLastError(bool pop = true)
        {
            var thisError = localErrors.Last();
            if (pop)
                localErrors.Remove(thisError);
            return thisError;
        }

        /// <summary>
        /// Clear any errors
        /// </summary>
        public void ClearErrors()
        {
            localErrors.Clear();
        }

        private void addError(Exception ex)
        {
            localErrors.Add(new SDJsonError(ex));
        }

        private void addError(int errorcode, string errormessage, SDJsonError.ErrorSeverity errorseverity = SDJsonError.ErrorSeverity.Error, string errordescription = "", string errorsource = "")
        {
            localErrors.Add(new SDJsonError(errorcode, errormessage, errorseverity, errordescription, errorsource));
        }

        /// <summary>
        /// Schedules Direct Error Structure
        /// </summary>
        public class SDJsonError
        {
            public Exception exception;
            public bool isException;
            public int code;
            public string message;
            public string description;
            public string source;
            public ErrorSeverity severity;

            public enum ErrorSeverity
            {
                Info,
                Warning,
                Error,
                Fatal
            }

            public SDJsonError(Exception ex)
            {
                isException = true;
                exception = ex;
                code = ex.HResult;
                message = ex.Message;
                description = ex.StackTrace;
                source = ex.Source;
                severity = ErrorSeverity.Fatal;
            }

            public SDJsonError(int errorcode, string errormessage, ErrorSeverity errorseverity = ErrorSeverity.Error, string errordescription = "", string errorsource = "")
            {
                isException = false;
                exception = null;
                code = errorcode;
                message = errormessage;
                description = errordescription;
                source = errorsource;
                severity = errorseverity;
            }
        }

        /// <summary>
        /// Provide password hash in correct format for ScheduleDirect login
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string hashPassword(string password)
        {
            byte[] passBytes = Encoding.UTF8.GetBytes(password);
            SHA1 hash = SHA1.Create();
            byte[] hashBytes = hash.ComputeHash(passBytes);

            string hexString = BitConverter.ToString(hashBytes);
            return hexString.Replace("-", "").ToLower();
        }

        public bool LoggedIn
        {
            get { return loginToken != null && loginToken != string.Empty; }
        }

        /// <summary>
        /// Log in, and retrieve login token for session
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public SDTokenResponse Login(string username, string password, bool isHash = false)
        {
            try
            {
                string passHash = isHash ? password : hashPassword(password);
                SDTokenResponse response = PostJSON<SDTokenResponse, SDTokenRequest>("token", new SDTokenRequest(username, passHash));
                if (response == null || response.code != 0)
                    return null;

                loginToken = response.token;
                return response;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Get system status, and account lineups (Requires login token)
        /// </summary>
        /// <returns></returns>
        public SDStatusResponse GetStatus()
        {
            try
            {
                if (loginToken == string.Empty)
                    return null;

                return GetJSON<SDStatusResponse>("status", loginToken);
            }
            catch(Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Returns version information for specified client name
        /// </summary>
        /// <param name="clientname"></param>
        /// <returns></returns>
        public SDVersionResponse GetVersion(string clientname)
        {
            try
            {
                if (clientname == string.Empty)
                    return null;

                return GetJSON<SDVersionResponse>("token/" + clientname);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Returns list of available services
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SDAvailableResponse> GetAvailable()
        {
            try
            {
                return GetJSON<IEnumerable<SDAvailableResponse>>("available");
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Returns list of countries supported.
        /// </summary>
        /// <returns></returns>
        public SDCountries GetCountries()
        {
            try
            {
                dynamic result = GetDynamic(WebGet("available/countries"));
                if (result == null)
                    return null;

                SDCountries countries = new SDCountries();
                foreach (string key in result.Keys)
                {
                    SDCountries.Continent thisContinent = new SDCountries.Continent();
                    thisContinent.continentname = key;

                    foreach (dynamic country in result[key])
                    {
                        if (country == null)
                            continue;

                        SDCountries.Country thisCountry = new SDCountries.Country();
                        try { thisCountry.fullName = country["fullName"]; } catch { }
                        try { thisCountry.shortName = country["shortName"]; } catch { }
                        try { thisCountry.postalCodeExample = country["postalCodeExample"]; } catch { }
                        try { thisCountry.postalCode = country["postalCode"]; } catch { }
                        try { thisCountry.onePostalCode = country["onePostalCode"]; } catch { }

                        thisContinent.countries.Add(thisCountry);
                    }
                    countries.continents.Add(thisContinent);
                }
                return countries;
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Get list of transmitters for specified country
        /// </summary>
        /// <param name="countrycode"></param>
        /// <returns></returns>
        public IEnumerable<SDTransmitter> GetTransmitters(string countrycode)
        {
            try
            {
                dynamic result = GetDynamic(WebGet("transmitters/" + countrycode));
                if (result == null)
                    return null;

                List<SDTransmitter> txList = new List<SDTransmitter>();
                foreach (string key in result.Keys)
                {
                    SDTransmitter thisTx = new SDTransmitter();
                    thisTx.transmitterArea = key;
                    thisTx.transmitterID = result[key];
                    txList.Add(thisTx);
                }

                return txList.AsEnumerable();
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Retrieves the list of headends for the specified country and postcode (Requires login token)
        /// </summary>
        /// <param name="country"></param>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public IEnumerable<SDHeadendsResponse> GetHeadends(string country, string postcode)
        {
            try
            {
                return GetJSON<IEnumerable<SDHeadendsResponse>>(string.Format("headends?country={0}&postalcode={1}", country, postcode), loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Adds specified lineup to this account (Requires login token)
        /// </summary>
        /// <param name="lineupID"></param>
        /// <returns></returns>
        public SDAddRemoveLineupResponse AddLineup(string lineupID)
        {
            try
            {
                return PutJSON<SDAddRemoveLineupResponse>("lineups/" + lineupID, loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        public SDAddRemoveLineupResponse DeleteLineup(string lineupID)
        {
            try
            {
                return DeleteJSON<SDAddRemoveLineupResponse>("lineups/" + lineupID, loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Return lineups for this account (Requires login token)
        /// </summary>
        /// <returns></returns>
        public SDLineupsResponse GetLineups()
        {
            try
            {
                return GetJSON<SDLineupsResponse>("lineups", loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Return map, stations and metadata for the specified lineup
        /// </summary>
        /// <param name="lineup"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public SDGetLineupResponse GetLineup(string lineup, bool verbose = false)
        {
            try
            {
                WebHeaderCollection headers = null;

                if (verbose)
                {
                    headers = new WebHeaderCollection();
                    headers.Add("verboseMap: true");
                }

                return GetJSON<SDGetLineupResponse>("lineups/" + lineup, loginToken, headers);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        public IEnumerable<SDPreviewLineupResponse> GetLineupPreview(string lineup)
        {
            try
            {
                return GetJSON<IEnumerable<SDPreviewLineupResponse>>("lineups/preview/" + lineup, loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Return programme information for the specified list of programmes
        /// </summary>
        /// <param name="programmes"></param>
        /// <returns></returns>
        public IEnumerable<SDProgrammeResponse> GetProgrammes(string[] programmes)
        {
            try
            {
                return PostJSON<IEnumerable<SDProgrammeResponse>, string[]>("programs", programmes, loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Return program descriptions for the specified list of programmes
        /// </summary>
        /// <param name="programmes"></param>
        /// <returns></returns>
        public IEnumerable<SDDescriptionResponse> GetDescriptions(string[] programmes)
        {
            try
            {
                dynamic result = GetDynamic(WebPost("metadata/description", CreateJSONstring<string[]>(programmes), loginToken));

                if (result == null)
                    return null;

                var programmeData = new List<SDDescriptionResponse>();
                foreach (string key in result.Keys)
                {
                    var thisProgramme = new SDDescriptionResponse();
                    thisProgramme.episodeID = key;
                    dynamic temp = result[key];
                    try { thisProgramme.episodeDescription.code = temp["code"]; } catch { };
                    try { thisProgramme.episodeDescription.description100 = temp["description100"]; } catch { };
                    try { thisProgramme.episodeDescription.description1000 = temp["description1000"]; } catch { };
                    programmeData.Add(thisProgramme);
                }

                return programmeData.AsEnumerable();
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Retrieve schedule for the provides list of station/timeframe
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<SDScheduleResponse> GetSchedules(IEnumerable<SDScheduleRequest> request)
        {
            try
            {
                return PostJSON<IEnumerable<SDScheduleResponse>, IEnumerable<SDScheduleRequest>>("schedules", request, loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Retrieve MD5 hashes for provided list of station/timeframe
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<SDMD5Response> GetMD5(IEnumerable<SDMD5Request> request)
        {
            try
            {
                dynamic result = GetDynamic(WebPost("schedules/md5", CreateJSONstring<IEnumerable<SDMD5Request>>(request), loginToken));

                if (result == null)
                    return null;

                var md5Data = new List<SDMD5Response>();
                foreach (string resultKey in result.Keys)
                {
                    var thisResponse = new SDMD5Response();
                    thisResponse.stationID = resultKey;

                    dynamic dates = result[resultKey];

                    List<SDMD5Response.SDMD5Day> daysTemp = new List<SDMD5Response.SDMD5Day>();
                    foreach (string dateKey in dates.Keys)
                    {
                        SDMD5Response.SDMD5Day thisDay = new SDMD5Response.SDMD5Day();
                        thisDay.date = dateKey;
                        try { thisDay.md5data.code = dates[dateKey]["code"]; } catch { };
                        try { thisDay.md5data.message = dates[dateKey]["message"]; } catch { };
                        DateTime testDate;
                        if (DateTime.TryParse(dates[dateKey]["lastModified"], null, DateTimeStyles.RoundtripKind, out testDate))
                            thisDay.md5data.lastModified = testDate;

                        try { thisDay.md5data.md5 = dates[dateKey]["md5"]; } catch { };
                        daysTemp.Add(thisDay);
                    }
                    thisResponse.md5day = daysTemp.ToArray();
                    md5Data.Add(thisResponse);
                }

                return md5Data.AsEnumerable();
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Delete specified system message from login status
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public SDDeleteResponse DeleteMessage(string messageID)
        {
            try
            {
                return DeleteJSON<SDDeleteResponse>("messages/" + messageID, loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Obtain live programme information for example, for sporting events (if available)
        /// </summary>
        /// <param name="programmeID"></param>
        /// <returns></returns>
        public SDStillRunningResponse GetStillRunning(string programmeID)
        {
            try
            {
                return GetJSON<SDStillRunningResponse>("metadata/stillRunning/" + programmeID, loginToken);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Obtain image data for assets for specified programme ID(s)
        /// </summary>
        /// <param name="programmes"></param>
        /// <returns></returns>
        public IEnumerable<SDProgrammeMetadataResponse> GetProgramMetadata(string[] programmes)
        {
            try
            {
                return PostJSON<IEnumerable<SDProgrammeMetadataResponse>, string[]>("metadata/programs/", programmes);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Obtain image data for assets for specified root ID
        /// </summary>
        /// <param name="rootId"></param>
        /// <returns></returns>
        public IEnumerable<SDImageData> GetProgrammeRootMetadata(string rootId)
        {
            try
            {
                return GetJSON<IEnumerable<SDImageData>>("metadata/programs/" + rootId);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        /// <summary>
        /// Return image data for assets for specified celebrity ID
        /// </summary>
        /// <param name="celebrityID"></param>
        /// <returns></returns>
        public IEnumerable<SDImageData> GetCelebrityMetadata(string celebrityID)
        {
            try
            {
                return GetJSON<IEnumerable<SDImageData>>("metadata/celebrity/" + celebrityID);
            }
            catch (Exception ex)
            {
                addError(ex);
            }
            return null;
        }

        // For cases where we can't create a known object type
        // Parse JSON string and return dynamic type
        private dynamic GetDynamic(string jsonstring)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Deserialize<dynamic>(jsonstring);
        }

        // Parse known class object and return JSON string
        private string CreateJSONstring<T>(T obj)
        {
            MemoryStream jsonStream = new MemoryStream();
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ssZ"),
            });
            jsonSer.WriteObject(jsonStream, obj);

            jsonStream.Position = 0;
            StreamReader sr = new StreamReader(jsonStream);
            return sr.ReadToEnd();
        }

        // Parse JSON string and return known class object
        private T ParseJSON<T>(string input)
        {
            if (input == string.Empty)
                return default(T);

            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ssZ"),
            });

            return (T)jsonSer.ReadObject(jsonStream);
        }

        // Parse incoming known object with JSON serializer.
        // Perform post action and parse response via JSON serializer to known object type
        private V PostJSON<V, T>(string command, T obj, string token = "", WebHeaderCollection headers = null)
        {
            string requestString = CreateJSONstring(obj);
#if DEBUG
            DebugLog($"JSON Post [{command}] Request: {requestString}{Environment.NewLine}");
#endif
            var response = WebPost(command, requestString, token, headers);
            var result = ParseJSON<V>(response);
#if DEBUG
            DebugLog($"JSON Post Response: {response}{Environment.NewLine}");
#endif
            return result;
        }

        // Perform get action and parse response via JSON serializer to known object type
        private T GetJSON<T>(string command, string token = "", WebHeaderCollection headers = null)
        {
            return ParseJSON<T>(WebGet(command, token, headers));
        }

        // Perform put action and parse response via JSON serializer to known object type
        private T PutJSON<T>(string command, string token = "", WebHeaderCollection headers = null)
        {
            return ParseJSON<T>(WebPut(command, token, headers));
        }

        private T DeleteJSON<T>(string command, string token = "", WebHeaderCollection headers = null)
        {
            return ParseJSON<T>(WebDelete(command, token, headers));
        }

        // Handle get request, return response as string
        private string WebGet(string command, string token = "", WebHeaderCollection headers = null)
        {
            var getRequest = WebAction(urlBase + command, "GET", token, headers);

            try
            {
                var resp = (HttpWebResponse)getRequest.GetResponse();
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                queueError(ex);
                return "";
            }
        }

        // Handle put request, return response as string
        private string WebPut(string command, string token = "", WebHeaderCollection headers = null)
        {
            var putRequest = WebAction(urlBase + command, "PUT", token, headers);

            try
            {
                putRequest.Timeout = 5000;
                var resp = (HttpWebResponse)putRequest.GetResponse();
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                queueError(ex);
                return "";
            }
        }

        // Handle delete request, return response as string
        private string WebDelete(string command, string token = "", WebHeaderCollection headers = null)
        {
            var deleteRequest = WebAction(urlBase + command, "DELETE", token, headers);

            try
            {
                deleteRequest.Timeout = 5000;
                var resp = (HttpWebResponse)deleteRequest.GetResponse();
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                queueError(ex);
                return "";
            }
        }

        // Handle post request, return response as string
        private string WebPost(string command, string jsonstring, string token = "", WebHeaderCollection headers = null)
        {
            var postRequest = WebAction(urlBase + command, "POST", token, headers);

            using (var sr = new StreamWriter(postRequest.GetRequestStream()))
            {
                sr.Write(jsonstring);
                sr.Flush();
            }

            try
            {
                var resp = (HttpWebResponse)postRequest.GetResponse();
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                addError(ex);
                return "";
            }
        }

        // Create web request for specified action and URL
        private HttpWebRequest WebAction(string url, string action = "GET", string token = "", WebHeaderCollection headers = null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = action;
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Accept = "application/json; charset=utf-8";
            webRequest.UserAgent = userAgentFull;
            webRequest.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);

            if (headers != null)
                webRequest.Headers = headers;

            if (token != "")
                webRequest.Headers.Add("token: " + token);

            return webRequest;
        }

        // Queue exception to local errors
        // @Todo: Create local error class, encompassing local errors and exceptions
        private void queueError(Exception ex)
        {
            addError(ex);
        }

#if DEBUG
        private void DebugLog(string debugText)
        {
            string logStamp = DateTime.Now.ToString("O");
            File.AppendAllText(DEBUG_FILE, $"{logStamp}: {debugText}");
        }
#endif
    }
}
