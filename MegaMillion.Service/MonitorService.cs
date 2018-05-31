using log4net;
using MegaMillion.Data;
using MegaMillion.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace MegaMillion.Service
{

    public class MonitorService : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Timer _MonitorTimer;
        private MegaMillionDB _DB;
        private SortedLists _SortedLists;
        private int _HourInterval;

        public MonitorService()
        {
            this._SortedLists = new SortedLists(Properties.Settings.Default.ConnectionString);
        }

        public MonitorService(string[] args, int intervalInHours = 2)
        {
            this._SortedLists = new SortedLists(Properties.Settings.Default.ConnectionString);
            if (args.Count() > 0)
            {
                if (int.TryParse(args[0], out this._HourInterval) == false)
                {
                    this._HourInterval = intervalInHours;
                }
            }
            else
            {
                this._HourInterval = intervalInHours;
            }
        }

        private void _MonitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.MontiorFunctions();
        }

        public void Dispose()
        {
            try
            {
                this._MonitorTimer.Elapsed -= new ElapsedEventHandler(this._MonitorTimer_Elapsed);
                this._MonitorTimer.Stop();
            }
            catch (Exception exception)
            {
                log.Error("Dispose", exception);
            }
        }

        public void Initialize()
        {
            this._DB = new MegaMillionDB(Properties.Settings.Default.ConnectionString);
            this._MonitorTimer = new Timer();
            this._MonitorTimer.Interval = 1000* 60 * 60 * this._HourInterval;
            this._MonitorTimer.Elapsed += new ElapsedEventHandler(this._MonitorTimer_Elapsed);
            this._MonitorTimer.AutoReset = false;
            this.MontiorFunctions();
        }

        private void MontiorFunctions()
        {
            try
            {
                log.Info("Calling SaveDataFromWebsiteIntoLotteryNumbers...");
                _DB.SaveDataFromWebsiteIntoLotteryNumbers();
                log.Info("Calling SavePredicatedLotteryNumbers...");
                _SortedLists.SavePredicatedLotteryNumbers();
                log.Info("Calling UpdatePredicatedLotteryNumbers...");
                UpdatePredicatedLotteryNumbers();
            }
            catch (Exception exception)
            {
                log.Error("Monitor Timer Elapsed", exception);
            }
            finally
            {
                this._MonitorTimer.Start();
            }
        }

        private void UpdatePredicatedLotteryNumbers()
        {
            List<ILotteryNumber> lotteryNumbers = this._DB.GetLotteryNumbers();
            List<ILotteryNumber> predictedLotteryNumbers = this._DB.GetPredictedLotteryNumbers();
            foreach (ILotteryNumber lotteryNumber in lotteryNumbers)
            {
                IEnumerable<ILotteryNumber> lotteryNumberOnPredicatedDate = from ln in predictedLotteryNumbers
                                                                            where ln.DateWon == lotteryNumber.DateWon
                                                                            select ln;
                List<int> winningNumbers = new List<int> {
                    lotteryNumber.Number1,
                    lotteryNumber.Number2,
                    lotteryNumber.Number3,
                    lotteryNumber.Number4,
                    lotteryNumber.Number5
                };
                foreach (ILotteryNumber number in lotteryNumberOnPredicatedDate)
                {
                    int numbersCorrectlyPicked = 0;
                    foreach (int winningNumber in winningNumbers)
                    {
                        if (number.Number1 == winningNumber)
                        {
                            numbersCorrectlyPicked++;
                        }
                        else if (number.Number2 == winningNumber)
                        {
                            numbersCorrectlyPicked++;
                        }
                        else if (number.Number3 == winningNumber)
                        {
                            numbersCorrectlyPicked++;
                        }
                        else if (number.Number4 == winningNumber)
                        {
                            numbersCorrectlyPicked++;
                        }
                        else if (number.Number5 == winningNumber)
                        {
                            numbersCorrectlyPicked++;
                        }
                    }
                    if (((numbersCorrectlyPicked == 3) && (lotteryNumber.MegaBall == number.MegaBall)) || 
                        (numbersCorrectlyPicked > 3))
                    {
                        this._DB.UpdatePredictions(number, true);
                    }
                    else
                    {
                        this._DB.UpdatePredictions(number, false);
                    }
                }
            }
        }
    }
}
