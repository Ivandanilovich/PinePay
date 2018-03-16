using Microsoft.Win32;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace PinePay
{
    public partial class SignUp : Window
    {
        public SignUp()
        {
            CreateTable();
            InitializeComponent();
        }

        private string GenerateKey()
        {
            string key = "";
            var r = new Random();
            for (int i = 0; i < 32; i++)
            {
                key += ((char)r.Next(48, 123)).ToString();
            }
            return key;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string pass1 = Password1.Password.ToString(),
                pass2 = Password2.Password.ToString();
            if (pass1.Length != 6 || pass2.Length != 6)
            {
                MessageBox.Show("Пароль должен состоять из шести символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!pass1.Equals(pass2))
            {
                MessageBox.Show("Пароли неравны", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var currentUserKey = Registry.CurrentUser.CreateSubKey("PinePayUserKey");
            var key = GenerateKey();
            currentUserKey.SetValue("key", key);
            currentUserKey.SetValue("password", pass1);
            currentUserKey.Close();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void CreateTable()
        {
            SQLiteConnection.CreateFile("PinePay.db");
            var con = new SQLiteConnection("Data Source = PinePay.db;");
            var com = new SQLiteCommand("CREATE TABLE transactions (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, flag INTEGER, cash REAL, date TEXT);", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

        private new void Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.startS.Dispose();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                Stream st;
                if ((st = dlg.OpenFile()) != null)
                {
                    StreamReader str = new StreamReader(st);
                    string s;
                    s = str.ReadLine();
                    string[] t = s.Split(' ');
                    var currentUserKey = Registry.CurrentUser.CreateSubKey("PinePayUserKey");
                    currentUserKey.SetValue("key", t[0]);
                    currentUserKey.SetValue("password", t[1]);
                    currentUserKey.Close();
                    while ((s = str.ReadLine())!=null)
                    {
                        string[] tok = s.Split(' ');
                        var con = new SQLiteConnection("Data Source=PinePay.db;");
                        con.Open();
                        string date = tok[3] + " " + tok[4];
                        string scom = String.Format("INSERT INTO 'transactions' ('flag', 'cash', 'date') VALUES ('{0}', '{1}', '{2}');", tok[1], tok[2], date);
                        var com = new SQLiteCommand(scom, con);
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
