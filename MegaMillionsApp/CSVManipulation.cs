using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace MegaMillionsApp
{
    public class CSVManipulation
    {
        private string ColumnHeadingI = "Winning Numbers(Sorted)";
        private string ColumnHeadingII = "Mega Ball";
        private string ColumnHeadingIII = "Most Recent Winning Date";
        private string ColumnHeadingIV = "Number of Time Won";

        private DataTable returnTable { get; set; }

        public string[] GetCSV(string _url)
        {
            string fileList;
            string[] tempStr;
            List<string> splitted = new List<string>();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            fileList = sr.ReadToEnd();
            sr.Close();
            //Removes 1st column in CSV.
            tempStr = fileList.Replace("Mega Millions", "").Split(',');
            //Removes newline feed charcters created by .Split
            tempStr = tempStr.Select(x => x.Replace("\r\n", "")).ToArray();
            //Adds each stirng into a new List.
            foreach (string MegaNumbers in tempStr)
            {
                if (!string.IsNullOrWhiteSpace(MegaNumbers))
                {
                    splitted.Add(MegaNumbers);
                }
            }
            //Returns a converted List to Array.
            return splitted.ToArray();
        }

        /*  
            Data comes in latest to earliest, so we read from last to first.
            Use 10 numbers at a time, read right to left.
            [Month, Day, Year, 1st #, 2nd #, 3rd #, 4th #, 5th #, PowerBall, Unused]  
            Grab 0-2 on a 0 index to create the Date
            Grab 3-7 to create winning Mega Million Numbers
            Grab 8 for power ball
            Inex 9 is unused (MegaPiler 1-5)
        */
        public DataTable SortedNumbers(string [] listOfWinners)
        {
            returnTable = new DataTable();
            returnTable.Columns.Add(ColumnHeadingI, typeof(string));
            returnTable.Columns.Add(ColumnHeadingII, typeof(string));
            returnTable.Columns.Add(ColumnHeadingIII, typeof(string));
            returnTable.Columns.Add(ColumnHeadingIV, typeof(string));

            //Count Down to bring newest winning Numbers First
            for (int arrayIndex = listOfWinners.Count(); arrayIndex > 0; arrayIndex = arrayIndex - 10)
            {
                //Create Table of 4 Columns and Winning Number of Rows
                DataRow returnRows = returnTable.NewRow();

                //Grab 5 Winning Numbers and seperate by comma to sort later
                returnRows[ColumnHeadingI] = listOfWinners[arrayIndex - 7] + ',' + listOfWinners[arrayIndex - 6] + ',' + listOfWinners[arrayIndex - 5] + ',' + listOfWinners[arrayIndex - 4] + ',' + listOfWinners[arrayIndex - 3];

                //Mega Balls
                returnRows[ColumnHeadingII] = listOfWinners[arrayIndex - 2];

                //Dates                                   //Year                          // Month                             //Day
                returnRows[ColumnHeadingIII] = listOfWinners[arrayIndex - 8] + "-" + listOfWinners[arrayIndex - 10] + "-" + listOfWinners[arrayIndex - 9];

                returnTable.Rows.Add(returnRows);
            }
            CountAndDeleteRepeatedWinners(returnTable);
            ConvertToDates(returnTable);
            SortWinningNumbers(returnTable);
            return returnTable;
        }

        private void CountAndDeleteRepeatedWinners(DataTable returnTable)
        {
            List<DataRow> rowsToDelete = new List<DataRow>();
            //Shows How Many Winning Numbers are repeated.
            foreach (DataRow dr in returnTable.Rows)
            {
                int repeatedCount = 0;
                for (int rowNumber = returnTable.Rows.Count-1; rowNumber > -1; --rowNumber)
                {
                    if (returnTable.Rows[rowNumber][ColumnHeadingI].ToString() == dr[ColumnHeadingI].ToString() &&
                       returnTable.Rows[rowNumber][ColumnHeadingII].ToString() == dr[ColumnHeadingII].ToString())
                    {
                        ++repeatedCount;
                        returnTable.Rows[rowNumber][ColumnHeadingIV] = repeatedCount.ToString();
                        if(repeatedCount > 1)
                        {
                            rowsToDelete.Add(dr);
                        }
                            
                    }
                }
            }
            
            foreach (DataRow rowNumber in rowsToDelete.Distinct().ToList().Skip(1))
            {
                returnTable.Rows.Remove(rowNumber);
            }
        }

        public void ConvertToDates(DataTable returnTable)
        {
            int arrayCount = 0;
            //Formats Column 3 (Date) to be Date Time for easier sorting.
            foreach (DataRow dr in returnTable.Rows)
            {
                DateTime date = DateTime.Parse(dr[ColumnHeadingIII].ToString());
                returnTable.Rows[arrayCount][ColumnHeadingIII] = date.ToString("yyyy-MM-dd");
                ++arrayCount;
            }
        }

        private void SortWinningNumbers(DataTable returnTable)
        {
            int arrayCount = 0;
            //Formats Column 1(Winning Numbers) to Sorted Integer Array for easier sorting.
            foreach (DataRow dr in returnTable.Rows)
            {
                string winnningNumbers = dr[ColumnHeadingI].ToString();
                int[] intArray = winnningNumbers.Split(',').Select(int.Parse).ToArray();
                Array.Sort(intArray);
                returnTable.Rows[arrayCount][ColumnHeadingI] = returnTable.Rows[arrayCount][ColumnHeadingI] = "[" + intArray[0] + " - " + intArray[1] + " - " + intArray[2] + " - " + intArray[3] + " - " + intArray[4] + "]";
                ++arrayCount;
            }
        }

        public static Dictionary<int,int> HighestPercentWinningNumbersMid2013()
        {
            Dictionary<int, int> blank = new Dictionary<int, int>();
            blank.Add(0, 0);
            return blank;
        }

        public static Dictionary<int, int> HighPercentPowerBallNumberMid2013()
        {
            Dictionary<int,int> blank  = new Dictionary<int, int>();
            blank.Add(0, 0);
            return blank;
        }
    }
}
