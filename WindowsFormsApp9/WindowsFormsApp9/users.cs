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
    public partial class users : UserControl
    {
        DatabaseConnection con = new DatabaseConnection();
        public users()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("would you like to save this user?", "Users-Reg", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (txtusername.Text == "")
                {
                    MessageBox.Show("User name required.", "Users-Reg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtuseremail.Text == "")
                {
                    MessageBox.Show("User email required.", "Users-Reg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtpassword.Text == "")
                {
                    MessageBox.Show("User password required.", "Users-Reg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtmobile.Text == "")
                {
                    MessageBox.Show("User mobile number required.", "Users-Reg", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    save_user();

                }
            }

           
        }

        //****************************************************save user**************************************************
        
        private string username2;
        void save_user()
        {
            try
            {
  
              
                string roll = "user";

                string query1 = "SELECT user_name FROM `users` where `user_roll`='" + roll + "'and user_name='"+txtusername.Text+"'";
                MySqlCommand cmd = new MySqlCommand(query1, con.connectDB);
                con.connectDB.Open();
                MySqlDataReader dr;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    username2 =dr[0].ToString();            

                }
                dr.Close(); 
                con.connectDB.Close();

                if(txtusername.Text==username2)
                {
                    MessageBox.Show("Existung User", "Users-Reg", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    string query = "INSERT INTO `users`( `user_name`, `user_email`, `mobile_No`,`user_roll`,`password`) VALUES('" + txtusername.Text + "','" + txtuseremail.Text + "','" + txtmobile.Text + "','" + roll + "','" + txtpassword.Text + "')";
                    MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);
                    con.connectDB.Open();
                    MyCommand2.ExecuteNonQuery();     // Here our query will be executed and data saved into the database.  
                    con.connectDB.Close();

                    MessageBox.Show("Saved.");
                    clear_box();
                    load_users();
                }



                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //****************************************************load user**************************************************
        void load_users()
        {
            string roll = "user";
            dataGridView1.Rows.Clear();
            string query = "SELECT `uid`,`user_name`, `user_email`, `mobile_No`, `password`, `reg_date` FROM `users` where `user_roll`='" + roll+"'";
            int i = 0;
            MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);
            MySqlDataReader dr;
            con.connectDB.Open();
            dr = MyCommand2.ExecuteReader();

            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(dr["uid"].ToString(), dr["user_name"].ToString(), dr["user_email"].ToString(), dr["mobile_No"].ToString(), dr["password"].ToString(), dr["reg_date"].ToString());
            }
            con.connectDB.Close();
        }

        private void users_Load(object sender, EventArgs e)
        {
            load_users();
        }

        private void txtusername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtusername, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void txtuseremail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtuseremail, true, true, true, true);
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
        //****************************************************delete user**************************************************
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                MessageBox.Show("آرجو إختيار المستخدم من الجدول");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to delete this user? ", "Users-Reg", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        string query = "DELETE FROM `users` WHERE `uid`='" + id + "'";
                        MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);

                        con.connectDB.Open();
                        if (MyCommand2.ExecuteNonQuery() == 1)
                        {

                            MessageBox.Show("Deleted.");
                        }
                        con.connectDB.Close();
                        load_users();
                        clear_box();

                    }
                    catch
                    {
                        MessageBox.Show("Please Contact Software developer", "Config - Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        //****************************************************clear testbox**************************************************

        void clear_box()
        {
            txtusername.Text = "";
            txtuseremail.Text = "";
            txtmobile.Text = "";
            txtpassword.Text = "";
        }
        int id;
       
        //****************************************************Update**************************************************

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (txtusername.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtusername, "empty field");
            }
            else if (txtuseremail.Text == "")
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtuseremail, "empty field");
            }
            else if(txtmobile.Text=="")
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
                DialogResult dialogResult = MessageBox.Show("Would you like to update this user details ?", "User-Reg", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    try
                    {
                        string query = "UPDATE `users` SET `user_name`='" + txtusername.Text + "',`user_email`='" + txtuseremail.Text + "',`mobile_No`='" + txtmobile.Text + "',`password`='" + txtpassword.Text + "' WHERE `uid`='" + id + "'";
                        MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
                        MySqlDataReader dr;
                        con.connectDB.Open();
                        dr = cmd.ExecuteReader();
                        con.connectDB.Close();
                        MessageBox.Show("Updated..");
                        clear_box();
                        load_users();

                    }

                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }

                }
            }
           
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
          try {
                if(dataGridView1.Rows!=null)
                {
                    id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    txtusername.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                    txtuseremail.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    txtmobile.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    txtpassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                }
              
            }
            catch
            {

            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
