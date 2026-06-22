namespace F8Framework.Core
{
    internal static class ArrayHelper
    {
        internal static T GetSafeElement<T>(T[] array, int index) where T : class
        {
            return array != null && index >= 0 && index < array.Length ? array[index] : null;
        }
    }
}
