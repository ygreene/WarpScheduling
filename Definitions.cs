using System.IO;

namespace WarpScheduling
{
    class Definitions
    {
        /// <summary>
        /// MYSQL SERVER
        /// </summary>
        public static string baseM = "MYSQLCONN";
        // public static string baseM = "MYSQLTEST";

        /// <summary>
        /// KM SQL SERVER
        /// </summary>
        public static string baseS = "SQLCONNFSKM";
        public static string baseA = "ACCESSCONN";

        /// <summary>
        /// SQL QUERY/COMMAND STRING
        /// </summary>
        public static string sql_user = "";
        public static string photodb = "photos";
        public static string loginLogsdb = "loginlogs";
        /// <summary>
        /// SQL FSDBMR
        /// </summary>
        public static string FSDBMRdb = "FSDBMR";
        /// <summary>
        /// SQL- STI DB
        /// </summary>
        public static string STIdb = "STI";
        public static string Manudb = "manufacturing";
        public static string STIdatadb = "O:\\Data\\StiData.mdb";
        public static string userlog = "";
        public static int useridlog = 0;
        public static string path = Directory.GetCurrentDirectory();
        public static string foldericons = "\\icons";
        public static string routeimagedefault = "\\001-worker-1.png";
        public static string SystemName = "KMLIVE";
        public static string ServerName = "192.168.100.20";
        public static string Port = "7361";
        public static bool UnifiedLogon = false;
        public static bool Impersonation = false;
        public static string UserIdF = "XXX";
        public static string PasswordF = "NIGHT";
        public static string FileLogWriter = "C:\\STIRoot\\HPTranLog.txt";
        public static string FileLocalPathEdi = "C:\\STIRoot\\";
        public static string FileRemotePathEdiMC = "STI0002P/";
        public static string ServerNameEdiFileMC = "mft.svcs.hpe.com";
        public static string UserNameEdiFileMC = "STIMMPROD";
        public static string PWDEdiFileMC = "STIProd1!";
        public static string ACCEdiFileMC = "STIProd1!";
       
    }
}
