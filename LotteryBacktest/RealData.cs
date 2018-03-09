using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Timers;

namespace LotteryBacktest
{
    public class RealData
    {
        //http://f.apiplus.cn/cqssc.xml  时时彩API 接口
        //http://t.apiplus.cn/newly.do?token=demo&code=cqssc 演示接口
        // http://t.apiplus.cn/newly.do?code=cqssc&rows=20&format=json

        // 验证码识别
        // http://www.cnblogs.com/yuanbao/archive/2007/09/25/905322.html
        // http://bbs.csdn.net/topics/390309042/
        // http://outofmemory.cn/code-snippet/3086/C-yanzheng-code-identify-class
        // http://www.oschina.net/code/snippet_2253319_46533
        // http://www.shangxueba.com/jingyan/1831143.html

        // Web scrapping 
        // http://watin.org/
        // https://github.com/SimpleBrowserDotNet/SimpleBrowser
        // http://seleniumhq.org/ Selenium WebDriver 2.53.0


        public DateTime OpenTime { get; set; }

        // Retrieve real data and do testing on it, store all data in db
        public void getData()
        {
            var url = "http://t.apiplus.cn/newly.do?code=cqssc&rows=20&format=json";
            using (var data = new WebClient())
            {
                // try catch block here
                var json_data = data.DownloadString(url);
                var result = JsonConvert.DeserializeObject<dynamic>(json_data);
                List<ApiData> api = result.data.ToObject<List<ApiData>>();

                DateTime CurrentTime = DateTime.Now;
                Console.WriteLine("Current time: " + CurrentTime);

                Database db = new Database();
                ApiData lastdata = db.GetLastApiData();
                var dbdate = lastdata.Opentime;
                // Save data to db
                foreach (var i in api)
                {
                    // Expect calculation for a day
                    if(i.Opentime > dbdate)
                    {
                        // calculate current expect and compare
                        db.SaveApiData(i);
                        Console.WriteLine("expect: {0}, opencode:{1}, opentime:{2}, opentimestamp:{3}, winning:{4}",
                        i.Expect, i.Opencode, i.Opentime, i.Opentimestamp, i.Single);
                    }
                }
            }
        }

        public void CurrentExpect()
        {
            DateTime CurrentTime = DateTime.Now;
            var date = CurrentTime.ToString("yyyyMMdd");
            Console.WriteLine(date);

            DateTime testTime = new DateTime(2016, 6, 27, 21, 50, 55);
            TimeSpan tSpan = new TimeSpan(10, 0, 0);

            var time = testTime - tSpan;
            int minute = time.Hour * 60 + time.Minute;

            int count = minute / 10;

            Console.WriteLine(minute);
            //return "";
        }

        public bool? WinCalculation(string currentexpect, List<int> PickedNumber)
        {
            var url = "http://t.apiplus.cn/newly.do?code=cqssc&rows=20&format=json";
            using (var data = new WebClient())
            {
                // try catch block here
                var json_data = data.DownloadString(url);
                var result = JsonConvert.DeserializeObject<dynamic>(json_data);
                List<ApiData> api = result.data.ToObject<List<ApiData>>();
                // Contain test
                bool contains = api.Any(p => p.Expect == currentexpect);
                if (!contains)
                {
                    throw (new Exception("API does not contain current expect"));
                }

                foreach (var i in api)
                {
                    if (i.Expect == currentexpect)
                    {
                        // get time data
                        OpenTime = i.Opentime;

                        if (PickedNumber.Contains(i.Single))
                        {
                            Console.WriteLine("Winning Number: " + i.Single);
                            return true;
                        }
                        if (!PickedNumber.Contains(i.Single))
                        {
                            Console.WriteLine("Not win, Number is: " + i.Single);
                            return false;
                        }
                    }
                }
                return null;
            }
        }

        public TimeSpan TimeLeft(string currentexpect)
        {
            var url = "http://t.apiplus.cn/newly.do?code=cqssc&rows=20&format=json";
            using (var data = new WebClient())
            {
                // try catch block here
                var json_data = data.DownloadString(url);
                var result = JsonConvert.DeserializeObject<dynamic>(json_data);
                List<ApiData> api = result.data.ToObject<List<ApiData>>();
                // Contain test
                int lastExpect = Convert.ToInt32(currentexpect) - 1;

                bool contains = api.Any(p => p.Expect == lastExpect.ToString());
                if (!contains)
                {
                    throw (new Exception("API does not contain current expect"));
                }

                foreach (var i in api)
                {
                    if (i.Expect == lastExpect.ToString())
                    {
                        DateTime CurrentTime = DateTime.Now;
                        // Not complete yet
                    }
                }
                return new TimeSpan(0,0,30);
            }
        }

        public void testcocd()
        {
            Console.WriteLine("This code run every 5 seconds");
        }



    }
}
