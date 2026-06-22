using System;

namespace F8Framework.Core
{
    public class DonwloadUpdateEventArgs : DownloadEventArgsBase
    {
        public long TotalDownloadedLength { get; private set; }
        public TimeSpan TimeSpan { get; private set; }

        public override void Clear()
        {
            base.Clear();
            TotalDownloadedLength = 0;
            TimeSpan = TimeSpan.Zero;
        }

        public static DonwloadUpdateEventArgs Create(DownloadInfo downloadInfo, int currentTaskIndex, int taskCount,
            long totalDownloadedLength, TimeSpan timeSpan)
        {
            var eventArgs = ReferencePool.Acquire<DonwloadUpdateEventArgs>();
            eventArgs.SetBase(downloadInfo, currentTaskIndex, taskCount);
            eventArgs.TotalDownloadedLength = totalDownloadedLength;
            eventArgs.TimeSpan = timeSpan;
            return eventArgs;
        }

        public static void Release(DonwloadUpdateEventArgs eventArgs)
        {
            ReferencePool.Release(eventArgs);
        }
    }
}
