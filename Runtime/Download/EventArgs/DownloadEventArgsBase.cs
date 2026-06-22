namespace F8Framework.Core
{
    public abstract class DownloadEventArgsBase : IReference
    {
        public DownloadInfo DownloadInfo { get; protected set; }
        public int CurrentDownloadTaskIndex { get; protected set; }
        public int DownloadTaskCount { get; protected set; }

        public virtual void Clear()
        {
            DownloadInfo = default;
            CurrentDownloadTaskIndex = 0;
            DownloadTaskCount = 0;
        }

        protected void SetBase(DownloadInfo info, int currentTaskIndex, int taskCount)
        {
            DownloadInfo = info;
            CurrentDownloadTaskIndex = currentTaskIndex;
            DownloadTaskCount = taskCount;
        }
    }
}
