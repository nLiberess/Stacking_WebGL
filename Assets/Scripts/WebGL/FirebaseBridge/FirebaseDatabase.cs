using System.Runtime.InteropServices;

namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        [DllImport("__Internal")]
        public static extern void InitGame(string userKey, string secretKey, string language, string maxCount, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void NotifyMessage(string messageType, string objectName, string callback, string fallback);
        
        [DllImport("__Internal")]
        public static extern void PostToLauncher(string userKey, string promotionKey, string gameScore, string objectName, string callback, string fallback);
        
        [DllImport("__Internal")]
        public static extern void PostJSON(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void GetJSON(string path, string objectName, string callback, string fallback);
        
        [DllImport("__Internal")]
        public static extern void PostSET(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void ForRanking(string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void GetDate(string path, string objectName, string callback, string fallback);
        [DllImport("__Internal")]
        public static extern void PostDate(string path, string value, string objectName, string callback, string fallback);
        [DllImport("__Internal")]
        public static extern void PostRANK(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void GetVALUE(string path, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void PostDayCount(string path, string pathValue ,string value, string objectName, string callback, string fallback);
    }
}