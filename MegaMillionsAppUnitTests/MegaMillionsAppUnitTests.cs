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
        CSVManipulation TestInstance = new CSVManipulation();
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

        [TestMethod]
        public void SortedNumbersTest()
        {
            bool AreTheTablesEqual = true;
            string[] TestUnSortedNumbers = ParameterizedCSVTestData.UnSortedNumbersInputTestData();
            DataTable ExpectedTable = TestData.SortedNumbersOutputTestData();
            DataTable InputSortedTable = TestInstance.SortedNumbers(TestUnSortedNumbers);

            if (ExpectedTable == null)
                AreTheTablesEqual = false;
            if (InputSortedTable == null)
                AreTheTablesEqual = false;
            if (ExpectedTable.Rows.Count != InputSortedTable.Rows.Count)
                AreTheTablesEqual = false;
       
            if (ExpectedTable.Columns.Count != InputSortedTable.Columns.Count)
                AreTheTablesEqual = false;

            if (ExpectedTable.Columns.Cast<DataColumn>().Any(dc => !InputSortedTable.Columns.Contains(dc.ColumnName)))
            {
                AreTheTablesEqual = false;
            }

            for (int rows = 0; ExpectedTable.Rows.Count - 1 >= rows; ++rows)
            {
                if (ExpectedTable.Columns.Cast<DataColumn>().Any(dc1 => ExpectedTable.Rows[rows][dc1.ColumnName].ToString() != InputSortedTable.Rows[rows][dc1.ColumnName].ToString()))
                {
                    AreTheTablesEqual = false;
                }
            }

            Assert.AreEqual(AreTheTablesEqual, true);

        }
    }
}
