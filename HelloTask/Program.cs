using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HelloTask
{
    class Program
    {
        private static ConcurrentStack<int> MyStack;
        public static void PrintSync()
        {
            var date = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                Print(i);
            }
            Console.WriteLine("Time takes : " +(DateTime.Now-date).TotalMilliseconds);
        }
        public static void Print(object num)
        {
            Console.WriteLine(num);
        }

        public static void PrintWithThreads()
        {
            var date = DateTime.Now;

            for (int i = 0; i < 10000; i++)
            {
                int num = i;
                Thread t1 = new Thread(Print);
                t1.Start(num);
                
            }
            
            Console.WriteLine("Time takes : " + (DateTime.Now - date).TotalMilliseconds);

        }
        public static void PrintWithThreadPool()
        {
            for (int i = 0; i < 1000; i++)
            {
                int num = i;
                //Thread t1 = new Thread(Print);
                //t1.Start(i);
                ThreadPool.QueueUserWorkItem(obj => Print(num)); ;

            }
        }
        public static void FillStack()
        {
            MyStack = new ConcurrentStack<int>();
            for (int i = 1; i <= 5000; i++)
            {
                MyStack.Push(i);
            }

        }
        public static void RunAsyncStack(int numOfTasks)
        {
            for (int i = 0; i < numOfTasks; i++)
            {
                Task.Run(() =>
                {
                    int num;

                    while (!MyStack.IsEmpty)
                    {
                        MyStack.TryPop(out num);
                        Print(num);
                    }
                }
);
            }

        } //for 2.1
        public static void RunStackParallel()
        {
            Parallel.ForEach(MyStack, f => { Print(f); });
        } //for 2.2

        static void Main(string[] args)
        {
            //PrintSync();
            //PrintWithThreads();
            //PrintWithThreadPool();
            FillStack();
            //RunAsyncStack(3);
            RunStackParallel();
            Console.ReadLine();
        }
    }
}
