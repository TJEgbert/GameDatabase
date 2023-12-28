using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.APIAccess
{
    /// <summary>
    /// Contains the paths for IGBA API
    /// </summary>
    public class clsAPIPaths
    {
        /// <summary>
        /// Contains " to help with querying the database
        /// </summary>
        const string quote = "\"";

        /// <summary>
        /// The path used to search for a title for IGBA API
        /// </summary>
        /// <param name="GameTitle">The title of the game the user is looking for</param>
        /// <returns>string of path needed to query the IGBA API</returns>
        public string SearchFor(string GameTitle)
        {
            return "fields name, platforms; search" + quote + GameTitle + quote + "; limit 50;";
        }

        /// <summary>
        /// The path used to query for a games Developers
        /// </summary>
        /// <param name="GameID">The ID used in the IGBA API</param>
        /// <returns>string of path needed to query the IGBA API</returns>
        public string GetDevelopers(string GameID)
        {
            return "fields name; where developed = ["+ GameID +"];";
        }

        /// <summary>
        /// The path used to query for a games Publishers
        /// </summary>
        /// <param name="GameID">The ID used in the IGBA API</param>
        /// <returns>string of path needed to query the IGBA API</returns>
        public string GetPublishers(string GameID)
        {
            return "fields name; where published = [" + GameID + "];";
        }

        /// <summary>
        /// The path used query for all the Platforms 
        /// </summary>
        /// <returns>string of path needed to query the IGBA API</returns>
        public string GetPlatforms()
        {
            return "fields name; sort name asc; limit 200;";
        }


    }
}
