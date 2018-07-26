using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            string token = this.GetAuthenticationToken("admin@test.com", "password", baseUrl);

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
                var webRequest = WebRequest.CreateHttp($"{baseUrl}/api/default/newsitems");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";

                // Set the authorization header to the acquired token. Authorization is needed for every request that requires restricted data.
                webRequest.Headers.Add(HttpRequestHeader.Authorization, token);

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
        /// Code inspired by https://docs.sitefinity.com/request-access-token-for-calling-web-services
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="baseUrl">The base URL of the service.</param>
        /// <returns></returns>
        internal string GetAuthenticationToken(string username, string password, string baseUrl)
        {
            var tokenClient = new TokenClient($"{baseUrl}/Sitefinity/Authenticate/OpenID/connect/token", "testApp", "secret", AuthenticationStyle.PostValues);

            var parameters = new Dictionary<string, string>()
            {
                { "membershipProvider", "Default" }
            };

            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(username, password, "openid offline_access", parameters).Result;

            if (tokenResponse.IsError)
            {
                throw new ApplicationException("Couldn't get access token. Error: " + tokenResponse.Error);
            }

            return $"Bearer {tokenResponse.AccessToken}";
        }
    }
}
