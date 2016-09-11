using System;
using System.Data;
using System.Windows;

namespace MegaMillionsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// To-Do:
    /// Add Unit Tests
    /// Add Exceptions
    /// Add Try Catches
    /// Have an algotrigthm guess Mega Million Winner (Most likely value building off each other)
    /// Add Ping to email if I won the Lottery
    /// Create backup incase deleted 
    /// </summary>
    public partial class MainWindow : Window
    {
        SortedLists SortedList = new SortedLists();
        private bool _dataEmpty = true;
        private string[] _dataResponse = new string[] { };
        

        public MainWindow()
        {
            InitializeComponent();

        }

        public void NoInternet()
        {
            const string message =
            "There seems to be no internet please try again later.";
            const string caption = "No Internet";
            var result = System.Windows.Forms.MessageBox.Show(message, caption);
        }

        public string[] returnCSVFile()
        {

            if (_dataEmpty)
            {
                _dataResponse = SortedList.GetCSV("http://txlottery.org/export/sites/lottery/Games/Mega_Millions/Winning_Numbers/megamillions.csv");
                _dataEmpty = false;
            }
            if (_dataResponse == null)
            {
                throw new ArgumentException("Data is gone FIND IT !");
            }

            return _dataResponse;
        }

        private void Lottery_Loaded(object sender, RoutedEventArgs e)
        {
            string[] stringData = returnCSVFile();

            DataTable megaBallGrid = SortedList.SortedNumbers(stringData);

            MegaBallGrid.DataContext = megaBallGrid.DefaultView;

        }

        private void DisplayNumbersPercentages_Click(object sender, RoutedEventArgs e)
        {
            string[] stringData = returnCSVFile();

            DataTable megaBallGrid = SortedList.HighestPercentWinningNumbersMid2013(stringData);

            MegaBallGrid.DataContext = megaBallGrid.DefaultView;
        }

        private void DisplayMegaBallPercentages_Click(object sender, RoutedEventArgs e)
        {
            string[] stringData = returnCSVFile();

            DataTable megaBallGrid = SortedList.HighPercentMegaBallNumberMid2013(stringData);

            MegaBallGrid.DataContext = megaBallGrid.DefaultView;
        }

        private void DisplayUnsortedList_Click(object sender, RoutedEventArgs e)
        {
            string[] stringData = returnCSVFile();

            DataTable megaBallGrid = SortedList.UnsortedNumbers(stringData);

            MegaBallGrid.DataContext = megaBallGrid.DefaultView;
        }

        private void DispalyOriginalList_Click(object sender, RoutedEventArgs e)
        {
            string[] stringData = returnCSVFile();

            DataTable megaBallGrid = SortedList.SortedNumbers(stringData);

            DataTable fakeone = SortedList.HighestWinningRatePicks(stringData);

            MegaBallGrid.DataContext = megaBallGrid.DefaultView;
        }

        private void DisplayPickedNumbers_Click(object sender, RoutedEventArgs e)
        {

            string[] stringData = returnCSVFile();

            DataTable megaBallGrid = SortedList.HighestWinningRatePicks(stringData);

            MegaBallGrid.DataContext = megaBallGrid.DefaultView;
        }
    }
}
