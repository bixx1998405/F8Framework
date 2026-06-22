namespace F8Framework.Core
{
    public class DownloadStartEventArgs : DownloadEventArgsBase
    {
        public static DownloadStartEventArgs Create(DownloadInfo info, int currentTaskIndex, int taskCount)
        {
            var eventArgs = ReferencePool.Acquire<DownloadStartEventArgs>();
            eventArgs.SetBase(info, currentTaskIndex, taskCount);
            return eventArgs;
        }

        public static void Release(DownloadStartEventArgs eventArgs)
        {
            ReferencePool.Release(eventArgs);
        }
    }
}
