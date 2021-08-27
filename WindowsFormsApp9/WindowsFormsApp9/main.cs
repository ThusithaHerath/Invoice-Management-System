using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp9
{
    public partial class main : Form
    {
        private string useremail;
        private string roll;
        public main(string s,string a)
        {
            InitializeComponent();
            useremail=a;
            roll = s;
            
        }

        private void main_Load(object sender, EventArgs e)
        {
            label1.Text = roll.ToString();
            lbluser.Text = useremail.ToString();

            timer1.Start();
            label3.Text = DateTime.Now.ToLongDateString();
            if(roll=="user")
            {
                guna2GradientButton5.Enabled = false;
                btnedit.Visible = false;
            }
            else
            {
               
            }

            Dashboard obj = new Dashboard(lbluser.Text);
            showContorl(obj);
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Dashboard obj = new Dashboard(lbluser.Text);
            showContorl(obj);

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            activity obj = new activity(lbluser.Text);
            showContorl(obj);

        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            Form1 ob = new Form1();
            ob.Show();
            this.Close();
        }
        public void showContorl(Control control)    // User cobtrol Eaxg forms load to Content Panel
        {
            Container_panel.Controls.Clear();

            control.Dock = DockStyle.Fill;
            control.BringToFront();
            control.Focus();

            Container_panel.Controls.Add(control);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongTimeString();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            users obj = new users();
            showContorl(obj);
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            payments obj = new payments(lbluser.Text);
            showContorl(obj);
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            reports obj = new reports();
            showContorl(obj);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            

        }

        private void btnedit_Click_1(object sender, EventArgs e)
        {
            AdminController obj = new AdminController();
           
            showContorl(obj);
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            frmEmails obj = new frmEmails();

            showContorl(obj);
        }
    }
}
