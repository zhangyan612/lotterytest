using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace LotteryBacktest
{
    class SqlConnection
    {
        SqlConnection myconcection;


        //private void getConnection()
        //{
        //    String connstring = ""; //your connection string
        //    SqlConnection myconcection = new SqlConnection(connstring);
        //}


        //// method to get data
        //public DataSet getData(String sql)
        //{
        //    getConnection();

        //    SqlDataAdapter adptre = new SqlDataAdapter();
        //    DataSet resultSet = new DataSet();

        //    SqlCommand sqlcmd = new SqlCommand(sql, myconcection);
        //    adptre.SelectCommand = sqlcmd;
        //    myconcection.Open();
        //    try
        //    {
        //        adptre.Fill(resultSet);
        //    }

        //    catch (Exception ex)
        //    {

        //    }
        //    myconcection.Close();
        //    return resultSet;
        //}

        //// method to add data
        //public void setData(string sqlcmd)
        //{
        //    getConnection();
        //    myconcection.Open();

        //    string sqlcommand = sqlcmd;
        //    SqlCommand cmd = new SqlCommand(sqlcommand, myconcection);
        //    cmd.ExecuteNonQuery();
        //    myconcection.Close();
        //}

    }

}
