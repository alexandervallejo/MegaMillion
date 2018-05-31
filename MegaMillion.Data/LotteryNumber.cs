using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillion.Data
{
    public class LotteryNumber : ILotteryNumber
    {
        public DateTime DateWon { get; set; }

        public int Number1 { get; set; }

        public int Number2 { get; set; }

        public int Number3 { get; set; }

        public int Number4 { get; set; }

        public int Number5 { get; set; }

        public int MegaBall { get; set; }
    }



}
