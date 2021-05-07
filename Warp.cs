using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Windows;
using System.ComponentModel;
using System.IO;
using MySqlConnector;

namespace WarpScheduling
{
    class Warp :  INotifyPropertyChanged 
    {
       internal static StreamWriter mywriter = new StreamWriter(@"c:\STIRoot\WarpScheduling");

        public static List<Warp> Warps = new List<Warp>();

        public event PropertyChangedEventHandler PropertyChanged;

        //private string _Notes;
        //public string Notes { get { return _Notes; } set { _Notes = value; OnPropertyChanged(nameof(Notes)); } }
        private int? _Priority;
        public int? Priority { get { return _Priority; } set { _Priority = value;OnPropertyChanged(nameof(Priority)); } }

        public string WarpMO { get; set; }
        public string WarpStyle { get; set; }
        public int TotalTickets { get; set; }
        public DateTime EarliestDueDate { get; set; }
        private string _YarnColorsOfWarp;

      

        public string YarnColorsOfWarp
        {
            get { return _YarnColorsOfWarp; }
            set { _YarnColorsOfWarp = WarpBill.FetchWarpColors(this.WarpStyle); }
        }


        public string Notes { get; set; }
        public int? WarperID { get; set; }
        public string IsRush { get { return WarpCustomers.CheckForRushedMO(this.WarpMO); } }
       public string SingleDouble { get { return (WarpCustomers.GetSingleDouble(this.WarpMO)=="D"?"Double":""); } }
        public int ChangesThisWarp { get { return WarpCustomers.GetStyleChanges(this.WarpMO); } }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (PropertyChanged != null)
            {
                int sr;
                bool success = Int32.TryParse(MainWindow.main.cb_Warper.SelectedValue.ToString(), out sr);

                if (success==true)
                {
                    this.WarperID = sr;
                }
              //  UpdatePlanLogNotes(this);
            }


        }

        internal static bool HasWarpBeenProcessed(string warpmofilter)
        {
            bool isprocessed = false;
            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
            MySqlDataReader reader;
            try
            {

                //if (warpmofilter=="A52914")
                //{
                //    Console.WriteLine("Got it");
                //}
                conn.Open();
                cmd.CommandText = string.Format("Select p.Warp_MO,p.WarpProcessed From manufacturing.`Plan Log` p Where p.Warp_MO ='{0}' and WarpProcessed = 1", warpmofilter);
                reader = cmd.ExecuteReader();
                if (reader.HasRows==true)
                                    {
                    mywriter.WriteLine(string.Format("Warp '{0}' has been processed at HasWarpBeenProcessed!", warpmofilter));
                    Console.WriteLine(warpmofilter);
                                      isprocessed = true;     
                }

                return isprocessed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conn.Close();conn.Dispose(); }

           
        }

