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
    public partial class frmEmails : UserControl
    {
        DatabaseConnection con = new DatabaseConnection();
        public frmEmails()
        {
            InitializeComponent();
        }
       


    }
}
