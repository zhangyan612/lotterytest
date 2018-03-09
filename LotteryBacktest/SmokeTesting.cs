using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryBacktest
{
    public class SmokeTesting
    {
        public void Run()
        {
            //BackTest on real history data
            BackTest test = new BackTest();

            int limit = 50;
            for(var i = 0; i < limit; i++)
            {
                test.RealDataTest();
            }
            

        }
    }
}
