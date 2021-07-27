using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test01.Test01.Model;

namespace Test01.Test01
{
    class Realization
    {
        private const string _way = "./data";
        List<Product> productsByThreadPool = new List<Product>();
        List<Product> productsByParallelFor = new List<Product>();
        List<Product> productsByTask = new List<Product>();

        private void CheckWay(in string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path is empty eigther null");
            }

            if (!Directory.Exists(path))
            {
                throw new ArgumentException("incorrect way to file");
            }
        }

        public async void Run()
        {
            Task[] tasks;
            Task<double> summ;

            Stopwatch stopwatch = new Stopwatch();
            Converter converter = new Converter();

            CheckWay(_way);
            string[] paths = Directory.GetFiles(_way);
            int count = paths.Length;


            tasks = new Task[paths.Length];

            stopwatch.Start();

           int toProcess = paths.Length;
              using (ManualResetEvent resetEvent = new ManualResetEvent(false))
              {
                  for (int i = 0; i < paths.Length; ++i)
                  {
                      ThreadPool.QueueUserWorkItem((some) => { converter.ParceCSV(productsByThreadPool, paths[(int)some]);
                          if (Interlocked.Decrement(ref toProcess) == 0)
                          { resetEvent.Set(); }
                      }, i);
                  }

                  resetEvent.WaitOne();
              }
              stopwatch.Stop();   

              Console.WriteLine("Time of parce by threadPool is " + stopwatch.Elapsed.TotalMilliseconds);

              stopwatch.Restart();

              Parallel.For(0, paths.Length , (index) =>
              {
                  int i = index;
                  converter.ParceCSV(productsByParallelFor, paths[i]);
              });

              stopwatch.Stop();

              Console.WriteLine("Time of parce by Parallel.For is " + stopwatch.Elapsed.TotalMilliseconds);

              stopwatch.Restart();



            for (int i = 0; i < count; i++)
            {
                int index = i;

                tasks[i] = Task.Run(() =>
                {
                    converter.ParceCSV(productsByTask, paths[index]);
                });

            }

            Task.WaitAll(tasks);

            stopwatch.Stop();

            Console.WriteLine("Time of parce by Task is " + stopwatch.Elapsed.TotalMilliseconds);

            summ = CalculateSumTotalProfitsAsync();

            summ.Wait();

            Console.WriteLine("summ of totalProfits is " +  summ.Result);

            Console.ReadKey();
        }

        private async Task<double> CalculateSumTotalProfitsAsync()
        {
            double number = await Task.Run(CalculateSumTotalProfits);

            return number;
        }

        private double CalculateSumTotalProfits()
        {
            if (productsByTask is null)
            {
                throw new ArgumentNullException("collection is null");
            }


            double result = 0;

            for (int i = 0; i < productsByTask.Count; ++i)
            {
                result += (double)productsByTask[i].TotalProfit;
            }

            Task.Delay(10_000);

            return result;
        }


    }
}
