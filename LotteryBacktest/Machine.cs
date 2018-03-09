using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotteryBacktest.Logic;

namespace LotteryBacktest
{
    public class Machine
    {
        private static readonly Random intRandom = new Random();
        private static readonly object syncLock = new object();
        private static readonly Random intRandomList = new Random();

        // 统计：中奖概率，中奖次数
        public int TotalCount { get; set; }
        public int WinCount { get; set; }
        public int LossCount { get; set; }
        public decimal AccountBalance { get; set; }
        public int Bet { get; set; }
        public bool WhetherWin { get; set; }
        public List<int> LastNumber { get; set; }
        public List<int> PickedNumber { get; set; }


        public int RandomInt(int minNum, int maxNum)
        {
            lock (syncLock)
            {
                // synchronize
                return intRandom.Next(minNum, maxNum);
            }
        }

        public List<int> RandomIntList(int minNum, int maxNum)
        {
            lock (syncLock)
            {
                // synchronize
                var list = Enumerable.Range(minNum, maxNum).OrderBy(x => intRandomList.Next()).Take(5).ToList();
                list.Sort();
                return list;
            }
        }

        public void looping(decimal balance, int times, int initialBet, int increment)
        {
            AccountBalance = balance;
            Bet = initialBet;

            bool GameOver = false;

            for (int i = 1; i < times + 1; i++)
            {
                int winning = RandomInt(0, 9);
                List<int> PickedNumber = RandomIntList(0, 9);
                string showList = string.Join(" ", PickedNumber.ToArray());

                if (!GameOver)
                {
                    if (PickedNumber.Contains(winning))
                    {
                        WinCount++;
                        AccountBalance = BalanceCalculation(AccountBalance, Bet, true);
                        Console.WriteLine("第 {0} 次开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 中奖!  可用资金： {4}", i, winning, showList, Bet, AccountBalance);
                        Bet = initialBet;
                    }
                    else
                    {
                        LossCount++;
                        AccountBalance = BalanceCalculation(AccountBalance, Bet, false);
                        Console.WriteLine("第 {0} 次开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 未中奖!  可用资金： {4}", i, winning, showList, Bet, AccountBalance);
                        Bet = BetStrategy(Bet, increment, AccountBalance, initialBet);
                    }
                }
                if (AccountBalance <= 0)
                {
                    GameOver = true;
                    Console.WriteLine("本金不足, 爆仓！！！");
                    break;
                }
                TotalCount++;
            }
        }

        public void Backtest(decimal balance, int times, int initialBet, int increment, string note, Guid testId)
        {
            AccountBalance = balance;
            Bet = initialBet;

            bool GameOver = false;
            Database db = new Database();
            List<History> data = new List<History>();
            data = db.GetHistoryList();
            DateTime testDate = DateTime.Now;
            
            foreach (var i in data)
            {
                if(i.ID <= times)
                {
                    int winning = Convert.ToInt32(i.Single);
                
                    List<int> PickedNumber = RandomIntList(0, 9);
                    string showList = string.Join(" ", PickedNumber.ToArray());

                    if (!GameOver)
                    {
                        if (PickedNumber.Contains(winning))
                        {
                            WinCount++;
                            AccountBalance = BalanceCalculation(AccountBalance, Bet, true);
                            Console.WriteLine("{0} 期开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 中奖!  可用资金： {4}", i.ID, winning, showList, Bet, AccountBalance);
                            db.SaveStatistics(i, AccountBalance, testId, testDate, Bet, showList, false, true, note);
                            // Bet for next time
                            Bet = initialBet;
                        }
                        else
                        {
                            LossCount++;
                            AccountBalance = BalanceCalculation(AccountBalance, Bet, false);
                            Console.WriteLine("{0} 期开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 未中奖!  可用资金： {4}", i.ID, winning, showList, Bet, AccountBalance);
                            // Bet for next time 
                            int lastBet = Bet;
                            // Strategy Testing
                            Bet = BetStrategy(Bet, increment, AccountBalance, initialBet);
                            bool stopLoss= Bet < lastBet ? true : false;
                            db.SaveStatistics(i, AccountBalance, testId, testDate, lastBet, showList, stopLoss, false, note);
                        }
                    }
                    if (AccountBalance <= 0)
                    {
                        GameOver = true;
                        Console.WriteLine("本金不足, 爆仓！！！");
                        break;
                    }
                    TotalCount++;
                }
            }
        }

