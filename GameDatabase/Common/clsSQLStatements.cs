using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace GameDatabase.Common
{
    /// <summary>
    /// Used to access the SQL Statements needed throughout the program
    /// </summary>
    public class clsSQLStatements
    {
        #region wndGameAdd Related

        /// <summary>
        /// Adds the passed in game with all information into the database
        /// </summary>
        /// <param name="Game">clsGame the game to be added to the database</param>
        /// <returns>string with query to do so</returns>
        public string AddGame(clsGame Game)
        {
            return "INSERT INTO Game (Game_ID, Title, Developer, Platform, Format, Date_Purchased, Status, Rating, Completed, Publisher)" +
                   "VALUES ("+ Game.ID +","+"'"+ Game.Title + "', '"+ Game.Developer + "', '"+ Game.Platform +"', '"+ Game.Format+"', " +
                   "#"+Game.Date_Purchased+"#, '"+Game.Status+"', "+ Game.Rating +", "+ Game.Completed +", '"+ Game.Publisher + "');";
        }

        /// <summary>
        /// Adds the passed in game with all information expect the date into the database
        /// </summary>
        /// <param name="Game">clsGame the game to be added to the database</param>
        /// <returns>string with query to do so</returns>
        public string AddGameWithNoDate(clsGame Game)
        {
            return "INSERT INTO Game (Title, Developer, Platform, Format, Status, Rating, Completed, Publisher)" +
                   "VALUES ('"+ Game.ID +","  + Game.Title + "', '" + Game.Developer + "', '" + Game.Platform + "', '" + Game.Format + "', '"
                   + Game.Status + "', " + Game.Rating + ", " + Game.Completed + ", '" + Game.Publisher + "');";
        }

        /// <summary>
        /// Gets the Starter_Content field from ClientInfo
        /// </summary>
        /// <returns>Returns the string to do so</returns>
        public string StarterContent()
        {
            return "SELECT Starter_Content FROM ClientInfo";
        }

        /// <summary>
        /// Sets the Starter_Content field from ClientInfo to false
        /// </summary>
        /// <returns>Returns the string to do so</returns>
        public string UpdateStarterContent(string ClientID, bool Status)
        {
            return "UPDATE ClientInfo SET Starter_Content = "+ Status + " WHERE Client_ID = '"+ ClientID +"';";
        }

        /// <summary>
        /// Sets the Starter_Content field from ClientInfo to false
        /// </summary>
        /// <returns>Returns the string to do so</returns>
        public string GetLastEnteredID()
        {
            return "SELECT MAX(Game_ID) From Game;";
        }


        #endregion

        #region MainWindow Related

        /// <summary>
        /// Gets the all version of which ever selection that gets passed in
        /// </summary>
        /// <param name="Selection">The selection the comes from filter 1</param>
        /// <returns>string with query to do so</returns>
        public string GetFilter2(string Selection)
        {
            return "SELECT DISTINCT " + Selection + " FROM Game;";
        }

        /// <summary>
        /// Get everything from the Game table and orders it by title
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string GetGames()
        {
            return "Select * FROM Game ORDER BY Title";
        }

        /// <summary>
        /// Get everything from the database where filter1 = filter 2 for a string related search
        /// </summary>
        /// <param name="Filter1">The column you want to search</param>
        /// <param name="Filter2">The specific info you looking for</param>
        /// <returns>string with query to do so</returns>
        public string GetFilteredGamesByString(string Filter1, string Filter2)
        {
            return "Select * FROM Game " +
                   "WHERE " + Filter1 + " = '" + Filter2 + "';";
        }

        /// <summary>
        /// Gets all the game where there is no date associated with them
        /// </summary>
        /// <param name="Filter1">Purchased_Date</param>
        /// <returns>string with query to do so</returns>
        public string GetFilteredGameNoDate(string Filter1)
        {
            return "Select * FROM Game " +
                   "WHERE " + Filter1 + " IS NUll;";
        }

        /// <summary>
        /// Get everything from the database where filter1 = filter 2 for an int related search
        /// </summary>
        /// <param name="Filter1">The column you want to search</param>
        /// <param name="Filter2">The specific info your looking for</param>
        /// <returns>string with query to do so</returns>
        public string GetFilteredGamesByInt(string Filter1, int Filter2)
        {
            return "Select * FROM Game " +
                   "WHERE " + Filter1 + " = " + Filter2 + ";";
        }

        /// <summary>
        /// Get everything from the database where filter1 = filter 2 for a bool related search
        /// </summary>
        /// <param name="Filter1">The column you want to search</param>
        /// <param name="Filter2">The specific info your looking for</param>
        /// <returns>string with query to do so</returns>
        public string GetFilteredGamesByBool(string Filter1, bool Filter2)
        {
            return "Select * FROM Game " +
                   "WHERE " + Filter1 + " = " + Filter2 + ";";
        }

        /// <summary>
        /// Get everything where a fall on certain date
        /// </summary>
        /// <param name="Filter1">Purchased_Date</param>
        /// <param name="Filter2">The specific date your looking for</param>
        /// <returns>string with query to do so</returns>
        public string GetFilteredGamesByDate(string Filter1, string Filter2)
        {
            return "Select * FROM Game " +
                   "WHERE " + Filter1 + " = #" + Filter2 + "#;";
        }

        /// <summary>
        /// Deletes a specific date from the database
        /// </summary>
        /// <param name="GameID"></param>
        /// <returns>string with query to do so</returns>
        public string DeleteGame(string GameID)
        {
            return "DELETE FROM Game WHERE Game_ID = "+ GameID +";";
        }

        /// <summary>
        /// Searches search for a specific game by its id
        /// </summary>
        /// <param name="GameID">The ID of the game</param>
        /// <returns>string with query to do so</returns>
        public string Search(string GameID)
        {
            return "Select * FROM Game WHERE Game_ID = "+ GameID +";";
        }

        #endregion


        #region wndPlayList Releated
        /// <summary>
        /// Get the title of a specific game by its id
        /// </summary>
        /// <param name="GameID">The ID of the game</param>
        /// <returns>string with query to do so</returns>
        public string SearchForTitle(string GameID)
        {
            return "Select Title FROM Game WHERE Game_ID = " + GameID + ";";
        }

        /// <summary>
        /// Gets everything from the PlayList table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string GetGameOrder()
        {
            return "SELECT * FROM PlayList";
        }

        /// <summary>
        /// Deletes everything form the PlayList table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string DeleteGameOrder()
        {
            return "DELETE * FROM PlayList";
        }

        /// <summary>
        /// Adds a game to the PlayList table
        /// </summary>
        /// <param name="Game">clsPlayListGame the Game that is needed to be added</param>
        /// <returns>string with query to do so</returns>
        public string AddGameToPlayList(clsPlayListGame Game)
        {
            return "INSERT INTO PlayList(Play_Order, Game_ID) " +
                "VALUES(" + Game.PlayOrder + ", " + Game.GameID + ")";
        }

        #endregion

        #region wndClientInfo Releated

        /// <summary>
        /// Adds the unique user token into the database with its expiration date 
        /// </summary>
        /// <param name="TokenID">unique user token from the IGBA API</param>
        /// <param name="EndDate">expiration date </param>
        /// <returns>string with query to do so</returns>
        public string AddTokenToDatabase(string TokenID, string EndDate)
        {
            return "INSERT INTO Token (Token_ID, End_Date)" +
                   "Values('Bearer " + TokenID + "', #" + EndDate + "#);";
        }

        /// <summary>
        /// Gets token id from Token table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string GetToken()
        {
            return "SELECT Token_ID FROM Token";
        }

        /// <summary>
        /// Gets the client ID from the ClientInfo table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string GetClientID()
        {
            return "Select Client_ID From ClientInfo;";
        }

        /// <summary>
        /// Gets the client Secret from the ClientInfo table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string GetClientSecret()
        {
            return "Select Client_Secret From ClientInfo;";
        }

        /// <summary>
        /// Add the clientID and ClientSecret into the ClientInfo table
        /// </summary>
        /// <param name="ClientID">string of the clients id</param>
        /// <param name="ClientSecret">string of the client secret</param>
        /// <returns>string with query to do so</returns>
        public string InsertClientInfo(string ClientID, string ClientSecret, bool StartContent, bool UseAPI)
        {
            return "INSERT INTO ClientInfo(Client_ID, Client_Secret, Starter_Content, Use_API)" +
                   "VALUES('" + ClientID + "','" + ClientSecret +"', "+ StartContent +", "+ UseAPI+");";
        }

        public string GetUseAPIStatus()
        {
            return "SELECT Use_API FROM ClientInfo";
        }

        /// <summary>
        /// Gets the expiration date from the Token table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string GetExpirationDate()
        {
            return "SELECT End_Date FROM Token;";
        }

        /// <summary>
        /// Deletes everything from the Token table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string DeleteOldToken()
        {
            return "DELETE * FROM Token;";
        }

        /// <summary>
        /// Deletes everything from the ClientInfo table
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string DeleteClientInfo()
        {
            return "DELETE * FROM ClientInfo;";
        }

        /// <summary>
        /// Removes the Client_ID, Client_Secret and sets Use_API to false
        /// </summary>
        /// <returns>string with query to do so</returns>
        public string RemoveAPIInfo()
        {
            return ("UPDATE ClientInfo SET Client_ID = 'Did not use', Client_Secret = 'Did not use', Use_API = false;");
        }
        #endregion
    }
}
