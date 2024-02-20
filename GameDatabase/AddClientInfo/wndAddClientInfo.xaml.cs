using GameDatabase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameDatabase.AddClientInfo
{
    /// <summary>
    /// Interaction logic for wndAddClientInfo.xaml
    /// </summary>
    public partial class wndAddClientInfo : Window
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

        /// <summary>
        /// Runs the logic for AddClient window
        /// </summary>
        private clsAddClientInfoLogic logic;

        /// <summary>
        /// Constructor for the wndAddClientInfo
        /// </summary>
        /// <param name="data">clsDataAccess to pass along to clsAddClientInfoLogic</param>
        /// <param name="SQL">clsSQLStatements to pass along to clsAddClientInfoLogic</param>
        /// <param name="Handler">clsErrorHandler handles any exception that is thrown</param>
        public wndAddClientInfo(clsDataAccess data, clsSQLStatements SQL, clsErrorHandler Handler)
        {
            try
            {
                database = data;
                SQLStatements = SQL;
                errorHandler = Handler;
                logic = new clsAddClientInfoLogic(data, SQL, Handler);
                InitializeComponent();
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Adds the client info the the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_SubmitClientInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_ClientID.Text != string.Empty && txt_ClientSecret.Text != string.Empty)
                {
                    logic.UpdateClientInfo(txt_ClientID.Text, txt_ClientSecret.Text);
                    MessageBox.Show("Client Info has been successfully entered", "Client Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("One or more required fields are missing", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }
    }
}
