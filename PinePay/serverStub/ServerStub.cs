using PinePay.client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PinePay.serverStub
{
    public static class ServerStub
    {
        static public event Action<double> justSend;
        static private int n = 10;
        static private Random[] rnd = new Random[n];

        public static void Start()
        {
            Task receiver = new Task(ReceiveData);
            receiver.Start();
            for (int i = 0; i < n; i++)
            {
                rnd[i] = new Random(i);
            }
            ThreadPool.SetMaxThreads(n - 1, n - 1);
            ThreadPool.GetMaxThreads(out int f, out int y);
            Console.WriteLine(f + " " + y);
            while (true)
            {
                for (int i = 0; i < n; i++)
                {
                    ThreadPool.QueueUserWorkItem(Send);
                }
                Thread.Sleep(1000 * 60);
            }
        }

        public static void Send(object obj)
        {
            Console.WriteLine("TREAD " + Thread.CurrentThread.ManagedThreadId + " " + Thread.CurrentThread.ManagedThreadId % 10);
            double x = rnd[Thread.CurrentThread.ManagedThreadId % 10].Next(1, 200000) / 100d;
            justSend(x);
        }

        static private void ReceiveData()
        {
            Client.onReceive += OnReceive;
        }

        static private bool OnReceive(string id, double cash)
        {
            //throw new NotImplementedException();  // Обработка данных от клиента
            Console.WriteLine(@"ServerStub: id={0}, cash={1}", id, cash);
            if (id != "" && id != null && cash != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
