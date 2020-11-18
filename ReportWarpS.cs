using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraPrinting;
using System.Collections.Generic;

namespace WarpScheduling
{
    public partial class ReportWarpS : DevExpress.XtraReports.UI.XtraReport
    {
      //  public static string value1 = "0";
        private DataTable ds;
        public List<string> GroupValues = new List<string>();
        string GroupFieldName = string.Empty;
        public ReportWarpS()
        {
            InitializeComponent();
        }

        private void ReportWarpS_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
           
            ds = Warp.FetchNewWarpsDT();
           
            this.DataSource = ds;
            this.DataMember = ds.TableName;
            GroupField groupField1  = new GroupField("JacorBase");
            GroupHeader1.GroupFields.Add(groupField1);
            xrLabel_group.DataBindings.Add("Text", Report.DataSource, "JacorBase");
            xrLabel_priority.DataBindings.Add("Text", Report.DataSource, "Priority");
            xrLabel_warp.DataBindings.Add("Text", Report.DataSource, "Warp_MO");
            xrLabel_ws.DataBindings.Add("Text", Report.DataSource, "Expr1");
            xrLabel4_nPCS.DataBindings.Add("Text", Report.DataSource, "TotalRolls");
            xrLabel_lmn.DataBindings.Add("Text", Report.DataSource, "YarnColorsOfWarp");
            xrLabel_duedate.DataBindings.Add("Text", Report.DataSource, "duedate", "{0:dd MMM yyyy}");
            PrintingSystem.XlSheetCreated += PrintingSystem_XlsxDocumentCreated;


        }

        private void PrintingSystem_XlsxDocumentCreated(object sender, XlSheetCreatedEventArgs e)
        {
          
                e.SheetName = GroupValues[e.Index].ToString();
               
           
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
          
            GroupFieldName = (sender as GroupHeaderBand).GroupFields[0].FieldName;
          //  Console.WriteLine((sender as GroupHeaderBand).Report.GetCurrentColumnValue(GroupFieldName));
            GroupValues.Add(Convert.ToString((sender as GroupHeaderBand).Report.GetCurrentColumnValue(GroupFieldName)));
        }
    }
}
