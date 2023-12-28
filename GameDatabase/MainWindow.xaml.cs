using GameDatabase.APIAccess;
using GameDatabase.Common;
using GameDatabase.PlayListWindow;
using GameDatabase.SettingsWindow;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Reflection;

/*
 * Version 1.0 Last updated 12/28/2023
 * Made by TJEgbert
 * GitHub: https://github.com/TJEgbert
 */
namespace GameDatabase
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Holds the clsDataAccess that gets passed around for the program
        /// </summary>
        public clsDataAccess database;

        /// <summary>
        /// Holds the clSQLStatements that gets passed around for the program
        /// </summary>
        public clsSQLStatements SQLStatements;

        /// <summary>
        /// Runs the logic for the main window
        /// </summary>
        public clsMainWindowLogic Logic;

        /// <summary>
        /// Holds the ObservableCollection of clsGame used for displaying on the main window
        /// </summary>
        private ObservableCollection<clsGame> GamesList;

        /// <summary>
        /// Holds the intaIGAApi that gets passed around to the program
        /// </summary>
        public intaIGBAApi IGBAApi;

        /// <summary>
        /// Holds the errorHandler handles general exceptions
        /// </summary>
        public clsErrorHandler errorHandler;

        /// <summary>
        /// Get the ObservableCollection of clsGames and updates the window accordingly
        /// </summary>
        public void GetGameCollections()
        {
            try
            {
                GamesList = Logic.GetGamesList();
                dtg_GameList.ItemsSource = GamesList;
                cbx_SearchByTitle.ItemsSource = Logic.GetGamesList();
                cmd_Delete.IsEnabled = false;
            }
            catch (Exception ex)
            {

               throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
               MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            
            }
        }

        /// <summary>
        /// Sets the Main Window starting state
        /// </summary>
        private void StartingState()
        {
            try
            {
                GetGameCollections();
                cbx_FilterOptions.ItemsSource = Logic.GetFilter1List();
                cbx_Filter2Options.IsEnabled = false;
                cmd_FilterButton.IsEnabled = false;
                cmd_RemoveFilter.IsEnabled = false;
                cmd_Delete.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + " . " +
                MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        /// <summary>
        /// The constructor for the MainWindow it also gets everything ready to be passed around
        /// If its a new user it creates wndClientInfo and opens that window
        /// MainWindow will close itself if no information was entered
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                errorHandler = new clsErrorHandler();
                database = new clsDataAccess();
                SQLStatements = new clsSQLStatements();
                clsTwitchToken Token = new clsTwitchToken(database, SQLStatements);
                Logic = new clsMainWindowLogic(database, SQLStatements, Token);
                IGBAApi = RestClient.For<intaIGBAApi>("https://api.igdb.com/v4");

                if (Logic.NewClient())
                {
                    wndClientInfo clientInfo = new wndClientInfo(database, SQLStatements, errorHandler);
                    clientInfo.ShowDialog();
                }

                if (Logic.NewClient())
                {
                    this.Close();
                }

                Logic.CheckTokenExpDate();

                IGBAApi.ClientId = database.ExecuteScalerStatement(SQLStatements.GetClientID());
                IGBAApi.Authorization = database.ExecuteScalerStatement(SQLStatements.GetToken());

                StartingState();
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
           

        }

        /// <summary>
        /// Once an option form filter1 is selected it will get the associated values for filter 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateFilter2(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_FilterOptions.SelectedIndex >= 0)
                {

                    cbx_Filter2Options.SelectedIndex = -1;
                    cbx_Filter2Options.IsEnabled = true;

                    if (cbx_FilterOptions.SelectedItem.ToString() == "Date Purchased") // Used for Dates
                    {
                        string DateRelated = cbx_FilterOptions.SelectedItem.ToString().Replace(" ", "_");
                        cbx_Filter2Options.ItemsSource = Logic.GetFilter2List(DateRelated);
                    }
                    else
                    {
                        cbx_Filter2Options.ItemsSource = Logic.GetFilter2List(cbx_FilterOptions.SelectedItem.ToString());
                    }

                }
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Once filter2 has been selected it will call Logic to get relevant games 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_FilterButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (cbx_Filter2Options.IsEnabled == true)
                {
                    if (cbx_FilterOptions.SelectedItem.ToString() == "Rating") // used for ints
                    {
                        GamesList = Logic.GetFilteredGameList(cbx_FilterOptions.SelectedItem.ToString(), int.Parse(cbx_Filter2Options.SelectedItem.ToString()));
                    }
                    else if (cbx_FilterOptions.SelectedItem.ToString() == "Completed") // used for bools
                    {
                        GamesList = Logic.GetFilteredGameList(cbx_FilterOptions.SelectedItem.ToString(), bool.Parse(cbx_Filter2Options.SelectedItem.ToString()));
                    }
                    else
                    {
                        GamesList = Logic.GetFilteredGameList(cbx_FilterOptions.SelectedItem.ToString(), cbx_Filter2Options.SelectedItem.ToString()); // covers strings or dates
                    }
                    dtg_GameList.ItemsSource = GamesList;
                    cmd_RemoveFilter.IsEnabled = true;
                    cmd_Delete.IsEnabled = false;


                }
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Sets filter1 and 2 to being empty and calls starting date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_RemoveFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cbx_FilterOptions.SelectedIndex = -1;
                cbx_Filter2Options.SelectedIndex = -1;
                StartingState();
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Creates and opens the AddGameWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGameWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                wndGameAdd AddGameWindow = new wndGameAdd(IGBAApi, database, SQLStatements, this, errorHandler);
                AddGameWindow.ShowDialog();
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Enables the FilterButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableFilterButton(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmd_FilterButton.IsEnabled = true;
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Set the edited game.BeenEdited = true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HasBeenEdited(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                clsGame EditedGame = (clsGame)dtg_GameList.SelectedItem;
                EditedGame.BeenEdited = true;
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Once editMode button is clicked adjust the window accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_editMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtg_GameList.IsReadOnly = false;
                grb_Search.IsEnabled = false;
                cmd_Delete.IsEnabled = false;
                cmd_editMode.IsEnabled = false;
                cmd_SaveEdits.IsEnabled = true;
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Verifies that rating and date is entered in correctly and if so call Logic to update the edited games
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_SaveEdits_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool Error = false;
                foreach (clsGame game in GamesList)
                {
                    if (game.BeenEdited)
                    {
                        if (!(int.TryParse(game.Rating, out int rating)))
                        {
                            MessageBox.Show("Rating must contain a number", "Rating error", MessageBoxButton.OK, MessageBoxImage.Information);
                            Error = true;
                        }

                        if (!(DateTime.TryParse(game.Date_Purchased, out DateTime purchased)))
                        {
                            if (game.Date_Purchased != string.Empty)
                            {
                                MessageBox.Show("Please enter in date in mm/dd/yyyy or remove the date", "Purchase Date error", MessageBoxButton.OK, MessageBoxImage.Information);
                                Error = true;
                            }
                        }
                    }
                }
                if (!Error)
                {
                    Logic.UpdateGames(GamesList);
                    GetGameCollections();
                    cbx_SearchByTitle.SelectedIndex = -1;
                    dtg_GameList.IsReadOnly = true;
                    cbx_SearchByTitle.Text = string.Empty;
                    cmd_SaveEdits.IsEnabled = false;
                    grb_Search.IsEnabled = true;
                    cmd_editMode.IsEnabled = true;
                    MessageBox.Show("Changes have been saved", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
           
        }

        /// <summary>
        /// Searches for a specifics game in the database and if not found catches the exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(cbx_SearchByTitle.SelectedIndex == -1 && cbx_SearchByTitle.Text == ""))
                {
                    clsGame SelectedGame = (clsGame)cbx_SearchByTitle.SelectedItem;
                    dtg_GameList.ItemsSource = Logic.GetSingleGame(SelectedGame.ID);
                    cmd_RemoveFilter.IsEnabled = true;
                    cmd_Delete.IsEnabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Game could not found\nPlease try again.", "Game not found", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        /// <summary>
        /// Sets filter1 and filter2 to having no selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cbx_FilterOptions.SelectedIndex = -1;
                cbx_Filter2Options.SelectedIndex = -1;
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Creates and opens the wndPlayList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenListWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                wndPlayList PlayListWindow = new wndPlayList(database, SQLStatements, errorHandler);
                PlayListWindow.ShowDialog();
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// If enter is pressed in the search bar it will call cmd_Search_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterPressed(object sender, KeyEventArgs e)
        {

            try
            {
                if (cbx_SearchByTitle.Text != string.Empty)
                {
                    if (e.Key == Key.Enter)
                    {
                        cmd_Search_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
       
        }

        /// <summary>
        /// Enables delete button if somethings is clicked in the dtg_GameList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableDelete(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtg_GameList.SelectedIndex != -1)
                {
                    cmd_Delete.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Verifies with the user they want to delete the game if so calls Logic to do so
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsGame game = dtg_GameList.SelectedItem as clsGame;

                if (game != null)
                {
                    MessageBoxResult answer = MessageBox.Show("Are you sure you want to delete the selected game?", "Confirmation",
                                                                      MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (answer == MessageBoxResult.Yes)
                    {
                        Logic.DeleteGame(game);
                        cmd_Delete.IsEnabled = false;
                        GetGameCollections();
                    }
                }
            }
            catch (Exception ex)
            {

                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        /// <summary>
        /// Creates and opens wndSettings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSettingWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                wndSettings SettingsWindow = new wndSettings(database, SQLStatements, errorHandler);
                SettingsWindow.ShowDialog();
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
