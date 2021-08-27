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
    public partial class Report1 : Form
    {
        DatabaseConnection con = new DatabaseConnection();
        public Report1()
        {
            InitializeComponent();
        }

        private void Report1_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
        public void loadreport1(string combobox)
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT *FROM `report` WHERE `supplier_name`='" + combobox + "'OR `company`='" + combobox + "'OR `invoice_id`='" + combobox + "'",con.connectDB);
            DataSet1 ds = new DataSet1();
            da.Fill(ds, "Report1");

            ReportDataSource data = new ReportDataSource("DataSet1", ds.Tables[1]);

            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(data);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            //this.reportViewer1.ZoomPercent = 100;

            con.connectDB.Close();

           
        }
        public void loardreport2(string sample)
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT i.invoID,s.supplier_name,s.company,i.Invoicr_total,i.Paid,i.Due,i.order_status,i.Status,i.update_by,i.payment,i.rcpID,i.date FROM all_updates i INNER JOIN suppliers s ON i.supID = s.sup_id WHERE  `supplier_name`='" + sample + "'OR `company`='" + sample + "'OR `invoID`='" + sample + "'", con.connectDB);

            DataSet1 ds = new DataSet1();
            da.Fill(ds, "Report2");

            ReportDataSource data1 = new ReportDataSource("DataSet2", ds.Tables[2]);

            //this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(data1);
            //this.reportViewer1.RefreshReport();
            //this.reportViewer1.ZoomMode = ZoomMode.Percent;
            //this.reportViewer1.ZoomPercent = 100;

            con.connectDB.Close();
        }

    }
}
