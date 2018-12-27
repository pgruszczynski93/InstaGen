using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InstaGen
{
    public class DisplayKeyboardInfo
    {
        public int keyboardHeight;
        public float keyboardRatio;
    }

    public class AndroidNativeHelper : MonoBehaviour {

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
    }

}
