using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace LotteryBacktest
{
    public class BackTest
    {
        public bool WhetherWin { get; set; }


        public void RealDataTest()
        {
            decimal InitialFund = 5000M;
            int drawingTimes = 1000; // 900 one week  3600 one month  43800 one year
            int initialBet = 20;
            int Multiplier = 2;
            string selfDescribe = "1/6 Stop Loss";
            Guid newID = Guid.NewGuid();

            string Note = String.Format("Initial Fund: {0}, Drawing Times: {1}, Initial Bet: {2}, Multiplier: {3}, {4}", 
                InitialFund.ToString(),drawingTimes.ToString(),initialBet.ToString(),Multiplier.ToString(), selfDescribe);

            Machine M = new Machine();
            //M.looping(InitialFund, drawingTimes, 5, 3); // 本金, 起始押注, 抽奖次数, 递增倍数
            //M.BackTest
            M.StatisticalBig(InitialFund, drawingTimes, initialBet, Multiplier, Note, newID); // 本金, 起始押注, 抽奖次数 348680, 递增倍数

            double percentage = (double)M.WinCount / M.TotalCount;
            int day = M.TotalCount / 120;

            Console.WriteLine("----------------------------");
            Console.WriteLine("总结： 总次数： {0}， 获胜次数：{1}， 盈率：{2}", M.TotalCount, M.WinCount, percentage);
            Console.WriteLine("起始资金： {0}， 最终资金：{1}， 盈利：{2}", InitialFund, M.AccountBalance, M.AccountBalance - InitialFund);
            Console.WriteLine("用时： {0} 天", day);

            // Save total Statistics to db
            Database db = new Database();
            db.SaveTotalData(newID, InitialFund, drawingTimes, initialBet, Multiplier, Note, M.TotalCount, M.WinCount, percentage, M.AccountBalance, M.AccountBalance - InitialFund);
        }


        public void MockTest()
        {
            decimal InitialFund = 1000M;
            int drawingTimes = 43800; // 3600 one month

            Machine M = new Machine();
            M.looping(InitialFund, drawingTimes, 5, 3); // 本金, 起始押注, 抽奖次数, 递增倍数

            double percentage = (double)M.WinCount / M.TotalCount;
            int day = drawingTimes / 120;

            Console.WriteLine("----------------------------");
            Console.WriteLine("总结： 总次数： {0}， 获胜次数：{1}， 盈率：{2}", M.TotalCount, M.WinCount, percentage);
            Console.WriteLine("起始资金： {0}， 最终资金：{1}， 盈利：{2}", InitialFund, M.AccountBalance, M.AccountBalance - InitialFund);
            Console.WriteLine("用时： {0} 天", day);
        }


        public void ParseTest()
        {
            string TimeRemain = "00:01:41";
            var ts = TimeSpan.Parse(TimeRemain); //@"hh/:mm/:ss", CultureInfo.CurrentCulture
            Console.WriteLine("MilliSeconds Remain: " + ts.TotalMilliseconds);

        }

        public void boolTest()
        {
            Console.WriteLine("WhetherWin" + WhetherWin);  // The initial Value is False
        }
    }
}
