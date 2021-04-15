using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WarpScheduling
{
    class Customer
    {

        public string CustID { get; set; }
        public string CustomerName { get; set; }

        internal static List<Customer> FetchCustomers()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.fsconn };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
            SqlDataReader reader;
            List<Customer> cl = new List<Customer>();
            try
            {
                conn.Open();
                cmd.CommandText = "Select c.CustomerID, c.CustomerName From dbo.FS_Customer c Where c.CustomerStatus = 'A'";
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    cl.Add(new Customer() { CustID = reader.GetString(0), CustomerName = reader.GetString(1) });
                }
                return cl;
            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); conn.Dispose(); }

        }
    }
}
