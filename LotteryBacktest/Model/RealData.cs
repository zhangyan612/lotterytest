using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryBacktest.Model
{
    public class RealData
    {
        public string Expect { get; set; }
        public string Picked { get; set; }
        public int WinNumber { get; set; }
        public DateTime Time { get; set; }
        public decimal Balance { get; set; }
        public int Bet { get; set; }
    }
}
