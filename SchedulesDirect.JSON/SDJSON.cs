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

namespace SchedulesDirect
{

    public partial class SDJson
    {
        private string loginToken;
        private List<Exception> localErrors;
        private static string urlBase = "https://json.schedulesdirect.org/20141201/";
        private static string userAgent = "SDJSharp JSON C# Library/1.0 (https://github.com/M0OPK/SDJSharp)";

        public SDJson()
        {
            localErrors = new List<Exception>();
        }

        /// <summary>
        /// Return raw error details (if any)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Exception> GetRawErrors()
        {
            return localErrors.AsEnumerable();
        }

        /// <summary>
        /// Retrieve the most recent reported error. If specified pop (remove) after returning
        /// </summary>
        /// <param name="pop"></param>
        /// <returns></returns>
        public Exception GetLastError(bool pop = true)
        {
            Exception ex = localErrors.Last();
            if (pop)
                localErrors.Remove(ex);
            return ex;
        }

        /// <summary>
        /// Clear any errors
        /// </summary>
        public void ClearErrors()
        {
            localErrors.Clear();
        }

        /// <summary>
        /// Log in, and retrieve login token for session
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public SDTokenResponse Login(string username, string password)
        {
            SDTokenRequest logon = new SDTokenRequest();
            logon.username = username;
            logon.password = hashPassword(password);

            SDTokenResponse response = PostJSON<SDTokenResponse, SDTokenRequest>("token", logon);
            if (response == null || response.code != 0)
                return null;

            loginToken = response.token;
            return response;
        }

        /// <summary>
        /// Get system status, and account lineups (Requires login token)
        /// </summary>
        /// <returns></returns>
        public SDStatusResponse GetStatus()
        {
            if (loginToken == string.Empty)
                return null;

            return GetJSON<SDStatusResponse>("status", loginToken);
        }

        /// <summary>
        /// Returns version information for specified client name
        /// </summary>
        /// <param name="clientname"></param>
        /// <returns></returns>
        public SDVersionResponse GetVersion(string clientname)
        {
            if (clientname == string.Empty)
                return null;

            return GetJSON<SDVersionResponse>("token/" + clientname);
        }

        /// <summary>
        /// Returns list of available services
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SDAvailableResponse> GetAvailable()
        {
            return GetJSON<IEnumerable<SDAvailableResponse>>("available");
        }

        /// <summary>
        /// Returns list of countries supported.
        /// </summary>
        /// <returns></returns>
        public SDCountries GetCountries()
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

        /// <summary>
        /// Get list of transmitters for specified country
        /// </summary>
        /// <param name="countrycode"></param>
        /// <returns></returns>
        public IEnumerable<SDTransmitter> GetTransmitters(string countrycode)
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

        /// <summary>
        /// Retrieves the list of headends for the specified country and postcode (Requires login token)
        /// </summary>
        /// <param name="country"></param>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public IEnumerable<SDHeadendsResponse> GetHeadends(string country, string postcode)
        {
            return GetJSON<IEnumerable<SDHeadendsResponse>>(string.Format("headends?country={0}&postalcode={1}", country, postcode), loginToken);
        }

        /// <summary>
        /// Adds specified lineup to this account (Requires login token)
        /// </summary>
        /// <param name="lineupID"></param>
        /// <returns></returns>
        public SDAddRemoveLineupResponse AddLineup(string lineupID)
        {
            return PutJSON<SDAddRemoveLineupResponse>("lineups/" + lineupID, loginToken);
        }

        /// <summary>
        /// Return lineups for this account (Requires login token)
        /// </summary>
        /// <returns></returns>
        public SDLineupsResponse GetLineups()
        {
            return GetJSON<SDLineupsResponse>("lineups", loginToken);
        }

        /// <summary>
        /// Return map, stations and metadata for the specified lineup
        /// </summary>
        /// <param name="lineup"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public SDGetLineupResponse GetLineup(string lineup, bool verbose = false)
        {
            WebHeaderCollection headers = null;

            if (verbose)
            {
                headers = new WebHeaderCollection();
                headers.Add("verboseMap: true");
            }

            return GetJSON<SDGetLineupResponse>("lineups/" + lineup, loginToken, headers);
        }

        /// <summary>
        /// Return program information for the specified list of programs
        /// </summary>
        /// <param name="programs"></param>
        /// <returns></returns>
        public IEnumerable<SDProgramResponse> GetPrograms(string[] programs)
        {
            return PostJSON<IEnumerable<SDProgramResponse>, string[]>("programs", programs, loginToken);
        }

        /// <summary>
        /// Return program descriptions for the specified list of programs
        /// </summary>
        /// <param name="programs"></param>
        /// <returns></returns>
        public IEnumerable<SDDescriptionResponse> GetDescriptions(string[] programs)
        {
            dynamic result = GetDynamic(WebPost("metadata/description", CreateJSONstring<string[]>(programs), loginToken));

            if (result == null)
                return null;

            var programData = new List<SDDescriptionResponse>();
            foreach (string key in result.Keys)
            {
                var thisProgram = new SDDescriptionResponse();
                thisProgram.episodeID = key;
                dynamic temp = result[key];
                //thisProgram.episodeDescription = (SDDescriptionResponse.SDProgramDescription)result[key];
                try { thisProgram.episodeDescription.code = temp["code"]; } catch { };
                try { thisProgram.episodeDescription.description100 = temp["description100"]; } catch { };
                try { thisProgram.episodeDescription.description1000 = temp["description1000"]; } catch { };
                programData.Add(thisProgram);
            }

            return programData.AsEnumerable();
        }

        /// <summary>
        /// Retrieve schedule for the provides list of station/timeframe
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<SDScheduleResponse> GetSchedules(IEnumerable<SDScheduleRequest> request)
        {
            return PostJSON<IEnumerable<SDScheduleResponse>, IEnumerable<SDScheduleRequest>>("schedules", request, loginToken);
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
            return ParseJSON<V>(WebPost(command, requestString, token, headers));
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
                sr.Close();
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
                localErrors.Add(ex);
                return "";
            }
        }

        // Create web request for specified action and URL
        HttpWebRequest WebAction(string url, string action = "GET", string token = "", WebHeaderCollection headers = null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = action;
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Accept = "application/json; charset=utf-8";
            webRequest.UserAgent = userAgent;
            webRequest.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);

            if (headers != null)
                webRequest.Headers = headers;

            if (token != "")
                webRequest.Headers.Add("token: " + token);

            return webRequest;
        }

        // SHA1 hash functuion for passwords
        private string hashPassword(string password)
        {
            byte[] passBytes = Encoding.UTF8.GetBytes(password);
            SHA1 hash = SHA1.Create();
            byte[] hashBytes = hash.ComputeHash(passBytes);

            string hexString = BitConverter.ToString(hashBytes);
            return hexString.Replace("-", "").ToLower();
        }

        // Queue exception to local errors
        // @Todo: Create local error class, encompassing local errors and exceptions
        private void queueError(Exception ex)
        {
            localErrors.Add(ex);
        }
    }
}
