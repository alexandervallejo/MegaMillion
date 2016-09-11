using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using FastMember;

namespace MegaMillionsApp
{
    public class SortedLists
    {
        private string ColumnHeadingI = "Winning Numbers(Sorted)";
        private string ColumnHeadingII = "Mega Ball";
        private string ColumnHeadingIII = "Most Recent Winning Date";
        private string ColumnHeadingIV = "Number of Time Won";

        private DataTable returnTable { get; set; }

        public static bool HasInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public string[] GetCSV(string _url)
        {
            string fileList = null;
            string[] tempStr;
            List<string> splitted = new List<string>();
            if(HasInternetConnection())
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    using (var sr = new StreamReader(resp.GetResponseStream()))
                    {
                        fileList = sr.ReadToEnd();
                        sr.Close();
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
            }
            else
            {
                var main = App.Current.MainWindow as MainWindow;
                main.NoInternet();
                Process.GetCurrentProcess().Kill();
            }

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

        public DataTable UnsortedNumbers(string [] listOfWinners)
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

        public DataTable SortedNumbers(string[] listOfWinners)
        {
            UnsortedNumbers(listOfWinners);
            CountAndDeleteRepeatedWinners(returnTable);
            ConvertToDates(returnTable);
            SortWinningNumbers(returnTable);
            return returnTable;
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
        
        public static int Return2013Index(string[] listOfWinners)
        {
            //  Minus 2 inorder to keep check year in array. 
            //  [Month, Day, Year, 1st #, 2nd #, 3rd #, 4th #, 5th #, PowerBall, Unused]
            int indexOf2013 = Array.IndexOf(listOfWinners, "2013") - 2;

            //Hit the first Month: 10   and Day: 15 Year: 2013
            //Mega Million changes to 1-75 Winning Numbers and 1-15 MegaBall
            for (int endingIndexOf2013 = indexOf2013; listOfWinners.Count() > endingIndexOf2013; ++endingIndexOf2013)
            {
                if (listOfWinners[endingIndexOf2013] == "10" && listOfWinners[endingIndexOf2013 + 1] == "15")
                {
                    indexOf2013 = endingIndexOf2013;
                    break;
                }
            }
            return indexOf2013;
        }

        public DataTable HighestPercentWinningNumbersMid2013(string[] listOfWinners)
        {
            DataTable DisplayWinningPercentages = new DataTable();
            List<int> megaMillionNumbers = new List<int>();
            DisplayWinningPercentages.Columns.Add("MegaMillion Number", typeof(string));
            DisplayWinningPercentages.Columns.Add("Percentage Of Winning", typeof(string));


            for (int arrayIndex = listOfWinners.Count(); arrayIndex > Return2013Index(listOfWinners); arrayIndex = arrayIndex - 10)
            {
                megaMillionNumbers.Add(int.Parse(listOfWinners[arrayIndex - 7]));
                megaMillionNumbers.Add(int.Parse(listOfWinners[arrayIndex - 6]));
                megaMillionNumbers.Add(int.Parse(listOfWinners[arrayIndex - 5]));
                megaMillionNumbers.Add(int.Parse(listOfWinners[arrayIndex - 4]));
                megaMillionNumbers.Add(int.Parse(listOfWinners[arrayIndex - 3]));
            }

            //Divide Each Number in List by count of List.
            for (int megaNumber = 1; megaNumber <= 75; ++megaNumber)
            {
                //Create Percent of MegaNumber
                double inputValue = ((double)megaMillionNumbers.Count(x => x == megaNumber) / (double)megaMillionNumbers.Count()) * 100.0;

                inputValue = Math.Round(inputValue, 2);

                DataRow returnRows = DisplayWinningPercentages.NewRow();
                //Winning Number
                returnRows["MegaMillion Number"] = megaNumber;

                //Winning Percentage
                returnRows["Percentage Of Winning"] = inputValue.ToString() + "%";

                DisplayWinningPercentages.Rows.Add(returnRows);
            }
           
            return DisplayWinningPercentages;
        }

        public DataTable HighPercentMegaBallNumberMid2013(string[] listOfWinners)
        {
            DataTable DisplayMegaBallPercentages = new DataTable();
            List<int> megaBallNumbers = new List<int>();
            DisplayMegaBallPercentages.Columns.Add("MegaBall Number", typeof(string));
            DisplayMegaBallPercentages.Columns.Add("Percentage Of Winning", typeof(string));
            for (int arrayIndex = listOfWinners.Count(); arrayIndex > Return2013Index(listOfWinners); arrayIndex = arrayIndex - 10)
            {
                megaBallNumbers.Add(int.Parse(listOfWinners[arrayIndex - 2]));
            }

            //Divide Each Number in List by count of List.
            for (int megaNumber = 1; megaNumber <= 15; ++megaNumber)
            {
                //Create Percent of MegaNumber
                double inputValue = ((double)megaBallNumbers.Count(x => x == megaNumber) / (double)megaBallNumbers.Count())*100.0;

                inputValue = Math.Round(inputValue, 2);

                DataRow returnRows = DisplayMegaBallPercentages.NewRow();
                //Winning Number
                returnRows["MegaBall Number"] = megaNumber;

                //Winning Percentage
                returnRows["Percentage Of Winning"] = inputValue.ToString() + "%";

                DisplayMegaBallPercentages.Rows.Add(returnRows);
            }

            return DisplayMegaBallPercentages;
        }

        public DataTable HighestWinningRatePicks(string[] listOfWinners)
        {
            DataTable numbersTable = HighestPercentWinningNumbersMid2013(listOfWinners);
            DataTable megaBallNumbersTable = HighPercentMegaBallNumberMid2013(listOfWinners);
            DataTable displayWinningPicks = new DataTable();
            // Declare DataColumn and DataRow variables.
            DataColumn pickedWinningNumbersCol;
            DataRow pickedWinningNumbersRow;
            pickedWinningNumbersCol = new DataColumn();
            displayWinningPicks.Columns.Add("Picked Winning Numbers", typeof(string));
            displayWinningPicks.Columns.Add("Picked Winning Mega Ball", typeof(string));

            Dictionary<int, double> numberPercents = new Dictionary<int, double>();
            Dictionary<int, double> megaBallNumberPercents = new Dictionary<int, double>();

            List<string> winningNumbers = new List<string>();
            List<string> numberPercentsSorted = new List<string>();
            List<string> megaBallNumberPercentsSorted = new List<string>();

            
            
            int key = 0;
            int megaBall = 1;
            int numberStart = 1;
            int wrappingStart = 1;
            int numberReStart = 0;
            int wrappingReStart = 1;
            double value = 0.0;
            string checkPickedNumbers, newCheckPickedNumbers = null;
            string[] sortNumsToCompare = new string[] { };
            
            // Grab percents for the 5 winning numbers, percentages.
            foreach (DataRow percents in numbersTable.Rows)
            {
                key = int.Parse(percents[0].ToString());
                value = double.Parse(percents[1].ToString().Trim('%'));
                numberPercents.Add(key, value);
            }

            // Grab percents for the mega ball numbers, percentages.
            foreach (DataRow percents in megaBallNumbersTable.Rows)
            {
                key = int.Parse(percents[0].ToString());
                value = double.Parse(percents[1].ToString().Trim('%'));
                megaBallNumberPercents.Add(key, value);
            }

            for (int arrayIndex = listOfWinners.Count(); arrayIndex > 0; arrayIndex = arrayIndex - 10)
            {
                //Grab 5 Winning Numbers and seperate by comma to sort later
                string[] sortValues = { listOfWinners[arrayIndex - 7], listOfWinners[arrayIndex - 6], listOfWinners[arrayIndex - 5], listOfWinners[arrayIndex - 4], listOfWinners[arrayIndex - 3], listOfWinners[arrayIndex - 2] };
                Array.Sort(sortValues);
                winningNumbers.Add(sortValues[0] + "," + sortValues[1] + "," + sortValues[2] + ","+ sortValues[3] + "," + sortValues[4]);

            }

            foreach (KeyValuePair<int, double> numbers in numberPercents.OrderByDescending(keys => keys.Value))
            {
                numberPercentsSorted.Add(numbers.Key.ToString());
            }

            foreach (KeyValuePair<int, double> megaBalls in megaBallNumberPercents.OrderByDescending(keys => keys.Value))
            {
                megaBallNumberPercentsSorted.Add(megaBalls.Key.ToString());
            }

            sortNumsToCompare = numberPercentsSorted.Take(5).ToArray();
            Array.Sort(sortNumsToCompare);
            checkPickedNumbers = (string.Join(",", sortNumsToCompare) + "," + 
                                  string.Join(",",megaBallNumberPercentsSorted.Take(1).ToArray()));
            Array.Clear(sortNumsToCompare,0,5);

            sortNumsToCompare = numberPercentsSorted.ToList().GetRange(1, 5).ToArray();
            Array.Sort(sortNumsToCompare);
            newCheckPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                              string.Join(",", megaBallNumberPercentsSorted.Take(1).ToArray()));
            Array.Clear(sortNumsToCompare, 0, 5);

            
            //Tried all first megaBall numbers start with 2 when cycling through
            for (int totalArrayCount = 0; totalArrayCount < numberPercents.Count(); ++totalArrayCount)
            {
               if(!(winningNumbers.Any(str => str.Contains(checkPickedNumbers))) && checkPickedNumbers != newCheckPickedNumbers)
                {
                    
                    //Add List of picked numbers upto 100 break after 100.
                    if (displayWinningPicks.Rows.Count < 100)
                    {
                        pickedWinningNumbersRow = displayWinningPicks.NewRow();
                        pickedWinningNumbersRow["Picked Winning Numbers"] = checkPickedNumbers;
                        displayWinningPicks.Rows.Add(pickedWinningNumbersRow);

                        Console.WriteLine(checkPickedNumbers);
                        checkPickedNumbers = newCheckPickedNumbers;
                        newCheckPickedNumbers = "";
                    }
                    else
                    {
                        return ParsedPickedNumbers(displayWinningPicks); ;
                    }
                }
                else
                {
                    totalArrayCount = 0;
                    if (numberStart < numberPercents.Count()-6)
                    {

                        sortNumsToCompare = numberPercentsSorted.ToList().GetRange(numberStart, 5).ToArray();
                        Array.Sort(sortNumsToCompare);
                        checkPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                              string.Join(",", megaBallNumberPercentsSorted.ToList().
                                              GetRange(megaBall, 1).ToArray()));
                        Array.Clear(sortNumsToCompare, 0, 5);


                        sortNumsToCompare = numberPercentsSorted.ToList().GetRange(numberStart + 1, 5).ToArray();
                        Array.Sort(sortNumsToCompare);
                        newCheckPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                                string.Join(",", megaBallNumberPercentsSorted.ToList().GetRange(megaBall, 1).ToArray()));
                        Array.Clear(sortNumsToCompare, 0, 5);
                                              
                        ++numberStart;
                    }
                    else
                    {
                        if(wrappingStart < 4)
                        {
                            sortNumsToCompare = numberPercentsSorted.ToList().GetRange(numberStart, 5 - wrappingStart).
                                    Concat(numberPercentsSorted.ToList().GetRange(0, wrappingStart)).ToArray();

                            Array.Sort(sortNumsToCompare);
                            checkPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                              string.Join(",", megaBallNumberPercentsSorted.ToList().
                                              GetRange(megaBall, 1).ToArray()));
                            Array.Clear(sortNumsToCompare, 0, 5);


                            sortNumsToCompare = numberPercentsSorted.ToList().GetRange(numberStart + 1, 5 - wrappingStart).
                                 Concat(numberPercentsSorted.ToList().GetRange(0, wrappingStart)).ToArray();
                            Array.Sort(sortNumsToCompare);
                            newCheckPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                                    string.Join(",", megaBallNumberPercentsSorted.ToList().GetRange(megaBall, 1).ToArray()));
                            Array.Clear(sortNumsToCompare, 0, 5);

                            ++wrappingStart;
                        }
                        else
                        {
                            if (megaBall < megaBallNumberPercentsSorted.Count()-1)
                            {
                                //Choose top 5 number percentage numbers with different megaball.
                                ++megaBall;
                                numberStart = 0;
                                wrappingStart = 0;
                            }
                            else
                            {
                                //Run trhough all numbers with different Megaballs megaball while it runs through 
                                foreach (string megaBallRestart in megaBallNumberPercentsSorted)
                                {      
                                    if (numberReStart < numberPercents.Count() - 6)
                                    {
                                        sortNumsToCompare = numberPercentsSorted.ToList().GetRange(numberReStart, 5).ToArray();
                                        Array.Sort(sortNumsToCompare);
                                        checkPickedNumbers = string.Join(",", sortNumsToCompare) + "," + int.Parse(megaBallRestart);
                                        Array.Clear(sortNumsToCompare, 0, 5);


                                        sortNumsToCompare = numberPercentsSorted.ToList().GetRange(numberReStart + 1, 5).ToArray();
                                        Array.Sort(sortNumsToCompare);
                                        newCheckPickedNumbers = (string.Join(",", sortNumsToCompare) + "," + int.Parse(megaBallRestart));
                                        Array.Clear(sortNumsToCompare, 0, 5);
                                        
                                    }
                                    else
                                    {
                                        if (wrappingReStart < 4)
                                        {
                                            sortNumsToCompare = numberPercentsSorted.ToList().
                                                                GetRange(numberStart, 5 - wrappingReStart).Concat(numberPercentsSorted.ToList().
                                                                GetRange(0, wrappingReStart)).ToArray();
                                            Array.Sort(sortNumsToCompare);
                                            checkPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                                              string.Join(",", megaBallNumberPercentsSorted.ToList().GetRange(megaBall, 1).ToArray()));
                                            Array.Clear(sortNumsToCompare, 0, 5);


                                            sortNumsToCompare = numberPercentsSorted.ToList().GetRange(numberStart + 1, 5 - wrappingReStart).
                                                                Concat(numberPercentsSorted.ToList().GetRange(0, wrappingReStart)).ToArray();
                                            Array.Sort(sortNumsToCompare);
                                            newCheckPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                                                    string.Join(",", megaBallNumberPercentsSorted.ToList().
                                                                                GetRange(megaBall, 1).ToArray()));
                                            Array.Clear(sortNumsToCompare, 0, 5);

                                            ++wrappingReStart;
                                        }
                                        if (!(winningNumbers.Any(str => str.Contains(checkPickedNumbers))))
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            checkPickedNumbers = "";
                                        }
                                    }
                                }
                                ++numberReStart;
                            }
                        }
                        
                    }
                }
            }
            return displayWinningPicks;
        }

        DataTable ParsedPickedNumbers(DataTable PickedNumbers)
        {
            int n = 0;
            foreach(DataRow Numbers in PickedNumbers.Rows)
            {
                string temp = Numbers[0].ToString();
                PickedNumbers.Rows[n][0] = Numbers[0].ToString().Substring(0, Numbers[0].ToString().LastIndexOf(','));
                PickedNumbers.Rows[n][1] = "[" + temp.Substring(temp.LastIndexOf(',')+1,temp.Length - temp.LastIndexOf(',') - 1) + "]";
                ++n;
            }

            return PickedNumbers;
        }
    }
}
