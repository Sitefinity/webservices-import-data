using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace ImportDataFromExternalSystem
{
    public class DataImporter
    {
        /// <summary>
        /// Imports the data.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public void Import(string baseUrl)
        {
            // Get the data from the external news provider
            var news = new ExternalNewsProvider().GetNews();

            // Get an authentication token. The user must have rights to create news items - Editor for example.
            string token = this.GetAuthenticationToken("admin", "admin@2", baseUrl);

            foreach (var entry in news)
            {
                // Create the payload for the new item
                var jObj = JObject.FromObject(new
                {
                    Title = entry.Title,
                    Content = entry.Content,
                    Author = entry.Author,
                    UrlName = entry.UrlName
                });

                // Create the web request
                var webRequest = WebRequest.CreateHttp(baseUrl + "newsitems");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";

                // Set the authorization header to the acquired token. Authorization is needed for every request that requires restricted data.
                webRequest.Headers.Add(HttpRequestHeader.Authorization, token);

                // This header is important - it indicates to Sitefinty that this is a service call.
                webRequest.Headers.Add("X-SF-Service-Request", bool.TrueString);

                using (var writer = new StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(jObj.ToString());
                }

                // execute the request
                using (webRequest.GetResponse()) { }
            }
        }

        /// <summary>
        /// Acquires an authentication token from the server.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="baseUrl">The base URL of the service.</param>
        /// <returns></returns>
        internal string GetAuthenticationToken(string username, string password, string baseUrl)
        {
            var jObj = JObject.FromObject(new
            {
                username = username,
                password = password
            });

            // The endpoint for authentication is located at /api/default/login
            WebRequest request = WebRequest.CreateHttp(baseUrl + "login");
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jObj.ToString());
            }

            // Make the request and get the auth token
            var response = request.GetResponse() as HttpWebResponse;
            JToken token;
            using (response)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var contents = reader.ReadToEnd();
                    token = JToken.Parse(contents);
                }
            }

            return token["value"].ToString();
        }
    }
}
