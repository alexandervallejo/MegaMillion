using MegaMillion.DB;
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
        SortedLists _SortedList = new SortedLists(Properties.Settings.Default.ConnectionString);
        private bool _dataEmpty = true;
        private string[] _dataResponse = new string[] { };

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Lottery_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MegaBallGrid.DataContext = _SortedList.SortedNumbers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lottery_Loaded");
            }
        }

        private void DispalyLotteryNumbersList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MegaBallGrid.DataContext = _SortedList.SortedNumbers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DispalyLotteryNumbersList_Click");
            }
        }

        private void DisplayNumbersPercentages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MegaBallGrid.DataContext = _SortedList.HighestPercentWinningNumbersMid2013();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DisplayNumbersPercentages_Click");
            }
        }

        private void DisplayMegaBallPercentages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MegaBallGrid.DataContext = _SortedList.HighPercentMegaBallNumberMid2013();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DisplayMegaBallPercentages_Click");
            }
        }


        private void DisplayHighestPickedNumbers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _SortedList.HighestPercentChoosen = true;
                MegaBallGrid.DataContext = _SortedList.HighestWinningRatePicks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DisplayHighestPickedNumbers_Click");
            }
        }

        private void DisplayLowestPickedNumbers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _SortedList.HighestPercentChoosen = false;
                MegaBallGrid.DataContext = _SortedList.HighestWinningRatePicks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DisplayLowestPickedNumbers_Click");
            }
        }
    }
}
