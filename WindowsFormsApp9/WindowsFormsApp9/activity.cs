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
    public partial class activity : UserControl
    {
     
        DatabaseConnection con = new DatabaseConnection();
        private string username; 
        public activity(string name)
        {
            InitializeComponent();
            username = name;
        }

        private void activity_Load(object sender, EventArgs e)
        {
            
            load_data();

        }
        //SELECT `invoice_id`, `inovoice_total`, `paid_amount`, `due_amount`,`status`, `supplier_name`, `company` FROM `report
        void load_data()
        {
            try
            {
                dataGridView1.Rows.Clear();
                string query = "SELECT i.invoice_id,i.inovoice_total,i.paid_amount,i.due_amount,i.status,s.supplier_name,s.company,s.sup_id FROM invoices i INNER JOIN suppliers s ON i.sup_id=s.sup_id";
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
                        dataGridView1.Rows.Add(dr["invoice_id"].ToString(), dr["supplier_name"].ToString(), dr["company"].ToString(), dr["inovoice_total"].ToString(), dr["paid_amount"].ToString(), dr["due_amount"].ToString(), dr["status"].ToString(), dr["sup_id"].ToString());
                    }

                }
                else
                {
                    dataGridView1.DataSource = null;
                }
                dr.Close();
                con.connectDB.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private int supid;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {      
                txtinvoice.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtsup.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txttotal.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtpaid.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                supid = int.Parse(dataGridView1.SelectedRows[0].Cells[7].Value.ToString());
            }
            catch
            {

            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if(txttotal.Text=="")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txttotal, "empty field");
            }
            else if(txtpaid.Text=="")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtpaid, "empty field");
            }
            else
            {
                errorProvider1.Clear();
                DialogResult dialogResult = MessageBox.Show("would you like update this? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    update();
                }
                
            }
        }
        string status;
        private decimal due = 0;
        void update()
        {
            try
            {
                
                decimal total = decimal.Parse(txttotal.Text);
                decimal paid = decimal.Parse(txtpaid.Text);
                
                due = total - paid;
                if (due == 0)
                {
                    status = "P";

                }
                else
                {
                    status = "N/P";
                }

                string query = "UPDATE `invoices` SET `inovoice_total`='"+total+"',`paid_amount`='"+paid+"',`due_amount`='"+due+"',`status`='"+status+ "',`update_by`='"+ username+ "',`update_amount`='"+ total+"' WHERE `invoice_id`='" + txtinvoice.Text+"'";
                MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);
                MySqlDataReader dr;
                con.connectDB.Open();
                dr = MyCommand2.ExecuteReader();
                MessageBox.Show("Selected Invoice Updated");

                con.connectDB.Close();
                load_data();
                allupdates();
                textbox_clear();
                         

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        void textbox_clear()
        {
            txtinvoice.Clear();
            txttotal.Clear();
            txtsup.Clear();
            txtpaid.Clear();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(txtinvoice.Text=="")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtinvoice, "empty field");
            }
            else
            {
                errorProvider1.Clear();
                DialogResult dialogResult = MessageBox.Show("Would You like Deelete this ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        string query = "DELETE FROM `invoices` WHERE `invoice_id`='" + txtinvoice.Text + "'";
                        MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);

                        con.connectDB.Open();
                        MyCommand2.ExecuteNonQuery();
                        con.connectDB.Close();
                        
                       

                        string query2 = "DELETE FROM `scan_images` WHERE `invoice_id`='" + txtinvoice.Text + "'";
                        MySqlCommand MyCommand1 = new MySqlCommand(query2, con.connectDB);
                        con.connectDB.Open();
                        MyCommand1.ExecuteNonQuery();
                        con.connectDB.Close();
                        MessageBox.Show("Invoice Deleted");
                        load_data();
                        textbox_clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void txtpaid_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txttotal_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            image_view ob = new image_view(txtinvoice.Text);
            ob.Show();
        }

        private void allupdates()
        {
            string status1 = "تحديث";

            string query = "INSERT INTO `all_updates`(`invoID`, `supID`, `Invoicr_total`, `Paid`, `Due`, `Status`, `update_by`,`order_status`) VALUES('" + txtinvoice.Text + "','" + supid + "','" + txttotal.Text + "','" + txtpaid.Text + "','" + due + "','" + status1 + "','" + username + "','"+status+"')";

            MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
            con.connectDB.Open();
            cmd.ExecuteNonQuery();
            con.connectDB.Close();
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
