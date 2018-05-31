using log4net;
using MegaMillion.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillion.DB
{
    public class SortedLists
    {
        // Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private MegaMillionDB _DataBase;

        private DataTable returnTable { get; set; }
        private List<ILotteryNumber> _WinningNumbers;
        public bool HighestPercentChoosen = true;

        // Methods
        public SortedLists(string connectionString)
        {
            try
            {
                this._DataBase = new MegaMillionDB(connectionString);
                this._WinningNumbers = this._DataBase.GetLotteryNumbers();
            }
            catch(Exception ex)
            {
                log.Error("SortedLists", ex);
            }
        }

        public void ConvertToDates(DataTable returnTable)
        {
            int num = 0;
            foreach (DataRow row in returnTable.Rows)
            {
                returnTable.Rows[num][MegaMillionConsts.ColumnHeadingIII] = DateTime.Parse(row[MegaMillionConsts.ColumnHeadingIII].ToString()).ToShortDateString();
                num++;
            }
        }

        private void CountAndDeleteRepeatedWinners(DataTable returnTable)
        {
            {
                List<DataRow> rowsToDelete = new List<DataRow>();
                //Shows How Many Winning Numbers are repeated.
                foreach (DataRow dr in returnTable.Rows)
                {
                    int repeatedCount = 0;
                    for (int rowNumber = returnTable.Rows.Count - 1; rowNumber > -1; --rowNumber)
                    {
                        if (returnTable.Rows[rowNumber][MegaMillionConsts.ColumnHeadingI].ToString() == dr[MegaMillionConsts.ColumnHeadingI].ToString() &&
                           returnTable.Rows[rowNumber][MegaMillionConsts.ColumnHeadingII].ToString() == dr[MegaMillionConsts.ColumnHeadingII].ToString())
                        {
                            ++repeatedCount;
                            returnTable.Rows[rowNumber][MegaMillionConsts.ColumnHeadingIV] = repeatedCount.ToString();
                            if (repeatedCount > 1)
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
        }

        private Tuple<LotteryNumber, decimal, decimal> GetPredictedLotteryNumberFromDataTableRow(DataRow dataRow)
        {
            LotteryNumber lotteryNumberPrediction = new LotteryNumber();
            DateTime today = DateTime.Today;
            if ((((today.DayOfWeek == DayOfWeek.Tuesday) && (today.TimeOfDay > new TimeSpan(20, 5, 0))) || (today.DayOfWeek == DayOfWeek.Wednesday)) || ((today.DayOfWeek == DayOfWeek.Thursday) && (today.TimeOfDay < new TimeSpan(20, 0, 0))))
            {
                int num3 = ((DayOfWeek.Thursday - today.DayOfWeek) + 7) % 7;
                DateTime time3 = DateTime.Parse(today.AddDays((double)num3).ToShortDateString());
                lotteryNumberPrediction.DateWon = time3;
            }
            else
            {
                int num4 = ((DayOfWeek.Tuesday - today.DayOfWeek) + 7) % 7;
                DateTime time5 = DateTime.Parse(today.AddDays((double)num4).ToShortDateString());
                lotteryNumberPrediction.DateWon = time5;
            }

            var winningNumberArray = dataRow[MegaMillionConsts.ColumnHeaderPickedNumbers].ToString().Replace('[', ' ').Replace(']', ' ').Split(',');
            lotteryNumberPrediction.Number1 = int.Parse(winningNumberArray[0].ToString());
            lotteryNumberPrediction.Number2 = int.Parse(winningNumberArray[1].ToString());
            lotteryNumberPrediction.Number3 = int.Parse(winningNumberArray[2].ToString());
            lotteryNumberPrediction.Number4 = int.Parse(winningNumberArray[3].ToString());
            lotteryNumberPrediction.Number5 = int.Parse(winningNumberArray[4].ToString());
            lotteryNumberPrediction.MegaBall = int.Parse(dataRow[MegaMillionConsts.ColumnHeaderPickedMegaBall].ToString());
            var avgPickedWinRate = decimal.Parse(dataRow[MegaMillionConsts.ColumnHeaderPickedAvgWinPerc].ToString().Replace('%', ' '));
            var megaBallWinRate = decimal.Parse(dataRow[MegaMillionConsts.ColumnHeaderPickedMegaBallPerc].ToString().Replace('%', ' '));
            return new Tuple<LotteryNumber, decimal, decimal>(lotteryNumberPrediction, avgPickedWinRate, megaBallWinRate);
        }

        public IEnumerable<ILotteryNumber> GetMegaBallsAfterOctober152013() =>
            this._DataBase.GetUpdatedLotteryNumbers();

        public DataTable HighestPercentWinningNumbersMid2013()
        {
            DataTable DisplayWinningPercentages = new DataTable();
            DisplayWinningPercentages.Columns.Add(MegaMillionConsts.ColumnHeadingMegaMillionNumber, typeof(string));
            DisplayWinningPercentages.Columns.Add(MegaMillionConsts.ColumnHeaderPickedNumbers, typeof(string));

            //Divide Each Number in List by count of List.
            for (int megaNumber = 1; megaNumber <= 75; ++megaNumber)
            {
                //Create Percent of MegaNumber
                double inputValue = ((double)_WinningNumbers.Count(x => x.MegaBall == megaNumber) / (double)_WinningNumbers.Count()) * 100.0;

                inputValue = Math.Round(inputValue, 2);

                DataRow returnRows = DisplayWinningPercentages.NewRow();
                //Winning Number
                returnRows[MegaMillionConsts.ColumnHeadingMegaMillionNumber] = megaNumber;

                //Winning Percentage
                returnRows[MegaMillionConsts.ColumnHeaderPickedNumbers] = inputValue.ToString() + "%";

                DisplayWinningPercentages.Rows.Add(returnRows);
            }

            return DisplayWinningPercentages;
        }

        public DataTable HighestWinningRatePicks()
        {
            DataTable numbersTable = this.HighestPercentWinningNumbersMid2013();
            DataTable megaBallNumbersTable = this.HighPercentMegaBallNumberMid2013();
            DataTable displayWinningPicks = new DataTable();
            displayWinningPicks.Columns.Add(MegaMillionConsts.ColumnHeaderPickedNumbers, typeof(string));
            displayWinningPicks.Columns.Add(MegaMillionConsts.ColumnHeaderPickedAvgWinPerc, typeof(string));
            displayWinningPicks.Columns.Add(MegaMillionConsts.ColumnHeaderPickedMegaBall, typeof(string));
            displayWinningPicks.Columns.Add(MegaMillionConsts.ColumnHeaderPickedMegaBallPerc, typeof(string));
            Dictionary<int, double> numberPercents = new Dictionary<int, double>();
            Dictionary<int, double> megaBallNumberPercents = new Dictionary<int, double>();
            List<string> winningNumbers = new List<string>();
            List<int> numberPercentsSorted = new List<int>();
            List<int> megaBallNumberPercentsSorted = new List<int>();
            int key = 0;
            int wrappingStart = 1;
            int[] sortNumsToCompare = new int[] { };
            double value = 0.0;
            string checkPickedNumbers = null;

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

            foreach (ILotteryNumber number in this._WinningNumbers)
            {
                string[] strArray2 = new string[] { number.Number1.ToString(), number.Number2.ToString(), number.Number3.ToString(), number.Number1.ToString(), number.Number1.ToString() };
                Array.Sort<string>(strArray2);
                winningNumbers.Add(strArray2[0] + "," + strArray2[1] + "," + strArray2[2] + "," + strArray2[3] + "," + strArray2[4]);
            }
            if (this.HighestPercentChoosen)
            {
                foreach (KeyValuePair<int, double> numbers in from keys in numberPercents
                                                           orderby keys.Value descending
                                                           select keys)
                {
                    numberPercentsSorted.Add(numbers.Key);
                }
                foreach (KeyValuePair<int, double> megaBalls in from keys in megaBallNumberPercents
                                                            orderby keys.Value descending
                                                            select keys)
                {
                    megaBallNumberPercentsSorted.Add(megaBalls.Key);
                }
            }
            else
            {
                foreach (KeyValuePair<int, double> numbers in from keys in numberPercents
                                                            orderby keys.Value
                                                            select keys)
                {
                    numberPercentsSorted.Add(numbers.Key);
                }
                foreach (KeyValuePair<int, double> megaBalls in from keys in megaBallNumberPercents
                                                            orderby keys.Value
                                                            select keys)
                {
                    megaBallNumberPercentsSorted.Add(megaBalls.Key);
                }
            }
            for (int megaBallNumber = 0; megaBallNumber < 15; ++megaBallNumber)
            {
                for (int megamillionNumber = 0; megamillionNumber < 75; ++megamillionNumber)
                {
                    checkPickedNumbers = "";
                    if (megamillionNumber < 70)
                    {
                        sortNumsToCompare = numberPercentsSorted.ToList().GetRange(megamillionNumber, 5).ToArray();
                        Array.Sort(sortNumsToCompare);
                        checkPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                              string.Join(",", megaBallNumberPercentsSorted.ToList().
                                              GetRange(megaBallNumber, 1).ToArray()));
                        Array.Clear(sortNumsToCompare, 0, 5);
                    }
                    else
                    {
                        if (wrappingStart < 5)
                        {
                            sortNumsToCompare = numberPercentsSorted.ToList().GetRange(megamillionNumber, 5 - wrappingStart).
                                Concat(numberPercentsSorted.ToList().GetRange(0, wrappingStart)).ToArray();

                            Array.Sort(sortNumsToCompare);
                            checkPickedNumbers = (string.Join(",", sortNumsToCompare) + "," +
                                                string.Join(",", megaBallNumberPercentsSorted.ToList().
                                                GetRange(megaBallNumber, 1).ToArray()));
                            Array.Clear(sortNumsToCompare, 0, 5);
                            ++wrappingStart;
                        }
                    }

                    if (!(winningNumbers.Any(str => str.Contains(checkPickedNumbers))))
                    {
                        displayWinningPicks.Rows.Add(checkPickedNumbers);
                    }
                }
            }

            return this.ParsedPickedNumbers(displayWinningPicks, megaBallNumberPercents, numberPercents);
        }    

         public DataTable HighPercentMegaBallNumberMid2013()
        {
            int megaBallCount;
            DataTable DisplayMegaBallPercentages = new DataTable();
            DisplayMegaBallPercentages.Columns.Add(MegaMillionConsts.ColumnHeaderPickedMegaBall, typeof(string));
            DisplayMegaBallPercentages.Columns.Add(MegaMillionConsts.ColumnHeadingPickedMegaBallPerc, typeof(string));
            IEnumerable<ILotteryNumber> source = this.GetMegaBallsAfterOctober152013();
            for (int megaNumber = 1; megaNumber <= 15; megaNumber = megaBallCount)
            {
                double num = (((double)source.Count<ILotteryNumber>(x => (x.MegaBall == megaNumber))) / ((double)source.Count<ILotteryNumber>())) * 100.0;
                num = Math.Round(num, 2);
                DataRow returnRows = DisplayMegaBallPercentages.NewRow();
                returnRows[MegaMillionConsts.ColumnHeaderPickedMegaBall] = megaNumber;
                returnRows[MegaMillionConsts.ColumnHeadingPickedMegaBallPerc] = num.ToString() + "%";
                DisplayMegaBallPercentages.Rows.Add(returnRows);
                megaBallCount = megaNumber + 1;
            }
            return DisplayMegaBallPercentages;
        }

        private DataTable ParsedPickedNumbers(DataTable PickedNumbers, Dictionary<int, double> megaBallNumberPercents, Dictionary<int, double> numberPercents)
        {
            int rowIndex = 0;
            double avgWinningPercents = 0.0;
            string winningNumbers = null;
            string megaBallNumbers = null;
            string[] CompareArray = null;
            foreach (DataRow Numbers in PickedNumbers.Rows)
            {
                string temp = Numbers[0].ToString();
                megaBallNumbers = temp.Substring(temp.LastIndexOf(',') + 1, (temp.Length - temp.LastIndexOf(',')) - 1);
                winningNumbers = Numbers[0].ToString().Substring(0, Numbers[0].ToString().LastIndexOf(','));
                CompareArray = winningNumbers.Split(',');
                avgWinningPercents = ((((numberPercents[int.Parse(CompareArray[0])] + numberPercents[int.Parse(CompareArray[1])]) + numberPercents[int.Parse(CompareArray[2])]) + numberPercents[int.Parse(CompareArray[3])]) + numberPercents[int.Parse(CompareArray[4])]) / 5.0;
                PickedNumbers.Rows[rowIndex][MegaMillionConsts.ColumnHeaderPickedNumbers] = "[" + winningNumbers + "]";
                PickedNumbers.Rows[rowIndex][MegaMillionConsts.ColumnHeaderPickedAvgWinPerc] = avgWinningPercents.ToString() + "%";
                PickedNumbers.Rows[rowIndex][MegaMillionConsts.ColumnHeaderPickedMegaBall] = megaBallNumbers;
                PickedNumbers.Rows[rowIndex][MegaMillionConsts.ColumnHeaderPickedMegaBallPerc] = megaBallNumberPercents[int.Parse(megaBallNumbers)].ToString() + "%";
                rowIndex++;
                Array.Clear(CompareArray, 0, 5);
            }
            return PickedNumbers;
        }

        public void SavePredicatedLotteryNumbers()
        {
            this._WinningNumbers = this._DataBase.GetLotteryNumbers();
            DataTable winRateTable = this.HighestWinningRatePicks();
            int count = 1;
            int centerStart = winRateTable.Rows.Count / 2;
            int centerEnd = centerStart + 20;
            int endStart = winRateTable.Rows.Count - 20;
            foreach (DataRow row in winRateTable.Rows)
            {
                if (((count <= 20) || ((count > centerStart) && (count <= centerEnd))) || (count > endStart))
                {
                    try
                    {
                        Tuple<LotteryNumber, decimal, decimal> lotteryNumberFromDataTableRow = this.GetPredictedLotteryNumberFromDataTableRow(row);
                        this._DataBase.SaveTopPrediction(lotteryNumberFromDataTableRow.Item1, lotteryNumberFromDataTableRow.Item2, lotteryNumberFromDataTableRow.Item3);
                    }
                    catch (Exception exception)
                    {
                        log.Error($"SavePredicatedLotteryNumbers count = {count}", exception);
                    }
                }
                count++;
            }
        }

        public DataTable SortedNumbers()
        {
            this.UnsortedNumbers();
            this.CountAndDeleteRepeatedWinners(this.returnTable);
            this.ConvertToDates(this.returnTable);
            this.SortWinningNumbers(this.returnTable);
            return this.returnTable;
        }

        private void SortWinningNumbers(DataTable returnTable)
        {
            int arrayCount = 0;
            foreach (DataRow dr in returnTable.Rows)
            {
                string winnningNumbers = dr[MegaMillionConsts.ColumnHeadingI].ToString();
                int[] intArray = winnningNumbers.Split(',').Select(int.Parse).ToArray();
                Array.Sort(intArray);
                returnTable.Rows[arrayCount][MegaMillionConsts.ColumnHeadingI] = returnTable.Rows[arrayCount][MegaMillionConsts.ColumnHeadingI] = "[" + intArray[0] + " - " + intArray[1] + " - " + intArray[2] + " - " + intArray[3] + " - " + intArray[4] + "]";
                ++arrayCount;
            }
        }

        public DataTable UnsortedNumbers()
        {
            returnTable = new DataTable();
            returnTable.Columns.Add(MegaMillionConsts.ColumnHeadingI, typeof(string));
            returnTable.Columns.Add(MegaMillionConsts.ColumnHeadingII, typeof(string));
            returnTable.Columns.Add(MegaMillionConsts.ColumnHeadingIII, typeof(string));
            returnTable.Columns.Add(MegaMillionConsts.ColumnHeadingIV, typeof(string));

            foreach (ILotteryNumber number in this._WinningNumbers)
            {
                DataRow row = this.returnTable.NewRow();
                string[] numbersCombined = new string[] { number.Number1.ToString(), ",", number.Number2.ToString(), ",", number.Number3.ToString(), ",", number.Number4.ToString(), ",", number.Number5.ToString() };
                row[MegaMillionConsts.ColumnHeadingI] = string.Concat(numbersCombined);
                row[MegaMillionConsts.ColumnHeadingII] = number.MegaBall;
                row[MegaMillionConsts.ColumnHeadingIII] = number.DateWon.ToString("MM-dd-yyyy");
                this.returnTable.Rows.Add(row);
            }

            return returnTable;
        }


    }
}
