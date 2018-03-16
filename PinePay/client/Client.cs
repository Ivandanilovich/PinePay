using PinePay.serverStub;
using System;
using System.Data.SQLite;

namespace PinePay.client
{
    public delegate bool Sender(string id, double cash);

    public static class Client
    {
        public static Sender onReceive;
        static object locker = new object();

        public static void Start()
        {
            ServerStub.justSend += Halndle;
        }

        private static void Halndle(double x)
        {
            lock (locker)
            {
                var con = new SQLiteConnection("Data Source=PinePay.db;");
                con.Open();
                string s = String.Format("INSERT INTO 'transactions' ('flag', 'cash', 'date') VALUES (1, {0}, '{1}');",
                    x.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")), DateTime.Now.ToString());
                var com = new SQLiteCommand(s, con);
                com.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
