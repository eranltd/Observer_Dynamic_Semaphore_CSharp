using Observable_Imp_Login_Exmaple.Model;
using Observable_Imp_Login_Exmaple.Publisher;
using Observable_Imp_Login_Exmaple.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observable_Imp_Login_Exmaple
{
    public class ServiceSession : IObserver<ServiceStatusData>
    {

        private IDisposable unsubscriber;

         public ServiceStatusData data;

        //Singletone, ThreadSafe
        public static ServiceSession Instance { get; } = new ServiceSession();

        //Observer Implementation Section...

        //publisher
        public static ServiceStatusProvider serviceStatusProvider;

        //subscriber
        //this class...


            //Observer Implementation Section...




            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
        static ServiceSession()
        {
        }
        private ServiceSession()
        {
            serviceStatusProvider = new ServiceStatusProvider();
            data = new ServiceStatusData();
            unsubscriber = serviceStatusProvider.Subscribe(this);

        }

        public void Status()
        {
            Console.WriteLine();
            Console.WriteLine($"Got Updated From Publisher");
            Console.WriteLine($"ServiceAvailable:{data.serviceAvailable}, SemaphoreNumberOfThreads{data.Semaphore.MaximumCount} ");
            Console.WriteLine();

        }

        public void Subscribe(IObservable<ServiceStatusData> provider)
        {
            if (unsubscriber == null)
            {
                unsubscriber = provider.Subscribe(this);
            }
            Console.WriteLine("Subscribed Successfully");

        }



        public void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted - ServiceAvailable got notified from Publisher");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("OnError - ServiceAvailable got notified from Publisher about an Error");
        }

        public void OnNext(ServiceStatusData value)
        {
            this.data = value;
            Status();
        }


        public void Notify(bool boolean) => serviceStatusProvider.Notify(boolean);

        public void Notify(ServiceStatusData data) => serviceStatusProvider.Notify(data);

        public void SetNumberOfThreads(int n) => serviceStatusProvider.SetNumberOfThreads(n);

        public void BlockSemaphore() => serviceStatusProvider.BlockSemaphore();

    }
}
