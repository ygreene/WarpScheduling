using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace WarpScheduling
{
    class WarpCustomers :INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<WarpCustomers> WrpCustomers = new List<WarpCustomers>();
        public string WarpMO { get; set; }
        public string  FinishMO { get; set; }
        public string Customer { get; set; }
        public int Priority { get; set; }
        public string ItemNumber { get; set; }
        public int Tickets { get; set; }
        public DateTime DueDate { get; set; }
        public string MarketOrder { get; set; }
        private string _Notes;
        public string Notes { get { return _Notes; } set {_Notes=value;  OnPropertyChanged(nameof(Notes)); } }
       public  bool  JKRush  { get { return MORush.IsMOARush(this.FinishMO); }  }
        public string SingleDouble { get; set; }


        internal static string CheckForRushedMO(string WrpMO)
        {
            string isrush = "";
           var x= WrpCustomers.Where(c => c.WarpMO == WrpMO && c.JKRush==true);
            if (x.Count()>0)
            { isrush = "RUSH"; }
            return isrush;
            
        }
        protected void OnPropertyChanged(string propertyName)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (PropertyChanged!=null)
          {
              
                UpdatePlanLogNotes(this);
            }


            }

        internal static int GetStyleChanges(string warpMO)
        {
            var x = WrpCustomers.Where(c => c.WarpMO == warpMO).OrderBy(g=> g.Priority);
            return x.LastOrDefault().Priority ;
        }

        internal static string GetSingleDouble(string warpMO)
        {
            var x = WrpCustomers.Where(c => c.WarpMO == warpMO);
            if (x.Count() > 0)
            { return x.ElementAt(0).SingleDouble; }
            else { return "S"; }
        }


        //public  static void testmysqlsqp()
        //{
        //    MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
        //    MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.StoredProcedure, CommandText= "sp_getwarprk" };
        //    MySqlDataReader reader;
        //    try
        //    {
        //        conn.Open();
        //        // cmd.CommandText = string.Format("Update `Plan Log` Set Notes= '{0}' Where Warp_MO='{1}' and PriorityNumber={2};", warpCustomers.Notes, warpCustomers.WarpMO, warpCustomers.Priority);
        //       reader= cmd.ExecuteReader();
        //       while (reader.Read())
        //            {


        //        }
        //    }
        //    catch (MySqlException ex)
        //    {

        //        throw ex;
        //    }
        //    finally { conn.Close(); conn.Dispose(); }
        //}

    

        private void UpdatePlanLogNotes(WarpCustomers warpCustomers)
        {
            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };

            try
            {
                conn.Open();
               cmd.CommandText = string.Format("Update `Plan Log` Set Notes= '{0}' Where Warp_MO='{1}' and PriorityNumber={2};", warpCustomers.Notes, warpCustomers.WarpMO, warpCustomers.Priority);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {

                throw ex;
            }
            finally { conn.Close();conn.Dispose(); }
        }

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
                    WrpCustomers.Add(new WarpCustomers() { WarpMO = reader.GetString(0), Customer = reader.GetString(1), Priority=reader.GetInt32(2),ItemNumber = reader.GetString(3), Tickets = reader.GetInt32(4) ,DueDate=reader.GetDateTime(5), MarketOrder=reader.GetString(6), Notes=reader .GetString (7), FinishMO=reader.GetString(8), SingleDouble=reader.GetString(10)});
                
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

