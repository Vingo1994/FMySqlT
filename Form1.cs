using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace StaticalR
{

    public partial class Form1 : Form
    {
        static string dbHost = "10.2.35.35";        //需連結的資料庫IP位址
        static string dbUser = "centeruser";              //需連結的資料庫帳戶
        static string dbPass = "w12kytpc110";            //需連結的資料庫密碼
        static string dbName = "pbxrec";              //需連結的資料庫名稱

        static string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
        MySqlConnection conn = new MySqlConnection(connStr);

        string SQL;


        public Form1()
        {
            InitializeComponent();

            try
            {
                conn.Open();
                textBox1.Text = ("Connected Success!");
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        textBox1.Text = ("無法連線到資料庫.");
                        break;
                    case 1042:
                        textBox1.Text = ("IP 錯誤.");
                        break;
                    case 1045:
                        textBox1.Text = ("使用者帳號或密碼錯誤.");
                        break;
                }
            }

            SQL = $"SHOW tables FROM {dbName} like 'pbxlog%'";    //Find specify table

            using (MySqlCommand cmd = new MySqlCommand(SQL, conn))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string tbadapt = dr[0].ToString();
                        comboBox1.Items.Add(dr[0].ToString());
                        
                    }
                }
            }


        }




        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = (string)comboBox1.SelectedItem;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //Number of records in the table
            SQL = $"SELECT COUNT(*) FROM {comboBox1.Text} WHERE extno BETWEEN '2301' and '2304' and action='Answer'";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            int _count = (int)(long)cmd.ExecuteScalar();
            textBox1.Text = $"共{Convert.ToString(_count)}筆 ";
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
