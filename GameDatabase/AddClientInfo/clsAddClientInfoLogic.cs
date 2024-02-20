using GameDatabase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.AddClientInfo
{
    public class clsAddClientInfoLogic
    {
        /// <summary>
        /// Holds the clsDataAccess object to pass to clsGameAddLogic
        /// </summary>
        public clsDataAccess database;

        /// <summary>
        /// Holds the SQLStatements to pass to clsGameAddLogic
        /// </summary>
        public clsSQLStatements SQLStatements;

        /// <summary>
        /// Holds the errorHandler handles general exceptions
        /// </summary>
        public clsErrorHandler errorHandler;

        public clsAddClientInfoLogic(clsDataAccess data, clsSQLStatements SQL, clsErrorHandler Handler)
        {
            database = data;
            SQLStatements = SQL;
            errorHandler = Handler;
        }

        public void UpdateClientInfo(string ClientID, string ClientSecret)
        {
            bool Start_Content = bool.Parse(database.ExecuteScalerStatement(SQLStatements.StarterContent()));
            database.ExecuteNonQuery(SQLStatements.DeleteClientInfo());
            database.ExecuteNonQuery(SQLStatements.InsertClientInfo(ClientID, ClientSecret, Start_Content, true));

           GetNewToken();
        }

        public void GetNewToken()
        {
            try
            {
                clsTwitchToken TwitchToken;
                TwitchToken = new clsTwitchToken(database, SQLStatements);
                TwitchToken.GetToken(database.ExecuteScalerStatement(SQLStatements.GetClientID()), database.ExecuteScalerStatement(SQLStatements.GetClientSecret()));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

    }
}
