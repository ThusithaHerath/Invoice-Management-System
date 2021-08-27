using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp9
{
 
    class DatabaseConnection
    {

        public MySqlConnection connectDB;
        public DatabaseConnection()
        {

            connectDB = new MySqlConnection("server=localhost;port=3306;uid=root;password=;database=invoice_db");

        }

    }
}
