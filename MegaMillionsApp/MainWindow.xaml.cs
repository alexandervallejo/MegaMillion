using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        class LotteryNumberSets           

        {
            public string Numbers { get; set; }
            public string MegaBall { get; set; }
            public string NumOfTimes { get; set; }
            public string LastWon { get; set;}
        }

        private void Lottery_Loaded(object sender, RoutedEventArgs e)
        {
            CSVManipulation SortedLists = new CSVManipulation();
            string[] displayDates = SortedLists.SortedDates();
            string[] displayNumbers = SortedLists.SortedNumbers();
            string[] displayMegaBall = SortedLists.SortedMegaBall();

            for (var sizeOfArrays = displayDates.Count()-1; sizeOfArrays > 0; --sizeOfArrays)
            {
                LotteryNumbers.Items.Add(new LotteryNumberSets() { Numbers = displayNumbers[sizeOfArrays], MegaBall = displayMegaBall[sizeOfArrays], NumOfTimes = "NA", LastWon = displayDates[sizeOfArrays] });
            }
        }

        private void Ascending_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //LotteryNumbers.Items.Re
        }

        private void Descending_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
