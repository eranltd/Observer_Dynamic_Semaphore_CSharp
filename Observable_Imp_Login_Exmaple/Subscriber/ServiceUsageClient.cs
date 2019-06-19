using Observable_Imp_Login_Exmaple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observable_Imp_Login_Exmaple.Subscriber
{
    class ServiceUsageClient : IObserver<ServiceStatusData>
    {
        ServiceStatusData data;

        private IDisposable unsubscriber;

        public ServiceUsageClient()
        {

        }
        public ServiceUsageClient(IObservable<ServiceStatusData> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }
        public void Status()
        {
            Console.WriteLine($"Got Updated From Publisher");
            Console.WriteLine($"ServiceAvailable:{data.serviceAvailable}, SemaphoreNumberOfThreads{data.Semaphore.MaximumCount} ");
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
            Console.WriteLine("OnCompleted - ServiceUsageClient");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("OnError - ServiceUsageClient");
        }

        public void OnNext(ServiceStatusData value)
        {
            this.data = value;
            Status();
        }
    }
}
