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
        public MainWindow()
        {
            InitializeComponent();
             WarpBill.FetchWarpBill();
            Warp.FetchNewWarps();
            Warper.FetchWarpers();
            WarpCustomers.FetchWarpCustomers();
            listwarps.ItemsSource = Warp.Warps.OrderBy(w=> w.EarliestDueDate);
            cb_Warper.ItemsSource = Warper.Warpers;

              // listwarps.Items.IndexOf()
        }
       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           // WarpBill.FetchWarpBill();
        //    Warp.FetchNewWarps();
          //  System.Windows.Data.CollectionViewSource warpViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("warpViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // warpViewSource.Source = [generic data source]
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
          //save to db where Priority is not null and for the warper id
        }
    }
}
