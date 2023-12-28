using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.Common
{
    /// <summary>
    /// A class to hold the information related to Game table in the database
    /// </summary>
    public class clsGame
    {
        /// <summary>
        /// ID of the game from IGBA API
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Title of the game
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Developer of the game
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// Platform that the user owns the game on 
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Format that the user owns the game on
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The date the user purchased the game
        /// </summary>
        public string Date_Purchased { get; set; }

        /// <summary>
        /// Personal status the user chooses
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Personal rating the user gives
        /// </summary>
        public string Rating { get; set; }

        /// <summary>
        /// If the game has been completed
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// The publisher of the game
        /// </summary>
        public string Publisher {  get; set; }

        /// <summary>
        /// Used to verify if the game has been updated and need to changed in the database
        /// </summary>
        public bool BeenEdited { get; set; }

        /// <summary>
        /// Override of the ToString methods
        /// </summary>
        /// <returns>Returns the Title of the game</returns>
        public override string ToString()
        {
            return Title;
        }
    }
}
