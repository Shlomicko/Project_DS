using System.Timers;

namespace GiftShop_DS.Utils
{
    abstract class JobRunner
    {

        private readonly Timer _timer;

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

        protected abstract void DoJob();

    }
}
