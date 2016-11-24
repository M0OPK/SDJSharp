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

    public partial class SDJSON
    {
        private string loginToken;
        private List<Exception> localErrors;
        private static string urlBase = "https://json.schedulesdirect.org/20141201/";
        private static string userAgent = "SDJSharp JSON C# Library/1.0 (https://github.com/M0OPK/SDJSharp)";

        public SDJSON()
        {
            localErrors = new List<Exception>();
        }

        public IEnumerable<Exception> GetRawErrors()
        {
            return localErrors.AsEnumerable();
        }

        public Exception GetLastError(bool pop = true)
        {
            Exception ex = localErrors.Last();
            if (pop)
                localErrors.Remove(ex);
            return ex;
        }

        public void ClearErrors()
        {
            localErrors.Clear();
        }

        public SDTokenResponse Login(string username, string password)
        {
            SDTokenMessage logon = new SDTokenMessage();
            logon.username = username;
            logon.password = hashPassword(password);

            SDTokenResponse response = SendJSON<SDTokenResponse, SDTokenMessage>("token", logon);
            if (response == null || response.code != 0)
                return null;

            loginToken = response.token;
            return response;
        }

        public SDStatusResponse GetStatus()
        {
            if (loginToken == string.Empty)
                return null;

            return SendJSON<SDStatusResponse>("status", loginToken);
        }

        public SDVersionResponse GetVersion(string clientname)
        {
            if (clientname == string.Empty)
                return null;

            return SendJSON<SDVersionResponse>("token/" + clientname);
        }

        public IEnumerable<SDAvailableResponse> GetAvailable()
        {
            return SendJSON<IEnumerable<SDAvailableResponse>>("available");
        }

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

        public IEnumerable<SDHeadendsResponse> GetHeadends(string country, string postcode)
        {
            return SendJSON<IEnumerable<SDHeadendsResponse>>(string.Format("headends?country={0}&postalcode={1}", country, postcode), loginToken);
        }

        private string CreateJSONstring<T>(T obj)
        {
            MemoryStream jsonStream = new MemoryStream();
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
            jsonSer.WriteObject(jsonStream, obj);

            jsonStream.Position = 0;
            StreamReader sr = new StreamReader(jsonStream);
            return sr.ReadToEnd();
        }

        private dynamic GetDynamic(string jsonstring)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Deserialize<dynamic>(jsonstring);
        }

        private T GetJSON<T>(string input)
        {
            if (input == string.Empty)
                return default(T);

            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));

            return (T)jsonSer.ReadObject(jsonStream);
        }

        private V SendJSON<V, T>(string command, T obj, string token = "")
        {
            string requestString = CreateJSONstring(obj);
            return GetJSON<V>(WebPost(command, requestString, token));
        }

        private T SendJSON<T>(string command, string token = "")
        {
            return GetJSON<T>(WebGet(command, token));
        }

        private string WebGet(string command, string token = "")
        {
            var tokenRequest = (HttpWebRequest)WebRequest.Create(urlBase + command);
            tokenRequest.Method = "GET";
            tokenRequest.ContentType = "application/json; charset=utf-8";
            tokenRequest.Accept = "application/json; charset=utf-8";
            tokenRequest.UserAgent = userAgent;
            if (token != "")
                tokenRequest.Headers.Add("token: " + token);

            try
            {
                var resp = (HttpWebResponse)tokenRequest.GetResponse();
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

        private string WebPost(string command, string jsonstring, string token = "")
        {
            var tokenRequest = (HttpWebRequest)WebRequest.Create(urlBase + command);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/json; charset=utf-8";
            tokenRequest.Accept = "application/json; charset=utf-8";
            tokenRequest.UserAgent = userAgent;
            if (token != "")
                tokenRequest.Headers.Add("token: " + token);

            using (var sr = new StreamWriter(tokenRequest.GetRequestStream()))
            {
                sr.Write(jsonstring);
                sr.Flush();
                sr.Close();
            }

            try
            {
                var resp = (HttpWebResponse)tokenRequest.GetResponse();
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

        private string hashPassword(string password)
        {
            byte[] passBytes = Encoding.UTF8.GetBytes(password);
            SHA1 hash = SHA1.Create();
            byte[] hashBytes = hash.ComputeHash(passBytes);

            string hexString = BitConverter.ToString(hashBytes);
            return hexString.Replace("-", "").ToLower();
        }

        private void queueError(Exception ex)
        {
            localErrors.Add(ex);
        }
    }
}
