using GameDatabase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.ClientInfoWindow
{
    class clsClintInfoLogic
    {
        /// <summary>
        /// Use to access the database
        /// </summary>
        public clsDataAccess Database;

        /// <summary>
        /// Used to access the SQL statements used
        /// </summary>
        public clsSQLStatements SQLStatements;


        /// <summary>
        /// Constructor for the clsClientInfoLogic 
        /// </summary>
        /// <param name="Data">sets using clsDataAccess</param>
        /// <param name="SQL">sets using clsSQLStatements</param>
        public clsClintInfoLogic(clsDataAccess Data, clsSQLStatements SQL)
        {
            try
            {
                Database = Data;
                SQLStatements = SQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Adds the client information to the database 
        /// </summary>
        /// <param name="ClientID">Client ID number from Twitch</param>
        /// <param name="ClientSecret">Client Secret from Twitch</param>
        /// <param name="StarterContent">If the user wants start content or not</param>
        public void AddClientInfo(string ClientID, string ClientSecret, bool StarterContent, bool UseAPI)
        {
            try
            {
                Database.ExecuteNonQuery(SQLStatements.InsertClientInfo(ClientID, ClientSecret, StarterContent, UseAPI));
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
                clsTwitchToken TwitchToken;
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
        /// Deletes the client info from the database
        /// </summary>
        public void DeleteClientInfo()
        {
            try
            {
                Database.ExecuteNonQuery(SQLStatements.DeleteClientInfo());
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }
    }
}
