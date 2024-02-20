using GameDatabase.AddClientInfo;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameDatabase.SettingsWindow
{
    /// <summary>
    /// Interaction logic for wndSettings.xaml
    /// </summary>
    public partial class wndSettings : Window
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
        /// Runs the logic for Settings window
        /// </summary>
        private clsSettingsLogic Logic;

        /// <summary>
        /// Constructor for the wndSettings sets and pass passed in objects
        /// </summary>
        /// <param name="Base">clsDataAccess passed to clsSettingsLogic</param>
        /// <param name="SQL">clsSQLStatements passed to clsSQLStatements</param>
        public wndSettings(clsDataAccess Base, clsSQLStatements SQL, clsErrorHandler ErrorHandler)
        {
            try
            {
                InitializeComponent();

                database = Base;
                SQLStatements = SQL;
                errorHandler = ErrorHandler;
                Logic = new clsSettingsLogic(database, SQLStatements);

                cbx_StartContentStatus.IsChecked = Logic.StartContentStatus();
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Once the checked box is clicked it updates that status of start content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateStarterContentStatus(object sender, RoutedEventArgs e)
        {
            try
            {
                Logic.UpdateStatus(cbx_StartContentStatus.IsChecked.Value);
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
           
        }

        /// <summary>
        /// Let the user add their client info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_AddClientInfor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndAddClientInfo AddClientInfo = new wndAddClientInfo(database, SQLStatements, errorHandler);
                AddClientInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Lets the user remove their client info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_RemoveClientInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult answer = MessageBox.Show("Are you sure you want to remove your client info?", "Confirmation",
                                                  MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    Logic.RemoveClientInfo();
                    MessageBox.Show("Client info successfully removed!", "Client info removed", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }
    }
}
