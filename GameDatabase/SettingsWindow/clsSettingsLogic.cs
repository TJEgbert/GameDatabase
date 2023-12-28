using GameDatabase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabase.SettingsWindow
{
    public class clsSettingsLogic
    {
        /// <summary>
        /// Holds a copy of clsDataAccess to be able to access the personal database
        /// </summary>
        public clsDataAccess Database { get; set; }

        /// <summary>
        /// Holds a copy of clsSQLStatements to easily access SQL statements
        /// </summary>
        public clsSQLStatements SQLStatements { get; set; }


        /// <summary>
        /// Constructor for the clsSettingsLogic
        /// </summary>
        /// <param name="Base">sets using clsDataAccess</param>
        /// <param name="SQL">sets using clsSQLStatements</param>
        public clsSettingsLogic(clsDataAccess Base, clsSQLStatements SQL)
        {
            try
            {
                Database = Base;
                SQLStatements = SQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Update the users preference on starter content
        /// </summary>
        /// <param name="Status">bool if the user wants the starter content of or on</param>
        public void UpdateStatus(bool Status)
        {
            try
            {
                string ClientID = Database.ExecuteScalerStatement(SQLStatements.GetClientID());
                Database.ExecuteNonQuery(SQLStatements.UpdateStarterContent(ClientID, Status));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Gets the current status of Starter Content
        /// </summary>
        /// <returns>bool true if its on false if its off</returns>
        public bool StartContentStatus()
        {
            try
            {
                return bool.Parse(Database.ExecuteScalerStatement(SQLStatements.StarterContent()));
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
