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
    public partial class AdminController : UserControl
    {
        DatabaseConnection con = new DatabaseConnection();
        private int id = 0;

        public AdminController()
        {
            InitializeComponent();
        }

        private void txtadminname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtadminname, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void txtemail_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtemail, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void txtmobile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtmobile, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void txtpassword_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtpassword, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }
        private void loadata()
        {
            try
            {
                string roll = "admin";
                dataGridView1.Rows.Clear();
                string query = "SELECT `uid`,`user_name`, `user_email`, `mobile_No`, `password` FROM `users` where `user_roll`='" + roll + "'";
                int i = 0;
                MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);
                MySqlDataReader dr;
                con.connectDB.Open();
                dr = MyCommand2.ExecuteReader();

                while (dr.Read())
                {
                    i++;
                    dataGridView1.Rows.Add(dr["uid"].ToString(), dr["user_name"].ToString(), dr["user_email"].ToString(), dr["mobile_No"].ToString(), dr["password"].ToString());
                }
                con.connectDB.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void AdminController_Load(object sender, EventArgs e)
        {
            loadata();
        }
        private string adminname;
        private void btnsave_Click(object sender, EventArgs e)
        {
            if (txtadminname.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtadminname, "empty field");
            }
            else if (txtemail.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtemail, "empty field");
            }
            else if (txtmobile.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtmobile, "empty field");
            }
            else if (txtpassword.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtpassword, "empty field");
            }
            else
            {
                errorProvider1.Clear();
                try
                {
                    string roll = "admin";
                    string query1 = "SELECT `user_name` FROM `users` where `user_roll`= '" + roll + "' and user_name='"+txtadminname.Text+"'";
                    MySqlCommand cmd = new MySqlCommand(query1, con.connectDB);
                    con.connectDB.Open();
                    MySqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        adminname = dr[0].ToString();

                    }
                    dr.Close();
                    con.connectDB.Close();
                    

                    if (adminname == txtadminname.Text)
                    {
                        MessageBox.Show("Existing Admin", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string query = "INSERT INTO `users`( `user_name`, `user_email`, `mobile_No`,`user_roll`,`password`) VALUES('" + txtadminname.Text + "','" + txtemail.Text + "','" + txtmobile.Text + "','" + roll + "','" + txtpassword.Text + "')";
                        MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);
                        con.connectDB.Open();
                        MyCommand2.ExecuteNonQuery();     // Here our query will be executed and data saved into the database.  
                        con.connectDB.Close();


                        clear_box();
                        loadata();

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows != null)
                {
                    id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    txtadminname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                    txtemail.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    txtmobile.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    txtpassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                }

            }
            catch
            {

            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (txtadminname.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtadminname, "empty field");
            }
            else if (txtemail.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtemail, "empty field");
            }
            else if (txtmobile.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtmobile, "empty field");
            }
            else if (txtpassword.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtpassword, "empty field");
            }
            else
            {
                errorProvider1.Clear();
                try
                {
                    string query = "UPDATE `users` SET `user_name`='" + txtadminname.Text + "',`user_email`='" + txtemail.Text + "',`mobile_No`='" + txtmobile.Text + "',`password`='" + txtpassword.Text + "' WHERE `uid`='" + id + "'";
                    MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
                    MySqlDataReader dr;
                    con.connectDB.Open();
                    dr = cmd.ExecuteReader();
                    con.connectDB.Close();
                    MessageBox.Show("Admin Details Updated");
                    clear_box();
                    loadata();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                MessageBox.Show("Please select Admin");
            }
            else
            {
                try
                {
                    string query = "DELETE FROM `users` WHERE `uid`='" + id + "'";
                    MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);

                    con.connectDB.Open();
                    if (MyCommand2.ExecuteNonQuery() == 1)
                    {

                        MessageBox.Show("Deleted ");
                    }
                    con.connectDB.Close();
                    loadata();
                    clear_box();

                }
                catch
                {
                    MessageBox.Show("Please Contact Software developer", "Config - Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        private void clear_box()
        {
            txtadminname.Clear();
            txtemail.Clear();
            txtmobile.Clear();
            txtpassword.Clear();

        }
    }
}
