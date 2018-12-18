#if !UNITY_EDITOR && ( UNITY_ANDROID || UNITY_IOS )
#define ENABLE_LOG
#endif

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Treal.Browser;
using Treal.Browser.Core;
using Treal.Browser.Util;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class Bugfender : MonoBehaviour , ITrealLogger
{
    public string APP_KEY;

#if UNITY_ANDROID    
    private static AndroidJavaClass bugfender;
    private static string TAG = "TrealBrowser";
#elif UNITY_IOS
    [DllImport("__Internal")]
    private static extern void BugfenderInit(string key);
    [DllImport("__Internal")]
    private static extern void BugfenderLog(string msg);
    [DllImport("__Internal")]
    private static extern void BugfenderWarn(string msg);
    [DllImport("__Internal")]
    private static extern void BugfenderErr(string msg);
#endif

    private void Awake()
    {
        //print("Init Logger");
        Init();
    }

    private void Start()
    {
        //var browser = GetComponent<TrealBrowser>();
        TrealBrowser.SetLogger(this);
    }

    [Conditional("ENABLE_LOG")]//, Conditional("BUGFENDER")]
    private void Init()
    {
#if UNITY_ANDROID    
        if (bugfender == null)
        {
            Debug.Log("*** INITIALIZING BUGFENDER ***");
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

                bugfender = new AndroidJavaClass("com.bugfender.sdk.Bugfender");
                if (bugfender != null)
                {
                    bugfender.CallStatic("init", activityContext, APP_KEY, false);
                    //bugfender.CallStatic("enableLogcatLogging");
                }
            }
        }
#elif UNITY_IOS && !UNITY_EDITOR
        BugfenderInit(APP_KEY);
#endif
    }

    public void Log(string s)
    {
#if UNITY_ANDROID    
        Log("d", TAG, s);
#elif UNITY_IOS && !UNITY_EDITOR
        BugfenderLog(s);
#endif
    }

    public void LogWarning(string s)
    {
#if UNITY_ANDROID    
        Log("w", TAG, s);
#elif UNITY_IOS && !UNITY_EDITOR 
        BugfenderWarn(s);
#endif
    }

    public void LogError(string s)
    {
#if UNITY_ANDROID    
        Log("e", TAG, s);
#elif UNITY_IOS && !UNITY_EDITOR
        BugfenderErr(s);    
#endif
    }

    [Conditional("ENABLE_LOG")]
    private void Log(string level, string tag, string message)
    {
#if UNITY_ANDROID    
        if (bugfender == null) return;

        bugfender.CallStatic(level, new object[] { tag, message });
#elif UNITY_IOS    
#endif
    }

    [Conditional("ENABLE_LOG")]
    public void SendIssue(string title, string text)
    {
#if UNITY_ANDROID    
        if (bugfender == null) return;

        bugfender.CallStatic("sendIssue", new object[] { title, text });
#elif UNITY_IOS    
#endif
    }
}