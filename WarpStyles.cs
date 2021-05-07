using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace WarpScheduling
{
    class WarpStyles
    {
        public static List<WarpStyles> WarperIDWithStyles = new List<WarpStyles>();
        public int WarperID { get; set; }
        public string WarpStyle { get; set; }

        public static void fetchWarperStyles()
        {
            var filename = "WarperStyles.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var WarpStylePath = Path.Combine(currentDirectory, filename);

            XDocument xDoc = XDocument.Load(WarpStylePath);
            IEnumerable<XElement> wstyles = xDoc.Descendants("Warper");

            foreach (var i in wstyles)
            {

               // Console.WriteLine(WarperID=i.Value);
               WarperIDWithStyles.Add(new WarpStyles() { WarperID =int.Parse(i.Element("Id").Value) , WarpStyle = i.Element("WarpStyle").Value});
            }
            
      
        }
        public static List<string> FetchStylesByWarpID(int machineID)
        {
            List<string> styles = new List<string>();
            var x = WarperIDWithStyles.Where(c => c.WarperID == machineID);
            foreach (var i in x)
            {
                styles.Add(i.WarpStyle);
            }
            return styles;
        }
    }
}
//List<Loom> looms = new List<Loom>();
//var doc = XDocument.Load(@"C:\STIRoot\Loom.xml");
//IEnumerable<XElement> rows = doc.Descendants().Where(d => d.Name == "LoomNumber");
//        foreach (var i in rows)
//        {
//            looms.Add(new Loom { LoomNumber = int.Parse(i.Value)  });
//        }
//            return looms;
//        }
       
//        public int LoomNumber { get; set; }
