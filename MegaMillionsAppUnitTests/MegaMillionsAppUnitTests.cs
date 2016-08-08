using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MegaMillionsApp;
using System.Collections.Generic;
using System.Data;

namespace MegaMillionsAppUnitTests
{

    
    [TestClass]
    public class CSVManipulationTest
    {
        private bool Successful { get; set; }
        private string Information { get; set; }
        SortedLists TestInstance = new SortedLists();
        ParameterizedCSVTestData TestData = new ParameterizedCSVTestData();
        [TestMethod]
        public void CSVFileTest()
        {
            int count = 0;
            List<string> expectedTable = new List<string>();
            string [] returnedTable = TestInstance.GetCSV("http://www.sample-videos.com/csv/Sample-Spreadsheet-10-rows.csv");
            expectedTable = ParameterizedCSVTestData.CSVFileTestCompareData();

           foreach (var Compare in returnedTable)
           {
                Assert.AreEqual<string>(expectedTable[count], Compare);
                ++count;
           }
        }

        public void CompareTestTables(DataTable ExpectedTable, DataTable InputSortedTable)
        {
            Successful = true;
            if (ExpectedTable == null)
            {
                Successful = false;
                Information += "\nExpected Table Empty";
            }
            if (InputSortedTable == null)
            {
                Successful = false;
                Information += "\nInput Table Empty";
            }
            if (ExpectedTable.Rows.Count != InputSortedTable.Rows.Count)
            {
                Successful = false;
                Information += ("\nRow Count of expect: " + ExpectedTable.Rows.Count + "Row count of Input: " + InputSortedTable.Rows.Count);
            }


            if (ExpectedTable.Columns.Count != InputSortedTable.Columns.Count)
            {
                Information += ("\nColumn Count of expect: " + ExpectedTable.Columns.Count + "Column count of Input: " + InputSortedTable.Columns.Count);
                Successful = false;
            }


            if (ExpectedTable.Columns.Cast<DataColumn>().Any(dc => !InputSortedTable.Columns.Contains(dc.ColumnName)))
            {
                Information += "\nColumn Values MisMatch";
                Successful = false;
            }

            for (int rows = 0; ExpectedTable.Rows.Count - 1 >= rows; ++rows)
            {
                if (ExpectedTable.Columns.Cast<DataColumn>().Any(dc1 => ExpectedTable.Rows[rows][dc1.ColumnName].ToString() != InputSortedTable.Rows[rows][dc1.ColumnName].ToString()))
                {
                    Information += "\nRow Values MisMatch";
                    Successful = false;
                }
            }
        }
        [TestMethod]
        public void UnSortedNumbersTest()
        {
            string[] TestUnSortedNumbers = ParameterizedCSVTestData.UnSortedNumbersInputTestData();
            DataTable ExpectedTable = TestData.UnSortedNumbersOutputTestData();
            DataTable InputSortedTable = TestInstance.UnsortedNumbers(TestUnSortedNumbers);
            CompareTestTables(ExpectedTable, InputSortedTable);
            Assert.AreEqual(Successful, true, Information);
        }
        [TestMethod]
        public void SortedNumbersTest()
        {
            string[] TestUnSortedNumbers = ParameterizedCSVTestData.UnSortedNumbersInputTestData();
            DataTable ExpectedTable = TestData.SortedNumbersOutputTestData();
            DataTable InputSortedTable = TestInstance.SortedNumbers(TestUnSortedNumbers);
            CompareTestTables(ExpectedTable, InputSortedTable);
            Assert.AreEqual(Successful, true, Information);

        }

        [TestMethod]
        public void HighestPercentWinningNumbersMid2013Test()
        {
            string[] TestUnSortedNumbers = ParameterizedCSVTestData.UnSortedMegaMillionAndPowerBallTestData();
            DataTable ExpectedTable = TestData.HighestPercentWinningNumbersMid2013TestData();
            DataTable InputSortedTable = TestInstance.HighestPercentWinningNumbersMid2013(TestUnSortedNumbers);
        }

        [TestMethod]
        public void HighestPercentWinningMegaBallMid2013Test()
        {
            string[] TestUnSortedNumbers = ParameterizedCSVTestData.UnSortedMegaMillionAndPowerBallTestData();
            DataTable ExpectedTable = TestData.HighestPercentWinningMegaBallMid2013TestData();
            DataTable InputSortedTable = TestInstance.HighPercentMegaBallNumberMid2013(TestUnSortedNumbers);
        }
    }
}
