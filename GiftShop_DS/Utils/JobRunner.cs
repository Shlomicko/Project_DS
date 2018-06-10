using System.Timers;

namespace GiftShop_DS.Utils
{
    abstract class JobRunner
    {

        private readonly Timer _timer;
        private double _interval = 10000;

        public JobRunner()
        {
            _timer = new Timer
            {
                AutoReset = true
            };
            _timer.Elapsed += OnTimeToDoJob;
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private void OnTimeToDoJob(object sender, ElapsedEventArgs e)
        {
            DoJob();
        }
        
        protected double Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
                _timer.Interval = _interval;
            }
            
        }
        protected abstract void DoJob();

    }
}
