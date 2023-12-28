using GameDatabase.Common;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GameDatabase
{
    /// <summary>
    /// Used to get and save token from Twitch to be able to acces the IGBA API
    /// </summary>
    public class clsTwitchToken
    {
        /// <summary>
        /// Used to access SQL statements needed to access the database
        /// </summary>
        private clsSQLStatements SQLStatements { get; set; }

        /// <summary>
        /// Used to access the database
        /// </summary>
        private clsDataAccess Database { get; set; }

        /// <summary>
        /// The constructor for clsTwitchToken
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="SQL"></param>
        public clsTwitchToken(clsDataAccess Data, clsSQLStatements SQL)
        {
            Database = Data;
            SQLStatements = SQL;
        }

        /// <summary>
        /// Class used to be able to convert .json into c#
        /// </summary>
        public class AccessToken
        {
            [JsonProperty("access_token")]
            public string access_token { get; set; }

            [JsonProperty("expires_in")]
            public double expires_in { get; set; }

            [JsonProperty("token_type")]
            public string token_type { get; set; }
        }

        /// <summary>
        /// The interface to get the access_token from Twitch
        /// </summary>
        [Header("Accept", "application/json")]
        public interface iTwitch
        {
            [Post("/oauth2/token")]
            Task<AccessToken> CollectAsync([Body(BodySerializationMethod.UrlEncoded)] IDictionary<string, string> data);
        }

        /// <summary>
        /// Queries Twitch to get the access token needed to connect to IGBA API
        /// Then it saves that into the database along with the expiration date
        /// </summary>
        /// <param name="sClientID">The ClientID of the user</param>
        /// <param name="sClientSecret">The ClientSecret of the user</param>
        public void GetToken(string sClientID, string sClientSecret)
        {
            iTwitch api = RestClient.For<iTwitch>("https://id.twitch.tv");

            var data = new Dictionary<string, string>
            {
               {"client_id", sClientID},
               {"client_secret", sClientSecret},
               {"grant_type", "client_credentials"}
            };

            AccessToken Token = api.CollectAsync(data).Result;

         
            DateTime startDate = DateTime.Now;
            double safe = Token.expires_in;
            DateTime endDate = startDate.Add(TimeSpan.FromSeconds(safe));
            Database.ExecuteNonQuery(SQLStatements.AddTokenToDatabase(Token.access_token, endDate.ToString("MM/dd/yyyy")));
        }

    }
}
