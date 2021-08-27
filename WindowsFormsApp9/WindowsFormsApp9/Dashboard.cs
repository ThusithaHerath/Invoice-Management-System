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
using WIA;
using static WindowsFormsApp9.ADFScanner;
using System.Drawing.Imaging;

namespace WindowsFormsApp9
{
    public partial class Dashboard : UserControl
    {
        ADFScan _scanner;
        int[] _colors = { 1, 2, 4 };
        int count = 0;

        DatabaseConnection con = new DatabaseConnection();
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
       
        private string username;
        public Dashboard(string user)
        {
            InitializeComponent();
            username = user;
           
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            this.combobox_sup.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox1.SelectedIndex = 0;
        }
       
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Registernewcustomer obj = new Registernewcustomer();
            obj.Show();
        }
        private void TriggerScan()
        {
            Console.WriteLine("تم المسح بنجاح");
        }

     
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            _scanner = new ADFScan();
            _scanner.Scanning += new EventHandler<WiaImageEventArgs>(_scanner_Scanning);
            _scanner.ScanComplete += new EventHandler(_scanner_ScanComplete);
            ScanColor selectedColor = (ScanColor)_colors[comboBox1.SelectedIndex];
            int dpi = (int)numericUpDown1.Value;
            _scanner.BeginScan(selectedColor, dpi);

        }
        void _scanner_ScanComplete(object sender, EventArgs e)
        {
            MessageBox.Show("Scan Complete");
        }
        void _scanner_Scanning(object sender, WiaImageEventArgs e)
        {
            string filename = txtoutput.Text + "/image" + (count++).ToString() + ".jpg";
            listBox1.Items.Add(filename);
            e.ScannedImage.Save(filename, ImageFormat.Jpeg);//FILES ARE RETURNED AS BITMAPS
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if(txtinvoiceid.Text=="" || txtinvoiceid.Text ==null)
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtinvoiceid, "empty field");
            }
            else if(txtinvoicetotal.Text=="" || txtinvoicetotal.Text==null)
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtinvoicetotal, "empty field");
            }
            else if(txtpaidamount.Text=="" || txtpaidamount.Text ==null)
            {
                errorProvider1.Clear();

                errorProvider1.SetError(txtpaidamount, "empty field");
            }
            else if(flowLayoutPanel1.Controls.Count==0)
            {
                MessageBox.Show("Please scan invoice", "ScannerImage",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                errorProvider1.Clear();
                try
                {
                    string query = "SELECT * FROM `invoices` WHERE `invoice_id`='" + txtinvoiceid.Text + "'";
                    MySqlDataAdapter adp = new MySqlDataAdapter(query, con.connectDB);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    con.connectDB.Close();
                    if (dt.Rows.Count == 1)
                    {
                        MessageBox.Show("Existing Invoice Number", "Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    else
                    {
                        save_invoice();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            newinvoice();

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            newinvoice();
        }

         public void supplier_load()
        {
            MySqlDataAdapter adp = new MySqlDataAdapter("SELECT `supplier_name` FROM `suppliers` ", con.connectDB);
            DataSet dt = new DataSet();
            adp.Fill(dt);
            combobox_sup.DataSource = dt.Tables[0];
            combobox_sup.DisplayMember = dt.Tables[0].Columns[0].ToString();

            con.connectDB.Close();
        }

        private void combobox_sup_MouseClick(object sender, MouseEventArgs e)
        {
            supplier_load();
        }

        private void combobox_sup_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(combobox_sup, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void txtinvoiceid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtinvoiceid, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void txtinvoicetotal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtinvoicetotal, true, true, true, true);
                e.Handled = e.SuppressKeyPress = true;
            }
        }
       //************************************************Select supplier name where id*******************************************************************
        int supid;
        private void combobox_sup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = combobox_sup.Text;
            string query = "SELECT *FROM `suppliers` WHERE `supplier_name`='" + name + "'";

            MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
            con.connectDB.Open();
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();

            if (dr.HasRows)
            {
                dr.Read();

                supid = int.Parse(dr[0].ToString());
                
            }
            dr.Close();
            con.connectDB.Close();
        }

        //************************************************save invoice*******************************************************************
        decimal due_amount = 0;
        string status;
        void save_invoice()
        {
            string date = guna2DateTimePicker1.Text;
                decimal total_amount = decimal.Parse(txtinvoicetotal.Text);
            decimal paid_amoun = decimal.Parse(txtpaidamount.Text);
            
            
            string invoice_id = txtinvoiceid.Text;

            due_amount = total_amount - paid_amoun;
            if(due_amount==0)
            {
                status = "P";
            }
            else
            {
                status = "N/P";
            }
            
            string query = "INSERT INTO `invoices`(`invoice_id`, `sup_id`, `inovoice_total`, `paid_amount`, `due_amount`, `status`,`created_at`,`insert_by`) VALUES(@invoice_id,@supid,@total_amount,@paid_amoun,@due_amount,@status,@date,@username)";
            MySqlCommand MyCommand2 = new MySqlCommand(query, con.connectDB);
            con.connectDB.Open();

            MyCommand2.Parameters.Add(new MySqlParameter("@invoice_id", invoice_id)); 
            MyCommand2.Parameters.Add(new MySqlParameter("@supid", supid));
            MyCommand2.Parameters.Add(new MySqlParameter("@total_amount", total_amount));
            MyCommand2.Parameters.Add(new MySqlParameter("@paid_amoun", paid_amoun));
            MyCommand2.Parameters.Add(new MySqlParameter("@due_amount", due_amount));
            MyCommand2.Parameters.Add(new MySqlParameter("@status", status));
            MyCommand2.Parameters.Add(new MySqlParameter("@date", date));
            MyCommand2.Parameters.Add(new MySqlParameter("@username", @username));

            MyCommand2.ExecuteNonQuery();     // Here our query will be executed and data saved into the database.  
            con.connectDB.Close();

           
           for(int i=0;i< flowLayoutPanel1.Controls.Count; i++)
            {
                PictureBox p = (PictureBox)flowLayoutPanel1.Controls[i];
              
                var image1 = p.Image;
                byte[] image = GetBytesFromImage(image1);
               
                string query1 = "INSERT INTO `scan_images`(`invoice_id`, `image`) VALUES(@invoice_id,@path)";
                MySqlCommand MyCommand3 = new MySqlCommand(query1, con.connectDB);
                con.connectDB.Open();

                MyCommand3.Parameters.Add(new MySqlParameter("@invoice_id", invoice_id));
                 MyCommand3.Parameters.Add(new MySqlParameter("@path", image));


                MyCommand3.ExecuteNonQuery();     // Here our query will be executed and data saved into the database.  
                con.connectDB.Close();
            }

            allupdates();




            MessageBox.Show("Invoice Saved", "IMS",MessageBoxButtons.OK,MessageBoxIcon.Information);
            flowLayoutPanel1.Controls.Clear();
            listBox1.Items.Clear();
            cnt = 0;

        }
        byte[] GetBytesFromImage(System.Drawing.Image images)
        {
            MemoryStream ms = new MemoryStream();
            //Image img = Image.FromFile(imageFile);
            images.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }
        //*******************************************************************************************************************************

        private void allupdates()
        {
            string status1 = "Inoice Inserted";

            string query = "INSERT INTO `all_updates`(`invoID`, `supID`, `Invoicr_total`, `Paid`, `Due`, `Status`, `update_by`,`order_status`) VALUES('" + txtinvoiceid.Text+"','"+supid+"','"+txtinvoicetotal.Text+"','"+txtpaidamount.Text+"','"+ due_amount+"','"+status1+"','"+username+"','"+status+"')";

            MySqlCommand cmd = new MySqlCommand(query, con.connectDB);
            con.connectDB.Open();
            cmd.ExecuteNonQuery();
            con.connectDB.Close();
        }


        //************************************************new invoice*******************************************************************
        void newinvoice()
        {
            txtinvoiceid.Clear();
            txtinvoicetotal.Clear();
            txtpaidamount.Clear();
            combobox_sup.DataSource = null;
            flowLayoutPanel1.Controls.Clear();
            listBox1.Items.Clear();
            cnt = 0;

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
        int counts = 0;
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            ////Your opendialog box title name.
            openFileDialog1.Title = "Select scanned Images";

            //which type file format you want to upload in database. just add them.

            openFileDialog1.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|All file(*.*)|*.*";
            //FilterIndex property represents the index of the filter currently selected in the file dialog box.
            openFileDialog1.FilterIndex = 1;
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialog1.CheckFileExists)
                    {


                        int x = 20;
                        int y = 20;
                        int maxheight = -1;
                        PictureBox pic = new PictureBox();
                        pic.Height = 400;
                        pic.Width = 400;
                        pic.BackColor = Color.Black;
                        pic.Location = new Point(x, y);
                        pic.Size = new Size(400, 400);
                        pic.SizeMode = PictureBoxSizeMode.Zoom;

                        x += pic.Width + 10;
                        maxheight = Math.Max(pic.Height, maxheight);
                        if (x > this.ClientSize.Width - 100)
                        {
                            x = 20;
                            y += maxheight + 10;
                        }
                        pic.Click += new EventHandler(picture_click);
                        flowLayoutPanel1.Controls.Add(pic);
                        string path = System.IO.Path.GetFullPath(openFileDialog1.FileName);

                        PictureBox p = (PictureBox)flowLayoutPanel1.Controls[counts];
                        p.Image = new Bitmap(path);
                        counts ++;

                  

                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txtinvoicetotal_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtpaidamount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fld = new FolderBrowserDialog();
            if (fld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtoutput.Text = fld.SelectedPath;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        int cnt = 0;
       
        void picture_click(object sender, EventArgs e)
        {
            PictureBox pic2remove = (PictureBox)sender;
           
            flowLayoutPanel1.Controls.Remove(pic2remove);
            cnt--;
            listBox1.Items.Clear();

        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void btndeletscanneimage_Click(object sender, EventArgs e)
        {
            Control[] ctrls = flowLayoutPanel1.Controls.Find("searchkey", false);

            // Note: only one can be selected (made active), so if Find returns more than one,
            // the last one in array will be the selected control using this loop...
            foreach (Control c in ctrls)
                c.Select();

            // if only one control returned (exact match), make it active...
            if (ctrls.Length > 0)
            {
                ctrls[0].Select();

                // to remove this control...
                flowLayoutPanel1.Controls.Remove(ctrls[0]);
            }

            //listBox1.Items.Clear();
            //flowLayoutPanel1.Controls.Clear();
        }

        private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
          
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int x = 20;
                int y = 20;
                int maxheight = -1;
                PictureBox pic = new PictureBox();
                pic.Height = 400;
                pic.Width = 400;
                pic.BackColor = Color.Black;
                pic.Location = new Point(x, y);
                pic.Size = new Size(400, 400);
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                
                x += pic.Width + 10;
                maxheight = Math.Max(pic.Height, maxheight);
                if (x > this.ClientSize.Width - 100)
                {
                    x = 20;
                    y += maxheight + 10;
                }
                pic.Click += new EventHandler(picture_click);
                flowLayoutPanel1.Controls.Add(pic);

                PictureBox p = (PictureBox)flowLayoutPanel1.Controls[cnt];
                p.Image = Image.FromFile(listBox1.SelectedItem.ToString());
                cnt++;

            }
        }
    }

}
