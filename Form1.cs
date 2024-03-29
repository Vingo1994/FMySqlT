﻿using System;
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
        static string dbHost = "127.0.0.1";        //需連結的資料庫IP位址
        static string dbUser = "user";              //需連結的資料庫帳戶
        static string dbPass = "password";            //需連結的資料庫密碼
        static string dbName = "test";              //需連結的資料庫名稱

        static string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
        MySqlConnection conn = new MySqlConnection(connStr);

        string SQL;

        private class PbxLog
        {
            public PbxLog(string table, string calender)
            {
                Table = table;
                Calender = calender;
            }

            public string Table { get; set; }
            public string Calender { get; set; }
            public override string ToString()
            {
                return Calender;
            }
        }
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
                      comboBox1.Items.Add(new PbxLog(dr[0].ToString(), dr[0].ToString().Remove(0, 7).Insert(3, "/")));
                    }
                }
            }

        }

        
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PbxLog item = comboBox1.Items[comboBox1.SelectedIndex] as PbxLog;
            textBox1.Text = item.Calender;
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //Number of records in the table
            PbxLog item = comboBox1.Items[comboBox1.SelectedIndex] as PbxLog;
            SQL = $"SELECT COUNT(*) FROM {item.Table} WHERE extno BETWEEN '2301' and '2304' and action='Answer'";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);

            int _count = (int)(long)cmd.ExecuteScalar();
            textBox1.Text = $"共{Convert.ToString(_count)}筆 ";
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

