using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;
using System.Globalization;

namespace LotteryBacktest
{
    public class BrowserTest
    {
        //Test on Selenium System
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;

        private static readonly Random intRandom = new Random();
        private static readonly object syncLock = new object();
        private static readonly Random intRandomList = new Random();


        public decimal AccountBalance { get; set; }
        public bool Winning { get; set; }
        public decimal TestAccountBalance { get; set; }
        public List<int> LastNumber { get; set; }
        public bool StopLoss { get; set; }


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

        public void run()
        {
            driver = new FirefoxDriver();
            baseURL = "http://fulibao7.com";
            verificationErrors = new StringBuilder();

            driver.Navigate().GoToUrl(baseURL + "/");
            Thread.Sleep(5000);

            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("zhangyanwin");
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("zymeng90612");
            driver.FindElement(By.Id("validCode")).Clear();

            Console.WriteLine("Please input verification code");
            string code = Console.ReadLine();

            driver.FindElement(By.Id("validCode")).SendKeys(code);
            driver.FindElement(By.Id("subDiv")).Click();

            // 等待页面load 
            //driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 0, 10));
            Thread.Sleep(5000);


            // 刷新页面
            driver.Navigate().GoToUrl(baseURL + "/Home/Main");
            string url = driver.Url;
            Console.WriteLine("Current page: " + url);

            // 我的余额
            string balance = driver.FindElement(By.Id("usermoney")).Text;
            Console.WriteLine("Current balance: " + balance);
            AccountBalance = Convert.ToDecimal(balance);

            bool GameOver = false;
            if (Convert.ToDecimal(balance) <= 300)
            {
                EmailClient.Sending("本金不足, 请添加资金！！！");
                Logger.Out("本金不足, 请添加资金！！！");

            }
            if (Convert.ToDecimal(balance) <= 100)
            {
                GameOver = true;
                EmailClient.Sending("爆仓风险, 请添加资金！！！");
                Logger.Out("爆仓风险, 请添加资金！！！");
            }

            // 初始化变量
            int initialBet = 5;
            int TotalBet = 5;
            Winning = true;
            //Database db = new Database();
            //TestAccountBalance = 1000M;  // 测试账户资金
            MongoDAO db = new MongoDAO();

            do
            {
                // 游戏页面
                driver.SwitchTo().Frame("gameiframe");

                // 正在销售期号
                string expect = driver.FindElement(By.Id("newqh")).Text;
                Logger.Out("Current Selling: " + expect);

                // 剩余时间
                string TimeRemain = driver.FindElement(By.Id("djs")).Text;
                TimeSpan ts = TimeSpan.Parse(TimeRemain);
                Logger.Out("Time Remain: " + ts);
                int Millisecond = Convert.ToInt32(ts.Add(new TimeSpan(0, 1, 0)).TotalMilliseconds);

                // 点击定位
                driver.FindElement(By.XPath("//div[@id='swanfa']/div/ul/li[8]/div")).Click();

                Thread.Sleep(2000);


                // 选号
                List<int> PickedNumber = RandomIntList(0, 9);
                if (!Winning)
                {
                    PickedNumber = LastNumber;
                }

                foreach (var i in PickedNumber)
                {
                    string click = "wuwei" + i;
                    Logger.Out("Clicking: " + click);
                    driver.FindElement(By.Id(click)).Click();
                }

                // 点击并清除原有数据
                driver.FindElement(By.Id("btadd")).Click(); 
                driver.FindElement(By.Id("dzje")).Clear();

                // 根据上次输赢情况下注
                int bet = TotalBet / 5;
                if (Winning == true)
                {
                    driver.FindElement(By.Id("dzje")).SendKeys(bet.ToString()); // 每注金额
                    Logger.Out("Bet " + TotalBet + " in this round");
                }
                else if (Winning == false)
                {
                    driver.FindElement(By.Id("dzje")).SendKeys(bet.ToString());
                    Logger.Out("Bet " + TotalBet + " in this round");
                }
                else
                {
                    throw (new Exception("Unable to decide whether winning"));
                }
                driver.FindElement(By.Id("submitbt")).Click(); // 确认投注

                // 点击确认投注窗口
                driver.FindElement(By.XPath("(//button[@type='button'])[1]")).Click(); // 确认投注

                // 取消作为测试
                //driver.FindElement(By.XPath("(//button[@type='button'])[2]")).Click(); // 取消投注

                // 页面转换等待开奖
                Thread.Sleep(1000);
                Thread.Sleep(Millisecond); //Millisecond

                driver.Navigate().GoToUrl(baseURL + "/Home/Main");
                driver.SwitchTo().Frame("gameiframe");

                // 拿到开奖号码以及中奖信息
                bool openning = driver.FindElement(By.Id("sopencode")).Displayed;
                do
                {
                    Logger.Out("We have to wait for opening");
                    Thread.Sleep(1000 * 10);  // sleep 10 seconds
                    openning = driver.FindElement(By.Id("sopencode")).Displayed;

                    // Refresh Page if needed

                } while (openning);
                Logger.Out("Game Opened!");

                // 上期期号
                string opened = driver.FindElement(By.Id("oldqh")).Text;

                // 确定输赢
                int winNumber;
                if (opened == expect)
                {
                    winNumber =Convert.ToInt32(driver.FindElement(By.Id("hm5")).Text);
                    Logger.Out("Winning Number" + winNumber);
                    Winning = PickedNumber.Contains(winNumber) ? true : false;  
                }
                else
                {
                    throw (new Exception("There is an error getting opened game data"));
                }

                StopLoss = false;
                // 本次数据计算以及存入数据库
                string Picked = string.Join("", PickedNumber.ToArray());
                driver.Navigate().GoToUrl(baseURL + "/Home/Main");
                AccountBalance = Convert.ToDecimal(driver.FindElement(By.Id("usermoney")).Text);

                string message = string.Format("Expect: {0}, Picked Number: {1}, Winning Number: {2}, Win: {3}, Balance: {4}, Bet: {5} ",
                    expect, Picked, winNumber, Winning, AccountBalance, TotalBet);
                Logger.Out(message);

                //TestAccountBalance = BalanceCalculation(TestAccountBalance, TotalBet, Winning);

                db.SaveRealData(expect, Picked, winNumber, Winning, AccountBalance, TotalBet); // 更改账户余额

                Logger.Out("Deciding Next bet based on result...");
                // 决定下次下注规则
                if (Winning)
                {
                    TotalBet = initialBet; // 重置Bet
                    Console.WriteLine("Win Game, Next Bet stays inital: " + TotalBet);
                }
                else if (!Winning)
                {
                    int lastBet = TotalBet;
                    TotalBet = ConstantStop(TotalBet, 3, AccountBalance, initialBet); // 下注规则，决定下次下注金额
                    LastNumber = PickedNumber; // 决定下次号码
                    bool stopLoss = TotalBet < lastBet ? true : false;
                    Logger.Out("Lost, Bet increased to: " + TotalBet + " Stop Loss: " + stopLoss);
                    // db.update real time data
                    if (stopLoss)
                    {
                        db.updateStopLoss(expect);
                    }
                }
                else
                {
                    throw (new Exception("Unable to decide next bid"));
                }

            } while (!GameOver);

            
        }

        public int BetStrategy(int bet, int increment, decimal balance, int initialBet)
        {
            //止损规则 1/6 仓位止损
            //记录止损
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

        public int ConstantStop(int bet, int increment, decimal balance, int initialBet)
        {
            //止损规则 1/8 仓位止损
            //记录止损
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



        public decimal BalanceCalculation(decimal fund, decimal bet, bool win)
        {
            if (win)
            {
                fund = fund + bet * 1.965M;
            }
            else
            {
                fund = fund - bet;
            }
            return fund;
        }



    }
}
