using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotteryBacktest.Model;

namespace LotteryBacktest.Logic
{
    public class HistoryParse
    {
        public static void parse()
        {
            Database db = new Database();

            History data = new History();
            HistoryCount history = new HistoryCount();

            for (var i = 346800; i > 0; i--)
            {
                data = db.GetHistoryData(i);
                history.WinNumber = data.WinNumber;
                history.Date = data.Date;

                history.Wan = Convert.ToInt32(data.WinNumber.Substring(0, 1));
                history.Thousand = Convert.ToInt32(data.WinNumber.Substring(1, 1));
                history.Hundred = Convert.ToInt32(data.WinNumber.Substring(2, 1));
                history.Tenth = Convert.ToInt32(data.WinNumber.Substring(3, 1));
                history.Single = Convert.ToInt32(data.WinNumber.Substring(4, 1));

                history = bigorsmall(history);

                db.SaveHistoryTrend(history);
                Console.WriteLine("Parsing " + history.Date);
            }

            //data = db.GetHistoryData(348680);




            //Console.WriteLine("{0}: {1},{2},{3},{4},{5}", history.WinNumber, history.Wan,
            //    history.Thousand, history.Hundred, history.Tenth, history.Single);



        }

        public static HistoryCount bigorsmall(HistoryCount history)
        {
            // Wan
            if (history.Wan < 5)
            {
                history.WanDa++;
                history.WanXiao = 0;
            }
            else if (history.Wan >= 5)
            {
                history.WanXiao++;
                history.WanDa = 0;
            }
            // Qian
            if (history.Thousand < 5)
            {
                history.QianDa++;
                history.QianXiao = 0;
            }
            else if (history.Thousand >= 5)
            {
                history.QianXiao++;
                history.QianDa = 0;
            }
            // Bai
            if (history.Hundred < 5)
            {
                history.BaiDa++;
                history.BaiXiao = 0;
            }
            else if (history.Hundred >= 5)
            {
                history.BaiXiao++;
                history.BaiDa = 0;
            }
            // shi
            if (history.Tenth < 5)
            {
                history.ShiDa++;
                history.ShiXiao = 0;
            }
            else if (history.Tenth >= 5)
            {
                history.ShiXiao++;
                history.ShiDa = 0;
            }
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




        public int JiorOu(int num)
        {
            int[] Ji = new int[] { 1, 3, 5, 7, 9 };
            int[] Ou = new int[] { 2, 4, 6, 8, 0 };

            if (Ji.Contains(num))
            {
                return 0; // Ji
            }
            else
            {
                return 1; // Ou
            }
        }



    }
}
