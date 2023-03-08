// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

/// Kyle Byassee
/// 2023-03-07
/// 
/// This program estimates pi via the Monte Carlo method
/// 

namespace Threading
{
    class FindPiThread
    {
        private int dartsThrown = 0;
        private int dartsInside = 0;
        private readonly Random randy;

        public FindPiThread(int toThrow)
        {
            dartsThrown = toThrow;
            randy = new Random();
        }

        public int getdartsInside()
        {
            return dartsInside;
        }

        public void throwDarts()
        {
            for (int i = 0; i < dartsThrown; i++)
            {
                double x = randy.NextDouble();
                double y = randy.NextDouble();

                double r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

                if (r <= 1)
                {
                    dartsInside++;
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Number of throws each thread should make: ");
            int throws = Int32.Parse(Console.ReadLine());
            Console.Write("Number of threads to run: ");
            int threads = Int32.Parse(Console.ReadLine());

            List<Thread> threadList = new(threads);
            List<FindPiThread> piList = new(threads);

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            for (int i = 0; i < threads; i++)
            {
                FindPiThread piThread = new(throws);
                piList.Add(piThread);
                Thread myThread = new(new ThreadStart(piThread.throwDarts));
                threadList.Add(myThread);
                myThread.Start();
                Thread.Sleep(16);
            }

            foreach (Thread i in threadList)
            {
                i.Join();
            }

            int numInside = 0;
            foreach (FindPiThread i in piList)
            {
                numInside += i.getdartsInside();
            }

            double pi = 4 * ((double)numInside / (throws * threads));
            watch.Stop();

            Console.WriteLine("pi is approx = " + pi);
            Console.WriteLine($"Calculation time: {watch.ElapsedMilliseconds} ms");

            Console.ReadKey();
        }
    }
}