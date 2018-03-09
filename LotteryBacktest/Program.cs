using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LotteryBacktest.Logic;

namespace LotteryBacktest
{
    class Program
    {

        static void Main(string[] args)
        {
            //BackTest on real history data
            BackTest test = new BackTest();
            test.RealDataTest();

            //SmokeTesting smoke = new SmokeTesting();
            //smoke.Run();

            //Import History data to db
            //FileReader historydata = new FileReader();
            //historydata.readtxt();

            //Retrieve real time data from API
            //RealData api = new RealData();
            //api.getData();

            //Run scheduler
            //Scheduler run = new Scheduler();
            //run.Schedule();

            //BackTest test = new BackTest();
            //test.ParseTest();

            // Real time program

            //BrowserTest test = new BrowserTest();
            //test.run();
            //try
            //{
            //    BrowserTest test = new BrowserTest();
            //    test.run();
            //}
            //catch (Exception ex)
            //{
            //    Logger.Out(ex.Message);
            //    EmailClient.Sending(ex.Message);
            //    //Alert warning = new Alert();
            //    //warning.Sound();
            //}

            //MongoDAO db = new MongoDAO();
            //db.updateStopLoss();

            //HistoryParse.parse();

            //Redirect.Write();


            Console.ReadKey();
        }

    }

}
