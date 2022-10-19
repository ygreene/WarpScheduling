using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySTI.STI;
using System.Data;

namespace WarpScheduling
{
    class WarpPriorityHistory
    {
        internal static List<WarpPriorityHistory> wpHistory;
        public string WarpMO { get; set; }


        internal static void FetchWarpHistory()
        {
            Definitions.sql_user = string.Format("Select * From t_WarpingPriorityHistory Where DateScheduled > Dateadd(m, -6, GetDate())");
            DataTable dt = connection.Generate(Definitions.sql_user, Definitions.STIdb, Definitions.baseS).Tables[0];

            wpHistory = (from rw in dt.AsEnumerable()

                         select new WarpPriorityHistory()
                         {
                             WarpMO = Convert.ToString(rw["Warp_MO"])
                         }
                         ).ToList();
        }
       internal static bool IsWarpInWarpPriorityHistoryTable(string warpmo)
        {
            bool inhistory = false;

            if (wpHistory.Where(w => w.WarpMO == warpmo).Count()>0)
                { inhistory = true;}

            return inhistory;
        }

        internal static void SaveWarpHistory(Warp w)
        {
            string sqlvalue = string.Format("Insert into t_WarpingPriorityHistory (Warp_MO, WarpStyle,WarperID,DateScheduled) Values('{0}','{1}', {2},'{3}')", w.WarpMO, w.WarpStyle, w.WarperID, DateTime.Now);
            connection.Execute(sqlvalue, Definitions.STIdb, Definitions.baseS);
        }

    }
}
