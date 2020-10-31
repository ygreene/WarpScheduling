using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WarpScheduling
{
    class WarpCustomers
    {
        public static List<WarpCustomers> WrpCustomers = new List<WarpCustomers>();
        public string WarpMO { get; set; }
        public string Customer { get; set; }
        public string ItemNumber { get; set; }
        public int Tickets { get; set; }

        public static void FetchWarpCustomers()
        {
            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text, CommandText = Properties.Resources.WpMOCustomer };
            MySqlDataReader reader;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    WrpCustomers.Add(new WarpCustomers() { WarpMO = reader.GetString(0), Customer = reader.GetString(1), ItemNumber = reader.GetString(2), Tickets = reader.GetInt32(3) });
                
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conn.Close();conn.Dispose(); }

        }
        private IEnumerable<WarpCustomers> FetchCustomersOnWarp(string warpmo)
        {
            var x= WrpCustomers.Where(c => c.WarpMO == warpmo);
            return x;
        }
    }
}
