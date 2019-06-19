using Observable_Imp_Login_Exmaple.Model;
using Observable_Imp_Login_Exmaple.Publisher;
using Observable_Imp_Login_Exmaple.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Observable_Imp_Login_Exmaple
{
    class Program
    {
        private static System.Timers.Timer aTimer;
        public static ServiceSession serviceSession = ServiceSession.Instance;

        public static int maxThreads = 1;

        static void Main(string[] args)
        {

            //This main is used to simulate multiple threads who want to reach to the same class that provides access to some login feature


            SetTimer();
            aTimer.Start();

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);


                while (true)
                {

                    for (int i = 1; i <= maxThreads; i++)
                    {
                        Thread t1 = new Thread(SimulateThreadJob);
                        t1.Start(i);
                    }
                    
                    Thread.Sleep(500);
                }



            Console.Read();
            aTimer.Stop();

        }

        public static void SetTimer()
        {
            // Create a timer with a two second interval.
            // Hook up the Elapsed event for the timer. 
            //aTimer.Enabled = true;
            aTimer = new System.Timers.Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = TimeSpan.FromSeconds(4).TotalMilliseconds
            };
            aTimer.Elapsed += SimulateServiceFailure;

        }


        static void SimulateThreadJob(object i)
        {
            ServiceSession serviceSession = ServiceSession.Instance;


                serviceSession.data.Semaphore.WaitOne();
                Console.WriteLine($"Currently Runing Thread:[{i}]");
                serviceSession.Notify(!serviceSession.data.serviceAvailable);
                Thread.Sleep(500);
                serviceSession.data.Semaphore.Release();
            
            
        }

        static void SimulateServiceFailure(Object source, ElapsedEventArgs e)
        {
            ServiceSession serviceSession = ServiceSession.Instance;

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("******* Service Went down please wait 4 seconds... **********");
                Console.WriteLine();
                Console.WriteLine("******* Blocking...... **********");
                Console.WriteLine("******* Blocking...... **********");
                Console.WriteLine("******* Blocking...... **********");

            serviceSession.BlockSemaphore();

                Thread.Sleep(4000);

            Console.WriteLine("******* Un Blocking...... **********");
            Console.WriteLine("******* Un Blocking...... **********");
            Console.WriteLine("******* Un Blocking...... **********");

            serviceSession.SetNumberOfThreads(maxThreads += 2);

                Console.WriteLine($"******* SetNumberOfThreads to |{maxThreads}|... **********");

                Console.WriteLine("******* Service is up and Running... **********");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
          


        }
    }
}
