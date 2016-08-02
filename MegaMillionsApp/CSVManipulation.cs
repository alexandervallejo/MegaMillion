using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsApp
{
    class CSVManipulation
    {
        

        public List<string> GetCSV()
        {
            string fileList;
            string[] tempStr;
            List<string> splitted = new List<string>();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://txlottery.org/export/sites/lottery/Games/Mega_Millions/Winning_Numbers/megamillions.csv");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            fileList = sr.ReadToEnd();
            sr.Close();
            tempStr = fileList.Replace("Mega Millions", "").Split(',');
            tempStr = tempStr.Select(x => x.Replace("\r\n", "")).ToArray();
            foreach (string MegaNumbers in tempStr)
            {
                if (!string.IsNullOrWhiteSpace(MegaNumbers))
                {
                    splitted.Add(MegaNumbers);
                }
            }
            return splitted;
        }

        /* Find date and time of 
            Grab 0-2 on a 0 index to create the Date
            Grab 3-7 to create winning Mega Million Numbers
            Grab 8 for power ball
        */
        public string[] SortedDates()
        {      
            string[] listOfWinners = GetCSV().ToArray();
            int sizeOfStrings = listOfWinners.Count() / 10;
            int dateArrayCount = 0;
            string[] dates = new string[sizeOfStrings];

            for (int arrayIndex = 0; arrayIndex < listOfWinners.Count(); arrayIndex = arrayIndex + 10)
            {
                //Month                          // Day                             //Year
                string temp = "(" + listOfWinners[arrayIndex + 2] + "," + listOfWinners[arrayIndex] + "," + listOfWinners[arrayIndex + 1] + ")";
                dates[dateArrayCount] = temp;
                ++dateArrayCount;
            }
            return dates;
        }

        public string[] SortedNumbers()
        {
            string[] listOfWinners = GetCSV().ToArray();
            int numbersArrayCount = 0;
            int sizeOfStrings = listOfWinners.Count() / 10;
            string[] winningNumbers = new string[sizeOfStrings];

            for (int arrayIndex = 0; arrayIndex < listOfWinners.Count(); arrayIndex = arrayIndex + 10)
            {
                string temp = "[" + listOfWinners[arrayIndex + 3] + " " + listOfWinners[arrayIndex + 4] + " " + listOfWinners[arrayIndex + 5] + " " + listOfWinners[arrayIndex + 6] + " " + listOfWinners[arrayIndex + 7] + "]";
                winningNumbers[numbersArrayCount] = temp;
                ++numbersArrayCount;
            }
            return winningNumbers;
        }

        public string[] SortedMegaBall()
        {
            string[] listOfWinners = GetCSV().ToArray();
            int sizeOfStrings = listOfWinners.Count() / 10;
            int megaArrayCount = 0;
            string[] megaBall = new string[sizeOfStrings];

            for (int arrayIndex = 0; arrayIndex < listOfWinners.Count(); arrayIndex = arrayIndex + 10)
            {
                megaBall[megaArrayCount] = listOfWinners[arrayIndex + 8];
                ++megaArrayCount;
            }
            return megaBall;
        }
    }
}
