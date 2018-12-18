using UnityEngine;
using UniRx;
using System.Threading;
using System;
using System.Collections;

public class TrealPlugin : MonoBehaviour
{
    public const string VERSION = "1.0.1";

    public Console mConsole;
    public KidsPlayer mPlayer;

    void Awake()
    {
        Debug.Log("TrealPlugin(" + VERSION + "): Initialized..");
    }

    #region From Android

    public void AndroidLoad(string json)
    {
        Debug.Log("TrealPlugin: Load: " + json);

        var content = JsonUtility.FromJson<KContent>(json);

        MessageBroker.Default.Publish(new ContentLoadCommand {
            Content = content
        });
    }

    public void AndroidPlay()
    {
        MessageBroker.Default.Publish(new PlayerStartCommand());
    }

    public void AndroidPause()
    {
        MessageBroker.Default.Publish(new PlayerPauseCommand());
    }

    public void AndroidResume()
    {
        MessageBroker.Default.Publish(new PlayerStartCommand());
    }

    public void AndroidStop()
    {
        MessageBroker.Default.Publish(new PlayerStopCommand());
    }

    public void AndroidUnload()
    {
        MessageBroker.Default.Publish(new ContentUnloadCommand());
    }

    public void AndroidSetConsole(bool on)
    {
        mConsole.isVisible = on;
    }

    public void AndroidSetConsole(string on)
    {
        AndroidSetConsole(string.Equals(on, "true", System.StringComparison.OrdinalIgnoreCase));
    }

    public void AndroidShow()
    {
        MessageBroker.Default.Publish(new ContentShowCommand { Show = true });
    }

    public void AndroidHide()
    {
        MessageBroker.Default.Publish(new ContentShowCommand { Show = false });
    }
    #endregion

}

