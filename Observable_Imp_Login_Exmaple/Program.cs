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
                    
                    Thread.Sleep(1000);
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
                Interval = TimeSpan.FromSeconds(10).TotalMilliseconds
            };
            aTimer.Elapsed += SimulateServiceFailure;

        }


        static void SimulateThreadJob(object i)
        {
            ServiceSession serviceSession = ServiceSession.Instance;


            lock (serviceSession.data._workLock) {
                //Console.WriteLine(i + " wants to enter");
                serviceSession.data.Semaphore.Wait();
                Console.WriteLine($"Currently Runing Thread:[{i}]");
                //Console.WriteLine("Notify Publisher about failed Service...");
                serviceSession.Notify(!serviceSession.data.serviceAvailable);
                Thread.Sleep(100);
                //Console.WriteLine(i + " is leaving");       // a time.
                Console.WriteLine($"SimulateThreadJob - Semaphore Number Of Threads{serviceSession.data.Semaphore.CurrentCount},maxThreads={maxThreads} ");

                if(serviceSession.data.Semaphore.CurrentCount !=maxThreads)
                serviceSession.data.Semaphore.Release();
            }
            
        }

        static void SimulateServiceFailure(Object source, ElapsedEventArgs e)
        {
            ServiceSession serviceSession = ServiceSession.Instance;


            lock(serviceSession.data._workLock)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("******* Service Went down please wait 20 seconds... **********");
                Console.WriteLine();
                Thread.Sleep(20000);

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
}
