namespace F8Framework.Core
{
    internal static class NetworkLogHelper
    {
        internal static void SetupTelepathyLogging()
        {
            Telepathy.Log.Info = (s) => LogF8.LogNet(s);
            Telepathy.Log.Warning = (s) => LogF8.LogWarning(s);
            Telepathy.Log.Error = (s) => LogF8.LogError(s);
        }
    }
}
