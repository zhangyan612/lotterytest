using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace LotteryBacktest
{
    public class Alert
    {
        public void Sound()
        {
            using (SoundPlayer player = new SoundPlayer("./Sound/alert.wav"))
            {
                do
                {
                    player.PlaySync();
                } while (true);
            }
        }

        public static void Monitor()
        {
            using (SoundPlayer player = new SoundPlayer("./Sound/alert.wav"))
            {
                for(var i = 0; i<= 5; i++)
                {
                    player.PlaySync();

                }
            }
        }

        public void testcode()
        {
            // Something should break here
            throw(new Exception("There is an error. System went down!!!"));
        }


        public void testlog()
        {
            Logger.Out("Test log file write");
        }

    }
}
