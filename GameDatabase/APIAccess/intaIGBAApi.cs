using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameDatabase.MainWindow;

namespace GameDatabase.APIAccess
{
    /// <summary>
    /// The interface used to access the IGBA API
    /// </summary>
    [Header("Accept", "application/json")]
    public interface intaIGBAApi
    {

        /// <summary>
        /// Stores the Client ID
        /// </summary>
        [Header("client-id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Stores the Client Authorization
        /// </summary>
        [Header("Authorization")]
        public string Authorization { get; set; }

        /// <summary>
        /// The task used to query the IGBA API
        /// </summary>
        /// <typeparam name="T">The variable type you are expecting to receive from the API</typeparam>
        /// <param name="endpoint">The part of the API you want to access</param>
        /// <param name="query">The path need to query your desired results</param>
        /// <returns>The results of the query</returns>
        [Post("/{endpoint}")]
        Task<T[]> QueryAsync<T>([Path] string endpoint, [Body] string query);

    }

    // Game[] collection = IGBAApi.QueryAsync<Game>("games", "fields name; search \"The legend of Heroes: Trails in the Sky\"; limit 50;").Result;

}
