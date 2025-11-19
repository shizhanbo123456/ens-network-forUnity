using System;

namespace Utils
{
    public static class Debug
    {
        public static void Log(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#elif !UNITY_2017_1_OR_NEWER
            Console.WriteLine("Log:"+msg);
#endif
        }
        public static void LogWarning(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(msg);
#elif !UNITY_2017_1_OR_NEWER
            Console.WriteLine("Log:" + msg);
#endif
        }
        public static void LogError(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(msg);
#elif !UNITY_2017_1_OR_NEWER
            Console.WriteLine("Log:" + msg);
#endif
        }
    }
}