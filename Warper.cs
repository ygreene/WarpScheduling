using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WarpScheduling
{
    class Warper
    {
        public static List<Warper> Warpers = new List<Warper>();
        public int WarperID { get; set; }
        public string WarperName { get; set; }
        public string  Model { get; set; }
       public  List <string> WarpStylesIRun { get; set; }


        public static int FetchWarperIDFromWarpName(string WarpName)
        {
           if (WarpName.Length>0 )
            {
                return Warpers.Where(c => c.WarperName == WarpName).ElementAt(0).WarperID;
            }
           else
            { return 0; }
        }
        public static void FetchWarpers()
        {
            SqlConnection conn = new SqlConnection { ConnectionString = Properties.Settings.Default.sti };
            SqlCommand cmd = new SqlCommand { Connection = conn, CommandType = System.Data.CommandType.Text };
            SqlDataReader reader;
            try
            {
                cmd.CommandText = "Select WarperID, Warper, Model From dbo.t_Warper";


                conn.Open();
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    Warpers.Add(new Warper() { WarperID = reader.GetInt32(0), WarperName = reader.GetString(1), Model = reader.GetString(2) , WarpStylesIRun=WarpStyles.FetchStylesByWarpID(reader.GetInt32(0))}); 

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conn.Close();conn.Dispose(); }



        }

    }
}
