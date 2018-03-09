using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LotteryBacktest
{
    public class Redirect
    {
        public static void Write()
        {
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream("./Logs/Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);
            Console.WriteLine("Program started running!");
            Console.WriteLine("Everything written to Console.Write() or");
            Console.WriteLine("Console.WriteLine() will be written to a file");

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Done");

        }
    }
}
