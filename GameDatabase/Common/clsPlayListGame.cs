using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.Common
{
    /// <summary>
    /// Used to store information from the database and the PlayList table
    /// </summary>
    public class clsPlayListGame
    {
        /// <summary>
        /// The ID of the Playlist object
        /// </summary>
        public int ID {  get; set; }

        /// <summary>
        /// The order the user would like to play the game in
        /// </summary>
        public string PlayOrder {  get; set; }

        /// <summary>
        /// The ID of the game that is associated with the play order
        /// </summary>
        public int GameID { get; set; }

        /// <summary>
        /// The title of the game
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tells with or not the item was edited on the data grid 
        /// </summary>
        public bool BeenEdited { get; set; }
    }
}
