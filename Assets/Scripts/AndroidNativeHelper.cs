using UnityEngine;

namespace InstaGen
{
    public class DisplayKeyboardInfo
    {
        public int keyboardHeight;
        public float keyboardRatio;
    }

    public class AndroidNativeHelper {

        public static DisplayKeyboardInfo GetKeyboardDisplayInfo()
        {
#if UNITY_ANDROID && !UNITY_EDITOR  
            using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
                using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect")) {
                    View.Call("getWindowVisibleDisplayFrame", rect);

                    int screenHeight = Screen.height;
                    int leftSpaceHeight = rect.Call<int>("height");
                    DisplayKeyboardInfo keyboardInfo = new DisplayKeyboardInfo
                    {
                        keyboardHeight = screenHeight - leftSpaceHeight,
                        keyboardRatio = (float) (screenHeight - leftSpaceHeight) /screenHeight
                    };
                    return keyboardInfo;
                }
            }
#else
        return new DisplayKeyboardInfo
        {
            // 500 is debug value!!!
            keyboardHeight = 500,
            keyboardRatio = 500
        };
#endif
        }

        public static float GetFreeSpace()
        {
            float freeMegabytes = -1.0f;
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment"))
                {
                    using (AndroidJavaObject file = environment.CallStatic<AndroidJavaObject>("getDataDirectory"))
                    {
                        string path = file.Call<string>("getAbsolutePath");

                        using (AndroidJavaObject statistics = new AndroidJavaObject("android.os.StatFs", path))
                        {
                            long blocks = statistics.Call<long>("getAvailableBlocksLong");
                            long blockSize = statistics.Call<long>("getBlockSizeLong");

                            freeMegabytes = (blocks * blockSize) / (1024.0f * 1024.0f);
                        }
                    }
                }
            }
            catch (System.Exception exception)
            {
                Debug.Log(string.Format("Error occurs when trying to get free internal memory: {0}",exception.Message));
            }
#endif
            return freeMegabytes;
        }

        public static void CopyTextToClipboard(string stringToPaste)
        {
            
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                
                using (AndroidJavaClass clipboardLibrary =
                    new AndroidJavaClass("com.mindwalkerstudio.clipboardsupportlibrary.ClipboardPaster"))
                using(AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {

                    using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    using (AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext"))
                    {
                        clipboardLibrary.CallStatic("pasteTextToClipboard",context, stringToPaste);
                    }
                }                
            }
            catch (System.Exception exception)
            {
                Debug.Log(string.Format("Error occurs when trying to paste to clipboard",exception.Message));
            }
#endif
        }
    }

}
