using System;

namespace F8Framework.Core
{
    public class DownloadFailureEventArgs : DownloadEventArgsBase
    {
        public string ErrorMessage { get; private set; }
        public TimeSpan TimeSpan { get; private set; }

        public override void Clear()
        {
            base.Clear();
            ErrorMessage = null;
            TimeSpan = TimeSpan.Zero;
        }

        public static DownloadFailureEventArgs Create(DownloadInfo info, int currentTaskIndex, int taskCount,
            string errorMessage, TimeSpan timeSpan)
        {
            var eventArgs = ReferencePool.Acquire<DownloadFailureEventArgs>();
            eventArgs.SetBase(info, currentTaskIndex, taskCount);
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.TimeSpan = timeSpan;
            return eventArgs;
        }

        public static void Release(DownloadFailureEventArgs eventArgs)
        {
            ReferencePool.Release(eventArgs);
        }
    }
}
