using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LotteryBacktest
{
    public class Scheduler
    {
        public void Schedule()
        {
            Console.WriteLine("App Started at " + DateTime.Now);
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay; //get current time
            //TimeSpan testHour = new TimeSpan(2, 35, 23);

            TimeSpan startTime = new TimeSpan(9, 30, 00);
            TimeSpan interTime = new TimeSpan(22, 00, 00);
            TimeSpan endTime = new TimeSpan(2, 0, 0);

            if(CurrentTime > startTime && CurrentTime < interTime)
            {
                Console.WriteLine("Regular Timer is counting!");
                Timer aTimer = new Timer(10000 * 6 * 10); // run every 10 second * 6 
                aTimer.Start();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            }
            if (CurrentTime > interTime || CurrentTime < endTime)
            {
                Console.WriteLine("Faster Timer is counting!");
                Timer bTimer = new Timer(6000 * 10 * 5); // run every 5 second
                bTimer.Start();
                bTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            }

        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RealData code = new RealData();
            code.getData();

        }
    }
}
