using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
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
using GameDatabase.APIAccess;
using GameDatabase.Common;
using GameDatabase.GameAddWindow;
using Newtonsoft.Json;

namespace GameDatabase
{
    /// <summary>
    /// Interaction logic for wndGameAdd.xaml
    /// </summary>
    public partial class wndGameAdd : Window
    {
        /// <summary>
        /// Holds the intaIGBAApi interface to pass around
        /// </summary>
        public intaIGBAApi IGBAApi;

        /// <summary>
        /// Holds the clsDataAccess object to pass to clsGameAddLogic
        /// </summary>
        public clsDataAccess database;

        /// <summary>
        /// Holds the SQLStatements to pass to clsGameAddLogic
        /// </summary>
        public clsSQLStatements SQLStatements;

        /// <summary>
        /// Holds onto the MainWindow to call functions on it
        /// </summary>
        public MainWindow MainWindow;

        /// <summary>
        /// Holds an object of clsGameAddLogic to run logic
        /// </summary>
        private clsGameAddLogic Logic;

        /// <summary>
        /// Holds the errorHandler handles general exceptions
        /// </summary>
        public clsErrorHandler errorHandler;

        /// <summary>
        /// Sets the everything to its default state of when the window opens
        /// </summary>
        private void StartingSate()
        {
            try
            {
                txt_Title.Text = string.Empty;

                cbx_DevelopedList.SelectedIndex = -1;
                cbx_DevelopedList.Text = string.Empty;

                cbx_Publishers.SelectedIndex = -1;
                cbx_Publishers.Text = string.Empty;

                cbx_PlatformList.SelectedIndex = -1;
                cbx_PlatformList.Text = string.Empty;

                cbx_Format.SelectedIndex = -1;

                dt_PurchasedDate.Text = string.Empty;

                cbx_Rating.SelectedIndex = -1;
                cbx_Rating.Text = string.Empty;

                cbx_Status.SelectedIndex = -1;
                cbx_Status.Text = string.Empty;

                chx_Completed.IsChecked = false;
                gb_GameInfo.IsEnabled = false;

                if (Logic.StartContentOn())
                {
                    Logic.FillStarterContent();
                }

                cbx_Status.ItemsSource = Logic.GetStatuses();
                cbx_Rating.ItemsSource = Logic.GetRatings();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }

        }

        /// <summary>
        /// Checks to make sure all the required fields are filled out
        /// </summary>
        /// <returns>true if all are field out false if not</returns>
        private bool RequiredFields()
        {
            try
            {
                bool Title = false;
                bool Developer = false;
                bool Platform = false;
                bool Format = false;
                bool Status = false;
                bool Rating = false;
                bool Publishers = false;

                if (txt_Title.Text == string.Empty)
                {
                    Title = false;
                }
                else
                {
                    Title = true;
                }

                if (cbx_DevelopedList.Text == string.Empty)
                {
                    Developer = false;
                }
                else
                {
                    Developer = true;
                }

                if (cbx_PlatformList.Text == string.Empty)
                {
                    Platform = false;
                }
                else
                {
                    Platform = true;
                }

                if (cbx_Format.Text == string.Empty)
                {
                    Format = false;
                }
                else
                {
                    Format = true;
                }

                if (cbx_Status.Text == string.Empty)
                {
                    Status = false;
                }
                else
                {
                    Status = true;
                }

                if (cbx_Rating.Text == string.Empty)
                {
                    Rating = false;
                }
                else
                {
                    Rating = true;
                }

                if (cbx_Publishers.Text == string.Empty)
                {
                    Publishers = false;
                }
                else
                {
                    Publishers = true;
                }

                if (Title && Developer && Platform && Format && Status && Rating && Publishers)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("One or more required fields are missing", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
           
        }

        /// <summary>
        /// Constructor of wndGame class set variables that get passed in and sets up window
        /// Also creates Logic class to
        /// </summary>
        /// <param name="API">intaIGBAApi to pass on to Logic</param>
        /// <param name="Base">clsDataAccess to pass onto Logic</param>
        /// <param name="SQL"></param>
        /// <param name="mainWindow"></param>
        public wndGameAdd(intaIGBAApi API, clsDataAccess Base, clsSQLStatements SQL, MainWindow mainWindow, clsErrorHandler ErrorHandler)
        {
            try
            {
                InitializeComponent();
                IGBAApi = API;
                database = Base;
                SQLStatements = SQL;
                MainWindow = mainWindow;
                errorHandler = ErrorHandler;

                Logic = new clsGameAddLogic(IGBAApi, database, SQLStatements);
                cbx_PlatformList.ItemsSource = Logic.GetPlatforms();
                if (Logic.StartContentOn())
                {
                    Logic.FillStarterContent();
                }
                cbx_Status.ItemsSource = Logic.GetStatuses();
                cbx_Rating.ItemsSource = Logic.GetRatings();
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Calls Logic to search IGBA API to look for games that is in the txt_SearchBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_SearchBox.Text != "")
                {
                    Array TitleList = Logic.SearchGame(txt_SearchBox.Text);
                    dtg_SearchResults.ItemsSource = TitleList;
                    if (TitleList.Length == 0)
                    {
                        MessageBox.Show("No results found in IGBA database check spelling and try again." +
                                        " Or enter information manually", "No Results Found", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        /// <summary>
        /// Calls Logic to get information about the game from IGBA API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetInfo(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtg_SearchResults.SelectedIndex >= 0)
                {
                    clsJasonRelated SelectedGame = (clsJasonRelated)dtg_SearchResults.SelectedItem;
                    cbx_DevelopedList.ItemsSource = Logic.GetDevelopersList(SelectedGame.id.ToString());
                    cbx_Publishers.ItemsSource = Logic.GetDevelopersPublishers(SelectedGame.id.ToString());
                    txt_Title.Text = SelectedGame.name;
                    gb_GameInfo.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        /// <summary>
        /// Creates an clsGame and inserts into the Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_AddGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RequiredFields())
                {
                    clsJasonRelated SelectedGame = (clsJasonRelated)dtg_SearchResults.SelectedItem;
                    clsGame NewGame = new clsGame();

                    NewGame.ID = SelectedGame.id.ToString();
                    NewGame.Title = SelectedGame.name;
                    NewGame.Developer = cbx_DevelopedList.Text;
                    NewGame.Platform = cbx_PlatformList.Text;
                    NewGame.Format = cbx_Format.Text;
                    if (dt_PurchasedDate.Text != string.Empty)
                    {
                        NewGame.Date_Purchased = dt_PurchasedDate.SelectedDate.Value.Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        NewGame.Date_Purchased = string.Empty;
                    }
                    NewGame.Status = cbx_Status.Text;
                    NewGame.Rating = cbx_Rating.Text;
                    NewGame.Completed = chx_Completed.IsChecked.Value;
                    NewGame.Publisher = cbx_Publishers.Text;
                    Logic.AddGame(NewGame);

                    MessageBox.Show("Game Successfully Added", "Game Added", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.GetGameCollections();
                    StartingSate();
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        /// <summary>
        /// Calls search if the "Enter" is clicked while in the search bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterPressed(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    cmd_Search_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Only allows numbers to pressed in the Rating combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumbersOnly(object sender, KeyEventArgs e)
        {
            try
            {
                if (!(e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                {
                    if (!(e.Key == Key.Back || e.Key == Key.Delete))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Prevents special character ', ", and \ from being entered into string related 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpecialCharacters(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.OemQuotes || e.Key == Key.OemPipe)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }
    }
}
