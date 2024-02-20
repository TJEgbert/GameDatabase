using GameDatabase.APIAccess;
using GameDatabase.Common;
using Newtonsoft.Json;
using RestEase.Implementation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.GameAddWindow
{
    public class clsGameAddLogic
    {
        /// <summary>
        /// Holds a copy of intaIGBAApi to Connect to IGBA API
        /// </summary>
        public intaIGBAApi IGBAApi { get; set; }

        /// <summary>
        /// Holds a copy of clsDataAccess to be able to access the personal database
        /// </summary>
        public clsDataAccess Database { get; set; }

        /// <summary>
        /// Holds a copy of clsSQLStatements to easily access SQL statements
        /// </summary>
        public clsSQLStatements SQLStatements { get; set; }

        /// <summary>
        /// An object that holds currently used endPoints from IGBA API
        /// </summary>
        private clsAPIEndPoints EndPoints;

        /// <summary>
        /// An object that holds the paths to query the IGBA API
        /// </summary>
        private clsAPIPaths Paths;

        /// <summary>
        /// Holds the starter content for status fields
        /// </summary>
        private List<string> StatusStarterContent;

        /// <summary>
        /// Holds the starter content for Rating fields
        /// </summary>
        private List<string> RatingStarterContent;

        /// <summary>
        /// Shows starter content for rating are used
        /// </summary>
        private bool RatingStarterContentUsed;

        /// <summary>
        /// Shows starter content for status are used
        /// </summary>
        private bool StatusStarterContentUsed;

        /// <summary>
        /// Constructor for the clsGameAddLogic sets passed in parameters and creates instances of clsAPIEndsPoints and clsAPIPaths
        /// </summary>
        /// <param name="API">Passed in intaIGBAApi</param>
        /// <param name="Base">Passed in clsDataAccess </param>
        /// <param name="SQL">Passed in clsSQLStatements</param>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public clsGameAddLogic(intaIGBAApi API, clsDataAccess Base, clsSQLStatements SQL)
        {
            try
            {
                IGBAApi = API;
                Database = Base;
                SQLStatements = SQL;
                EndPoints = new clsAPIEndPoints();
                Paths = new clsAPIPaths();

                StatusStarterContent = new List<string>();
                RatingStarterContent = new List<string>();
                RatingStarterContentUsed = false;
                StatusStarterContentUsed = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Calls the IGBA API and converts returning .jason into a array games
        /// </summary>
        /// <param name="GameTitle">The title of the game to be searched to in IGBA API</param>
        /// <returns>An array of clsJasonRelated objects</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public Array SearchGame(string GameTitle)
        {
            try
            {
                return IGBAApi.QueryAsync<clsJasonRelated>(EndPoints.Games(), Paths.SearchFor(GameTitle)).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Calls the IGBA API converts returning .jason into an array developers of the passed in GameID
        /// </summary>
        /// <param name="GameID">The ID of the selected game</param>
        /// <returns>An array of clsJasonRelated objects</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public Array GetDevelopersList(string GameID)
        {
            try
            {
                return IGBAApi.QueryAsync<clsJasonRelated>(EndPoints.Companies(), Paths.GetDevelopers(GameID)).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Calls the IGBA API converts returning .jason into an array publishers of passed in GameID
        /// </summary>
        /// <param name="GameID">The ID of the selected game</param>
        /// <returns>An array of clsJasonRelated objects</returns>
        public Array GetDevelopersPublishers(string GameID)
        {
            try
            {
                return IGBAApi.QueryAsync<clsJasonRelated>(EndPoints.Companies(), Paths.GetPublishers(GameID)).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Calls the IGBA API converts returning .jason into an array of different platforms
        /// </summary>
        /// <returns>An array of clsJasonRelated objects</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public Array GetPlatforms()
        {
            try
            {
                return IGBAApi.QueryAsync<clsJasonRelated>(EndPoints.Platforms(), Paths.GetPlatforms()).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Gets a list of unique status from the database and starter content if on
        /// </summary>
        /// <returns>A list of strings of status</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public List<string> GetStatuses()
        {
            try
            {
                List<string> StatusList = new List<string>();
                DataSet Data = new DataSet();
                int rows = 0;

                Data = Database.ExecuteSQLStatement(SQLStatements.GetFilter2("Status"), ref rows);

                if (Data != null)
                {

                    for (int i = 0; i < rows; i++)
                    {
                        StatusList.Add(Data.Tables[0].Rows[i][0].ToString());
                    }

                }

                if (StartContentOn())
                {
                    foreach (string Status in StatusStarterContent)
                    {
                        bool found = false;
                        foreach (string StatusInList in StatusList)
                        {
                            if (Status.ToLower() == StatusInList.ToLower())
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            StatusList.Add(Status);
                            StatusStarterContentUsed = true;
                        }
                    }
                }
                StatusList.Sort();
                TurnOffStartContent();
                return StatusList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of unique Ratings from the database and starter content if on
        /// </summary>
        /// <returns>A list of strings of status</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public List<string> GetRatings()
        {
            try
            {
                List<string> RatingList = new List<string>();
                DataSet Data = new DataSet();
                int rows = 0;

                Data = Database.ExecuteSQLStatement(SQLStatements.GetFilter2("Rating"), ref rows);

                if (Data != null)
                {

                    for (int i = 0; i < rows; i++)
                    {
                        RatingList.Add(Data.Tables[0].Rows[i][0].ToString());
                    }

                }
                if (StartContentOn())
                {
                    foreach (string Rating in RatingStarterContent)
                    {
                        bool found = false;
                        foreach (string RatingInList in RatingList)
                        {
                            if (Rating == RatingInList)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            RatingList.Add(Rating);
                            RatingStarterContentUsed = true;
                        }
                    }
                }

                RatingList.Sort();
                TurnOffStartContent();
                return RatingList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Call the database to add the newly created game to the database
        /// </summary>
        /// <param name="NewGame">clsGame the new game to added</param>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public void AddGame(clsGame NewGame)
        {
            try
            {
                NewGame.ID = GetNextID().ToString();
                if (NewGame.Date_Purchased != string.Empty)
                {
                    Database.ExecuteNonQuery(SQLStatements.AddGame(NewGame));
                }
                else
                {
                    Database.ExecuteNonQuery(SQLStatements.AddGameWithNoDate(NewGame));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }

        /// <summary>
        /// Checks to see if starter content is on and if so calls FillStartContent
        /// </summary>
        /// <returns>true if start content is on false if not</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public bool StartContentOn()
        {
            try
            {
                if (bool.Parse(Database.ExecuteScalerStatement(SQLStatements.StarterContent())))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Fills the starter content strings if need
        /// </summary>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public void FillStarterContent()
        {
            try
            {
                StatusStarterContent.Add("100% Completed");
                StatusStarterContent.Add("Never played");
                StatusStarterContent.Add("Need to pick up");
                StatusStarterContent.Add("Played");
                StatusStarterContent.Add("Played through once");
                StatusStarterContent.Add("Currently playing");
                StatusStarterContent.Add("Need to go back to 100%");

                for (int i = 0; i < 6; i++)
                {
                    RatingStarterContent.Add(i.ToString());
                    RatingStarterContent.Sort();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// If all the starter content been used once.  This turns off Starter Content for the user
        /// </summary>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        private void TurnOffStartContent()
        {
            try
            {
                if (!StatusStarterContentUsed && !RatingStarterContentUsed)
                {
                    string ClientID = Database.ExecuteScalerStatement(SQLStatements.GetClientID());
                    Database.ExecuteNonQuery(SQLStatements.UpdateStarterContent(ClientID, false));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
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

        /// <summary>
        /// Gets a list of unique platforms from the database and starter content if on
        /// </summary>
        /// <returns>A list of strings of platforms</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public List<string> GetPlatformList()
        {
            try
            {
                List<string> PlatformList = new List<string>();
                DataSet Data = new DataSet();
                int rows = 0;

                Data = Database.ExecuteSQLStatement(SQLStatements.GetFilter2("Platform"), ref rows);

                if (Data != null)
                {

                    for (int i = 0; i < rows; i++)
                    {
                        PlatformList.Add(Data.Tables[0].Rows[i][0].ToString());
                    }
                    PlatformList.Sort();

                }

                return PlatformList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of developers status from the database and starter content if on
        /// </summary>
        /// <returns>A list of strings of developers</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public List<string> GetDevelopersList()
        {
            try
            {
                List<string> DeveloperList = new List<string>();
                DataSet Data = new DataSet();
                int rows = 0;

                Data = Database.ExecuteSQLStatement(SQLStatements.GetFilter2("Developer"), ref rows);

                if (Data != null)
                {

                    for (int i = 0; i < rows; i++)
                    {
                        DeveloperList.Add(Data.Tables[0].Rows[i][0].ToString());
                    }
                    DeveloperList.Sort();
                }

                return DeveloperList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of developers status from the database and starter content if on
        /// </summary>
        /// <returns>A list of strings of developers</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        public List<string> GetPublishersList()
        {
            try
            {
                List<string> PublisherList = new List<string>();
                DataSet Data = new DataSet();
                int rows = 0;

                Data = Database.ExecuteSQLStatement(SQLStatements.GetFilter2("Developer"), ref rows);

                if (Data != null)
                {

                    for (int i = 0; i < rows; i++)
                    {
                        PublisherList.Add(Data.Tables[0].Rows[i][0].ToString());
                    }

                }
                PublisherList.Sort();
                return PublisherList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// Get the highest number index from the Game table and increments it by one
        /// </summary>
        /// <returns>The next GameID</returns>
        /// <exception cref="Exception">Throws it up to th next level</exception>
        private int GetNextID()
        {
            try
            {
                int nextID = 0;
                if (int.TryParse(Database.ExecuteScalerStatement(SQLStatements.GetLastEnteredID()), out nextID))
                {
                    nextID++;
                    return nextID;
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

    }

}
