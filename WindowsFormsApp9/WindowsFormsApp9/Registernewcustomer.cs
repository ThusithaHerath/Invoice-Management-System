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

namespace WindowsFormsApp9
{
    public partial class Registernewcustomer : Form
    {
        DatabaseConnection con = new DatabaseConnection();
        public Registernewcustomer()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if(txtname.Text=="")
            {
                MessageBox.Show("Supplier name required", "Supplier-Reg", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(txtcompany.Text=="")
            {
                MessageBox.Show("Supplier company required", "Supplier-Reg", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(txtmobile.Text=="")
            {
                MessageBox.Show("Supplier mobile required", "Supplier-Reg", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to save this supplier ?", "Supplier-Reg", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        string query = "INSERT INTO `suppliers`(`supplier_name`, `company`, `mobile`,`address`) VALUES('" + txtname.Text + "','" + txtcompany.Text + "','" + txtmobile.Text + "','"+guna2TextBox1.Text+"')";

                        MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
                        con.connectDB.Open();
                        cmd.ExecuteNonQuery();
                        con.connectDB.Close();

                        MessageBox.Show("Saved", "Supplier-Reg", MessageBoxButtons.OK, MessageBoxIcon.Information); //supplier saved

                    }
                    catch
                    {
                        MessageBox.Show("Please Contact Software developer", "Config - Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Not saved");
                }
            }
        }

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtcompany.Focus();

            }
        }

        private void txtcompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtmobile.Focus();

            }
        }

        private void txtmobile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2TextBox1.Focus();

            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button1.PerformClick();

            }
        }
    }
}
