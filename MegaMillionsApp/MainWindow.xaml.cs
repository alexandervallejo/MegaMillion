using System;
using System.Collections.Generic;
using System.Data;
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

        private int sortColumn = -1;


        //private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        //{
        //    // Determine whether the column is the same as the last column clicked.
        //    if (e.Column != sortColumn)
        //    {
        //        // Set the sort column to the new column.
        //        sortColumn = e.Column;
        //        // Set the sort order to ascending by default.
        //        listView1.Sorting = SortOrder.Ascending;
        //    }
        //    else
        //    {
        //        // Determine what the last sort order was and change it.
        //        if (listView1.Sorting == SortOrder.Ascending)
        //            listView1.Sorting = SortOrder.Descending;
        //        else
        //            listView1.Sorting = SortOrder.Ascending;
        //    }

        //    // Call the sort method to manually sort.
        //    listView1.Sort();
        //    // Set the ListViewItemSorter property to a new ListViewItemComparer
        //    // object.
        //    this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column,
        //                                                      listView1.Sorting);
        //}

        class LotteryNumberSets           

        {
            public string WinningNumbersPercent { get; set; }
            public string MegaBallPercent { get; set; }
        }

        private void Lottery_Loaded(object sender, RoutedEventArgs e)
        {
            CSVManipulation SortedLists = new CSVManipulation();
            string[] dataResponse = SortedLists.GetCSV("http://txlottery.org/export/sites/lottery/Games/Mega_Millions/Winning_Numbers/megamillions.csv");
            DataTable megaBallGrid = SortedLists.SortedNumbers(dataResponse);
            MegaBallGrid.DataContext = megaBallGrid.DefaultView;

            //string[,] allMegaMillionValues = SortedLists.SortedNumbers();
            //int numberOfColInArray = 3;

            //for (var sizeOfArrays = 0; (allMegaMillionValues.Length / numberOfColInArray) - 1 > sizeOfArrays; ++sizeOfArrays)
            //{
            //    LotteryNumbers.Items.Add(new LotteryNumberSets()
            //            { Numbers = allMegaMillionValues[0,sizeOfArrays],
            //              MegaBall = allMegaMillionValues[2,sizeOfArrays],
            //              NumOfTimesNumbers = allMegaMillionValues[3, sizeOfArrays],
            //              LastWon = allMegaMillionValues[4,sizeOfArrays] });
            //}
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
