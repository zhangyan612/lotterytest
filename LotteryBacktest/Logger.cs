using System;  
using System.IO;

namespace LotteryBacktest
{
    public static class Logger
    {
        public static void Out(string str)
        {
            Console.WriteLine(str);
            string time = DateTime.Now.ToString("yyyyMMddHH");
            string logname = string.Format("./Logs/{0}.txt", time);
            using (StreamWriter file = new StreamWriter(logname, true))
            {
                file.WriteLine(str);
            }
        }
    }

}