        public void LossStay(decimal balance, int times, int initialBet, int increment, string note, Guid testId)
        {
            AccountBalance = balance;
            Bet = initialBet;

            bool GameOver = false;
            Database db = new Database();
            List<History> data = new List<History>();
            data = db.GetHistoryList();
            //Guid testId = Guid.NewGuid();
            DateTime testDate = DateTime.Now;
            WhetherWin = true;

            foreach (var i in data)
            {
                if (i.ID <= times)
                {
                    int winning = Convert.ToInt32(i.Single);

                    List<int> PickedNumber = RandomIntList(0, 9);
                    if (!WhetherWin)
                    {
                        PickedNumber = LastNumber;
                    }

                    string showList = string.Join(" ", PickedNumber.ToArray());

                    if (!GameOver)
                    {
                        if (PickedNumber.Contains(winning))
                        {
                            WinCount++;
                            AccountBalance = BalanceCalculation(AccountBalance, Bet, true);
                            Console.WriteLine("{0} 期开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 中奖!  可用资金： {4}", i.ID, winning, showList, Bet, AccountBalance);
                            db.SaveStatistics(i, AccountBalance, testId, testDate, Bet, showList, false, true, note);
                            // Bet for next time
                            Bet = initialBet;
                            WhetherWin = true;
                        }
                        else
                        {
                            LossCount++;
                            AccountBalance = BalanceCalculation(AccountBalance, Bet, false);
                            Console.WriteLine("{0} 期开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 未中奖!  可用资金： {4}", i.ID, winning, showList, Bet, AccountBalance);
                            // Bet for next time 
                            int lastBet = Bet;
                            // Strategy Testing
                            Bet = ConstantInc(Bet, increment, AccountBalance, initialBet);
                            bool stopLoss = Bet < lastBet ? true : false;
                            db.SaveStatistics(i, AccountBalance, testId, testDate, lastBet, showList, stopLoss, false, note);
                            WhetherWin = false;
                            LastNumber = PickedNumber;
                        }
                    }
                    if (AccountBalance <= 0)
                    {
                        GameOver = true;
                        Console.WriteLine("本金不足, 爆仓！！！");
                        break;
                    }
                    TotalCount++;
                }
            }
        }


        // Only bet on wan big
        public void StatisticalBig(decimal balance, int times, int initialBet, int increment, string note, Guid testId)
        {
            AccountBalance = balance;
            Bet = initialBet;

            bool GameOver = false;
            Database db = new Database();
            List<History> data = new List<History>();
            data = db.GetHistoryList();
            DateTime testDate = DateTime.Now;
            HistoryCount history = new HistoryCount();
            bool trade = false;
            string showList = "";

            foreach (var i in data)
            {
                if (TotalCount <= times)
                {
                    int winning = Convert.ToInt32(i.Single);

                    if (!GameOver && trade)
                    {
                        if (PickedNumber.Contains(winning))
                        {
                            WinCount++;
                            AccountBalance = BalanceCalculation(AccountBalance, Bet, true);
                            Console.WriteLine("{0} 期开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 中奖!  可用资金： {4}", i.ID, winning, showList, Bet, AccountBalance);
                            db.SaveStatistics(i, AccountBalance, testId, testDate, Bet, showList, false, true, note);
                            // Bet for next time
                            Bet = initialBet;
                        }
                        else
                        {
                            LossCount++;
                            AccountBalance = BalanceCalculation(AccountBalance, Bet, false);
                            Console.WriteLine("{0} 期开奖, 中奖号码：{1}, 投注号码：{2}, 购买金额：{3}, 未中奖!  可用资金： {4}", i.ID, winning, showList, Bet, AccountBalance);
                            // Bet for next time 
                            int lastBet = Bet;
                            // Strategy Testing
                            Bet = ConstantInc(Bet, increment, AccountBalance, initialBet);
                            bool stopLoss = Bet < lastBet ? true : false;
                            db.SaveStatistics(i, AccountBalance, testId, testDate, lastBet, showList, stopLoss, false, note);
                        }
                        TotalCount++;
                    }

                    // get from db 
                    history = db.GetHistoryTrend(i.Date);

                    // Pick Number for next round 
                    if (history.GeDa >= 7)
                    {
                        PickedNumber = new List<int> { 5, 6, 7, 8, 9 };
                        trade = true;
                        showList = string.Join(" ", PickedNumber.ToArray());
                    }
                    else if (history.GeXiao >= 7)
                    {
                        PickedNumber = new List<int> { 0, 1, 2, 3, 4 };
                        trade = true;
                        showList = string.Join(" ", PickedNumber.ToArray());
                    }
                    else
                    {
                        trade = false;
                        Bet = initialBet;
                    }

                    if (AccountBalance <= 0)
                    {
                        GameOver = true;
                        Console.WriteLine("本金不足, 爆仓！！！");
                        break;
                    }
                }
            }
        }

