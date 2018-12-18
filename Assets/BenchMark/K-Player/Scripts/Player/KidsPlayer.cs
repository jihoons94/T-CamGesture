using System;
using System.Collections;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;

public class KidsPlayer : MonoBehaviour {

    public const string TYPE_ROLEPLAY = "001";
    public const string TYPE_DRAW = "010";
    public const string TYPE_VOICE = "100";

    [HideInInspector]
    public IVideoPlayer mPlayerCallback;

    public Transform[] ContentPlayers;
    
    public IContent mContent;

    private Boolean mInPlay = false;
    private float mLastSyncPos = 0;
    //public IUserContent mUserContent;

    private void Start()
    {
        // Set Video Controller for PTS
#if UNITY_EDITOR
        mPlayerCallback = EditorVideoPlayer.Create(this);
#elif UNITY_ANDROID
        mPlayerCallback = AndroidVideoPlayer.Create(this);
#endif

        Observable.IntervalFrame(30)
            .Where(_ => mInPlay == true)
            .Subscribe(xs =>
            {
                mLastSyncPos = mPlayerCallback.GetCurrentPos();
                SetTime(mLastSyncPos);
            }).AddTo(this);

        MessageBroker.Default.Receive<ContentLoadCommand>()
            .ObserveOnMainThread().Subscribe(x => 
            {
                var state = LoadContent(x.Content);
                mPlayerCallback.OnLoad(state);
            }).AddTo(this);

        MessageBroker.Default.Receive<ContentUnloadCommand>()
            .ObserveOnMainThread().Subscribe(x => 
            {
                UnloadContent();
                mPlayerCallback.OnUnload(0);
            }).AddTo(this);

        MessageBroker.Default.Receive<PlayerStartCommand>()
            .ObserveOnMainThread().Subscribe(x => 
            {
                var state = Play();
                mPlayerCallback.OnPlay(state);
            }).AddTo(this);

        MessageBroker.Default.Receive<PlayerPauseCommand>()
            .ObserveOnMainThread().Subscribe(x =>
            {
                Pause();
                mPlayerCallback.OnPause(0);
            }).AddTo(this);

        MessageBroker.Default.Receive<PlayerStopCommand>()
            .ObserveOnMainThread().Subscribe(x =>
            {
                Stop();
                mPlayerCallback.OnStop(0);
            }).AddTo(this);

        MessageBroker.Default.Receive<ContentShowCommand>()
            .ObserveOnMainThread().Subscribe(x => 
            {
                Show(x.Show);
            }).AddTo(this);

        mPlayerCallback.OnReady();
    }

    public int LoadContent(KContent content)
    {

        Debug.Log("KidsPlayer: LoadContent - " + content.path);

        mLastSyncPos = 0f;

        //0. Unload Current Content?
        if (mContent != null)
            mContent.UnloadContent();

        //1. Check File Path
        CheckContentPath(content.path);

        //2. Load Video ( only Editor )
#if UNITY_EDITOR
        var player = mPlayerCallback as EditorVideoPlayer;
        if (player != null)
        {
            var dirname = content.path.Split(new char[] { '\\', '/' }).Last();
            player.LoadVideo(dirname);
        }
#endif

        //3. Set mContent 
        //  Error: {type}100 

        Transform contentObject;

        switch (content.type)
        {
            case TYPE_ROLEPLAY: //Roleplay
                contentObject = Instantiate(ContentPlayers[0], null);
                mContent = contentObject.GetComponent<RolePlayContent>();
                break;

            case TYPE_DRAW: //Draw
                contentObject = Instantiate(ContentPlayers[1], null);
                mContent = contentObject.GetComponent<DrawContent>();
                break;

            default:
                SendError(KidsError.UndefindeContentType);
                //mPlayerCallback.OnLoad((int)KidsError.UndefindeContentType);
                return (int)KidsError.UndefindeContentType;
        }

        var loadState = mContent.LoadContent(content);

        if ( loadState != KidsError.None)
        {
            //load fail
            mContent = null;
            DestroyImmediate(contentObject.gameObject, true);

            return (int)loadState;
        }

        mContent.Hide();
        return 0;
    }

    public void UnloadContent()
    {
        Stop();
        //TODO: Unload Current Content

        //1. Clear Scene
        // Error 1900;


        //2. Unload User Content
        // Error {type}900

        //3. Unload Content
        // Error {type}950
        if (mContent != null)
        {
            mContent.UnloadContent();
            //DestroyImmediate( true);
            mContent = null;
        }
        Resources.UnloadUnusedAssets();
    }

    public void SetTime(float time)
    {
        //1. SetContent Time;
        // Error {type}200;
        if(mContent != null)
            mContent.SetTime(time);
    }

    public int Play()
    {
        if (mContent == null)
            return (int)KidsError.PlayEmptyContent;
        //1. Start Get PTS Info
        mInPlay = true;

        //2. Wait First PTS?
        StartCoroutine(CoWaitFirstPTS());

        return 0;
    }

    public void Stop()
    {
        Pause();
        if (mContent != null)
            mContent.Hide();
        SetTime(0);
    }

    public void Pause()
    {
        // 1. Stop Get PTS Info
        mInPlay = false;

        // 2. Content Pause
        if (mContent != null)
            mContent.Pause();
        //mUserContent.Pause();

        mLastSyncPos = mPlayerCallback.GetCurrentPos();
    }

    public void Show(bool show)
    {
        if (show)
        {
            mContent.Show();
        }
        else
        {
            mContent.Hide();
        }
    }


    #region private
    private IEnumerator CoWaitFirstPTS()
    {
        while (mInPlay)
        {
            if (mPlayerCallback.GetCurrentPos() > mLastSyncPos)
            {
                mContent.SetTime(mPlayerCallback.GetCurrentPos());
                mContent.Play();
                mContent.Show();
                yield break;
            }
            yield return null;
        }
    }

    private KContentInfo GetContentInfo(string path)
    {
        try
        {
            var infoPath = Path.Combine(path, "info.json");
            if (File.Exists(infoPath))
                SendError(KidsError.LoadInfoNotFound);

            var jsonString = File.ReadAllText(infoPath);
            if (string.IsNullOrEmpty(jsonString.Trim()))
                SendError(KidsError.LoadInfoJsonEmpty);

            return JsonUtility.FromJson<KContentInfo>(jsonString);
        }
        catch (Exception)
        {
            SendError(KidsError.LoadGetInfoError);
        }

        return null;
    }

    private void CheckContentPath(string path)
    {
        if (Directory.Exists(path))
            SendError(KidsError.LoadPathNotFound);

        var files = Directory.GetFiles(path);

        if (files.Length == 0)
            SendError(KidsError.LoadPathIsEmpty);
    }

    private void SendError(KidsError error)
    {
        
    }

    #endregion


    private void OnDestroy()
    {
        UnloadContent();
    }

    private void OnApplicationQuit()
    {
        UnloadContent();
    }
}
