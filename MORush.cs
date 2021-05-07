using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WarpScheduling
{
    class MORush
    {
        public static List<MORush> Rushes = new List<MORush>();
        public string  MONumber { get; set; }
        public string ItemNumber { get; set; }


        internal static Boolean IsMOARush(string mo)
        {
            Boolean isrush = false;

            var x = Rushes.Where(r => r.MONumber == mo);
            if (x.Count()>0)
            {
                isrush = true;
            }
            return isrush;
        
        }

    
       internal static void RemoveStatus5MOs()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text, CommandText =Properties.Resources.RemoveFromRushTable };

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally { conn.Close();conn.Dispose(); }

        }
        internal static void FetchMORushList()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
            SqlDataReader reader;

            try
            {
                conn.Open();
                cmd.CommandText = "Select MONumber, ItemNumber From STI.dbo.ChaseListMOs";
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Rushes.Add(new MORush() { MONumber = reader.GetString(0), ItemNumber = reader.GetString(1) });
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();

            }
        }


    }
}
//SELECT  [ItemNumber]
//      ,[Chase]
//      ,[AddText]
//      ,[MONumber]
//FROM[STI].[dbo].[ChaseListMOs] 