using UniRx;
using UnityEngine;

public class AndroidVideoPlayer : MonoBehaviour ,IVideoPlayer {

    private float mGap = 0;// 8f / 30f;
    private System.Diagnostics.Stopwatch mStopWatch;

    private AndroidJavaObject mTrealPlayerListener;
    private AndroidJavaObject mMediaPosition;

    public static AndroidVideoPlayer Create(KidsPlayer player)
    {
        var obj = FindObjectOfType<AndroidVideoPlayer>();

        if (obj == null)
        {
            var go = new GameObject("AndroidVideoPlayer");
            obj = go.AddComponent<AndroidVideoPlayer>();
        }

        return obj;
    }

    public void Awake()
    {
        Init();
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("Unity->OnPuase: " + pause);
    }

    private void OnDestroy()
    {
        using (AndroidJavaClass ctr = new AndroidJavaClass("com.sktelecom.trealplayer"))
        {
            ctr.SetStatic("PlayerReady", false);
        }
    }

    public void Init()
    {
        using (AndroidJavaClass clss = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj = clss.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                mTrealPlayerListener = obj.Get<AndroidJavaObject>("mTrealPlayerListener");
                mMediaPosition = obj.Get<AndroidJavaObject>("mMediaPosition");
            }
        }

        mStopWatch = new System.Diagnostics.Stopwatch();
    }


    public void OnReady()
    {
        using (AndroidJavaClass ctr = new AndroidJavaClass("com.sktelecom.trealplayer.TrealPlayerController"))
        {
            ctr.SetStatic("PlayerReady", true);
        }

        Debug.Log("AndroidVideoPlayer: OnReady");
        CallListener("onReady");
    }

    public void OnLoad(int result)
    {
        CallListener("onLoad", result);
    }

    public void OnPlay(int result)
    {
        CallListener("onPlay", result);
    }

    public void OnPause(int result)
    {
        CallListener("onPause", result);
    }

    public void OnResume(int result)
    {
        CallListener("onResume", result);
    }

    public void OnStop(int result)
    {
        CallListener("onStop", result);
    }

    public void OnPlayComplete()
    {
        CallListener("onPlayComplete");
    }

    public void OnSeek(int result)
    {
        CallListener("onSeek", result);
    }

    public void OnGoFrame(int result)
    {
        CallListener("onGoFrame", result);
    }

    public void OnUnload(int result)
    {
        CallListener("onUnload", result);
    }

    private void CallListener(string method)
    {
        //Debug.Log("TrealPlugin: Call " + method + "(" + Thread.CurrentThread.ManagedThreadId + ")");
        Observable.Start(() =>
        {
            //Debug.Log("TrealPlugin: Call Rx " + method + "(" + Thread.CurrentThread.ManagedThreadId + ")");
            AndroidJNI.AttachCurrentThread();
            mTrealPlayerListener.Call(method);
            AndroidJNI.DetachCurrentThread();
        }).Subscribe();
    }

    private void CallListener(string method, int result)
    {
        //Debug.Log("TrealPlugin: Call " + method + "(" + Thread.CurrentThread.ManagedThreadId + ")");
        Observable.Start(() =>
        {
            //Debug.Log("TrealPlugin: Call Rx " + method + "(" + Thread.CurrentThread.ManagedThreadId + ")");
            AndroidJNI.AttachCurrentThread();
            mTrealPlayerListener.Call(method, new object[] { result });
            AndroidJNI.DetachCurrentThread();
        }).Subscribe();
    }

    public float GetCurrentPos()
    {
        if (mMediaPosition == null) return 0;

        var v = 0;
        try
        {
            mStopWatch.Reset();
            mStopWatch.Start();
            v = mMediaPosition.Call<int>("getCurrentPosition");
            mStopWatch.Stop();


            Debug.Log("AndroidVideoPlayer->PTS: " + v);


            //mGap = ((int)((mStopWatch.ElapsedMilliseconds / 2f / 1000f) % (1f / 29.97f)) + 1) * (1f / 29.97f);
            mGap = (int)(((mStopWatch.ElapsedMilliseconds / 2f / 1000f) % (1f / 29.97f) + 0.5f)) * (1f / 29.97f);

            if (v == 0)
                return 0;

            return (v / 45000f) + mGap;
        }
        catch {
            return 0;
        }
    }
}
