using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WarpScheduling
{
    class Warp
    {
       public static List<Warp> Warps = new List<Warp>();
        public int Priority { get; set; }
        public string WarpMO { get; set; }
        public string WarpStyle { get; set; }
        public int TotalTickets { get; set; }
        public DateTime EarliestDueDate { get; set; }
        public string YarnsColorsOfWarp { get; set; }
        public string Notes { get; set; }


        public static void FetchNewWarps()
        {

            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
            MySqlDataReader reader;

            try
            {
                conn.Open();
                cmd.CommandText = Properties.Resources.NewWarps;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Warps.Add(new Warp() { WarpMO = reader.GetString(0), WarpStyle = reader.GetString(1), TotalTickets = reader.GetInt32(2), EarliestDueDate = reader.GetDateTime(3) });
                }
            }
            catch ( Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();conn.Dispose();
            }

        }



    }

//    SELECT p.Warp_MO, p.Expr1, Sum(p.Tickets) TotalRolls, Min(DueDate) FROM manufacturing.`plan log` p
//Group by p.Warp_MO, p.Expr1
//Order by Warp_MO;
}
