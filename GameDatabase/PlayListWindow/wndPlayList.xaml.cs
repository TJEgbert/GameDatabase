using GameDatabase.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GameDatabase.PlayListWindow
{
    /// <summary>
    /// Interaction logic for wndPlayList.xaml
    /// </summary>
    public partial class wndPlayList : Window
    {
        /// <summary>
        /// Holds the clsDataAccess object to pass around
        /// </summary>
        public clsDataAccess database;

        /// <summary>
        /// Holds the SQLStatements to pass to clsGameAddLogic
        /// </summary>
        public clsSQLStatements SQLStatements;

        /// <summary>
        /// The Logic used in the wndPlayList
        /// </summary>
        public clsPlayListLogic Logic;

        /// <summary>
        /// Holds the errorHandler handles general exceptions
        /// </summary>
        public clsErrorHandler errorHandler;

        /// <summary>
        /// Holds the collection of clsPlayListGame
        /// </summary>
        private ObservableCollection<clsPlayListGame> PlayList;

        /// <summary>
        /// Holds the collection of clsPlayListGame
        /// </summary>
        private ObservableCollection<clsGame> GameList;

        /// <summary>
        /// The number used to assign PlayOrder
        /// </summary>
        private int NumberOfGames;

        /// <summary>
        /// Constructor for the wndPlayList call logic to get the window set up
        /// </summary>
        /// <param name="Database">clsDataAccess to pass along to clsPlayListLogic</param>
        /// <param name="SQL">clsSQLStatements to pass along to clsPlayListLogic</param>
        public wndPlayList(clsDataAccess Database, clsSQLStatements SQL, clsErrorHandler ErrorHandler)
        {
            try
            {
                InitializeComponent();

                database = Database;
                SQLStatements = SQL;
                errorHandler = ErrorHandler;
                Logic = new clsPlayListLogic(Database, SQL);

                GameList = new ObservableCollection<clsGame>();
                GameList = Logic.GetGamesList();
                dtg_GameList.ItemsSource = GameList;

                PlayList = new ObservableCollection<clsPlayListGame>();
                PlayList = Logic.GetPlayOrderList();
                dtg_GameOrder.ItemsSource = PlayList;


                foreach (clsPlayListGame game in PlayList)
                {
                    int HighestNum = int.Parse(game.PlayOrder);

                    if (HighestNum > NumberOfGames)
                    {
                        NumberOfGames = HighestNum;
                    }
                }

                if (PlayList.Count > 0)
                {
                    cmd_Remove.IsEnabled = true;
                    cmd_Edit.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Adds a game to PlayList and updates the window accordingly 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_AddGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_GameList.SelectedItem != null)
                {
                    NumberOfGames++;
                    clsGame SelectedGame = dtg_GameList.SelectedItem as clsGame;
                    clsPlayListGame GameOrderItem = new clsPlayListGame();
                    GameOrderItem.PlayOrder = NumberOfGames.ToString();
                    GameOrderItem.GameID = int.Parse(SelectedGame.ID);
                    GameOrderItem.Title = SelectedGame.Title;

                    PlayList.Add(GameOrderItem);
                    cmd_Remove.IsEnabled = true;
                    cmd_Edit.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        /// <summary>
        /// Removes a game from Playlist and updates the window accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_GameOrder.SelectedItem != null)
                {
                    clsPlayListGame SelectedGame = dtg_GameOrder.SelectedItem as clsPlayListGame;
                    PlayList.Remove(SelectedGame);
                    NumberOfGames--;

                    if (PlayList.Count == 0)
                    {
                        cmd_Remove.IsEnabled = false;
                        cmd_Edit.IsEnabled = false;
                        NumberOfGames = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        /// <summary>
        /// Allows the user to edits the clsPlayListGame directly from the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtg_GameOrder.IsReadOnly = false;
                cmd_Save.IsEnabled = true;
                NumberOfGames--;
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Verifies everything that needs to field and and if so it updates the window accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool Error = false;

                foreach (clsPlayListGame game in PlayList)
                {
                    if (game.BeenEdited)
                    {
                        if (game.Title == string.Empty)
                        {
                            MessageBox.Show("Please fill out the title information", "Title Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                            Error = true;
                        }

                        if (!(int.TryParse(game.PlayOrder, out int intPlayOrder)))
                        {
                            MessageBox.Show("Play Order must contain a number", "Wrong type", MessageBoxButton.OK, MessageBoxImage.Error);
                            Error = true;
                        }
                    }
                }
                if (!Error)
                {
                    cmd_Save.IsEnabled = false;
                    MessageBox.Show("Your game playlist order has been saved", "Order Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                    foreach (clsPlayListGame game in PlayList)
                    {
                        game.BeenEdited = false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Marks the game as been edited in the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameBeenEdited(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                clsPlayListGame game = dtg_GameOrder.SelectedItem as clsPlayListGame;
                game.BeenEdited = true;
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

        /// <summary>
        /// Calls Logic to save the PlaList to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdatePlaylist(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (cmd_Save.IsEnabled)
                {
                    e.Cancel = true;
                    MessageBox.Show("Please save your playlist order", "Save Order", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Logic.SaveOrderList(PlayList);
                    e.Cancel = false;
                }
            }
            catch (Exception ex)
            {
                errorHandler.logError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
