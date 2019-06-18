using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Observable_Imp_Login_Exmaple.Model
{
    public class ServiceStatusData
    {
        public ServiceStatusData(int numberOfThreads = 1)
        {
            serviceAvailable = true;
            __lockWasTaken = false;
            Semaphore = new SemaphoreSlim((numberOfThreads < 0 || numberOfThreads > 100 * 1000) ? 10 : numberOfThreads);
        }
        public bool serviceAvailable { get; set; } //isRemoteServiceAvailable?

        public  bool __lockWasTaken = false;


        public SemaphoreSlim Semaphore { get; set; } //set number of concurrent threads

        public readonly object _workLock = new object(); //pause all work on Subscriber, service is down for some reason...

        internal ServiceStatusData Clone()
        {
            return new ServiceStatusData { Semaphore = this.Semaphore, serviceAvailable = this.serviceAvailable };
        }
    }
}
