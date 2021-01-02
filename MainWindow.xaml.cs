using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;


namespace WarpScheduling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow main;
        public MainWindow()
        {
            InitializeComponent();
            MORush.RemoveStatus5MOs();
            MORush.FetchMORushList();
            WarpStyles.fetchWarperStyles();
            WarpBill.FetchWarpBill();
            Warp.FetchNewWarps();
            Warp.FetchPriortizedWarps();
            Warper.FetchWarpers();
            WarpCustomers.FetchWarpCustomers();
           // listwarps.ItemsSource = Warp.Warps.OrderBy(w=> w.WarpStyle).ThenBy(w=> w.EarliestDueDate).ThenBy(w=> w.WarpMO);
            cb_Warper.ItemsSource = Warper.Warpers;

              // listwarps.Items.IndexOf()
        }
       
 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = this;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Warp> WarpPriority;
            //foreach ( )
            WarpPriority=Warp.Warps.Where(c => c.Priority > 0);
            dynamic wpget = cb_Warper.SelectedItem as dynamic;
            //save to db where Priority is not null and for the warper id
            Warp.SavePriority(WarpPriority, wpget.WarperID);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dynamic listget = listwarps.SelectedItem as dynamic;

            WarpDetail.ItemsSource = WarpCustomers.WrpCustomers.Where(c => c.WarpMO == listget.WarpMO).OrderBy(c => c.Priority);
         //   MessageBox.Show("Priority: " + listget.Priority + " WarpMO: " + listget.WarpMO + " WarpStyle:" + listget.WarpStyle +  " Tickets: " + listget.TotalTickets + " YarnColorsOfWarp:"+ listget.YarnColorsOfWarp +  " Notes:" + listget.Notes + " Row =" +  listwarps.SelectedIndex.ToString());
        }

        private void listwarps_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // MessageBox.Show(e.AddedItems );
            dynamic listget = e.AddedItems as dynamic;
            WarpDetail.ItemsSource = WarpCustomers.WrpCustomers.Where(c => c.WarpMO == listget[0].WarpMO).OrderBy(c => c.Priority).ToList();

        }

        private void cb_Warper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x = int.Parse(cb_Warper.SelectedValue.ToString());
           
           List <string> m= Warper.Warpers.Where(j => j.WarperID == x).ElementAt(0).WarpStylesIRun;
            var g = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle)).OrderBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            //listwarps.ItemsSource = Warp.Warps.Where (w=> m.Any(r=> r==w.WarpStyle)).OrderBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            if (cb_SortChad.IsChecked==true)
            {
                listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderBy(w => w.Priority).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                listwarps.IsSynchronizedWithCurrentItem = true;
            }
            else
            { 
                listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID ==null)).OrderByDescending(w => w.Priority ).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                listwarps.IsSynchronizedWithCurrentItem = true;
            }
        }

        private void btn_UpdateNotes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            List<Warp> WarpPriority;
            WarpPriority = Warp.Warps.Where(c => c.Priority > 0).ToList();
            ExcelReport.CreateSpreadSheet(WarpPriority,"Wpriority.xlsx");
            MessageBox.Show("Priority Exported!");
        }
    }
        

    }
