using GiftShop_DS.Structure;
using System;
using System.Timers;

namespace GiftShop_DS.Utils
{
    internal class ExpirationJobRunner : JobRunner
    {

        public event EventHandler<JobDoneArgs> OnJobDone;
        private StoreQueue _queue;                
        public double? ExpiretionThreshHoldInMilliseconds { get; set; }
        

        public ExpirationJobRunner(StoreQueue queue):base()
        {
            _queue = queue;            
        }                
                
        protected override void DoJob()
        {
            var head = _queue.Peek();
            if(head != null)
            {
                var diff = DateTime.Now.Subtract(head.Data.Data.InsertionDate).TotalMilliseconds;
                if (diff > ExpiretionThreshHoldInMilliseconds)
                {
                    JobDoneArgs args = new JobDoneArgs(_queue.Dequeue());
                    OnJobDone?.Invoke(this, args);
                }
            }
        }
    }

    public class JobDoneArgs :EventArgs
    {
        internal DataQueue DataQueue { get; }
        internal JobDoneArgs(DataQueue dq)
        {
            DataQueue = dq;
        }
    }
}
