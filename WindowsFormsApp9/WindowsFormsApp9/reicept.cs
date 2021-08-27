using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;

namespace WindowsFormsApp9
{
    public partial class reicept : Form
    {
        DatabaseConnection con = new DatabaseConnection();
        public reicept()
        {
            InitializeComponent();
        }

        private void reicept_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
        public void loadreport(string trno)
        {

            MySqlDataAdapter da = new MySqlDataAdapter("SELECT `invoice_id`, `inovoice_total`, `paid_amount`, `due_amount`, `payment`, `status`, `transaction_no` FROM `trc` WHERE `transaction_no`='"+ trno+"'", con.connectDB);
            DataSet1 ds = new DataSet1();
            da.Fill(ds, "reciept");


            ReportParameter billno = new ReportParameter("billno", trno);
            reportViewer1.LocalReport.SetParameters(billno);
            ReportDataSource data = new ReportDataSource("DataSet1", ds.Tables[0]);
            
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(data);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            //this.reportViewer1.ZoomPercent = 95;

            con.connectDB.Close();

        }
    }
}
