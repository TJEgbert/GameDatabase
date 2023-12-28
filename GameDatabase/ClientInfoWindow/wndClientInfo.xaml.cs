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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameDatabase.ClientInfoWindow;
using GameDatabase.Common;

namespace GameDatabase
{
    /// <summary>
    /// Interaction logic for wndClientInfo.xaml
    /// </summary>
    public partial class wndClientInfo : Window
    {
        /// <summary>
        /// Holds the clsDataAccess object to pass around
        /// </summary>
        public clsDataAccess Database;

        /// <summary>
        /// Holds the SQLStatements to pass to pass around
        /// </summary>
        public clsSQLStatements Statement;

        /// <summary>
        /// Holds the errorHandler handles general exceptions
        /// </summary>
        public clsErrorHandler errorHandler;


        /// <summary>
        /// Runs the logic for the client info window
        /// </summary>
        private clsClintInfoLogic Logic;

        /// <summary>
        /// Constructor for the wndClientInfo
        /// </summary>
        /// <param name="DatabaseAccess">clsDataAccess to pass along to Logic</param>
        /// <param name="SQL">clsSQLStatements to pass along to Logic</param>
        public wndClientInfo(clsDataAccess DatabaseAccess, clsSQLStatements SQL, clsErrorHandler ErrorHandler)
        {
            try
            {
                InitializeComponent();
                Database = DatabaseAccess;
                Statement = SQL;
                Logic = new clsClintInfoLogic(Database, Statement);
                errorHandler = ErrorHandler;
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Checks to make sure both fields have text in them and calls Logic to create and add the information to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_SubmitClientInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(txt_ClientID.Text == "" || txt_ClientSecret.Text == ""))
                {
                    Logic.AddClientInfo(txt_ClientID.Text, txt_ClientSecret.Text, cbx_StarterContent.IsChecked.Value);
                    Logic.GetNewToken();
                    this.Close();
                }
            }
            catch
            {
                Logic.DeleteClientInfo();
                MessageBox.Show("Twitch did not recognize the Client ID or Client Secret please verify that they are correct", "Client Info", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
