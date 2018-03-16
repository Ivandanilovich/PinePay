using Microsoft.Win32;
using PinePay.client;
using PinePay.serverStub;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Data.SQLite;
using System.IO;

namespace PinePay
{
    public partial class MainWindow : Window
    {
        private string key;
        private bool flagIsStarted = false;
        public static Task startS;

        public MainWindow()
        {
            if (!CheckFirstTime() && !flagIsStarted)
            {
                flagIsStarted = true;
                startS = new Task(JustStart);
                startS.Start();
            }
            InitializeComponent();
        }

        private void JustStart()
        {
            Client.Start();
            ServerStub.Start();
        }

        private bool CheckFirstTime()
        {
            var currentUserKey = Registry.CurrentUser.OpenSubKey("PinePayUserKey");
            if (currentUserKey == null)
            {
                var signUp = new SignUp();
                signUp.Show();
                Close();
                return true;
            }
            key = currentUserKey.GetValue("Password").ToString();
            return false;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Password.ToString().Equals(key))
            {
                var report = new Report();
                report.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Пароль неверен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    var currentUserKey = Registry.CurrentUser.OpenSubKey("PinePayUserKey", true);
                    currentUserKey.SetValue("key", t[0]);
                    currentUserKey.SetValue("password", t[1]);
                    currentUserKey.Close();
                    while ((s = str.ReadLine()) != null)
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
        }

        void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show(" ;;");
        }
    }
}
