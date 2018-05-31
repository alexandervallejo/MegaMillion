using log4net;
using MegaMillion.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillion.DB
{
    public class MegaMillionDB
    {
        // Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _ConnectionString;

        // Methods
        public MegaMillionDB(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        public List<ILotteryNumber> GetLotteryNumbers()
        {
            List<ILotteryNumber> list = new List<ILotteryNumber>();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_MegaMillion_Fetch";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(this.Fetch(reader));
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }

        public List<ILotteryNumber> GetPredictedLotteryNumbers()
        {
            List<ILotteryNumber> list = new List<ILotteryNumber>();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_MegaMillionPredicted_Fetch";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(this.Fetch(reader));
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }

        public List<ILotteryNumber> GetUpdatedLotteryNumbers()
        {
            List<ILotteryNumber> list = new List<ILotteryNumber>();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_MegaMillionOct152013_Fetch";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(this.Fetch(reader));
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }

        private ILotteryNumber Fetch(SqlDataReader reader) =>
                                                            new LotteryNumber
                                                            {
                                                                DateWon = DateTime.Parse(reader["WinningDate"].ToString()),
                                                                MegaBall = int.Parse(reader["MegaBall"].ToString()),
                                                                Number1 = int.Parse(reader["Number1"].ToString()),
                                                                Number2 = int.Parse(reader["Number2"].ToString()),
                                                                Number3 = int.Parse(reader["Number3"].ToString()),
                                                                Number4 = int.Parse(reader["Number4"].ToString()),
                                                                Number5 = int.Parse(reader["Number5"].ToString())
                                                            };


        public void SaveTopPrediction(ILotteryNumber lotteryNumber, decimal averagePercetage, decimal megaBallPercentage)
        {
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_MegaMillionPredicted_Insert";
                    command.Parameters.AddWithValue("@PredictedLotteryDate", lotteryNumber.DateWon);
                    command.Parameters.AddWithValue("@Number1", lotteryNumber.Number1);
                    command.Parameters.AddWithValue("@Number2", lotteryNumber.Number2);
                    command.Parameters.AddWithValue("@Number3", lotteryNumber.Number3);
                    command.Parameters.AddWithValue("@Number4", lotteryNumber.Number4);
                    command.Parameters.AddWithValue("@Number5", lotteryNumber.Number5);
                    command.Parameters.AddWithValue("@MegaBall", lotteryNumber.MegaBall);
                    command.Parameters.AddWithValue("@PredictedWinPercentage", averagePercetage);
                    command.Parameters.AddWithValue("@MegaballWinPercentage", megaBallPercentage);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdatePredictions(ILotteryNumber lotteryNumber, bool won)
        {
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_MegaMillionPredicted_Update";
                    command.Parameters.AddWithValue("@PredictedLotteryDate", lotteryNumber.DateWon);
                    command.Parameters.AddWithValue("@Number1", lotteryNumber.Number1);
                    command.Parameters.AddWithValue("@Number2", lotteryNumber.Number2);
                    command.Parameters.AddWithValue("@Number3", lotteryNumber.Number3);
                    command.Parameters.AddWithValue("@Number4", lotteryNumber.Number4);
                    command.Parameters.AddWithValue("@Number5", lotteryNumber.Number5);
                    command.Parameters.AddWithValue("@MegaBall", lotteryNumber.MegaBall);
                    command.Parameters.AddWithValue("@Won", won);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


        public void SaveDataFromWebsiteIntoLotteryNumbers()
        {
            string requestUriString = "http://txlottery.org/export/sites/lottery/Games/Mega_Millions/Winning_Numbers/megamillions.csv";
            var strArray = GetCSV(requestUriString);
            List<string> splitLoteryNumbers = new List<string>();
            foreach (string splitNumber in strArray)
            {
                if (!string.IsNullOrWhiteSpace(splitNumber))
                {
                    splitLoteryNumbers.Add(splitNumber);
                }
            }
            List<LotteryNumber> lotteryNumbers = new List<LotteryNumber>();
            for (int numberIndex = splitLoteryNumbers.Count<string>(); numberIndex > 0; numberIndex -= 10)
            {
                LotteryNumber lotteryNumber = new LotteryNumber
                {
                    DateWon = new DateTime(int.Parse(splitLoteryNumbers[numberIndex - 8]), int.Parse(splitLoteryNumbers[numberIndex - 10]), int.Parse(splitLoteryNumbers[numberIndex - 9]))
                };
                List<int> loterryNumber = new List<int> {
                int.Parse(splitLoteryNumbers.ElementAtOrDefault<string>(numberIndex - 3)),
                int.Parse(splitLoteryNumbers.ElementAtOrDefault<string>(numberIndex - 4)),
                int.Parse(splitLoteryNumbers.ElementAtOrDefault<string>(numberIndex - 5)),
                int.Parse(splitLoteryNumbers.ElementAtOrDefault<string>(numberIndex - 6)),
                int.Parse(splitLoteryNumbers.ElementAtOrDefault<string>(numberIndex - 7))
            };
                loterryNumber = (from x in loterryNumber
                                 orderby x
                                 select x).ToList<int>();
                lotteryNumber.Number1 = loterryNumber[0];
                lotteryNumber.Number2 = loterryNumber[1];
                lotteryNumber.Number3 = loterryNumber[2];
                lotteryNumber.Number4 = loterryNumber[3];
                lotteryNumber.Number5 = loterryNumber[4];
                lotteryNumber.MegaBall = int.Parse(splitLoteryNumbers.ElementAtOrDefault<string>(numberIndex - 2));
                lotteryNumbers.Add(lotteryNumber);
            }
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    foreach (LotteryNumber number2 in lotteryNumbers)
                    {
                        command.CommandText = $@"IF NOT EXISTS(SELECT 1 From dbo.WinningNumbers
                                                        WHERE WinningDate = '{number2.DateWon}' AND
                                                              Number1 = { number2.Number1 } AND
                                                              Number2 = { number2.Number2 } AND
                                                              Number3 = { number2.Number3 } AND
                                                              Number4 = { number2.Number4 } AND
                                                              Number5 = { number2.Number5 } AND
                                                              MegaBall = { number2.MegaBall })
	                                                    BEGIN
                                                                INSERT INTO[dbo].[WinningNumbers]
                                                                      ([WinningDate]
                                                                       ,[Number1]
                                                                       ,[Number2]
                                                                       ,[Number3]
                                                                       ,[Number4]
                                                                       ,[Number5]
                                                                       ,[MegaBall])
                                                                VALUES
                                                                      ('{number2.DateWon}'
                                                                      ,{ number2.Number1}
                                                                      ,{number2.Number2}
                                                                      ,{number2.Number3}
                                                                      ,{number2.Number4}
                                                                      ,{number2.Number5}
                                                                      ,{number2.MegaBall})
                                                        END";
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

        public string[] GetCSV(string requestUriString)
        {
            string streamData = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    streamData = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (WebException exception)
            {
                using (Stream stream = exception.Response.GetResponseStream())
                {
                    using (StreamReader reader2 = new StreamReader(stream))
                    {
                        Console.WriteLine(reader2.ReadToEnd());
                    }
                }
            }
            return (from x in streamData.Replace("Mega Millions", "").Split(',') select x.Replace("\r\n", "")).ToArray<string>();
        }

    }
}
