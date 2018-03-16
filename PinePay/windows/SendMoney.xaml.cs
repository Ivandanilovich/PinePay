using PinePay.client;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PinePay
{
    public partial class SendMoney : Window
    {
        DispatcherTimer dispatcherTimer;
        private int timerCount=0;

        public SendMoney()
        {
            InitializeComponent();
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            var report = new Report();
            report.Show();
            Close();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string id = ID.Text.ToString();
            string cashS = Cash.Text.ToString();
            if (id != "" && id != null && Double.TryParse(cashS, out double cash))
            {
                Task send = new Task(() => JustSend(id, cash));
                send.Start();
            }
            else
            {
                MessageBox.Show("Некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void JustSend(string id, double cash)
        {
            var con = new SQLiteConnection("Data Source=PinePay.db;");
            con.Open();
            string s = String.Format("INSERT INTO 'transactions' ('flag', 'cash', 'date') VALUES (0, {0}, '{1}');",
                cash.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")), DateTime.Now.ToString());
            var com = new SQLiteCommand(s, con);
            com.ExecuteNonQuery();
            con.Close();
            if(Client.onReceive(id, cash))
            {
                MessageBox.Show("Перевод осуществлен");
            }
            else
            {
                MessageBox.Show("Произошла ошибка на стороне сервера,/nзначение равно нулю", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private new void Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.startS.Dispose();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            timerCount = 0;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            timerCount = 0;
        }
    }
}
