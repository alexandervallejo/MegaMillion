using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            MegaBallGrid.DataContext = megaBallGrid.DefaultView;
        }
    }
}
