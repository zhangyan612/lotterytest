using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LotteryBacktest
{
    public class FileReader
    {
        public static void Main3()
        {
            //直接读取出字符串
            //string text = File.ReadAllText(@"C:\Users\Administrator\Desktop\LotteryBacktest\LotteryBacktest\Files\LotteryA.txt");
            //Console.WriteLine(text);

            ////按行读取为字符串数组
            //string[] lines = File.ReadAllLines(@"C:\testDir\test.txt");
            //foreach (string line in lines)
            //{
            //    Console.WriteLine(line);
            //}

            
        }

        //other ways
        public void readtxt()
        {
            //从头到尾以流的方式读出文本文件
            //该方法会一行一行读出文本
            using (var reader = new StreamReader(@"C:\Users\Administrator\Desktop\LotteryBacktest\LotteryBacktest\Files\LotteryA.txt"))
            {
                string line;
                Database db = new Database();

                while ((line = reader.ReadLine()) != null)
                {
                    var date = line.Substring(0, 12);
                    var winningNub = line.Substring(13);
                    var single = line.Substring(17);
                    //db.InsertHistory(date, winningNub, single);
                    Console.WriteLine("Date: {0}, Win Number: {1}, Single: {2}", date, winningNub, single);
                }
            }
            Console.Read();
        }

        static void Main2(string[] args)
        {
            string filePath = @"c:temptest.txt";
            string line;

            if (File.Exists(filePath))
            {
                StreamReader file = null;
                try
                {
                    file = new StreamReader(filePath);
                    while ((line = file.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }

            Console.ReadLine();
        }




    }

}
