using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryBacktest
{
    public class ApiData
    {
        public int ID { get; set; }
        public string Expect { get; set; }
        public string Opencode { get; set; }
        public DateTime Opentime { get; set; }
        public string Opentimestamp { get; set; }

        public int Single
        {
            get { return Convert.ToInt32(Opencode.Substring(8)); }
            set { single = value; }
        }

        private int single;

    }
}
