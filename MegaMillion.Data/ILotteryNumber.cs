using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillion.Data
{
    public interface ILotteryNumber
    {
        DateTime DateWon { get; set; }
        int Number1 { get; set; }
        int Number2 { get; set; }
        int Number3 { get; set; }
        int Number4 { get; set; }
        int Number5 { get; set; }
        int MegaBall { get; set; }
    }



}
