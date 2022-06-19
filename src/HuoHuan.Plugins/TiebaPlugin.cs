using HuoHuan.Glue;

namespace HuoHuan.Plugins
{
    public class TiebaPlugin : IPlugin
    {
        private Task _mainTask;

        #region [Properties]
        public string Name => "贴吧";

        public bool CanConfig => true;

        public SpiderStatus Status { get; set; } = SpiderStatus.Waiting;
        #endregion

        public event ProgressEventHandler ProgressStatusChanged;
        public event CrawledEventHandler Crawled;

        #region [Methods]
        public void Start()
        {
            if (this.Status != SpiderStatus.Running)
            {
                this.Status = SpiderStatus.Running;
                this._mainTask = Task.Run(() =>
                {

                });
            }
        }

        public void Pause()
        {
            if (this.Status == SpiderStatus.Running)
            {
                this.Status = SpiderStatus.Paused;
            }
        }

        public void Continue()
        {
            if (this.Status == SpiderStatus.Paused)
            {
                this.Status = SpiderStatus.Running;

            }
        }

        public void Stop()
        {
            this.Status = SpiderStatus.Waiting;
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            this._mainTask = null;
        }
        #endregion
    }
}
