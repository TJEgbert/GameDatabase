using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using System.Reflection;

namespace GameDatabase.Common
{
    /// <summary>
    /// Lets you interface with a local database
    /// </summary>
    public class clsDataAccess
    {
        /// <summary>
        /// The connection string needed to access the database
        /// </summary>
        private string sConnection;

        /// <summary>
        /// Constructor for clsDataAccess
        /// </summary>
        public clsDataAccess()
        {
            sConnection = @"Provider=Microsoft.Jet.OLEDB.4.0;Data source= " + Directory.GetCurrentDirectory() + "\\GameDatabase.mdb";
        }

        /// <summary>
        /// Execute a SQL statement that returns multiple rows
        /// </summary>
        /// <param name="sSQL">string of the SQL statement to execute</param>
        /// <param name="iRetVal">ref int to number of rows returned</param>
        /// <returns>DataSet of the data queried</returns>
        /// <exception cref="Exception">Throws exception up the the next level</exception>
        public DataSet ExecuteSQLStatement(string sSQL, ref int iRetVal)
        {
            try
            {
                DataSet ds = new DataSet();

                using (OleDbConnection conn = new OleDbConnection(sConnection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        conn.Open();
                        OleDbCommand command = conn.CreateCommand();

                        adapter.SelectCommand = new OleDbCommand(sSQL, conn);
                        adapter.SelectCommand.CommandTimeout = 0;

                        adapter.Fill(ds);
                    }

                }

                iRetVal = ds.Tables[0].Rows.Count;

                return ds;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodBase.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodBase.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Execute a query that returns one result
        /// </summary>
        /// <param name="sSQL">string of the SQL statement to execute</param>
        /// <returns>string of the one result</returns>
        /// <exception cref="Exception">Throws exception up the the next level</exception>
        public string ExecuteScalerStatement(string sSQL)
        {
            try
            {
                object obj;

                using (OleDbConnection conn = new OleDbConnection(sConnection))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        conn.Open();

                        adapter.SelectCommand = new OleDbCommand(sSQL, conn);
                        adapter.SelectCommand.CommandTimeout = 0;

                        obj = adapter.SelectCommand.ExecuteScalar();
                    }

                }

                //See if the object is null
                if (obj == null)
                {
                    //Return a blank
                    return "";
                }
                else
                {
                    //Return the value
                    return obj.ToString();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(MethodBase.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodBase.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Execute a query with no return data 
        /// </summary>
        /// <param name="sSQL">string of the SQL statement to execute</param>
        /// <returns>the number of rows effected by the query</returns>
        /// <exception cref="Exception">Throws exception up the the next level</exception>
        public int ExecuteNonQuery(string sSQL)
        {
            try
            {
                int iNumOfRowsAffected;

                using (OleDbConnection conn = new OleDbConnection(sConnection))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand(sSQL, conn);
                    command.CommandTimeout = 0;


                    iNumOfRowsAffected = command.ExecuteNonQuery();

                }

                return iNumOfRowsAffected;
            }
            catch (Exception ex)
            {

                throw new Exception(MethodBase.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodBase.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }
    }
}
