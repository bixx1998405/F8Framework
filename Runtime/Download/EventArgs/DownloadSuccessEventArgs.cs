using System;

namespace F8Framework.Core
{
    public class DownloadSuccessEventArgs : DownloadEventArgsBase
    {
        public TimeSpan TimeSpan { get; private set; }

        public override void Clear()
        {
            base.Clear();
            TimeSpan = TimeSpan.Zero;
        }

        public static DownloadSuccessEventArgs Create(DownloadInfo info, int currentTaskIndex, int taskCount,
            TimeSpan timeSpan)
        {
            var eventArgs = ReferencePool.Acquire<DownloadSuccessEventArgs>();
            eventArgs.SetBase(info, currentTaskIndex, taskCount);
            eventArgs.TimeSpan = timeSpan;
            return eventArgs;
        }

        public static void Release(DownloadSuccessEventArgs eventArgs)
        {
            ReferencePool.Release(eventArgs);
        }
    }
}
