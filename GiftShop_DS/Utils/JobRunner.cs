using GiftShop_DS.Structure;
using System;
using System.Timers;

namespace GiftShop_DS.Utils
{
    internal class JobRunner<T> where T : IComparable<T>
    {

        private T _object;        
        private readonly StoreQueue<T> _queue;
        private readonly Timer _timer;

        public JobRunner(StoreQueue<T> queue)
        {
            _queue = queue;
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += CheckQueue;
        }

        public void Begin() => _timer.Start();
        public void Stop() => _timer.Stop();
        private void CheckQueue(object sender, ElapsedEventArgs e)
        {
            if(_object == null)
            {
                throw new NullReferenceException();
            }
            _object = _queue.Peek().Data;
        }        
        
        public void IfTrue(Predicate<T> predicate, Action action)
        {
            if (_object == null)
            {
                throw new NullReferenceException();
            }
            if (predicate.Invoke(_object))
            {
                action.Invoke();
            }
        }

    }
}
