using Observable_Imp_Login_Exmaple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Observable_Imp_Login_Exmaple.Publisher
{
    public class ServiceStatusProvider : IObservable<ServiceStatusData>
    {
         ServiceStatusData serviceStatusData;

        List<IObserver<ServiceStatusData>> observers;

        public ServiceStatusProvider()
        {
            observers = new List<IObserver<ServiceStatusData>>();
            serviceStatusData = new ServiceStatusData();
        }

        public IDisposable Subscribe(IObserver<ServiceStatusData> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new UnSubscriber(observers, observer);
        }

        private class UnSubscriber : IDisposable
        {
            private List<IObserver<ServiceStatusData>> lstObservers;
            private IObserver<ServiceStatusData> observer;

            public UnSubscriber(List<IObserver<ServiceStatusData>> ObserversCollection,
                                IObserver<ServiceStatusData> observer)
            {
                this.lstObservers = ObserversCollection;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (this.observer != null)
                {
                    lstObservers.Remove(this.observer);
                }
            }
        }

        private void ServiceStatusChanged(ServiceStatusData data)
        {
            //deal with change...
            Console.WriteLine("ServiceStatusChanged fixing...");
            Console.WriteLine($"new data is:ServiceAvailable:{data.serviceAvailable}, SemaphoreNumberOfThreads{data.Semaphore.CurrentCount}");

            foreach (var obs in observers)
            {
                obs.OnNext(data.Clone());
                obs.OnCompleted();

            }
        }

        public void Notify(ServiceStatusData data)
        {
            ServiceStatusChanged(data);
        }

        public void SetNumberOfThreads(int numberOfThreads)
        {
            Console.WriteLine($"ServiceStatusProvider - SetNumberOfThreads = |{numberOfThreads}|");
            this.serviceStatusData.Semaphore = new SemaphoreSlim(numberOfThreads, numberOfThreads);
            ServiceStatusChanged(this.serviceStatusData);
        }

        public void Notify(bool serviceAvailable)
        {
            string workStatus = serviceAvailable ? "Working" : "Failed";
            Console.WriteLine($"Got Notified about a {workStatus} service...");
            this.serviceStatusData.serviceAvailable = serviceAvailable;
            ServiceStatusChanged(this.serviceStatusData);
        }

    }
}
