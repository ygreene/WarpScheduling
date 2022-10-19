using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace WarpScheduling
{
    class WarpBill
    {
        public static List<WarpBill> WarpBills = new List<WarpBill>();
        public string WarpStyle { get; set; }
        public string Component { get; set; }
        public string ComponentDesc { get; set; }
        private string compcolor;
        public string CompColor
        {
            get { return compcolor; }
            set
            {
                compcolor = value.Substring(value.LastIndexOf("-")+1, value.Length - value.LastIndexOf("-")-1);
            }
        }

        public static void FetchWarpBill()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.fsconn };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text, CommandText = Properties.Resources.WBIll };
            SqlDataReader reader;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    if (reader.GetString(0)=="WS1000")
                    {
                        Console.WriteLine("Got It");

                    }
                    WarpBills.Add(new WarpBill() { WarpStyle = reader.GetString(0), Component = reader.GetString(1), ComponentDesc = reader.GetString(2) , CompColor=reader.GetString(2)});

                }
            }
            catch ( SqlException ex)
            {

                throw ex;
            }
            finally { conn.Close(); conn.Dispose(); }

        }
        public static string FetchWarpColors(string warpstyle)
        {
            String yarncolors = "";
            var x = WarpBills.Where(w => w.WarpStyle == warpstyle.Trim());

            foreach (var i in x )
            {
                yarncolors  += i.compcolor + "/";
            }

            //erase this please and put  return yarncolors.Substring(0, yarncolors.Length - 1); 
            if (yarncolors.Length == 0)
            {
                return yarncolors = "no data";
            }
            else
            {
                return yarncolors.Substring(0, yarncolors.Length - 1); 
            }
          
        }
    }
}