        public decimal BalanceCalculation(decimal fund, decimal bet, bool win)
        {
            if (win)
            {
                fund = fund + bet * 0.965M;
            }
            else
            {
                fund = fund - bet;
            }
            return fund;
        }

        public int BetStrategy(int bet, int increment, decimal balance, int initialBet)
        {
            //止损规则 1/6 仓位止损
            //记录止损
            var threshold = balance / 6;
            if (bet < threshold)
            {
                bet = bet * 2;
            }
            else
            {
                bet = initialBet;
            }
            return bet;
        }

        public int ConstantInc(int bet, int increment, decimal balance, int initialBet)
        {
            //止损规则 1/8 仓位止损
            //记录止损
            // 50, 25, 12.5, 6.25, 3.125 =98.43  +  1.5625  = 
            // 5, 15, 45, 135, 225, 450 
            if (bet == 20)
            {
                bet = 50;
            }
            else if (bet == 50)
            {
                bet = 100;
            }
            else if (bet == 100)
            {
                bet = 200;
            }
            else if (bet == 200)
            {
                bet = 400;
            }
            else if (bet == 400)
            {
                bet = 810;
            }
            else if (bet == 810)
            {
                bet = 1650;
            }
            else if (bet == 1650)
            {
                bet = 3350;
            }
            else if (bet == 3350)
            {
                bet = 6820;
            }
            else
            {
                bet = initialBet;
            }
            return bet;
        }


        public int ConstantStop(int bet, int increment, decimal balance, int initialBet)
        {
            //止损规则 1/8 仓位止损
            //记录止损
            // 50, 25, 12.5, 6.25, 3.125 =98.43  +  1.5625  = 
            // 5, 15, 45, 135, 225, 450 
            var threshold = 135;
            if (bet < threshold)
            {
                bet = bet * increment;
            }
            else if (bet == 135)
            {
                bet = 225;
            }
            else
            {
                bet = initialBet;
            }
            return bet;
        }


        public int RealBetStrategy(int single, int increment, decimal balance, int initialBet)
        {
            //止损规则 1/8 仓位止损
            //记录止损
            int bet = single * 5;

            var threshold = balance / 6;
            if (bet < threshold)
            {
                bet = bet * increment;
            }
            else
            {
                bet = initialBet;
            }
            return bet;
        }


        public int LowIncrementBet(int single, int increment, decimal balance, int initialBet)
        {
            //止损规则 1/8 仓位止损
            //记录止损
            int bet = single * 5;

            var threshold = 135;
            if (bet < threshold)
            {
                bet = bet * increment;
            } else if (bet == 135)
            {
                bet = 225;
            }
            else
            {
                bet = initialBet;
            }
            return bet;
        }


        public static HistoryCount bigorsmall(HistoryCount history)
        {
            // ge
            if (history.Single < 5)
            {
                history.GeDa++;
                history.GeXiao = 0;
            }
            else if (history.Single >= 5)
            {
                history.GeXiao++;
                history.GeDa = 0;
            }
            return history;
        }



    }

}
