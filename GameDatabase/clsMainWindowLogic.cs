using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Navigation;
using GameDatabase.Common;

namespace GameDatabase
{
    public class clsMainWindowLogic
    {
        /// <summary>
        /// Used to access the database
        /// </summary>
        private clsDataAccess Database { get; set; }

        /// <summary>
        /// Used to access the SQL statements need for the program
        /// </summary>
        private clsSQLStatements SQLStatements { get; set; }

        /// <summary>
        /// Used to store TwitchToken
        /// </summary>
        private clsTwitchToken TwitchToken { get; set; }

        /// <summary>
        /// Constructor for clsMainWindowLogic
        /// </summary>
        /// <param name="database">sets using clsDataAccess</param>
        /// <param name="sQLStatements">sets using clsSQLStatements</param>
        /// <param name="twitchToken">sets using clsTwitchToken</param>
        /// <param name="errorHandler">sets using errorHandler</param>
        public clsMainWindowLogic(clsDataAccess database, clsSQLStatements sQLStatements, clsTwitchToken twitchToken)
        {
            try
            {
                Database = database;
                SQLStatements = sQLStatements;
                TwitchToken = twitchToken;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }


        /// <summary>
        /// Uses the dataset the passed in the create a clsGame object
        /// </summary>
        /// <param name="Data">The dataset the contains the database response</param>
        /// <param name="RowNumber">The number of rows that was received from the database</param>
        /// <returns>clsGame object containing information from the database</returns>
        private clsGame CreateGame(DataSet Data, int RowNumber)
        {
            try
            {
                clsGame game = new clsGame();
                game.ID = Data.Tables[0].Rows[RowNumber][0].ToString();
                game.Title = Data.Tables[0].Rows[RowNumber][1].ToString();
                game.Developer = Data.Tables[0].Rows[RowNumber][2].ToString();
                game.Platform = Data.Tables[0].Rows[RowNumber][3].ToString();
                game.Format = Data.Tables[0].Rows[RowNumber][4].ToString();
                string DatePurchased = Data.Tables[0].Rows[RowNumber][5].ToString();
                if (DatePurchased != string.Empty)
                {
                    DateTime Date = DateTime.Parse(DatePurchased);
                    game.Date_Purchased = Date.ToString("MM/dd/yyyy");
                }
                else
                {
                    game.Date_Purchased = string.Empty;
                }
                game.Status = Data.Tables[0].Rows[RowNumber][6].ToString();
                game.Rating = Data.Tables[0].Rows[RowNumber][7].ToString();
                game.Completed = bool.Parse(Data.Tables[0].Rows[RowNumber][8].ToString());
                game.Publisher = Data.Tables[0].Rows[RowNumber][9].ToString();
                game.BeenEdited = false;

                return game;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
           
        }

        /// <summary>
        /// Checks if the first the user is using the program
        /// </summary>
        /// <returns></returns>
        public bool NewClient()
        {
            try
            {
                if (Database.ExecuteScalerStatement(SQLStatements.GetClientID()) == "")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Gets the token to be able to access the IGBA API
        /// </summary>
        public void GetNewToken()
        {
            try
            {
                TwitchToken = new clsTwitchToken(Database, SQLStatements);
                TwitchToken.GetToken(Database.ExecuteScalerStatement(SQLStatements.GetClientID()), Database.ExecuteScalerStatement(SQLStatements.GetClientSecret()));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Checks tokens expiration date and get a new token if needed
        /// </summary>
        public void CheckTokenExpDate()
        {
            try
            {
                DateTime ExpirationDate = DateTime.Parse(Database.ExecuteScalerStatement(SQLStatements.GetExpirationDate()));

                if (ExpirationDate <= DateTime.Now)
                {
                    Database.ExecuteNonQuery(SQLStatements.DeleteOldToken());
                    GetNewToken();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
           
        }

        /// <summary>
        /// Queries the database to get DataSet containing all the games and create ObservableCollection<clsGame>
        /// From the object from CreateGame()
        /// </summary>
        /// <returns>ObservableCollection<clsGame></returns>
        public ObservableCollection<clsGame> GetGamesList()
        {
            try
            {
                ObservableCollection<clsGame> GameList = new ObservableCollection<clsGame>();
                DataSet GameData = new DataSet();
                int rows = 0;

                GameData = Database.ExecuteSQLStatement(SQLStatements.GetGames(), ref rows);

                if (GameData != null)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        GameList.Add(CreateGame(GameData, i));
                    }
                }

                return GameList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }

        /// <summary>
        /// Returns a list strings for all the columns in Game table in the database
        /// </summary>
        /// <returns>List of strings</returns>
        public List<string> GetFilter1List()
        {
            try
            {
                List<string> FilterList = new List<string>();
                FilterList.Add("Developer");
                FilterList.Add("Platform");
                FilterList.Add("Publisher");
                FilterList.Add("Format");
                FilterList.Add("Date Purchased");
                FilterList.Add("Status");
                FilterList.Add("Rating");
                FilterList.Add("Completed");

                return FilterList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Get a list of all unique instances from the Filter1List
        /// </summary>
        /// <param name="Selection">The selection from Filter1List</param>
        /// <returns>List of strings</returns>
        public List<string> GetFilter2List(string Selection)
        {
            try
            {
                List<string> Filter2List = new List<string>();
                DataSet Data = new DataSet();
                int rows = 0;

                bool NoDate = false;

                if (Selection == "Date_Purchased")
                {
                    NoDate = true;
                }

                Data = Database.ExecuteSQLStatement(SQLStatements.GetFilter2(Selection), ref rows);

                if (Data != null)
                {

                    for (int i = 0; i < rows; i++)
                    {
                        DateTime date;
                        if (DateTime.TryParse(Data.Tables[0].Rows[i][0].ToString(), out date))
                        {
                            Filter2List.Add(date.ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            if (NoDate)
                            {
                                Filter2List.Add("No Purchased Date");
                                NoDate = false;
                            }
                            else
                            {
                                Filter2List.Add(Data.Tables[0].Rows[i][0].ToString());
                            }
                        }
                    }

                }
                return Filter2List;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// String version Gets ObservableCollection<clsGame> filter1 equals a specific string from filter2
        /// </summary>
        /// <param name="Filter1">String of specific column from the Game Table</param>
        /// <param name="Filter2">String of a specific value from specific column</param>
        /// <returns>ObservableCollection<clsGame> with filter applied</returns>
        public ObservableCollection<clsGame> GetFilteredGameList(string Filter1, string Filter2)
        {
            try
            {
                ObservableCollection<clsGame> GameList = new ObservableCollection<clsGame>();
                DataSet GameData = new DataSet();
                int rows = 0;

                DateTime date;

                if (Filter2 == "No Purchased Date")
                {
                    string UpdatedFilter1 = Filter1.Replace(" ", "_");
                    GameData = Database.ExecuteSQLStatement(SQLStatements.GetFilteredGameNoDate(UpdatedFilter1), ref rows);
                }
                else if (DateTime.TryParse(Filter2, out date))
                {
                    string UpdatedFilter1 = Filter1.Replace(" ", "_");
                    GameData = Database.ExecuteSQLStatement(SQLStatements.GetFilteredGamesByDate(UpdatedFilter1, Filter2), ref rows);
                }
                else
                {
                    GameData = Database.ExecuteSQLStatement(SQLStatements.GetFilteredGamesByString(Filter1, Filter2), ref rows);
                }

                if (GameData != null)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        GameList.Add(CreateGame(GameData, i));
                    }
                }

                return GameList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }

        /// <summary>
        /// Int version Gets ObservableCollection<clsGame> filter1 equals a specific int from filter2
        /// </summary>
        /// <param name="Filter1">String of specific column from the Game Table</param>
        /// <param name="Filter2">Int of a specific value from specific column</param>
        /// <returns>ObservableCollection<clsGame> with filter applied</returns>
        public ObservableCollection<clsGame> GetFilteredGameList(string Filter1, int Filter2)
        {
            try
            {
                ObservableCollection<clsGame> GameList = new ObservableCollection<clsGame>();
                DataSet GameData = new DataSet();
                int rows = 0;

                GameData = Database.ExecuteSQLStatement(SQLStatements.GetFilteredGamesByInt(Filter1, Filter2), ref rows);

                if (GameData != null)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        GameList.Add(CreateGame(GameData, i));
                    }
                }

                return GameList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Bool version Gets ObservableCollection<clsGame> filter1 equals a specific bool from filter2
        /// </summary>
        /// <param name="Filter1">String of specific column from the Game Table</param>
        /// <param name="Filter2">Bool of a specific value from specific column</param>
        /// <returns>ObservableCollection<clsGame> with filter applied</returns>
        public ObservableCollection<clsGame> GetFilteredGameList(string Filter1, bool Filter2)
        {
            try
            {
                ObservableCollection<clsGame> GameList = new ObservableCollection<clsGame>();
                DataSet GameData = new DataSet();
                int rows = 0;

                GameData = Database.ExecuteSQLStatement(SQLStatements.GetFilteredGamesByBool(Filter1, Filter2), ref rows);

                if (GameData != null)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        GameList.Add(CreateGame(GameData, i));
                    }
                }

                return GameList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }

        /// <summary>
        /// Updates any game that has been edited in the database
        /// </summary>
        /// <param name="GameList">ObservableCollection<clsGame> with games that needed to be updated</param>
        public void UpdateGames(ObservableCollection<clsGame> GameList)
        {
            try
            {
                foreach (clsGame game in GameList)
                {
                    if (game.BeenEdited == true)
                    {
                        Database.ExecuteNonQuery(SQLStatements.DeleteGame(game.ID));

                        if (game.Date_Purchased != string.Empty)
                        {
                            Database.ExecuteNonQuery(SQLStatements.AddGame(game));
                        }
                        else
                        {
                            Database.ExecuteNonQuery(SQLStatements.AddGameWithNoDate(game));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            

        }

        /// <summary>
        /// Returns a single game from Game table in the database
        /// </summary>
        /// <param name="GameID">The ID of the game needed to be gotten</param>
        /// <returns>ObservableCollection<clsGame> of the game</returns>
        /// <exception cref="Exception">Throws the Exception to the next level</exception>
        public ObservableCollection<clsGame> GetSingleGame(string GameID)
        {

            try
            {
                ObservableCollection<clsGame> GameList = new ObservableCollection<clsGame>();
                DataSet GameData = new DataSet();
                int rows = 0;

                GameData = Database.ExecuteSQLStatement(SQLStatements.Search(GameID), ref rows);

                if (GameData != null)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        GameList.Add(CreateGame(GameData, i));
                    }
                }

                return GameList;
            }
            catch (Exception ex)
            {

               throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
               MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Deletes a specific game from the database
        /// </summary>
        /// <param name="game">clsGame of which needs to be deleted</param>
        public void DeleteGame(clsGame game)
        {
            try
            {
                Database.ExecuteNonQuery(SQLStatements.DeleteGame(game.ID));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the user API status
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public bool GetClientAPIStatus()
        {
            try
            {
                return bool.Parse(Database.ExecuteScalerStatement(SQLStatements.GetUseAPIStatus()));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

    }
}
