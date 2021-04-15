using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WarpScheduling
{
    
    class WarpInventory
    {
      internal  static List<WarpInventory> CurrentWarpInventory = new List<WarpInventory>();

        public string ItemNumber { get; set; }
        public string  LotNumber { get; set; }
        public string  StockRoom { get; set; }

        public static void FetchCurrentWarpInv()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.fsconn };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
            SqlDataReader reader;

            try
            {
                conn.Open();
                cmd.CommandText = "Select i.ItemNumber, ii.LotNumber, ii.Stockroom from dbo.FS_Item i inner join dbo.FS_ItemInventory ii " +
                                    "On i.ItemKey = ii.ItemKey " +
                                    "Where i.ItemNumber Like 'WS%'";
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                    CurrentWarpInventory.Add(new WarpInventory() { ItemNumber = reader.GetString(0), LotNumber = reader.GetString(1), StockRoom=reader.GetString(2) });

                     }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();conn.Dispose();

            }
        }
        internal static bool CheckWarpInventory(string warp)
        {
           if ( CurrentWarpInventory.Where(w => w.LotNumber == warp).Count()>0)
            { return true; }
            else { return false; }

        }

    }
}

//Select i.ItemNumber, ii.LotNumber, ii.Stockroom from dbo.FS_Item i inner join dbo.FS_ItemInventory ii
//On i.ItemKey = ii.ItemKey
//Where i.ItemNumber Like 'WS%'
