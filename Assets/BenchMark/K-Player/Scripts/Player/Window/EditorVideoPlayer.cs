using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.Video;

public class EditorVideoPlayer : MonoBehaviour , IVideoPlayer {

    public KidsPlayer mContentPlayer;
    public VideoPlayer mVideoPlayer;

    private bool mInit = false;

    public static EditorVideoPlayer Create(KidsPlayer player)
    {
        
        var obj = player.gameObject.GetComponent<EditorVideoPlayer>();

        if (obj == null)
        {
            obj = player.gameObject.AddComponent<EditorVideoPlayer>();
        }

        obj.mContentPlayer = player;

        return obj;
    }

    void Start()
    {
        Init();
        OnReady();
    }

#if UNITY_EDITOR
    public void LoadVideo(string name)
    {

        var path = Path.GetFullPath(Path.Combine(Application.dataPath, "../../Video/" + name + ".mp4"));

        mVideoPlayer.url = path;

        mVideoPlayer.Prepare();
        mVideoPlayer.prepareCompleted += MVideoPlayer_prepareCompleted;
    }
#endif
    public void OnReady()
    {
        StartCoroutine(WaitInit());
    }


    public void OnLoad(int result)
    {
        //mContentPlayer.SendMessage("AndroidPlay");
    }

    public void OnPlay(int result)
    {
        mVideoPlayer.Play();
    }


    public void OnGoFrame(int result)
    {

    }


    public void OnPause(int result)
    {

    }


    public void OnPlayComplete()
    {

    }

    public void OnResume(int result)
    {

    }

    public void OnSeek(int result)
    {
    }

    public void OnStop(int result)
    {
    }

    public void OnUnload(int result)
    {
    }


    private IEnumerator WaitInit()
    {
        while (!mInit)
        {
            yield return null;
        }

        OnLoad(0);
    }

    private void Init()
    {
        if (mVideoPlayer == null)
        {
            mVideoPlayer = gameObject.AddComponent<VideoPlayer>();
            mVideoPlayer.playOnAwake = false;
            mVideoPlayer.targetCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            mVideoPlayer.renderMode = VideoRenderMode.CameraFarPlane;

            mVideoPlayer.source = VideoSource.Url;

            
        }
    }

    private void MVideoPlayer_prepareCompleted(VideoPlayer source)
    {
        mInit = true;
    }

    public float GetCurrentPos()
    {
        var time = (float)mVideoPlayer.time;

        //time = Mathf.Max(time - 1f / 29.97f, 0f);
        return time;
    }
}
