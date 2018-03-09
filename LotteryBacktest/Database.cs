using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web;

namespace LotteryBacktest
{
    public class Database
    {
        string strConnection = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString.ToString();
        //string strConnection = @"Data Source=DELL\SQLEXPRESS;Initial Catalog=lottery;Integrated Security=True;MultipleActiveResultSets=True";

        void InsertHistory(string date, string winNumber, string single)
        {
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO [lottery].[dbo].[history] ([Date],[WinNumber],[Single])" +
                                            "VALUES (@Date,@WinNumber,@Single)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.Parameters.AddWithValue("@WinNumber", winNumber);
            cmd.Parameters.AddWithValue("@Single", single);
            
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public List<History> GetHistoryList()
        {
            List<History> _lst = new List<History>();
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("Select * from [lottery].[dbo].[History] order by id desc", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                History data = new History();
                data.ID = Convert.ToInt32(dr["ID"].ToString());
                data.Date = dr["Date"].ToString();
                data.WinNumber = dr["WinNumber"].ToString();
                data.Single = dr["Single"].ToString();

                _lst.Add(data);
            }
            return _lst;
        }

        public void SaveStatistics(History history, decimal balance, Guid guid, DateTime time, int bet, string pickedNumber, bool stopLoss, bool WhetherWin, string Note)
        {
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO [lottery].[dbo].[BacktestResult] ([ID],[Date],[Single],[Balance],[GUID],[Time],[Bet],[PickedNumber],[StopLoss],[WhetherWin],[Note])" +
                                            "VALUES (@ID, @Date, @Single, @Balance, @GUID, @Time, @Bet, @PickedNumber, @StopLoss,@WhetherWin,@Note)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", history.ID);
            cmd.Parameters.AddWithValue("@Date", history.Date);
            cmd.Parameters.AddWithValue("@Single", history.Single);
            cmd.Parameters.AddWithValue("@Balance", balance);
            cmd.Parameters.AddWithValue("@GUID", guid);
            cmd.Parameters.AddWithValue("@Time", time);
            cmd.Parameters.AddWithValue("@Bet", bet);
            cmd.Parameters.AddWithValue("@PickedNumber", pickedNumber);
            cmd.Parameters.AddWithValue("@StopLoss", stopLoss);
            cmd.Parameters.AddWithValue("@WhetherWin", WhetherWin);
            cmd.Parameters.AddWithValue("@Note", Note);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public void SaveApiData(ApiData data)
        {
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO [lottery].[dbo].[ApiData] ([Expect],[Opencode],[Opentime],[Opentimestamp],[Single])" +
                                            "VALUES (@Expect, @Opencode, @Opentime, @Opentimestamp, @Single)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Expect", data.Expect);
            cmd.Parameters.AddWithValue("@Opencode", data.Opencode);
            cmd.Parameters.AddWithValue("@Opentime", data.Opentime);
            cmd.Parameters.AddWithValue("@Opentimestamp", data.Opentimestamp);
            cmd.Parameters.AddWithValue("@Single", data.Single);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void SaveTotalData(Guid newID, decimal InitialFund, int DrawingTimes, int InitialBet, int Multiplier, string Note, int TotalCount, int WinCount, double Percentage, decimal AccountBalance, decimal WinAmount)
        {
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO [lottery].[dbo].[TotalTestData] ([GUID],[InitialFund],[DrawingTimes],[InitialBet],[Multiplier]" +
                                            ",[Note],[TotalCount],[WinCount],[Percentage],[AccountBalance],[WinAmount])" +
                                            "VALUES (@GUID, @InitialFund, @DrawingTimes, @InitialBet, @Multiplier, @Note, @TotalCount, @WinCount, @Percentage, @AccountBalance, @WinAmount)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@GUID", newID);
            cmd.Parameters.AddWithValue("@InitialFund", InitialFund);
            cmd.Parameters.AddWithValue("@DrawingTimes", DrawingTimes);
            cmd.Parameters.AddWithValue("@InitialBet", InitialBet);
            cmd.Parameters.AddWithValue("@Multiplier", Multiplier);
            cmd.Parameters.AddWithValue("@Note", Note);
            cmd.Parameters.AddWithValue("@TotalCount", TotalCount);
            cmd.Parameters.AddWithValue("@WinCount", WinCount);
            cmd.Parameters.AddWithValue("@Percentage", Percentage);
            cmd.Parameters.AddWithValue("@AccountBalance", AccountBalance);
            cmd.Parameters.AddWithValue("@WinAmount", WinAmount);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void SaveRealData(string expect, string Picked, int winnumber, bool whetherwin, decimal balance, int bet)
        {
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO [lottery].[dbo].[RealData] ([Expect],[Picked],[WinNumber],[WhetherWin],[Time],[Balance],[Bet])" +
                                            "VALUES (@Expect, @Picked, @WinNumber, @WhetherWin, @Time, @Balance, @Bet)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Expect", expect);
            cmd.Parameters.AddWithValue("@Picked", Picked);
            cmd.Parameters.AddWithValue("@WinNumber", winnumber);
            cmd.Parameters.AddWithValue("@WhetherWin", whetherwin);
            cmd.Parameters.AddWithValue("@Time", DateTime.Now);
            cmd.Parameters.AddWithValue("@Balance", balance);
            cmd.Parameters.AddWithValue("@Bet", bet);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public void SaveHistoryTrend(HistoryCount data)
        {
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO [lottery].[dbo].[HistoryTrend] ([Date],[WinNumber]," +
                "[WanDa],[WanXiao],[QianDa],[QianXiao],[BaiDa],[BaiXiao],[ShiDa],[ShiXiao],[GeDa],[GeXiao])" +
                                            "VALUES (@Date, @WinNumber, " +
                 "@WanDa,@WanXiao, @QianDa,@QianXiao,@BaiDa,@BaiXiao,@ShiDa,@ShiXiao,@GeDa,@GeXiao)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Date", data.Date);
            cmd.Parameters.AddWithValue("@WinNumber", data.WinNumber);
            cmd.Parameters.AddWithValue("@WanDa", data.WanDa);
            cmd.Parameters.AddWithValue("@WanXiao", data.WanXiao);
            cmd.Parameters.AddWithValue("@QianDa", data.QianDa);
            cmd.Parameters.AddWithValue("@QianXiao", data.QianXiao);
            cmd.Parameters.AddWithValue("@BaiDa", data.BaiDa);
            cmd.Parameters.AddWithValue("@BaiXiao", data.BaiXiao);
            cmd.Parameters.AddWithValue("@ShiDa", data.ShiDa);
            cmd.Parameters.AddWithValue("@ShiXiao", data.ShiXiao);
            cmd.Parameters.AddWithValue("@GeDa", data.GeDa);
            cmd.Parameters.AddWithValue("@GeXiao", data.GeXiao);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public ApiData GetLastApiData()
        {
            ApiData data = new ApiData();
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("SELECT top 1 * from [lottery].[dbo].[ApiData] order by Opentime desc", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                data.ID = Convert.ToInt32(dr["ID"].ToString());
                data.Expect = dr["Expect"].ToString();
                data.Opentime = Convert.ToDateTime(dr["Opentime"]);
                data.Opencode = dr["Opencode"].ToString();
                data.Single = Convert.ToInt32(dr["Single"].ToString());
            }
            con.Close();
            return data;
        }

        public History GetHistoryData(int id)
        {
            History data = new History();
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("SELECT * from [lottery].[dbo].[History] Where id = @ID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                data.ID = Convert.ToInt32(dr["ID"].ToString());
                data.Date = dr["Date"].ToString();
                data.WinNumber = dr["WinNumber"].ToString();
                data.Single = dr["Single"].ToString();
            }
            con.Close();
            return data;
        }

        public HistoryCount GetHistoryTrend(string date)
        {
            HistoryCount data = new HistoryCount();
            SqlConnection con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand("SELECT * from [lottery].[dbo].[HistoryTrend] Where [Date] = @Date", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Date", date);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                data.ID = (int)dr["ID"];
                data.Date = dr["Date"].ToString();
                data.WinNumber = dr["WinNumber"].ToString();
                data.GeDa = (int)dr["GeDa"];
                data.GeXiao = (int)dr["GeXiao"];
            }
            con.Close();
            return data;
        }



    }
}
