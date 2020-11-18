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
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dynamic listget = listwarps.SelectedItem as dynamic;
            MessageBox.Show("Priority: " + listget.Priority + " WarpMO: " + listget.WarpMO + " WarpStyle:" + listget.WarpStyle +  " Tickets: " + listget.TotalTickets + " YarnColorsOfWarp:"+ listget.YarnColorsOfWarp +  " Notes:" + listget.Notes + " Row =" +  listwarps.SelectedIndex.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            // Invoke the standard Print Preview window modally.
            ReportWarpS report = new ReportWarpS();
            XlsxExportOptions xlsxOptions = report.ExportOptions.Xlsx;
            report.RollPaper = true;
            xlsxOptions.ShowGridLines = true;
            xlsxOptions.TextExportMode = TextExportMode.Value;
            xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;
            PrintHelper.ShowPrintPreviewDialog(this, report);
            this.Cursor = Cursors.Arrow;
        }
    }
}
