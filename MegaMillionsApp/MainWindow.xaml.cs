using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Net;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MegaMillionsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            public int NumOfTimes { get; set; }
            public string LastWon { get; set;}
        }

        private void Lottery_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient web = new WebClient();

           web.DownloadFile("http://txlottery.org/export/sites/lottery/Games/Mega_Millions/Winning_Numbers/megamillions.csv", "MegaMillionNumbers");

        }

        private void Ascending_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Descending_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
