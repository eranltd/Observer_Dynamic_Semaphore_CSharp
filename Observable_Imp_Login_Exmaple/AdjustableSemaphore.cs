using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Observable_Imp_Login_Exmaple
{
    /// <summary>
    /// The AdjustableSemaphore is a semaphore that can be dynamically change the available
    /// counter at run-time.
    /// </summary>
    public class AdjustableSemaphore
    {
        private object m_Monitor = new object();
        private int m_Count = 0;
        private int m_MaximumCount = 0;

        public AdjustableSemaphore(int maximumCount)
        {
            this.MaximumCount = maximumCount;
        }

        public int MaximumCount
        {
            get
            {
                lock (m_Monitor)
                    return m_MaximumCount;
            }
            set
            {
                lock (m_Monitor)
                {
                    if (value < 1)
                        throw new ArgumentOutOfRangeException("Must be greater than or equal to 1.", "MaximumCount");


                    m_Count += (value - m_MaximumCount); // m_Count can go negative. That's okay.
                    m_MaximumCount = value;
                    Monitor.PulseAll(m_Monitor);
                }
            }
        }

        public void BlockSemaphore()
        {
            lock (m_Monitor)
            {
                m_MaximumCount = 0;
                Monitor.PulseAll(m_Monitor);
            }
        }

        public void OpenSemaphore(int numOfThreads)
        {
            this.MaximumCount = numOfThreads;
        }

        public void OpenSemaphoreSlowly(int numOfThreads)
        {

            if (numOfThreads < 1)
                throw new ArgumentOutOfRangeException("Must be greater than or equal to 1.", "MaximumCount");

            for (int i = 1; i <= numOfThreads; i++)
            {
                lock (m_Monitor)
                {

                    m_Count += (numOfThreads - m_MaximumCount); // m_Count can go negative. That's okay.

                    m_MaximumCount = i;
                    Monitor.Pulse(m_Monitor);
                }
                Thread.Sleep(500);
            }
        }


        public void WaitOne()
        {
            lock (m_Monitor)
            {
                while (m_Count <= 0)
                    System.Threading.Monitor.Wait(m_Monitor);

                m_Count--;
            }
        }

        public void Release()
        {
            lock (m_Monitor)
            {
                if (m_Count < m_MaximumCount)
                {
                    m_Count++;
                    System.Threading.Monitor.Pulse(m_Monitor);
                }
                //else
                //throw new System.Threading.SemaphoreFullException("Semaphore released too many times.");
            }
        }

        public void GetSemaphoreInfo(out int totalResource, out int usedResource, out int avaliableResource)
        {
            lock (m_Monitor)
            {
                totalResource = m_MaximumCount;
                usedResource = m_MaximumCount - m_Count;
                avaliableResource = m_MaximumCount - usedResource;
            }
        }
    }
}
