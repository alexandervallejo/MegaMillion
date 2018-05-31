using MegaMillion.Data;
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
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingI, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingII, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingIII, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingIV, typeof(string));
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });
            testTable.Rows.Add(new string[4] { "0", "0", "0", null });

            //Rows 1-7 Column Numbers
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingI] = "50,100,150,15,7";
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingI] = "1,5,15,3,2";
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingI] = "5,10,15,20,25";
            testTable.Rows[3][MegaMillionConsts.ColumnHeadingI] = "5,10,15,20,25";
            testTable.Rows[4][MegaMillionConsts.ColumnHeadingI] = "5,10,15,20,25";
            testTable.Rows[5][MegaMillionConsts.ColumnHeadingI] = "5,10,15,20,25";
            testTable.Rows[6][MegaMillionConsts.ColumnHeadingI] = "5,10,15,20,25";

            //Rows 1-7 Column MegaBall
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingII] = "50";
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingII] = "15";
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingII] = "5";
            testTable.Rows[3][MegaMillionConsts.ColumnHeadingII] = "5";
            testTable.Rows[4][MegaMillionConsts.ColumnHeadingII] = "5";
            testTable.Rows[5][MegaMillionConsts.ColumnHeadingII] = "5";
            testTable.Rows[6][MegaMillionConsts.ColumnHeadingII] = "5";


            //Rows 1-7 Column Date
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingIII] = "3030-7-1";
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingIII] = "2030-05-10";
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingIII] = "2012-12-30";
            testTable.Rows[3][MegaMillionConsts.ColumnHeadingIII] = "1999-05-10";
            testTable.Rows[4][MegaMillionConsts.ColumnHeadingIII] = "1999-04-10";
            testTable.Rows[5][MegaMillionConsts.ColumnHeadingIII] = "1999-03-10";
            testTable.Rows[6][MegaMillionConsts.ColumnHeadingIII] = "1999-02-10";

            //Rows 1-7 Column MegaBall Count
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingIV] = null;
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingIV] = null;
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingIV] = null;
            testTable.Rows[3][MegaMillionConsts.ColumnHeadingIV] = null;
            testTable.Rows[4][MegaMillionConsts.ColumnHeadingIV] = null;
            testTable.Rows[5][MegaMillionConsts.ColumnHeadingIV] = null;
            testTable.Rows[6][MegaMillionConsts.ColumnHeadingIV] = null;

            return testTable;
        }

        public DataTable SortedNumbersOutputTestData()
        {
            testTable = new DataTable();
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingI, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingII, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingIII, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingIV, typeof(string));
            testTable.Rows.Add(new string[4] { "0", "0", "0", "" });
            testTable.Rows.Add(new string[4] { "0", "0", "0", "" });
            testTable.Rows.Add(new string[4] { "0", "0", "0", "" });

            //Rows 1-3 Column Numbers
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingI] =  "[7 - 15 - 50 - 100 - 150]";
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingI] =  "[1 - 2 - 3 - 5 - 15]";
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingI] =  "[5 - 10 - 15 - 20 - 25]";
            
            
            //Rows 1-4 Column MegaBall
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingII] =  "50";
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingII] =  "15";
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingII] =  "5" ;


            //Rows 1-3 Column Date
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingIII] = DateTime.Parse("3030-7-01").ToString("yyyy-MM-dd");
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingIII] = DateTime.Parse("2030-5-10").ToString("yyyy-MM-dd");
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingIII] = DateTime.Parse("2012-12-30").ToString("yyyy-MM-dd");

            //Rows 1-3 Column MegaBall Count
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingIV] = "1";
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingIV] = "1";
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingIV] = "5";

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
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingMegaMillionNumber, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingPickedMegaBallPerc, typeof(string));

            //Rows 1-4 Column Numbers
            for(int index = 0; index <= 75; ++index)
            {
                testTable.Rows.Add(new double[2] { 0.0, 0.0 });
                testTable.Rows[index][MegaMillionConsts.ColumnHeadingMegaMillionNumber] = index + 1;
            }
                
            //Rows 1-4 Column MegaBall
            testTable.Rows[4][MegaMillionConsts.ColumnHeadingPickedMegaBallPerc] = Math.Round(5.0/25.0);
            testTable.Rows[9][MegaMillionConsts.ColumnHeadingPickedMegaBallPerc] = Math.Round(10.0 / 25.0);
            testTable.Rows[14][MegaMillionConsts.ColumnHeadingPickedMegaBallPerc] = Math.Round(5.0 / 25.0);

            return testTable;
        }

        public DataTable HighestPercentWinningMegaBallMid2013TestData()
        {
            testTable = new DataTable();
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingMegaMillionNumber, typeof(string));
            testTable.Columns.Add(MegaMillionConsts.ColumnHeadingPickedMegaBallPerc, typeof(string));

            //Rows 1-4 Column Numbers
            for (int index = 0; index <= 15; ++index)
            {
                testTable.Rows.Add(new double[2] { 0.0, 0.0 });
                testTable.Rows[index][MegaMillionConsts.ColumnHeadingMegaMillionNumber] = index + 1;
            }

            //Rows 1-4 Column MegaBall
            testTable.Rows[0][MegaMillionConsts.ColumnHeadingPickedMegaBallPerc] = Math.Round(2.0 / 5.0);
            testTable.Rows[1][MegaMillionConsts.ColumnHeadingPickedMegaBallPerc] = Math.Round(2.0 / 5.0);
            testTable.Rows[2][MegaMillionConsts.ColumnHeadingPickedMegaBallPerc] = Math.Round(1.0 / 5.0);

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
                       { null, null, null, "1","5","6","7","8","1",null,
                         null, null, null, "1","5","7","8","9","2",null,
                         null, null, null, "1","5","10","11","12","2",null,
                         null, null, null, "1","6","13","15","16","13",null,
                         null, null, null, "1","6","40","50","60","30",null,
                         null, null, null, "1","6","41","53","60","3",null,
                         null, null, null, "1","7","43","54","61","4",null,
                         null, null, null, "1","7","42","55","62","4",null,
                         null, null, null, "2","7","44","56","63","4",null,
                         null, null, null, "2","8","45","57","64","4",null,
                         null, null, null, "2","8","46","58","66","5",null,
                         null, null, null, "2","8","47","59","67","6",null,
                         null, null, null, "2","9","41","50","60","6",null,
                         null, null, null, "2","10","21","52","68","5",null,
                         null, null, null, "2","10","17","28","69","6",null,
                         null, null, null, "2","11","16","29","66","6",null,
                         null, null, null, "2","12","15","30","67","6",null,
                         null, null, null, "2","13","21","31","64","6",null,
                         null, null, null, "2","14","22","32","48","7",null,
                         null, null, null, "3","4","7","36","70","7",null,
                         null, null, null, "3","5","8","38","71","7",null,
                         null, null, null, "3","6","9","39","72","8",null,
                         null, null, null, "3","7","10","41","73","9",null,
                         null, null, null, "3","8","11","42","74","10",null,
                         null, null, null, "3","9","12","43","75","11",null,
                         null, null, null, "3","10","13","44","60","12",null,
                         null, null, null, "3","11","14","47","61","13",null,
                         null, null, null, "4","12","15","48","62","14",null,
                         null, null, null, "4","10","16","50","75","15",null,
                         null, null, null, "4","12","17","52","70","15",null,};
        }

        public static string[] ExpectedHighestPickedNumbersTestData()
        {
            /*  

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
