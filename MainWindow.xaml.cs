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
            Warp.FetchPriortizedWarps();  // needs to run this before fetching new warps.  
            Warp.FetchNewWarps();
           Warper.FetchWarpers();
            WarpCustomers.FetchWarpCustomers();
           // listwarps.ItemsSource = Warp.Warps.OrderBy(w=> w.WarpStyle).ThenBy(w=> w.EarliestDueDate).ThenBy(w=> w.WarpMO);
            cb_Warper.ItemsSource = Warper.Warpers;
            cb_Customer.ItemsSource = Customer.FetchCustomers();

              // listwarps.Items.IndexOf()
        }
       
 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = this;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Warper.Text.Length==0)
            {
                MessageBox.Show("Please Choose a Warper !");
                return; }
           WarpInventory. FetchCurrentWarpInv();
            IEnumerable<Warp> WarpPriority;
            //foreach ( )
            WarpPriority=Warp.Warps.Where(c => c.Priority > 0 && c.WarperID==Warper.FetchWarperIDFromWarpName(cb_Warper.Text )); //fetch warperid
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
            if (e.AddedItems.Count ==0)
            {
                MessageBox.Show("Please choose an item that runs on this warper!");
                return;
            }

            WarpDetail.ItemsSource = WarpCustomers.WrpCustomers.Where(c => c.WarpMO == listget[0].WarpMO).OrderBy(c => c.Priority).ToList();
          //  MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            //TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
            //MoveFocus(request);
        }

        private void cb_Warper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x = int.Parse(cb_Warper.SelectedValue.ToString());
           
           List <string> m= Warper.Warpers.Where(j => j.WarperID == x).ElementAt(0).WarpStylesIRun;
           // var g = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle)).OrderBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            //listwarps.ItemsSource = Warp.Warps.Where (w=> m.Any(r=> r==w.WarpStyle)).OrderBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
          if (x<5)
            {
            if (cb_SortChad.IsChecked==true)
            {
                    //  List<Warp> lwarpsource= Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && !(w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();
                    List<Warp> lwarpsource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && !(w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenByDescending(w => w.IsRush).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();
                    listwarps.ItemsSource = lwarpsource;// Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority*-1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                listwarps.IsSynchronizedWithCurrentItem = true;
            }
            else
            {

                    //IEnumerable<Warp> lwarpsource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle.Trim()) && (w.WarperID == x || w.WarperID == null) && !(w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenByDescending(w => w.IsRush).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    //lwarpsource.Where(c => c.WarpMO == "J00099");
                    // listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID ==null) && !(w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority*-1 ).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle.Trim()) && (w.WarperID == x || w.WarperID == null) && !(w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenByDescending(w=> w.IsRush).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.IsSynchronizedWithCurrentItem = true;
            }
            }
          else
            {
                if (cb_SortChad.IsChecked == true)
                {
                    List<Warp> lwarpsource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && (w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenByDescending(w=> w.IsRush).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();

                    listwarps.ItemsSource = lwarpsource;// Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority*-1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.IsSynchronizedWithCurrentItem = true;
                }
                else
                {
                    listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && (w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenByDescending(w => w.IsRush).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.IsSynchronizedWithCurrentItem = true;
                }
            }

        }

        private void btn_UpdateNotes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            List<string> FirstOnly = new List<string>{ "WS8133", "WS8134", "WS8135", "WS8136", "WS8137", "WS8138", "WS8139", "WS8140", "WS8141", "WS8142", "WS8143", "WS8144", "WS8145", "WS8146", "WS8147", "WS8148", "WS8149", "WS8150", "WS8151", "WS8152", "WS8153", "WS8154", "WS8155", "WS8156", "WS8157", "WS8169", "WS8170" };
           
            List<string> NOMIX  = new List<string>{ "WS9031", "WS9040", "WS9041", "WS9042", "WS9043", "WS9044", "WS9045", "WS9046", "WS9047", "WS9048", "WS9049" };
            List<Warp> WarpPriority;
            WarpPriority = Warp.Warps.Where(c => c.Priority > 0).ToList();
         
            foreach (var i in WarpPriority.Where(r => NOMIX.Any(x => r.WarpStyle.Contains(x))))
            { i.Notes += " DO NOT MIX LOTS"; }
            foreach (var i in WarpPriority.Where(g => FirstOnly.Any(x => g.WarpStyle.Contains(x))))
            { i.Notes += " RUN ON 1ST-PHILIS TO APPROVE"; }

            ExcelReport.CreateSpreadSheet(WarpPriority,"Wpriority.xlsx");
            MessageBox.Show("Priority Exported!");
        }

        private void btn_ReSort_Click(object sender, RoutedEventArgs e)
        {
            int x = int.Parse(cb_Warper.SelectedValue.ToString());

            List<string> m = Warper.Warpers.Where(j => j.WarperID == x).ElementAt(0).WarpStylesIRun;
            // var g = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle)).OrderBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            //listwarps.ItemsSource = Warp.Warps.Where (w=> m.Any(r=> r==w.WarpStyle)).OrderBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            if (x < 5)
            {
                if (cb_SortChad.IsChecked == true)
                {
                    List<Warp> lwarpsource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && !(w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();

                    listwarps.ItemsSource = lwarpsource;// Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority*-1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.IsSynchronizedWithCurrentItem = true;
                }
                else
                {
                    listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && !(w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.IsSynchronizedWithCurrentItem = true;
                }
            }
            else
            {
                if (cb_SortChad.IsChecked == true)
                {
                    List<Warp> lwarpsource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && (w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();

                    listwarps.ItemsSource = lwarpsource;// Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority*-1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.IsSynchronizedWithCurrentItem = true;
                }
                else
                {
                    listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null) && (w.WarpMO.StartsWith("P"))).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
                    listwarps.IsSynchronizedWithCurrentItem = true;
                }
            }
            //if (cb_SortChad.IsChecked == true)
            //{
            //    List<Warp> lwarpsource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();

            //    listwarps.ItemsSource = lwarpsource;// Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority*-1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            //    listwarps.IsSynchronizedWithCurrentItem = true;
            //}
            //else
            //{
            //    listwarps.ItemsSource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.WarpStyle).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            //    listwarps.IsSynchronizedWithCurrentItem = true;
            //}
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<string> wp;
            if(cb_Warper.Text.Length==0)
            { MessageBox.Show("Please choose warper !");
                return;
            }
            int x = int.Parse(cb_Warper.SelectedValue.ToString());
            List<string> m = Warper.Warpers.Where(j => j.WarperID == x).ElementAt(0).WarpStylesIRun;

            List<Warp> lwarpsource = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority * -1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();
           if(tb_WarpFilter.Text.Length >0)
            {
                wp = WarpCustomers.WrpCustomers.Where(c => c.WarpMO == tb_WarpFilter .Text).Select(j => j.WarpMO).ToList<string>();
            }
           else if (tb_Item.Text.StartsWith("WS"))
            {
                wp =lwarpsource.Where(w=> w.WarpStyle ==tb_Item.Text ).Select(j => j.WarpMO).ToList<string>();
            }
            else if (tb_Item.Text.Length > 0 && cb_Customer.Text.Length > 0)
            {
                wp = WarpCustomers.WrpCustomers.Where(c => c.Customer == cb_Customer.Text && c.ItemNumber == tb_Item.Text).Select(j => j.WarpMO).ToList<string>();
            }
            else if(cb_Customer .Text.Length>0)
            {
                wp = WarpCustomers.WrpCustomers.Where(c => c.Customer == cb_Customer.Text).Select(j => j.WarpMO).ToList<string>();
            }
                      else if (tb_Item.Text.Length > 0)
            {
                wp = WarpCustomers.WrpCustomers.Where(c => c.ItemNumber == tb_Item.Text).Select(j => j.WarpMO).ToList<string>();
            }
            else
            {
                wp = WarpCustomers.WrpCustomers.Where(c => c.ItemNumber.Contains("-F")).Select(j => j.WarpMO).ToList<string>(); ;
            }
        
            listwarps.ItemsSource = lwarpsource.Where (r=> wp.Any(z=> z==r.WarpMO ));// Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority*-1).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO);
            listwarps.IsSynchronizedWithCurrentItem = true;
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            cb_Customer.Text="";
            tb_Item.Text = "";
            tb_WarpFilter.Text = "";
            cb_Warper_SelectionChanged(cb_Warper,null);
        }

        private void priorityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb!=null)
            {
                e.Handled = true;
                tb.Focus();
                tb.SelectAll();

            }
        }

        private void listwarps_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Style_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void btn_RemoveMO_Click(object sender, RoutedEventArgs e)
        {
            Warp.RemoveExistingPriorityforWarpMO(tb_WarpFilter.Text);
        }

        private void btn_ResetMO_Click(object sender, RoutedEventArgs e)
        {
            Warp.UpdatePlanLogWarpToUnProcessed(tb_WarpFilter.Text);
            Warp.RemoveExistingPriorityforWarpMO2(tb_WarpFilter .Text);
            Warp.Warps.RemoveAll(w => w.WarpMO == tb_WarpFilter.Text);        }

        private void btn_ExtAll_Click(object sender, RoutedEventArgs e)
        {

            int x = int.Parse(cb_Warper.SelectedValue.ToString());

            List<string> m = Warper.Warpers.Where(j => j.WarperID == x).ElementAt(0).WarpStylesIRun;


            List<Warp> WarpPriority = Warp.Warps.Where(w => m.Any(r => r == w.WarpStyle) && (w.WarperID == x || w.WarperID == null)).OrderByDescending(w => w.Priority * -1).ThenByDescending(w => w.IsRush).ThenBy(w => w.EarliestDueDate).ThenBy(w => w.WarpMO).ToList<Warp>();
            //List<Warp> WarpPriority;
            //WarpPriority = Warp.Warps.ToList();
            ExcelReport.CreateSpreadSheetAll(WarpPriority, "WpriorityAll.xlsx", (WarpScheduling.ExcelReport.Wpr)Warper.FetchWarperIDFromWarpName(cb_Warper .Text));
            MessageBox.Show("All Exported!");
        }
    }
        

    }
