using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp9
{
    public partial class reports : UserControl
    {
        DatabaseConnection con = new DatabaseConnection();
        public reports()
        {
            InitializeComponent();
        }

        private void btnserach_Click(object sender, EventArgs e)
        {
            if (combobox1.Text == "")
            {
                MessageBox.Show("Please select invoice/company or supplier name.", "Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                search_report();
                serach_trc();


            }
        }
        void suplier_name()
        {
            MySqlDataAdapter adp = new MySqlDataAdapter("SELECT `supplier_name` FROM `suppliers`  ", con.connectDB);
            DataSet dt = new DataSet();
            adp.Fill(dt);
            combobox1.DataSource = dt.Tables[0];
            combobox1.DisplayMember = dt.Tables[0].Columns[0].ToString();

            con.connectDB.Close();
        }

        void suplier_company()
        {
            MySqlDataAdapter adp = new MySqlDataAdapter("SELECT  `company` FROM `suppliers` ", con.connectDB);
            DataSet dt = new DataSet();
            adp.Fill(dt);
            combobox1.DataSource = dt.Tables[0];
            combobox1.DisplayMember = dt.Tables[0].Columns[0].ToString();

            con.connectDB.Close();
        }

        void inovoice_id()
        {
            MySqlDataAdapter adp = new MySqlDataAdapter("SELECT `invoice_id` FROM `invoices` ", con.connectDB);
            DataSet dt = new DataSet();
            adp.Fill(dt);
            combobox1.DataSource = dt.Tables[0];
            combobox1.DisplayMember = dt.Tables[0].Columns[0].ToString();

            con.connectDB.Close();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void search_report()
        {
            try
            {
                dataGridView1.Rows.Clear();
                
                string query = "SELECT `id`,`invoice_id`, `inovoice_total`, `paid_amount`, `due_amount`, `created_at`, `status`, `supplier_name`, `company`,`insert_by` FROM `report` WHERE `supplier_name`='" + combobox1.Text + "'OR `company`='" + combobox1.Text + "'OR `invoice_id`='" + combobox1.Text + "'";
                int k = 0;
                MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);
                MySqlDataReader dr;
                con.connectDB.Open();
                dr = MyCommand2.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        k++;
                        dataGridView1.Rows.Add(k,dr["invoice_id"].ToString(), dr["supplier_name"].ToString(), dr["company"].ToString(), dr["inovoice_total"].ToString(), dr["paid_amount"].ToString(), dr["due_amount"].ToString(), dr["status"].ToString(), dr["created_at"].ToString(), dr["insert_by"].ToString());
                    }

                }
                else
                {

                    //valid supplier name    
                    dataGridView1.DataSource = null;
                }
                dr.Close();
                con.connectDB.Close();

                     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        void serach_trc()
        {
           
            try
            {
                dataGridView2.Rows.Clear();
                string query1 = "SELECT i.invoID,s.supplier_name,s.company,i.Invoicr_total,i.Paid,i.Due,i.order_status,i.Status,i.update_by,i.payment,i.date,i.rcpID FROM all_updates i INNER JOIN suppliers s ON i.supID = s.sup_id WHERE  `supplier_name`='" + combobox1.Text + "'OR `company`='" + combobox1.Text + "'OR `invoID`='" + combobox1.Text + "'";
                int l = 0;
                MySqlCommand MyCommand2 = new MySqlCommand(query1, con.connectDB);
                MySqlDataReader dr;
                con.connectDB.Open();
                dr = MyCommand2.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        l++;
                        dataGridView2.Rows.Add(dr["invoID"].ToString(), dr["supplier_name"].ToString(), dr["company"].ToString(), dr["Invoicr_total"].ToString(), dr["Paid"].ToString(), dr["Due"].ToString(), dr["order_status"].ToString(), dr["payment"].ToString(), dr["update_by"].ToString(), dr["Status"].ToString(), dr["date"].ToString(), dr["rcpID"].ToString());
                    }

                }
                else
                {

                   
                    dataGridView2.DataSource = null;
                }
                dr.Close();
                con.connectDB.Close();

            }
            catch
            {

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Report1 ob = new Report1();
            ob.loadreport1(combobox1.Text);
            ob.loardreport2(combobox1.Text);
            ob.ShowDialog();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (guna2ComboBox1.SelectedIndex == 0)
            {
                
                suplier_name();
            }
            else if (guna2ComboBox1.SelectedIndex == 1)
            {
               
                suplier_company();
            }
            else
            {
                inovoice_id();
            }
        }
    }
}
