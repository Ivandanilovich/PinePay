using Microsoft.Win32;
using PinePay.client;
using System;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PinePay
{
    public partial class Report : Window
    {
        Object locker = new Object();
        private int i = 0;
        double sum = 0;
        int timerCount = 0;
        DispatcherTimer dispatcherTimer;

        public Report()
        {
            InitializeComponent();
            ShowTransaction();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimerTick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            timerCount++;
            if (timerCount < 60)
                return;
            timerCount = 0;
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
            dispatcherTimer.Stop();
        }

        public void ShowTransaction()
        {
            var con = new SQLiteConnection("Data Source = PinePay.db;");
            con.Open();
            var select = new SQLiteCommand(String.Format("SELECT * FROM 'transactions' WHERE id>{0};", i), con);
            var reader = select.ExecuteReader();
            foreach (System.Data.Common.DbDataRecord record in reader)
            {
                i++;
                var g = new Grid();
                g.ColumnDefinitions.Add(new ColumnDefinition());
                g.ColumnDefinitions.Add(new ColumnDefinition());
                g.ColumnDefinitions.Add(new ColumnDefinition());
                int id = Int32.Parse(record["id"].ToString());
                int flag = Int32.Parse(record["flag"].ToString());
                double cash = Double.Parse(record["cash"].ToString());
                string date = record["date"].ToString();
                if (flag == 1)
                    sum += cash;
                else sum -= cash;
                Label l1 = new Label
                {
                    Content = flag == 1 ? "▼ Входящий" : "▲ Исходящий"
                };
                Label l2 = new Label
                {
                    Content = cash
                };
                Label l3 = new Label
                {
                    Content = date
                };
                Grid.SetColumn(l1, 0);
                g.Children.Add(l1);
                Grid.SetColumn(l2, 1);
                g.Children.Add(l2);
                Grid.SetColumn(l3, 2);
                g.Children.Add(l3);
                Page.Children.Add(g);
            }
            con.Close();
            Scroller.ScrollToBottom();
            Balance.Content = (String.Format("Баланс: {0} руб.", sum));
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            var sendMoney = new SendMoney();
            sendMoney.Show();
            Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }


        private void DeleteT_Click(object sender, RoutedEventArgs e)
        {
            Task deleteT = new Task(DeleteTable);
            deleteT.Start();
        }

        private void DeleteTable()
        {
            lock (locker)
            {
                var con = new SQLiteConnection("Data Source=PinePay.db;");
                con.Open();
                var com = new SQLiteCommand("DELETE FROM transactions", con);
                com.ExecuteNonQuery();
                var com2 = new SQLiteCommand("update sqlite_sequence set seq = 0 WHERE Name = 'transactions'", con);
                com2.ExecuteNonQuery();
                con.Close();
            }
            MessageBox.Show("Данные удалены");
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var currentUserKey = Registry.CurrentUser.CreateSubKey("PinePayUserKey");
            var pass = currentUserKey.GetValue("Password").ToString();
            var key = currentUserKey.GetValue("Key").ToString();
            string fileText = key + " " + pass + @"
";
            currentUserKey.Close();
            lock (locker)
            {
                var con = new SQLiteConnection("Data Source = PinePay.db;");
                con.Open();
                var select = new SQLiteCommand("SELECT * FROM 'transactions';", con);
                var reader = select.ExecuteReader();
                foreach (System.Data.Common.DbDataRecord record in reader)
                {
                    int id = Int32.Parse(record["id"].ToString());
                    int flag = Int32.Parse(record["flag"].ToString());
                    string cash = record["cash"].ToString();
                    string date = record["date"].ToString();

                    fileText += String.Format(@"{0} {1} {2} {3}
", id, flag, cash, date);
                }
                con.Close();
            }
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = ".txt",
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, fileText);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ShowTransaction();
        }

        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            timerCount = 0;
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            timerCount = 0;
        }
    }
}
