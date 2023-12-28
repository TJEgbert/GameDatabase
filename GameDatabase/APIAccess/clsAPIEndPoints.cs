using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.APIAccess
{
    /// <summary>
    /// Holds the used Endpoints for the IGBA API
    /// </summary>
    public class clsAPIEndPoints
    {
        /// <summary>
        /// Endpoint to get access to Games
        /// </summary>
        /// <returns>string</returns>
        public string Games()
        {
            return "games";
        }

        /// <summary>
        /// Endpoint to get access to Companies
        /// </summary>
        /// <returns>string</returns>
        public string Companies()
        {
            return "companies";
        }

        /// <summary>
        /// Endpoint to get access to Platforms
        /// </summary>
        /// <returns>string</returns>
        public string Platforms()
        {
            return "platforms";
        }
    }
}
