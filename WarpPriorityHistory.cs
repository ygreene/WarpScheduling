using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using LibrarySTI.STI;
using System.Data.SqlClient;

namespace WarpScheduling
{
    class WarpPriorityHistory
    {
       internal static List<WarpPriorityHistory> wpHistory = new List<WarpPriorityHistory> ();
        public string WarpMO { get; set; }


        internal static void FetchWarpHistory()
        {
            SqlConnection conn = new SqlConnection() { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand() { Connection = conn, CommandType = System.Data.CommandType.Text };
            SqlDataReader reader;

            try
            {
               // wpHistory.Add(new WarpPriorityHistory() { WarpMO = "A00000" });
                conn.Open();
                string squery = string.Format("Select * From t_WarpingPriorityHistory Where DateScheduled > Dateadd(m, -6, GetDate())");
                cmd.CommandText = squery;
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                    wpHistory.Add(new WarpPriorityHistory() { WarpMO = reader.GetString(0) });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
    }

        internal static bool IsWarpInWarpPriorityHistoryTable(string warpMO)
        {
            bool isfound = false;
            if (wpHistory==null)
            { return isfound; }
            if (wpHistory.Any(i=> i.WarpMO==warpMO))
            {
                isfound= true;
            }
            return isfound;
        }

        internal static int SaveHistory(Warp warp)
        {
            SqlConnection conn = new SqlConnection() { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand() { Connection = conn, CommandType = System.Data.CommandType.Text };

            cmd.CommandText = string.Format("Insert into dbo.t_WarpingPriorityHistory (Warp_MO, WarpStyle, WarperID, DateScheduled) Values ('{0}','{1}',{2},'{3}')", warp.WarpMO, warp.WarpStyle, warp.WarperID, DateTime.Now);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }


    //    INSERT INTO[dbo].[t_WarpingPriorityHistory]
    //    ([Warp_MO]
    //      ,[WarpStyle]
    //      ,[WarperID]
    //      ,[DateScheduled])
    //VALUES
    //      (<Warp_MO, nvarchar(15),>
    //       ,<WarpStyle, nvarchar(15),>
    //       ,<WarperID, int,>
    //       ,<DateScheduled, datetime,>)

        //     Definitions.sql_user = string.Format("Select * From t_WarpingPriorityHistory Where DateScheduled > Dateadd(m, -6, GetDate())");
        //     DataTable dt = connection.Generate(Definitions.sql_user, Definitions.STIdb, Definitions.baseS).Tables[0];

        //     wpHistory = (from rw in dt.AsEnumerable()

        //                  select new WarpPriorityHistory()
        //                  {
        //                      WarpMO = Convert.ToString(rw["Warp_MO"])
        //                  }
        //                  ).ToList();
    }
       //internal static bool IsWarpInWarpPriorityHistoryTable(string warpmo)
       // {
       //     bool inhistory = false;

       //     if (wpHistory.Where(w => w.WarpMO == warpmo).Count()>0)
       //         { inhistory = true;}

       //     return inhistory;
       // }

       // internal static void SaveWarpHistory(Warp w)
       // {
       //     string sqlvalue = string.Format("Insert into t_WarpingPriorityHistory (Warp_MO, WarpStyle,WarperID,DateScheduled) Values('{0}','{1}', {2},'{3}')", w.WarpMO, w.WarpStyle, w.WarperID, DateTime.Now);
       //    // connection.Execute(sqlvalue, Definitions.STIdb, Definitions.baseS);
       // }

    }

