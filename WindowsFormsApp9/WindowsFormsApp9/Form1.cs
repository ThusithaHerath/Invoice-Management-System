using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        DatabaseConnection con=new DatabaseConnection();   //database connection class

        public Form1()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentUICulture =
                new System.Globalization.CultureInfo("ar-EG");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtuseremail.Focus();       
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState =FormWindowState.Minimized;
        }     

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Login_function();
            

        }

        private void txtuseremail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtuseremail, true, true, true, true);
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

        void Login_function()
        {
          
            if(txtuseremail.Text=="")
            {
                MessageBox.Show("User Name Required..", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(txtpassword.Text=="")
            {
                MessageBox.Show("User Password Required..", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                           
                    string email = txtuseremail.Text;
                    string password = txtpassword.Text;
                    string roll;

                    try
                    {
                        for (int i = 0; i <= 100; i++)
                        {
                            Thread.Sleep(5);
                            guna2CircleProgressBar1.Value = i;
                            guna2CircleProgressBar1.Update();
                        }

                        string query = "SELECT * FROM `users` WHERE `user_name`='" + email + "'AND `password`='" + password + "'";
                        MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
                        con.connectDB.Open();
                        MySqlDataReader dr;
                        dr = cmd.ExecuteReader();

                    

                        if (dr.HasRows)
                        {
                            dr.Read();

                            roll = dr[5].ToString();
                            main ob = new main(roll,email);
                            ob.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Login Details Invalid,Please enter valid username and password ", "Login - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    con.connectDB.Close();
                    dr.Close();
                }
                    catch (Exception)
                    {
                        MessageBox.Show("Please Contact Software developer", "Config - Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                
              

            }
        }
    }
}
