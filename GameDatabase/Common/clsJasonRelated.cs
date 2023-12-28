using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.Common
{
    /// <summary>
    /// Used to store results from the API the come has .jason
    /// </summary>
    public class clsJasonRelated
    {
        /// <summary>
        /// Stores the ID of the .jason item from the API
        /// </summary>
        [JsonProperty("id")]
        public int id { get; set; }

        /// <summary>
        /// Stores the name of the .jason item from the API
        /// </summary>
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// Override for the ToString to return the name attribute
        /// </summary>
        /// <returns>string the name of associated with the ID</returns>
        public override string ToString()
        {
            return name;
        }
    }
}
