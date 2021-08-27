using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;
using static WindowsFormsApp9.ADFScanner;

namespace WindowsFormsApp9
{
    public partial class image_view : Form
    {

        ADFScan _scanner;
        int[] _colors = { 1, 2, 4 };
        int count = 0;


        private PictureBox pic = new PictureBox();
        DatabaseConnection con = new DatabaseConnection();
        string id;
    
        public image_view(string a)
        {
            id = a;           
            InitializeComponent();
        }
        
    void getData()
      {
        
            con.connectDB.Open();
            MySqlCommand cm=new MySqlCommand("SELECT `image` FROM `scan_images` WHERE `invoice_id`= '" + id + "'", con.connectDB);
            MySqlDataAdapter adp = new MySqlDataAdapter(cm);
            MySqlDataReader read = cm.ExecuteReader(); ;


            while (read.Read())
            {
             
                byte[] images = (byte[])read["image"];
               
                pic = new PictureBox();
                pic.Width = 400;
                pic.Height = 400;
                pic.BackgroundImageLayout = ImageLayout.Stretch;
                pic.SizeMode = PictureBoxSizeMode.Zoom;

                Image s = ByteToImage(images);
                //pic.Click += new EventHandler(butns_Click);
                flowLayoutPanel1.Controls.Add(pic);
              
                pic.Image = new Bitmap(s);
            }
            con.connectDB.Close();
            read.Close();
            
      }
        //private void butns_Click(object sender, EventArgs e)
        //{
        //    var pictureBox = (PictureBox)sender;
        //    int index = flowLayoutPanel1.Controls.GetChildIndex(pictureBox); 
        //    MessageBox.Show(index .ToString());
        //}

        private void image_view_Load(object sender, EventArgs e)
        {
            getData();
            comboBox1.SelectedIndex = 0;
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
        private int imagesToPrintCount;
        private DialogResult dialogResult;

        private void button1_Click(object sender, EventArgs e)
        {
            
            imagesToPrintCount = flowLayoutPanel1.Controls.Count;
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += Doc_printPage;
            PrintDialog dialog = new PrintDialog();
            dialog.Document = doc;
            doc.Print();
        }

        private void Doc_printPage(object sender, PrintPageEventArgs e)
        {

            e.Graphics.DrawImage(GetNextImage(), e.MarginBounds);
            e.HasMorePages = imagesToPrintCount > 0;
        }
        private Image GetNextImage()
        {
            PictureBox pictureBox = (PictureBox)flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - imagesToPrintCount];
            imagesToPrintCount--;
            return pictureBox.Image;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Would you like delete this scanned images ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                   

                    string query2 = "DELETE FROM `scan_images` WHERE `invoice_id`='" + id + "'";
                    MySqlCommand MyCommand1 = new MySqlCommand(query2, con.connectDB);
                    con.connectDB.Open();
                    MyCommand1.ExecuteNonQuery();
                    con.connectDB.Close();
                    MessageBox.Show("Deleted");
                    flowLayoutPanel1.Controls.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            
                FolderBrowserDialog fld = new FolderBrowserDialog();
                if (fld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtoutput.Text = fld.SelectedPath;
                }
            
        }
        int cnt = 0;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
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
        void picture_click(object sender, EventArgs e)
        {
            PictureBox pic2remove = (PictureBox)sender;

            flowLayoutPanel1.Controls.Remove(pic2remove);
            cnt--;


        }

        private void button3_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                PictureBox p = (PictureBox)flowLayoutPanel1.Controls[i];

                var image1 = p.Image;
                byte[] image = GetBytesFromImage(image1);

                string query1 = "INSERT INTO `scan_images`(`invoice_id`, `image`) VALUES(@invoice_id,@path)";
                MySqlCommand MyCommand3 = new MySqlCommand(query1, con.connectDB);
                con.connectDB.Open();

                MyCommand3.Parameters.Add(new MySqlParameter("@invoice_id", id));
                MyCommand3.Parameters.Add(new MySqlParameter("@path", image));


                MyCommand3.ExecuteNonQuery();     // Here our query will be executed and data saved into the database.  
                con.connectDB.Close();

                MessageBox.Show("Image saved");
                flowLayoutPanel1.Controls.Clear();
                listBox1.Items.Clear();
            }

        }
        byte[] GetBytesFromImage(System.Drawing.Image images)
        {
            MemoryStream ms = new MemoryStream();
            //Image img = Image.FromFile(imageFile);
            images.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }
    }
}
