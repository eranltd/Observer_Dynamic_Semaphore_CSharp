using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
                        throw new ArgumentException("Must be greater than or equal to 1.", "MaximumCount");


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

