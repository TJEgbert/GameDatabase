using GameDatabase.Common;
using GameDatabase.GameAddWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GameDatabase.PlayListWindow
{
    public class clsPlayListLogic
    {
        /// <summary>
        /// Holds the clsDataAccess object to pass around
        /// </summary>
        public clsDataAccess database;

        /// <summary>
        /// Holds the SQLStatements to pass to pass around
        /// </summary>
        public clsSQLStatements SQLStatements;

        /// <summary>
        /// Constructor for clsPlayListLogic
        /// </summary>
        /// <param name="Database">clsDataAccess used to access the database</param>
        /// <param name="SQL">clsSQLStatements used to query that database</param>
        public clsPlayListLogic(clsDataAccess Database, clsSQLStatements SQL)
        {
            try
            {
                database = Database;
                SQLStatements = SQL;
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
        /// Uses the dataset the passed in the create a clsPlayListGame object
        /// </summary>
        /// <param name="Data">The dataset the contains the database response</param>
        /// <param name="RowNumber">The number of rows that was received from the database</param>
        /// <returns>clsPlayListGame object containing information from the database</returns>
        private clsPlayListGame CreatePlaylistItem(DataSet Data, int RowNumber)
        {
            try
            {
                clsPlayListGame game = new clsPlayListGame();

                game.ID = int.Parse(Data.Tables[0].Rows[RowNumber][0].ToString());
                game.PlayOrder = Data.Tables[0].Rows[RowNumber][1].ToString();
                game.GameID = int.Parse(Data.Tables[0].Rows[RowNumber][2].ToString());
                game.Title = database.ExecuteScalerStatement(SQLStatements.SearchForTitle(game.GameID.ToString()));
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

                GameData = database.ExecuteSQLStatement(SQLStatements.GetGames(), ref rows);

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
        /// Queries the database to get DataSet containing all the games and create ObservableCollection<clsPlayListGame>
        /// From the object from CreatePlaylistItem()
        /// </summary>
        /// <returns>ObservableCollection<clsPlayListGame></returns>
        public ObservableCollection<clsPlayListGame> GetPlayOrderList()
        {
            try
            {
                ObservableCollection<clsPlayListGame> GameOrderList = new ObservableCollection<clsPlayListGame>();

                DataSet GameData = new DataSet();
                int rows = 0;

                GameData = database.ExecuteSQLStatement(SQLStatements.GetGameOrder(), ref rows);

                if (GameData != null)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        GameOrderList.Add(CreatePlaylistItem(GameData, i));
                    }
                }

                return GameOrderList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }

        /// <summary>
        /// Queries the database to delete everything from PlayList table and then adds everything back into it
        /// </summary>
        /// <param name="GameList">ObservableCollection<clsPlayListGame></param>
        public void SaveOrderList(ObservableCollection<clsPlayListGame> GameList)
        {
            try
            {
                database.ExecuteNonQuery(SQLStatements.DeleteGameOrder());

                foreach (clsPlayListGame game in GameList)
                {
                    database.ExecuteNonQuery(SQLStatements.AddGameToPlayList(game));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

    }


}
