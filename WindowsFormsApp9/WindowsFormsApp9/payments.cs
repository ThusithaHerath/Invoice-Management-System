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
using System.IO;

namespace WindowsFormsApp9
{
    public partial class payments : UserControl
    {
        DatabaseConnection con = new DatabaseConnection();
        string invoice_id ;
        string invo;
        int invoice_no = 0;
        private string username;
        public payments(string user)
        {
            username = user;
            InitializeComponent();
        }

        private void payments_Load(object sender, EventArgs e)
        {
            this.combobox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            orderNo();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox2.SelectedIndex == 0)
            {
                suplier_name();
            }
            else if (ComboBox2.SelectedIndex == 1)
            {
                suplier_company();
            }
            else
            {
                inovoice_id();
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
        /*
         * SELECT i.invoice_id,i.inovoice_total,i.paid_amount,i.due_amount,i.created_at,i.status,s.supplier_name,s.company
        FROM invoices i
        INNER JOIN suppliers s
        ON i.sup_id=s.sup_id
         * 
         * 
         * 
         * 
         * */
        private void btnserach_Click(object sender, EventArgs e)
        {
            if (combobox1.Text == "")
            {
                MessageBox.Show("Supplier Details Required..", "IMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                search_invoice();
               
            }
        }

        void search_invoice()
        {
            try
            {
                dataGridView1.Rows.Clear();
                string query = "SELECT i.id,i.invoice_id,i.inovoice_total,i.paid_amount,i.due_amount,i.created_at,i.status,s.supplier_name,s.company,s.sup_id FROM invoices i INNER JOIN suppliers s ON i.sup_id=s.sup_id WHERE `supplier_name`='" + combobox1.Text + "'OR `company`='" + combobox1.Text + "'OR `invoice_id`='" + combobox1.Text + "'";
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
                        dataGridView1.Rows.Add(dr["invoice_id"].ToString(), dr["inovoice_total"].ToString(), dr["paid_amount"].ToString(), dr["due_amount"].ToString(), dr["created_at"].ToString(), dr["status"].ToString(), dr["supplier_name"].ToString(), dr["company"].ToString(), dr["id"].ToString(), dr["sup_id"].ToString());
                    }

                }
                else
                {

                    MessageBox.Show("Invoice Details Not Found");  //valid supplier name    
                    dataGridView1.DataSource = null;
                }

                con.connectDB.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private int supid;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
               
                invoice_id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtinvoice.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtpaid.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                txtdue.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                invoice_no = int.Parse(dataGridView1.SelectedRows[0].Cells[8].Value.ToString());
                invo = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                supid = int.Parse(dataGridView1.SelectedRows[0].Cells[9].Value.ToString());
            }
            catch
            {

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (invoice_id == "")
            {
                MessageBox.Show("Please select invoice", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtpayment.Text == "")
            {
                MessageBox.Show("please Enter payment", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Would Like to pay for selected Invoice", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    pay();
                    insert_transactiondata();
                    search_invoice();
                }
                else
                {
                    MessageBox.Show("Payment canceled");
                }
            }
        }
        //*******************************update payment*******************************************************
        string status;
        decimal newpaid_amount = 0;
        decimal newdue_amount = 0;
        void pay()
        {
            try
            {
               
                decimal invoicetot = decimal.Parse(txtinvoice.Text);
                decimal paid = decimal.Parse(txtpaid.Text);
                decimal due = decimal.Parse(txtdue.Text);
                decimal payment = decimal.Parse(txtpayment.Text);
                newdue_amount = due - payment;
                newpaid_amount = paid + payment;

                if (newdue_amount == 0)
                {
                    status = "P";
                }
                else
                {
                    status = "N/P";
                }

                new_trc();

                string query = "UPDATE `invoices` SET `paid_amount`='" + newpaid_amount + "',`due_amount`='" + newdue_amount + "',`status`='" + status + "'  WHERE `invoice_id`='" + invoice_id + "'";
                MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
                con.connectDB.Open();
                cmd.ExecuteNonQuery();
                con.connectDB.Close();
                MessageBox.Show("Payment successful..");
                allupdates();
                clear_textbox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //*************************************************clear***************************************************
        void clear_textbox()
        {
            invoice_id = "";
            txtinvoice.Clear();
            txtpaid.Clear();
            txtdue.Clear();
           
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            
            reicept ob = new reicept();
            ob.loadreport(label1.Text);
            ob.ShowDialog();

        }
        //************************************order no************************************************************************
        void orderNo()
        {
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter("select transaction_no from transaction_genarate order by transaction_no desc", con.connectDB);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    label1.Text = (int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();

                }
                else
                {
                    label1.Text = "1";
                }
                con.connectDB.Close();
            }

            catch
            {

                MessageBox.Show("Please check your Database server Connection..", "Configur Error Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void new_trc()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `transaction_genarate`(`transaction_no`) VALUES ('" + label1.Text + "')", con.connectDB);
            con.connectDB.Open();
            cmd.ExecuteNonQuery();
            con.connectDB.Close();

            orderNo();
        }

        void insert_transactiondata()
        {
            
            string query = "INSERT INTO `transaction`(`transaction_no`, `invoice_no`,`payment`,`user_by`) VALUES('" + label1.Text+"','"+ invoice_no +"','"+ txtpayment.Text + "','"+username+"')";

            MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
            con.connectDB.Open();
            cmd.ExecuteNonQuery();
            con.connectDB.Close();
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }

        private void txtpayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar)
&& !char.IsDigit(e.KeyChar)
&& e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if (e.KeyChar == '.'
                    && (sender as TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
            catch
            {

            }
        }

        private void allupdates()
        {
            string status1 = "Invoice Payment";

            string query = "INSERT INTO `all_updates`(`invoID`, `supID`, `Invoicr_total`, `Paid`, `Due`, `Status`, `update_by`,`order_status`,`payment`,`rcpID`) VALUES('" + invoice_id + "','" + supid + "','" + txtinvoice.Text + "','" + newpaid_amount + "','" + newdue_amount + "','" + status1 + "','" + username + "','" + status + "','"+txtpayment.Text+"','"+label1.Text+"')";

            MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
            con.connectDB.Open();
            cmd.ExecuteNonQuery();
            con.connectDB.Close();
        }
    }
}
