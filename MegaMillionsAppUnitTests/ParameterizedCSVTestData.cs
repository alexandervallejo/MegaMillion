using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsAppUnitTests
{
    class ParameterizedCSVTestData
    { 
        private DataTable testTable { get; set; }
        private string ColumnI = "Winning Numbers(Sorted)";
        private string ColumnII = "Mega Ball";
        private string ColumnIII = "Most Recent Winning Date";
        private string ColumnIV = "Number of Time Won";

        public static List<string> CSVFileTestCompareData()
        {
            return new List<string> {"1","\"Eldon Base for stackable storage shelf", " platinum\"", "Muhammed MacIntyre","3","-213.25","38.94","35","Nunavut","Storage & Organization","0.82",
            "\"1.7 Cubic Foot Compact \"\"Cube\"\" Office Refrigerators\"","Barry French","293","457.81","208.16","68.02","Nunavut","Appliances","0.583",
            "\"Cardinal Slant-D� Ring Binder", " Heavy Gauge Vinyl\"","Barry French","293","46.71","8.69","2.99","Nunavut","Binders and Binder Accessories","0.394",
            "R380","Clay Rozendal","483","1198.97","195.99","3.99","Nunavut","Telephones and Communication","0.585",
            "Holmes HEPA Air Purifier","Carlos Soltero","515","30.94","21.78","5.94","Nunavut","Appliances","0.56",
            "G.E. Longer-Life Indoor Recessed Floodlight Bulbs","Carlos Soltero","515","4.43","6.64","4.95","Nunavut","Office Furnishings","0.377",
            "\"Angle-D Binders with Locking Rings", " Label Holders\"","Carl Jackson","613","-54.04","7.3","7.72","Nunavut","Binders and Binder Accessories","0.388",
            "\"SAFCO Mobile Desk Side File", " Wire Frame\"","Carl Jackson","613","127.70","42.76","6.22","Nunavut","Storage & Organization", "9",
            "\"SAFCO Commercial Wire Shelving", " Black\"","Monica Federle","643","-695.26","138.14","35","Nunavut","Storage & Organization", "10",
            "Xerox 198","Dorothy Badders","678","-226.36","4.98","8.33","Nunavut","Paper","0.38"};
        }

        public static string[] UnSortedNumbersInputTestData()
        {
            /*  Data comes in latest to earliest, so we read from last to first.
            Use 10 numbers at a time, read right to left.
            [Month, Day, Year, 1st #, 2nd #, 3rd #, 4th #, 5th #, PowerBall, Unused]  
            Grab 0-2 on a 0 index to create the Date
            Grab 3-7 to create winning Mega Million Numbers
            Grab 8 for power ball
            Inex 9 is unused (MegaPiler 1-5)
            */
            return new string[70]
                       { "02", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "03", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "04", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "05", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "12", "30", "2012", "5","10","15","20","25","5","Don't Use",
                         "05", "10", "2030", "1","5","15","3","2","15","Don't Use",
                         "7", "1",   "3030", "50","100","150","15","7","50","Don't Use"};
        }

        public DataTable UnSortedNumbersOutputTestData()
        {
            testTable = new DataTable();
            testTable.Columns.Add(ColumnI, typeof(string));
            testTable.Columns.Add(ColumnII, typeof(string));
            testTable.Columns.Add(ColumnIII, typeof(string));
            testTable.Columns.Add(ColumnIV, typeof(string));
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });

            //Rows 1-7 Column Numbers
            testTable.Rows[0][ColumnI] = "50,100,150,15,7";
            testTable.Rows[1][ColumnI] = "1,5,15,3,2";
            testTable.Rows[2][ColumnI] = "5,10,15,20,25";
            testTable.Rows[3][ColumnI] = "5,10,15,20,25";
            testTable.Rows[4][ColumnI] = "5,10,15,20,25";
            testTable.Rows[5][ColumnI] = "5,10,15,20,25";
            testTable.Rows[6][ColumnI] = "5,10,15,20,25";

            //Rows 1-7 Column MegaBall
            testTable.Rows[0][ColumnII] = "50";
            testTable.Rows[1][ColumnII] = "15";
            testTable.Rows[2][ColumnII] = "5";
            testTable.Rows[3][ColumnII] = "5";
            testTable.Rows[4][ColumnII] = "5";
            testTable.Rows[5][ColumnII] = "5";
            testTable.Rows[6][ColumnII] = "5";


            //Rows 1-7 Column Date
            testTable.Rows[0][ColumnIII] = "3030-7-1";
            testTable.Rows[1][ColumnIII] = "2030-05-10";
            testTable.Rows[2][ColumnIII] = "2012-12-30";
            testTable.Rows[3][ColumnIII] = "1999-05-10";
            testTable.Rows[4][ColumnIII] = "1999-04-10";
            testTable.Rows[5][ColumnIII] = "1999-03-10";
            testTable.Rows[6][ColumnIII] = "1999-02-10";

            //Rows 1-7 Column MegaBall Count
            testTable.Rows[0][ColumnIV] = null;
            testTable.Rows[1][ColumnIV] = null;
            testTable.Rows[2][ColumnIV] = null;
            testTable.Rows[3][ColumnIV] = null;
            testTable.Rows[4][ColumnIV] = null;
            testTable.Rows[5][ColumnIV] = null;
            testTable.Rows[6][ColumnIV] = null;

            return testTable;
        }

        public DataTable SortedNumbersOutputTestData()
        {
            testTable = new DataTable();
            testTable.Columns.Add(ColumnI, typeof(string));
            testTable.Columns.Add(ColumnII, typeof(string));
            testTable.Columns.Add(ColumnIII, typeof(string));
            testTable.Columns.Add(ColumnIV, typeof(string));
            testTable.Rows.Add(new string[4] { "0", "0", "0", "" });
            testTable.Rows.Add(new string[4] { "0", "0", "0", "" });
            testTable.Rows.Add(new string[4] { "0", "0", "0", "" });

            //Rows 1-3 Column Numbers
            testTable.Rows[0][ColumnI] =  "[7 - 15 - 50 - 100 - 150]";
            testTable.Rows[1][ColumnI] =  "[1 - 2 - 3 - 5 - 15]";
            testTable.Rows[2][ColumnI] =  "[5 - 10 - 15 - 20 - 25]";
            
            
            //Rows 1-4 Column MegaBall
            testTable.Rows[0][ColumnII] =  "50";
            testTable.Rows[1][ColumnII] =  "15";
            testTable.Rows[2][ColumnII] =  "5" ;


            //Rows 1-3 Column Date
            testTable.Rows[0][ColumnIII] = DateTime.Parse("3030-7-01").ToString("yyyy-MM-dd");
            testTable.Rows[1][ColumnIII] = DateTime.Parse("2030-5-10").ToString("yyyy-MM-dd");
            testTable.Rows[2][ColumnIII] = DateTime.Parse("2012-12-30").ToString("yyyy-MM-dd");

            //Rows 1-3 Column MegaBall Count
            testTable.Rows[0][ColumnIV] = "1";
            testTable.Rows[1][ColumnIV] = "1";
            testTable.Rows[2][ColumnIV] = "5";

            return testTable;
        }

        public static string[] UnSortedMegaMillionAndPowerBallTestData()
        {
            /*  Data comes in latest to earliest, so we read from last to first.
            Use 10 numbers at a time, read right to left.
            [Month, Day, Year, 1st #, 2nd #, 3rd #, 4th #, 5th #, PowerBall, Unused]  
            Grab 0-2 on a 0 index to create the Date
            Grab 3-7 to create winning Mega Million Numbers
            Grab 8 for power ball
            Inex 9 is unused (MegaPiler 1-5)
            */
            return new string[50]
                       { "02", "10", "2012", "5","10","10","15","15","1","Don't Use",
                         "03", "10", "2013", "5","10","10","15","15","1","Don't Use",
                         "04", "10", "2015", "5","10","10","15","15","2","Don't Use",
                         "05", "10", "2016", "5","10","10","15","15","2","Don't Use",
                         "12", "30", "2017", "5","10","10","15","15","3","Don't Use",};
        }

        public DataTable HighestPercentWinningNumbersMid2013TestData()
        {
            testTable = new DataTable();
            testTable.Columns.Add("MegaMillion Number", typeof(string));
            testTable.Columns.Add("Percentage Of Winning", typeof(string));

            //Rows 1-4 Column Numbers
            for(int index = 0; index <= 75; ++index)
            {
                testTable.Rows.Add(new double[2] { 0.0, 0.0 });
                testTable.Rows[index]["MegaMillion Number"] = index + 1;
            }
                
            //Rows 1-4 Column MegaBall
            testTable.Rows[4]["Percentage Of Winning"] = Math.Round(5.0/25.0);
            testTable.Rows[9]["Percentage Of Winning"] = Math.Round(10.0 / 25.0);
            testTable.Rows[14]["Percentage Of Winning"] = Math.Round(5.0 / 25.0);

            return testTable;
        }

        public DataTable HighestPercentWinningMegaBallMid2013TestData()
        {
            testTable = new DataTable();
            testTable.Columns.Add("MegaMillion Number", typeof(string));
            testTable.Columns.Add("Percentage Of Winning", typeof(string));

            //Rows 1-4 Column Numbers
            for (int index = 0; index <= 15; ++index)
            {
                testTable.Rows.Add(new double[2] { 0.0, 0.0 });
                testTable.Rows[index]["MegaMillion Number"] = index + 1;
            }

            //Rows 1-4 Column MegaBall
            testTable.Rows[0]["Percentage Of Winning"] = Math.Round(2.0 / 5.0);
            testTable.Rows[1]["Percentage Of Winning"] = Math.Round(2.0 / 5.0);
            testTable.Rows[2]["Percentage Of Winning"] = Math.Round(1.0 / 5.0);

            return testTable;
        }



        public static string[] InputDataPickedNumbersTestData()
        {
            /*  Testing the Picked Numbers need to pick the highest change of
                the 5 numbers cycle through them with the highest powerball,
                then increment to the next highest powerball until all non already 
                one data is available.
                Use 10 numbers at a time, read right to left.
                Grab 3-7 to create winning Mega Million Numbers
                Grab 8 for power ball
            */
            return new string[300]
                       { null, null, null, "1","5","4","5","6","1",null,
                         null, null, null, "1","5","4","5","6","2",null,
                         null, null, null, "1","5","4","5","6","2",null,
                         null, null, null, "1","6","4","5","6","3",null,
                         null, null, null, "1","6","4","5","6","3",null,
                         null, null, null, "1","6","4","5","6","3",null,
                         null, null, null, "1","7","4","5","6","4",null,
                         null, null, null, "1","7","4","5","6","4",null,
                         null, null, null, "2","7","4","5","6","4",null,
                         null, null, null, "2","8","4","5","6","4",null,
                         null, null, null, "2","8","4","5","6","5",null,
                         null, null, null, "2","8","4","5","6","5",null,
                         null, null, null, "2","28","41","50","60","5",null,
                         null, null, null, "2","2","4","5","6","5",null,
                         null, null, null, "2","2","4","5","6","6",null,
                         null, null, null, "2","2","4","5","6","6",null,
                         null, null, null, "2","2","4","5","6","6",null,
                         null, null, null, "2","2","4","5","6","6",null,
                         null, null, null, "2","2","4","5","6","7",null,
                         null, null, null, "3","2","4","5","6","7",null,
                         null, null, null, "3","2","4","5","6","7",null,
                         null, null, null, "3","2","4","5","6","8",null,
                         null, null, null, "3","2","4","5","6","8",null,
                         null, null, null, "3","2","4","5","6","8",null,
                         null, null, null, "3","2","4","5","6","9",null,
                         null, null, null, "3","2","4","5","6","9",null,
                         null, null, null, "3","2","4","5","6","9",null,
                         null, null, null, "4","2","4","5","6","9",null,
                         null, null, null, "4","2","4","5","6","9",null,
                         null, null, null, "4","2","4","5","6","9",null,};
        }

        public static string[] ExpectedPickedNumbersTestData()
        {
            /*  Testing the Picked Numbers need to pick the highest change of
                the 5 numbers cycle through them with the highest powerball,
                then increment to the next highest powerball until all non already 
                one data is available.
                Use 10 numbers at a time, read right to left.
                Grab 3-7 to create winning Mega Million Numbers
                Grab 8 for power ball
            */
            return new string[70]
                       { "02", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "03", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "04", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "05", "10", "1999", "5","10","15","20","25","5","Don't Use",
                         "12", "30", "2012", "5","10","15","20","25","5","Don't Use",
                         "05", "10", "2030", "1","5","15","3","2","15","Don't Use",
                         "7", "1",   "3030", "50","100","150","15","7","50","Don't Use"};
        }
    }
}