        public static void FetchNewWarps()
        {

            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
            MySqlDataReader reader;

            string wpmo;
            try
            {
                conn.Open();
                cmd.CommandText = Properties.Resources.NewWarps;
                List<string> listwarpmo = new List<string>();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wpmo = reader.GetString(0);
                    //if (reader.GetString(0)=="T26719")
                    //{
                    //    Console.WriteLine(wpmo);
                    //}
                  //  Console.WriteLine(wpmo);
                    
                    if (HasWarpBeenProcessed(wpmo) == true)
                    {

                        Console.WriteLine(String.Format("Updating '{0}' to processed", wpmo));
                       // UpdatePlanLogWarpToProcessed(wpmo); // take long time 
                        listwarpmo.Add(wpmo);
                    }
                    else
                    {
                        Warps.Add(new Warp() { WarpMO = wpmo, WarpStyle = reader.GetString(1), TotalTickets = reader.GetInt32(2), EarliestDueDate = reader.GetDateTime(3), YarnColorsOfWarp = "" });
                    }
                }
                if (listwarpmo.Count > 0)
                {
                    UpdatePlanLogWarpToProcessedall(string.Join("','", listwarpmo)); 
                } // take long time solution
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close(); conn.Dispose();
            }

        }

        public static void FetchJacquardWarps()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.fsconn };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
        }
        public static void FetchPriortizedWarps()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text, CommandText = Properties.Resources.PrioritizedWarps };
            SqlDataReader reader;

            try
            {
                List<string> listwarpmo = new List<string>();
                conn.Open();
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
               
             
                while (reader.Read())
                {
                    mywriter.WriteLine(string.Format("Warp '{0}' has been processed at GetPioritizedWarp!", reader.GetString(1)));
                    // UpdatePlanLogWarpToProcessed(reader.GetString(1)); // take long time // added this in to update items added to warps after they have been priortized.  So as to not allow MO's back in that have been processed. 
                    listwarpmo.Add(reader.GetString(1));
                    Warps.Add(new Warp() { Priority = reader.GetInt32(0), WarpMO = reader.GetString(1), WarpStyle = reader.GetString(2), TotalTickets = reader.GetInt32(3), EarliestDueDate = reader.GetDateTime(4), Notes = reader.GetString(5), WarperID = reader.GetInt32(6) });
                }
                UpdatePlanLogWarpToProcessedall(string.Join("','", listwarpmo)); // take long time solution // added this in to update items added to warps after they have been priortized.  So as to not allow MO's back in that have been processed. 
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally { conn.Close(); conn.Dispose(); }
        }

        public static void SavePriority(IEnumerable<Warp> SelectedWarps, int wpmachineid)
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
          //  var x = wp.GroupBy(j => new { j.WarperID }).Select(group => new { warperid = group.Key.WarperID });
            string x;
            var wpid = SelectedWarps.GroupBy(j => new { j.WarperID }).Select(group => new { warperid=group.Key.WarperID });
            foreach (var wid in wpid)
            {
                if (wid.warperid == null)
                { }
                else
                { RemoveExistingPriorityforWarper(int.Parse(wid.warperid.ToString())); }
            }

            try
            {
                conn.Open();

                foreach (var i in SelectedWarps.Where(c => c.WarperID != null))
                {
                    x = string.Format("Insert into dbo.t_WarpingPriority (Priority, Warp_MO, WarpStyle, RollsOnWarp, DueDate, Notes, WarperID) Values({0},'{1}','{2}',{3},'{4}','{5}',{6})", i.Priority, i.WarpMO, i.WarpStyle, i.TotalTickets, i.EarliestDueDate.ToShortDateString(), i.Notes, i.WarperID);
                    cmd.CommandText = x;
                    if (WarpInventory.CheckWarpInventory(i.WarpMO) == false)
                    { cmd.ExecuteNonQuery(); }
                    else
                    { //remove from export}
                        UpdatePlanLogWarpToProcessed(i.WarpMO);
                    }
                    
                }
                MessageBox.Show("Priority Saved!");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            { conn.Close(); conn.Dispose(); }
        }
        private static void UpdatePlanLogWarpToProcessed(string warpmo)
        {
            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };

            try
            {
                conn.Open();
                cmd.CommandText = string.Format("Update Manufacturing.`Plan Log` Set WarpProcessed=1 Where Warp_MO='{0}'", warpmo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conn.Close(); conn.Dispose(); }

        }
        private static void UpdatePlanLogWarpToProcessedall(string warpmo)
        {
            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };

            try
            {
                conn.Open();
                cmd.CommandText = string.Format("Update Manufacturing.`Plan Log` Set WarpProcessed=1 Where Warp_MO in ('{0}')", warpmo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conn.Close(); conn.Dispose(); }

        }
        internal static void UpdatePlanLogWarpToUnProcessed(string warpmo)
        {
            MySqlConnection conn = new MySqlConnection { ConnectionString = Properties.Settings.Default.mysql };
            MySqlCommand cmd = new MySqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };

            try
            {
                conn.Open();
                cmd.CommandText = string.Format("Update Manufacturing.`Plan Log` Set WarpProcessed=0 Where Warp_MO='{0}'", warpmo);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conn.Close(); conn.Dispose(); }

        }
       internal static void RemoveExistingPriorityforWarpMO(String wpMo)
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };

           // string x;
            string removeFromMachine = string.Format("Delete from dbo.t_WarpingPriority Where Warp_MO='{0}'", wpMo);
         
            try
            {


                conn.Open();
                cmd.CommandText = removeFromMachine;
                cmd.ExecuteNonQuery();
                Warp.UpdatePlanLogWarpToProcessed(wpMo);
                MessageBox.Show("Removed");
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close(); conn.Dispose(); //Delay(5000); }
            }
        }
        internal static void RemoveExistingPriorityforWarpMO2(String wpMo)
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };

            // string x;
            string removeFromMachine = string.Format("Delete from dbo.t_WarpingPriority Where Warp_MO='{0}'", wpMo);

            try
            {


                conn.Open();
                cmd.CommandText = removeFromMachine;
                cmd.ExecuteNonQuery();
               // Warp.UpdatePlanLogWarpToProcessed(wpMo);
                MessageBox.Show("Removed");
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close(); conn.Dispose(); //Delay(5000); }
            }
        }
        private static void RemoveExistingPriorityforWarper(int wpmachineid)
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };

            string x;
            string removeFromMachine = string.Format("Delete from dbo.t_WarpingPriority Where WarperID={0}", wpmachineid);

            try
            {


                conn.Open();
                cmd.CommandText = removeFromMachine;
                cmd.ExecuteNonQuery();


            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close(); conn.Dispose(); //Delay(5000); }
            }
            //private static bool Delay(int millisecond)
            //{

            //    Stopwatch sw = new Stopwatch();
            //    sw.Start();
            //    bool flag = false;
            //    while (!flag)
            //    {
            //        if (sw.ElapsedMilliseconds > millisecond)
            //        {
            //            flag = true;
            //        }
            //    }
            //    sw.Stop();
            //    return true;

            //}
        }

    }
}
    //    SELECT p.Warp_MO, p.Expr1, Sum(p.Tickets) TotalRolls, Min(DueDate) FROM manufacturing.`plan log` p
    //Group by p.Warp_MO, p.Expr1
    //Order by Warp_MO;


    //INSERT INTO[dbo].[t_WarpingPriority]
    //([Priority]
    //      ,[Warp_MO]
    //      ,[WarpStyle]
    //      ,[RollsOnWarp]
    //      ,[DueDate]
    //      ,[Notes]
    //      ,[WarperID])
    //VALUES
    //      (<Priority, int,>
    //       ,<Warp_MO, nvarchar(15),>
    //       ,<WarpStyle, nvarchar(15),>
    //       ,<RollsOnWarp, int,>
    //       ,<DueDate, datetime,>
    //       ,<Notes, nvarchar(255),>
    //       ,<WarperID, int,>)

